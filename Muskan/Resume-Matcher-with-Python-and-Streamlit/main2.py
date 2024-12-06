import streamlit as st
import pandas as pd
import joblib
import nltk
import os
from sklearn.metrics.pairwise import cosine_similarity
from nltk.tokenize import word_tokenize
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer
import PyPDF2
import spacy


svc = joblib.load('svc_model.pkl')
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label = joblib.load('label_encoder.pkl')
nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')
nlp = spacy.load('en_core_web_sm')


def preprocess_text(text):
    stop_words = set(stopwords.words('english'))
    tokens = word_tokenize(text.lower())
    lemmatizer = WordNetLemmatizer()
    return " ".join(
        lemmatizer.lemmatize(word)
        for word in tokens
        if word.isalnum() and word not in stop_words
    )


def pdf(file):
    pdf_reader = PyPDF2.PdfReader(file)
    text = ""
    for page in pdf_reader.pages:
        text += page.extract_text()
    return text


def predict_category_for_resumes(resumes):
    preprocessed_resumes = [preprocess_text(resume) for resume in resumes]
    input_resumes = word_vectorizer.transform(preprocessed_resumes)
    predictions = svc.predict(input_resumes)
    predicted_categories = label.inverse_transform(predictions)
    return predicted_categories


def extract_name(resume_text):
    doc = nlp(resume_text)
    for ent in doc.ents:
        if ent.label_ == "PERSON":
            return ent.text
    return "Unknown"


def save_resumes_to_folders(df_results, upload_files):
    base_directory = "Resume"
    if not os.path.exists(base_directory):
        os.makedirs(base_directory)

    for idx, row in df_results.iterrows():
        category = row['Predicted Category']
        category_folder = os.path.join(base_directory, category)
        if not os.path.exists(category_folder):
            os.makedirs(category_folder)

        file = upload_files[idx]
        save_path = os.path.join(category_folder, file.name)
        with open(save_path, "wb") as f:
            f.write(file.getbuffer())
        print(f"Saved {file.name} to {category_folder}")


st.title("Resume Classifier")
st.sidebar.header("Resumes")


upload_files_first = st.sidebar.file_uploader("Upload Resumes", type=["pdf"], accept_multiple_files=True)
if upload_files_first:
    st.sidebar.text(f"{len(upload_files_first)} files selected for first batch.")
    resumes_first = [pdf(file) for file in upload_files_first]
    predicted_categories_first = predict_category_for_resumes(resumes_first)
    names_first = [extract_name(resume) for resume in resumes_first]
    
    
    df_results_first = pd.DataFrame({
        "Name": names_first,
        "Predicted Category": predicted_categories_first,
        "File": [file.name for file in upload_files_first],
        "Resume Text": resumes_first
    })
    st.dataframe(df_results_first)
    
   
    save_resumes_to_folders(df_results_first, upload_files_first)
    
   
    categories_first = df_results_first["Predicted Category"].unique()
    selected_category = st.selectbox("Select a Category", categories_first)
    
    if selected_category:
        st.session_state.selected_category = selected_category


if 'selected_category' in st.session_state:
    st.sidebar.header("Upload  Resumes for Selected Category")
    upload_files_second = st.sidebar.file_uploader(f"Upload  Resumes for Category: {st.session_state.selected_category}",
                                                  type=["pdf"], accept_multiple_files=True)
    
    if upload_files_second:
        st.sidebar.text(f"{len(upload_files_second)} files selected for second batch.")
        resumes_second = [pdf(file) for file in upload_files_second]
        
        
        selected_category_vector = word_vectorizer.transform([st.session_state.selected_category])
        preprocessed_resumes_second = [preprocess_text(resume) for resume in resumes_second]
        tfidf_matrix = word_vectorizer.transform(preprocessed_resumes_second)
        similarity_scores = cosine_similarity(selected_category_vector, tfidf_matrix).flatten()
        
       
        top_resumes = pd.DataFrame({
            # "Resume": resumes_second,
            "File": [file.name for file in upload_files_second],
            "Similarity": similarity_scores
        }).sort_values(by="Similarity", ascending=False).head(5)
        
        st.subheader(f"Top 5 Resumes Similar to Role: {st.session_state.selected_category}")
        for idx, row in top_resumes.iterrows():
            st.write(f"**File**: {row['File']}")
            st.write(f"**Similarity Score**: {row['Similarity']:.2f}")
            # st.write(f"**Resume Text Preview**: {row['Resume'][:500]}...")
            st.write("---")
