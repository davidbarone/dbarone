import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet } from '../../utils/ApiFacade';
import { PostType } from '../../models/PostModel';
import ButtonWidget from '../../widgets/ButtonWidget';
import SliderWidget from '../../widgets/SliderWidget';
import EditPostComponent from '../../components/EditPostComponent';
import ViewCommentsComponent from '../../components/ViewCommentsComponent';
import { PostRelationsModel } from '../../models/PostRelationsModel';

import style from './style.css';

interface PostProps {
    id: number;
}

const PostRoute: FunctionComponent<PostProps> = ({ id }) => {
    const [post, setPost] = useState<PostType>({} as PostType);
    const [relations, setRelations] = useState<PostRelationsModel>({} as PostRelationsModel);
    const visibilityState = useState<boolean>(false);
    const [sliderVisibility, setSliderVisibility] = visibilityState;

    const init = () => {
        httpGet(`/posts/${id}`, `Loaded post ${id} successfully.`).then((result) => setPost(result.body.data));
        httpGet(`/posts/${id}/related`, `Loaded post ${id} relations successfully.`).then((result) => setRelations(result.body.data));
    };

    useEffect(() => {
        init();
    }, [id]);

    const postDiv = (post: PostType): any => (
        <div className={style.postContainer}>
            <h1>{post.title}</h1>
            <div className={style.postedBy}>
                <p>By {post.updatedBy} on {post.updatedDt}</p>
                <div>
                    <button className="button" onClick={() => { alert('d'); }}>
                        Edit
                    </button>
                </div>
            </div>
            <div
                style={{ marginTop: '6px' }}
                dangerouslySetInnerHTML={{ __html: post.content }}
            ></div>
            <ButtonWidget click={() => { setSliderVisibility(!sliderVisibility); }} label="Edit Post"></ButtonWidget>
            <ButtonWidget href='/posts' label="Posts"></ButtonWidget>
            <ViewCommentsComponent postId={id} comments={post.comments}></ViewCommentsComponent>
            {/* Slider for creating new posts */}
            <SliderWidget visibilityState={visibilityState} onClose={init}>
                <EditPostComponent id={post.id}></EditPostComponent>
            </SliderWidget>

            {/* script inserted here */}
        </div>
    );

    const relationsDiv = () => {
        return (
            relations && relations.hasRelations ? (
                <div className={style.navbar}>
                    {relations.parent && (
                        <>
                            <h2>Parent</h2>
                            <a href={`/post/${relations.parent.id}`}>
                                {relations.parent.title}
                            </a>
                        </>

                    )}

                    {Array.isArray(relations.siblings) && !!relations.siblings.length && (
                        <>
                            <h2>Siblings</h2>
                            {relations.siblings.map((value, index) => {
                                return (
                                    <a key={index} href={`/post/${value.id}`}>
                                        {value.title}
                                    </a>
                                );
                            })}
                        </>
                    )}

                    {Array.isArray(relations.children) && !!relations.children.length && (
                        <>
                            <h2>Children</h2>
                            {relations.children.map((value, index) => {
                                return (
                                    <a key={index} href={`/post/${value.id}`}>
                                        {value.title}
                                    </a>
                                );
                            })}
                        </>
                    )}
                </div>
            ) : <></>);
    };


    return (
        <>
            <div className={style.wrapper}>
                {relationsDiv()}
                {postDiv(post)}
            </div>
        </>
    );
};

export default PostRoute;