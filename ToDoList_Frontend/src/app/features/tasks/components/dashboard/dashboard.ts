import { Component, OnInit, ChangeDetectorRef, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, ActivatedRoute } from '@angular/router';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { TaskItem } from '../../../../core/models/TaskItem';
import { TaskService } from '../../../../core/services/task-service';
import { TaskListComponent } from '../task-list/task-list';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    TaskListComponent,
    MatPaginatorModule,
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit, AfterViewInit {

  tasks: TaskItem[] = [];
  isLoading = false;
  errorMessage = '';
  currentFilter = 'all';

  pageNumber = 0;
  pageSize = 3;
  length = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.currentFilter = params['filter'] || 'all';

      this.pageNumber = 0;
      this.pageSize = this.paginator?.pageSize ?? 5;

      if (this.paginator) {
        this.paginator.firstPage();
      }
      this.loadByFilter(1, this.pageSize);
    });
  }

  ngAfterViewInit(): void {
    this.paginator._intl.itemsPerPageLabel = 'Items per page';
    this.cdr.detectChanges();
  }

  loadByFilter(page: number, size: number): void {
    switch (this.currentFilter) {
      case 'pending': this.loadDueTasks(page, size); break;
      case 'completed': this.loadCompletedTasks(page, size); break;
      default: this.loadTasks(page, size); break;
    }
  }

  loadTasks(page: number, size: number): void {
    this.isLoading = true;
    this.taskService.getAll(page, size).subscribe({
      next: (result) => {
        this.tasks = result.items;
        this.length = result.total;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Error loading tasks.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadCompletedTasks(page: number, size: number): void {
    this.isLoading = true;
    this.taskService.getCompletedTasks(page, size).subscribe({
      next: (result) => {
        this.tasks = result.items;
        this.length = result.total;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Error loading completed tasks.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadDueTasks(page: number, size: number): void {
    this.isLoading = true;
    this.taskService.getDueTasks(page, size).subscribe({
      next: (result) => {
        this.tasks = result.items;
        this.length = result.total;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Error loading pending tasks.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  changePage(event: PageEvent): void {
    this.pageNumber = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadByFilter(
      event.pageIndex + 1,
      event.pageSize
    );
  }

  // ── Stats ──────────────────────────────────────
  get totalTasks() { return this.length; }
  get completedTasks() { return this.tasks.filter(t => t.isCompleted).length; }
  get pendingTasks() { return this.tasks.filter(t => !t.isCompleted).length; }
  get overdueTasks() { return this.tasks.filter(t => !t.isCompleted && new Date(t.dueDate) < new Date()).length; }
  get completedPercent() { return this.totalTasks ? (this.completedTasks / this.totalTasks) * 100 : 0; }
  get pendingPercent() { return this.totalTasks ? (this.pendingTasks / this.totalTasks) * 100 : 0; }
  get overduePercent() { return this.totalTasks ? (this.overdueTasks / this.totalTasks) * 100 : 0; }

  // ── Navegación ─────────────────────────────────
  goToNewTask(): void { this.router.navigate(['/tasks/new']); }
  goToEditTask(task: TaskItem): void { this.router.navigate(['/tasks', task.id, 'edit']); }

  // ── Acciones ───────────────────────────────────
  deleteTask(id: number): void {
    this.taskService.deleteTask(id).subscribe({
      next: () => this.loadByFilter(this.pageNumber + 1, this.pageSize),
      error: (err) => console.error(err)
    });
  }

  markComplete(id: number): void {
    this.taskService.markTaskAsCompleted(id).subscribe({
      next: () => this.loadByFilter(this.pageNumber + 1, this.pageSize),
      error: (err) => console.error(err)
    });
  }
}