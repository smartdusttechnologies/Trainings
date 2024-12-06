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
