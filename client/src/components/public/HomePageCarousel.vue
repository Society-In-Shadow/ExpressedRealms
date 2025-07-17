<script setup lang="ts">
import Carousel from 'primevue/carousel';

const items = [
  {
    type: 'event',
    name: 'Upcoming Event',
    description: 'Come join us at Sioux City Geek Con!',
    link: 'upcoming-events'
  },
  {
    type: 'expression',
    name: 'Adepts',
    description: 'Incredible martial artists. Masters of the mind. Enlightened healers. Stalwart defenders.',
    link: 'expressions#adepts'
  },
  {
    type: 'expression',
    name: 'Vampyres',
    description: 'Vampyres of legend have been portrayed as everything from vicious monsters to cursed saints.',
    link: 'expressions#vampyres'
  },
  {
    type: 'expression',
    name: 'Sidhe',
    description: 'Eldritch Knights. Carefree Illusionists. Archer\'s of Legend.',
    link: 'expressions#sidhe'
  },
  {
    type: 'expression',
    name: 'Aeternari',
    description: 'The Immortals. The Ageless.',
    link: 'expressions#aeternari'
  },
  {
    type: 'expression',
    name: 'Sorcerers',
    description: 'Elementalists. Witches. Magi.',
    link: 'expressions#sorcerers'
  },
  {
    type: 'expression',
    name: 'Shammas',
    description: 'Werecreatures. Medicine men. Hybrid of humanity and the wild.',
    link: 'expressions#shammas'
  }
]

// One tile per expression
// One tile for next upcoming event (first tile displayed)
// one tile for news articles

</script>

<template>
  <div class="constrain-carousal mt-5 mb-5">
    <Carousel :value="items" :numVisible="1" :autoplay-interval="5000">
      <template #item="slotProps">
        <a class="text-decoration-none text-reset" :href="slotProps.data.link">
          <div class="card">
            <div class="d-flex flex-column flex-md-row align-items-center justify-content-between">
              <div class="mr-3">
                <h2 class="mt-0 pt-0">{{slotProps.data.name}}</h2>
                <p>{{slotProps.data.description}}</p>
              </div>
              <div>
                <img src="/public/favicon.png" alt="Six Stones Logo" width="175px"/>
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