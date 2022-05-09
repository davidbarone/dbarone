import React, { useState, useEffect, FunctionComponent, useRef } from 'react';
import InputWidget from '../../widgets/InputWidget';
import { LoginModel } from '../../models/LoginModel';
import { httpPost } from '../../utils/ApiFacade';
import { formToJson } from '../../utils/Utilities';
import style from './style.css';

const LoginRoute: FunctionComponent = () => {
    const state = useState<LoginModel>({Username: '', Password: ''});

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        const json = formToJson(e.currentTarget);
        httpPost('/users/authenticate', json, 'Authenticated successfully.').then((response) => {
            sessionStorage.setItem('user', JSON.stringify(response.envelope.data));
        });
        e.preventDefault();
    };
    
    return (
        <>
            <div className={style.centredContainer}>
                <h1>Login</h1>
                <form onSubmit={handleSubmit}>            
                    <InputWidget type='input' state={state} name='username' label='Username'></InputWidget>
                    <InputWidget type='password' state={state} name='password' label='Password'></InputWidget>
                    <InputWidget type='submit'></InputWidget>
                </form>
            </div>
        </>
    );
};

export default LoginRoute;