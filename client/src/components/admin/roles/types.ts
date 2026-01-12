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

export interface Permission {
  id: string // UUID
  name: string
  description: string | null
}

export interface Resource {
  id?: string // UUID
  name: string
  description: string | null
  permissions: Permission[]
}

export interface ResourceResponse {
  resources: Resource[]
}
