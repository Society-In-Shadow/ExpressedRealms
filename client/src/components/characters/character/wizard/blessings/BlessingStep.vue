<script setup lang="ts">

import {computed, onBeforeMount, ref} from "vue";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import {makeIdSafe} from "@/utilities/stringUtilities.ts";
import SelectBlessingItem from "@/components/characters/character/wizard/blessings/supports/SelectBlessingItem.vue";
import {
  characterBlessingsStore
} from "@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts";
import {useRoute} from "vue-router";
import type {Blessing, SubCategory} from "@/components/blessings/types.ts";
import Button from "primevue/button";
import EditCharacterBlessing
  from "@/components/characters/character/wizard/blessings/supports/EditCharacterBlessing.vue";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";

const route = useRoute();
const store = blessingsStore();
const characterBlessingData = characterBlessingsStore();
const userInfo = userStore();
const xpInfo = experienceStore();


const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

const showEdit = ref(false);

onBeforeMount(async () => {
  await store.getBlessings()
  showEdit.value = await userInfo.hasUserRole(UserRoles.BlessingsManagementRole);
  await characterBlessingData.getCharacterBlessings(route.params.id)
  await xpInfo.updateExperience(route.params.id);
})

const filteredTypes = computed(() => {
  const selectedIds = characterBlessingData.selectedBlessingIds;
  
  return store.types.map(type => {
    
    const subCategories = type.subCategories.map(subCategory => {
      return {
        name: subCategory.name,
        blessings: subCategory.blessings.filter(x => !selectedIds.includes(x.id))
      } as SubCategory
    })
    
    return {
      ...type,
      subCategories: subCategories
    };
  })
  
})

const wizardContentInfo = wizardContentStore();
const updateWizardContent = (blessing: Blessing) => {
  wizardContentInfo.updateContent(
      {
        headerName: 'Edit Blessing',
        component: EditCharacterBlessing,
        props: { blessing: blessing }
      } as WizardContent
  )
}

</script>

<template>

  <div v-for="type in characterBlessingData.types">
    <h1>Selected {{type.name}}s</h1>
    <div v-for="trait in type.subCategories">
      <h3 class="ml-3 pb-2">{{trait.name}}</h3>
      <div v-for="blessing in trait.blessings">
        <div class="ml-5 d-flex flex-column flex-md-row align-self-center justify-content-between">
          <div>
            <h3 class="p-0 m-0">{{blessing.name}}</h3>
          </div>
          <div class="p-0 m-2">
            <Button label="View" size="small" @click="updateWizardContent(blessing)" />
          </div>
        </div>
      </div>
    </div>
  </div>

  <div v-for="type in filteredTypes" :key="type.name">
    <h1 :id="makeIdSafe(type.name)">{{ type.name }}</h1>
    <div v-for="subCategory in type.subCategories" :key="subCategory.name">
      <h2 class="pl-3 pb-2" :id="makeIdSafe(subCategory.name)">{{ subCategory.name }}</h2>
      <div v-for="blessing in subCategory.blessings" :key="blessing.id">
        <SelectBlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
      </div>
    </div>
  </div>
</template>
