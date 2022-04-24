import { CommentModel } from './CommentModel';

export type PostType = {
    id: number,
    title: string,
    slug: string,
    teaser: string,
    content: string,
    code: string,
    style: string,
    head: string,
    postType: string,
    parentId: number,
    createdDt: Date,
    createdBy: string,
    updatedDt: Date,
    updatedBy: string
    comments: CommentModel[]
}

    