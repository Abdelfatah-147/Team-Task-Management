import { Component, computed, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { ProjectService } from '../../../core/services/project.service';
import { TeamMemberService } from '../../../core/services/team-member.service';
import { UserService } from '../../../core/services/user.service';
import { AuthService } from '../../../core/services/auth.service';
import {
  TaskItem, TasksStatus, TaskPriority,
  TasksStatusLabels, TaskPriorityLabels
} from '../../../core/models/task/task.model';
import { Project } from '../../../core/models/project/project.model';
import { UserListItem } from '../../../core/models/user/user.model';
import { TaskFormModalComponent } from '../../tasks/task-form-modal/task-form-modal.component';
import { CommentsSectionComponent } from '../../comments/comments-section-component/comments-section.component';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [CommonModule, DatePipe, RouterModule, TaskFormModalComponent, CommentsSectionComponent],
  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.scss'
})
export class ProjectDetailsComponent implements OnInit {
  // Services
  private readonly route = inject(ActivatedRoute);
  private readonly taskService = inject(TaskService);
  private readonly projectService = inject(ProjectService);
  private readonly teamMemberService = inject(TeamMemberService);
  private readonly userService = inject(UserService);
  private readonly authService = inject(AuthService);

  // Expose to template
  readonly TasksStatusLabels = TasksStatusLabels;
  readonly TaskPriorityLabels = TaskPriorityLabels;
  readonly TasksStatus = TasksStatus;
  readonly TaskPriority = TaskPriority;
  readonly statusOrder = [
    TasksStatus.Todo,
    TasksStatus.InProgress,
    TasksStatus.InReview,
    TasksStatus.Done
  ];

  projectId!: string;

  readonly project = signal<Project | null>(null);
  readonly members = signal<UserListItem[]>([]);
  readonly isLoadingProject = signal(true);
  readonly selectedTask = signal<TaskItem | null>(null);

  readonly showTaskModal = signal(false);
  readonly taskToEdit = signal<TaskItem | null>(null);
  readonly taskToDeleteId = signal<string | null>(null);
  readonly isDeleting = signal(false);

  readonly tasks = this.taskService.tasks;
  readonly isLoadingTasks = this.taskService.isLoading;

  readonly canManage = computed(() => {
    const roles = this.authService.roles();
    return roles.includes('Admin') || roles.includes('Manager');
  });

  readonly canDelete = computed(() =>
    this.authService.roles().includes('Admin')
  );

  readonly groupedByStatus = computed(() => {
    const grouped: Record<number, TaskItem[]> = {};
    for (const s of this.statusOrder) {
      grouped[s] = this.tasks().filter(t => t.status === s);
    }
    return grouped;
  });

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('id')!;
    this.taskService.clearTasks();
    this.loadProject();
    this.loadTasks();
  }

  

  private loadProject(): void {
    this.isLoadingProject.set(true);
    this.projectService.getById(this.projectId).subscribe({
      next: (project) => {
        this.project.set(project);
        this.isLoadingProject.set(false);
        this.loadMembers(project.teamId);
      },
      error: () => this.isLoadingProject.set(false)
    });
  }

  private loadTasks(): void {
    this.taskService.getByProjectId(this.projectId).subscribe({
      next: (tasks) => console.log('tasks loaded:', tasks),
      error: (err) => console.error('tasks error:', err)
    });
  }

  private loadMembers(teamId: string): void {
    this.userService.getAll().subscribe({
      next: (allUsers) => {
        this.members.set(allUsers ?? []);
      },
      error: () => {
        this.members.set([]);
      }
    });
  }

  getMemberName(userId?: string): string {
    if (!userId) return 'Unassigned';
    return this.members().find(m => m.id === userId)?.fullName ?? 'Unknown';
  }

  openCreateModal(): void {
    this.taskToEdit.set(null);
    this.showTaskModal.set(true);
  }

  openEditModal(task: TaskItem): void {
    this.taskToEdit.set(task);
    this.showTaskModal.set(true);
  }

  closeModal(): void {
    this.showTaskModal.set(false);
    this.taskToEdit.set(null);
  }

  onTaskSaved(): void {
    this.closeModal();
    this.loadTasks();
    if (this.selectedTask()) {
      const updatedId = this.taskToEdit()?.id;
      if (updatedId) {
        this.taskService.getById(updatedId).subscribe({
          next: (task) => this.selectedTask.set(task)
        });
      }
    }
  }

  confirmDelete(taskId: string): void {
    this.taskToDeleteId.set(taskId);
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
        this.isDeleting.set(false);
        this.taskToDeleteId.set(null);
      },
      error: () => this.isDeleting.set(false)
    });
  }

  openTaskDrawer(task: TaskItem): void {
    this.selectedTask.set(task);
  }

  closeTaskDrawer(): void {
    this.selectedTask.set(null);
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