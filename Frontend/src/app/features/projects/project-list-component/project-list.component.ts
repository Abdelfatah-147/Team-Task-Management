import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { forkJoin } from 'rxjs';
import { ProjectService } from '../../../core/services/project.service';
import { TeamService } from '../../../core/services/team.service';
import { AuthService } from '../../../core/services/auth.service';
import { Project, ProjectStatusLabels } from '../../../core/models/project/project.model';
import { Team } from '../../../core/models/team/team.model';
import { ProjectFormModalComponent } from '../project-form-modal-component/project-form-modal.component';

interface ProjectGroup {
  teamId: string;
  teamName: string;
  projects: Project[];
}

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [RouterLink, DatePipe, CommonModule, ProjectFormModalComponent],
  templateUrl: './project-list.component.html',
  styleUrl: './project-list.component.scss'
})
export class ProjectListComponent implements OnInit {
  private readonly projectService = inject(ProjectService);
  private readonly teamService = inject(TeamService);
  private readonly authService = inject(AuthService);

  isLoading = signal(true);
  errorMessage = signal<string | null>(null);
  teams = signal<Team[]>([]);
  projects = signal<Project[]>([]);

  showModal = signal(false);
  projectToEdit = signal<Project | null>(null);
  projectToDeleteId = signal<string | null>(null);
  isDeleting = signal(false);

  statusLabels = ProjectStatusLabels;

  readonly canManage = computed(() => {
    const roles = this.authService.roles();
    return roles.includes('Admin') || roles.includes('Manager');
  });

  readonly canDelete = computed(() =>
    this.authService.roles().includes('Admin')
  );

  groupedProjects = computed<ProjectGroup[]>(() => {
    const teamsMap = new Map(this.teams().map(t => [t.id, t.name]));
    const groups = new Map<string, ProjectGroup>();
    for (const project of this.projects()) {
      if (!groups.has(project.teamId)) {
        groups.set(project.teamId, {
          teamId: project.teamId,
          teamName: teamsMap.get(project.teamId) ?? 'Unknown Team',
          projects: []
        });
      }
      groups.get(project.teamId)!.projects.push(project);
    }
    return Array.from(groups.values());
  });

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    this.isLoading.set(true);
    forkJoin({
      teams: this.teamService.getAll(),
      projects: this.projectService.getAll()
    }).subscribe({
      next: ({ teams, projects }) => {
        this.teams.set(teams);
        this.projects.set(projects ?? []);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load projects.');
        this.isLoading.set(false);
      }
    });
  }

  openEditModal(project: Project, event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    this.projectToEdit.set(project);
    this.showModal.set(true);
  }

  openCreateModal(): void {
    this.projectToEdit.set(null);
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.projectToEdit.set(null);
  }

  onProjectSaved(): void {
    this.closeModal();
    this.loadData();
  }

  confirmDelete(projectId: string, event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    this.projectToDeleteId.set(projectId);
  }

  cancelDelete(): void {
    this.projectToDeleteId.set(null);
  }

  executeDelete(): void {
    const id = this.projectToDeleteId();
    if (!id) return;
    this.isDeleting.set(true);
    this.projectService.delete(id).subscribe({
      next: () => {
        this.projects.update(list => list.filter(p => p.id !== id));
        this.isDeleting.set(false);
        this.projectToDeleteId.set(null);
      },
      error: () => this.isDeleting.set(false)
    });
  }

  getTeamIdForProject(project: Project): string {
    return project.teamId;
  }

  getStatusClass(status: number): string {
    const map: Record<number, string> = {
      1: 'bg-gray-100 text-gray-600',
      2: 'bg-blue-100 text-blue-700',
      3: 'bg-yellow-100 text-yellow-700',
      4: 'bg-green-100 text-green-700'
    };
    return map[status] ?? 'bg-gray-100 text-gray-600';
  }
}