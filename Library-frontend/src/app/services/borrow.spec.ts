import { describe, beforeEach, afterEach, it, expect } from 'vitest';
//creeaza o aplicatie Angular falsa
import { TestBed } from '@angular/core/testing';
//creeaza un client Http
import { provideHttpClient } from '@angular/common/http';

import {
  provideHttpClientTesting,//creeaza cereri http false
  HttpTestingController //permite inspectarea cererilor http
} from '@angular/common/http/testing';

import { Borrow } from './borrow';
import { BorrowBookRequest } from '../models/borrow-book-request';
import { BorrowedBook } from '../models/borrowed-book';

describe('Borrow', () => {
  let service: Borrow;
  let httpTesting: HttpTestingController;
  const baseUrl = 'http://localhost:5041/api';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        Borrow, provideHttpClient(), provideHttpClientTesting()
      ]      
    });

    service = TestBed.inject(Borrow);

    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {

    httpTesting.verify();
  });

  //test1 -> se asteapta crearea serviciului Borrow
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //test2 -> verif borrowBook()
  it('should borrow a book', () => {
    //arrange
    const requestData: BorrowBookRequest = {
      bookId: 1
    };

    const response = { message: 'Book borrowed successfully'};

    //act
    const observable = service.borrowBook(requestData);

    //assert
    observable.subscribe(res => )
    const request = httpTesting.expectOne(`${baseUrl}/borrow/borrow`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(requestData);
    request.flush(response);
  });
});
 