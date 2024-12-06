import streamlit as st
import pandas as pd
from sklearn.metrics.pairwise import cosine_similarity
import nltk
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.stem import WordNetLemmatizer
import joblib

nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')

# Load models and vectorizer
svc = joblib.load('svc_model.pkl')
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label = joblib.load('label_encoder.pkl')

# Text Preprocessing
def preprocess_text(text):
    stop_words = set(stopwords.words('english'))
    tokens = word_tokenize(text.lower())
    lemmatizer = WordNetLemmatizer()
    return " ".join(
        lemmatizer.lemmatize(word)
        for word in tokens
        if word.isalnum() and word not in stop_words
    )

# Predict categories for resumes
def predict_category_for_resumes(resumes):
    preprocessed_resumes = [preprocess_text(resume) for resume in resumes]
    input_resumes = word_vectorizer.transform(preprocessed_resumes)
    predictions = svc.predict(input_resumes)
    predicted_categories = label.inverse_transform(predictions)
    return predicted_categories

# Calculate similarity scores
def calculate_similarity(role, resumes):
    role_vector = word_vectorizer.transform([role])
    preprocessed_texts = [preprocess_text(text) for text in resumes]
    tfidf_matrix = word_vectorizer.transform(preprocessed_texts)
    similarity_scores = cosine_similarity(role_vector, tfidf_matrix).flatten()
    return similarity_scores

# Streamlit UI
st.title("Resume Classifier and Similarity Finder")

# Step 1: Upload resumes for classification
st.header("Step 1: Upload Resumes")
uploaded_files = st.file_uploader("Upload resumes (PDFs)", type=["pdf"], accept_multiple_files=True)

if uploaded_files:
    # Process uploaded resumes
    resumes = [file.read().decode("utf-8") for file in uploaded_files]
    predictions = predict_category_for_resumes(resumes)
    
    # Save predictions in session state
    st.session_state["resumes"] = resumes
    st.session_state["predictions"] = predictions
    st.session_state["files"] = [file.name for file in uploaded_files]
    
    # Display predictions
    df_results = pd.DataFrame({
        "File Name": st.session_state["files"],
        "Resume Text": resumes,
        "Predicted Category": predictions
    })
    st.session_state["df_results"] = df_results
    st.write("Predicted categories:")
    st.dataframe(df_results)

# Step 2: Select category
if "df_results" in st.session_state:
    st.header("Step 2: Select a Category")
    categories = st.session_state["df_results"]["Predicted Category"].unique()
    selected_category = st.selectbox("Select a category", categories)
    
    if selected_category:
        filtered_resumes = st.session_state["df_results"][
            st.session_state["df_results"]["Predicted Category"] == selected_category
        ]
        st.session_state["filtered_resumes"] = filtered_resumes

# Step 3: Upload role file and find top 5 resumes
if "filtered_resumes" in st.session_state:
    st.header("Step 3: Upload Role/Job Description File")
    role_file = st.file_uploader("Upload a role description (TXT or PDF)", type=["txt", "pdf"])
    
    if role_file:
        # Read role text
        if role_file.type == "application/pdf":
            import PyPDF2
            pdf_reader = PyPDF2.PdfReader(role_file)
            role_text = " ".join(page.extract_text() for page in pdf_reader.pages)
        else:
            role_text = role_file.read().decode("utf-8")
        
        # Calculate similarity
        similarity_scores = calculate_similarity(role_text, st.session_state["filtered_resumes"]["Resume Text"])
        st.session_state["filtered_resumes"] = st.session_state["filtered_resumes"].assign(Similarity=similarity_scores)
        top_resumes = st.session_state["filtered_resumes"].sort_values(by="Similarity", ascending=False).head(5)
        
        # Display top 5 resumes
        st.subheader(f"Top 5 Resumes for Category: {selected_category}")
        for idx, row in top_resumes.iterrows():
            st.write(f"**File Name**: {row['File Name']}")
            st.write(f"**Similarity Score**: {row['Similarity']:.2f}")
            st.write(f"**Resume Text Preview**: {row['Resume Text'][:200]}...")
            st.write("---")
