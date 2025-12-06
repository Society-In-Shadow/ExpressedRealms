import { useConfirm } from 'primevue/useconfirm'
import toaster from '@/services/Toasters'
import axios from 'axios'
import { playerList } from '@/components/admin/players/stores/playerListStore'

export const userConfirmationPopups = (userId: string) => {
  const confirm = useConfirm()
  const playerListStore = playerList()

  const deleteConfirmation = (event: MouseEvent) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: 'Do you want to disable this player?',
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Disable Player',
        severity: 'danger',
      },
      accept: () => {
        axios.put(`/admin/user/${userId}/lockout`,
          {
            userId: userId,
            lockoutEnabled: true,
          })
          .then(async () => {
            await playerListStore.fetchPlayers()
            toaster.success('Successfully Disabled Player!')
          })
      },
    })

  const enableConfirmation = (event: MouseEvent) => {
    confirm.require({
      target: event.currentTarget as HTMLElement,
      group: 'popup',
      message: 'Do you want to enable this player?',
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Enable Player',
        severity: 'warning',
      },
      accept: () => {
        axios.put(`/admin/user/${userId}/lockout`,
          {
            userId: userId,
            lockoutEnabled: false,
          })
          .then(async () => {
            await playerListStore.fetchPlayers()
            toaster.success('Successfully Enabled Player!')
          })
      },
    })
  }

  const unlockConfirmation = (event: MouseEvent) => {
    confirm.require({
      target: event.currentTarget as HTMLElement,
      group: 'popup',
      message: 'Do you want to unlock this player?',
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Unlock Player',
        severity: 'warning',
      },
      accept: () => {
        axios.put(`/admin/user/${userId}/lockout`,
          {
            userId: userId,
            lockoutEnabled: false,
          })
          .then(async () => {
            await playerListStore.fetchPlayers()
            toaster.success('Successfully Unlocked Player!')
          })
      },
    })
  }

  const bypassConfirmation = (event: MouseEvent) => {
    confirm.require({
      target: event.currentTarget as HTMLElement,
      group: 'popup',
      message: 'Make sure you manually verify their email address!  Manually verify by asking them to send you an email from the registered email address with some random sentance.  ONLY THEN APPROVE THIS.',
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'I have Confirmed Their Email Address.',
        severity: 'warn',
      },
      accept: () => {
        axios.post(`/admin/user/${userId}/bypassEmailConfirmation`)
          .then(async () => {
            await playerListStore.fetchPlayers()
            toaster.success('Successfully Bypassed Email for Player!')
          })
      },
    })
  }

  return { deleteConfirmation, enableConfirmation, unlockConfirmation, bypassConfirmation }
}
