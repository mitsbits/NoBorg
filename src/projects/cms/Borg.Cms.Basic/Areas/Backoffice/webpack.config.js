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
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }),
            extractCSS,
            new CopyWebpackPlugin([

                { from: 'node_modules/jquery/dist/jquery.js' },
                { from: 'node_modules/jquery-validation/dist/', to: 'jquery-validation/' },
                { from: 'node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js' },
                { from: 'node_modules/admin-lte/dist/', to: 'adminlte/' },
                { from: 'node_modules/bootstrap/dist/', to: 'bootstrap/' },
                { from: 'node_modules/icheck/', to: 'icheck' },
                { from: 'node_modules/icheck/skins/', to: 'icheck/' },
                { from: 'node_modules/font-awesome/', to: 'font-awesome/' },
                { from: 'node_modules/ionicons/dist/', to: 'ionicons/' },
                { from: 'node_modules/select2/dist/', to: 'select2/' },
                { from: 'node_modules/datatables.net/js/jquery.dataTables.js' },
                { from: 'node_modules/datatables.net-bs/', to: 'datatables.net-bs/' },
                { from: 'node_modules/jsoneditor/dist/', to: 'jsoneditor/' },
                { from: 'node_modules/bootstrap-treeview/dist/', to: 'bootstrap-treeview/' },
                { from: 'node_modules/moment/min/moment-with-locales.min.js', to: 'moment.js' },
                { from: 'node_modules/wookmark/wookmark.min.js', to: 'wookmark.js' },
                { from: 'node_modules/ckeditor/', to: 'ckeditor/' },
                { from: 'node_modules/eonasdan-bootstrap-datetimepicker/build/', to: 'bootstrap-datetimepicker/' },
                { from: 'node_modules/daterangepicker/', to: 'daterangepicker/' },
                { from: 'Areas/Backoffice/Static/assets/', to: 'assets/' },
                { from: 'Areas/Backoffice/Static/ckeditor_4.8.0_full/ckeditor', to: 'ckeditor4/' },
            
                { from: 'Areas/Backoffice/Static/js/site.js', to: 'site.js' },

                { from: 'Areas/Backoffice/Static/css/backoffice.css' },
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