<script setup lang="ts">

  import Button from 'primevue/button';
  import {useRouter} from "vue-router";
  import axios from "axios";
  import toaster from "@/services/Toasters";
  import Badge from 'primevue/badge';
  import type {ExpressionMenuItem} from "@/components/navbar/navMenuItems/types.ts";
  import {type PropType, ref} from "vue";
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
  
  function deleteExpression(){
    axios.delete(`/expression/${props.item.id}`).then(() => {
      cmsData.refreshCmsInformation()
      toaster.success(`Successfully Deleted Expression ${props.item.name}!`);
      toggleDelete();
    });
  }
  
  const deleteConfirm = ref(false);
  function toggleDelete(){
    deleteConfirm.value = !deleteConfirm.value;
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
      <template v-if="deleteConfirm">
        <Button label="Confirm Delete" severity="danger" @click.stop="deleteExpression" />
        <Button label="Cancel" severity="secondary" @click.stop="toggleDelete" />
      </template>
      <template v-else>
        <Button label="Edit" @click.stop="expressionDialogs.showEditExpression(item.id, item.expressionTypeId)" />
        <Button label="Delete" severity="danger" @click.stop="toggleDelete" />
      </template>

    </span>
  </div>
</template>
