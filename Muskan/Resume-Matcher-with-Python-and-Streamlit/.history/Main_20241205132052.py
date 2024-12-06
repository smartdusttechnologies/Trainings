import streamlit as st
import pandas as pd
import matplotlib.pyplot as plt
import joblib
import nltk
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.stem import WordNetLemmatizer

# Load saved models
svc = joblib.load('svc_model.pkl')
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label = joblib.load('label_encoder.pkl')


nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')
