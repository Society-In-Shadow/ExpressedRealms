<script setup lang="ts">

import ToggleSwitch from 'primevue/toggleswitch';
import axios from "axios";
import {onMounted, ref} from 'vue'
import toaster from "@/services/Toasters";
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";

const roles = ref([ {}, {}]);
const isLoading = ref(true);

const props = defineProps({
  userId: {
    type: String,
    required: true,
  }
});

function fetchData() {
  axios.get(`/admin/user/${props.userId}/roles`)
      .then((response) => {
        roles.value = response.data.roles;
        isLoading.value = false;
      });
}

function updateRole(roleName: string, isEnabled: boolean) {
  axios.put(`/admin/user/${props.userId}/role`,
      {
        userId: props.userId,
        roleName: roleName,
        isEnabled: isEnabled
      })
      .then((response) => {
        toaster.success(`Successfully Updated Policy!`);
      });
}

onMounted(() =>{
  fetchData();
})

</script>

<template>
  
  <div class="p-2" v-for="role in roles" :key="role.name">

    <SkeletonWrapper :show-skeleton="isLoading" height="3rem" width="20em">
      <div class="d-flex d-flex-column">
        <div class="align-self-center">
          <ToggleSwitch v-model="role.isEnabled" @change="updateRole(role.name, role.isEnabled)"/>
        </div>
        <div class="align-self-center p-3">
          {{ role.name }}
        </div>
      </div>
    </SkeletonWrapper>

  </div>
</template>

<style scoped>

</style>