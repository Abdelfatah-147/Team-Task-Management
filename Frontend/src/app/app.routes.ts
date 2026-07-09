import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {path: '' , pathMatch:'full',redirectTo:'dashboard'},
    {path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(c => c.LoginComponent)},
    {path: 'register', loadComponent: () => import('./features/auth/register/register.component').then(c => c.RegisterComponent)},
    {path: 'dashboard', canActivate: [authGuard], loadComponent: () => import('./layout/main-layout/main-layout.component').then(c => c.MainLayoutComponent),
        children:[
            { path: '', pathMatch: 'full', redirectTo: 'home' },
            { path: 'home', loadComponent: () => import('./features/dashboard/dashboard-home/dashboard-home.component').then(c => c.DashboardHomeComponent) },    
            {path: 'teams', loadComponent: () => import('./features/teams/team-list/team-list.component').then(c => c.TeamListComponent)},
            {path: 'teams/:id', loadComponent: () => import('./features/teams/team-details/team-details.component').then(c => c.TeamDetailsComponent)},
            {path: 'projects', loadComponent: () => import('./features/projects/project-list-component/project-list.component').then(c => c.ProjectListComponent)},
            {path: 'projects/:id', loadComponent: () => import('./features/projects/project-details/project-details.component').then(c => c.ProjectDetailsComponent)},
            {path: 'tasks', loadComponent: () => import('./features/tasks/task-list/task-list.component').then(c => c.TaskListComponent)}
        ]
    },
    {path: 'unauthorized', loadComponent: () => import('./shared/components/unauthorized/unauthorized-component/unauthorized.component').then(c => c.UnauthorizedComponent)},
    {path: '**', loadComponent: () => import('./shared/components/not-found/not-found-component/not-found.component').then(c => c.NotFoundComponent)}
];

