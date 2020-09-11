import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/Alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() likesParam = 'Likers';
  @Output() likeDeleted = new EventEmitter<void>();
  @Input() user: User;
  @Input() unlikeMode = false;

  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService) { }

  ngOnInit() {
  }

  sendLike(id: number) {
    this.userService.sendLike(this.authService.decodedToken.nameid, id)
      .subscribe(data => {
        this.alertify.success('You have liked: ' + this.user.knownAs);
      }, error => {
        this.alertify.error(error);
      });
  }

  deleteLike(id: number) {
    this.userService.deletelike(this.authService.decodedToken.nameid, id)
    .subscribe(data => {
      this.alertify.success('You have Unliked: ' + this.user.knownAs);
      this.likeDeleted.emit();
    }, error => {
      this.alertify.error(error);
    });
  }

}
