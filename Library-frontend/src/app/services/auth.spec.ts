import { describe, beforeEach, afterEach, it, expect } from 'vitest';
//creeaza o aplicatie Angular falsa
import { TestBed } from '@angular/core/testing';
//creeaza un client Http
import { provideHttpClient } from '@angular/common/http';

import {
  provideHttpClientTesting,//creeaza cereri http false
  HttpTestingController //permite inspectarea cererilor http
} from '@angular/common/http/testing';

import { Auth } from './auth';

describe('Auth', () => {
  let service: Auth;
  let httpTesting: HttpTestingController;
  const baseUrl = 'http://localhost:5041/api';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        Auth, provideHttpClient(), provideHttpClientTesting()
      ]
    });

    //fiecare test porneste cu un nou Auth service,
    //un nou BE fals si cu localStorage gol
    service = TestBed.inject(Auth);

    httpTesting = TestBed.inject(HttpTestingController);

    localStorage.clear();
  });

  afterEach(() => {

    httpTesting.verify();

    localStorage.clear();
  });

  //test1 -> se asteapta crearea serviciului auth
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //test2 -> verifica saveLogin()
  it('should save login data to localStorage', () => {
    //arrange -> construieste rasp de la login cu succes
    const response = {
      message: 'Login successful',
      token: 'abc123',
      userId: 1,
      name: 'John'
    };

    //act -> apeleaza serviciul
    service.saveLogin(response);

    //assert -> verif daca s-au salvat datele corect in localStorage
    expect(localStorage.getItem('token')).toBe('abc123');
    expect(localStorage.getItem('userId')).toBe('1');
    expect(localStorage.getItem('name')).toBe('John');
  });

  //test3 -> verifica logout()
  it('should remove login data from localStorage', () => {
    //arrange -> salveaza date in localStorage
    localStorage.setItem('token', 'abc123');
    localStorage.setItem('userId', '1');
    localStorage.setItem('name', 'John');  

    //act -> apeleaza serviciul
    service.logout();

    //assert -> verif daca au fost scoase datele din localStorage
    expect(localStorage.getItem('token')).toBeNull();
    expect(localStorage.getItem('userId')).toBeNull();
    expect(localStorage.getItem('name')).toBeNull();    
  });

  //test4 -> verif daca isLoggedIn() ret true cand e utiliz logat
  it('should return true when token exists', () => {
    //arrange -> salveaza date in localStorage
    localStorage.setItem('token', 'abc123');

    //act -> apeleaza serviciul
    var result = service.isLoggedIn();

    //assert -> verif ca a ret true
    expect(result).toBe(true);
  });

  //test5 -> verif daca isLoggedIn() ret false cand e utiliz nelogat
  it('should return false when token does not exists', () => {
    //act -> apeleaza serviciul
    var result = service.isLoggedIn();
    
    //assert -> verif ca a ret true
    expect(result).toBe(false);
  }); 
  
  //test6 -> verif daca se trimite corect cererea de register
  it('should send a register request', () => {
    //arrange -> pregateste datele
    const requestData = {
      name: 'John',
      email: 'john@gmail.com',
      password: '123',
      confirmPassword: '123'
    };

    const response = 
    {
      message: 'Register successful'
    };

    //act -> apeleaza register()
    const observable = service.register(requestData);

    //assert
    observable.subscribe(result => {
      expect(result).toEqual(response);
    });
    const request = httpTesting.expectOne(`${baseUrl}/auth/register`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(requestData);
    request.flush(response);
  });

  //test7 -> verif daca se trimite corect cererea de login
  it('should send a login request', () => {
    //arrange -> pregateste datele
    const requestData = {
      email: 'john@gmail.com',
      password: '123'
    };

    const response = 
    {
      message: 'Login successful'
    };

    //act -> apeleaza register()
    const observable = service.login(requestData);

    //assert
    observable.subscribe(result => {
      expect(result).toEqual(response);
    });
    const request = httpTesting.expectOne(`${baseUrl}/auth/login`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(requestData);
    request.flush(response);
  });
});
