<script setup lang="ts">
import Card from 'primevue/card';
</script>

<template>
<h1>Our Upcoming Events!</h1>
  
<Card>
  <template #title>Sioux City Geek Con</template>
  <template #content>
    <div>When? - August 23rd-25th</div>
    <div>Where? - Sioux City, 801 4th St, Sioux City, IA 51101, USA</div>
    <div>Con Details? - <a href="https://siouxcitytabletopg.wixsite.com/my-site">Sioux City Table Top RPG</a></div>
    <div>Society in Shadows Main Points of Contact</div>
    <div>
      <div class="d-flex flex-column flex-md-row">
        <div>
          <h1>Person 1</h1>
          <p>It's all about me and who I am</p>
        </div>
        <div>
          <img src="/public/favicon.png" alt="If I had one" width="150">
        </div>
      </div>
      <div class="d-flex flex-column flex-md-row">
        <div>
          <h1>Person 2</h1>
          <p>It's all about me and who I am</p>
        </div>
        <div>
          <img src="/public/favicon.png" alt="If I had one" width="150">
        </div>
      </div>
    </div>
  </template>
</Card>
</template>

<style scoped>

</style>