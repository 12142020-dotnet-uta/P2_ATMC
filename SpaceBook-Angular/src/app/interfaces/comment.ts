export interface Comment {
    CommentID: number;
    CommentText:string;
    Date: Date;
    UserCommentedID:number;
    PictureCommentedID:number;
    ParentCommentID?:number;
}
