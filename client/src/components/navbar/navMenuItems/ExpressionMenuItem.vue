<script setup lang="ts">

  import Button from 'primevue/button';
  import Router from "@/router";
  import {useRouter} from "vue-router";
  const router = useRouter();

  const emit = defineEmits<{
    showPopup: [expressionId: number]
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
    if(props.item.id === 0) return;
    Router.push("/expressions/" + props.item.name.toLowerCase());
  }
  
  function showEditPopup(){
    emit('showPopup', props.item.id);
  }
  
</script>
<template>
  <div class="flex flex-shrink-1 align-items-center p-3 cursor-pointer mb-2 gap-2" >
    <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem" @click="redirect">
      <i :class="['pi', item.navMenuImage, 'text-lg', 'text-white']" />
    </span>
    <span class="inline-flex flex-column gap-1" @click="redirect">
      <span class="font-medium text-lg text-900">{{ item.name }}</span>
      <span class="">{{ item.shortDescription }}</span>
    </span>
    <span class="inline-flex flex-column gap-1" v-if="showEdit && item.id !==0">
      <Button label="Edit" @click="showEditPopup"/>
    </span>
  </div>
</template>
