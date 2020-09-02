import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent  {

  login(loginForm: NgForm) {
    console.log(`username : ${loginForm.value.username} , password : ${loginForm.value.password}`);
  }

}
