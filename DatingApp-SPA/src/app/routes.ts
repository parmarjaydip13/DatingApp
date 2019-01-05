import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessageComponent } from './message/message.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
  {
    path: '',
    component: HomeComponent
  },

  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'members',
        component: MemberListComponent
      },
      {
        path: 'message',
        component: MessageComponent
      },
      {
        path: 'lists',
        component: ListsComponent
      }
    ]
  },

  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];
