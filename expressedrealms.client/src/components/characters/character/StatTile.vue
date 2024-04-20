<script setup lang="ts">
const props = defineProps({
  stateInfo: {
    type: Object,
    required: true,
  },
});
</script>

<template>
  <div class="row">
    <div class="col">
      <div class="row">
        <div class="col-4 text-center">
          <div>Level</div>
          <div>{{props.stateInfo.level}}</div>
        </div>
        <div class="col-4 text-center">
          <div>Bonus</div>
          <div>{{props.stateInfo.bonus}}</div>
        </div>
        <div class="col-4 text-center">
          <div>XP</div>
          <div>{{props.stateInfo.xp}}</div>
        </div>
      </div>
      <div class="row">
        <div class="col">
          {{props.stateInfo.description}}
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>