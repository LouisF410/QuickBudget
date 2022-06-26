import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'side-nav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SideNavComponent implements OnInit {

  public menuItems: any[] = [
    {
      title: 'Transactions',
      path: '/transactions',
      icon: 'fa-file-invoice-dollar'
    },
    {
      title: 'Budget',
      path: '/budget',
      icon: ' fa-chart-pie'
    },
    {
      title: 'Accounts',
      path: '/accounts',
      icon: 'fa-building-columns'
    }
  ];

  constructor() { }

  ngOnInit() {
  }

}
