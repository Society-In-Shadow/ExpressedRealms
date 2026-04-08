import axios from 'axios'

export async function downloadFile(url: string, fallbackName = 'download.pdf') {
  const res = await axios.get(url, { responseType: 'blob' })

  const fileName = fallbackName

  const objectUrl = URL.createObjectURL(res.data)
  const a = Object.assign(document.createElement('a'), { href: objectUrl, download: fileName })
  a.click()
  URL.revokeObjectURL(objectUrl)
}
