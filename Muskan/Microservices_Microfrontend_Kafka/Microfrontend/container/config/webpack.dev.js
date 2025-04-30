const { merge } = require("webpack-merge");
const commonConfig = require("./webpack.common.js");
const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const packageJson = require("../package.json");
const path = require("path");
const devConfig = {
  mode: "development",
  // output: {
  //   publicPath: "http://localhost:3000/",
  // },
  devServer: {
    port: 3000,
    // historyApiFallback: {
    //   index: "index.html",
    // },
    historyApiFallback: true,
    static: {
      directory: path.resolve(__dirname, "dist"),
    },
    hot: true,
    open: true,
  },
  plugins: [
    new ModuleFederationPlugin({
      name: "container",
      remotes: {
        marketing: "marketing@http://localhost:3001/remoteEntry.js",
        auth: "auth@http://localhost:3002/remoteEntry.js",
        dashboard: "dashboard@http://localhost:3003/remoteEntry.js",
        product: "product@http://localhost:3004/remoteEntry.js",
        basket: "basket@http://localhost:3005/remoteEntry.js",
        order: "order@http://localhost:3006/remoteEntry.js",
      },
      shared: packageJson.dependencies,
    }),
  ],
};

module.exports = merge(commonConfig, devConfig);
