import React from 'react'
import Iframe from 'react-iframe'
const LocationMap = () => {
  return (
      <div style={{width:'99%' , margin:'auto'}}>

            <Iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d16268.837487922925!2d85.03436917713583!3d25.62474119891454!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x39ed565f7ecb04eb%3A0xfeac56c8e4af9bc7!2sDanapur%2C%20Bihar!5e0!3m2!1sen!2sin!4v1678525769743!5m2!1sen!2sin"
            width="100%"
            height="350px"
            frameBorder="0"
            style={{ border: 0 }}
            allowFullScreen=""
            aria-hidden="false"
            tabIndex="0"
            allowfullscreen=""
            loading="lazy"
            referrerpolicy="no-referrer-when-downgrade"
            ></Iframe>
      </div>
   
  )
}

export default LocationMap