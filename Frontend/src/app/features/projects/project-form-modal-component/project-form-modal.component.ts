import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectService } from '../../../core/services/project.service';
import { Project, ProjectStatus, ProjectStatusLabels } from '../../../core/models/project/project.model';
import { Team } from '../../../core/models/team/team.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-project-form-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './project-form-modal.component.html',
  styleUrl: './project-form-modal.component.scss',
})
export class ProjectFormModalComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly projectService = inject(ProjectService);

  @Input() teamId!: string;
  @Input() project: Project | null = null;
  @Input() teams: Team[] = []; 
  @Output() close = new EventEmitter<void>();

  isSubmitting = signal(false);
  errorMessage = signal<string | null>(null);

  statusOptions = Object.entries(ProjectStatusLabels).map(([value, label]) => ({
    value: Number(value) as ProjectStatus,
    label
  }));

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(700)]],
    status: [ProjectStatus.NotStarted, [Validators.required]],
    startDate: [''],
    endDate: [''],
    teamId: ['']
  });

  ngOnInit(): void {
    if (this.project) {
      this.form.patchValue({
        name: this.project.name,
        description: this.project.description ?? '',
        status: this.project.status,
        startDate: this.project.startDate?.substring(0, 10) ?? '',
        endDate: this.project.endDate?.substring(0, 10) ?? '',
        teamId: this.project.teamId
      });
    } else {
      
      if (this.teamId) {
        this.form.patchValue({ teamId: this.teamId });
      }
      
      if (this.teams.length > 0) {
        this.form.get('teamId')!.setValidators([Validators.required]);
        this.form.get('teamId')!.updateValueAndValidity();
      }
    }
  }

  get isEditMode(): boolean {
    return !!this.project;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const raw = this.form.value;
    const startDate = raw.startDate || undefined;
    const endDate = raw.endDate || undefined;
    const resolvedTeamId = this.teamId || raw.teamId!;

    const request = this.isEditMode
      ? this.projectService.update(this.project!.id, {
          name: raw.name!,
          description: raw.description || undefined,
          status: Number(raw.status) as ProjectStatus,
          startDate,
          endDate
        })
      : this.projectService.create({
          name: raw.name!,
          description: raw.description || undefined,
          teamId: resolvedTeamId,
          startDate,
          endDate
        });

    request.subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.close.emit();
      },
      error: (err) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(err.error ?? 'Something went wrong.');
      }
    });
  }

  onCancel(): void {
    this.close.emit();
  }
}