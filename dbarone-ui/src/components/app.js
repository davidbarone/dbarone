import { h } from "preact";
import { Router } from "preact-router";

import Header from "./header";

// Code-splitting is automated for `routes` directory
import Home from "../routes/home";
import Profile from "../routes/profile";
import PostList from "../routes/PostList";

const App = () => (
  <div id="app">
    <Header />
    <Router>
      <Home path="/" />
      <PostList path="/posts/" />
      <Profile path="/profile/" user="me" />
      <Profile path="/profile/:user" />
    </Router>
  </div>
);

export default App;
