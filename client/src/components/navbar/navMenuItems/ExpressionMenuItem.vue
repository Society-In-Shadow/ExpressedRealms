<script setup lang="ts">

  import Button from 'primevue/button';
  import { useConfirm } from "primevue/useconfirm";
  import {useRouter} from "vue-router";
  import axios from "axios";
  import toaster from "@/services/Toasters";
  import Badge from 'primevue/badge';
  const Router = useRouter();

  const emit = defineEmits<{
    showEditPopup: [expressionId: number],
    showCreatePopup: [],
    refreshList: []
  }>();
  
  let props = defineProps({
    item: {
      type: Object,
      required: true,
    },
    showEdit: {
      type: Boolean,
      required: true
    }
  });

  function redirect(){
    if(props.item.id === 0)
    {
      emit('showCreatePopup');
      return;
    }
    Router.push("/expressions/" + props.item.name.toLowerCase());
  }

  function showEditPopup(){
    emit('showEditPopup', props.item.id);
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
          emit('refreshList');
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
  <div class="flex flex-shrink-1 align-items-center p-3 cursor-pointer mb-2 gap-2">
    <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem" @click="redirect">
      <i :class="['pi', item.navMenuImage, 'text-lg', 'text-white']" />
    </span>
    <span class="inline-flex flex-grow-1 flex-column gap-1" @click="redirect">
      <span class="font-medium text-lg text-900">{{ item.name }} <Badge v-if="showEdit && item.id !== 0" :value="item.statusName" :severity="getStatus()" /></span>
      <span class="">{{ item.shortDescription }}</span>
    </span>
    <span v-if="showEdit && item.id !==0" class="inline-flex flex-column gap-1">
      
      <Button label="Edit" @click="showEditPopup" />
      <Button label="Delete" severity="danger" @click="deleteExpression($event)" />
    </span>
  </div>
</template>
