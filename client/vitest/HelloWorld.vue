<template>
  <div>
    <h1>{{ msg }}</h1>
    <button @click="handleClick">{{ buttonText }}</button>
  </div>
</template>

<script lang="ts" setup>
import {ref} from 'vue';

defineProps<{
  msg: string;
}>();

const buttonText = ref('Click Me');

function handleClick() {
  buttonText.value = 'You clicked!';
}
</script>
