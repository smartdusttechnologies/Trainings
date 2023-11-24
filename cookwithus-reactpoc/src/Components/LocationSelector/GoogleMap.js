import React, { Component } from 'react';
import { Map, GoogleApiWrapper, Marker } from 'google-maps-react';

class GoogleMap extends Component {
  render() {
    const { latitude, longitude, zoom } = this.props;
    const mapStyles = {
      width: '70%',
      height: '400px',
      margin:'20px auto',
    };

    return (
      <Map
        google={this.props.google}
        zoom={zoom ? zoom : 12}
        style={mapStyles}
        initialCenter={{ lat: latitude, lng: longitude }}
      >
        <Marker position={{ lat: latitude, lng: longitude }} />
      </Map>
    );
  }
}

export default GoogleApiWrapper({
  apiKey: 'YOUR_GOOGLE_MAPS_API_KEY',
})(GoogleMap);