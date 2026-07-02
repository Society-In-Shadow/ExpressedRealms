# Dialogs
Dialogs are used to display edit / create behavior in the system.

## Key Features
 - Lazy Loading – useAppDialog will force the caller to use lazy loading till the action is called - saving initial load time
 - Standardized dialog size – Provides a central location to define the size of dialogs

## Location

The main location is [client/utilities/dialogUtilities.ts](./../../client/utilities/dialogUtilities.ts)

## Setup

dailog.ts
```ts

import { useAppDialog } from '@/utilities/dialogUtilities.ts'
import type { AddPowerType } from '@/components/expressions/powers/types.ts'

const AddPower = () => import('@/components/expressions/powers/AddPower.vue')

export const powerDialogs = () => {
  const dialog = useAppDialog()

  return {
    showAddPower: (data: AddPowerType) => dialog.open(AddPower, { header: 'Add Power', data }),
  }
}

```

### Component Usage

```ts
import type { DialogRef } from '@/utilities/dialogUtilities.ts'

const dialogRef = inject('dialogRef') as DialogRef<AddPowerType>

const values = {
  target: dialogRef.value.data.target,
  targetId: dialogRef.value.data.targetId,
}

const reset = () => {
  form.customResetForm()
  dialogRef.value.close({
    action: 'cancelled', // or 'added", etc
  })
}
```

### Caller Usage

```ts
const dialogs = powerDialogs()

const showAddPower = async () => {
  const result = await dialogs.showAddPower({ target: TargetPowerType.PowerPath, targetId: props.powerPathId })

  if (result?.action == 'added') {
    await powerInfo.updatePowersByPathId(props.powerPathId)
  }
}
```
