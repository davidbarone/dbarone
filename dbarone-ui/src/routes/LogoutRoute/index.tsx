import React, { useState, useEffect, FunctionComponent, useRef } from 'react';
import { TokenModel } from '../../models/TokenModel';
import { httpPost } from '../../utils/ApiFacade';
import ButtonWidget from '../../widgets/ButtonWidget';

const LogoutRoute: FunctionComponent = () => {
    const handleLogout = (e: React.FormEvent<HTMLButtonElement>) => {
        const tokenStr = sessionStorage.getItem('user');
        let user: TokenModel | null = null;
        if (tokenStr) {
            user = JSON.parse(tokenStr);
        }

        // Use the refresh token sent in cookie, so don't need to pass explicit refresh token here.
        httpPost('/users/revoke-token', {token: null }, 'Logged out successfully.');
        e.preventDefault();
    };

    return (
        <>
            <div>
                <h1>Logout</h1>

                <ButtonWidget label='Logout' click={handleLogout}></ButtonWidget>
            </div>
        </>
    );
};

export default LogoutRoute;