
import { HttpClient, HttpClientModule  } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { User } from './_models/user';
import { AccountService } from './_services/account.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users:any;
  
  constructor( private http:HttpClient,private accountService:AccountService){

  }
  ngOnInit(): void {
  
   this.setcurrentuser()
  }
  setcurrentuser(){
    const user:User=JSON.parse(localStorage.getItem('user'));
    this.accountService.setcurrentuser(user);
  }
  
  
 getUsers(){
   this.http.get('https://localhost:5001/user').subscribe(response=>{
     this.users=response;

   },error=>{
     console.log(error)
   })
 }
}
