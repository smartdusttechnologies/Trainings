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

const data = [
    {
      id: 1,
      src: 'https://i.ytimg.com/vi/pLqipJNItIo/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLBkklsyaw9FxDmMKapyBYCn9tbPNQ',
      title: 'Don Diablo @ Tomorrowland Main Stage 2019 | Official…',
      channel: 'Don Diablo',
      views: '396k views',
      createdAt: 'a week ago',
      price: 350,
      quantity: 0,
    },
    {
      id: 2,
      src: 'https://i.ytimg.com/vi/_Uu12zY01ts/hqdefault.jpg?sqp=-oaymwEZCPYBEIoBSFXyq4qpAwsIARUAAIhCGAFwAQ==&rs=AOn4CLCpX6Jan2rxrCAZxJYDXppTP4MoQA',
      title: 'Queen - Greatest Hits',
      channel: 'Queen Official',
      views: '40M views',
      createdAt: '3 years ago',
      price: 350,
      quantity: 0,
    },
    {
      id: 3,
      src: 'https://i.ytimg.com/vi/kkLk2XWMBf8/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLB4GZTFu1Ju2EPPPXnhMZtFVvYBaw',
      title: 'Calvin Harris, Sam Smith - Promises (Official Video)',
      channel: 'Calvin Harris',
      views: '130M views',
      createdAt: '10 months ago',
      price: 350,
      quantity: 0,
    },
    {
      id: 4,
      src: 'https://i.ytimg.com/vi/pLqipJNItIo/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLBkklsyaw9FxDmMKapyBYCn9tbPNQ',
      title: 'Don Diablo @ Tomorrowland Main Stage 2019 | Official…',
      channel: 'Don Diablo',
      views: '396k views',
      createdAt: 'a week ago',
      price: 350,
      quantity: 0,
    },
    {
      id: 5,
      src: 'https://i.ytimg.com/vi/kkLk2XWMBf8/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLB4GZTFu1Ju2EPPPXnhMZtFVvYBaw',
      title: 'Calvin Harris, Sam Smith - Promises (Official Video)',
      channel: 'Calvin Harris',
      views: '130M views',
      createdAt: '10 months ago',
      price: 350,
      quantity: 0,
    },
  ];
  
const Products = () => {
  const [meals, setMeals] = useState(data);
  const [cart, setCart] = useState([]);

  const handleAddToCart = (item) => {
    // let cartData = JSON.parse(localStorage.getItem('cart')) || [];
    // const alreadyInCart = cartData.some((i) => i.id === item.id);


    // if (alreadyInCart) {
    //   console.log('Item is already in the cart.');
    // } else {
    //   cartData = [...cartData, item];
    //   localStorage.setItem('cart', JSON.stringify(cartData));
    //   console.log('Item added to the cart');

    //   setCart(cartData);
    // }

    let cartData = JSON.parse(localStorage.getItem('cart')) || [];
    const itemInCart = cartData.find((cartItem) => cartItem.id === item.id);

    if (itemInCart) {
      // If the item is already in the cart, update its quantity
      // const updatedCart = cartData.map((cartItem) => {
      //   if (cartItem.id === item.id) {
      //     return {
      //       ...cartItem,
      //       quantity: cartItem.quantity + 1,
      //     };
      //   }
      //   return cartItem;
      // });
      // cartData = updatedCart;
      // setCart(updatedCart);
      console.log('Item is already in the cart.')
      toast.warn('Item is already in the cart.', { position: "bottom-center", theme: "dark" });
    } else {
      // If the item is not in the cart, add it with a quantity of 1
      cartData = [...cartData, { ...item, quantity: 1 }]
      setCart([...cartData, { ...item, quantity: 1 }]);
      toast.success('Item Added to cart', { position: "bottom-center", theme: "dark" });

      console.log('Item Added to cart')
    }
    localStorage.setItem('cart', JSON.stringify(cartData));
    console.log(cartData)
  }

  return (
    <div 
    style={{
            height:'43rem'
    }}
    >
        <Grid container 
        sx={{
            width:'75%',
            margin:'auto',
            display:'grid',
            gridTemplateColumns:"repeat(4, 1fr)",
        }}
        >
           {data.map((item, index) => (
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
                        }
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
                        <Typography variant="caption" color="text.secondary">
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
