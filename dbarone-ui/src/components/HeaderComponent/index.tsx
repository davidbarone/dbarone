import React, { FunctionComponent } from 'react';
import { Link } from 'react-router-dom';
import style from './style.css';
import logo from '../../assets/logo.png';

const Header: FunctionComponent = () => (
    <header className={style.header}>
        <div className={style.logoContainer}>
            <img className={style.logo} src={logo} />
        </div>
        <h1>David Barone</h1>
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