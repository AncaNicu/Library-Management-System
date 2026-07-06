import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { BorrowedBook } from '../models/borrowed-book';
import { BorrowBookRequest } from '../models/borrow-book-request';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Borrow {

  //portul pe care ruleaza BE
  private baseUrl = 'http://localhost:5041/api';

  //starea shared
  private borrowedBooksSubject = new BehaviorSubject<BorrowedBook[]>([]);

  //observable pt componente
  borrowedBooks$ = this.borrowedBooksSubject.asObservable();

  //nr de carti imprumutate
  borrowedCount$ = this.borrowedBooks$.pipe(
    map(books => books.length)
  );

  constructor(private http: HttpClient) {}

  borrowBook(borrowRequest: BorrowBookRequest): Observable<any>
  {
    return this.http.post(`${this.baseUrl}/borrow/borrow`, borrowRequest);
  }

  returnBook(borrowId: number): Observable<any>
  {
    return this.http.post(`${this.baseUrl}/borrow/return/${borrowId}`, {});
  }

  loadBorrowedBooks(): void 
  {
    this.http.get<BorrowedBook[]>(`${this.baseUrl}/borrow/my-borrowed-books`)
      .subscribe(borrowBooks => {
        this.borrowedBooksSubject.next(borrowBooks);
      });
  }
}
