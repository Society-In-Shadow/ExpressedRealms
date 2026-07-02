import { useDialog } from 'primevue/usedialog'
import { type Component, type Ref } from 'vue'

type ComponentLoader = () => Promise<{ default: Component }>

export const useAppDialog = () => {
  const dialog = useDialog()

  const open = async <TData, TResult = any>(
    loader: ComponentLoader,
    options: {
      header: string
      data: TData
    },
  ): Promise<TResult> => {
    const component = (await loader()).default
    return new Promise((resolve) => {
      dialog.open(component, {
        props: {
          header: options.header,
          style: {
            width: '500px',
          },
          breakpoints: {
            '960px': '75vw',
            '640px': '90vw',
          },
          modal: true,
        },
        data: options.data,

        onClose: (opts: any) => {
          resolve(opts?.data)
        },
      })
    })
  }

  return { open }
}

export type DialogRef<TData = any, TResult = any> = Ref<{
  data: TData
  close: (result?: TResult) => void
}>
