export enum ProjectStatus {
    NotStarted = 1,
    InProgress = 2,
    OnHold = 3,
    Completed = 4
}


export interface Project {
    id: string;
    name: string;
    description?: string;
    teamId: string;
    status: ProjectStatus;
    startDate?: string;
    endDate?: string;
    createdAt: string;
}


export interface CreatedProjectDto {
    name: string;
    description?: string;
    teamId: string;
    startDate?: string;
    endDate?: string;
}

export interface UpdateProjectDto {
    name: string;
    description?: string;
    status: ProjectStatus;
    startDate?: string;
    endDate?: string;
}

export const ProjectStatusLabels: Record<ProjectStatus, string> = {
    [ProjectStatus.NotStarted]: 'Not Started',
    [ProjectStatus.InProgress]: 'In Progress',
    [ProjectStatus.OnHold]: 'On Hold',
    [ProjectStatus.Completed]: 'Completed'
};

