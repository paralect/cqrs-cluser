"use strict";

module.exports = {
    entry: "./wwwroot/index.jsx",
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