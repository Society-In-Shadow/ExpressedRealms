<script setup lang="ts">
import Card from "primevue/card";
import type {PlayerListItem} from "@/components/players/Objects/Player";
import type {PropType} from "vue";

const props = defineProps({
  playerInfo: {
    type: Object as PropType<PlayerListItem>,
    required: true
  }
});

</script>

<template>
  <Card class="m-3">
    <template #content>
      <div class="row">
        <div class="col">
          <h2 class="p-0 m-0">
            {{ props.playerInfo.username }}
          </h2>
        </div>
      </div>
      <div class="row">
        <div class="col">
          {{ props.playerInfo.email }}
        </div>
      </div>
    </template>
  </Card>
</template>

