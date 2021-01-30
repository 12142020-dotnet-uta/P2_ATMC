import { MediaType } from "./media-type";

export class Picture {
    pictureID: number;
    title: string;
    mediaType: MediaType;
    imageURL: string;
    imageHDURL:string;
    description: string;
    date: Date;
    isUserPicture:boolean
}
