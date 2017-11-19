import { Injectable } from '@angular/core'
import { Http, Headers, RequestOptions, Response } from '@angular/http'

import { AppConfig } from '../app.config'
import { Teacher } from '../_models/index'

@Injectable()
export class TeacherService {
  constructor (private http: Http, private config: AppConfig) { }

  getAll () {
    return this.http.get(this.config.apiUrl + '/teachers', this.jwt()).map((response: Response) => response.json())
  }

  getById (id: number) {
    return this.http.get(this.config.apiUrl + '/teachers/' + id, this.jwt()).map((response: Response) => response.json())
  }

  create (teacher: Teacher) {
    return this.http.post(this.config.apiUrl + '/teachers', teacher, this.jwt())
  }

  update (teacher: Teacher) {
    return this.http.put(this.config.apiUrl + '/teachers/' + teacher.id, teacher, this.jwt())
  }

  delete (id: number) {
    return this.http.delete(this.config.apiUrl + '/teachers/' + id, this.jwt())
  }

  // private helper methods
  private jwt () {
    // create authorization header with jwt token
    let currentTeacher = JSON.parse(localStorage.getItem('currentUser'))
    if (currentTeacher && currentTeacher.token) {
      let headers = new Headers({ 'Authorization': 'Bearer ' + currentTeacher.token })
      return new RequestOptions({ headers: headers })
    }
  }
}
