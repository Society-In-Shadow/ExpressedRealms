import type { Directive } from 'vue'
import { userStore } from '@/stores/userStore.ts'
import type { UserPermission } from '@/types/UserPermissions.ts'

export const permissionDirective: Directive<
  HTMLElement,
    UserPermission | readonly UserPermission[]
> = {
  mounted(el, binding) {
    const authStore = userStore()

    if (!authStore.userPermissions.includes(binding.value as UserPermission)) {
      el.remove()
    }
  },
}
