// server.js
// https://www.npmjs.com/package/json-server
// Alternative to running JsonServer through command line, e.g. npx json-server --watch db.json
const jsonServer = require('json-server')
const path = require("path");
const server = jsonServer.create()
//const router = jsonServer.router('db.json')
const router = jsonServer.router(path.join(__dirname, "db.json"));
const middlewares = jsonServer.defaults()

server.use(middlewares)

// To handle POST, PUT and PATCH you need to use a body-parser. Using JSON Server's bodyParser
server.use(jsonServer.bodyParser);

// Simulate delay on all requests
server.use(function(req, res, next) {
    setTimeout(next, 0);
});

// Add createdAt to all POSTS
server.use((req, res, next) => {
    if (req.method === "POST") {
      req.body.createdDt = Date.now();
    } else if (req.method === "PUT") {
        req.body.updatedDt = Date.now();
    }
    // Continue to JSON Server router
    next();
});

// Fix up data types
server.use((req, res, next) => {
  if (req.method === "POST" || req.method === "PUT") {
    req.body.postId = parseInt(req.body.postId);
  }
  // Continue to JSON Server router
  next();
});


server.use(router)

server.listen(3000, () => {
  console.log('JSON Server is running')
})