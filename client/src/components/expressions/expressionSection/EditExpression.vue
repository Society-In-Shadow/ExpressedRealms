<script setup lang="ts">

import { inject, onMounted, ref } from 'vue'
import toaster from '@/services/Toasters'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import { getValidationInstance } from '@/components/expressions/expressionSection/validators/editExpressionValidation.ts'
import type { EditExpressionSection, EditSectionType } from '@/components/expressions/expressionSection/types.ts'
import type { DialogRef } from '@/utilities/dialogUtilities.ts'
import axios from 'axios'
import { expressionStore } from '@/stores/expressionStore.ts'

import { useConfirm } from 'primevue/useconfirm'
import type { ListItem } from '@/types/ListItem.ts'
import FormEditorWrapper from '@/FormWrappers/FormEditorWrapper.vue'

const confirm = useConfirm()
const expressionInfo = expressionStore()
const dialogRef = inject('dialogRef') as DialogRef<EditSectionType>

const sectionTypeOptions = ref([])
const isLoading = ref(true)

const dialogValues = {
  id: dialogRef.value.data.id,
  isHeaderSection: dialogRef.value.data.isHeaderSection,
}

onMounted(() => {
  loadSectionInfo()
})

function reset() {
  isLoading.value = true
  loadSectionInfo()
}

const form = getValidationInstance()

function loadSectionInfo() {
  if (!isLoading.value) return // Don't load in 2nd time
  axios.get(`/expressionSubSections/${expressionInfo.currentExpressionId}/${dialogValues.id}/options`)
    .then(async (response) => {
      if (!dialogValues.isHeaderSection) {
        sectionTypeOptions.value = response.data.sectionTypes.filter(sectionType => sectionType.name.toLowerCase() !== 'expression')
      }
      else {
        sectionTypeOptions.value = response.data.sectionTypes
      }

      axios.get(`/expressionSubSections/${expressionInfo.currentExpressionId}/${dialogValues.id}`)
        .then(async (json) => {
          form.setValues({
            name: json.data.name,
            content: json.data.content,
            sectionType: sectionTypeOptions.value.find(x => x.id == json.data.sectionTypeId) as ListItem,
          } as EditExpressionSection)
          isLoading.value = false
        })
    })
}

const onSubmit = form.handleSubmit((values) => {
  let sectionTypeId = values.isHeaderSection ? 2 : values.sectionType.id

  axios.put(`/expressionSubSections/${expressionInfo.currentExpressionId}/${dialogValues.id}`, {
    name: values.name,
    content: values.content,
    sectionTypeId: sectionTypeId,
  }).then(() => {
    dialogRef.value.close({
      action: 'refreshList',
    })
    toaster.success('Successfully Updated Expression Section Info!')
  })
})

const deleteExpression = (event) => {
  confirm.require({
    target: event.currentTarget,
    header: 'Deleting Section',
    message: `Are you sure you want delete this section?  This will delete this section and any sub children`,
    icon: 'pi pi-exclamation-triangle',
    group: 'popup',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary',
      outlined: true,
    },
    acceptProps: {
      label: 'Save',
    },
    accept: () => {
      axios.delete(`/expressionSubSections/${expressionInfo.currentExpressionId}/${dialogValues.id}`).then(() => {
        dialogRef.value.close({
          action: 'refreshList',
        })
        toaster.success(`Successfully Deleted the Section!`)
      })
    },
    reject: () => {},
  })
}

</script>

<template>
  <FormWrapper :show-skeleton="isLoading" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormEditorWrapper v-model="form.fields.content" />

    <FormDropdownWrapper v-if="!dialogValues.isHeaderSection" v-model="form.fields.sectionType" :options="sectionTypeOptions ?? []" option-label="name" />

    <div class="flex">
      <div class="col-flex flex-grow-1">
        <Button v-if="!dialogValues.isHeaderSection" severity="danger" label="Delete" class="m-2" @click="deleteExpression($event)" />
        <div class="float-end">
          <Button label="Reset" class="m-2" @click="reset()" />
          <Button label="Cancel" class="m-2" @click="dialogRef.close()" />
          <Button label="Save" class="m-2" @click="onSubmit" />
        </div>
      </div>
    </div>
  </FormWrapper>
</template>
