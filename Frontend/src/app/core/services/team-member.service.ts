import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TeamMember } from '../models/team/team-member.model';

@Injectable({ providedIn: 'root' })
export class TeamMemberService {
  private readonly apiUrl = `${environment.apiUrl}/TeamMember`;

  constructor(private http: HttpClient) {}

  getByTeamId(teamId: string): Observable<TeamMember[]> {
    return this.http.get<TeamMember[]>(`${this.apiUrl}/${teamId}`);
  }

  addMember(teamId: string, userId: string): Observable<TeamMember> {
    return this.http.post<TeamMember>(`${this.apiUrl}`, null, {
      params: { teamId, userId }
    });
  }

  removeMember(teamId: string, userId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}`, {
      params: { teamId, userId }
    });
  }
}