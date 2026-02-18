<script setup lang="ts">

import { onMounted, ref, watch } from 'vue'
import InputText from 'primevue/inputtext'
import { adminCharacterListStore } from '@/components/admin/characterList/stores/characterListStore.ts'
import CharacterTile from '@/components/admin/characterList/CharacterTile.vue'
import Tab from 'primevue/tab'
import TabPanel from 'primevue/tabpanel'
import TabList from 'primevue/tablist'
import TabPanels from 'primevue/tabpanels'
import Tabs from 'primevue/tabs'

const characterListInfo = adminCharacterListStore()

const searchQuery = ref<string>('')

onMounted(async () => {
  await characterListInfo.fetchCharacters()
})

// Debounce function
function debounce(fn: Function, delay: number) {
  let timeout: number | undefined
  return (...args: any[]) => {
    clearTimeout(timeout)
    timeout = setTimeout(() => {
      fn(...args)
    }, delay)
  }
}

// Debounced filter function
const debouncedFilterPlayers = debounce((query: string) => {
  characterListInfo.filterCharacters(query)
}, 250)

// Watch for changes to the search query and trigger the debounced filter function
watch(searchQuery, (newQuery) => {
  debouncedFilterPlayers(newQuery)
})

</script>

<template>
  <div class="container">
    <div class="row">
      <div class="col">
        <h1 class="m-3">
          Characters ({{ characterListInfo.filteredCharacters.length }})
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
    <div v-if="characterListInfo.filteredCharacters.length === 0" class="m-3">
      No characters with that name or email address
    </div>
    <Tabs value="0">
      <TabList>
        <Tab value="0">
          Awaiting Checkin ({{ characterListInfo.getAwaitingCheckin().length }})
        </Tab>
        <Tab v-if="characterListInfo.getAwaitingGoApproval().length > 0" value="1">
          Awaiting GO Approval ({{ characterListInfo.getAwaitingGoApproval().length }})
        </Tab>
        <Tab v-if="characterListInfo.getAwaitingCrbCreation().length > 0" value="2">
          Awaiting CRB Creation ({{ characterListInfo.getAwaitingCrbCreation().length }})
        </Tab>
        <Tab v-if="characterListInfo.getAwaitingPickup().length > 0" value="3">
          Awaiting Pickup ({{ characterListInfo.getAwaitingPickup().length }})
        </Tab>
        <Tab v-if="characterListInfo.getAwaitingDay2().length > 0" value="4">
          Awaiting Day 2 ({{ characterListInfo.getAwaitingDay2().length }})
        </Tab>
        <Tab v-if="characterListInfo.getAwaitingDay3().length > 0" value="5">
          Awaiting Day 3 ({{ characterListInfo.getAwaitingDay3().length }})
        </Tab>
        <Tab v-if="characterListInfo.getCompletedCharacters().length > 0" value="6">
          Completed Characters ({{ characterListInfo.getCompletedCharacters().length }})
        </Tab>
      </TabList>
      <TabPanels>
        <TabPanel value="0">
          <div v-for="character in characterListInfo.getAwaitingCheckin()" :key="character.id">
            <CharacterTile :character="character" />
          </div>
          <h2 v-if="characterListInfo.getAwaitingCheckin().length == 0">
            No characters awaiting check-in
          </h2>
        </TabPanel>
        <TabPanel value="1">
          <div v-for="character in characterListInfo.getAwaitingGoApproval()" :key="character.id">
            <CharacterTile :character="character" />
          </div>
          <h2 v-if="characterListInfo.getAwaitingGoApproval().length == 0">
            No characters awaiting Go Approval
          </h2>
        </TabPanel>
        <TabPanel value="2">
          <div v-for="character in characterListInfo.getAwaitingCrbCreation()" :key="character.id">
            <CharacterTile :character="character" />
          </div>
          <h2 v-if="characterListInfo.getAwaitingCrbCreation().length == 0">
            No characters awaiting CRB Creation
          </h2>
        </TabPanel>
        <TabPanel value="3">
          <div v-for="character in characterListInfo.getAwaitingPickup()" :key="character.id">
            <CharacterTile :character="character" />
          </div>
          <h2 v-if="characterListInfo.getAwaitingPickup().length == 0">
            No characters awaiting CRB Pickup
          </h2>
        </TabPanel>
        <TabPanel value="4">
          <div v-for="character in characterListInfo.getAwaitingDay2()" :key="character.id">
            <CharacterTile :character="character" />
          </div>
          <h2 v-if="characterListInfo.getAwaitingDay2().length == 0">
            No characters awaiting Day 2 Approval
          </h2>
        </TabPanel>
        <TabPanel value="5">
          <div v-for="character in characterListInfo.getAwaitingDay3()" :key="character.id">
            <CharacterTile :character="character" />
          </div>
          <h2 v-if="characterListInfo.getAwaitingDay3().length == 0">
            No characters awaiting Day 3 Approval
          </h2>
        </TabPanel>
        <TabPanel value="6">
          <div v-for="character in characterListInfo.getCompletedCharacters()" :key="character.id">
            <CharacterTile :character="character" />
          </div>
          <h2 v-if="characterListInfo.getCompletedCharacters().length == 0">
            No one has completed their character yet
          </h2>
        </TabPanel>
      </TabPanels>
    </Tabs>
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
