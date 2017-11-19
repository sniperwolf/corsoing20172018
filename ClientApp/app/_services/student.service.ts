import { Injectable } from '@angular/core'
import { Http, Headers, RequestOptions, Response } from '@angular/http'

import { AppConfig } from '../app.config'
import { Student } from '../_models/index'

@Injectable()
export class StudentService {
  constructor (private http: Http, private config: AppConfig) { }

  getAll () {
    return this.http.get(this.config.apiUrl + '/students', this.jwt()).map((response: Response) => response.json())
  }

  getById (id: number) {
    return this.http.get(this.config.apiUrl + '/students/' + id, this.jwt()).map((response: Response) => response.json())
  }

  create (student: Student) {
    return this.http.post(this.config.apiUrl + '/students', student, this.jwt())
  }

  update (student: Student) {
    return this.http.put(this.config.apiUrl + '/students/' + student.id, student, this.jwt())
  }

  delete (id: number) {
    return this.http.delete(this.config.apiUrl + '/students/' + id, this.jwt())
  }

  // private helper methods
  private jwt () {
    // create authorization header with jwt token
    let currentStudent = JSON.parse(localStorage.getItem('currentUser'))
    if (currentStudent && currentStudent.token) {
      let headers = new Headers({ 'Authorization': 'Bearer ' + currentStudent.token })
      return new RequestOptions({ headers: headers })
    }
  }
}
