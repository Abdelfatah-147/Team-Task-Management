import { Component, EventEmitter, inject, Input, OnInit, Output, output, signal } from '@angular/core';
import { Team } from '../../../core/models/team/team.model';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TeamService } from '../../../core/services/team.service';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-team-form-modal',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './team-form-modal.component.html',
  styleUrl: './team-form-modal.component.scss',
})
export class TeamFormModalComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly teamService = inject(TeamService);

  @Input() team: Team | null = null
  @Output() close = new EventEmitter<void>();

  isSubmitting = signal(false);
  errorMessage = signal<string | null> (null);
  
  form = this.fb.group({
    TName: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(500)]]
  });

  ngOnInit(): void {
    if(this.team){
      this.form.patchValue({
        TName: this.team.name,
        description:this.team.description ?? ''
      });
    }
  }

  get isEditMode(): boolean {
    return !!this.team;
  }

  onSubmit():void{
    if (this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const dto = {
      name: this.form.value.TName!,
      description:this.form.value.description || undefined
    };

    const request = this.isEditMode ? this.teamService.update(this.team!.id, dto) : this.teamService.create(dto);

    request.subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.close.emit();
      },
      error: (err) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(err.error ?? "Somthing went wrong.");
      }
    });
  }

  onCancel() : void {
    this.close.emit();
  }

}
