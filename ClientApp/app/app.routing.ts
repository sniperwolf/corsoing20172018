import { Routes, RouterModule } from '@angular/router'

import { HomeComponent, HomeStudentComponent, HomeTeacherComponent } from './home/index'
import { LoginComponent } from './login/index'
import { RegisterStudentComponent, RegisterTeacherComponent } from './register/index'
import { NoAuthGuard, AuthGuard, isStudentGuard, isTeacherGuard } from './_guards/index'

const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [NoAuthGuard] },
  { path: 'students', component: HomeStudentComponent, canActivate: [AuthGuard, isStudentGuard] },
  { path: 'teachers', component: HomeTeacherComponent, canActivate: [AuthGuard, isTeacherGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register-student', component: RegisterStudentComponent, canActivate: [NoAuthGuard] },
  { path: 'register-teacher', component: RegisterTeacherComponent, canActivate: [NoAuthGuard] },
  //{ path: 'register-admin', component: RegisterAdminComponent },


  // otherwise redirect to home
  { path: '**', redirectTo: '' }
]

export const routing = RouterModule.forRoot(appRoutes)
