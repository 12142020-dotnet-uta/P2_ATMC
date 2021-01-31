import { User } from "./user";

export interface Message {
    messageID: number;
    text: string;
    date:Date;
    senderID: string;
    recipientId: string;
    parentMessageID?: number;
    sender: User;
    recipient: User;
}
