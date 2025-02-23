<script setup lang="ts">
  import Card from "primevue/card";
  import type {PropType} from "vue";
  import type {Log} from "@/components/players/Objects/ActivityLogs";
  
  const props = defineProps({
    log: {
      type: Object as PropType<Log>,
      required: true,
    }
  });

</script>

<template>
  <Card class="card-outline mb-3">
    <template #title>
      {{ props.log.action }} - {{ props.log.location }}
    </template>
    <template #subtitle>
      {{ new Date(props.log.timeStamp).toLocaleString('en-US', { year: 'numeric',
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
            <thead class="p-datatable-thead">
              <tr>
                <th class="p-datatable-header-cell">
                  Property
                </th>
                <th class="p-datatable-header-cell">
                  Old Value
                </th>
                <th class="p-datatable-header-cell">
                  New Value
                </th>
              </tr>
            </thead>

            <!-- Table body -->
            <tbody class="p-datatable-tbody">
              <tr
                v-for="(row, index) in props.log.changedPropertiesList" :key="row.id"
                :class="index % 2 === 0 ? 'p-row-even' : 'p-row-odd'"
              >
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
</template>

<style scoped>
.card-outline {
  border: 1px solid var(--p-form-field-disabled-background);
}
</style>
