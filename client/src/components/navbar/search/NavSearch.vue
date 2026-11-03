<script setup lang="ts">
import { AutoComplete } from 'primevue'
import type { SearchResponse, SearchResult } from '@/components/navbar/types.ts'
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
  const response = await axios.get<SearchResponse>('search', {
    params: {
      query: event.query,
    },
  })
  hasData.value = event.query.length > 0
  suggestions.value = response.data.items
}

const searchInput = ref()

const showFullSearch = ref(false)
const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')
const searchText = ref('')

const togglePanel = async () => {
  showFullSearch.value = !showFullSearch.value
  await nextTick()
  searchInput.value?.$el.querySelector('input')?.focus()
}

const redirect = (suggestion: SearchResult) => {
  let section = ''
  if (suggestion.section == 'Rule Book')
    section = 'rulebook'
  else if (suggestion.section == 'World Background')
    section = 'worldbackground'
  else if (suggestion.section == 'Expression')
    section = 'expressions'
  return router.resolve({
    name: section,
    params: { slug: suggestion.slug },
    hash: '#' + makeIdSafe(suggestion.sectionName),
  })
}

const hideSideBar = () => {
  showFullSearch.value = false
}

</script>

<template>
  <div v-if="!showFullSearch" class="flex align-items-center cursor-pointer">
    <Avatar class="pi pi-search avatar-size" shape="circle" @click="togglePanel" />
  </div>
  <div v-else>
    <Drawer
      v-model:visible="showFullSearch" header="Global Search" :dismissable="false"
      :block-scroll="isMobile" :position="isMobile ? 'full' : 'right'" class="search-drawer"
    >
      <AutoComplete
        ref="searchInput"
        v-model="searchText"
        placeholder="Search..."
        :min-length="2"
        :delay="600"
        scroll-height="20em"
        class="w-100 pb-3"
        input-class="w-100"
        loading-icon="si-search"
        @clear="suggestions = []"
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

      <div v-for="suggestion in suggestions" :key="suggestion.entryId">
        <RouterLink :to="redirect(suggestion)" class="no-underline text-color" @click.left="hideSideBar">
          <div class="flex flex-column gap-1 pb-3">
            <h3 class="m-0 p-0">
              {{ suggestion.sectionName }}
            </h3>
            <h5 class="m-0 p-0 pb-1 pt-1 text-secondary">
              {{ suggestion.section }} > {{ suggestion.navName }} <span v-if="suggestion.sectionSubSection"> > {{ suggestion.sectionSubSection }}</span>
            </h5>
            <small class="text-color-secondary">
              <div v-html="suggestion.sectionContent" />
            </small>
          </div>
        </RouterLink>
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
