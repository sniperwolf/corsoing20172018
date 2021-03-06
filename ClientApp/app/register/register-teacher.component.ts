﻿import { Component } from '@angular/core'
import { Router } from '@angular/router'

import { AlertService, TeacherService } from '../_services/index'

@Component({
  moduleId: module.id,
  templateUrl: 'register-teacher.component.html'
})

export class RegisterTeacherComponent {
  model: any = {}
  loading = false

  constructor (
    private router: Router,
    private teacherService: TeacherService,
    private alertService: AlertService) { }

  register () {
    this.loading = true
    this.teacherService
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
