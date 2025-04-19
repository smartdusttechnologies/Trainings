const { merge } = require("webpack-merge");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const commonConfig = require("./webpack.common.js");
const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const packageJson = require("../package.json");
const path = require("path");

const devConfig = {
  mode: "development",
  output: {
    publicPath: "http://localhost:3004/",
  },
  devServer: {
    port: 3004,
    // historyApiFallback: {
    //   index: "index.html",
    // },
    historyApiFallback: true,
    static: {
      // directory: "./dist",
      directory: path.join(__dirname, "../public"),
    },
    hot: true,
    open: true,
  },
  plugins: [
    new ModuleFederationPlugin({
      name: "product",
      filename: "remoteEntry.js",
      exposes: {
        "./ProductApp": "./src/bootstrap",
      },
      remotes: {
        auth: "auth@http://localhost:3002/remoteEntry.js",
      },
      shared: packageJson.dependencies,
    }),
    new HtmlWebpackPlugin({
      template: "./public/index.html",
    }),
  ],
};

module.exports = merge(commonConfig, devConfig);
