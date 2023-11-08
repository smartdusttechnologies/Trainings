import React, { useState } from 'react'
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Skeleton from '@mui/material/Skeleton';
import { Button } from '@mui/material';

const data = [
    {
      src: 'https://i.ytimg.com/vi/pLqipJNItIo/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLBkklsyaw9FxDmMKapyBYCn9tbPNQ',
      title: 'Don Diablo @ Tomorrowland Main Stage 2019 | Official…',
      channel: 'Don Diablo',
      views: '396k views',
      createdAt: 'a week ago',
      price: '₹ 350',
    },
    {
      src: 'https://i.ytimg.com/vi/_Uu12zY01ts/hqdefault.jpg?sqp=-oaymwEZCPYBEIoBSFXyq4qpAwsIARUAAIhCGAFwAQ==&rs=AOn4CLCpX6Jan2rxrCAZxJYDXppTP4MoQA',
      title: 'Queen - Greatest Hits',
      channel: 'Queen Official',
      views: '40M views',
      createdAt: '3 years ago',
      price: '₹ 350',
    },
    {
      src: 'https://i.ytimg.com/vi/kkLk2XWMBf8/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLB4GZTFu1Ju2EPPPXnhMZtFVvYBaw',
      title: 'Calvin Harris, Sam Smith - Promises (Official Video)',
      channel: 'Calvin Harris',
      views: '130M views',
      createdAt: '10 months ago',
      price: '₹ 350',
    },
    {
      src: 'https://i.ytimg.com/vi/pLqipJNItIo/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLBkklsyaw9FxDmMKapyBYCn9tbPNQ',
      title: 'Don Diablo @ Tomorrowland Main Stage 2019 | Official…',
      channel: 'Don Diablo',
      views: '396k views',
      createdAt: 'a week ago',
      price: '₹ 350',
    },
    {
      src: 'https://i.ytimg.com/vi/kkLk2XWMBf8/hqdefault.jpg?sqp=-oaymwEYCNIBEHZIVfKriqkDCwgBFQAAiEIYAXAB&rs=AOn4CLB4GZTFu1Ju2EPPPXnhMZtFVvYBaw',
      title: 'Calvin Harris, Sam Smith - Promises (Official Video)',
      channel: 'Calvin Harris',
      views: '130M views',
      createdAt: '10 months ago',
      price: '₹ 350',
    },
  ];
  
const Products = () => {
  const [cart, setCart] = useState([]);

  function handleAddToCart(item) {
    const updatedCart = [...cart];
    // Add the item to the cart
    updatedCart.push(item);
    // Update the cart state with the new cart
    setCart(updatedCart);
    console.log(item)
  }

  return (
    <div style={{
        height:'43rem'
    }}>
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
                        {/* <Typography display="block" variant="caption" color="text.secondary">
                            {item.channel}
                        </Typography> */}
                        <Typography  display="block" variant="caption" >
                            {item.price}
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
    </div>
  )
}

export default Products
