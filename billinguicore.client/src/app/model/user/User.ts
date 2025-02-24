export class User {
  userId: number;
  userName: string;
  email: string;
  roleName: string;
  roleId: number;
  permissions: Permission[];

  constructor(userId: number, userName: string, email: string, roleName: string, roleId: number, permissions: Permission[]) {
    this.userId = userId;
    this.userName = userName;
    this.email = email;
    this.roleName = roleName;
    this.roleId = roleId;
    this.permissions = permissions;
  }
}

export class Permission {
  canRead: boolean;
  canWrite: boolean;
  moduleName: string;

  constructor(canRead: boolean, canWrite: boolean, moduleName: string) {
    this.canRead = canRead;
    this.canWrite = canWrite;
    this.moduleName = moduleName;
  }
}

export interface UserRequest {
  userName: string;
  email: string;
  role: number;
}

export interface UserSearchResponse {
  data: Array<User>,
  totalCount: number
}

export class UserRole {
  roleId: number;
  roleName: string;

  constructor(roleId: number, roleName: string) {
    this.roleName = roleName;
    this.roleId = roleId;
  }
}

export interface UserRoleResponse {
  data: Array<UserRole>
}
