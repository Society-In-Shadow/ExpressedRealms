<script setup lang="ts">

import {onMounted, ref} from "vue";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore";
import BlessingItem from "@/components/blessings/BlessingItem.vue";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import AddBlessing from "@/components/blessings/AddBlessing.vue";
import Button from "primevue/button";

const store = blessingsStore();
const userInfo = userStore();


const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

const showEdit = ref(false);
const showAdd = ref(false);

const toggleAdd = () =>{
  showAdd.value = !showAdd.value;
}

onMounted(async () => {
  await store.getBlessings()
  showEdit.value = await userInfo.hasUserRole(UserRoles.BlessingsManagementRole);
})

</script>

<template>
  <div v-if="showEdit" class="text-right">
    <Button v-if="!showAdd" label="Add Advantage" @click="toggleAdd" />
    <Button v-else label="Cancel Add" @click="toggleAdd" />
    
  </div>
  <AddBlessing v-if="showAdd" @canceled="toggleAdd" />
  <div>
    <h1>Advantage</h1>
    <div v-for="subCategory in store.advantages">
      <h2>{{ subCategory.name }}</h2>
      <div v-for="blessing in subCategory.blessings" :key="blessing.id">
        <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
      </div>
    </div>
  </div>

  <div>
    <h1>Disadvantage</h1>
    <div v-for="subCategory in store.disadvantages">
      <h2>{{ subCategory.name }}</h2>
      <div v-for="blessing in subCategory.blessings" :key="blessing.id">
        <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
      </div>
    </div>
  </div>

  <div>
    <h1>Mixed Blessings</h1>
    <div v-for="subCategory in store.mixedBlessings">
      <h2>{{ subCategory.name }}</h2>
      <div v-for="blessing in subCategory.blessings" :key="blessing.id">
        <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
      </div>
    </div>
  </div>
</template>
