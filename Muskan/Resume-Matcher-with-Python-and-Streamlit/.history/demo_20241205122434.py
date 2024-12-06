import streamlit as st
import numpy as np 
import pandas as pd
# st.title("Hello World")
# st.header("Header")
# st.subheader("Sub header")
# st.text("text")
# st.markdown("""
# # h1 
# ## h2
# ### h3
# :moon:<br>
# :sunglasses:<br>
# **bold**
# _italic_
# """,True)
# d = {"name" : "Insaan",
# "Langauge": "Insani"
# }
# st.write(d)


a = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20]
n = np.array(a)
c = n.reshape((4,5))
dic = { 
  "name" : ["Hallo" , "Sam" ,"Peter"],
  "Age" : [21,54,8],
  "City" : ["Noida" ,"delhi" ,"Chandigarh"]
}

data = pd.read_csv("UpdatedResumeDataSet.csv")
st.dataframe(data ,width=500,height=1500)
st.table(dic)
st.json(dic)
