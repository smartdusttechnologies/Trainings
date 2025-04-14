const { merge } = require("webpack-merge");
const commonConfig = require("./webpack.common.js");
const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const packageJson = require("../package.json");
const devConfig = {
  mode: "development",
  // output: {
  //   publicPath: "http://localhost:3000/",
  // },
  devServer: {
    port: 3000,
    historyApiFallback: {
      index: "index.html",
    },
    static: {
      directory: "./dist",
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
      },
      shared: packageJson.dependencies,
    }),
  ],
};

module.exports = merge(commonConfig, devConfig);
