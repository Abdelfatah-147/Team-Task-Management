import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { CreatedProjectDto, Project, UpdateProjectDto } from "../models/project/project.model";
import { Observable, tap } from "rxjs";

@Injectable({ providedIn: 'root' })
export class ProjectService {
    private readonly apiUrl = `${environment.apiUrl}/project`;
    private readonly http = inject(HttpClient);

    projects = signal<Project[]>([]);
    isLoading = signal(false);

    getAll(): Observable <Project[]> {
        this.isLoading.set(true);
        return this.http.get<Project[]>(this.apiUrl).pipe(
            tap({
                next: (projects) => {
                    this.projects.set(projects ?? []);
                    this.isLoading.set(false);
                },
                error: () => this.isLoading.set(false)
            })
        );
    }

    getByTeamId(teamId: string): Observable<Project[]> {
        this.isLoading.set(true);
        return this.http.get<Project[]>(`${this.apiUrl}/team/${teamId}`).pipe(
            tap({
                next: (projects) => {
                    this.projects.set(projects ?? []);
                    this.isLoading.set(false);
                },
                error: () => this.isLoading.set(false)
            })
        );
    }


    getById(id: string) : Observable<Project> {
        return this.http.get<Project>(`${this.apiUrl}/${id}`);
    };
    
    create(dto: CreatedProjectDto): Observable<Project> {
        return this.http.post<Project>(this.apiUrl, dto).pipe(
            tap((newProject) => this.projects.update(projects => [...projects, newProject]))
        );
    }

    update(id: string, dto: UpdateProjectDto) : Observable<Project> {
        return this.http.put<Project>(`${this.apiUrl}/${id}`, dto).pipe(
            tap((updated) => this.projects.update(projects => projects.map(p => p.id === id ? updated : p)))
        );
    }


    delete(id: string) : Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.projects.update(projects => projects.filter(p => p.id !== id)))
        );
    }
}

