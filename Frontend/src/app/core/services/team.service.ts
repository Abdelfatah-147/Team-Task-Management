import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Team, CreateTeamDto, UpdateTeamDto } from '../models/team/team.model';

@Injectable({ providedIn: 'root' })
export class TeamService {
  private readonly apiUrl = `${environment.apiUrl}/team`;

  teams = signal<Team[]>([]);
  isLoading = signal(false);

  constructor(private http: HttpClient) {}

  getAll(): Observable<Team[]> {
    this.isLoading.set(true);
    return this.http.get<Team[]>(this.apiUrl).pipe(
      tap({
        next: (teams) => {
          this.teams.set(teams);
          this.isLoading.set(false);
        },
        error: () => this.isLoading.set(false)
      })
    );
  }

  getById(id: string): Observable<Team> {
    return this.http.get<Team>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateTeamDto): Observable<Team> {
    return this.http.post<Team>(this.apiUrl, dto).pipe(
      tap((newTeam) => this.teams.update(teams => [...teams, newTeam]))
    );
  }

  update(id: string, dto: UpdateTeamDto): Observable<Team> {
    return this.http.put<Team>(`${this.apiUrl}/${id}`, dto).pipe(
      tap((updated) => this.teams.update(teams =>
        teams.map(t => t.id === id ? updated : t)
      ))
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => this.teams.update(teams => teams.filter(t => t.id !== id)))
    );
  }
}