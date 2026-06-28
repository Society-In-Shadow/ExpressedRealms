import axios from 'axios'

export const featureFlagEndpoints = {
  availableFlags: (): Promise<string[]> => axios.get('/navMenu/featureFlags')
    .then(async (response) => { return response.data.featureFlags }),
}
