<script setup lang="ts">

import {computed, ref, watch} from "vue";
import Skeleton from 'primevue/skeleton';
import Editor from "primevue/editor";
import ContextMenu from 'primevue/contextmenu';

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

const editorValue = ref();

import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Table from '@tiptap/extension-table';
import TableRow from '@tiptap/extension-table-row';
import TableCell from '@tiptap/extension-table-cell';
import TableHeader from '@tiptap/extension-table-header';
import Underline from '@tiptap/extension-underline';

const CustomTable = Table.extend({
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




const editor = useEditor({
  content: editorValue.value,
  extensions: [
    StarterKit,
    CustomTable.configure({ resizable: false, HTMLAttributes: { class: ''} }),
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

const menu = ref();
const onImageRightClick = (event) => {
  menu.value.show(event);
};

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
        label: 'Add Row Before',
        icon: 'bi bi-arrow-bar-up',
        command: () => editor.value.chain().focus().addRowBefore().run()
      },
      {
        label: 'Add Row After',
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
    <ContextMenu ref="menu" :model="contextOptions" />
    <div class="d-none"><Editor/></div>
    <label :for="dataCyTagCalc">{{ props.fieldName }}</label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="10em" />
    <div v-else class="p-editor" :class="{ 'p-invalid': errorText }">
      <div class="p-editor-toolbar ql-toolbar ql-snow">
        <span class="ql-formats" data-pc-section="formats">
          <button @click="editor.chain().focus().toggleBold().run()" type="button" data-pc-section="">
            <i class="bi bi-type-bold icon-fix"></i>
          </button>
          <button @click="editor.chain().focus().toggleItalic().run()" type="button">
            <i class="bi bi-type-italic icon-fix"></i>
          </button>
          <button @click="editor.chain().focus().toggleUnderline().run()"  type="button">
            <i class="bi-type-underline icon-fix"></i>
          </button>
          <button @click="editor.chain().focus().insertTable({ rows: 4, cols: 3, withHeaderRow: true }).run()"  type="button">
            <i class="bi bi-table icon-fix"></i>
          </button>
        </span>
      </div>
      <div class="p-editor-content ql-container ql-snow">
        <editor-content :editor="editor"
                        :id="dataCyTagCalc" 
                        :data-cy="dataCyTagCalc"
                        v-bind="$attrs"
                        @contextmenu="onImageRightClick"/>
      </div>
    </div>
    
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
    <slot />
  </div>
</template>

<style>
  .icon-fix{
    font-size: 1.5em;
  }

  .custom-table {
    width: 100%; /* Full width table */
    border-collapse: collapse; /* Clean collapse of borders */
  }

  .custom-table tr {
    background-color: var(--surface-ground); /* PrimeVue table row color */
    border-bottom: 1px solid var(--surface-border); /* Row bottom border */
  }

  .custom-table tr:nth-child(odd) {
    background: var(--p-datatable-row-striped-background);
  }

  .custom-table td {
    text-align: start;
    border-color: var(--p-datatable-body-cell-border-color);
    border-style: solid;
    border-width: 0 0 1px 0;
    padding: var(--p-datatable-body-cell-padding);
  }

  .p-editor-content .custom-table td {
    border-width: 3px;
  }

  .custom-table td p{
    margin: 0px;
    padding: 0px;
    min-height: 1em;
  }
  
  .custom-table th p{
    margin: 0px;
    padding: 0px;
    min-height: 1em;
  }
  
  .custom-table th {
    padding: var(--p-datatable-header-cell-padding);
    background: var(--p-datatable-header-cell-background);
    border-color: var(--p-datatable-header-cell-border-color);
    border-style: solid;
    border-width: 0 0 1px 0;
    color: var(--p-datatable-header-cell-color);
    font-weight: normal;
    text-align: start;
    transition: background var(--p-datatable-transition-duration), color var(--p-datatable-transition-duration), border-color var(--p-datatable-transition-duration), outline-color var(--p-datatable-transition-duration), box-shadow var(--p-datatable-transition-duration);
  }

  .custom-table-container {
    display: block; /* Container for overflow */
    margin: 1rem 0; /* Add space around the table */
    overflow-x: auto; /* Enable horizontal scrolling for small screens */
    border: 1px solid var(--surface-border); /* Border matching PrimeVue style */
    border-radius: 4px; /* Rounding of corners */
    background-color: var(--surface-card); /* Background color for container */
  }

</style>