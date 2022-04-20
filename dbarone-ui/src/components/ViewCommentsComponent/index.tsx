import React, { useState, FunctionComponent } from 'react';
import ButtonWidget from '../../widgets/ButtonWidget';
import { CommentModel } from '../../models/CommentModel';
import SliderWidget from '../../widgets/SliderWidget';
import EditCommentComponent from '../EditCommentComponent';

interface ViewCommentProps {
    postId: number,
    comments: CommentModel[];
}

const SingleComment = (comment: CommentModel) => {
    return (
        <table>
            <tr>
                <td style={{ minWidth: '200px', verticalAlign: 'top', hyphens: 'none' }}>
                    <div style={{ fontWeight: 600 }}>{comment.user} on {comment.createdDt.toString()}</div>
                </td>
                <td>
                    {comment.comment}
                </td>
            </tr>
        </table>
    );
};

const ViewCommentsComponent: FunctionComponent<ViewCommentProps> = ({ postId, comments }) => {
    const visibilityState = useState<boolean>(false);
    const [sliderVisibility, setSliderVisibility] = visibilityState;


    return (
        <>
            <h2>Comments</h2>
            {
                comments ? comments.map(c => SingleComment(c)) : ''
            }

            <ButtonWidget click={() => { setSliderVisibility(!sliderVisibility); }} label="New Comment"></ButtonWidget>

            {/* Slider for creating new comments */}
            <SliderWidget visibilityState={visibilityState}>
                <EditCommentComponent postId={postId}></EditCommentComponent>
            </SliderWidget>

        </>
    );
};

export default ViewCommentsComponent;