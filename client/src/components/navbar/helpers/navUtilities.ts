export function addRootMenuAndChildren(rootName: string, rootIcon: string, menuItems: any[]) {
  const column1 = menuItems.slice(0, Math.ceil(menuItems.length / 2))
  const column2 = menuItems.slice(Math.ceil(menuItems.length / 2), menuItems.length)

  if (column1.length === 0) {
    return
  }

  const rootMenu = {
    root: true,
    label: rootName,
    icon: rootIcon,
    items: [],
  }

  const expressionMenu = rootMenu.items

  if (expressionMenu !== undefined) {
    expressionMenu.push([{
      items: column1,
    }])
    expressionMenu.push([{
      items: column2,
    }])
  }

  return rootMenu
}
