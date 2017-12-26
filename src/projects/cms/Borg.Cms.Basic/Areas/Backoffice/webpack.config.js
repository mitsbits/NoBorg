var path = require('path');
var webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = function (env) {

    env = env || {};
    var isProd = env.NODE_ENV === 'production';

    var extractCSS = new ExtractTextPlugin('backoffice.css');
    // Setup base config for all environments
    var config = {
        entry: {
            backoffice: './Areas/Backoffice/entry'
        },
        output: {
            path: path.join(__dirname, '../../wwwroot/dist'),
            filename: '[name].js'
        },
        devtool: 'eval-source-map',
        resolve: {
            extensions: ['.ts', '.tsx', '.js', '.jsx']
        },
        plugins: [
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }),
            extractCSS,
        ],
        module: {
            rules: [
                { test: /\.css?$/, use: extractCSS.extract({ use: !isProd ? 'css-loader' : 'css-loader?minimize' })  },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' }
            ]
        }
    }

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