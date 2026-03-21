export function makeIdSafe(text: string) {
  return text.trim().toLowerCase().replace(/\s+/g, '-').replace(/[^a-z0-9-]/g, '')
}

/** This will also treat empty paragraph tags as whitespace / null
 *
 * @param input
 */
export function isNullOrWhiteSpace(input: string | null | undefined) {
  return !input || input.trim().length === 0 || input.trim().toLowerCase() === '<p></p>'
}

export function formatSign(input: number | null | undefined) {
  if (input === null || input === undefined) {
    return 'N/A'
  }
  return input >= 0 ? `+${input}` : `${input}`
}
