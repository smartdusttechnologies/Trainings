import streamlit as st
import numpy as np 
import pandas as pd
import time
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


# a = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20]
# n = np.array(a)
# c = n.reshape((4,5))
# dic = { 
#   "name" : ["Hallo" , "Sam" ,"Peter"],
#   "Age" : [21,54,8],
#   "City" : ["Noida" ,"delhi" ,"Chandigarh"]
# }

# data = pd.read_csv("UpdatedResumeDataSet.csv")
# # st.dataframe(data ,width=500,height=1500)
# st.table(dic)
# st.json(dic)
# st.write(dic)

# @st.cache_data 
# def ret_time(a):
#  time.sleep(2)
#  return time.time()

# if st.checkbox("1"):
#   st.write(ret_time(1)) 
# if st.checkbox("2"):
#   st.write(ret_time(2)) 


import streamlit as st


st.set_page_config(page_title="Registration", layout="centered", page_icon=":clipboard:")


st.title("Registration Form")


col1, col2 = st.columns(2)

col1.text_input("First Name", value="first name")
col2.text_input("Second Name")

#
col3, col4 = st.columns([3, 1])

col3.text_input("Email")
col4.text_input("Mobile")

col5,col6 ,col7 = st.columns(3)
col5.text_input("Username")
a = col6.text_input("Password",tyem="password")

col7.text_input("Repeat Password",tyem="password")
but1 ,but2 ,but3 = st.columns([1,4,1])
agree = but1.checkbox("I agree")
if but3.button("submit"):
  if agree :
    st.success("Done")
  else :
    st.warning()