export interface CommentDto {
  id: string;
  content: string;
  taskId: string;
  userId: string;
  userFullName: string;
  createdAt: string;
}

export interface CreateCommentDto {
  content: string;
  taskId: string;
}