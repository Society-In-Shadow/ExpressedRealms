<script setup lang="ts">

import Dropdown from 'primevue/dropdown';
import {computed} from "vue";
import Skeleton from 'primevue/skeleton';
import InputGroup from 'primevue/inputgroup';
import Button from 'primevue/button'
import { useRouter } from "vue-router";
const Router = useRouter();

const model = defineModel({ required: false, default: {}, type: Object });

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  options: {
    type: Array,
    required: true
  },
  optionLabel: {
    type: String,
    required: true
  },
  dataCyTag: {
    type: String,
    default: ""
  },
  errorText: {
    type: String,
    default: ""
  },
  showSkeleton: {
    type: Boolean,
    default: false
  },
  redirectUrl:{
    type: String,
    required: true
  },
  redirectToDifferentPage:{
    type: Boolean,
    default: false
  }
});

const dataCyTagCalc = computed(() => {
  if(props.dataCyTag != ""){
    return props.dataCyTag;
  }
  return props.fieldName.replace(" ", "-").toLowerCase();
});

function redirectUser(openInNewTab:boolean) {
  if(!openInNewTab){
    Router.push(props.redirectUrl);
  }else{
    window.open(props.redirectUrl, "_blank");
  }
}

</script>

<template>
  <div class="mb-3">
    <label :for="dataCyTagCalc">{{ props.fieldName }}</label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <InputGroup v-else>
      <Dropdown
        :id="dataCyTagCalc" v-model="model" :options="options" :option-label="optionLabel" :data-cy="dataCyTagCalc"
        class="w-100" :class="{ 'p-invalid': errorText }" v-bind="$attrs"
      />
      <Button icon="pi pi-info-circle" severity="info" :disabled="!model" @click="redirectUser(props.redirectToDifferentPage)" @click.middle="redirectUser(true)" />
    </InputGroup>
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
    <slot />
  </div>
</template>
