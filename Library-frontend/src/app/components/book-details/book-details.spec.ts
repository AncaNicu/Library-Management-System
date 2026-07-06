import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { BookDetails } from './book-details';
import { Books } from '../../services/books';

describe('BookDetails', () => {
  let component: BookDetails;
  let fixture: ComponentFixture<BookDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookDetails],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ id: 1 })
          }
        },
        {
          provide: Books,
          useValue: {
            getBookById: () => of({
              id: 1,
              title: 'Test Book',
              author: 'Test Author',
              category: 'Test',
              noOfAvailableCopies: 5,
              imageUrl: null
            })
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(BookDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
