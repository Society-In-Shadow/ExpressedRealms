<script setup lang="ts">
import Carousel from 'primevue/carousel';
import {publicExpressionsStore} from "@/components/public/stores/publicExpressionStore";
import {ref, onBeforeMount} from "vue";
import {eventStore} from "@/components/public/stores/eventStore";
import {makeIdSafe} from "@/utilities/stringUtilities";

interface CarouselItem {
  name: string;
  description: string;
  link: string;
  dateRange: string | null | undefined;
}

const expressionStore =  publicExpressionsStore();
const eventsStore = eventStore();
onBeforeMount(async () => {
  await expressionStore.getExpressions();
  await eventsStore.getEvents();
  
  items.value.push(...eventsStore.events
      .sort((a, b) => a.startDate.getTime() - b.startDate.getTime())
      .slice(0, 2)
      .map(event => {
        return {
          name: "Upcoming Event",
          description: `Come join us at ${event.name}!`,
          dateRange: `${event.startDate.toDateString()} - ${event.endDate.toDateString()}`,
          link: 'upcoming-events'
        }
      }));
  
  items.value.push(...expressionStore.expressions
      .map(expression => {
        return {
          name: expression.name,
          description: expression.archetypes,
          dateRange: null,
          link: `expressions#${makeIdSafe(expression.name)}`
        }
      }));
})

const items = ref<Array<CarouselItem>>([]);

</script>

<template>
  <div class="constrain-carousal mt-5 mb-5">
    <Carousel :value="items" :num-visible="1" :autoplay-interval="5000">
      <template #item="slotProps">
        <a class="text-decoration-none text-reset" :href="slotProps.data.link">
          <div class="card">
            <div class="d-flex flex-column flex-md-row align-items-center justify-content-between">
              <div class="mr-3">
                <h2 class="mt-0 pt-0">{{ slotProps.data.name }}</h2>
                <p>{{ slotProps.data.description }}</p>
                <p v-if="slotProps.data.dateRange">{{ slotProps.data.dateRange }}</p>
              </div>
              <div>
                <img src="/public/favicon.png" alt="Six Stones Logo" width="175px">
              </div>
            </div>
          </div>
        </a>
      </template>
    </Carousel>
  </div>
</template>

<style scoped>
  .constrain-carousal{
    max-width: 700px;
    margin: 0 auto;
  }
</style>
