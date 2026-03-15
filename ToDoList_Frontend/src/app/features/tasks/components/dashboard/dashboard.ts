import { Component, OnInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Router, RouterLink, RouterLinkActive, ActivatedRoute } from '@angular/router';

import { TaskItem } from '../../../../core/models/TaskItem';
import { TaskService } from '../../../../core/services/task-service';
import { TaskListComponent } from '../task-list/task-list';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    TaskListComponent,
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {

  tasks: TaskItem[] = [];
  isLoading = false;
  errorMessage = '';
  currentFilter: string = 'all';

  totalItems  = 0;
  currentPage = 1;
  pageSize    = 10;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.currentFilter = params['filter'] || 'all';
      this.currentPage   = 1;  
      this.loadByFilter();
    });
  }

  loadByFilter(): void {
    switch (this.currentFilter) {
      case 'pending':   this.loadDueTasks();       break;
      case 'completed': this.loadCompletedTasks(); break;
      default:          this.loadTasks();           break;
    }
  }

  loadTasks(): void {
    this.isLoading = true;
    this.taskService.getAll(this.currentPage, this.pageSize).subscribe({
      next: (tasks) => {
        this.tasks      = tasks;
        this.totalItems = tasks.length;
        this.isLoading  = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Error loading tasks.';
        this.isLoading    = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadCompletedTasks(): void {
    this.isLoading = true;
    this.taskService.getCompletedTasks(this.currentPage, this.pageSize).subscribe({
      next: (tasks) => {
        this.tasks      = tasks;
        this.totalItems = tasks.length;
        this.isLoading  = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Error loading completed tasks.';
        this.isLoading    = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadDueTasks(): void {
    this.isLoading = true;
    this.taskService.getDueTasks(this.currentPage, this.pageSize).subscribe({
      next: (tasks) => {
        this.tasks      = tasks;
        this.totalItems = tasks.length;
        this.isLoading  = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Error loading pending tasks.';
        this.isLoading    = false;
        this.cdr.detectChanges();
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize    = event.pageSize;
    this.loadByFilter();
  }

  // ── Stats ──────────────────────────────────────
  get totalTasks()       { return this.tasks.length; }
  get completedTasks()   { return this.tasks.filter(t =>  t.isCompleted).length; }
  get pendingTasks()     { return this.tasks.filter(t => !t.isCompleted).length; }
  get overdueTasks()     { return this.tasks.filter(t => !t.isCompleted && new Date(t.dueDate) < new Date()).length; }
  get completedPercent() { return this.totalTasks ? (this.completedTasks / this.totalTasks) * 100 : 0; }
  get pendingPercent()   { return this.totalTasks ? (this.pendingTasks   / this.totalTasks) * 100 : 0; }
  get overduePercent()   { return this.totalTasks ? (this.overdueTasks   / this.totalTasks) * 100 : 0; }

  // ── Navegación ─────────────────────────────────
  goToNewTask(): void {
    this.router.navigate(['/tasks/new']);
  }

  goToEditTask(task: TaskItem): void {
    this.router.navigate(['/tasks', task.id, 'edit']);
  }

  // ── Acciones ───────────────────────────────────
  deleteTask(id: number): void {
    this.taskService.deleteTask(id).subscribe({
      next: () => this.loadByFilter(),
      error: (err) => console.error(err)
    });
  }

  markComplete(id: number): void {
    this.taskService.markTaskAsCompleted(id).subscribe({
      next: () => this.loadByFilter(),
      error: (err) => console.error(err)
    });
  }


  // ── Pagination ───────────────────────────────────
  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize) || 1;
  }

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadByFilter();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadByFilter();
    }
  }

  goToPage(page: number): void {
    this.currentPage = page;
    this.loadByFilter();
  }

}