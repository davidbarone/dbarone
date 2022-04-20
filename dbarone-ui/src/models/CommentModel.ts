export type CommentModel = {
    id?: number,
    createdDt: Date,
    user: string,
    comment: string,
    postId: number,
}