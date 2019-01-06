import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(err => {
        if (err instanceof HttpErrorResponse) {
          console.log(err);

          if (err.status === 401) {
            return throwError(err.statusText);
          }

          const applicationError = err.headers.get('Application-Error');

          if (applicationError) {
            console.error(applicationError);
            return throwError(applicationError);
          }

          const serverError = err.error;
          let modelStateError = '';
          if (serverError && typeof serverError === 'object') {
            const errobject = serverError['errors'];

            for (const key in errobject) {
              if (errobject[key]) {
                modelStateError += errobject[key] + '\n';
              }
            }
          }
          return throwError(modelStateError || serverError || 'server error');
        }
      })
    );
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
