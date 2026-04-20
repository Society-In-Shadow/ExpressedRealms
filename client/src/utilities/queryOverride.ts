import { useQuery, type UseQueryOptions, type UseQueryReturn } from '@pinia/colada'
import { computed } from 'vue'

export function useQueryWithLoading<TData, TError = Error>(
  options: UseQueryOptions<TData, TError>,
): UseQueryReturn<TData, TError> & { isLoading: ComputedRef<boolean> } {
  const query = useQuery(options)

  const isLoading = computed(
    () => query.status.value === 'pending' || query.asyncStatus.value === 'loading',
  )

  return { ...query, isLoading }
}
