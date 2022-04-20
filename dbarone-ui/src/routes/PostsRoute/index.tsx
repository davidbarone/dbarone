import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet, httpDelete } from '../../utils/ApiFacade';
import TableWidget from '../../widgets/TableWidget';
import ButtonWidget from '../../widgets/ButtonWidget';
import SliderWidget from '../../widgets/SliderWidget';
import EditPostComponent from '../../components/EditPostComponent';

type PostType = {
    id: number,
    title: string,
    teaser: string
}

const PostsRoute: FunctionComponent = () => {
    const [posts, setPosts] = useState<Array<PostType>>([]);
    const visibilityState = useState<boolean>(false);
    const [sliderVisibility, setSliderVisibility] = visibilityState;

    const getPosts = () => {
        httpGet('/posts', 'Loaded posts successfully.').then((result) => setPosts(result.body));
    };

    const deletePost = (id: number) => {
        httpDelete(`/posts/${id}`, `Deleted post ${id} successfully.`).then(r => getPosts());
    };

    useEffect(() => {
        getPosts();
    }, []);

    return (
        <>
            <h1>Posts</h1>
            <TableWidget<PostType>
                data={posts}
                visible={true}
                mapping={{
                    'Id': (row) => (<>{row.id}</>),
                    'Title': (row) => (<>{row.title}</>),
                    'Teaser': (row) => (<>{row.teaser}</>),
                    'Edit': (row) => (
                        <>
                            <ButtonWidget
                                title="View post"
                                label="View"
                                href={`/post/${row.id}`}
                                visible={true}
                            />
                        </>
                    ),
                    'Delete': (row) => (
                        <>
                            <ButtonWidget
                                title="Delete post"
                                label="Delete"
                                visible={true}
                                click={(e) => {
                                    deletePost(row.id);
                                    e.preventDefault();
                                }} />
                        </>
                    )

                }}
            />

            {/* Slider for creating new posts */}
            <SliderWidget visibilityState={visibilityState} onClose={getPosts}>
                <EditPostComponent id={undefined}></EditPostComponent>
            </SliderWidget>

            <ButtonWidget click={() => { setSliderVisibility(!sliderVisibility); }} label="New Post"></ButtonWidget>
        </>
    );
};

export default PostsRoute;