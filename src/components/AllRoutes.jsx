import React from 'react'
import {Route, Routes} from 'react-router-dom'
import Home from '../Pages/Home/Home'
import Contact from '../Pages/Contact/Contact'
import About from '../Pages/About/About'

const AllRoutes = () => {
  return (
    <Routes>
      <Route path='/' element={<Home/>}></Route>
      <Route path='/contact' element={<Contact/>}></Route>
      <Route path='/about' element={<About/>}></Route>
    </Routes>
  )
}

export default AllRoutes
