const { merge } = require("webpack-merge");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const commonConfig = require("./webpack.common.js");
const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const packageJson = require("../package.json");

const devConfig = {
  mode: "development",
  // output: {
  //   publicPath: "http://localhost:3001/",
  // },
  devServer: {
    port: 3001,
    // historyApiFallback: {
    //   index: "index.html",
    // },
    historyApiFallback: true,
    static: {
      directory: "./dist",
    },
    hot: true,
    open: true,
  },
  plugins: [
    new ModuleFederationPlugin({
      name: "marketing",
      filename: "remoteEntry.js",
      exposes: {
        "./MarketingApp": "./src/bootstrap",
      },
      shared: packageJson.dependencies,
    }),
    new HtmlWebpackPlugin({
      template: "./public/index.html",
    }),
  ],
};

module.exports = merge(commonConfig, devConfig);
