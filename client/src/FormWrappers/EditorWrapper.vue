<script setup lang="ts">

import {computed, watch} from "vue";
import Skeleton from 'primevue/skeleton';

import Editor from "primevue/editor";

const model = defineModel<string>({ required: true });

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
import Table from '@tiptap/extension-table';
import TableRow from '@tiptap/extension-table-row';
import TableCell from '@tiptap/extension-table-cell';
import TableHeader from '@tiptap/extension-table-header';
import Underline from '@tiptap/extension-underline';


const editor = useEditor({
  content: model.value,
  extensions: [
    StarterKit,
    Table.configure({ resizable: true }),
    TableRow,
    TableCell,
    TableHeader,
    Underline
  ],
  onUpdate: ({editor}) => {
    console.log("i've been updated!");
    model.value = editor.getHTML();
  },
  editorProps: {
    attributes: {
      class: "ql-editor", // Add your custom classes here
    },
  },

});

watch(
    () => model.value, // Reactive dependency
    (newValue) => {
      // Update the editor content whenever `model.value` changes
      if (editor.value) {
        editor.value.commands.setContent(newValue);
      }
    }
);


const dataCyTagCalc = computed(() => {
  if(props.dataCyTag != ""){
    return props.dataCyTag;
  }
  return props.fieldName.replace(" ", "-").toLowerCase();
});

</script>

<template>
  <div class="mb-3">
    
    <div class="d-none"><Editor></Editor></div>
    <label :for="dataCyTagCalc">{{ props.fieldName }}</label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="10em" />
    <div v-else class="p-editor" :class="{ 'p-invalid': errorText }">
      <div class="p-editor-toolbar ql-toolbar ql-snow">
        <span class="ql-formats" data-pc-section="formats">
          <button @click="editor.chain().focus().toggleBold().run()" type="button" data-pc-section="">
            <i class="bi bi-type-bold icon-fix"></i>
          </button>
          <button @click="editor.chain().focus().toggleItalic().run()" type="button">
            <i class="pi bi-type-italic icon-fix"></i>
          </button>
          <button @click="editor.chain().focus().toggleUnderline().run()"  type="button">
            <i class="pi bi-type-underline icon-fix"></i>
          </button>
        </span>
      </div>
      <div class="p-editor-content ql-container ql-snow">
        <editor-content :editor="editor"
                        :id="dataCyTagCalc" 
                        :data-cy="dataCyTagCalc"
                        v-bind="$attrs"/>
      </div>
    </div>
    
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
    <slot />
  </div>
</template>

<style scoped>
  .icon-fix{
    font-size: 1.5em;
  }
</style>