import { CommonModule } from '@angular/common';
import { Component, computed, EventEmitter, inject, input, Input, OnInit, output, Output, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreatedTaskDto, TaskItem, TaskPriority, TaskPriorityLabels, TasksStatus, TasksStatusLabels, UpdateTaskDto } from '../../../core/models/task/task.model';
import { UserListItem } from '../../../core/models/user/user.model';
import { TaskService } from '../../../core/services/task.service';


@Component({
  selector: 'app-task-form-modal',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form-modal.component.html',
  styleUrl: './task-form-modal.component.scss',
})
export class TaskFormModalComponent implements OnInit{
  projectId = input.required<string>();
  taskToEdit = input<TaskItem | null>(null);
  members = input<UserListItem[]>([]);
  modalClosed = output<void>();
  taskSaved = output<void>();


  private readonly fb = inject(FormBuilder);
  private readonly taskService = inject(TaskService);

  readonly isSubmitting = signal(false);
  readonly errorMessage = signal('');

  readonly isEditMode = computed(() => !!this.taskToEdit());
  
  readonly statusOptions = Object.values(TasksStatus).filter((v): v is TasksStatus => typeof v === 'number').map(v => ({ value: v, label:TasksStatusLabels[v]}));

  readonly priorityOptions = Object.values(TaskPriority).filter((v): v is TaskPriority => typeof v === 'number').map(v => ({value: v, label: TaskPriorityLabels[v]}));

  form = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(2)]],
    description: ['', Validators.maxLength(700)],
    assignedToUserId: [''],
    status: [TasksStatus.Todo as TasksStatus, Validators.required],
    priority: [TaskPriority.Medium as TaskPriority, Validators.required],
    dueDate: ['']
  });

  ngOnInit(): void {
    const task = this.taskToEdit();
    if(task) {
      this.form.patchValue({
        title: task.title,
        description: task.description ?? '',
        assignedToUserId: task.assignedToUserId ?? '',
        status: task.status,
        priority: task.priority,
        dueDate: task.dueDate ? task.dueDate.substring(0,10) : ''
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');
    const raw = this.form.getRawValue();

    if(this.isEditMode()) {
      const dto: UpdateTaskDto = {
        title: raw.title!,
        description: raw.description || undefined,
        assignedToUserId: raw.assignedToUserId || undefined,
        status: Number(raw.status) as TasksStatus,
        priority: Number(raw.priority) as TaskPriority,
        dueDate: raw.dueDate || undefined
      };

      this.taskService.update(this.taskToEdit()!.id, dto).subscribe({
        next: () => {this.isSubmitting.set(false); this.taskSaved.emit();},
        error: (err) => {this.isSubmitting.set(false); this.errorMessage.set(err?.error || 'Failed to update task.');}
      });
    } else {
      const dto: CreatedTaskDto = {
        title: raw.title!,
        description: raw.description || undefined,
        projectId: this.projectId(),
        assignedToUserId: raw.assignedToUserId || undefined,
        priority: Number(raw.priority) as TaskPriority,
        dueDate: raw.dueDate || undefined
      };

      this.taskService.create(dto).subscribe({
        next: () => {this.isSubmitting.set(false); this.taskSaved.emit();},
        error: (err) => {this.isSubmitting.set(false); this.errorMessage.set(err?.error || 'Failed to create task.')}
      });
    }
  }

  onClose() : void {
    this.modalClosed.emit();
  }

}
