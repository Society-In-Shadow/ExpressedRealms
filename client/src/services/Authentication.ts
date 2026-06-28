import axios from 'axios'
import { userInfoQuery } from '@/auth/authStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { featureFlagQuery } from '@/stores/featureFlags/featureFlagStore.ts'

export async function logOff(router) {
  const { refetch } = useQueryWithLoading(userInfoQuery)
  const { refetch: featureRefetch } = useQueryWithLoading(featureFlagQuery)

  await axios.post('/auth/logoff').then(async () => {
    await refetch()
    await featureRefetch()
    router.push('/')
  })
}
