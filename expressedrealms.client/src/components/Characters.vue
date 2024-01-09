<template>
    <div>
        <h1>Character DB Test</h1>
        <p>This component demonstrates fetching data from the database via a protected endpoint.</p>

        <DataTable :value="characters" tableStyle="min-width: 50rem">
          <Column field="id" header="Id"></Column>
          <Column field="name" header="Name"></Column>
        </DataTable>
      
    </div>
</template>

<script setup lang="ts">
    import { onMounted, ref } from 'vue';
    import DataTable from 'primevue/datatable';
    import Column from 'primevue/column';
    import axios from "axios";

    let characters = ref([]);
    
    function fetchData() {
      axios.get('/api/characters')
          .then((json) => {
            characters.value = json.data;
          });
    }
     
    onMounted(() =>{
      fetchData();
    })
</script>

<style scoped>
</style>