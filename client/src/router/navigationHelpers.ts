import routerSetup from '@/router'
import type { RouteLocationRaw } from 'vue-router'

export function navigateWithNewTabSupport(route: RouteLocationRaw, event: MouseEvent) {
  const href = routerSetup.resolve(route).href

  if (event.ctrlKey || event.metaKey || event.button === 1) {
    window.open(href, '_blank')
  }
  else {
    routerSetup.push(route)
  }
}
