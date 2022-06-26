import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountsRoutes } from './accounts.routing';

@NgModule({
  declarations: [
    AccountsListComponent
  ],
  imports: [
    AccountsRoutes,
    CommonModule,
  ]
})
export class AccountsModule { }
