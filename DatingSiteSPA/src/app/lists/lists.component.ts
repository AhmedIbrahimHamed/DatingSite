import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from '../_models/Pagination';
import { User } from '../_models/user';
import { AlertifyService } from '../_services/Alertify.service';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam: string;

  constructor(private authService: AuthService, private userService: UserService,
              private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });

    this.likesParam = 'Likers';
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage.toString(),
      this.pagination.itemsPerPage.toString(),
      null, this.likesParam)
    .subscribe((response: PaginatedResult<User[]>) => {
      this.users = response.result;
      this.pagination = response.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }


}
