import React, { useState, useEffect, FunctionComponent, useRef } from 'react';
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

const loadScriptSync = async (script:HTMLScriptElement) => {
    return new Promise((resolve, reject) => {
        script.onload = function() {
            resolve({ script });
        };
        document.head.appendChild(script);
    });
};

const loadCode = async (post:PostType) => {
    
    // Add inline style if set
    if (post.style) {
        const style = document.createElement('style');
        style.innerText = post.style;
        document.head.appendChild(style);
    }

    // Add head if any
    // All head contents must be nodes with src attributes.
    if (post.head) {
        // Have to convert head string to node
        const div = document.createElement('div');
        div.innerHTML = post.head;
        for (let i = 0; i < div.childNodes.length; i++) {
            const node = div.childNodes[i];
            const child = node as HTMLScriptElement;
            if (node && node.nodeName === 'SCRIPT') {
                if (child.src !== '') {
                    const s = document.createElement('script');
                    s.type = 'text/javascript';
                    s.src = child.src;
                    s.async = false;
                    //document.head.appendChild(s);
                    await loadScriptSync(s);
                }
            } else {
                eval(child.innerHTML);
            }
        }
        // Change this to div.childNodes to support multiple top-level nodes
        div.childNodes.forEach(child => { const a = 1; });
    }
 
    // Add code if any
    // Wrap this in inline <script> block.
    if (post.code) {
        const script = document.createElement('script');
        script.type = 'text/javascript';
        script.async = false;
        try {
            // most browsers
            script.appendChild(document.createTextNode(post.code));
            document.head.appendChild(script);
        } catch (e) {
            // option (b) for other browsers
            alert('error');
            script.text = post.code;
            document.head.appendChild(script);
        }
    }
};
        
const PostRoute: FunctionComponent<PostProps> = ({ id }) => {
    const [post, setPost] = useState<PostType>({} as PostType);
    const [relations, setRelations] = useState<PostRelationsModel>({} as PostRelationsModel);
    const visibilityState = useState<boolean>(false);
    const [sliderVisibility, setSliderVisibility] = visibilityState;
    
    const init = () => {
        httpGet(`/posts/${id}`, `Loaded post ${id} successfully in viewer.`)
            .then((result) => {
                setPost(result.envelope.data);
            });
        httpGet(`/posts/${id}/related`, `Loaded post ${id} relations successfully.`).then((result) => setRelations(result.envelope.data));
        /* Head, style and code */
    };

    useEffect(() => {
        loadCode(post);
    }, [post]);

    useEffect(() => {
        init();
    }, [id]);

    const postDiv = (post: PostType): any => (
        <div className={style.postContainer}>
            <h1>{post.title}</h1>
            <div className={style.postedBy}>
                By {post.updatedBy} on {post.updatedDt}
            </div>
            <ButtonWidget click={() => { setSliderVisibility(!sliderVisibility); }} label="Edit Post"></ButtonWidget>
            <ButtonWidget href='/posts' label="Posts"></ButtonWidget>
            <div
                style={{ marginTop: '6px' }}
                dangerouslySetInnerHTML={{ __html: post.content }}
            ></div>
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
            relations.hasRelations ? (
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