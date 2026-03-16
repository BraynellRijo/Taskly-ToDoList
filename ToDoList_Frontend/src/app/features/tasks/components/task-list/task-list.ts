import { Component, Input, Output, EventEmitter, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskItem } from '../../../../core/models/TaskItem';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskListComponent implements OnInit, OnChanges {
  @Input() tasks: TaskItem[] = [];
  @Input() activeFilter: string = 'all';  
  @Output() onEdit     = new EventEmitter<TaskItem>();
  @Output() onDelete   = new EventEmitter<number>();
  @Output() onComplete = new EventEmitter<number>();

  filteredTasks: TaskItem[] = [];

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.filteredTasks = [...this.tasks];
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['tasks'] || changes['activeFilter']) {
      this.filteredTasks = [...this.tasks];
    }
  }

  setFilter(filter: string): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { filter: filter === 'all' ? null : filter },
      queryParamsHandling: 'merge'
    });
  }

  isOverdue(task: TaskItem): boolean {
    return !task.isCompleted && new Date(task.dueDate) < new Date();
  }

  formatDate(dateStr: string): string {
    if (!dateStr) return 'No date';
    return new Date(dateStr).toLocaleDateString('en-US', {
      month: 'short', day: 'numeric', year: 'numeric'
    });
  }

  edit(task: TaskItem):  void { this.onEdit.emit(task); }
  delete(id: number):    void { this.onDelete.emit(id); }
  complete(id: number):  void { this.onComplete.emit(id); }
}