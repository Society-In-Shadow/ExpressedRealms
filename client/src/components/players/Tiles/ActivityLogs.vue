<script setup lang="ts">

import {onMounted, ref, watch} from 'vue';
import axios from "axios";
import InputText from "primevue/inputtext";
import type {Log} from "@/components/players/Objects/ActivityLogs";
import Card from "primevue/card";
import Column from "primevue/column";
import DataTable from "primevue/datatable";
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
          
          parsedProperties.forEach(function(property:ChangedProperty, index:number) {
            property.id = index;
          });
          
          log.changedPropertiesList = parsedProperties;
        });

        logs.value = response.data.logs;
        filteredLogs.value = response.data.logs
            .sort((a, b) => b.timeStamp - a.timeStamp)
            .slice(0, 25);
      });
}

onMounted(() =>{
  fetchData();
})

function filter(query: string) {
  const lowercasedQuery = query.toLowerCase().trim();

  if (!lowercasedQuery) {
    // Reset showing all players if the query is empty
    filteredLogs.value = logs.value
        .sort((a, b) => b.timeStamp - a.timeStamp)
        .slice(0, 25);
  } else {
    // Filter players by username or email
    filteredLogs.value = logs.value.filter((logs) =>
        logs.location.toLowerCase().includes(lowercasedQuery) ||
        logs.changedProperties.toLowerCase().includes(lowercasedQuery)
    ).sort((a, b) => b.timeStamp - a.timeStamp)
        .slice(0, 25);
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

  <div v-for="log in filteredLogs" :key="log.id">
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

        <DataTable :value="log.changedPropertiesList" striped-rows dataKey="ColumnName">
          <Column field="ColumnName" header="Property">
            <template #body="slotProps">
              <strong>{{ slotProps.data.ColumnName }}</strong>
            </template>
          </Column>
          <Column field="OriginalValue" header="Old Value">
            <template #body="slotProps">
              {{ slotProps.data.OriginalValue }}
            </template>
          </Column>
          <Column field="NewValue" header="New Value">
            <template #body="slotProps">
              {{ slotProps.data.NewValue }}
            </template>
          </Column>
        </DataTable>
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
