import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Person } from '../../models/person';

@Component({
  selector: 'app-age-validator.component',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './age-validator.component.html',
  styleUrl: './age-validator.component.css',
})
export class AgeValidatorComponent {

  //creeaza intregul form
  ageForm = new FormGroup({

    name: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[a-zA-Z]+$/)
    ]),

    //un camp cu valoarea initiala null
    age: new FormControl<number | null>(null, 
      [
        Validators.required, 
        Validators.min(18)
      ])
  });

  submit()
  {
    const person: Person = this.ageForm.getRawValue() as Person;

    alert(`Name: ${person.name}\nAge: ${person.age}`);
    this.ageForm.reset();
  }

  get name()
  {
    return this.ageForm.get('name');
  }

  get age()
  {
    return this.ageForm.get('age');
  }

  get ageMessage(): string {

    if (this.age?.value == null)
        return "Please enter your age.";

    if (this.age.hasError('min'))
        return "Too young.";
    
    if(this.age.hasError('required'))
        return "Age is required.";

    return "Accepted";
  }
}
