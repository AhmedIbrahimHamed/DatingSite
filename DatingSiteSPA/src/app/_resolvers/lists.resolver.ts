import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ListsResolver implements Resolve<User[]> {
  pageNumber = '1';
  pageSize = '6';
  likesParam = 'Likers';

  constructor(private userService: UserService, private router: Router,
              private alertify: AlertifyService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    // This automatically subscribe to the .getUser() method so we don't need to
    // subscribe this ourselves.
    return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParam)
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
