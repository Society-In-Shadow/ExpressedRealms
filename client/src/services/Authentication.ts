import axios from 'axios'
import { useQuery } from '@pinia/colada'
import { userInfoQuery } from '@/auth/authStore.ts'

export async function logOff(router) {
  const { refetch } = useQuery(userInfoQuery)
  await axios.post('/auth/logoff').then(async () => {
    await refetch()
    router.push('/')
  })
}
