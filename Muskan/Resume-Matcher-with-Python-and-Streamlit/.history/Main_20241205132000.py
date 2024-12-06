import streamlit as st
import pandas as pd
import matplotlib.pyplot as plt

import joblib

# Load saved models
svc = joblib.load('svc_model.pkl')
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label = joblib.load('label_encoder.pkl')
