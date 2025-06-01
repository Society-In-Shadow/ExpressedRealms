<script setup lang="ts">

import {computed, onMounted, ref, watch} from "vue";
import Skeleton from 'primevue/skeleton';
import Editor from "primevue/editor";
import ContextMenu from 'primevue/contextmenu';
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Table from '@tiptap/extension-table';
import TableRow from '@tiptap/extension-table-row';
import TableCell from '@tiptap/extension-table-cell';
import TableHeader from '@tiptap/extension-table-header';
import Underline from '@tiptap/extension-underline';
import type {FormField} from "@/FormWrappers/Interfaces/FormField";

const model = defineModel<FormField>({ required: true });

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  dataCyTag: {
    type: String,
    default: ""
  },
  showSkeleton: {
    type: Boolean,
    default: false
  }
});

const editorValue = ref(model.value.field);
const startupComplete = ref(false);

const CustomTable = Table.configure().extend({
  renderHTML({ HTMLAttributes }) {

    // Add a new class to the table element
    const tableAttributes = {
      ...HTMLAttributes, // Spread existing table attributes
      class: `${HTMLAttributes.class || 'w-100 custom-table'}`.trim(), // Append new class
    };

    return [
      'div', // Outer div
      { class: 'custom-table-container' }, // Add custom attributes to the wrapper div
      [

          'table',
          tableAttributes, // Pass the original HTML attributes to the table
          0, // This represents a placeholder for the table's children (rows, cells, etc.)
      ]
      
    ];
  },
});

onMounted(() => {startupComplete.value = false;})

const editor = useEditor({
  content: editorValue.value,
  extensions: [
    StarterKit,
    CustomTable,
    TableRow,
    TableCell,
    TableHeader,
    Underline
  ],
  onUpdate: ({editor}) => {
    model.value.field.value = editor.getHTML();
  },
  editorProps: {
    attributes: {
      class: "ql-editor", // Add your custom classes here
    },
  },

});

watch(
    () => model.value.field.value, // Reactive dependency
    (newValue) => {
      // Update the editor content whenever `model.value` changes
      if (editor.value && startupComplete.value == false) {
        editor.value.commands.setContent(newValue);
        startupComplete.value = true;
        
      }
    }
);

const dataCyTagCalc = computed(() => {
  if(props.dataCyTag != ""){
    return props.dataCyTag;
  }
  return model.value.label.replace(" ", "-").toLowerCase();
});

const menu = ref();
const onImageRightClick = (event) => {
  menu.value.show(event);
};

const focusingOnTable = computed(() => {
  if(!editor.value){
    return false;
  }
  return editor.value.can().addColumnBefore() || 
      editor.value.can().addColumnAfter() ||
      editor.value.can().deleteColumn() ||
      editor.value.can().addRowBefore() ||
      editor.value.can().addRowAfter() ||
      editor.value.can().deleteRow() ||
      editor.value.can().deleteTable();
})

const contextOptions = ref([
  {
    label: 'Columns',
    icon: 'bi bi-layout-three-columns',
    items: [
      {
        label: 'Add Column Before',
        icon: 'bi bi-arrow-bar-left',
        command: () => editor.value.chain().focus().addColumnBefore().run()
      },
      {
        label: 'Add Column After',
        icon: 'bi bi-arrow-bar-right',
        command: () => editor.value.chain().focus().addColumnAfter().run()
      },
      {
        label: 'Delete Column',
        icon: 'bi bi-trash',
        items: [
          {
            label: 'Confirm Delete Column',
            icon: 'bi bi-trash',
            command: () => editor.value.chain().focus().deleteColumn().run()
          }
        ]
      },
    ]
  },
  { 
    label: "Row",
    icon: 'bi bi-list-task',
    items: [
      {
        label: 'Add Row Above',
        icon: 'bi bi-arrow-bar-up',
        command: () => editor.value.chain().focus().addRowBefore().run()
      },
      {
        label: 'Add Row Below',
        icon: 'bi bi-arrow-bar-down',
        command: () => editor.value.chain().focus().addRowAfter().run()
      },
      {
        label: 'Delete Row',
        icon: 'bi bi-trash',
        items: [
          {
            label: 'Confirm Delete Row',
            icon: 'bi bi-trash',
            command: () => editor.value.chain().focus().deleteRow().run()
          }
        ]
        
      },
    ]
  },
  {
    separator: true
  },
  {
    label: 'Delete Table',
    icon: 'bi bi-table',
    items: [
      {
        label: 'Confirm Delete Table',
        icon: 'bi bi-trash',
        command: () => editor.value.chain().focus().deleteTable().run()
      }
    ]
  },
]);

</script>

<template>
  <div class="mb-3">
    <ContextMenu v-if="focusingOnTable" ref="menu" :model="contextOptions" />
    <div class="d-none">
      <Editor />
    </div>
    <label :for="dataCyTagCalc">{{ model.label }}</label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="10em" />
    <div v-else class="p-editor" :class="{ 'p-invalid': model.error && model.error.length > 0 }">
      <div class="p-editor-toolbar ql-toolbar ql-snow">
        <span class="ql-formats" data-pc-section="formats">
          <button type="button" data-pc-section="" @click="editor.chain().focus().toggleBold().run()">
            <i class="bi bi-type-bold icon-fix" />
          </button>
          <button type="button" @click="editor.chain().focus().toggleItalic().run()">
            <i class="bi bi-type-italic icon-fix" />
          </button>
          <button type="button" @click="editor.chain().focus().toggleUnderline().run()">
            <i class="bi-type-underline icon-fix" />
          </button>
          <button type="button" @click="editor.chain().focus().insertTable({ rows: 4, cols: 3, withHeaderRow: true }).run()">
            <i class="bi bi-table icon-fix" />
          </button>
        </span>
      </div>
      <div class="p-editor-content ql-container ql-snow">
        <editor-content
          :id="dataCyTagCalc"
          :editor="editor" 
          :data-cy="dataCyTagCalc"
          v-bind="$attrs"
          @contextmenu="onImageRightClick"
        />
      </div>
    </div>
    
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>

<style>
  .icon-fix{
    font-size: 1.5em;
  }
</style>
