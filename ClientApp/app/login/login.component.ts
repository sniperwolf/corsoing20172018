import { Component, OnInit } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'

import { AlertService, AuthenticationService } from '../_services/index'

@Component({
  moduleId: module.id,
  templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {
  model: any = {}
  loading = false
  returnUrl: string

  constructor (
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private alertService: AlertService) { }

  ngOnInit () {
    // reset login status
    this.authenticationService.logout()
    this.model.role = 0;
  }

  login () {
    this.loading = true

    this.authenticationService.login(this.model.registrationNumber, this.model.password, this.model.role)
      .subscribe(
        data => {
          let returnUrl = this.model.role ? '/teachers' : 'students/'
          this.router.navigate([returnUrl])
        },
        error => {
          this.alertService.error('RegistrationNumber or password is incorrect.')
          this.loading = false
      })
  }
}
