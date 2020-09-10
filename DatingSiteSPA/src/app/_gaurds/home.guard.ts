import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/Alertify.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeGuard implements CanActivate {

  constructor(private authService: AuthService,
              private router: Router,
              private alertify: AlertifyService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.authService.loggedIn()) {
      this.router.navigate(['/members']);
      return false;
    }

    return true;
  }

}
