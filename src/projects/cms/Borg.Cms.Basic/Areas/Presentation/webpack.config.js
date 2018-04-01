var path = require('path');
var webpack = require('webpack');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

var node_dir = __dirname + '../node_modules';

module.exports = function (env) {
    env = env || {};
    var isProd = env.NODE_ENV === 'production';

    var extractCSS = new ExtractTextPlugin('Presentation.css');
    // Setup base config for all environments
    var config = {
        entry: {
            presentation: './Areas/Presentation/entry'
        },
        output: {
            path: path.join(__dirname, '../../wwwroot/pres'),
            filename: '[name].js'
        },
        devtool: 'eval-source-map',
        resolve: {
            extensions: ['.ts', '.tsx', '.js', '.jsx']
        },
        plugins: [
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }),
            extractCSS,
            new CopyWebpackPlugin([

                { from: 'node_modules/jquery/dist/jquery.js' },
                { from: 'node_modules/jquery-validation/dist/', to: 'jquery-validation/' },
                { from: 'node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js' },
    
                { from: 'node_modules/bootstrap/dist/', to: 'bootstrap/' },

                { from: 'node_modules/@fortawesome/fontawesome-free-webfonts/', to: 'font-awesome/' },
                { from: 'node_modules/ionicons/dist/', to: 'ionicons/' },

                { from: 'node_modules/moment/min/moment-with-locales.min.js', to: 'moment.js' },

                { from: 'Areas/Presentation/Static/assets/', to: 'assets/' },

                { from: 'Areas/Presentation/Static/js/site.js', to: 'site.js' },

                { from: 'Areas/Presentation/Static/css/theme.css' },
  
            ])
        ],
        module: {
            rules: [
                { test: /\.css?$/, use: extractCSS.extract({ use: !isProd ? 'css-loader' : 'css-loader?minimize' }) },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' },
                {
                    test: require.resolve('jquery'),
                    use: [{
                        loader: 'expose-loader',
                        options: 'jQuery'
                    }, {
                        loader: 'expose-loader',
                        options: '$'
                    }]
                }
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