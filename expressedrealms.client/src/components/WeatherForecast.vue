<template>
    <div class="weather-component">
        <h1>Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>

        <DataTable :value="forecastDates" tableStyle="min-width: 50rem">
          <Column field="date" header="Date"></Column>
          <Column field="temperatureC" header="Temp (C)"></Column>
          <Column field="temperatureF" header="Temp (F)"></Column>
          <Column field="summary" header="Summary"></Column>
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
          return;
        });
  }
  
  onMounted(() =>{
    fetchData();
  });
</script>

<style scoped>
</style>