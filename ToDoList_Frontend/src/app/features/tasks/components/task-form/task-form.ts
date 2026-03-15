import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../../../core/services/task-service';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css'
})
export class TaskFormComponent implements OnInit {

  taskForm: FormGroup;
  isEditing = false;
  taskId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService
  ) {
    this.taskForm = this.fb.group({
      title:       ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      dueDate:     ['', Validators.required],
    });
  }

  ngOnInit(): void {
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditing = true;
      this.taskId = +id;
      this.loadTask(this.taskId);
    }
  }

  loadTask(id: number): void {
    this.taskService.getById(id).subscribe({
      next: (task) => {
        const formatted = new Date(task.dueDate).toISOString().slice(0, 10);
        this.taskForm.patchValue({
          title:       task.title,
          description: task.description,
          dueDate:     formatted,
        });
      },
      error: (err) => console.error(err)
    });
  }

  hasError(field: string): boolean {
    const ctrl = this.taskForm.get(field);
    return !!(ctrl && ctrl.invalid && ctrl.touched);
  }

  submit(): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    const value = this.taskForm.value;
    const dueDate = new Date(value.dueDate).toISOString();

    if (this.isEditing && this.taskId) {
      this.taskService.updateTask(this.taskId, { ...value, dueDate }).subscribe({
        next: () => this.router.navigate(['/dashboard']),
        error: (err) => console.error(err)
      });
    } else {
      this.taskService.addTask({ ...value, dueDate }).subscribe({
        next: () => this.router.navigate(['/dashboard']),
        error: (err) => console.error(err)
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/dashboard']);  
  }
}