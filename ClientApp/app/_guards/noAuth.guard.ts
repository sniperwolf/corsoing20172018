import { Injectable } from '@angular/core'
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router'

@Injectable()
export class NoAuthGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let user = JSON.parse(localStorage.getItem('currentUser'))
    if (! user) {
      // not logged in so return false
      return true
    }

    let returnUrl = user.role ? '/teachers' : 'students/'
    this.router.navigate([returnUrl], { queryParams: { returnUrl: state.url } })
    return false
  }
}
