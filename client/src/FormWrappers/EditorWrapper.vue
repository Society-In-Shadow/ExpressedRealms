<script setup lang="ts">

import {computed} from "vue";
import Skeleton from 'primevue/skeleton';

const model = defineModel<string>({ required: true, default: "" });

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  dataCyTag: {
    type: String,
    default: ""
  },
  errorText: {
    required: true,
    type: String,
    default: ""
  },
  showSkeleton: {
    type: Boolean,
    default: false
  }
});

import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'

const editor = useEditor({
  content: model.value,
  extensions: [StarterKit],
  onUpdate: ({editor}) => {
    model.value = editor.getHTML();
  }
})

const dataCyTagCalc = computed(() => {
  if(props.dataCyTag != ""){
    return props.dataCyTag;
  }
  return props.fieldName.replace(" ", "-").toLowerCase();
});

</script>

<template>
  <div class="mb-3">
    <label :for="dataCyTagCalc">{{ props.fieldName }}</label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="10em" />
    <editor-content :editor="editor"
                    v-else
                    :id="dataCyTagCalc" :data-cy="dataCyTagCalc" class="p-inputtext p-component p-filled w-100"
                    :class="{ 'p-invalid': errorText }" v-bind="$attrs"
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
    <slot />
  </div>
</template>
