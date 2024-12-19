from flask import Flask, request, jsonify
import joblib
import pandas as pd
import os

app = Flask(__name__)

BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

MODEL_PATH = os.path.join(BASE_DIR, 'Model', 'xgb_model.pkl')
LABEL_ENCODER_PATH = os.path.join(BASE_DIR, 'Model', 'label_encoder.pkl')
FEATURE_NAMES_PATH = os.path.join(BASE_DIR, 'Model', 'feature_names.pkl')

model = joblib.load(MODEL_PATH)
label_encoder = joblib.load(LABEL_ENCODER_PATH)
feature_names = joblib.load(FEATURE_NAMES_PATH)

@app.route('/predict', methods=['POST'])
def predict():
    try:
        data = request.json

        input_data = pd.DataFrame([{
            'Product_Width': data['Product_Width'],
            'Product_Height': data['Product_Height'],
            'Product_Depth': data['Product_Depth'],
            'Store_Name': data['Store_Name'],
            'Department': data['Department']
        }])

        input_data = pd.get_dummies(input_data, columns=['Store_Name', 'Department'])
        input_data = input_data.reindex(columns=feature_names, fill_value=0)

        prediction = model.predict(input_data)
        predicted_category = label_encoder.inverse_transform(prediction)[0]

        if hasattr(model, "predict_proba"):
            probabilities = model.predict_proba(input_data)
            confidence_score = float(max(probabilities[0]))  # Convert float32 to float
        else:
            confidence_score = None  

        print(f"Prediction: {predicted_category}, Confidence: {confidence_score}")

        return jsonify({
            'prediction': predicted_category,
            'confidence': confidence_score
        })

    except Exception as e:
        print(f"Error: {e}")
        return jsonify({'error': str(e)}), 400


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
