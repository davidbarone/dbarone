# webapp
Web UI Reference Application using TypeScript and Webpack.

- [webapp](#webapp)
  - [Key design decisions](#key-design-decisions)
  - [Tooling](#tooling)
  - [Setup](#setup)
    - [TypeScript](#typescript)
    - [WebPack Packages](#webpack-packages)
    - [Babel Dependencies](#babel-dependencies)
    - [React Packages](#react-packages)
    - [CSS loaders](#css-loaders)
    - [Webpack Configuration](#webpack-configuration)
    - [Folder Setup](#folder-setup)
    - [index.tsx](#indextsx)
    - [Template.html](#templatehtml)
    - [Npm Scripts](#npm-scripts)
    - [Webpack Watch](#webpack-watch)
    - [Run development web server](#run-development-web-server)
  - [Coding Standards](#coding-standards)
  - [Components](#components)
  - [Environment Variables](#environment-variables)
  - [API](#api)
  - [Styles + CSS](#styles--css)
    - [Reset Style](#reset-style)
    - [CSS Modules](#css-modules)
    - [Global.d.ts](#globaldts)
  - [Testing](#testing)
  - [Linting](#linting)
  - [Routes](#routes)
  - [Context](#context)
  - [Debugging (VSCode)](#debugging-vscode)
  - [Fake API](#fake-api)
  - [Links](#links)

Modern web development is complicated. There are a million ways to skin a cat, and the average developer has to worry about many design decisions before they evey start to code. This project is an attempt to capture all my personal design decisions into a single place, so I can easily spin up new web projects when needed.

## Key design decisions
- 100% API driven - server functionality should be in separate project (normally .NET Core web-api)
- Web UI layer to be 100% javascript application
- Use standard modern node build chain, but keep to core well-used components
  - TypeScript (for typed JavaScript)
  - Webpack (for bundling) - https://webpack.js.org/
  - Babel for Transpiling - https://babeljs.io/
  - React used as default component library - https://reactjs.org/
    - Manual setup to keep in full control
  - Jest (for testing) - https://jestjs.io/
- Modern development features
  - Full typeing (TypeScript)
  - Hot Module Loading (HML)
  - Linting
  - Debugging

## Tooling
- VSCode
  - Extension: ESLint Extension (Microsoft)
  - Extension: Markdown All in One (Yu Zhang)
  - Extension: Prettier - Code Formatter (Prettier)
- React Dev Tools: https://reactjs.org/link/react-devtools
  
## Setup
- Initialising the project: `npm init`

### TypeScript
- Installing TypeScript: `npm install typescript --save-dev`
- TypeScript Loader (if not using Babel 7.0): `npm install ts-loader --save-dev`
- Initialising the TypeScript config file: `npx tsc --init`

### WebPack Packages
- Installing npm packages: `npm install webpack webpack-cli webpack-dev-server --save-dev`
- Babel Loader: `npm install babel-loader --save-dev`
- HTML Webpack Plugin: `npm install html-webpack-plugin --save-dev`
- For environment variables: `npm install dotenv --save-dev`

### Babel Dependencies
- Babel CLI (to test babel transpilations): `npm install @babel/cli --save-dev`
- Presets: `npm install @babel/core @babel/preset-react @babel/preset-env @babel/preset-typescript --save-dev`
- Support of JavaScript async / await in Babel: `npm install --save-dev @babel/plugin-transform-runtime`

### React Packages
- Runtime packages: `npm install react react-dom`
- Development packages (for Webpack & Babel): `npm install @babel/plugin-transform-react-jsx --save-dev`
- Development types for ReactDom: `npm install @types/react-dom --save-dev`
- Types: `npm install @types/react --save-dev`
- React Router: `npm install react-router-dom`

### CSS loaders
- CSS Webpack loaders: `npm install style-loader css-loader --save-dev`
- Normalize.css: `npm install normalize.css`

### Webpack Configuration

TypeScript can be transpiled in this project in 2 ways:
- Using ts-loader Webpack loader
- Using Babel Webpack loader

The TypeScript transpilation method can be controlled using different npm scripts and webpack config files.
``` js
    "serve:ts": "webpack-dev-server --mode development --config webpack.ts.config.js",
    "serve:babel": "webpack-dev-server --mode development --config webpack.babel.config.js",
```

The Babel version uses the additional `.babelrc` configuration file to control which Babel plugings to use.

The default Webpack configuration file is shown below:

``` js
const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

// Change variable to change names of output files.
let appName = "webapp";

module.exports = {
  entry: { main: path.resolve("./src/index.tsx") },
  devtool: "inline-source-map",
  module: {
    rules: [
      /*
      {
        test: /\.tsx?$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
      */
      {
        test: /\.(js|jsx|tsx|ts)$/,
        exclude: "/node_modules/",
        loader: "babel-loader",
      },
      {
        test: /\.css$/,
        use: [
          { loader: "style-loader" },
          {
            loader: "css-loader",
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
    extensions: [".tsx", ".ts", ".js"],
  },
  output: {
    path: path.resolve(__dirname, "dist"),
    filename: `${appName}.js`,
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: "./src/template.html",
      filename: `${appName}.html`,
    }),
  ],
  devServer: {
    static: {
      directory: path.join(__dirname, "dist"),
    },
    hot: true,
    compress: true,
    server: "http",
    open: [`${appName}.html`],
    port: 8080,
  },
};
```

Notes:
- Configuration for development web server included ('devServer')
- Includes rules for TypeScript
- Includes configuration for HtmlWebpackPlugin
- CSS loader, with CSS Modules (local CSS)

### Folder Setup

```
+--\src
|  +--\assets (for static files)
|  +--\components (for business components that are reused in multiple components)
|  +--\routes (top-level components - accessible from a route)
|  +--\style
|  +--\utils (utilities, helpers)
|  +--\widgets (standard UI components, e.g. inputs, controls etc.)
|  +--index.ts (top-level component)
|  +--template.html (HTML5 template)
+--\tests
|  +--component_a.test.js
|  +--component_b.test.js
```

### index.tsx

This file is used to bootstrap React into the DOM:

``` js
import React from "react";
import { createRoot } from 'react-dom/client';
import 'normalize.css';
import './style/index.css';
import App from './components/App';

const container = document.getElementById('root') as Element;
const root = createRoot(container);
root.render(<App />);
```

### Template.html

The following HTML5 template is used:

``` html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <meta name="mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>Web UI Reference Application</title>
    <!-- <link rel="stylesheet" href="./style/index.css" /> -->
  </head>
  <body>
    <div id="root"></div>
  </body>
</html>
```

### Npm Scripts

The following scripts are added to the `package.json` file:

``` json
    "build": "webpack --mode production --node-env production",
    "build:dev": "webpack --mode development --node-env development",
    "build:prod": "webpack --mode production --node-env production",
    "watch": "webpack --watch",
    "serve": "webpack-dev-server --mode development",
    "serve:dev": "webpack-dev-server --mode development",
    "serve:prod": "webpack-dev-server --mode production",
    "serve:ts": "webpack-dev-server --mode development --config webpack.ts.config.js",
    "serve:babel": "webpack-dev-server --mode development --config webpack.babel.config.js",
    "start": "webpack serve --port 8080 --mode development --open --hot",
    "compile": "tsc",
    "test": "jest --collectCoverage",
    "go": "start npm run watch && start npm run serve",
    "babel": "babel src/index.tsx --out-file dist/babel-bundle.js"
```

### Webpack Watch

type `npm run wpw` to start run Webpack in watch mode. This watches for any file changes, then recompiles. Edit the `index.ts` file, and note how Webpack detects the change and recompiles.

### Run development web server

Type `npm run serve`, and open browser to http://localhost:8080. Note this has hot module loading enabled (HMR) - try and change the template.html file and save, and see the browser page automatically refresh.

## Coding Standards

General JavaScript coding standards:
- Google JavaScript style guide: https://google.github.io/styleguide/jsguide.html
- AirBnB JavaScript style guide: https://github.com/airbnb/javascript
- W3Schools JavaScript style guide: https://www.w3schools.com/js/js_conventions.asp

Filename conventions (React)
- https://github.com/airbnb/javascript/tree/master/react

Paradigms, Coding Standards:
- Clean Code (Robert C Martin): https://www.amazon.com/gp/product/0132350882/ref=as_li_tl?ie=UTF8&camp=1789&creative=9325&creativeASIN=0132350882&linkCode=as2&tag=brandonwoz-20&linkId=8af093cb2b8d9a87993f285341ff015a
- Clean Code (Ryan McDermott - focus on JavaScript): https://github.com/ryanmcdermott/clean-code-javascript

## Components

React is used to enable a web site to be built using reusable components. Key decisions for using React are:
- Functional components only
- Typed props is mandatory (Use of TypeScript)

## Environment Variables

Environment variables need to be stored for multiple environments (dev, prod). This includes server names, API endpoints and other settings.

- Variables to be stored in .env files
  - Production: `.env.production`
  - Development: `.env.development`
- Standard .env file format
- The dotenv package to be installed using `npm install dotenv --save-dev`
- Use Webpack `DefinePlugin` plugin to create environment variables:

``` js
const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const webpack = require("webpack");
const { isConditionalExpression } = require("typescript");

// Change variable to change names of output files.
let appName = "webapp";

module.exports = (env, argv) => {
  // Get the correct .env file for the environment:

  const dotenv = require("dotenv").config({
    path: path.join(
      __dirname,
      env.development ? ".env.development" : ".env.production"
    ),
  });
  const appSettings = dotenv.parsed;

  return {

    ...

    plugins: [
      new webpack.DefinePlugin({
        "process.env.APP_SETTINGS": JSON.stringify(appSettings),
      }),
    ],
  };
};
```
- The environment variables will be stored at `process.env.APP_SETTINGS`;
- The environment must be passed in to webpack from the npm script, for example:
``` js
    "build:dev": "webpack --mode development --env development",
    "build:prod": "webpack --mode production --env production",
```
- To use in code (for example in helper class to call API):
``` js
var settings = process.env.APP_SETTINGS as any;
console.debug(settings.API_DOMAIN);
```
- Ensure the .gitignore file excludes .env files from source code repository.

## API

## Styles + CSS

CSS is added to the Webpack configuration as follows:

`npm install --save-dev css-loader style-loader`

webpack.config.js, module section (Note css-loader comes AFTER style-loader):

``` js
 module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
      {
        test: /\.css$/,
        use: [
          { loader: "style-loader" },
          {
            loader: "css-loader",
            options: {
              modules: true,
              importLoaders: 1,
            },
          },
        ],
      },
    ],
  },
```

### Reset Style
I use normalize.css as the reset style as follows:

`npm install normalize.css`

Then, add the following to the entry `index.ts`:

`import 'normalize.css';`

### CSS Modules

CSS Modules are locally scoped CSS files. CSS Modules are configured in thge `webpack.config.js` file.

``` js
{
        test: /\.css$/,
        use: [
          { loader: "style-loader" },
          {
            loader: "css-loader",
            options: {
              modules: true,
              importLoaders: 1,
            },
          },
        ],
      },
```

### Global.d.ts

When referencing .css files in .tsx files, you may get following error:

`Cannot find module 'module.css' or its corresponding type declarations`

To import custom file types, you must use TypeScript's declare module syntax to let it know that it's ok to import. To do this, simply create a globals.d.ts (declaration file) wherever your other code's root directory is and then add in this code:

``` js
declare module '*.css';
```

## Testing

## Linting

Linting is used to check syntax, find problems, and enforce code style. ESLint is used:

https://eslint.org/docs/user-guide/getting-started

`npm install eslint --save-dev`

To initialise, the eslint config file:

`npm init @eslint/config`

Note this will add the @eslint/create-config package.
- How would you like to use ESLint?: **To check syntax, find problems, and enforce code style**
- What type of modules does your project use?: **JavaScript modules (import/export)**
- Which framework does your project use?: **React**
- Does your project use TypeScript? **Yes**
- Where does your code run? **Browser**
- How would you like to define a style for your project: **Answer questions about your style**
- What format do you want your config file to be in? **JSON**
- What style of indentation do you use?: **Spaces** (4 space default)
- What quotes do you use for strings?: **Single**
- What line endings do you use?: **Windows**
- Do you require semicolons?: **Yes**

The eslint config tool may then install additional packages, for example:

`eslint-plugin-react@latest @typescript-eslint/eslint-plugin@latest @typescript-eslint/parser@latest`

- Would you like to install them now with npm?: **Yes**

To perform linting, add the following npm script to `package.json`:

``` js
  "lint": "eslint **/*.tsx"
```

To see all the ESLint rules (and which ones are enabled by default):

https://eslint.org/docs/rules/

## Routes

React Router is used to link components to the Browser's URL.

https://reactrouterdotcom.fly.dev/docs/en/v6

To configure:

- Add react-router-dom: `npm install react-router-dom`
- Add the `BrowserRouter` into the main index.tsx, wrapping the `<App />` component.
- Configure routes in `App.tsx`.
- Add links, for example in the Header component.

## Context

## Debugging (VSCode)

## Fake API

Fake APIs are useful for several reasons:
- Don't need to wait until real API is built
- Can test the API interface early
- Can use for testing (Fake APIs can be seeded with test data)
- Quicker (no network IO)
- Simpler - no dependency on API project

`Json-server` is used for generating fake API servers:

- `npm install json-server npm-run-all --save-dev`
- Create a db.json file (can be in root, or could be in utils folder)
- Populate with data
- Start with `npx json-server --watch db.json`
- To run in parallel with React, use the npm-run-all package

``` js
  "start:api": "npx json-server --watch db.json",
  "start:all" : "run-p start:api serve:dev"
```

(Note this uses run-p to run npm scripts in parallel.)

The `--watch` behaviour results in any POST, DELETE, PUT methods updating the db.json file. This may not be what you require.
For example, to ensure a repeatable data environment, you may require the data file to reinitialise each time the app is restarted.
To do this, two additional modules have been created:
- `createMockDb.js`: This creates a new clean version of db.json
- `jsonServer.js`: This runs Json-Server as a module, and allows extra configuration

``` js
    "start:api": "node tools/jsonServer.js",
    "prestart:api": "node tools/createMockDb.js",
    "start:all": "run-p start:api serve:dev"
```

Note: the `prestart:api` script will always run before `start:api` by convention.

(Thanks to https://github.com/coryhouse/react-flux-building-applications)

Other useful links:
- Lorem Ipsum generator: https://loremipsumgenerator.org/
- Free online fake API server: https://my-json-server.typicode.com/
- Json-server source (GitHub), including instructions: https://github.com/typicode/json-server
- 

## Links
- https://dev.to/ruppysuppy/create-react-app-from-scratch-like-a-pro-de0#:~:text=%20Create%20React%20App%20from%20Scratch%20like%20a,use%20npm%20run%20build%20or%20npm...%20More%20
- https://onoya.dev/react-webpack-babel7-typescript/