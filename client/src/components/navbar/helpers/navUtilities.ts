export function addAdminMenuItems(menuItems: any[]) {
  const column1 = menuItems.slice(0, Math.ceil(menuItems.length / 2))
  const column2 = menuItems.slice(Math.ceil(menuItems.length / 2), menuItems.length)

  if (column1.length === 0) {
    return
  }

  const expressionMenu = []

  expressionMenu.push([{
    items: column1,
  }])
  expressionMenu.push([{
    items: column2,
  }])

  return expressionMenu
}

function MapData(data, navMenuHeading: string) {
  return {
    navMenuType: navMenuHeading,
    data: data,
  }
}

export function fillComputedMenu(menuItems: any[], navMenuHeading: string) {
  const column1 = menuItems.slice(0, Math.ceil(menuItems.length / 2))
  const column2 = menuItems.slice(Math.ceil(menuItems.length / 2), menuItems.length)

  const menu = []

  menu.push([{
    items: column1.map(x => MapData(x, navMenuHeading)),
  }])
  menu.push([{
    items: column2.map(x => MapData(x, navMenuHeading)),
  }])

  return menu
}
