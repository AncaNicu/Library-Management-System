import { describe, beforeEach, afterEach, it, expect } from 'vitest';
//creeaza o aplicatie Angular falsa
import { TestBed } from '@angular/core/testing';
//creeaza un client Http
import { provideHttpClient } from '@angular/common/http';

import {
  provideHttpClientTesting,//creeaza cereri http false
  HttpTestingController //permite inspectarea cererilor http
} from '@angular/common/http/testing';

import { Books } from './books';
import { Book } from '../models/book';

describe('Books', () => {
  let service: Books;
  let httpTesting: HttpTestingController;
  const baseUrl = 'http://localhost:5041/api';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        Books, provideHttpClient(), provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(Books);

    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {

    httpTesting.verify();
  });

  //test1 -> se asteapta crearea serviciului Books
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //test2 -> verifica getBookById()
  it('should send a get book by id request', () => {
    //arrange -> creeaza cartea raspuns
    const response: Book = {
      id: 1,
      title: 'Book1',
      author: 'Author1',
      category: 'Crime',
      noOfAvailableCopies: 13,
      imageUrl: null
    };

    //act
    service.getBookById(1)
      .subscribe(book => {
        //assert
        expect(book).toEqual(response);
    });

    const request = httpTesting.expectOne(`${baseUrl}/books/1`);
    expect(request.request.method).toBe('GET');
    request.flush(response);
    
  });

  //test3 -> verifica loadBooks()
  it('should load books and update books$', () => {
    //arrange
    const books: Book[] = [
      {
        id: 1,
        title: '1984',
        author: 'George Orwell',
        category: 'Fiction',
        noOfAvailableCopies: 5,
        imageUrl: null
      },
      {
        id: 2,
        title: 'Dune',
        author: 'Frank Herbert',
        category: 'Sci-Fi',
        noOfAvailableCopies: 3,
        imageUrl: null
      }
    ];

    //act
    service.loadBooks();

    //assert
    const request = httpTesting.expectOne(`${baseUrl}/books`);
    expect(request.request.method).toBe('GET');
    //falsifica rasp de la BE, ca si cum BE ret books
    request.flush(books);

    //actualizeaza BehaviorSubject si 
    //verif daca books$ a fost actualizat la books
    service.books$.subscribe(result => {
      expect(result).toEqual(books);
    });
  });
});
