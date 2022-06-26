import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full'
  },
  {
    path: 'list',
    component: AccountsListComponent
  }
];

export const AccountsRoutes = RouterModule.forChild(routes);
