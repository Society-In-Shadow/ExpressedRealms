import axios from 'axios'
import { userStore } from '@/stores/userStore'

export async function logOff(router) {
  const userInfo = userStore()
  axios.post('/auth/logoff').then(() => {
    userInfo.$reset()
    router.push('/')
  })
}
