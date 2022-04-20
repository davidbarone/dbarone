import React, { FC } from 'react';
import { Routes, Route, useParams } from 'react-router-dom';
import style from './App.css';
import Welcome from '../routes/WelcomeRoute';
import Posts from '../routes/PostsRoute';
import Post from '../routes/PostRoute';
import CounterRoute from '../routes/CounterRoute';
import Header from './HeaderComponent';
import { ToastContainer } from '../widgets/ToastWidget';

const App: FC = () => {
    return (
        <div id="app">
            <Header />
            <div className={style.container}>
                <Routes>
                    <Route path="posts" element={<Posts />} />
                    <Route path="post/:id" element={<PostRoute />} />
                    <Route path="counter" element={<CounterRoute />} />
                    <Route path="/" element={<Welcome />} />
                    <Route path="*" element={<Welcome />} />
                </Routes>
                <ToastContainer />                
            </div>
        </div >
    );
};

function PostRoute() {
    const { id } = useParams();
    return <Post id={id as unknown as number} />;
}

export default App;