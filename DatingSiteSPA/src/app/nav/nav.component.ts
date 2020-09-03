import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/Alertify.service';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  providers: [{ provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: true } }]
})
export class NavComponent  {
  isCollapsed = false;

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  loggedIn() {
    return this.authService.loggedIn();
  }

  login(loginForm: NgForm) {
    // console.log(`${loginForm.value}`);
    this.authService.login(loginForm.value).subscribe(next => {
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error(error);
    });
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('Logged out');
  }

}
