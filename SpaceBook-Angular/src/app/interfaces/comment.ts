export interface Comment {
    CommentID: number;
    CommentText:string;
    Date: Date;
    UserCommentedID:string;
    PictureCommentedID:number;
    ParentCommentID?:number;
}
