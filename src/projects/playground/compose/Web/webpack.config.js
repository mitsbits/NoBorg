"use strict";
var path = require('path');
var webpack = require('webpack');
const bundleOutputDir = './wwwroot/dist';

module.exports = function (env) {

  env = env || {};
  var isProd = env.NODE_ENV === 'production';

  var config = {
    entry: {
      main: ['./Client/main.ts'],
      topic: ['./Client/topic.ts'],
    },
    output: {
      path: path.join(__dirname, bundleOutputDir),
      publicPath: './dist/',
      filename: '[name].js'
    },
    resolve: {
      alias: {
        'vue$': 'vue/dist/vue.esm.js'
      }
    },
    devtool: 'eval-source-map',
    plugins: [
      //new webpack.HotModuleReplacementPlugin(),
      new webpack.DefinePlugin({
        'process.env': {
          NODE_ENV: JSON.stringify(!isProd ? 'development' : 'production')
        }
      }),
      new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery', 'window.$': 'jquery', 'window.jQuery': 'jquery' }),
      new webpack.DllReferencePlugin({
        context: __dirname,
        manifest: require( bundleOutputDir + '/vendor-manifest.json')
      })
    ],
    module: {
      rules: [
        {
          test: /\.styl$/,
          use: [{
            loader: 'style-loader'
          }, {
            loader: 'css-loader',
            options: {
              modules: true,
              camelCase: true,
              importLoaders: 2,
              sourceMap: false,
              localIdentName: "[local]___[hash:base64:5]"
            }
          }, {
            loader: 'stylus-loader'
          }]
        },
        {
          test: /\.ts$/,
          loader: 'ts-loader',
          options: {
            appendTsSuffixTo: [/\.vue$/]
          }
        },
        {
          test: /\.vue$/,
          loader: 'vue-loader'
        },
        { test: /\.css/, loader: 'style-loader!css-loader' },
        { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' }
      ]
    }
  };

  // Alter config for prod environment
  if (isProd) {
    config.devtool = 'source-map';
    config.plugins = config.plugins.concat([
      new webpack.optimize.UglifyJsPlugin({
        sourceMap: true
      })
    ]);
  }

  return config;
};
