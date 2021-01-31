import { User } from "./user";

export class PictureComment {
    commentID: number;
    commentText:string;
    date: Date;
    userCommented:User
    userCommentedId:string;
    pictureCommentedId:number;
    parentCommentId?:number;
}
