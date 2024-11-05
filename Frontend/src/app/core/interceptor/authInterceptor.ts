import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, Observable, throwError } from "rxjs";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = localStorage.getItem('authToken');
        if (token) {
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }
    
        return next.handle(req).pipe(
            catchError((error) => {
                console.error('Interceptor caught an error:', error);
                if (error instanceof HttpErrorResponse) {
                    if (error.status === 401 || error.status === 403) {
                        console.log('Redirecting to login due to unauthorized or forbidden error');
                        localStorage.removeItem('authToken'); 
                        this.router.navigate(['/login']);
                    }
                }
                return throwError(() => error);
            })
        );
    }
    
}
