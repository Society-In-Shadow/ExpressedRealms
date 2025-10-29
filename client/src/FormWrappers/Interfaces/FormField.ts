import type { ComputedRef, Ref } from 'vue'
import { DateTime } from 'luxon'

export interface FormField {
  field: Ref<string | object | boolean | number | null | DateTime> // Ref for the input value
  error: ComputedRef<string | undefined> // ComputedRef for potential error
  label: string // Label for the field
  isRequired: boolean // dynamically generated from the validations applied to the field
}
