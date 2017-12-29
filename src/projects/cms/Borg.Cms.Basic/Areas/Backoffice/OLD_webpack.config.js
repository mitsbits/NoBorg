var path = require('path');
var webpack = require('webpack');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

var node_dir = __dirname + '../node_modules';

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
            //new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }),
            extractCSS,
            new CopyWebpackPlugin([
            // {output}/file.txt
                { from: 'node_modules/jquery-validation/dist/', to: 'jquery-validation/' },
                { from:  'node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js' },
                //{ from:  'node_modules/admin-lte/plugins/iCheck/', to: 'iCheck/' },
                { from: 'Areas/Backoffice/Static/assets/', to: 'assets/' },
                //{ from: 'node_modules/admin-lte/plugins/iCheck/all.css', to: 'iCheck.css' },
                //{ from: 'node_modules/admin-lte/plugins/iCheck/icheck.js' }
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
                },
                {
                    test: require.resolve('icheck'),
                    use: [{
                        loader: 'icheck',
                        options: 'iCheck'
                    }]
                }
            ],
            loaders: [
                {
                    test: /[\/\\]node_modules[\/\\]admin-lte[\/\\]plugins[\/\\]iCheck[\/\\]icheck\.js$/,
                    loader: "imports-loader?this=>window"
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