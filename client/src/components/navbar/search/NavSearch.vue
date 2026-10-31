<script setup lang="ts">
import { AutoComplete } from 'primevue'
import type { SearchResult } from '@/components/navbar/types.ts'
import axios from 'axios'
import { nextTick, ref } from 'vue'
import Avatar from 'primevue/avatar'
import Drawer from 'primevue/drawer'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import { useRouter } from 'vue-router'
import { makeIdSafe } from '@/utilities/stringUtilities.ts'

const suggestions = ref<SearchResult[]>([])
const hasData = ref(false)
const router = useRouter()

const search = async (event: { query: string }) => {
  const response = await axios.get<SearchResult[]>('search', {
    params: {
      q: event.query,
    },
  })
  hasData.value = event.query.length > 0
  suggestions.value = response.data.slice(0, 6)
}

const searchInput = ref()

const showFullSearch = ref(false)
const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

const togglePanel = async () => {
  showFullSearch.value = !showFullSearch.value
  await nextTick()
  searchInput.value?.$el.querySelector('input')?.focus()
}

const redirect = async (suggestion: SearchResult) => {
  let section = ''
  if (suggestion.section == 'Rule Book')
    section = 'rulebook'
  else if (suggestion.section == 'World Background')
    section = 'worldbackground'
  else if (suggestion.section == 'Expression')
    section = 'expressions'
  await router.push({ name: section, params: { slug: suggestion.slug }, hash: '#' + makeIdSafe(suggestion.sectionName) })
  showFullSearch.value = false
}

</script>

<template>
  <div v-if="!showFullSearch" class="flex align-items-center cursor-pointer" @click="togglePanel">
    <Avatar class="pi pi-search avatar-size" shape="circle" />
  </div>
  <div v-else>
    <Drawer
      v-model:visible="showFullSearch" header="Global Search" :dismissable="false"
      :block-scroll="isMobile" :position="isMobile ? 'full' : 'right'" class="search-drawer"
    >
      <AutoComplete
        ref="searchInput"
        placeholder="Search..."
        :min-length="2"
        :delay="250"
        scroll-height="20em"
        class="w-100 pb-3"
        input-class="w-100"
        loading-icon="si-search"
        @complete="search"
      />
      <div class="d-flex flex-column align-items-center pt-2">
        <div v-if="suggestions.length == 0 && !hasData">
          <p>Search above to get started!</p>
        </div>
        <div v-else-if="suggestions.length == 0 && hasData">
          <p>No Search Results</p>
        </div>
      </div>

      <div v-for="suggestion in suggestions" :key="suggestion.entryId" @click="redirect(suggestion)">
        <div class="flex flex-column gap-1 pb-3">
          <h3 class="m-0 p-0">
            {{ suggestion.sectionName }}
          </h3>
          <h5 class="m-0 p-0 pb-1 pt-1 text-secondary">
            {{ suggestion.section }} > {{ suggestion.navName }}
          </h5>
          <small class="text-color-secondary">
            <div v-html="suggestion.sectionContent" />
          </small>
        </div>
      </div>
    </Drawer>
  </div>
</template>

<style>
mark {
  font-weight: bold;
  background: inherit;
  color: var(--p-text-muted-color)
}
.search-drawer {
  width: 30rem !important;
  max-width: 100vw;
}
</style>
