import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import { createRoot } from 'react-dom/client';
import 'normalize.css';
import './style/index.css';
import App from './components/App';

const container = document.getElementById('root') as Element;
const root = createRoot(container);

root.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>
);
