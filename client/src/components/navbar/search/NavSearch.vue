<script setup lang="ts">
import { AutoComplete } from 'primevue'
import type { SearchResult } from '@/components/navbar/types.ts'
import axios from 'axios'
import { ref } from 'vue'

const suggestions = ref<SearchResult[]>([])

const search = async (event: { query: string }) => {
  const response = await axios.get<SearchResult[]>('search', {
    params: {
      q: event.query,
    },
  })

  suggestions.value = response.data.slice(0, 6)
}
</script>

<template>
  <AutoComplete
    :suggestions="suggestions"
    option-label="name"
    placeholder="Search..."
    :min-length="2"
    :delay="250"
    scroll-height="20em"
    @complete="search"
  >
    <template #option="{ option }">
      <div class="flex flex-column gap-1">
        <span class="font-medium">{{ option.section }} > {{ option.navName }} > {{ option.sectionName }}</span>
        <small class="text-color-secondary">
          <div v-html="option.sectionContent" />
        </small>
      </div>
    </template>
  </AutoComplete>
</template>

<style>
  mark {
    font-weight: bold;
    background: inherit;
    color: var(--p-text-muted-color)
  }
</style>
