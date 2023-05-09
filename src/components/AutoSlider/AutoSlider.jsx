import React, { Component } from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css"; 
import "slick-carousel/slick/slick-theme.css";

export default class SlickGoTo extends React.Component {
  state = {
    slideIndex: 0,
    updateCount: 0
  };

  render() {
    const settings = {
      dots: false,
      infinite: true,
      speed: 500,
      slidesToShow: 1,
      slidesToScroll: 1,
      autoplay: true,
      autoplaySpeed: 2000,

      afterChange: () =>
      this.setState(state => ({ updateCount: state.updateCount + 1 })),
    beforeChange: (current, next) => this.setState({ slideIndex: next })
    };

    const images = [
        'https://static.wixstatic.com/media/a00e7791c7984675a5c2439fdf4a37c6.jpg/v1/fill/w_1108,h_738,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/Modern%20Digital%20Watch.jpg' , 'https://static.wixstatic.com/media/11062b_0f162ab89785432f9b834066fa96982b~mv2.jpg/v1/fill/w_1108,h_738,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/Father%20and%20Son.jpg' , 'https://static.wixstatic.com/media/57d1fed6beee4982a729978553fb8ae2.jpg/v1/fill/w_1108,h_738,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/Computer%20Circuit%20Board.jpg' , 'https://static.wixstatic.com/media/ab483cd71cf946eaaf88b772ed132f63.jpg/v1/fill/w_1115,h_738,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/Motherboard%20Installation.jpg' , 'https://static.wixstatic.com/media/11062b_8818cb77a1be44d093816fa55b25cb57~mv2.jpg/v1/fill/w_1106,h_738,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/Gaming.jpg' , 'https://static.wixstatic.com/media/11062b_02f3dbceab3f4181a0ea4767efbf280d~mv2.jpg/v1/fill/w_1239,h_739,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/Servers.jpg'
    ]

    return (
      <div style={{width:'100%'}}>
        <Slider ref={slider => (this.slider = slider)} {...settings} arrows={false}>
            {
                images.map((i)=>(
                    <div style={{width:'100%'}}>
                        <img src={i} style={{width:'100%'}} alt="" />
                    </div>
                ))
            }
        </Slider>
      </div>
    );
  }
}