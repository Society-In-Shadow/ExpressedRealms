import type { DateTime } from 'luxon'

export function formatDate(date: DateTime) {
  if (date)
    return date.toFormat('cccc, LLLL dd, yyyy')
  return ''
}
