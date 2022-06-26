import { RouterModule } from '@angular/router';
import { SideNavComponent } from './sidenav/sidenav.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopnavComponent as TopNavComponent } from './topnav/topnav.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule
  ],
  declarations: [SideNavComponent, TopNavComponent],
  exports: [SideNavComponent, TopNavComponent]
})
export class LayoutModule { }
