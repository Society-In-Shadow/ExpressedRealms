<script setup lang="ts">

import { onMounted, ref } from 'vue'
import { blessingsStore } from '@/components/blessings/stores/blessingsStore'
import BlessingItem from '@/components/blessings/BlessingItem.vue'
import { UserRoles, userStore } from '@/stores/userStore.ts'
import AddBlessing from '@/components/blessings/AddBlessing.vue'
import Button from 'primevue/button'
import { makeIdSafe } from '@/utilities/stringUtilities.ts'

const store = blessingsStore()
const userInfo = userStore()

const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

const showEdit = ref(false)
const showAdd = ref(false)

const toggleAdd = () => {
  showAdd.value = !showAdd.value
}

onMounted(async () => {
  await store.getBlessings()
  showEdit.value = await userInfo.hasUserRole(UserRoles.BlessingsManagementRole)
})

</script>

<template>
  <div v-if="showEdit" class="text-right">
    <Button v-if="!showAdd" label="Add Advantage" @click="toggleAdd" />
    <Button v-else label="Cancel Add" @click="toggleAdd" />
  </div>
  <AddBlessing v-if="showAdd" @canceled="toggleAdd" />
  <div v-for="type in store.types" :key="type.name">
    <h1 :id="makeIdSafe(type.name)">
      {{ type.name }}
    </h1>
    <div v-for="subCategory in type.subCategories" :key="subCategory.name">
      <h2 :id="makeIdSafe(`${subCategory.name}-${type.name}`)">
        {{ subCategory.name }}
      </h2>
      <div v-for="blessing in subCategory.blessings" :key="blessing.id">
        <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
      </div>
    </div>
  </div>
</template>
