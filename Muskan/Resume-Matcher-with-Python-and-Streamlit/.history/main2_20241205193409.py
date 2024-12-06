
# Streamlit UI
st.title("Resume Classifier")
st.sidebar.header("Resumes")

# File uploader for multiple PDFs (Resumes)
upload_files = st.sidebar.file_uploader("Upload Resumes", type=["pdf"], accept_multiple_files=True)

# New section for uploading a large batch of PDFs
batch_upload_files = st.sidebar.file_uploader("Upload Batch of Resumes (up to 1000)", type=["pdf"], accept_multiple_files=True)

# File uploader for a new resume to compare
new_resume_file = st.sidebar.file_uploader("Upload Resume to Compare", type=["pdf"])

if upload_files:
    st.sidebar.text(f"{len(upload_files)} file(s) selected.")

    # Extract Text and Predict Categories
    resumes = pdfs(upload_files)
    predicted_categories = predict_category_for_resumes(resumes)
    names = [extract_name(resume) for resume in resumes]

    # Create DataFrame for Results
    df_results = pd.DataFrame({
        "Name": names,
        "Predicted Category": predicted_categories,
        "File": [file.name for file in upload_files],
        "Resume Text": resumes
    })
    st.dataframe(df_results)

    # Save Resumes in Folders
    save_resumes_to_folders(df_results, upload_files)

    # Category Dropdown
    categories = df_results["Predicted Category"].unique()
    selected_category = st.selectbox("Select a Category", categories)

    if selected_category:
        filtered_resumes = df_results[df_results["Predicted Category"] == selected_category]

        # Compute Cosine Similarity for Top 5 Resumes
        role_vector = word_vectorizer.transform([selected_category])
        preprocessed_texts = [preprocess_text(text) for text in filtered_resumes["Resume Text"]]
        tfidf_matrix = word_vectorizer.transform(preprocessed_texts)

        similarity_scores = cosine_similarity(role_vector, tfidf_matrix).flatten()
        filtered_resumes = filtered_resumes.assign(Similarity=similarity_scores)
        
        # Top 5 Resumes
        top_resumes = filtered_resumes.sort_values(by="Similarity", ascending=False).head(5)
        
        st.subheader(f"Top 5 Resumes Similar to Role: {selected_category}")
        for idx, row in top_resumes.iterrows():
            st.write(f"**Name**: {row['Name']}")
            st.write(f"**File**: {row['File']}")
            st.write(f"**Similarity Score with Role**: {row['Similarity']:.2f}")
            st.write(f"**Resume Text Preview**: {row['Resume Text'][:500]}...")
            st.write("---")

# Compare Uploaded Resume with Selected Category
if new_resume_file:
    st.sidebar.text("New resume uploaded for comparison.")

    # Extract text from the uploaded resume for comparison
    new_resume_text = pdf(new_resume_file)
    preprocessed_new_resume = preprocess_text(new_resume_text)

    # Transform the new resume text into the same vector space
    new_resume_vector = word_vectorizer.transform([preprocessed_new_resume])

    # Select Resumes from the chosen category
    if selected_category:
        filtered_resumes = df_results[df_results["Predicted Category"] == selected_category]

        # Calculate Cosine Similarity between the new resume and the filtered resumes
        preprocessed_texts = [preprocess_text(text) for text in filtered_resumes["Resume Text"]]
        tfidf_matrix = word_vectorizer.transform(preprocessed_texts)

        similarity_scores = cosine_similarity(new_resume_vector, tfidf_matrix).flatten()
        filtered_resumes = filtered_resumes.assign(Similarity=similarity_scores)
        
        # Top 5 Similar Resumes
        top_resumes = filtered_resumes.sort_values(by="Similarity", ascending=False).head(5)
        
        st.subheader(f"Top 5 Resumes Similar to the Uploaded Resume")
        for idx, row in top_resumes.iterrows():
            st.write(f"**Name**: {row['Name']}")
            st.write(f"**File**: {row['File']}")
            st.write(f"**Similarity Score with Uploaded Resume**: {row['Similarity']:.2f}")
            st.write(f"**Resume Text Preview**: {row['Resume Text'][:500]}...")
            st.write("---")

# New section for processing the batch of resumes
if batch_upload_files:
    st .sidebar.text(f"{len(batch_upload_files)} batch file(s) selected.")

    # Extract Text and Predict Categories for the batch
    batch_resumes = pdfs(batch_upload_files)
    batch_predicted_categories = predict_category_for_resumes(batch_resumes)
    batch_names = [extract_name(resume) for resume in batch_resumes]

    # Create DataFrame for Batch Results
    df_batch_results = pd.DataFrame({
        "Name": batch_names,
        "Predicted Category": batch_predicted_categories,
        "File": [file.name for file in batch_upload_files],
        "Resume Text": batch_resumes
    })
    st.dataframe(df_batch_results)

    # Category Dropdown for Batch
    batch_categories = df_batch_results["Predicted Category"].unique()
    selected_batch_category = st.selectbox("Select a Category for Batch", batch_categories)

    if selected_batch_category:
        filtered_batch_resumes = df_batch_results[df_batch_results["Predicted Category"] == selected_batch_category]

        # Compute Cosine Similarity for Top 5 Resumes in Batch
        role_vector_batch = word_vectorizer.transform([selected_batch_category])
        preprocessed_batch_texts = [preprocess_text(text) for text in filtered_batch_resumes["Resume Text"]]
        tfidf_batch_matrix = word_vectorizer.transform(preprocessed_batch_texts)

        batch_similarity_scores = cosine_similarity(role_vector_batch, tfidf_batch_matrix).flatten()
        filtered_batch_resumes = filtered_batch_resumes.assign(Similarity=batch_similarity_scores)
        
        # Top 5 Resumes from Batch
        top_batch_resumes = filtered_batch_resumes.sort_values(by="Similarity", ascending=False).head(5)
        
        st.subheader(f"Top 5 Resumes Similar to Role: {selected_batch_category} from Batch")
        for idx, row in top_batch_resumes.iterrows():
            st.write(f"**Name**: {row['Name']}")
            st.write(f"**File**: {row['File']}")
            st.write(f"**Similarity Score with Role**: {row['Similarity']:.2f}")
            st.write(f"**Resume Text Preview**: {row['Resume Text'][:500]}...")
            st.write("---")