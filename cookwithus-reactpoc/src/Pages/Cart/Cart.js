import React, { useEffect, useState } from 'react'
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Skeleton from '@mui/material/Skeleton';
import { Button } from '@mui/material';
import ButtonGroup from '@mui/material/ButtonGroup';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import RemoveCircleIcon from '@mui/icons-material/RemoveCircle';
import { useNavigate } from "react-router-dom";
import styled from "@emotion/styled";
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { useSelector } from 'react-redux';
import emptyCart from '../../assets/Cart.webp'

const FlexBox = styled(Box)`
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const Cart = () => {
  const navigate = useNavigate();
  const [cart, setCart] = useState([]);
  const [totalPrice, setTotalPrice] = useState(0);
  const isSideNavOpen = useSelector((state) => state.cart.isSideNavOpen);
  const darkMode = useSelector((state) => state.cart.darkMode);

  const increaseQuantity = (item)=>{
      const updatedCart = cart.map((cartItem) => {
        if (cartItem.id === item.id) {
          return {
            ...cartItem,
            quantity: cartItem.quantity + 1,
          };
        }
        return cartItem;
      });
    setCart(updatedCart)
    localStorage.setItem('cart', JSON.stringify(updatedCart));
  }

  const decreaseQuantity = (item)=>{
    if(item.quantity > 1){
      const updatedCart = cart.map((cartItem) => {
        if (cartItem.id === item.id && item.quantity > 1) {
          return {
            ...cartItem,
            quantity: cartItem.quantity - 1,
          };
        }
        return cartItem;
      });
      setCart(updatedCart)
      localStorage.setItem('cart', JSON.stringify(updatedCart));
    }else if(item.quantity === 1){
      const updatedCart = cart.filter(cartItem => cartItem.id !== item.id)
      setCart(updatedCart)
      localStorage.setItem('cart', JSON.stringify(updatedCart));
    }
  }

  useEffect(() => {
    // Calculate the total price when the cart changes
    const calculatedTotalPrice = cart.reduce(
      (total, cartItem) => total + cartItem.price * cartItem.quantity,
      0
    );
    setTotalPrice(calculatedTotalPrice);
  }, [cart]);

  const getCartData = () => {
    const cartData = JSON.parse(localStorage.getItem('cart')) || [];
    setCart(cartData)
  }

  useEffect(() => {
    getCartData()
  }, []);

  
  return (
    <div 
      style={{
        height:'100%',
        minHeight:'43rem',
        width:'100%',
        margin:'auto',
      }}
    >
    {
      cart.length === 0 ? (
        <Box
          sx={{
            display:'flex',
            justifyContent:'center',
            textAlign:'center',
            height:'35rem'
          }}
        >
          <Box
            sx={{
              margin:'auto'
            }}
          >
            <img 
              src={emptyCart} 
              style={{
                width:'50%',
                marginBottom:'20px'
              }}
            />
            <Typography fontSize='20px' 
              color={!darkMode ? "text.secondary" : 'white'} 
              sx={{ flex: 1 }}
            >
              Your cart is empty
            </Typography>
            <Typography 
              color={!darkMode ? "text.secondary" : 'white'} 
              sx={{ flex: 1 }}
            >
              You can go to home page to view more restaurants
            </Typography>
          </Box>
        </Box>
      ) : 
      (
        <>
        <Grid container 
          sx={{
              width:'75%',
              minHeight:'10rem',
              margin:'auto',
              display:'grid',
              gridTemplateColumns: isSideNavOpen ? 'repeat(4, 1fr)' : "repeat(5, 1fr)",
              gap:'20px',
              '@media (max-width: 1200px)': {
                  gridTemplateColumns: isSideNavOpen ? 'repeat(2, 1fr)' : 'repeat(3, 1fr)',
              },
              '@media (max-width: 800px)': {
                  gridTemplateColumns: isSideNavOpen ? 'repeat(1, 1fr)' : 'repeat(2, 1fr)',
              },
              '@media (max-width: 600px)': {
                  gridTemplateColumns: 'repeat(1, 1fr)',
                  width:'85%',
              }
          }}
        >
           { cart.length > 0 && cart.map((item, index) => (
                <Box 
                    key={index} 
                    sx={{ 
                        width: '210px',
                        marginRight: 0.5,
                        my: 5,
                        cursor:'pointer',
                        transition: 'transform 0.3s',
                        '&:hover': {
                            transform: 'scale(1.1)'
                        },
                        '@media (max-width: 500px)': {
                          margin:'auto',
                          my: 5,
                        },
                    }}
                >
                    <img
                        style={{ width: '210px', height: 118,borderRadius:'10px' }}
                        alt={item.title}
                        src={item.src}
                    />
                    
                    {/* <Skeleton variant="rectangular" width={210} height={118} /> */}

                    <Box sx={{ pr: 2,ml:1 }}>
                        <Typography gutterBottom variant="body2" noWrap>
                            {item.title}
                        </Typography>
                        <Typography  display="block" variant="caption" >
                            {`₹ ${item.price}`}
                        </Typography>
                        <Typography variant="caption" 
                          color={!darkMode ? "text.secondary" : 'white'} 
                        >
                            {`${item.views} • ${item.createdAt}`}
                        </Typography>
                    </Box>
                    <Box sx={{ pt: 0.5 }}>
                    {/* <Skeleton /> */}
                    {/* <Skeleton width="60%" /> */}
                    </Box>
                    
                    <Box sx={{ pr: 2,ml:1 }}>
                      <ButtonGroup variant="outlined" size="small">
                        <Button
                          onClick={()=>increaseQuantity(item)}
                        >
                          <AddCircleIcon/>  
                        </Button>
                        <Button>
                          {item.quantity}
                        </Button>
                        <Button
                          onClick={()=>decreaseQuantity(item)}
                        >
                          <RemoveCircleIcon/>
                        </Button>
                      </ButtonGroup>
                    </Box>
                </Box>
           ))}
        </Grid>

        <Grid container 
          sx={{
              width:'75%',
              minHeight:'10rem',
              margin:'auto',
              display:'grid',
              gridTemplateColumns: "repeat(2, 1fr)",
              gap:'20px',
              '@media (max-width: 900px)': {
                  gridTemplateColumns: 'repeat(1, 1fr)',
                  gap:'5px'
              },
          }}
        >
          <Box
            sx={{
              width:'400px',
              margin:'auto',
              '@media (max-width: 600px)': {
                width:'250px',
              },
            }}
          >
            <Typography fontSize='18px' gutterBottom variant="body2" fontWeight="bold">
              Select delivery address
            </Typography>
            <Box 
              sx={{ 
                width: '280px',
                height:'130px',
                padding:'10px',
                my: 2,
                cursor:'pointer',
                boxShadow: 'rgba(99, 99, 99, 0.2) 0px 2px 8px 0px',
                transition: 'transform 0.3s',
                '&:hover': {
                    transform: 'scale(1.05)'
                },
                display:'flex',
                flexDirection:'column',
                alignItems:'center',
                gap:'30px',
                '@media (max-width: 600px)': {
                  width:'220px',
                },
              }}   
            >
              <Box
                sx={{
                  width:'80%',
                  margin:'auto',
                  display:'flex',
                  justifyContent:'space-around',
                  marginTop:'10px',
                  gap:'20px'
                }}
              >
                  <LocationOnIcon/>
                <Typography 
                  color={!darkMode ? "text.secondary" : 'white'} 
                  sx={{ flex: 1 }}
                >
                  Add New Address
                </Typography>
              </Box>
              <Button
               variant='contained'
               color='secondary'
               onClick={()=> navigate('/checkout')}
              >
                Add New
              </Button>
            </Box>
          </Box>

          {/* ACTIONS */}
          <Box 
            m="20px 0"
            sx={{
              width:'400px',
              margin:'auto',
              '@media (max-width: 600px)': {
                width:'250px',
              },
            }}
          >
            <FlexBox m="20px 0">
              <Typography fontSize='16px' fontWeight="bold">SUBTOTAL</Typography>
              <Typography fontSize='16px' fontWeight="bold">₹ {totalPrice}</Typography>
            </FlexBox>
            <Button
              sx={{
                backgroundColor: '#1976d2',
                color: "white",
                borderRadius: 0,
                minWidth: "100%",
                padding: "20px 40px",
                m: "20px 0",
                '&:hover': {
                  backgroundColor: '#3d8ad7',
                },
              }}
              onClick={() => {
                navigate("/checkout");
              }}
            >
              CHECKOUT
            </Button>
          </Box>
        </Grid>
        </>
      )
    }
    </div>
  )
}

export default Cart
