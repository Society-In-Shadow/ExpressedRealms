<script setup lang="ts">

import HomePageCarousel from "@/components/public/HomePageCarousel.vue";
import Card from "primevue/card";

</script>

<template>
  
  <HomePageCarousel />

  <h1>The Three Pillars</h1>
  <div class="d-flex flex-column flex-md-row">
    <Card class="constrain-pillars m-2">
      <template #title>1st</template>
      <template #content>
        Thou shall not draw undue Mundane attention to the Society.
      </template>
    </Card>
    <Card class="constrain-pillars m-2">
      <template #title>2nd</template>
      <template #content class="">
        Thou shall defend the well-being of Mundanes as you do that of the Society, for  our fates are intertwined.
      </template>
    </Card>
    <Card class="constrain-pillars m-2">
      <template #title>3rd</template>
      <template #content>
        Thou shall assist in the education of any Supernatural beings so that they  understand the need to remain
        hidden, or assist in their removal so they do not threaten  the Society.
      </template>
    </Card>
  </div>
  
  
  
  
  <h1>The Foundation of the Society</h1>
  <p>
    Beyond the bickering, scheming and backstabbing, beyond any political manipulations, the Three Pillars  have remained 
    sacred since their creation. Acceptance of the Pillars unites the Society as a nation of  allies who follow this vital 
    code. Though every few generations of Expressed customize the precise  wording to fit their specific brand of justice 
    and debate the number of Pillars, the heart of the First Pillar  has remained constant since the days of Atlantis and 
    is known as the One Law. This One Law, “Thou  shall not draw undue Mundane attention to the Society,” is the ultimate 
    fail-safe for all Expressed, as it  gives them the liberty to live freely outside the influence of ignorant Mundanes.
  </p>
  <p>
    The Second and Third Pillars also support vital doctrines within the Society. The Second Pillar, “Thou  shall defend
    the well-being of Mundanes as you do that of the Society, for our fates are intertwined,” is a  core principle dating
    back to the fore-founders of the Society. It was the belief of the first Council that the  Society would remain as
    mankind’s undisclosed ally. Some may not care to admit it, but we all come from  humanity, and any mundane death may
    prevent another one of our kind from expressing. The Third Pillar,  “Thou shall assist in the education of any
    Supernatural beings so that they understand the need to remain  hidden, or assist in their removal so they do not
    threaten the Society,” imparts the need to mentor the  newly Expressed. Instruction of basic skills is essential for
    them to survive as a member of the Society  and to defend the One Law. It is the Grand Council’s belief that these
    benchmarks should be upheld to  protect the Society.
  </p>



</template>

<style scoped>

 @media(min-width: 768px){
   .constrain-pillars{
     max-width: 300px
   }
 } 
</style>