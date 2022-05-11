import React, { useState, useEffect, FunctionComponent, useRef } from 'react';
import { TokenModel } from '../../models/TokenModel';
import { httpPost } from '../../utils/ApiFacade';
import ButtonWidget from '../../widgets/ButtonWidget';
import { setSessionStorage, getSessionStorage } from '../../utils/Utilities';

const ProfileRoute: FunctionComponent = () => {
    const user = getSessionStorage<TokenModel>('user');

    const handleLogout = (e: React.FormEvent<HTMLButtonElement>) => {
        // Use the refresh token stored in cookie, so no need to pass explicit refresh token here.
        httpPost('/users/revoke-token', { token: null }, 'Logged out successfully.').then((r) => {
        // Clear the session user too
            setSessionStorage<TokenModel>('user', null);

            // redirect to home page
            window.location.href = '/';
        });
        e.preventDefault();
    };

    return (
        <>
            <div>
                <h1>Profile</h1>
                <label>
                    User:
                </label>
                <div>{user?.firstName} {user?.lastName}</div>

                <ButtonWidget label='Logout' click={handleLogout}></ButtonWidget>
            </div>
        </>
    );
};

export default ProfileRoute;