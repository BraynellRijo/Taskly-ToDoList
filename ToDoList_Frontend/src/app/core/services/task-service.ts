import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskItemDTO } from '../models/TaskItemDTO';
import { TaskItem } from '../models/TaskItem';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private apiURL: string  = environment.apiURl;
  private taskURL: string = '/Task';
  private fullURL: string = this.apiURL + this.taskURL;

  constructor(private http: HttpClient) {}

  getAll(page: number = 1, pageSize: number = 10): Observable<TaskItem[]> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<TaskItem[]>(this.fullURL, { params });
  }

  getById(id: number): Observable<TaskItem> {
    return this.http.get<TaskItem>(`${this.fullURL}/${id}`);
  }

  addTask(task: TaskItemDTO): Observable<void> {
    return this.http.post<void>(this.fullURL, task);
  }

  updateTask(id: number, task: TaskItemDTO): Observable<void> {
    return this.http.put<void>(`${this.fullURL}/${id}`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.fullURL}/${id}`);
  }

  markTaskAsCompleted(id: number): Observable<void> {
    return this.http.patch<void>(`${this.fullURL}/${id}`, {});
  }

  getCompletedTasks(page: number = 1, pageSize: number = 10): Observable<TaskItem[]> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<TaskItem[]>(`${this.fullURL}/completed`, { params });
  }


  getDueTasks(page: number = 1, pageSize: number = 10): Observable<TaskItem[]> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<TaskItem[]>(`${this.fullURL}/due`, { params });
  }

  getByStatus(isCompleted: boolean, page: number = 1, pageSize: number = 10): Observable<TaskItem[]> {
    const params = new HttpParams()
      .set('isCompleted', isCompleted)
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<TaskItem[]>(`${this.fullURL}/status`, { params });
  }
}