import { Component } from '@angular/core';
import { Books } from '../../services/books';
import { Book } from '../../models/book';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { Router } from '@angular/router';
import { Borrow } from '../../services/borrow';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-books-list',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './books-list.html',
  styleUrl: './books-list.css',
})
export class BooksList {

  books$!: Observable<Book[]>;
  book$!: Observable<Book>;

  constructor(
    private booksService: Books,
    private borrowService: Borrow,
    private authService: Auth,
    private router: Router
  )
  {
    //incarca global cartile
    this.booksService.loadBooks();

    this.borrowService.loadBorrowedBooks();

    //subscribe la starea shared
    this.books$ = this.booksService.books$;
  }

  viewDetails(bookId: number){
    this.router.navigate(['/books', bookId]);
  }

  borrowBook(bookId: number, bookTitle: string) {

    if (!this.authService.isLoggedIn()) {
      alert('You must be logged in to borrow a book');
      this.router.navigate(['/login']);
      return;
    }

    this.borrowService.borrowBook({ bookId })
      .subscribe({
        next: () => {

          //refresh global state
          this.booksService.loadBooks();

          this.borrowService.loadBorrowedBooks();

          console.log(`${bookTitle} borrowed`);
        },
        error: error => {
          console.log(error);
        }
      });
  }

}
