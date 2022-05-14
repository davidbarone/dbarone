import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet, httpDelete } from '../../utils/ApiFacade';
import TableWidget from '../../widgets/TableWidget';
import ButtonWidget from '../../widgets/ButtonWidget';
import SliderWidget from '../../widgets/SliderWidget';
import EditPostComponent from '../../components/EditPostComponent';
import { LinkType } from '../../types/LinkType';
import { PostSummaryModel } from '../../models/PostSummaryModel';
import style from './style.css';

interface ListPostsProps {
    allowEdit: boolean;
}

/**
 * Lists out posts - supports read-only list and tabular editable list.
 * @param param0 
 * @returns 
 */
const ListPostsComponent: FunctionComponent<ListPostsProps> = ({ allowEdit = false }) => {
    const [posts, setPosts] = useState<Array<PostSummaryModel>>([]);
    const visibilityState = useState<boolean>(false);
    const [sliderVisibility, setSliderVisibility] = visibilityState;
    const [links, setLinks] = useState<Array<LinkType>>([]);
    const [api, setApi] = useState<string>('/posts');

    const getPosts = () => {
        httpGet(api, 'Loaded posts successfully.').then((result) => {
            setPosts(result.envelope.data);
            setLinks(result.envelope.links);
        });
    };

    const deletePost = (id: number) => {
        httpDelete(`/posts/${id}`, `Deleted post ${id} successfully.`).then(r => getPosts());
    };

    useEffect(() => {
        getPosts();
    }, [api]);

    return (
        <>
            {!allowEdit && posts.map((p, i) => 
                <>
                    <div key={i} className={style.post}>
                        <h2>
                            <a href={'/post/' + p.id}>{p.title}</a>
                        </h2>
                        <div className={style.by}>
                            {`By ${p.createdBy} on ${p.createdDt}`}
                        </div>
                        <div className={style.teaser} dangerouslySetInnerHTML={{ __html: p.teaser }}>
                        </div>
                    </div>
                </>
            )}

            {allowEdit &&
                <>
                    <ButtonWidget click={() => { setSliderVisibility(!sliderVisibility); }} label="New Post"></ButtonWidget>
                    <TableWidget<PostSummaryModel>
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
                            'Created Dt': (row) => (<>{row.createdDt}</>),
                            'Created By': (row) => (<>{row.createdBy}</>),
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
                </>
            }

            {/* Pagination */}
            {links.map((l, i) => (
                <ButtonWidget key={i} label={l.rel} click={() => setApi(l.uri)}></ButtonWidget>
            ))}
             
        </>
    );
};

export default ListPostsComponent;