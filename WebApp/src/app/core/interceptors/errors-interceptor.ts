import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, map, of, tap } from 'rxjs';

export const errorsInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError(error=>{
      // handle when exception like 401, 404 or 500
      // i still export status code when not match common case
      return of(error);
    }),
    map(res=>{
      return res;
    })
  )
};
