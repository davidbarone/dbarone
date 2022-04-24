import { PostSummaryModel } from './PostSummaryModel';

export type PostRelationsModel = {
    current: PostSummaryModel;
    parent: PostSummaryModel;
    siblings: Array<PostSummaryModel>;
    children: Array<PostSummaryModel>;
    hasRelations: boolean;
}

    