import { Injectable } from '@angular/core'
import { Http, Headers, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/map'

import { AppConfig } from '../app.config'

@Injectable()
export class AuthenticationService {
  constructor (private http: Http, private config: AppConfig) { }

  login (RegistrationNumber: string, password: string, role: number) {
    let roleBool = (role == 1)

    return this.http.post(this.config.authUrl + '/authenticate', {
      'RegistrationNumber': RegistrationNumber, 'password': password, 'role': roleBool
    }).map((response: Response) => {
      let user = response.json()

      // login successful if there is a JWT
      if (user && user.token) {
        user.role = roleBool
        // store user Details and JWT in local storage to keep user logged in
        localStorage.setItem('currentUser', JSON.stringify(user))
      }
    })
  }

  logout () {
    // remove user from local storage
    localStorage.removeItem('currentUser')
  }
}
