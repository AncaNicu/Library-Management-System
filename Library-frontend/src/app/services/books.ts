import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import {Book} from '../models/book';

@Injectable({
  providedIn: 'root',
})
export class Books {

  //portul pe care ruleaza BE
  private baseUrl = 'http://localhost:5041/api';

  //starea shared
  private booksSubject = new BehaviorSubject<Book[]>([]);

  //observable pt componente
  books$ = this.booksSubject.asObservable();

  constructor(private http: HttpClient) {}

  loadBooks(): void {
    this.http.get<Book[]>(`${this.baseUrl}/books`)
      .subscribe(books => {
        this.booksSubject.next(books);
      });
  }
  
  getBookById(bookId: number) {
    return this.http.get<Book>(`${this.baseUrl}/books/${bookId}`);
  }
}
