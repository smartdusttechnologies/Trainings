const { merge } = require("webpack-merge");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const commonConfig = require("./webpack.common.js");
const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const packageJson = require("../package.json");
const path = require("path");

const devConfig = {
  mode: "development",
  output: {
    publicPath: "auto",
  },
  devServer: {
    port: 3002,
    historyApiFallback: {
      historyApiFallback: true,
    },
    static: {
      directory: path.resolve(__dirname, "dist"),
      publicPath: "/",
    },
    hot: true,
    open: true,
  },
  plugins: [
    new ModuleFederationPlugin({
      name: "auth",
      filename: "remoteEntry.js",
      exposes: {
        "./AuthApp": "./src/bootstrap",
        "./AuthProvider": "./src/Context/AuthProvider.js",
        "./useAuth": "./src/Hookes/useAuth.js",
        "./LoginButton": "./src/Component/Login.js",
        "./LogoutButton": "./src/Component/Logout.js",
        "./Callback": "./src/Component/Callback.js",
      },
      shared: packageJson.dependencies,
    }),
    new HtmlWebpackPlugin({
      template: "./public/index.html",
    }),
  ],
};

module.exports = merge(commonConfig, devConfig);
