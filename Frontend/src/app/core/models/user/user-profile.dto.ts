export interface UserProfileDto {
  id: string;
  fullName: string;
  email: string;
  isActive: boolean;
  createdAt: string;
  roles: string[];
}