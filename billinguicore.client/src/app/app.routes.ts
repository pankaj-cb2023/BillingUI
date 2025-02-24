import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';



export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'rate-plans',
    loadComponent: () => import('./components/video-connect-rates/rate-plans/rate-plans.component').then((c) => c.RatePlansComponent)
  },
  {
    path: 'sites',
    loadComponent: () => import('./components/video-connect-rates/sites/sites.component').then((c) => c.SitesComponent)
  }   ,
  {
    path: 'agencyresult',
    loadComponent: () => import('./components/video-connect-rates/agency-result/agency-result.component').then((c) => c.AgencyResultComponent)
  },
  {
    path: 'siteresult',
    loadComponent: () => import('./components/video-connect-rates/site-result/site-result.component').then((c) => c.SiteResultComponent)
  },
  {
    path: 'searchfilter',
    loadComponent: () => import('./components/video-connect-rates/search-filter/search-filter.component').then((c) => c.SearchFilterComponent)
  },
  {
    path: 'siteselection',
    loadComponent: () => import('./components/video-connect-rates/site-selection/site-selection.component').then((c) => c.SiteSelectionComponent)
  }, 
  {
    path: 'rate-plans-details',
    loadComponent: () => import('./components/video-connect-rates/rate-plans/rate-plans-details/rate-plans-details.component').then((c) => c.RatePlansDetailsComponent)   
  },
  {
    path: 'user-permission',
    loadComponent: () => import('./components/permission/user-permission/user-permission.component').then((c) => c.UserPermissionComponent)
  },
  {
    path: 'access-denied',
    loadComponent: () => import('./components/permission/access-denied/access-denied.component').then((c) => c.AccessDeniedComponent)
  }
   
];
