import { Injectable } from '@angular/core'
import { Http, Headers, RequestOptions, Response } from '@angular/http'

import { AppConfig } from '../app.config'
import { Admin } from '../_models/index'

@Injectable()
export class AdminService {
  constructor (private http: Http, private config: AppConfig) { }

  getAll () {
    return this.http.get(this.config.apiUrl + '/admins', this.jwt()).map((response: Response) => response.json())
  }

  getById (id: number) {
    return this.http.get(this.config.apiUrl + '/admins/' + id, this.jwt()).map((response: Response) => response.json())
  }

  create (admin: Admin) {
    return this.http.post(this.config.apiUrl + '/admins', admin, this.jwt())
  }

  update (admin: Admin) {
    return this.http.put(this.config.apiUrl + '/admins/' + admin.id, admin, this.jwt())
  }

  delete (id: number) {
    return this.http.delete(this.config.apiUrl + '/admins/' + id, this.jwt())
  }

  // private helper methods
  private jwt () {
    // create authorization header with jwt token
    let currentAdmin = JSON.parse(localStorage.getItem('currentUser'))
    if (currentAdmin && currentAdmin.token) {
      let headers = new Headers({ 'Authorization': 'Bearer ' + currentAdmin.token })
      return new RequestOptions({ headers: headers })
    }
  }
}
