import { Component, OnInit } from '@angular/core'

import { Teacher } from '../_models/index'
import { TeacherService } from '../_services/index'

@Component({
  moduleId: module.id,
  templateUrl: 'home-teacher.component.html'
})

export class HomeTeacherComponent implements OnInit {
  currentUser: Teacher
  teachers: Teacher[] = []

  constructor(private teacherService: TeacherService) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'))
  }

  ngOnInit() {
    this.loadAllTeachers()
  }

  deleteTeacher(id: number) {
    this.teacherService.delete(id).subscribe(() => { this.loadAllTeachers() })
  }

  private loadAllTeachers() {
    this.teacherService.getAll().subscribe(teachers => { this.teachers = teachers })
  }
}
