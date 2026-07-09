import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { ProjectService } from '../../../core/services/project.service';
import { TaskService } from '../../../core/services/task.service';
import { AuthService } from '../../../core/services/auth.service';
import { forkJoin, single } from 'rxjs';
import { TaskItem, TaskPriorityLabels, TasksStatus, TasksStatusLabels } from '../../../core/models/task/task.model';
import { Project, ProjectStatusLabels } from '../../../core/models/project/project.model';

@Component({
  selector: 'app-dashboard-home',
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard-home.component.html',
  styleUrl: './dashboard-home.component.scss',
})
export class DashboardHomeComponent implements OnInit {
  private readonly teamService = inject(TeamService);
  private readonly projectService = inject(ProjectService);
  private readonly taskService = inject(TaskService);
  private readonly authService = inject(AuthService);
  readonly currentUser = this.authService.currentUser;

  isLoading = signal(true);
  teamsCount = signal(0);
  projectsCount = signal(0);
  recentTasks = signal<TaskItem[]>([]);
  recentProjects = signal<Project[]>([]);

  readonly TasksStatusLabels = TasksStatusLabels;
  readonly TaskPriorityLabels = TaskPriorityLabels;
  readonly ProjectStatusLabels = ProjectStatusLabels;

  readonly todoCount = computed(() => {this.recentTasks().filter(t => t.status === TasksStatus.Todo).length});
  readonly inProgressCount = computed(() => this.recentTasks().filter(t => t.status === TasksStatus.InProgress).length);
  readonly doneCount = computed(() => this.recentTasks().filter(t => t.status === TasksStatus.Done).length);

  ngOnInit(): void {
    forkJoin({
      teams: this.teamService.getAll(),
      projects: this.projectService.getAll()
    }).subscribe({
      next: ({ teams, projects }) => {
        this.teamsCount.set(teams.length);
        this.projectsCount.set(projects.length);
        this.recentProjects.set(projects.slice(0,4));

        if (projects.length > 0){
          forkJoin(projects.slice(0,3).map(p => this.taskService.getByProjectId(p.id))).subscribe({
            next: (results) => {
              const all = results.flat().slice(0,6);
              this.recentTasks.set(all);
              this.isLoading.set(false);
            },
            error: () => this.isLoading.set(false)
          });
        } else {
          this.isLoading.set(false);
        }
      },
      error: () => this.isLoading.set(false)
    });
  }

  getStatusClass(status: TasksStatus): string {
    const map: Record <TasksStatus, string> = {
      [TasksStatus.Todo]:       'bg-gray-100 text-gray-700',
      [TasksStatus.InProgress]: 'bg-blue-100 text-blue-700',
      [TasksStatus.InReview]:   'bg-yellow-100 text-yellow-700',
      [TasksStatus.Done]:       'bg-green-100 text-green-700'
     };
     return map[status] ?? 'bg-gray-100 text-gray-700';
  }

  getProjectStatusClass(status: number): string {
    const map: Record<number, string> = {
      1: 'bg-gray-100 text-gray-600',
      2: 'bg-blue-100 text-blue-700',
      3: 'bg-yellow-100 text-yellow-700',
      4: 'bg-green-100 text-green-700'
    };
    return map[status] ?? 'bg-gray-100 text-gray-600';
  }

}
