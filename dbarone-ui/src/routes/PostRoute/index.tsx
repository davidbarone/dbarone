import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet } from '../../utils/ApiFacade';
import { PostType } from '../../models/PostModel';
import ButtonWidget from '../../widgets/ButtonWidget';
import SliderWidget from '../../widgets/SliderWidget';
import EditPostComponent from '../../components/EditPostComponent';
import ViewCommentsComponent from '../../components/ViewCommentsComponent';

interface PostProps {
    id: number;
}

const PostRoute: FunctionComponent<PostProps> = ({ id }) => {
    const [post, setPost] = useState<PostType>({} as PostType);
    const visibilityState = useState<boolean>(false);
    const [sliderVisibility, setSliderVisibility] = visibilityState;

    const init = () => {
        httpGet(`/posts/${id}?_embed=comments`, `Loaded post ${id} successfully.`).then((result) => setPost(result.body));
    };

    useEffect(() => {
        init();
    }, [id]);

    return (
        <>
            <h1>{post.title}</h1>
            <p>{post.content}</p>

            <ButtonWidget click={() => { setSliderVisibility(!sliderVisibility); }} label="Edit Post"></ButtonWidget>
            <ButtonWidget href='/posts' label="Posts"></ButtonWidget>

            <ViewCommentsComponent postId={id} comments={post.comments}></ViewCommentsComponent>

            {/* Slider for creating new posts */}
            <SliderWidget visibilityState={visibilityState} onClose={init}>
                <EditPostComponent id={post.id}></EditPostComponent>
            </SliderWidget>
        </>
    );
};

export default PostRoute;