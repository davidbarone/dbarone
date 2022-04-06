// ----------------------------------------
// custom webpack behaviour to pull in
// correct .env file for environment
// variables.
// https://github.com/preactjs/preact-cli/wiki/Config-Recipes#use-environment-variables-in-your-application
// https://dev.to/sanfra1407/how-to-use-env-file-in-javascript-applications-with-webpack-18df
//
// Reference in UI using {DOTENV.<variable name>}
// ----------------------------------------
const path = require("path");

export default (config, env, helpers) => {
  const dotenv = require("dotenv").config({
    path: path.join(
      __dirname,
      env.production ? ".env.production" : ".env.development"
    ),
  });

  console.log(env.production);
  const { plugin } = helpers.getPluginsByName(config, "DefinePlugin")[0];
  const value = dotenv.parsed;
  plugin.definitions["DOTENV"] = JSON.stringify(value);
};
