<script setup lang="ts">

import { computed, ref, watch } from 'vue'
import { QrcodeStream } from 'vue-qrcode-reader'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'

import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

const eventCheckinInfo = EventCheckinStore()
const loading = ref(true)
const showScanner = ref(false)
const scannerTimedOut = ref(false)
const result = ref('')

// Disable Camera When:
// - 45 seconds elasped
// - The QR Code was found
// - Manual lookup initiated && succeeded
// - Reset gets called

// Prevent automatic lookup if it doesn't follow the lookup pattern
// Need a way to show when it's invalid, and need to ability to change it
// Need a way to re-enable lookup camera

watch(() => eventCheckinInfo.isReset, () => {
  if (eventCheckinInfo.isReset) {
    loading.value = true
    showScanner.value = false
    result.value = ''
  }
})

const emit = defineEmits<{
  detectedCode: [lookupId: string]
}>()

function onCameraOn() {
  loading.value = false
}

async function onDetect(detectedCodes) {
  result.value = detectedCodes.map(code => code.rawValue)[0]
  emit('detectedCode', result.value)
}

async function lookupManual() {
  emit('detectedCode', result.value)
}

function paintOutline(detectedCodes, ctx) {
  for (const detectedCode of detectedCodes) {
    const [firstPoint, ...otherPoints] = detectedCode.cornerPoints

    ctx.strokeStyle = 'red'

    ctx.beginPath()
    ctx.moveTo(firstPoint.x, firstPoint.y)
    for (const { x, y } of otherPoints) {
      ctx.lineTo(x, y)
    }
    ctx.lineTo(firstPoint.x, firstPoint.y)
    ctx.closePath()
    ctx.stroke()
  }
}

const showReader = computed(() => !eventCheckinInfo.foundInfo && !showScanner.value)

const FIVE_MINUTES = 45 // seconds
let remaining = ref(FIVE_MINUTES)

const timeRemaining = computed(() => {
  const minutes = Math.floor(remaining.value / 60)
  const seconds = remaining.value % 60
  return `${minutes}:${seconds.toString().padStart(2, '0')}`
})

function updateCountdown() {
  if (remaining.value <= 0) {
    triggerRefresh()
    remaining.value = FIVE_MINUTES
  }
  else {
    remaining.value--
  }
}

async function triggerRefresh() {
  await refreshData()
}

</script>

<template>
  <div>
    <p>If the scanner is not working below, you can manually put in the lookup below</p>
    <InputText v-model="result" placeholder="Manual Lookup" minlength="8" maxlength="8" :disabled="eventCheckinInfo.foundInfo" />
    <Button label="Lookup" :disabled="eventCheckinInfo.foundInfo" @click="lookupManual" />
  </div>
  <p>Auto shutoff in {</p>
  <div class="w-100">
    <p />
    <qrcode-stream
      v-if="!showScanner && result == ''"
      :track="paintOutline"
      @camera-on="onCameraOn"
      @detect="onDetect"
    >
      <div
        v-if="loading"
        class="loading-indicator"
      >
        Loading...
      </div>
    </qrcode-stream>
  </div>
</template>
