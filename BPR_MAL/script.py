import pandas as pd

# Load the uploaded file
file_path = 'Data/Product_data_with_new_columns.csv'
data = pd.read_csv(file_path)

# Add a new column 'Shelf_Position'
# 0 = Bottom left (Good), 1 = Middle (Better), 2 = Top right (Best)
def determine_shelf_position(row):
    if row['Price'] >= data['Price'].quantile(0.67) and row['Brand'] == 1 and row['ShelfLife'] >= data['ShelfLife'].quantile(0.67):
        return 2  # Best
    elif row['Price'] >= data['Price'].quantile(0.33) or row['Brand'] == 1 or row['ShelfLife'] >= data['ShelfLife'].quantile(0.33):
        return 1  # Better
    else:
        return 0  # Good

# Apply the function to determine shelf position
data['Shelf_Position'] = data.apply(determine_shelf_position, axis=1)

# Save the modified data to a new file
output_file_path = '/mnt/data/Product_data_with_shelf_positions.csv'
data.to_csv(output_file_path, index=False)

output_file_path
