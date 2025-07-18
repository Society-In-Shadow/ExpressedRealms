<script setup lang="ts">

import HomePageCarousel from "@/components/public/HomePageCarousel.vue";
import Card from "primevue/card";

</script>

<template>
  <h1>The Three Pillars</h1>
  <div class="d-flex flex-column flex-md-row">
    <Card class="constrain-pillars m-2">
      <template #title>
        1st
      </template>
      <template #content>
        Thou shall not draw undue Mundane attention to the Society.
      </template>
    </Card>
    <Card class="constrain-pillars m-2">
      <template #title>
        2nd
      </template>
      <template #content>
        Thou shall defend the well-being of Mundanes as you do that of the Society, for  our fates are intertwined.
      </template>
    </Card>
    <Card class="constrain-pillars m-2">
      <template #title>
        3rd
      </template>
      <template #content>
        Thou shall assist in the education of any Supernatural beings so that they  understand the need to remain
        hidden, or assist in their removal so they do not threaten  the Society.
      </template>
    </Card>
  </div>
  
  <HomePageCarousel />
  
  <h1>Before the Shadows</h1>

  <p>
    The world of the Expressed began well before the Society, before we followed the Pillars, and before the 
    formation of the original Grand Council. What was this time like? What happened in this time before the 
    shadows, before the One Law? Perhaps, more importantly, what about this time shaped our lives as they 
    are now; and how did each Expression come to exist? These things and more will be covered here in our 
    historical origins known as ancient history.
  </p>
  <p>
    The ancient history for the Expressed overlaps with the history of Man. Though many things stated here 
    will seem to contradict the mundane histories you have learned, be aware that they do not unless ample 
    proof exists for us to believe otherwise. Also know, the spin doctors over in Obscura have done their best
    to keep the most damning of this evidence out of the history books. While most of you will not be familiar 
    with fact that there were cities populated with Expressed and mundanes as far back as 39,000 BC,
    remember that until recently you were also unfamiliar with the fact that Vampyres, Sorcerers, and shape shifters are real.
  </p>
  <p>
    That being said, sit back and enjoy the ride as I take you through the wonders that are our most ancient of histories.
  </p>
  <p>
    Orin Svenson<br>
    Order of Archivists: Third Circle
  </p>
</template>

<style scoped>

 @media(min-width: 768px){
   .constrain-pillars{
     max-width: 300px
   }
 } 
</style>
