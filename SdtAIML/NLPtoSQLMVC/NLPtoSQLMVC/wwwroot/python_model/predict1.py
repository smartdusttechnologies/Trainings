import sys
import joblib
import pyodbc
import json
import pandas as pd
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.ensemble import RandomForestClassifier
from sklearn.preprocessing import LabelEncoder
import logging
import os
from datetime import datetime

# 🔹 Configure Logging
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s",
    stream=sys.stderr,
)

# 🔹 Paths to Model & Data Files
BASE_DIR = os.path.dirname(os.path.abspath(__file__))  
CONFIG_PATH = os.path.join(BASE_DIR, "..", "..", "appsettings.json")  

MODEL_PATH = "wwwroot/python_model/sql_model.pkl"
VECTORIZER_PATH = "wwwroot/python_model/vectorizer.pkl"
LABEL_ENCODER_PATH = "wwwroot/python_model/label_encoder.pkl"
FEEDBACK_DATA_PATH = "wwwroot/dataset/user_feedback.csv"
OLD_DATASET_PATH = "wwwroot/dataset/generated_nlp_sql_dataset.csv"

# 🔹 Load Model, Vectorizer, and Label Encoder
try:
    model = joblib.load(MODEL_PATH)
    vectorizer = joblib.load(VECTORIZER_PATH)
    label_encoder = joblib.load(LABEL_ENCODER_PATH)
except FileNotFoundError as e:
    logging.error(f"Model files not found: {e}")
    print(json.dumps({"error": "Model files not found. Train the model first!"}), file=sys.stderr)
    sys.exit(1)

# 🔹 Load Database Configuration from appsettings.json
def load_db_config():
    try:
        if not os.path.exists(CONFIG_PATH):
            raise FileNotFoundError(f"Config file not found: {CONFIG_PATH}")

        with open(CONFIG_PATH, "r", encoding="utf-8") as file:
            config = json.load(file)
            db_config = config.get("DatabaseSettings")

            if not db_config:
                raise ValueError("DatabaseSettings section is missing in appsettings.json")

            return db_config
    except Exception as e:
        logging.error(f"Error loading database config: {e}")
        return None

# 🔹 Setup Database Connection
db_settings = load_db_config()

if db_settings:
    DB_CONNECTION = (
        f"DRIVER={{{db_settings['Driver']}}};"
        f"SERVER={db_settings['Server']};"
        f"DATABASE={db_settings['Database']};"
        f"UID={db_settings['User']};"
        f"PWD={db_settings['Password']};"
    )
else:
    print(json.dumps({"error": "Database settings could not be loaded."}))
    exit(1)

# 🔹 Connect to Database
try:
    conn = pyodbc.connect(DB_CONNECTION, timeout=5)
    cursor = conn.cursor()
except Exception as e:
    logging.error(f"Failed to connect to the database: {e}")
    print(json.dumps({"error": f"Database connection error: {str(e)}"}), file=sys.stderr)
    sys.exit(1)

# 🔹 Generate SQL Query from User Input
def generate_sql(user_input):
    try:
        input_vector = vectorizer.transform([user_input])
        predicted_label = model.predict(input_vector)[0]
        sql_query = label_encoder.inverse_transform([predicted_label])[0]
        return sql_query
    except Exception as e:
        logging.error(f"Error generating SQL query: {e}")
        return None

# 🔹 Execute SQL Query and Fetch Data
def execute_query(sql_query):
    try:
        cursor.execute(sql_query)
        rows = cursor.fetchall()

        if not rows:
            return {"message": "No data found"}

        columns = [desc[0] for desc in cursor.description] if cursor.description else []
        result_list = []

        for row in rows:
            row_data = []
            for value in row:
                if isinstance(value, datetime):  
                    row_data.append(value.isoformat())  
                else:
                    row_data.append(str(value))  
            result_list.append(row_data)

        return {"columns": columns, "data": result_list}
    except Exception as e:
        logging.error(f"Error executing SQL query: {e}")
        return {"error": str(e)}

# 🔹 Save User Feedback for Retraining
def save_feedback(user_query, correct_sql):
    try:
        if not user_query or not correct_sql:
            logging.error("Invalid feedback data: user_query or correct_sql is empty.")
            return False

        feedback_entry = f'"{user_query}","{correct_sql}"\n'
        with open(FEEDBACK_DATA_PATH, "a", encoding="utf-8") as f:
            f.write(feedback_entry)

        return True
    except Exception as e:
        logging.error(f"Error saving feedback: {e}")
        return False

# 🔹 Retrain the Model
def retrain_model():
    try:
        old_data_chunks = pd.read_csv(OLD_DATASET_PATH, chunksize=5000)
        feedback_data = pd.read_csv(FEEDBACK_DATA_PATH, names=["Natural Language Query", "SQL Query"], header=None)

        if feedback_data.empty:
            logging.info("No new feedback data available for retraining.")
            return

        combined_data = pd.concat([chunk for chunk in old_data_chunks] + [feedback_data], ignore_index=True)
        X_train = combined_data["Natural Language Query"]
        y_train = combined_data["SQL Query"]

        new_vectorizer = TfidfVectorizer(max_features=5000, min_df=2, max_df=0.8)
        X_train_vec = new_vectorizer.fit_transform(X_train)

        new_label_encoder = LabelEncoder()
        y_train_enc = new_label_encoder.fit_transform(y_train)

        new_model = RandomForestClassifier(n_estimators=50, max_depth=10, random_state=42)
        new_model.fit(X_train_vec, y_train_enc)

        joblib.dump(new_model, MODEL_PATH)
        joblib.dump(new_vectorizer, VECTORIZER_PATH)
        joblib.dump(new_label_encoder, LABEL_ENCODER_PATH)

        global model, vectorizer, label_encoder
        model = new_model
        vectorizer = new_vectorizer
        label_encoder = new_label_encoder

    except Exception as e:
        logging.error(f"Error retraining model: {e}")

# 🔹 Main Execution
if __name__ == "__main__":
    try:
        if len(sys.argv) == 2:
            user_input = sys.argv[1]
            sql_query = generate_sql(user_input)

            if not sql_query:
                print(json.dumps({"error": "Failed to generate SQL query."}), file=sys.stderr)
                sys.exit(1)

            result = execute_query(sql_query)
            print(json.dumps({"sql_query": sql_query, "result": result}))

        elif len(sys.argv) == 3:
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
