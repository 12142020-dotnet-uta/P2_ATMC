export interface Message {
    MessageID: number;
    Text: string;
    Date:Date;
    SenderID: number;
    RecipientId: number;
    ParentMessageID?: number;
}
