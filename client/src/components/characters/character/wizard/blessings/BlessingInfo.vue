<script setup lang="ts">

const props = defineProps({
  type: {
    type: String,
    required: true,
  },
})

</script>

<template>
  <div v-if="props.type === 'advantage'">
    <p>
      Advantages are an optional way of extending your character with Discretionary XP.
    </p>
    <p>
      You may only spend up to 8 XP on advantages.
    </p>
    <p>
      Choose your advantages carefully as they may only be taken during character creation.
    </p>
  </div>
  <div v-else>
    <p>Disadvantages are an optional way of gaining more Discretionary XP.</p>
    <p>You may only spend up to 8 XP on disadvantages.</p>
    <p>
      Choose your disadvantages carefully as they may only be taken during character creation.
    </p>
    <p>
      Disadvantages may be overcome, at a cost. A character has the ability of overcoming a disadvantage
      by spending twice the experience that it granted them. Removal of a disadvantage also requires
      approval from your GO and appropriate in game reasons.
    </p>
  </div>
</template>
