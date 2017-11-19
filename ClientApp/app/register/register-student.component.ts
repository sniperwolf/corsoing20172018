import { Component } from '@angular/core'
import { Router } from '@angular/router'

import { AlertService, StudentService } from '../_services/index'

@Component({
  moduleId: module.id,
  templateUrl: 'register-student.component.html'
})

export class RegisterStudentComponent {
  model: any = {}
  loading = false

  constructor (
    private router: Router,
    private studentService: StudentService,
    private alertService: AlertService) { }

  register () {
    this.loading = true
    this.studentService
      .create(this.model)
      .subscribe(
      data => {
        this.alertService.success('Registration successful', true)
        this.router.navigate(['/login'])
      },
      error => {
        this.alertService.error(error._body)
        this.loading = false
      })
  }
}
