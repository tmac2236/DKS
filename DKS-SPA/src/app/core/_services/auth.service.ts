import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../_models/user';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  //decodedToken: any; // keep the imformation from token
  //photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  //currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) {}

  //changeMemberPhoto(photoUrl: string) {
  //  this.photoUrl.next(photoUrl);
  // }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          //localStorage.setItem('user', JSON.stringify(user.user));
          //this.decodedToken = this.jwtHelper.decodeToken(user.token);
          
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
  checkIsThatGroup(groupNo:string){

    let result = false;
    const token = localStorage.getItem('token');
    if (token) {
      // token is expired?
      let isExpired = this.jwtHelper.isTokenExpired(token);
      console.log("Auth Service => Token is Expired? :" + isExpired);
      if(isExpired) return false;
      
      let roleArray = this.jwtHelper.decodeToken(token)["role"];
      console.log("Auth Service => the account role is :" + roleArray);
      result = roleArray.includes(groupNo);
      console.log("Auth Service => the account role is Valid? "+ result );
    }

    return result;
  }
}
