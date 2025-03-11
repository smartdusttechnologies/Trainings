import pyodbc
import pandas as pd
import joblib
import random
import json
import itertools
from sklearn.model_selection import train_test_split
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.preprocessing import LabelEncoder
from sklearn.ensemble import GradientBoostingClassifier
from sklearn.metrics import accuracy_score

# ======== 1. READ DATABASE SETTINGS FROM appsettings.json ========
with open('appsettings.json', 'r') as config_file:
    config = json.load(config_file)

db_settings = config["DatabaseSettings"]

DB_CONNECTION = f'DRIVER={{{db_settings["Driver"]}}};SERVER={db_settings["Server"]};DATABASE={db_settings["Database"]};UID={db_settings["User"]};PWD={db_settings["Password"]};'

conn = pyodbc.connect(DB_CONNECTION)
cursor = conn.cursor()

# ======== 2. GET TABLE SCHEMA ========
def get_table_schema():
    cursor.execute("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE';")
    tables = [row[0] for row in cursor.fetchall()]
    schema_info = {}

    for table in tables:
        cursor.execute(f"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='{table}';")
        columns = [col[0] for col in cursor.fetchall()]
        schema_info[table] = columns

    return schema_info

schema_info = get_table_schema()

# ======== 3. GENERATE NLP-SQL DATASET ========
natural_queries = []
sql_queries = []

query_templates = [
    ("Get all records from {table}", "SELECT * FROM {table};"),
    ("Show all {columns} from {table}", "SELECT {columns} FROM {table};"),
    ("List all {columns} in {table} where {column} is {value}", "SELECT {columns} FROM {table} WHERE {column} = '{value}';"),
    ("Find {columns} in {table} with {column} greater than {value}", "SELECT {columns} FROM {table} WHERE {column} > {value};"),
    ("Get {columns} from {table} where {column} is in ({value1}, {value2})", "SELECT {columns} FROM {table} WHERE {column} IN ('{value1}', '{value2}');"),
    ("Find all records in {table} where {column} is between {value1} and {value2}", "SELECT * FROM {table} WHERE {column} BETWEEN '{value1}' AND '{value2}';"),
    ("Join {table1} and {table2} on {table1_column} = {table2_column}", "SELECT * FROM {table1} JOIN {table2} ON {table1}.{table1_column} = {table2}.{table2_column};"),
    ("Count number of records in {table}", "SELECT COUNT(*) FROM {table};"),
    ("Find distinct {columns} from {table}", "SELECT DISTINCT {columns} FROM {table};"),
    ("Get maximum {column} from {table}", "SELECT MAX({column}) FROM {table};"),
    ("Get minimum {column} from {table}", "SELECT MIN({column}) FROM {table};"),
    ("Calculate average {column} in {table}", "SELECT AVG({column}) FROM {table};"),
]

for table, columns in schema_info.items():
    for column in columns:
        for template in query_templates:
            natural_query = template[0].format(
                table=table, columns=", ".join(columns), column=column, value=random.randint(1, 100),
                value1=random.randint(1, 50), value2=random.randint(51, 100),
                table1=table, table2=random.choice(list(schema_info.keys())),
                table1_column=column, table2_column=random.choice(columns)
            )
            sql_query = template[1].format(
                table=table, columns=", ".join(columns), column=column, value=random.randint(1, 100),
                value1=random.randint(1, 50), value2=random.randint(51, 100),
                table1=table, table2=random.choice(list(schema_info.keys())),
                table1_column=column, table2_column=random.choice(columns)
            )
            natural_queries.append(natural_query)
            sql_queries.append(sql_query)

df_dataset = pd.DataFrame({
    "Natural Language Query": natural_queries,
    "SQL Query": sql_queries
})
df_dataset.to_csv('wwwroot/dataset/generated_nlp_sql_dataset.csv', index=False)

# ======== 4. TRAIN ML MODEL ========
X = df_dataset["Natural Language Query"]
y = df_dataset["SQL Query"]

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Vectorization
vectorizer = TfidfVectorizer(max_features=8000, stop_words="english", ngram_range=(1, 2))
X_train_vec = vectorizer.fit_transform(X_train)
X_test_vec = vectorizer.transform(X_test)

# Label Encoding
label_encoder = LabelEncoder()
y_all = pd.concat([y_train, y_test])
label_encoder.fit(y_all)
y_train_enc = label_encoder.transform(y_train)
y_test_enc = label_encoder.transform(y_test)

# Train Model
model = GradientBoostingClassifier(n_estimators=100, learning_rate=0.1, max_depth=7, random_state=42)
model.fit(X_train_vec, y_train_enc)

# Accuracy
y_pred = model.predict(X_test_vec)
accuracy = accuracy_score(y_test_enc, y_pred)
print(f"Model Accuracy: {accuracy * 100:.2f}%")


# Save Model
joblib.dump(model, "wwwroot/python_model/sql_model.pkl")
joblib.dump(vectorizer, "wwwroot/python_model/vectorizer.pkl")
joblib.dump(label_encoder, "wwwroot/python_model/label_encoder.pkl")

print("Model Training Completed & Files Saved Successfully!")
