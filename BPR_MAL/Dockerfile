# Use the official lightweight Python image
FROM python:3.10-slim

# Set the working directory inside the container
WORKDIR /app

# Copy the requirements file to the working directory
COPY requirements.txt .

# Install the Python dependencies
RUN pip install --no-cache-dir -r requirements.txt

# Copy the entire Model folder into the Docker image
COPY Model /Model

# Copy the entire Server folder into the Docker image
COPY Server /Server

# Set environment variables to point to the model location
ENV MODEL_PATH=/Model/xgb_model.pkl

# Expose the application port
EXPOSE 5000


# Command to run your server
CMD ["python", "/Server/server.py" , "--host=0.0.0.0", "Flask" ]
