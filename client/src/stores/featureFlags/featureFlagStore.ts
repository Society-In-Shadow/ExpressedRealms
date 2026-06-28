import { defineQueryOptions, useQueryCache } from '@pinia/colada'
import { featureFlagEndpoints } from '@/stores/featureFlags/featureFlagEndpoints.ts'
import { type FeatureFlag, FeatureFlags } from '@/types/FeatureFlags.ts'

export const LOGIN_QUERY_KEYS = {
  root: ['featureFlags'] as const,
}

export const featureFlagQuery = defineQueryOptions({
  key: LOGIN_QUERY_KEYS.root,
  query: featureFlagEndpoints.availableFlags,
  gcTime: false,
})

type FeatureFlagCheck = {
  [P in keyof typeof FeatureFlags]: boolean
}

export const hasFlag = new Proxy({} as FeatureFlagCheck, {
  get(_, property: keyof typeof FeatureFlags) {
    const queryCache = useQueryCache()
    const query = queryCache.get(featureFlagQuery.key)
    const flags = query?.state.value.data ?? []
    return flags.includes(FeatureFlags[property])
  },
})

export const checkFlag = (flag: FeatureFlag) => {
  const queryCache = useQueryCache()
  const query = queryCache.get(featureFlagQuery.key)
  const flags = query?.state.value.data ?? []
  return flags.includes(flag)
}
