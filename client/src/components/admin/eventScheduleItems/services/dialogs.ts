import { useDialog } from 'primevue/usedialog'
import EventScheduledItemList from '@/components/admin/eventScheduleItems/EventScheduledItemList.vue'

export const adminEventScheduleDialogs = () => {
  const dialog = useDialog()

  const showScheduleDialog = (eventId: number, event: Event, isReadOnly: boolean) => {
    dialog.open(EventScheduledItemList, {
      props: {
        header: 'Scheduled Events',
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
        eventId: eventId,
        event: event,
        isReadOnly: isReadOnly,
      },
    })
  }
  return {
    showScheduleDialog,
  }
}
