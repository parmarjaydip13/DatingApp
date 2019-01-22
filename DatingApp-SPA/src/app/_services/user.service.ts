import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../_models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers(
    page?,
    itemPerPage?,
    userParams?,
    likesParmas?
  ): Observable<PaginatedResult<User[]>> {
    let parms = new HttpParams();
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<
      User[]
    >();

    if (page != null && itemPerPage != null) {
      parms = parms.append('pageNumber', page);
      parms = parms.append('pageSize', itemPerPage);
    }

    if (userParams != null) {
      parms = parms.append('minAge', userParams.minAge);
      parms = parms.append('maxAge', userParams.maxAge);
      parms = parms.append('gender', userParams.gender);
      parms = parms.append('orderBy', userParams.orderBy);
    }

    if (likesParmas === 'Likers') {
      parms = parms.append('likers', 'true');
    }

    if (likesParmas === 'Likees') {
      parms = parms.append('likees', 'true');
    }

    return this.http
      .get<User[]>(this.baseUrl + 'users', {
        observe: 'response',
        params: parms
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;

          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(userId: number, id: number) {
    return this.http.post(
      this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain',
      {}
    );
  }

  deletePhoto(userId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id);
  }

  sendLike(id: number, recipientId: number) {
    return this.http.post(
      this.baseUrl + 'users/' + id + '/like/' + recipientId,
      {}
    );
  }
}
