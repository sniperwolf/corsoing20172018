import { Component, OnInit } from '@angular/core'

import { Student } from '../_models/index'
import { StudentService } from '../_services/index'

@Component({
  moduleId: module.id,
  templateUrl: 'home-student.component.html'
})

export class HomeStudentComponent implements OnInit {
  currentUser: Student
  students: Student[] = []

  constructor (private studentService: StudentService) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'))
  }

  ngOnInit () {
    this.loadAllStudents()
  }

  deleteStudent (id: number) {
    this.studentService.delete(id).subscribe(() => { this.loadAllStudents() })
  }

  private loadAllStudents () {
    this.studentService.getAll().subscribe(students => { this.students = students })
  }
}
