"use strict";

module.exports = {
    entry: "./wwwroot/index.js",
    output: {
        filename: "./wwwroot/dist/bundle.js"
    },
    resolve: {
        modules: ['./wwwroot', 'node_modules']
    },
    module: {
        rules: [
            {
                test: /\.js|.jsx$/,
                use: [
                    'babel-loader'
                ]
            }
        ]
    }
};