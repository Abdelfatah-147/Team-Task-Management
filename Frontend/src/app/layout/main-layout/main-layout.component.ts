import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

interface NavLink {
  label: string;
  path:string;
  icon:string;
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent {
  isSidebarOpen = signal(false);
  navLinks : NavLink[] = [
    {label: 'Teams', path:'/dashboard/teams', icon:'M17 20h5v-2a4 4 0 00-3-3.87M9 20H4v-2a4 4 0 013-3.87m6-1.13a4 4 0 10-4-4 4 4 0 004 4zm6-6a4 4 0 10-4-4'},
    {label:'Projects', path:'/dashboard/projects',icon:'M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z'},
    {label: 'Tasks', path:'/dashboard/tasks',icon:'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2'}
  ];

  public authService = inject(AuthService);
  
  toggleSidebar(): void { 
    this.isSidebarOpen.set(!this.isSidebarOpen());
  }

  closeSidebar():void{
    this.isSidebarOpen.set(false);
  }

  logout():void{
    this.authService.logout();
  }

}