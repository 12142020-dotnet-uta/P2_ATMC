export interface Message {
    MessageID: number;
    Text: string;
    Date:Date;
    SenderID: string;
    RecipientId: string;
    ParentMessageID?: number;
}
