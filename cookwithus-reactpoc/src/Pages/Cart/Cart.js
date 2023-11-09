import React, { useEffect, useState } from 'react'
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Skeleton from '@mui/material/Skeleton';
import { Button } from '@mui/material';
import ButtonGroup from '@mui/material/ButtonGroup';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import RemoveCircleIcon from '@mui/icons-material/RemoveCircle';

const Cart = () => {
  const [cart, setCart] = useState([]);

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
           { cart.map((item, index) => (
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
                      <ButtonGroup variant="outlined" size="small">
                        <Button>
                          <AddCircleIcon/>  
                        </Button>
                        <Button>
                          {item.quantity}
                        </Button>
                        <Button>
                          <RemoveCircleIcon/>
                        </Button>
                      </ButtonGroup>
                    </Box>
                </Box>
           ))}
        </Grid>
    </div>
  )
}

export default Cart
