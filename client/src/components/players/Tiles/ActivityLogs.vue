<script setup lang="ts">

import {computed, onMounted, ref, watch} from 'vue';
import axios from "axios";
import InputText from "primevue/inputtext";
import type {Log} from "@/components/players/Objects/ActivityLogs";
import Card from "primevue/card";
import type {ChangedProperty} from "@/components/players/Objects/ChangedProperty";

let logs = ref<Array<Log>>([]);
const filteredLogs = ref<Array<Log>>([]);
const searchQuery = ref<string>("");

const props = defineProps({
  userId: {
    type: String,
    required: true,
  }
});

function fetchData() {
  axios.get(`/admin/user/${props.userId}/activitylogs`)
      .then((response) => {
        
        response.data.logs.forEach(function(log:Log) {
          var parsedProperties = JSON.parse(log.changedProperties);
          log.timeStamp = new Date(log.timeStamp);
          parsedProperties.forEach(function(property:ChangedProperty, index:number) {
            property.id = index;
          });
          
          log.changedPropertiesList = parsedProperties;
        });
        
        logs.value = response.data.logs;
        
        filteredLogs.value = logs.value
            
      });
}

onMounted(() =>{
  fetchData();
})

const sortedFilteredLogs = computed(() => {
  return filteredLogs.value.sort((a:Log, b:Log) => b.timeStamp.getTime() - a.timeStamp.getTime()); // Example calculation
});


function filter(query: string) {
  const lowercasedQuery = query.toLowerCase().trim();

  if (!lowercasedQuery) {
    // Reset showing all players if the query is empty
    filteredLogs.value = logs.value;
  } else {
    // Filter players by username or email
    filteredLogs.value = logs.value.filter((logs) =>
        logs.location.toLowerCase().includes(lowercasedQuery) ||
        logs.changedProperties.toLowerCase().includes(lowercasedQuery)
    );
  }
}

// Debounce function
function debounce(fn: Function, delay: number) {
  let timeout: number | undefined;
  return (...args: any[]) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
      fn(...args);
    }, delay);
  };
}

// Debounced filter function
const debouncedFilter = debounce((query: string) => {
  filter(query);
}, 250);

// Watch for changes to the search query and trigger the debounced filter function
watch(searchQuery, (newQuery) => {
  debouncedFilter(newQuery);
});

</script>

<template>

  <div class="row">
    <div class="col">
      <h1 class="m-3">
        Activity Logs
      </h1>
    </div>
    <div class="col">
      <InputText
        v-model="searchQuery"
        placeholder="Search..."
        class="float-end m-3"
      />
    </div>
  </div>
  
  <div v-if="filteredLogs.length === 0" class="m-3">
    No logs with that location or changed properties
  </div>

  <div v-for="log in sortedFilteredLogs" :key="log.id">
    <Card class="card-outline mb-3">
      <template #title>
        {{ log.action }} - {{ log.location }}
      </template>
      <template #subtitle>
        {{ new Date(log.timeStamp).toLocaleString('en-US', {  year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }) }}
      </template>
      <template #content>
        <div class="p-datatable p-component p-datatable-striped">
          <div class="p-datatable-table-container">
            <table class="w-100 p-datatable-table">
              <!-- Table header -->
              <thead class="p-datatable-thead" >
                <tr>
                  <th class="p-datatable-header-cell">Property</th>
                  <th class="p-datatable-header-cell">Old Value</th>
                  <th class="p-datatable-header-cell">New Value</th>
                </tr>
              </thead>
  
              <!-- Table body -->
              <tbody class="p-datatable-tbody">
                <tr v-for="(row, index) in log.changedPropertiesList" :key="row.id" :class="index % 2 === 0 ? 'p-row-even' : 'p-row-odd'">
                  <td>{{ row.ColumnName }}</td>
                  <td>{{ row.OriginalValue }}</td>
                  <td>{{ row.NewValue }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

      </template>
    </Card>
  </div>
</template>

<style scoped>
  .card-outline{
    border: 1px solid var(--p-form-field-disabled-background);
  }
  .container {
    width: 100%;
    margin-right: auto;
    margin-left: auto;
    max-width:1000px
  }
</style>
