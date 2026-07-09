import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { AuthService } from '../../../core/services/auth.service';
import { Team } from '../../../core/models/team/team.model';
import { TeamFormModalComponent } from '../team-form-modal/team-form-modal.component';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-team-list',
  standalone: true,
  imports: [RouterLink, TeamFormModalComponent, DatePipe],
  templateUrl: './team-list.component.html',
  styleUrl: './team-list.component.scss'
})
export class TeamListComponent implements OnInit {
  isModalOpen = signal(false);
  editingTeam = signal<Team | null>(null);
  deletingTeamId = signal<string | null>(null);

  public teamService = inject(TeamService);
  public authService = inject(AuthService);

  ngOnInit(): void {
    this.teamService.getAll().subscribe();
  }

  get canManage(): boolean {
    const roles = this.authService.roles();
    return roles.includes('Admin') || roles.includes('Manager');
  }

  get canDelete(): boolean {
    return this.authService.roles().includes('Admin');
  }

  openCreateModal(): void {
    this.editingTeam.set(null);
    this.isModalOpen.set(true);
  }

  openEditModal(team: Team): void {
    this.editingTeam.set(team);
    this.isModalOpen.set(true);
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.editingTeam.set(null);
  }

  confirmDelete(team: Team): void {
    this.deletingTeamId.set(team.id);
  }

  cancelDelete(): void {
    this.deletingTeamId.set(null);
  }

  deleteTeam(id: string): void {
    this.teamService.delete(id).subscribe({
      next: () => this.deletingTeamId.set(null),
      error: () => this.deletingTeamId.set(null)
    });
  }
}