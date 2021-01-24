import { Component, OnInit } from '@angular/core';
import { Follow } from "../../interfaces/follow";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor() { }

  varFollow: Follow;

  ngOnInit(): void {

    

  }

}
