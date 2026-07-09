import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { CommentDto, CreateCommentDto } from "../models/comment/comment.model";
import { Observable, tap } from "rxjs";

@Injectable({ providedIn: 'root' })
export class CommentService {
    private readonly apiUrl = `${environment.apiUrl}/comment`;
    private readonly http = inject(HttpClient);

    readonly comments = signal<CommentDto[]>([]);
    readonly isLoading = signal(false);

    getByTaskId(taskId: string): Observable<CommentDto[]> {
        this.isLoading.set(true);
        return this.http.get<CommentDto[]>(`${this.apiUrl}/task/${taskId}`).pipe(
            tap({
                next: (data) => {
                    this.comments.set(data);
                    this.isLoading.set(false);
                },
                error: () => this.isLoading.set(false)
            })
        );
    }

    create(dto: CreateCommentDto): Observable<CommentDto> {
        return this.http.post<CommentDto>(this.apiUrl, dto).pipe(
            tap((created) => this.comments.update(list => [...list, created]))
        );
    }

    delete(id: string) : Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.comments.update(list => list.filter(c=>c.id !== id)))
        );
    }

    clearComments(): void {
        this.comments.set([]);
    }
}
