import ToastEventBus from 'primevue/toasteventbus'

const isMobile = window.matchMedia('(max-width: 767.98px)').matches

function success(message: string): void
function success(title: string, message: string): void
function success(title: string, message?: string): void {
  if (message !== undefined) {
    ToastEventBus.emit('add', { severity: 'success', summary: title, detail: message, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
  else {
    ToastEventBus.emit('add', { severity: 'success', summary: 'Success', detail: title, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
}

function error(message: string): void
function error(title: string, message: string): void
function error(title: string, message?: string): void {
  if (message !== undefined) {
    ToastEventBus.emit('add', { severity: 'error', summary: title, detail: message, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
  else {
    ToastEventBus.emit('add', { severity: 'error', summary: 'Error', detail: title, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
}

function info(message: string): void
function info(title: string, message: string): void
function info(title: string, message?: string): void {
  if (message !== undefined) {
    ToastEventBus.emit('add', { severity: 'info', summary: title, detail: message, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
  else {
    ToastEventBus.emit('add', { severity: 'info', summary: 'Information', detail: title, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
}

function warning(message: string): void
function warning(title: string, message: string): void
function warning(title: string, message?: string): void {
  if (message !== undefined) {
    ToastEventBus.emit('add', { severity: 'warn', summary: title, detail: message, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
  else {
    ToastEventBus.emit('add', { severity: 'warn', summary: 'Warning', detail: title, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
  }
}

function secondary(title: string, message: string): void {
  ToastEventBus.emit('add', { severity: 'secondary', summary: title, detail: message, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
}

function contrast(title: string, message: string) {
  ToastEventBus.emit('add', { severity: 'contrast', summary: title, detail: message, life: 3000, group: isMobile ? 'mobile' : 'desktop' })
}

export default {
  success,
  error,
  info,
  warning,
  secondary,
  contrast,
}
