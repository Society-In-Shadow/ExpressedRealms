import { ref, type Ref, watch } from 'vue'

export function handleValidationErrors(error: any, onValidationError?: (errors: Record<string, any>) => void) {
  const isValidationError
    = error?.response?.data?.type
      === 'https://tools.ietf.org/html/rfc9110#section-15.5.1'

  if (!isValidationError) {
    return
  }

  onValidationError?.(error?.response?.data?.errors)
}

export function useHydrateFormOnce<T>(
  source: Ref<T | undefined>,
  hydrate: (value: T) => void,
) {
  const hydrated = ref(false)

  watch(
    source,
    (val) => {
      if (!val || hydrated.value) return

      hydrate(val)
      hydrated.value = true
    },
    { immediate: true },
  )
}
