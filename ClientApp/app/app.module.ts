import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { FormsModule } from '@angular/forms'
import { HttpModule } from '@angular/http'

import { AppComponent } from './app.component'
import { routing } from './app.routing'
import { AppConfig } from './app.config'

import { AlertComponent } from './_directives/index'
import { AuthGuard, NoAuthGuard, isStudentGuard, isTeacherGuard } from './_guards/index'
import { AlertService, AuthenticationService, StudentService, TeacherService } from './_services/index'
import { HomeComponent, HomeStudentComponent, HomeTeacherComponent } from './home/index'
import { LoginComponent } from './login/index'
import { RegisterStudentComponent, RegisterTeacherComponent } from './register/index'

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    routing
  ],
  declarations: [
    AppComponent,
    AlertComponent,
    HomeComponent,
    HomeStudentComponent,
    HomeTeacherComponent,
    LoginComponent,
    RegisterStudentComponent,
    RegisterTeacherComponent
  ],
  providers: [
    AppConfig,
    AuthGuard,
    NoAuthGuard,
    isStudentGuard,
    isTeacherGuard,
    AlertService,
    AuthenticationService,
    StudentService,
    TeacherService
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
