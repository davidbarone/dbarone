import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet, httpPost, httpPut } from '../../utils/ApiFacade';
import { PostType } from '../../models/PostModel';
import InputWidget from '../../widgets/InputWidget';
import { formToJson } from '../../utils/Utilities';

interface PostProps {
    id?: number;
}

const EditPost: FunctionComponent<PostProps> = ({ id = null }) => {
    const postState = useState<PostType>({} as PostType);
    const [post, setPost] = postState;

    useEffect(() => {
        if (id) {
            httpGet(`/posts/${id}`, `Loaded post ${id} successfully.`).then((result) => setPost(result.body));
        }
    }, [id]);

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        const json = formToJson(e.currentTarget);
        if (id) {
            // update
            httpPut(`/posts/${id}`, json, `Post ${id} updated successfully.`);
        } else {
            httpPost('/posts/', json, 'Post created successfully.');
        }
        e.preventDefault();
    };


    return (
        <form onSubmit={handleSubmit}>
            <h1>Add / Edit Post</h1>
            <InputWidget
                label='Title'
                name='title'
                state={ postState }
                type='text' />

            <InputWidget
                label='Teaser (Note: this section does NOT support HTML)'
                name='teaser'
                type='text'
                state={ postState }
                rows={5} />

            <InputWidget
                label='Content (Note: no executable code here. Put scripts in head or code sections).'
                name='content'
                type='text'
                state={ postState }
                rows={10} />

            <InputWidget
                label='Content (Note: no executable code here. Put scripts in head or code sections).'
                name='content'
                type='text'
                state={ postState }
                rows={10} />
            
            <InputWidget
                type='submit'
            />
        </form>
    );
};

export default EditPost;