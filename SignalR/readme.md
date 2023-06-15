# Angular Code to access SignalR Hub

Before copying the code remember to install SignalR Package first!  
<span style="color: cyan;">npm install @microsoft/signalr – save</span>

## Create a SignalR Service
<span style="color: cyan;">ng g s SignalRService</span>

```
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  //CHANGE THIS ENDPOINT AND PORT TO YOUR CHATGPTAPI ADDRESS AND PORT
  private apiUrl = 'http://localhost:5182';
  public chatgptHubConn!: signalR.HubConnection;

  constructor(
  ) {
    this.chatgptHubConn = new signalR.HubConnectionBuilder()
      .withUrl(this.apiUrl + "/ChatGPTHub")
      .build();    
  }
}
```

## SignalR Angular Component
<span style="color: cyan;">ng g c ChatGptComponent</span>
```
import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../services/signalr.service';
import { Subject, map, scan, tap } from 'rxjs';

@Component({
  selector: 'app-chat-gpt',
  templateUrl: './chat-gpt.component.html',
  styleUrls: ['./chat-gpt.component.css']
})
export class ChatGptComponent implements OnInit {

  subject = new Subject<string>();
  data$ = this.subject.pipe(
    scan((acc, value) => {
      if (value === "clear_response") return '';
      return acc + value
    }, '')
  );
  prompt: string = "";
  constructor(
    private signalR: SignalRService
  ) { }

  ngOnInit(): void {
    this.signalR.chatgptHubConn.on("ReceiveData", (data: any) => {
      let content = data.choices[0]?.message?.content;
      if (typeof (content) != undefined) { this.subject.next(content); }
    });

    this.signalR.chatgptHubConn.on("ReceiveStreamedData", (data: any) => {
      let obj = JSON.parse(data);
      let content = obj?.choices[0]?.delta?.content;
      if (typeof (content) != undefined) { this.subject.next(content); }
    });
  }

  ngAfterViewInit(): void {
    this.signalR.chatgptHubConn.start().then(() => {
      console.log("SignalR connection made");
    }).catch(error => console.log(error));
  }
 
  SubmitHttpReceiveStream() {
    this.subject.next("clear_response");
    let message = {
      messages: [
        {
          role: "user",
          content: this.prompt
        }
      ],
      model: "gpt-3.5-turbo",
      max_tokens: 1000
    };
    if (this.prompt === null || this.prompt.length === 0) {
      this.subject.next("Aren't you gonna ask something?");
    } else
      this.signalR.chatgptHubConn.send("SendStreamedMessage", message);
  }

  SubmitSignalR() {
    this.subject.next("clear_response");
    if (this.prompt === null || this.prompt.length === 0) {
      this.subject.next("Aren't you gonna ask something?");

    } else
      this.signalR.chatgptHubConn.send("SendMessage", this.prompt, "gpt-3.5-turbo", 1000);
  }
}
```

## Html of your SignalR Component

```
<h1>ChatGPT Prompt</h1>
<div class="form-group row">
    <label for="inputText" class="col-sm-2 col-form-label">Prompt</label>
    <div class="col-sm-10">
        <input type="text" class="form-control" id="inputText" [(ngModel)]="prompt">
    </div>        
  </div>  
  <button type="button" (click)="SubmitSignalR()" class="btn btn-primary">SignalR</button>
  <button type="button" (click)="SubmitHttpReceiveStream()" class="btn btn-secondary">SignalR Stream</button>  
  <br/><br/>
  <textarea class="form-control" [value]="data$ | async"></textarea>
```