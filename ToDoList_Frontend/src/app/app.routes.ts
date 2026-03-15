import { Routes } from '@angular/router';
import { DashboardComponent } from './features/tasks/components/dashboard/dashboard';
import { TaskFormComponent } from './features/tasks/components/task-form/task-form';

export const routes: Routes = [
{
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
  },
  {
    path: 'tasks/new',          
    component: TaskFormComponent,
  },
  {
    path: 'tasks/:id/edit',     
    component: TaskFormComponent,
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
