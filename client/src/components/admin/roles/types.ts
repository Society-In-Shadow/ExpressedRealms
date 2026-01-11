export interface RoleResponse {
  roles: Array<Role>
}

export interface Role {
  id: number
  name: string
  description: string | null
  permissionIds: string[]
}

export interface EditRole {
  id: number
  name: string
  description: string | null
  permissionIds: string[]
}
