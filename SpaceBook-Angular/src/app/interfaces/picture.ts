import { MediaType } from "./media-type";

export interface Picture {
    PictureID: number;
    Title: string;
    MediaType: MediaType;
    ImageURL: string;
    Description: string;
    Date: Date;
}
