import { type Path, useForm } from 'vee-validate'
import type { ObjectSchema } from 'yup'
import { computed, type Ref } from 'vue'
import type { FormField } from '@/FormWrappers/Interfaces/FormField'

type FormFields<T> = {
  [K in keyof T]: FormField<T[K]>
}

export function useGenericForm<T extends Record<string, any>>(validationSchema: ObjectSchema<T>) {
  const { defineField, handleSubmit, errors, handleReset, setErrors } = useForm<T>({
    validationSchema,
    validateOnMount: false,
    keepValuesOnUnmount: false,
  })

  function createFormField<K extends keyof T>(
    fieldName: K,
  ): FormField<T[K]> {
    const path = fieldName as unknown as Path<T>
    return {
      field: defineField(path)[0] as Ref<T[K]>,
      error: computed(() => errors.value[path] as string | undefined),
      label: (validationSchema.fields[path] as any)?.spec?.label ?? fieldName,
      isRequired: (validationSchema.fields[path] as any)?.spec?.optional === false,
    }
  }

  // Generate all form fields dynamically
  const fields = {} as FormFields<T>
  Object.keys(validationSchema.fields).forEach((key) => {
    fields[key as keyof T] = createFormField(key as keyof T)
  })

  return {
    fields,
    handleSubmit,
    handleReset,
    errors,
    setErrors,
  }
}
