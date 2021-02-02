// import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, Input, OnInit } from '@angular/core';
import { MessageService } from 'src/app/services/message.service';
import { Message } from 'src/app/interfaces/message';
import { User } from 'src/app/interfaces/user';
import { Observable, of, timer } from 'rxjs';
import { UserProfileService } from 'src/app/services/user-profile.service';

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

  constructor(private messageService: MessageService, private userService: UserProfileService) { }

  ngOnInit(): void {
    this.messageService.getMessages()
    .subscribe(data => this.message = data);

      this.getLoggedIn();
      this.getUsersInConversation()
  }


  getUsersInConversation(): void {
    this.messageService.getUsersInConversation()
    .subscribe(data => this.users = data);
  }

  interval: any;

  startTimer() {
    this.interval = setInterval(() => {

    },1000)
  }

  pauseTimer() {
    clearInterval(this.interval);
  }

  getMessagesBetweenUser(userId: string): void {
    clearInterval(this.interval);
    this.interval = setInterval(() => {
      this.messageService.getMessagesBetweenUser(userId)
      .subscribe(data => this.messages = data);
    },1000)

    // this.messageService.getMessagesBetweenUser(userId)
    // .subscribe(data => this.messages = data);


    this.currentlyMessaging = userId;
  }

  currentlyMessaging: string;

  currentMessageText: string = "";

  postMessageToUser(userId: string, message: string): void {
    this.messageService.postMessageToUser(userId, message)
    .subscribe(data => this.message = data);

    this.currentMessageText = "";

  }

  loggedIn: User;

  getLoggedIn(): void{
    this.userService.getLoggedIn().subscribe(loggedIn => {this.loggedIn = loggedIn});
  }

  keyPress(event: KeyboardEvent){
    if(event.key == "Enter")
    {
      this.postMessageToUser(this.currentlyMessaging, this.currentMessageText)
      // this.currentMessageText = "";
      event.preventDefault();
    }
  }



}

