import { Component, HostListener, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/Alertify.service';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
import { Router } from '@angular/router';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  providers: [{ provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: true } }]
})
export class NavComponent implements OnInit {
  isLoginCollapsed = false;
  isHomeCollapsed = true;
  photoUrl: string;

  @HostListener('window:resize', ['$event'])
  onResize(event) {
  const innerWidth = event.target.innerWidth;
  if (innerWidth > 768) {
    this.isLoginCollapsed = false;
    this.isHomeCollapsed = true;
  }
}

  constructor(public authService: AuthService,
              private alertify: AlertifyService, private router: Router) { }


  ngOnInit(): void {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  login(loginForm: NgForm) {
    // console.log(`${loginForm.value}`);
    this.authService.login(loginForm.value).subscribe(next => {
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  logout() {
    this.authService.logout();
  }

}
