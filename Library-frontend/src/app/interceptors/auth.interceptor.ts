import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const token = localStorage.getItem('token');

  //utilizator logat
  if (token) {
    //cloneaza cererea si adauga token-ul din local storage
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    //trimite cererea modificata
    return next(cloned);
  }

  //utilizator nelogat => nu exista token => se trimite cererea originala
  return next(req);
};