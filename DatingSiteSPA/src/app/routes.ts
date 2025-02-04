import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_gaurds/auth.guard';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberDetailResolver } from './_resolvers/member-details.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver.ts';
import { PreventUnsavedChanges } from './_gaurds/prevent-unsaved-changes.guard.ts';
import { HomeGuard } from './_gaurds/home.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full'},
  { path: 'home', component: HomeComponent, canActivate: [HomeGuard]},
  { path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MemberListComponent,
      resolve: {users: MemberListResolver}, canActivate: [AuthGuard]},
      {path: 'members/:id', component: MemberDetailsComponent, resolve: {users: MemberDetailResolver}},
      {path: 'member/edit', component: MemberEditComponent,
        resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
      {path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}},
      {path: 'lists', component: ListsComponent, resolve: {users: ListsResolver}}
    ]
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full'}
];
