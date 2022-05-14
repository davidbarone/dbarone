import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet, httpDelete } from '../../utils/ApiFacade';
import TableWidget from '../../widgets/TableWidget';
import ButtonWidget from '../../widgets/ButtonWidget';
import SliderWidget from '../../widgets/SliderWidget';
import EditPostComponent from '../../components/EditPostComponent';
import { LinkType } from '../../types/LinkType';
import ListPostsComponent from '../../components/ListPostsComponent';

const PostsRoute: FunctionComponent = () => {
    return (
        <ListPostsComponent allowEdit={false}></ListPostsComponent>
    );
};

export default PostsRoute;