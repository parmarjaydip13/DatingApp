import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwlHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((resonse: any) => {
        const user = resonse;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwlHelper.decodeToken(user.token);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwlHelper.isTokenExpired(token);
  }
}
