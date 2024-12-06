import streamlit as st
import pandas as pd
import matplotlib.pyplot as plt
import joblib
import nltk
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.stem import WordNetLemmatizer
import PyPDF2

# Load saved models
svc = joblib.load('svc_model.pkl')
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label = joblib.load('label_encoder.pkl')


nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')



def preprocess_text(text):
    stop_words = set(stopwords.words('english'))
    tokens = word_tokenize(text.lower())
    lemmatizer = WordNetLemmatizer()
    return " ".join(
        lemmatizer.lemmatize(word)
        for word in tokens
        if word.isalnum() and word not in stop_words
    )


def pdf(path):
    with open(path,'rb') as file:        
        pdf_reader = PyPDF2.PdfReader(file)
        text = ""
        for page in pdf_reader.pages:
                text += page.extract_text()
    return text


def pdfs(pdf_files):
    resume_texts = []
    for pdf_file in pdf_files:        
        resume_text = pdf(pdf_file)
        resume_texts.append(resume_text) 
    return resume_texts  

def predict_category_for_resumes(resumes):
    preprocessed_resumes = [preprocess_text(resume) for resume in resumes]
    input_resumes = word_vectorizer.transform(preprocessed_resumes)
    
    predictions = svc.predict(input_resumes)
    predicted_categories = label.inverse_transform(predictions)
    
    return predicted_categories      

    