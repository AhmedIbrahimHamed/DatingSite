import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent  {

  constructor(private authService: AuthService) { }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  login(loginForm: NgForm) {
    // console.log(`${loginForm.value}`);
    this.authService.login(loginForm.value).subscribe(next => {
      console.log('Logged in successfully');
    }, error => {
      console.log('Failed to login');
    });
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out');
  }

}
