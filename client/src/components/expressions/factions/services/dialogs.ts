import { useDialog } from 'primevue/usedialog'
import FactionEdit from '@/components/expressions/factions/FactionEdit.vue'

export const factionDialogs = () => {
  const dialog = useDialog()

  const showUpdateFaction = (factionId: number) => {
    dialog.open(FactionEdit, {
      props: {
        header: 'Update Faction Fields',
        style: {
          width: '500px',
        },
        breakpoints: {
          '960px': '75vw',
          '640px': '90vw',
        },
        modal: true,
      },
      data: {
        factionId: factionId,
      },
    })
  }
  return {
    showUpdateFaction,
  }
}
