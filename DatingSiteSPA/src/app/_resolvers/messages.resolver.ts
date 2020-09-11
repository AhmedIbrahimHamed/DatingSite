import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';
import { AuthService } from '../_services/auth.service';

// This is a resolver that will return a single type of User
// This is consumed by member-detail.component.ts as defined in the route.ts

@Injectable({
  providedIn: 'root'
})
export class MessagesResolver implements Resolve<Message[]> {
  pageNumber = '1';
  pageSize = '6';
  messageContainer = 'Unread';

  constructor(private userService: UserService, private router: Router,
              private alertify: AlertifyService, private authService: AuthService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    // This automatically subscribe to the .getUser() method so we don't need to
    // subscribe this ourselves.
    return this.userService.getMessages(this.authService.decodedToken.nameid,
      this.pageNumber, this.pageSize, this.messageContainer)
      // We want to catch any errors that occur so that we can so that we can redirectly
      // use a back and also get back of the method as well
      .pipe(
        catchError(error => {
          this.alertify.error('Problem retrieving data');
          // We will navigate them back to the /members page
          this.router.navigate(['/members/edit']);
          // We will return an observable of type null by using the of() method from 'rxjs'
          return of(null);
        })
      );
  }
}
