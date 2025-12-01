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
