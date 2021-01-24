import { User } from "./user";

export interface UserLogIn extends User {
    UserName:string;
    Password:string;
}
