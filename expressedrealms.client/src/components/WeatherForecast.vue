<template>
  <div class="weather-component">
    <h1>Weather forecast</h1>
    <p>This component demonstrates fetching data from the server.</p>

    <DataTable :value="forecastDates" table-style="min-width: 50rem">
      <Column field="date" header="Date" />
      <Column field="temperatureC" header="Temp (C)" />
      <Column field="temperatureF" header="Temp (F)" />
      <Column field="summary" header="Summary" />
    </DataTable>
  </div>
</template>

<script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';
  
  type Forecasts = {
      date: string,
      temperatureC: string,
      temperatureF: string,
      summary: string
  }[];
  
  let forecastDates = ref<Forecasts[]>([]);
  
  function fetchData(): void {
    fetch('/api/weatherforecast')
        .then(r => r.json())
        .then(json => {
          forecastDates.value = json as Forecasts[];
        });
  }
  
  onMounted(() =>{
    fetchData();
  });
</script>

<style scoped>
</style>