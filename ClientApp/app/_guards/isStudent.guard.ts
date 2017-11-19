import { Injectable } from '@angular/core'
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router'

@Injectable()
export class isStudentGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let user = JSON.parse(localStorage.getItem('currentUser'))
    if (user && user.role === false) {
      // It is a student, so return true
      return true
    }

    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } })
    return false
  }
}
