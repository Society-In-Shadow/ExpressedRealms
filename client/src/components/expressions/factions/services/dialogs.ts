import { useDialog } from 'primevue/usedialog'
import FactionEdit from '@/components/expressions/factions/FactionEdit.vue'
import FactionCreate from '@/components/expressions/factions/FactionCreate.vue'

export const factionDialogs = () => {
  const dialog = useDialog()

  const showUpdateFaction = (factionId: number) => {
    dialog.open(FactionEdit, {
      props: {
        header: 'Update Faction',
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

  const showCreateFaction = () => {
    dialog.open(FactionCreate, {
      props: {
        header: 'Create Faction',
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
      },
    })
  }
  return {
    showUpdateFaction,
    showCreateFaction,
  }
}
