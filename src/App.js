import React from "react";
import logo from "./logo.svg";
import "./App.css";
import ListPosts from "./components/Posts/ListPosts";
import HomePage from "./components/HomePage";
import SearchPage from "./components/SearchPage";
import AdminPage from "./components/AdminPage";
import AccountManagement from "./components/AccountManagement";
import SinglePost from "./components/Posts/SinglePost";
import LoginPage from "./components/Login/LoginPage";
import LogoutPage from "./components/Login/LogoutPage";
import UserInfo from "./components/Login/UserInfo";
import SignUpPage from "./components/Login/SignUpPage";

import { BrowserRouter as Router, Route, Link } from "react-router-dom";

// Font Awesome
import { library } from "@fortawesome/fontawesome-svg-core";
import { fab } from "@fortawesome/free-brands-svg-icons";
import { faGithub } from "@fortawesome/free-brands-svg-icons";
import { faLinkedin } from "@fortawesome/free-brands-svg-icons";
import { faEnvelopeSquare } from "@fortawesome/free-solid-svg-icons";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { faDownload } from "@fortawesome/free-solid-svg-icons";
library.add(
  fab,
  faGithub,
  faLinkedin,
  faEnvelopeSquare,
  faTrashAlt,
  faDownload
);

const MainMenu = () => {
  return (
    <div>
      <Link to="/">
        <button
          style={{
            fontFamily: "'Kalam', cursive",
            fontSize: "20px",
            fontWeight: 400,
            border: "none",
            background: "none"
          }}
        >
          David Barone-BI Developer
        </button>
      </Link>
      <Link to="/posts">
        <button style={{ border: "none", background: "none" }}>Posts</button>
      </Link>
      <Link to="/search">
        <button style={{ border: "none", background: "none" }}>Search</button>
      </Link>
      <Link to="/admin">
        <button style={{ border: "none", background: "none" }}>Admin</button>
      </Link>
      <Link to="/signup">
        <button style={{ border: "none", background: "none" }}>Signup</button>
      </Link>
    </div>
  );
};

function ViewPostX({ match }) {
  return (
    <>
      <SinglePost id={match.params.id} />
    </>
  );
}

function App() {
  return (
    <Router>
      <div className="app">
        <header>
          <MainMenu />
          <span style={{ position: "absolute", right: "0px" }}>
            <UserInfo />
          </span>
        </header>
        <main className="main">
          <Route path="/posts/:id" component={ViewPostX} />
          <div style={{ padding: "0% 10%" }}>
            <Route exact path="/" component={HomePage} />
            <Route exact path="/posts" component={ListPosts} />
            <Route exact path="/search" component={SearchPage} />
            <Route exact path="/account" component={AccountManagement} />
            <Route exact path="/admin" component={AdminPage} />
            <Route exact path="/login" component={LoginPage} />
            <Route exact path="/logout" component={LogoutPage} />
            <Route exact path="/signup" component={SignUpPage} />
          </div>
        </main>
        <footer>
          <div>Copyright (c) David Barone 2019</div>
        </footer>
      </div>
    </Router>
  );
}

export default App;
