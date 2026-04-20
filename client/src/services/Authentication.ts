import axios from 'axios'
import { userInfoQuery } from '@/auth/authStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'

export async function logOff(router) {
  const { refetch } = useQueryWithLoading(userInfoQuery)
  await axios.post('/auth/logoff').then(async () => {
    await refetch()
    router.push('/')
  })
}
