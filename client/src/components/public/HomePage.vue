<script setup lang="ts">

import HomePageCarousel from '@/components/public/HomePageCarousel.vue'
import Card from 'primevue/card'
</script>

<template>
  <div class="d-flex flex-md-row flex-column gap-3">
    <div class="flex-fill">
      <Card class="flex-fill">
        <template #title>
          Society in Shadows
        </template>
        <template #content>
          <p>
            Once secret and heavily guarded, this manual is now online, risky, but necessary. The Society preserves its
            stories so knowledge survives beyond memory and whispers.
          </p>
          <div>
            <router-link to="/about">
              <div class="d-flex flex-row gap-1">
                <div>Explore Who We Are</div>
                <div>
                  <span class="material-symbols-outlined">
                    arrow_right_alt
                  </span>
                </div>
              </div>
            </router-link>
          </div>
          <div>
            <router-link to="/upcoming-events">
              <div class="d-flex flex-row gap-1">
                <div>Explore Upcoming Events</div>
                <div>
                  <span class="material-symbols-outlined">
                    arrow_right_alt
                  </span>
                </div>
              </div>
            </router-link>
          </div>
        </template>
      </Card>
    </div>
    <div class="flex-fill">
      <Card class="flex-fill">
        <template #title>
          How will you Express?
        </template>
        <template #content>
          <p>
            In Society in Shadows, every person awakens to an Expression — a path that defines your abilities, choices,
            and place in the world.
          </p>
          <div>
            <router-link to="/expressions">
              <div class="d-flex flex-row gap-1">
                <div>Explore Expressions </div>
                <div>
                  <span class="material-symbols-outlined">
                    arrow_right_alt
                  </span>
                </div>
              </div>
            </router-link>
          </div>
          <div>
            <router-link to="/backstory">
              <div class="d-flex flex-row gap-1">
                <div>Explore Society's Backstory</div>
                <div>
                  <span class="material-symbols-outlined">
                    arrow_right_alt
                  </span>
                </div>
              </div>
            </router-link>
          </div>
        </template>
      </Card>
    </div>
  </div>

  <HomePageCarousel />

  <h1 class="text-center">
    Why Society in Shadows?
  </h1>

  <div class="d-flex flex-column flex-md-row">
    <Card class="constrain-pillars m-2">
      <template #title>
        Character Driven Stories
      </template>
      <template #content>
        All players can and do have an impact on the story. It's not unusual for players to change plot direction, for better or for worse.
      </template>
    </Card>
    <Card class="constrain-pillars m-2">
      <template #title>
        Life Long Friendships
      </template>
      <template #content>
        Many of our players became good friends through the hijinx brought forth during play. It's those pesky
        squirrels that connect us together.
      </template>
    </Card>
    <Card class="constrain-pillars m-2">
      <template #title>
        Shenanigans
      </template>
      <template #content>
        It's not every day that you rescue Sue, the reanimated T-Rex, and then find her the perfect companion up in
        North Dakota.
      </template>
    </Card>
  </div>
</template>

<style>
@media(min-width: 768px){
  .constrain-pillars{
    max-width: 300px
  }
}
</style>
