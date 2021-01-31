// import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, Input, OnInit } from '@angular/core';
import { MessageService } from 'src/app/services/message.service';
import { Message } from 'src/app/interfaces/message';
import { User } from 'src/app/interfaces/user';

@Component({
  selector: 'app-direct-messaging',
  templateUrl: './direct-messaging.component.html',
  styleUrls: ['./direct-messaging.component.css']
})
export class DirectMessagingComponent implements OnInit {

  @Input() message: Message;
  users: User[];
  messages: Message[];


  // messagesArr = [];

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.messageService.getMessages()
    .subscribe(data => this.message = data);
  }


  getUsersInConversation(): void {
    this.messageService.getUsersInConversation()
    .subscribe(data => this.users = data);
  }

  getMessagesBetweenUser(userId: string): void {
    this.messageService.getMessagesBetweenUser(userId)
    .subscribe(data => this.messages = data);
  }

  postMessageToUser(userId: string, message: string): void {
    this.messageService.postMessageToUser(userId, message)
    .subscribe(data => this.message = data);
  }

}

