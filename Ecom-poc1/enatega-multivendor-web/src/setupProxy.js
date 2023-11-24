const { createProxyMiddleware } = require('http-proxy-middleware');



module.exports = function(app) {

  app.use(

    '/msg91',

    createProxyMiddleware({

      target: 'https://control.msg91.com', // Replace with the URL of your API endpoint

      changeOrigin: true,

    })

  );

};