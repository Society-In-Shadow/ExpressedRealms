<script setup lang="ts">

import {computed, onMounted, ref, watch} from 'vue';
import axios from "axios";
import InputText from "primevue/inputtext";
import type {Log} from "@/components/players/Objects/ActivityLogs";
import DataTable from "primevue/datatable";
import type {ChangedProperty} from "@/components/players/Objects/ChangedProperty";
import Paginator from 'primevue/paginator';
import ActivityLogTile from "@/components/players/Tiles/ActivityLogTile.vue";

const startingPageSize = 25
let logs = ref<Array<Log>>([]);
const filteredLogs = ref<Array<Log>>([]);
const searchQuery = ref<string>("");
const first = ref(0);
const pageSize = ref(startingPageSize);

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
  return filteredLogs.value
      .slice() // Make sure that sort doens't modify filtered logs as a side effect
      .sort((a:Log, b:Log) => b.timeStamp.getTime() - a.timeStamp.getTime())
      .slice(first.value, first.value + pageSize.value);
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
        v-if="logs.length > 0"
        v-model="searchQuery"
        placeholder="Search..."
        class="float-end m-3"
      />
    </div>
  </div>
  <!-- This is needed to keep the stylings on the page, I'll figure out why later and fix it -->
  <DataTable />
  
  <div v-if="logs.length === 0" class="m-3">
    There are no logs for this user.
  </div>
  
  <div v-else-if="filteredLogs.length === 0" class="m-3">
    No logs with that location or changed properties
  </div>

  <div v-for="log in sortedFilteredLogs" :key="log.id">
    <ActivityLogTile :log="log" />
  </div>

  <div v-if="filteredLogs.length > pageSize || pageSize !== startingPageSize && logs.length > startingPageSize">
    <Paginator v-model:first="first" v-model:rows="pageSize" :total-records="filteredLogs.length" :rows-per-page-options="[25, 50, 75, 100]" />
  </div>
</template>

<style scoped>
  .container {
    width: 100%;
    margin-right: auto;
    margin-left: auto;
    max-width:1000px
  }
</style>
