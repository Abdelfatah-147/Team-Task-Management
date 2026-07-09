export enum TasksStatus {
    Todo = 1,
    InProgress = 2,
    InReview = 3,
    Done = 4,
}

export enum TaskPriority {
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

export const TasksStatusLabels : Record<TasksStatus, string> = {
    [TasksStatus.Todo]: 'Todo',
    [TasksStatus.InProgress]: 'In Progress',
    [TasksStatus.InReview]: 'In Review',
    [TasksStatus.Done]: 'Done'
};

export const TaskPriorityLabels : Record<TaskPriority, string> = {
    [TaskPriority.Low]: 'Low',
    [TaskPriority.Medium]: 'Medium',
    [TaskPriority.High]: 'High',
    [TaskPriority.Critical]: 'Critical'
};


export interface TaskItem {
    id: string;
    title: string;
    description?: string;
    projectId: string;
    assignedToUserId?: string;
    status: TasksStatus;
    priority: TaskPriority;
    dueDate?: string;
    createdAt: string;
}


export interface CreatedTaskDto {
    title: string;
    description?: string;
    projectId: string;
    assignedToUserId?: string;
    priority: TaskPriority;
    dueDate?: string;
}


export interface UpdateTaskDto {
    title: string;
    description?: string;
    assignedToUserId?:string;
    status: TasksStatus;
    priority: TaskPriority;
    dueDate?: string;
}