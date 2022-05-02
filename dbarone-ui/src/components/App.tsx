import React, { FC, useState, useEffect } from 'react';
import { Routes, Route, useParams } from 'react-router-dom';
import style from './App.css';
import Welcome from '../routes/WelcomeRoute';
import Posts from '../routes/PostsRoute';
import Post from '../routes/PostRoute';
import CounterRoute from '../routes/CounterRoute';
import Header from './HeaderComponent';
import { ToastContainer } from '../widgets/ToastWidget';
import FooterComponent from './FooterComponent';
import { PostType } from '../models/PostModel';
import { httpGet } from '../utils/ApiFacade';
import LoginRoute from '../routes/LoginRoute';

const App: FC = () => {
    return (
        <div id="app" className={style.app}>
            <Header />
            <div className={style.main}>
                <Routes>
                    <Route path="posts" element={<Posts />} />
                    <Route path="post/:id" element={<PostRouteById />} />
                    <Route path="slug/:slug" element={<PostRouteBySlug />} />
                    <Route path="counter" element={<CounterRoute />} />
                    <Route path="login" element={<LoginRoute />} />
                    <Route path="*" element={<Welcome />} />
                    <Route path="/" element={<Welcome />} />
                </Routes>
                <ToastContainer />
            </div>
            <FooterComponent />
        </div >
    );
};

function PostRouteById() {
    const { id } = useParams();
    return <Post id={id as unknown as number} />;
}

const PostRouteBySlug = () => {
    const { slug } = useParams();
    const [post, setPost] = useState<PostType>({} as PostType);

    const init = () => {
        httpGet(`/${slug}`, `Loaded post from slug: '${slug}' successfully.`)
            .then((result) => {
                setPost(result.envelope.data);
            });
    };

    useEffect(() => {
        init();
    }, [slug]);

    return (post && post.id) ? (<Post id={post.id} />) : (<></>);
};

export default App;