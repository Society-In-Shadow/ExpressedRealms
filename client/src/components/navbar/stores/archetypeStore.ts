import { defineQueryOptions } from '@pinia/colada'
import { navMenuService } from '@/components/navbar/services/navmenuServices.ts'

export const NAV_MENU_QUERY_KEYS = {
  root: ['nav_menu'] as const,
  characters: ['nav_menu', 'characters'] as const,
}

export const characterListQuery = defineQueryOptions({
  key: NAV_MENU_QUERY_KEYS.characters,
  query: navMenuService.getNavMenuCharacters,
})
