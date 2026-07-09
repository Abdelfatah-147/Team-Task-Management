import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { TeamService } from '../../../core/services/team.service';
import { TeamMemberService } from '../../../core/services/team-member.service';
import { UserService } from '../../../core/services/user.service';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';
import { Team } from '../../../core/models/team/team.model';
import { TeamMember } from '../../../core/models/team/team-member.model';
import { UserListItem } from '../../../core/models/user/user.model';
import { Project } from '../../../core/models/project/project.model';
import { ProjectFormModalComponent } from '../../projects/project-form-modal-component/project-form-modal.component';

@Component({
  selector: 'app-team-details',
  standalone: true,
  imports: [RouterLink, DatePipe, ProjectFormModalComponent],
  templateUrl: './team-details.component.html',
  styleUrl: './team-details.component.scss'
})
export class TeamDetailsComponent implements OnInit {
  team = signal<Team | null>(null);
  members = signal<TeamMember[]>([]);
  allUsers = signal<UserListItem[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);
  selectedUserId = signal('');

  // --- Projects ---
  projects = signal<Project[]>([]);
  isProjectModalOpen = signal(false);
  editingProject = signal<Project | null>(null);

  constructor(
    private route: ActivatedRoute,
    private teamService: TeamService,
    private teamMemberService: TeamMemberService,
    private userService: UserService,
    private projectService: ProjectService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    const teamId = this.route.snapshot.paramMap.get('id');
    if (!teamId) return;

    this.teamService.getById(teamId).subscribe({
      next: (team) => {
        this.team.set(team);
        this.loadMembers(teamId);
        this.loadProjects(teamId);
        if (this.canManageMembers) {
          this.loadUsers();
        }
      },
      error: () => {
        this.errorMessage.set('Team not found.');
        this.isLoading.set(false);
      }
    });
  }

  loadMembers(teamId: string): void {
    this.teamMemberService.getByTeamId(teamId).subscribe({
      next: (members) => {
        this.members.set(members);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  loadUsers(): void {
    // Admin/Manager only endpoint - safe to call here since canManageMembers already checked
    this.userService.getAll().subscribe({
      next: (users) => this.allUsers.set(users),
      error: () => this.allUsers.set([])
    });
  }

  // Resolves a member's display name from the loaded users list (Admin/Manager only)
  getUserDisplay(userId: string): string {
    const user = this.allUsers().find(u => u.id === userId);
    return user ? `${user.fullName} (${user.email})` : userId;
  }

  // Users not already in the team, for the "Add Member" dropdown
  get availableUsers(): UserListItem[] {
    const memberIds = new Set(this.members().map(m => m.userId));
    return this.allUsers().filter(u => !memberIds.has(u.id));
  }

  get canManageMembers(): boolean {
    const roles = this.authService.roles();
    return roles.includes('Admin') || roles.includes('Manager');
  }

  addMember(): void {
    const teamId = this.team()?.id;
    const userId = this.selectedUserId();
    if (!teamId || !userId) return;

    this.teamMemberService.addMember(teamId, userId).subscribe({
      next: () => {
        this.loadMembers(teamId);
        this.selectedUserId.set('');
      },
      error: (err) => this.errorMessage.set(err.error ?? 'Failed to add member.')
    });
  }

  removeMember(userId: string): void {
    const teamId = this.team()?.id;
    if (!teamId) return;

    this.teamMemberService.removeMember(teamId, userId).subscribe({
      next: () => this.loadMembers(teamId),
      error: (err) => this.errorMessage.set(err.error ?? 'Failed to remove member.')
    });
  }

  // --- Projects ---

  loadProjects(teamId: string): void {
    this.projectService.getByTeamId(teamId).subscribe({
      next: (projects) => this.projects.set(projects),
      error: () => this.projects.set([])
    });
  }

  openCreateProjectModal(): void {
    this.editingProject.set(null);
    this.isProjectModalOpen.set(true);
  }

  openEditProjectModal(project: Project): void {
    this.editingProject.set(project);
    this.isProjectModalOpen.set(true);
  }

  closeProjectModal(): void {
    this.isProjectModalOpen.set(false);
    this.editingProject.set(null);
    const teamId = this.team()?.id;
    if (teamId) this.loadProjects(teamId);
  }
}