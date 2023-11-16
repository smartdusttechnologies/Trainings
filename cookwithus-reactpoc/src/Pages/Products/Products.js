import React, { useState } from 'react'
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Skeleton from '@mui/material/Skeleton';
import { Button } from '@mui/material';
import ButtonGroup from '@mui/material/ButtonGroup';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import RemoveCircleIcon from '@mui/icons-material/RemoveCircle';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { useSelector } from 'react-redux';

const data = [
    {
      id: 1,
      src: 'https://i.ytimg.com/vi/pLqipJNItIo/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLBkklsyaw9FxDmMKapyBYCn9tbPNQ',
      title: 'Don Diablo @ Tomorrowland Main Stage 2019 | Official…',
      channel: 'Don Diablo',
      views: '396k views',
      createdAt: 'a week ago',
      price: 350,
    },
    {
      id: 2,
      src: 'https://i.ytimg.com/vi/_Uu12zY01ts/hqdefault.jpg?sqp=-oaymwEZCPYBEIoBSFXyq4qpAwsIARUAAIhCGAFwAQ==&rs=AOn4CLCpX6Jan2rxrCAZxJYDXppTP4MoQA',
      title: 'Queen - Greatest Hits',
      channel: 'Queen Official',
      views: '40M views',
      createdAt: '3 years ago',
      price: 350,
    },
    {
      id: 3,
      src: 'https://i.ytimg.com/vi/kkLk2XWMBf8/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLB4GZTFu1Ju2EPPPXnhMZtFVvYBaw',
      title: 'Calvin Harris, Sam Smith - Promises (Official Video)',
      channel: 'Calvin Harris',
      views: '130M views',
      createdAt: '10 months ago',
      price: 350,
    },
    {
      id: 4,
      src: 'https://i.ytimg.com/vi/pLqipJNItIo/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLBkklsyaw9FxDmMKapyBYCn9tbPNQ',
      title: 'Don Diablo @ Tomorrowland Main Stage 2019 | Official…',
      channel: 'Don Diablo',
      views: '396k views',
      createdAt: 'a week ago',
      price: 350,
    },
    {
      id: 5,
      src: 'https://i.ytimg.com/vi/kkLk2XWMBf8/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLB4GZTFu1Ju2EPPPXnhMZtFVvYBaw',
      title: 'Calvin Harris, Sam Smith - Promises (Official Video)',
      channel: 'Calvin Harris',
      views: '130M views',
      createdAt: '10 months ago',
      price: 350,
    },
  ];
  
const Products = () => {
  const [meals, setMeals] = useState(data);
  const [cart, setCart] = useState([]);
  const isSideNavOpen = useSelector((state) => state.cart.isSideNavOpen);
  const darkMode = useSelector((state) => state.cart.darkMode);

  const handleAddToCart = (item) => {
    let cartData = JSON.parse(localStorage.getItem('cart')) || [];
    const itemInCart = cartData.find((cartItem) => cartItem.id === item.id);

    if (itemInCart) {
      toast.warn('Item is already in the cart.', { position: "bottom-center", theme: "dark" });
    } else {
      // If the item is not in the cart, add it with a quantity of 1
      cartData = [...cartData, { ...item, quantity: 1 }]
      setCart([...cartData, { ...item, quantity: 1 }]);
      toast.success('Item Added to cart', { position: "bottom-center", theme: "dark" });
    }
    localStorage.setItem('cart', JSON.stringify(cartData));
  }

  return (
    <div 
    style={{
      height:'100%',
      minHeight:'43rem',
      width:'100%',
      margin:'auto',
    }}
    >
        <Grid container 
        sx={{
            width:'75%',
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
            }
        }}
        >
           { meals.length > 0 && meals.map((item, index) => (
                <Box 
                    key={index} 
                    sx={{ 
                        width: 210, 
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
                        style={{ width: 210, height: 118,borderRadius:'10px' }}
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
                      <Button 
                        variant='outlined'
                        size='small'
                        onClick={() => handleAddToCart(item)}
                      >
                        Add to Cart
                      </Button>
                    </Box>
                </Box>
           ))}
        </Grid>
        <ToastContainer />
    </div>
  )
}

export default Products
