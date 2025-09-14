<script setup lang="ts">

import Button from "primevue/button";
import Card from "primevue/card";
import {useRouter} from "vue-router";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";

const router = useRouter();
const userInfo = userStore();

async function redirectToAdd(){
  
  if(await userInfo.hasFeatureFlag(FeatureFlags.ShowCharacterWizard)){
    router.push('/characters/wizard')
  }else{
    router.push('/characters/add')
  }
  
}

</script>

<template>
  <Card class="mb-3 characterTile">
    <template #content>
      <div style="text-align: center;" class="align-self-center">
        <Button data-cy="character-delete-button" label="Add Character" @click="redirectToAdd" />
      </div>
    </template>
  </Card>
</template>
