<script setup lang="ts">

  import Button from 'primevue/button';
  import { useConfirm } from "primevue/useconfirm";
  import {useRouter} from "vue-router";
  import axios from "axios";
  import toaster from "@/services/Toasters";
  import Badge from 'primevue/badge';
  import type {ExpressionMenuItem} from "@/components/navbar/navMenuItems/types.ts";
  import type {PropType} from "vue";
  import {expressionDialogService} from "@/components/expressions/services/dialogs.ts";
  import {cmsStore} from "@/stores/cmsStore.ts";
  const Router = useRouter();
  const expressionDialogs = expressionDialogService();
  const cmsData = cmsStore();

  let props = defineProps({
    navHeading: {
      type: String,
      required: true,
    },
    item: {
      type: Object as PropType<ExpressionMenuItem>,
      required: true,
    }
  });

  function redirect(){
    if(props.item.id === 0)
    {
      expressionDialogs.showAddExpression(props.item?.expressionTypeId)
      return;
    }
    Router.push(`/${props.navHeading}/` + props.item?.slug);
  }

  const confirm = useConfirm();
  const deleteExpression = (event) => {
    confirm.require({
      target: event.currentTarget,
      header: 'Deleting Expression',
      message: `Are you sure you want delete ${props.item.name} expression?`,
      icon: 'pi pi-exclamation-triangle',
      group: 'popup',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true
      },
      acceptProps: {
        label: 'Save'
      },
      accept: () => {
        axios.delete(`/expression/${props.item.id}`).then(() => {
          cmsData.refreshCmsInformation()
          toaster.success(`Successfully Deleted Expression ${props.item.name}!`);
        });
      },
      reject: () => {}
    });
  };

  function getStatus() {
    switch (props.item.statusId) {
      case 1:
        return 'success';   // Publish
      case 2:
        return 'warning';      // Beta
      case 3:
        return 'secondary'; // Draft
      default:
        return 'unknown';   // Fallback for invalid values
    }
  }
  
</script>
<template>
  <div class="flex flex-shrink-1 align-items-center cursor-pointer gap-2" @click="redirect">
    <div class="flex gap-3 align-items-center flex-grow-1 p-3">
      <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem">
        <i :class="['pi', item.navMenuImage, 'text-lg', 'text-white']" />
      </span>
        <span class="inline-flex flex-grow-1 flex-column gap-1">
        <span class="font-medium text-lg text-900">{{ item.name }} <Badge v-if="cmsData.canEdit && item.id !== 0" :value="item.statusName" :severity="getStatus()" /></span>
        <span class="">{{ item.shortDescription }}</span>
      </span>
    </div>

    <span v-if="cmsData.canEdit && item.id !==0" class="inline-flex flex-column gap-1">
      <Button label="Edit" @click="expressionDialogs.showEditExpression(item.id, item.expressionTypeId)" />
      <Button label="Delete" severity="danger" @click="deleteExpression($event)" />
    </span>
  </div>
</template>
