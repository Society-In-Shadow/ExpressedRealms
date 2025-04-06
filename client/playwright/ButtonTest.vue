<template>
  <button @click="onClick">{{ buttonLabel }}</button>
</template>

<script>
export default {
  name: 'AddCharacter',
  props: {
    buttonLabel: {
      type: String,
      default: 'Click me',
    },
  },
  emits: ['onCharacterAdded'],
  methods: {
    onClick() {
      this.$emit('onCharacterAdded', 'New Character');
    },
  },
};
</script>