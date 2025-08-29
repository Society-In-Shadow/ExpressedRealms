<script setup lang="ts">

import axios from "axios";
import toaster from "@/services/Toasters";
import Button from "primevue/button";
import {useRouter} from "vue-router";
import {nameField, navMenuImageField, shortDescriptionField, handleSubmit} from "@/components/expressions/expression/AddExpressionValidation";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import {cmsStore} from "@/stores/cmsStore.ts";
import {inject, ref} from "vue";
const router = useRouter()

const cmsData = cmsStore();

const dialogRef = inject('dialogRef');
const expressionTypeId = ref(dialogRef.value.data.expressionTypeId);

const onSubmit = handleSubmit((values) => {
  axios.post(`/expression/`, {
    name: values.name,
    shortDescription: values.shortDescription,
    navMenuImage: values.navMenuImage,
    expressionTypeId: expressionTypeId.value
  }).then(async (response) => {
    await cmsData.refreshCmsInformation();
    let slug = '';
    console.log(response);
    switch(expressionTypeId.value){
      case 1:
        toaster.success(`Successfully added ${values.name} Expression as a Draft!`);
        slug = cmsData.expressionItems.filter(x => x.id == response.data)[0].slug;
        router.push("/expressions/" + slug);
        break;
      case 13:
        toaster.success(`Successfully added ${values.name} Rule Book Section as a Draft!`);
        slug = cmsData.rulebookItems.filter(x => x.id == response.data)[0].slug;
        console.log("slug " + slug)
        await router.push("/rulebook/" + slug);
        break;
      case 14:
        toaster.success(`Successfully added ${values.name} World Background Section as a Draft!`);
        slug = cmsData.worldBackgroundItems.filter(x => x.id == response.data)[0].slug;
        router.push("/worldbackground/" + slug);
        break;
    }
    dialogRef.value.close();

  });
});

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="nameField" />
    <FormTextAreaWrapper v-model="shortDescriptionField" />
    <div class="d-flex align-items-center gap-3 ">
      <div class="flex-shrink-1">
        <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem">
          <i :class="['material-symbols-outlined', 'text-white']"> {{navMenuImageField.field.value}}</i>
        </span>
      </div>
      <div class="flex-grow-1">
        <FormInputTextWrapper v-model="navMenuImageField" />
      </div>
    </div>
    <p>List of icons can be found here : <a href="https://fonts.google.com/icons?icon.size=24&icon.color=%23e3e3e3">Google Material Design Fonts</a></p>
    <p>You only need to add the name of the icon, with spaces being replaced with underlines.</p>

    <Button data-cy="add-expression-button" label="Add" class="w-100 mb-2" type="submit" />
  </form>
</template>

