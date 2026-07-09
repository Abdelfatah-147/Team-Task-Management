import { CommonModule, DatePipe } from '@angular/common';
import { Component, computed, inject, input, OnChanges, OnDestroy, signal, SimpleChanges } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommentService } from '../../../core/services/comment.service';
import { AuthService } from '../../../core/services/auth.service';
import { CommentDto } from '../../../core/models/comment/comment.model';

@Component({
  selector: 'app-comments-section',
  imports: [CommonModule, DatePipe, ReactiveFormsModule],
  templateUrl: './comments-section.component.html',
  styleUrl: './comments-section.component.scss',
})
export class CommentsSectionComponent implements OnChanges, OnDestroy {
  taskId= input.required<string>();

  private readonly commentService = inject(CommentService);
  private readonly authService = inject(AuthService);
  private readonly fb = inject(FormBuilder);
  
  readonly comments = this.commentService.comments;
  readonly isLoading = this.commentService.isLoading;
  readonly isSubmitting = signal(false);
  readonly errorMessage = signal('');
  readonly commentToDeleteId = signal<string | null>(null);
  readonly isDeleting = signal(false);

  private readonly currentUser = this.authService.currentUser;

  readonly canManage = computed(() => {
    const roles = this.authService.roles();
    return roles.includes('Admin') || roles.includes('Manager');
  });

  form = this.fb.group({
    content: ['', [Validators.required, Validators.minLength(1)]]
  });

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['taskId'] && this.taskId()) {
      this.loadComments();
    }
  }

  ngOnDestroy(): void {
    this.commentService.clearComments();
  }

  private loadComments(): void {
    this.commentService.getByTaskId(this.taskId()).subscribe();
  }

  isOwner(comment: CommentDto): boolean {
    return comment.userId === this.currentUser()?.id;
  }

  canDelete(comment: CommentDto): boolean {
    return this.isOwner(comment) || this.canManage();
  }

  onSubmit(): void {
    if(this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    this.commentService.create({
      content: this.form.getRawValue().content!,
      taskId: this.taskId()
    }).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.form.reset();
      },
      error: (err) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(err?.error || 'Failed to post comment.');
      }
    });
  }

  confirmDelete(commentId: string): void {
    this.commentToDeleteId.set(commentId);
  }

  cancelDelete(): void {
    this.commentToDeleteId.set(null);
  }

  executeDelete(): void {
    const id = this.commentToDeleteId();
    if(!id) return;
    this.isDeleting.set(true);
    this.commentService.delete(id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.commentToDeleteId.set(null);
      },
      error: () => this.isDeleting.set(false)
    });
  }
}
