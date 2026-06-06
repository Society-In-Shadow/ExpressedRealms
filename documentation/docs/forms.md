# Forms
Like all apps, we have forms.  In our case, we have custom form wrappers, located in the src/FormWrappers folder.

## Basics
There's an overall FormWrapper, then the following wrappers:

 - FormCheckboxWrapper
 - FormDropdownWrapper
 - FormEditorWrapper
 - FormInputDateOnlyWrapper
 - FormInputNumberWrapper
 - FormInputTextWrapper
 - FormInputTimeOnlyWrapper
 - FormListboxWrapper
 - FormMultiSelectWrapper
 - FormTextAreaWrapper

### Older Versions
There is an older version of the form inputs, from the initial implementation of the form wrappers.  Those need to be removed
and replaced as you are modifying things.

These are identified by the components that don't have "Form" as a prefix. and typically have multiple properties separated
out.  The new process consolidated all of those into a single system to allow for easier maintenance and consistency.

This process will typically include:

 - Reworking Validation Rules on the Front End to follow new norms
 - Replacing the existing form wrappers with the new ones

Notably, avoid also upgrading the stores to pinia colada, unless you are basically reworking the whole section.

You want to keep commits small, light and focused

## Validators and Forms
These Form wrappers are heavily reliant on the use of the generic form class that translates vee-validate into something
these can use.

More info will be provided later on this.

Suffice to say, it's the reason why forms look so simple, as it's handling a bunch of stuff in the background, as shown
below.

```html
<FormWrapper @onsubmit="...">
  <FormInputTextWrapper v-model="form.fields.name"/>
  <FormInputNumberWrapper v-model="form.fields.conExperience"/>
</FormWrapper>
```

## Show Skeleton
All of the fields come with a skeleton loading animation via the show-skeleton field as shown below.

```html
<FormInputTextWrapper v-model="form.fields.name" show-skeleton="true"/>
```
To help simplify the need to not have that on each field, a FormWrapper was added, with a "show-skeleton" property.

This will automatically set that value on all children, and can be used as shown below.

```vue
<script setup lang="ts">

  import { ref } from "vue";
  const showSkeleton = ref(false)

</script>

<template>
  <FormWrapper :show-skeleton="showSkeleton" @onsubmit="...">
    <FormInputTextWrapper v-model="form.fields.name"/>
    <FormInputNumberWrapper v-model="form.fields.conExperience"/>
  </FormWrapper>
</template>
```

## Handling API Validation Errors
This works with the Pinia Colada approach.  

So, you grab the information like normal on line 12, and then use the HydrateFormOnce function to fill in the form values once it's done

Your mutations / update statements will provide a way to tie into any validation errors, it will pass through the set
that came through the API, which can be ported directly into the form.  More information is in the Query section below

```vue linenums="1"
<script setup lang="ts">
  import { getValidationInstance } from '@/components/admin/characterList/validators/characterGoFieldsForm.ts'
  
  import { useQuery } from '@pinia/colada'
  import { goFieldQuery, useUpdateGoFields } from '@/components/admin/characterList/stores/goFieldColada.ts'
  import type { GoFields } from '@/components/admin/characterList/types.ts'
  import { useHydrateFormOnce } from '@/utilities/piniaColadaUtilities.ts'

  const form = getValidationInstance()
  const characterId = 32

  const { data, isPending } = useQuery(goFieldQuery(characterId))
  useHydrateFormOnce(data, form.setValues)

  const updateGoFields = useUpdateGoFields((errors) => {
    form.setErrors(errors)
  })

  const onSubmit = form.handleSubmit(async (values) => {
    await updateGoFields.mutateAsync({
      id: characterId,
      data: values as GoFields,
    })
    
    // Success Actions
    
  })
  
</script>
```

### Query Setup
Pinia Colada is what we are using to keep track of items.

Two key things here in regards to validation:

1. On line 17 - the onValidationError - This allows the vue component to tie the form to the event
2. On line 26 - handleValidationErrors - This is a shared function that will trigger the onValidationError if validation errors exist

```ts linenums="1"
import { defineQuery, useMutation, useQueryCache } from '@pinia/colada'
import { goFieldsService } from '@/components/admin/characterList/services/goFieldsService.ts'
import { handleValidationErrors } from '@/utilities/piniaColadaUtilities.ts'
import type { GoFields } from '@/components/admin/characterList/types.ts'

export const GO_FIELDS_QUERY_KEYS = {
  root: ['goFields'] as const,
}

export const goFieldQuery = (characterId: number) =>
  defineQuery(() => ({
    key: ['goFields', characterId],
    query: () => goFieldsService.getGoFields(characterId),
  }))

export const useUpdateGoFields = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ id, data }: { id: number, data: GoFields }) => goFieldsService.updateGoFields(id, data),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: GO_FIELDS_QUERY_KEYS.root })
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}

```

### Some Notes Here
 - The data being grabbed is cached until the user refreshes their page, this is due to a global setting set for the user information
 - You should avoid using toasts or passing through forms to this level, keep it dedicated to data management.