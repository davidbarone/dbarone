import React, { FunctionComponent } from 'react';
import { Link } from 'react-router-dom';
import style from './style.css';

const FooterComponent: FunctionComponent = () => (
    <footer className={style.footer}>
        <div>David Barone: https://www.dbarone.com</div>
    </footer>
);

export default FooterComponent;