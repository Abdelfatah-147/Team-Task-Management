import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../../environments/environment";
import { CreatedTaskDto, TaskItem, UpdateTaskDto } from "../models/task/task.model";
import { HttpClient } from "@angular/common/http";
import { Observable, tap } from "rxjs";
import { UpdateProjectDto } from "../models/project/project.model";

@Injectable({providedIn: 'root'})
export class TaskService {
    private readonly apiUrl = `${environment.apiUrl}/task`;
    private readonly http = inject(HttpClient);
    readonly tasks = signal<TaskItem[]>([]);
    readonly isLoading = signal(false);
    readonly allTasks = signal<TaskItem[]>([]);
    readonly isLoadingAll = signal(false);
    

    getByProjectId(projectId: string): Observable<TaskItem[]> {
        this.isLoading.set(true);
        return this.http.get<TaskItem[]>(`${this.apiUrl}/project/${projectId}`).pipe(
            tap({
                next: (data) => {
                    this.tasks.set(data);
                    this.isLoading.set(false);
                },
                error: () => this.isLoading.set(false)
            })
        );
    }
   
    getByProjectIdForList(projectId: string): Observable<TaskItem[]> {
        return this.http.get<TaskItem[]>(`${this.apiUrl}/project/${projectId}`);
    }

    getById(id:string) : Observable<TaskItem> {
        return this.http.get<TaskItem>(`${this.apiUrl}/${id}`);
    }

    create(dto:CreatedTaskDto):Observable<TaskItem> {
        return this.http.post<TaskItem>(this.apiUrl, dto).pipe(
            tap((created) => this.tasks.update(list => [...list, created]))
        );
    }

    update(id: string, dto: UpdateTaskDto) : Observable<TaskItem> {
        return this.http.put<TaskItem>(`${this.apiUrl}/${id}`, dto).pipe(
            tap((updated) => this.tasks.update(list => list.map(t => t.id === id ? updated : t)))
        );
    }

    delete(id:string) : Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.tasks.update(list => list.filter(t=>t.id !==id)))
        );
    }

    clearTasks() : void {
        this.tasks.set([])
    }

}