import { Component, OnInit } from '@angular/core'

import { Student } from '../_models/index'
import { StudentService, AlertService } from '../_services/index'

@Component({
  moduleId: module.id,
  templateUrl: 'home.component.html'
})

export class HomeComponent implements OnInit {
  students: Student[] = []

  constructor(private studentService: StudentService,
    private alertService: AlertService) {
  }

  ngOnInit() {
    this.loadAllStudents()
  }

  private loadAllStudents() {
    this.studentService.getAll().subscribe(students => { this.students = students })

    // if (!this.students.length) {
    //   this.alertService.error('You are not authorized.')
    // }
  }
}
