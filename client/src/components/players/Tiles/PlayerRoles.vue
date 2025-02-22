<script setup lang="ts">

import ToggleSwitch from 'primevue/toggleswitch';
import {onMounted, ref} from 'vue'
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import type {RoleInfo} from "@/components/players/Objects/RoleInfo";
import { fetchData, updateRole } from "@/components/players/Services/PlayerRoleService";

const roles = ref<Array<RoleInfo>>([ {}, {}]);
const isLoading = ref(true);

const props = defineProps({
  userId: {
    type: String,
    required: true,
  }
});

onMounted(() =>{
  fetchData(props.userId)
      .then((response) => {
        roles.value = response.data.roles;
        isLoading.value = false;
      });
})

</script>

<template>
  <div class="p-2" v-for="role in roles" :key="role.name">
    <SkeletonWrapper :show-skeleton="isLoading" height="3rem" width="20em">
      <div class="d-flex d-flex-column">
        <div class="align-self-center">
          <ToggleSwitch v-model="role.isEnabled" @change="updateRole(props.userId, role.name, role.isEnabled)"/>
        </div>
        <div class="align-self-center p-3">
          {{ role.name }}
        </div>
      </div>
    </SkeletonWrapper>
  </div>
</template>
