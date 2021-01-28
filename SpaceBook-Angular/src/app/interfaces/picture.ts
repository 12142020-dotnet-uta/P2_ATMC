import { MediaType } from "./media-type";

export class Picture {
    pictureID: number;
    title: string;
    mediaType: MediaType;
    imageURL: string;
    description: string;
    date: Date;
}
