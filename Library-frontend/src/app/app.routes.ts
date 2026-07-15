import { Routes } from '@angular/router';
import { BookDetails } from './components/book-details/book-details';
import { BooksList } from './components/books-list/books-list';
import { BorrowedBooks } from './components/borrowed-books/borrowed-books';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { authGuard } from './guards/auth.guard';
import { Counter } from './components/counter/counter';
import { CounterBehavior } from './components/counter-behavior/counter-behavior';
import { AgeValidatorComponent } from './components/age-validator.component/age-validator.component';

export const routes: Routes = [
    { path: '', component: BooksList },
    { path: 'books', component: BooksList },
    { path: 'books/:id', component: BookDetails },
    //accesibila doar utilizatorilor logati
    { path: 'borrowed-books', component: BorrowedBooks, canActivate: [authGuard] },
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'counter', component: Counter },
    { path: 'counter-behavior', component: CounterBehavior },
    { path: 'age-validator', component: AgeValidatorComponent }
];
