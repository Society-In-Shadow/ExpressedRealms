export function makeIdSafe(text: string) {
    return text.trim().toLowerCase().replace(/\s+/g, '-').replace(/[^a-z0-9-]/g, '');
}

export function isNullOrWhiteSpace(input: string | null | undefined) {
    return !input || input.trim().length === 0;
}