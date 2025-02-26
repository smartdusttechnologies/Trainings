
import sys
import joblib
import pyodbc
import json
import pandas as pd
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.ensemble import RandomForestClassifier
from sklearn.preprocessing import LabelEncoder
import logging
from datetime import datetime

# Configure logging to write to stderr
logging.basicConfig(level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s", stream=sys.stderr)

# Paths to model and data files
MODEL_PATH = "wwwroot/python_model/sql_model.pkl"
VECTORIZER_PATH = "wwwroot/python_model/vectorizer.pkl"
LABEL_ENCODER_PATH = "wwwroot/python_model/label_encoder.pkl"
FEEDBACK_DATA_PATH = "wwwroot/dataset/user_feedback.csv"
OLD_DATASET_PATH = "wwwroot/dataset/generated_nlp_sql_dataset.csv"

# Load model, vectorizer, and label encoder
try:
    model = joblib.load(MODEL_PATH)
    vectorizer = joblib.load(VECTORIZER_PATH)
    label_encoder = joblib.load(LABEL_ENCODER_PATH)
   
except FileNotFoundError as e:
    logging.error(f"Model files not found: {e}")
    print(json.dumps({"error": "Model files not found. Train the model first!"}), file=sys.stderr)
    sys.exit(1)

# Database connection
DB_CONNECTION = 'DRIVER={ODBC Driver 17 for SQL Server};SERVER=147.79.67.111;DATABASE=TestingAndCalibration;UID=sa;PWD=Prem@9123188;'
try:
    conn = pyodbc.connect(DB_CONNECTION)
    cursor = conn.cursor()
    
except Exception as e:
    logging.error(f"Failed to connect to the database: {e}")
    print(json.dumps({"error": f"Database connection error: {str(e)}"}), file=sys.stderr)
    sys.exit(1)

def generate_sql(user_input):
    """Generate SQL query from user input."""
    try:
        input_vector = vectorizer.transform([user_input])
        predicted_label = model.predict(input_vector)[0]
        sql_query = label_encoder.inverse_transform([predicted_label])[0]
        
        return sql_query
    except Exception as e:
        logging.error(f"Error generating SQL query: {e}")
        return None

def execute_query(sql_query):
    """Execute SQL query and return results."""
    try:
        cursor.execute(sql_query)
        rows = cursor.fetchall()

        if not rows:
            return {"message": "No data found"}

        columns = [desc[0] for desc in cursor.description] if cursor.description else []
        result_list = []

        # Convert each row to a list of strings
        for row in rows:
            row_data = []
            for value in row:
                if isinstance(value, datetime):  # Convert datetime to string
                    row_data.append(value.isoformat())
                else:
                    row_data.append(str(value))  # Convert other types to string
            result_list.append(row_data)

        return {"columns": columns, "data": result_list}
    except Exception as e:
        logging.error(f"Error executing SQL query: {e}")
        return {"error": str(e)}

def save_feedback(user_query, correct_sql):
    """Save user feedback and retrain the model."""
    try:
        # Validate feedback data
        if not user_query or not correct_sql:
            logging.error("Invalid feedback data: user_query or correct_sql is empty.")
            return False

        # Save feedback to CSV
        feedback_entry = f'"{user_query}","{correct_sql}"\n'
        with open(FEEDBACK_DATA_PATH, "a", encoding="utf-8") as f:
            f.write(feedback_entry)
        
        return True
    except Exception as e:
        logging.error(f"Error saving feedback: {e}")
        return False

def retrain_model():
    """Retrain the model using the updated dataset."""
    try:
        # Load old dataset in chunks to avoid memory issues
        old_data_chunks = pd.read_csv(OLD_DATASET_PATH, chunksize=5000)
        feedback_data = pd.read_csv(FEEDBACK_DATA_PATH, names=["Natural Language Query", "SQL Query"], header=None)

        # Check if new feedback data is available
        if feedback_data.empty:
            logging.info("No new feedback data available for retraining.")
            return

        # Combine old data and feedback data
        combined_data = pd.concat([chunk for chunk in old_data_chunks] + [feedback_data], ignore_index=True)

        # Prepare features and labels
        X_train = combined_data["Natural Language Query"]
        y_train = combined_data["SQL Query"]

        # Reinitialize vectorizer and label encoder
        new_vectorizer = TfidfVectorizer(max_features=5000, min_df=2, max_df=0.8)
        X_train_vec = new_vectorizer.fit_transform(X_train)

        new_label_encoder = LabelEncoder()
        y_train_enc = new_label_encoder.fit_transform(y_train)

        # Retrain the model
        new_model = RandomForestClassifier(n_estimators=50, max_depth=10, random_state=42)
        new_model.fit(X_train_vec, y_train_enc)

        # Save the updated model, vectorizer, and label encoder
        joblib.dump(new_model, MODEL_PATH)
        joblib.dump(new_vectorizer, VECTORIZER_PATH)
        joblib.dump(new_label_encoder, LABEL_ENCODER_PATH)
        

        # Update global variables
        global model, vectorizer, label_encoder
        model = new_model
        vectorizer = new_vectorizer
        label_encoder = new_label_encoder
       
    except Exception as e:
        logging.error(f"Error retraining model: {e}")

if __name__ == "__main__":
    try:
        if len(sys.argv) == 2:
            # Generate and execute SQL query
            user_input = sys.argv[1]
            sql_query = generate_sql(user_input)

            if not sql_query:
                print(json.dumps({"error": "Failed to generate SQL query."}), file=sys.stderr)
                sys.exit(1)

            result = execute_query(sql_query)
            print(json.dumps({"sql_query": sql_query, "result": result}))

        elif len(sys.argv) == 3:
            # Save feedback and retrain the model
            user_query = sys.argv[1]
            correct_sql = sys.argv[2]

            if save_feedback(user_query, correct_sql):
                retrain_model()
                print(json.dumps({"message": "Feedback saved and model retrained!"}))
            else:
                print(json.dumps({"error": "Failed to save feedback."}), file=sys.stderr)
        else:
            print(json.dumps({"error": "Invalid number of arguments."}), file=sys.stderr)
    except Exception as e:
        logging.error(f"Unexpected error: {e}")
        print(json.dumps({"error": f"Unexpected error: {str(e)}"}), file=sys.stderr)