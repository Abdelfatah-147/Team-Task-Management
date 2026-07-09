import { CommonModule, DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { ProjectService } from '../../../core/services/project.service';
import { TeamService } from '../../../core/services/team.service';
import { TeamMemberService } from '../../../core/services/team-member.service';
import { UserService } from '../../../core/services/user.service';
import { AuthService } from '../../../core/services/auth.service';
import { TaskItem, TaskPriority, TaskPriorityLabels, TasksStatus, TasksStatusLabels } from '../../../core/models/task/task.model';
import { Project } from '../../../core/models/project/project.model';
import { UserListItem } from '../../../core/models/user/user.model';
import { forkJoin } from 'rxjs';
import { TaskFormModalComponent } from '../task-form-modal/task-form-modal.component';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, DatePipe, RouterModule, TaskFormModalComponent],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss',
})
export class TaskListComponent implements OnInit {
  private readonly taskService = inject(TaskService);
  private readonly projectService = inject(ProjectService);
  private readonly teamService = inject(TeamService);
  private readonly teamMemberService = inject(TeamMemberService);
  private readonly userService = inject(UserService);
  private readonly authService = inject(AuthService);

  readonly TasksStatusLabels = TasksStatusLabels;
  readonly TaskPriorityLabels = TaskPriorityLabels;

  isLoading = signal(true);
  allTasks = signal<TaskItem[]>([]);
  projects = signal<Project[]>([]);
  members = signal<UserListItem[]>([]);
  filterStatus = signal<TasksStatus | null>(null);

  selectedTask = signal<TaskItem | null>(null);
  showEditModal = signal(false);
  taskToEdit = signal<TaskItem | null>(null);
  taskToDeleteId = signal<string | null>(null);
  isDeleting = signal(false);

  readonly canManage = computed(() => {
    const roles = this.authService.roles();
    return roles.includes('Admin') || roles.includes('Manager');
  });

  readonly canDelete = computed(() =>
    this.authService.roles().includes('Admin')
  );

  readonly statusOptions = [
    { value: null, label: 'All' },
    { value: TasksStatus.Todo, label: 'Todo' },
    { value: TasksStatus.InProgress, label: 'In Progress' },
    { value: TasksStatus.InReview, label: 'In Review' },
    { value: TasksStatus.Done, label: 'Done' }
  ];

  readonly filteredTasks = computed(() => {
    const status = this.filterStatus();
    return status === null
      ? this.allTasks()
      : this.allTasks().filter(t => t.status === status);
  });

  ngOnInit(): void {
    this.isLoading.set(true);
    forkJoin({
      projects: this.projectService.getAll(),
      allUsers: this.userService.getAll()
    }).subscribe({
      next: ({ projects, allUsers }) => {
        this.projects.set(projects ?? []);
        this.members.set(allUsers ?? []);

        if (projects.length === 0) {
          this.isLoading.set(false);
          return;
        }

        forkJoin(projects.map(p =>
          this.taskService.getByProjectIdForList(p.id)
        )).subscribe({
          next: (results) => {
            this.allTasks.set(results.flat());
            this.isLoading.set(false);
          },
          error: () => this.isLoading.set(false)
        });
      },
      error: () => this.isLoading.set(false)
    });
  }

  getProjectName(projectId: string): string {
    return this.projects().find(p => p.id === projectId)?.name ?? 'Unknown';
  }

  getMemberName(userId?: string): string {
    if (!userId) return 'Unassigned';
    return this.members().find(m => m.id === userId)?.fullName ?? 'Unknown';
  }

  getProjectIdForTask(task: TaskItem): string {
    return task.projectId;
  }

  setFilter(status: TasksStatus | null): void {
    this.filterStatus.set(status);
  }

  openDrawer(task: TaskItem): void {
    this.selectedTask.set(task);
  }

  closeDrawer(): void {
    this.selectedTask.set(null);
  }

  openEditModal(task: TaskItem): void {
    this.taskToEdit.set(task);
    this.showEditModal.set(true);
    this.closeDrawer();
  }

  closeEditModal(): void {
    this.showEditModal.set(false);
    this.taskToEdit.set(null);
  }

  onTaskSaved(): void {
    this.closeEditModal();
    this.isLoading.set(true);
    forkJoin(
      this.projects().map(p => this.taskService.getByProjectIdForList(p.id))
    ).subscribe({
      next: (results) => {
        this.allTasks.set(results.flat());
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  confirmDelete(taskId: string): void {
    this.taskToDeleteId.set(taskId);
    this.closeDrawer();
  }

  cancelDelete(): void {
    this.taskToDeleteId.set(null);
  }

  executeDelete(): void {
    const id = this.taskToDeleteId();
    if (!id) return;
    this.isDeleting.set(true);
    this.taskService.delete(id).subscribe({
      next: () => {
        this.allTasks.update(list => list.filter(t => t.id !== id));
        this.isDeleting.set(false);
        this.taskToDeleteId.set(null);
      },
      error: () => this.isDeleting.set(false)
    });
  }

  getStatusClass(status: TasksStatus): string {
    const map: Record<TasksStatus, string> = {
      [TasksStatus.Todo]:       'bg-gray-100 text-gray-700',
      [TasksStatus.InProgress]: 'bg-blue-100 text-blue-700',
      [TasksStatus.InReview]:   'bg-yellow-100 text-yellow-700',
      [TasksStatus.Done]:       'bg-green-100 text-green-700'
    };
    return map[status] ?? 'bg-gray-100 text-gray-700';
  }

  getPriorityClass(priority: TaskPriority): string {
    const map: Record<TaskPriority, string> = {
      [TaskPriority.Low]:      'bg-slate-100 text-slate-600',
      [TaskPriority.Medium]:   'bg-orange-100 text-orange-600',
      [TaskPriority.High]:     'bg-red-100 text-red-600',
      [TaskPriority.Critical]: 'bg-red-200 text-red-800 font-semibold'
    };
    return map[priority] ?? 'bg-slate-100 text-slate-600';
  }
}