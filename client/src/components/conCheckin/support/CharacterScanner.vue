<script setup lang="ts">
import { nextTick, onMounted, onUnmounted, ref, watch } from 'vue'
import { QrcodeStream } from 'vue-qrcode-reader'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

const eventCheckinInfo = EventCheckinStore()
const loading = ref(true)
const destroyed = ref(false)
const result = ref('')

watch(() => eventCheckinInfo.isReset, () => {
  if (eventCheckinInfo.isReset) resetCamera()
})

function resetCamera() {
  destroyed.value = true
  loading.value = true
  result.value = ''
  // Force renderer to stop and restart the camera
  nextTick(() => {
    destroyed.value = false
  })
}

const emit = defineEmits<{
  detectedCode: [lookupId: string]
}>()

function onCameraOn() {
  clearInterval(timer)
  startTimer()
  loading.value = false
}

async function onDetect(detectedCodes) {
  result.value = detectedCodes.map(code => code.rawValue)[0]
  destroyed.value = true
  clearInterval(timer)
  hidden.value = true
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

// Only have the camera showing for 30 seconds total
const TOTAL = 30
const timeLeft = ref(TOTAL)
const hidden = ref(false)
let startTime: number | null = null
let timer: number = 0

function startTimer() {
  startTime = Date.now()
  timer = setInterval(() => {
    const elapsed = Math.floor((Date.now() - startTime!) / 1000)
    timeLeft.value = Math.max(0, TOTAL - elapsed)
    if (timeLeft.value <= 0) {
      hidden.value = true
      destroyed.value = true
      clearInterval(timer)
    }
  }, 1000)
}

function resetTimer() {
  clearInterval(timer)
  timeLeft.value = TOTAL
  hidden.value = false
  resetCamera()
}

// If they move to another tab, trigger camera state
function handleVisibilityChange() {
  if (document.visibilityState === 'visible' && !hidden.value) {
    resetCamera()
  }
}

onMounted(() => document.addEventListener('visibilitychange', handleVisibilityChange))
onUnmounted(() => {
  document.removeEventListener('visibilitychange', handleVisibilityChange)
  clearInterval(timer)
})
</script>

<template>
  <div>
    <p>If the scanner is not working below, you can manually put in the lookup below</p>
    <InputText v-model="result" placeholder="Manual Lookup" minlength="8" maxlength="8" :disabled="eventCheckinInfo.foundInfo" />
    <Button label="Lookup" :disabled="eventCheckinInfo.foundInfo" @click="lookupManual" />
  </div>
  <div class="w-100 mt-3 mb-3">
    <div v-if="hidden">
      <Button label="Show Camera (Shuts off to Preserve Power)" @click="resetTimer" />
    </div>
    <div v-else>
      <qrcode-stream
        v-if="!destroyed && result == ''"
        :track="paintOutline"
        @camera-on="onCameraOn"
        @detect="onDetect"
      >
        <div v-if="loading" class="loading-indicator">
          Loading...
        </div>
      </qrcode-stream>
    </div>
  </div>
</template>
