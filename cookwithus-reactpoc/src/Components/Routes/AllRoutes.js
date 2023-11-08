import React from 'react'
import { Route, Routes } from 'react-router-dom'
import Home from '../../Pages/Home/Home'
import Products from '../../Pages/Products/Products'
import Cart from '../../Pages/Cart/Cart'

const AllRoutes = () => {
  return (
    <Routes>
        <Route path='/' element={<Home/>}></Route>
        <Route path='/meals' element={<Products/>}></Route>
        <Route path='/cart' element={<Cart/>}></Route>
    </Routes>
  )
}

export default AllRoutes
