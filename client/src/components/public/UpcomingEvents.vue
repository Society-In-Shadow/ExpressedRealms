<script setup lang="ts">
import Card from 'primevue/card';
</script>

<template>
<h1>Our Upcoming Events!</h1>
  
<Card>
  <template #title>Sioux City Geek Con</template>
  <template #content>
    <div>August 23rd-25th</div>
    <div>Sioux City, 801 4th St, Sioux City, IA 51101, USA</div>
    <div><a href="https://siouxcitytabletopg.wixsite.com/my-site">Sioux City Table Top RPG</a></div>
    <h1 class="pt-5">Society's Point of Contacts</h1>
    <div>
      <div class="d-flex flex-column flex-md-row align-items-center">
        <div>
          <h2>Person 1</h2>
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque egestas vehicula viverra. Aenean sagittis 
            pellentesque ultrices. Aenean eu sollicitudin odio, vel faucibus dolor. Proin vitae turpis vulputate, lacinia 
            justo ut, elementum ligula. Cras in nulla turpis. Maecenas id risus bibendum, molestie felis id, viverra nulla. 
            Cras velit sem, condimentum ac facilisis sit amet, placerat nec velit. Morbi urna purus, pharetra vel placerat 
            vel, vehicula non elit. Phasellus maximus justo sit amet accumsan pretium. Proin convallis nisi cursus pretium 
            volutpat. Donec et metus et eros viverra auctor non ut lacus. Pellentesque vel erat vitae ligula scelerisque gravida.
          </p>
        </div>
        <div class="">
          <img src="/public/favicon.png" alt="If I had one" width="150">
        </div>
      </div>
      <div class="d-flex flex-column flex-md-row align-items-center">
        <div>
          <h2>Person 2</h2>
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque egestas vehicula viverra. Aenean sagittis
            pellentesque ultrices. Aenean eu sollicitudin odio, vel faucibus dolor. Proin vitae turpis vulputate, lacinia
            justo ut, elementum ligula. Cras in nulla turpis. Maecenas id risus bibendum, molestie felis id, viverra nulla.
            Cras velit sem, condimentum ac facilisis sit amet, placerat nec velit. Morbi urna purus, pharetra vel placerat
            vel, vehicula non elit. Phasellus maximus justo sit amet accumsan pretium. Proin convallis nisi cursus pretium
            volutpat. Donec et metus et eros viverra auctor non ut lacus. Pellentesque vel erat vitae ligula scelerisque gravida.
          </p>
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