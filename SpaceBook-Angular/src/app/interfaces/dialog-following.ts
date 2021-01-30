import { User } from "./user";

export interface DialogFollowing {
    user: User;
    following: User[];
    followers: User[];
}