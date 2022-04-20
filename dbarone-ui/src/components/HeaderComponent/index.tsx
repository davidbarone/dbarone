import React, { FunctionComponent } from 'react';
import { Link } from 'react-router-dom';
import style from './style.css';

const Header: FunctionComponent = () => (
    <header className={style.header}>
        <h1>Web UI Reference Application</h1>
        <nav>
            <Link to="/">
                Home
            </Link>
            <Link to="/posts">
                Posts
            </Link>
            <Link to="/counter">
                Counter
            </Link>
        </nav>
    </header>
);

export default Header;