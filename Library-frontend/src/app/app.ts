import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { Router } from '@angular/router';
import { Auth } from './services/auth'
import { Borrow } from './services/borrow';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { Navbar } from './components/navbar/navbar';
import { NavbarBehavior } from './components/navbar-behavior/navbar-behavior';

@Component({
  selector:'app-root',
  imports:[RouterLink, RouterOutlet, AsyncPipe, Navbar, NavbarBehavior],
  templateUrl:'./app.html',
  styleUrl:'./app.css'

})

export class App {

  borrowedCount$!: Observable<number>;

  constructor(
    private auth: Auth,
    private router: Router,
    private borrowService: Borrow
  ) 
  {
    this.borrowedCount$ = this.borrowService.borrowedCount$;
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/books']);
  }

  isLoggedIn(): boolean {
    return this.auth.isLoggedIn();
  }

  ngOnInit() {
    if (this.auth.isLoggedIn()) {
      this.borrowService.loadBorrowedBooks();
    }
  }
}