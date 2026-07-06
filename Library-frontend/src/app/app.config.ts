import { ApplicationConfig } from '@angular/core';

import { provideRouter } from '@angular/router';

//permite apelarea BE si inregistrarea interceptorului
import { provideHttpClient, withInterceptors } from '@angular/common/http';

//adauga interceptor-ul
import { authInterceptor } from './interceptors/auth.interceptor';


import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {

  providers: [

    provideRouter(routes),

    provideHttpClient(
      withInterceptors([authInterceptor])
    )

  ]
};