import { CommentModel } from './CommentModel';

export type PostType = {
    id: number,
    title: string,
    teaser: string,
    content: string,
    comments: CommentModel[]
}