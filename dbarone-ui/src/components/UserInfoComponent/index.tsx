import React, { useState, useEffect, FunctionComponent, ReactEventHandler } from 'react';
import style from './style.css';
import { TokenModel } from '../../models/TokenModel';
import { getSessionStorage, setSessionStorage } from '../../utils/Utilities';
import { Link } from 'react-router-dom';

/**
 * Displays login / profile link in header
 * @returns 
 */
const UserInfoComponent: FunctionComponent = () => {
    const [user, setUser] = useState<TokenModel | null>(getSessionStorage<TokenModel>('user'));

    const sessionStorageUpdated = (e: CustomEvent) => {
        setUser(getSessionStorage<TokenModel>('user'));
    };

    document.addEventListener(
        'sessionStorageUpdated',
        sessionStorageUpdated
    );

    return (
        <>
            {(user && (
                <Link to="/profile">
                    Profile
                </Link>

            ))}

            {(!user && (
                <Link to="/login">
                    Login
                </Link>

            ))}
        </>
    );
};

export default UserInfoComponent;