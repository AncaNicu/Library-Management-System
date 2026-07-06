import { Component } from '@angular/core';
//pt a putea accesa id-ul din route
import { ActivatedRoute } from '@angular/router';

//modelul si serviciul
import { Book } from '../../models/book';
import { Books } from '../../services/books';

import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './book-details.html',
  styleUrl: './book-details.css',
})
export class BookDetails {

  book$!: Observable<Book>;

  constructor(
    private route: ActivatedRoute,
    private booksService: Books
  )
  {
    //obtine id-ul din url si obtine cartea pe baza lui
    this.route.params.subscribe(params =>
    {
      const bookId = Number(params['id']);

      this.book$ = this.booksService.getBookById(bookId);
    });
  }
}
