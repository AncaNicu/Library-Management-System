import { Component } from '@angular/core';
import { Borrow } from '../../services/borrow';
import { Observable } from 'rxjs';
import { BorrowedBook } from '../../models/borrowed-book';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-borrowed-books',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './borrowed-books.html',
  styleUrl: './borrowed-books.css',
})
export class BorrowedBooks {

  borrowedBooks$!: Observable<BorrowedBook[]>;
  constructor(
    private borrowService: Borrow
  )
  {
    this.borrowService.loadBorrowedBooks();
    this.borrowedBooks$ = this.borrowService.borrowedBooks$;
  }

  returnBook(borrowId: number, bookTitle: string){
    this.borrowService.returnBook( borrowId )
      .subscribe(
        {
          next: () =>
          {
            //actualizeaza cartile imprumutate
            this.borrowService.loadBorrowedBooks();
            console.log(`${bookTitle} returned`);
            //alert('You returned ' + bookTitle + ' successfully');
          },
          error: error =>
          {
            console.log(error);
            //alert(error.error);            
          }
        }
      )
  }
}
