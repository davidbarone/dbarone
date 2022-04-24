import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet, httpPost, httpPut } from '../../utils/ApiFacade';
import { PostType } from '../../models/PostModel';
import InputWidget from '../../widgets/InputWidget';
import { formToJson } from '../../utils/Utilities';
import DropdownWidget from '../../widgets/DropdownWidget';

interface PostProps {
    id?: number;
}

const EditPost: FunctionComponent<PostProps> = ({ id = null }) => {
    const postState = useState<PostType>({} as PostType);
    const [post, setPost] = postState;

    useEffect(() => {
        if (id) {
            httpGet(`/posts/${id}`, `Loaded post ${id} successfully in editor.`).then((result) => setPost(result.body.data));
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
                label='Id'
                name='id'
                state={ postState }
                type='hidden' />

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

            <DropdownWidget
                label='Post Type'
                name='postType'
                size={1}
                multiple={false}
                disabled={false}
                selectedValue = {post.postType}
                values={['MARKDOWN', 'HTML']}
                state={ postState }
            />

            <InputWidget
                label='Head (Note: used for script links to external files and other CSS links).'
                name='head'
                type='text'
                state={ postState }
                rows={3} />

            <InputWidget
                label='Code (Note: inline JavaScript - no not enter the script tag. Just the JavaScript code).'
                name='code'
                type='text'
                state={ postState }
                rows={10} />

            <InputWidget
                label='Style (Note: custom style selectors entered here).'
                name='style'
                type='text'
                state={ postState }
                rows={5} />

            <InputWidget
                type='submit'
            />
        </form>
    );
};

export default EditPost;