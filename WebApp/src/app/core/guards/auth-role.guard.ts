import { inject } from '@angular/core';
import {CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { RolesDefine } from '../auth/auth.model';
import { map } from 'rxjs';

export const authRoleGuard: CanActivateFn = (route, state) => {
  //  inject
  const auth = inject(AuthService);
  const router = inject(Router);

  const expectedRole = route.data['role'] as RolesDefine;


  return auth.hasRole(expectedRole).pipe(
    map(hasRole=>{
      if(!hasRole){
        router.navigate(['/not-authorized'])
      }
      return hasRole;
    })
  )
};
