import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class CounterBehaviorService {

  //private pentru a putea fi modificat doar din interiorul serviciului
  private counterSubject = new BehaviorSubject<number>(0);

  //counter-ul e declasat asObservable
  //pt a putea fi doar citit si nu modificat din afara
  counter$ = this.counterSubject.asObservable();

  increment() {
    //next e ca set la signal 
    this.counterSubject.next(this.counterSubject.value + 1);
  }

}