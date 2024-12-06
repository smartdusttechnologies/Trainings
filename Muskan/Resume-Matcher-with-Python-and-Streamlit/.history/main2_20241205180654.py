import streamlit as st
import pandas as pd
import joblib
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import accuracy_score, classification_report
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.pipeline import Pipeline

# Load vectorizer and encoder for job titles
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label_encoder = joblib.load('label_encoder.pkl')

# App title
st.title("Resume Ranking and Model Training")

# Upload labeled dataset for training
training_file = st.file_uploader("Upload Labeled Dataset (CSV)", type="csv")

if training_file:
    # Read dataset
    df = pd.read_csv(training_file)
    st.dataframe(df.head())

    # Preprocessing: Encode labels if needed
    if "Score" not in df.columns:
        st.error("Dataset must include a 'Score' column (e.g., labels like 1 for best).")
    else:
        X = df["Resume"]
        y = df["Score"]

        # Train/Test Split
        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

        # Pipeline: Vectorizer + Model
        pipeline = Pipeline([
            ("vectorizer", TfidfVectorizer(max_features=5000)),
            ("classifier", RandomForestClassifier(n_estimators=100, random_state=42))
        ])

        # Train the model
        pipeline.fit(X_train, y_train)
        st.success("Model trained successfully!")

        # Evaluate on test set
        y_pred = pipeline.predict(X_test)
        acc = accuracy_score(y_test, y_pred)
        st.write(f"Accuracy: {acc:.2f}")
        st.text("Classification Report:")
        st.text(classification_report(y_test, y_pred))

        # Save the trained model
        joblib.dump(pipeline, "resume_ranking_model.pkl")
        st.success("Model saved as 'resume_ranking_model.pkl'!")

# Prediction: Upload Resumes
uploaded_resumes = st.file_uploader("Upload Resumes for Ranking", type=["pdf"], accept_multiple_files=True)

if uploaded_resumes:
    resume_texts = []
    for pdf in uploaded_resumes:
        text = pdf_to_text(pdf)  # Extract text from PDFs
        resume_texts.append(text)

    # Load trained model
    trained_model = joblib.load("resume_ranking_model.pkl")
    
    # Predict scores for uploaded resumes
    predictions = trained_model.predict(resume_texts)
    scores = trained_model.predict_proba(resume_texts)[:, 1]  # Probability for the "best" class

    # Create a DataFrame for display
    result_df = pd.DataFrame({
        "Resume": [file.name for file in uploaded_resumes],
        "Score": scores,
    }).sort_values(by="Score", ascending=False)

    st.subheader("Ranked Resumes")
    st.dataframe(result_df)
