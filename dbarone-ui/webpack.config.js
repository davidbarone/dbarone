const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');
const { isConditionalExpression } = require('typescript');

// Change variable to change names of output files.
let appName = 'webapp';

module.exports = (env, argv) => {
    // Get the correct .env file for the environment:

    const dotenv = require('dotenv').config({
        path: path.join(
            __dirname,
            env.development ? '.env.development' : '.env.production'
        ),
    });
    const appSettings = dotenv.parsed;

    return {
        entry: { main: path.resolve('./src/index.tsx') },
        devtool: 'inline-source-map',
        module: {
            rules: [
                {
                    test: /\.(js|jsx|tsx|ts)$/,
                    exclude: '/node_modules/',
                    loader: 'babel-loader',
                },
                {
                    test: /\.css$/,
                    use: [
                        { loader: 'style-loader' },
                        {
                            loader: 'css-loader',
                            options: {
                                modules: true,
                                importLoaders: 1,
                            },
                        },
                    ],
                },
            ],
        },
        resolve: {
            extensions: ['.tsx', '.ts', '.js'],
        },
        output: {
            path: path.resolve(__dirname, 'dist'),
            publicPath: '/',
            filename: `${appName}.js`,
        },
        plugins: [
            new HtmlWebpackPlugin({
                template: './src/template.html',
                filename: `${appName}.html`,
            }),
            new webpack.DefinePlugin({
                'process.env.APP_SETTINGS': JSON.stringify(appSettings),
            }),
        ],
        devServer: {
            static: {
                directory: path.join(__dirname, 'dist'),
            },
            hot: true,
            historyApiFallback: {
                rewrites: [
                    { from: /./, to: 'webapp.html' }
                ]
            },
            compress: true,
            server: 'http',
            open: [`${appName}.html`],
            port: 8080,
        },
    };
};
