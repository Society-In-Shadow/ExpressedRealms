<script setup lang="ts">

import { ref, watch } from 'vue'
import { QrcodeStream } from 'vue-qrcode-reader'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'

import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

const eventCheckinInfo = EventCheckinStore()
const loading = ref(true)
const destroyed = ref(false)
const result = ref('')

watch(() => eventCheckinInfo.isReset, () => {
  if (eventCheckinInfo.isReset) {
    loading.value = true
    destroyed.value = false
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
</script>

<template>
  <div class="w-100">
    <qrcode-stream
      v-if="!destroyed && result == ''"
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
  <div>
    <p>If the scanner is not working, you can manually put in the lookup below</p>
    <InputText v-model="result" placeholder="Manual Lookup" minlength="8" maxlength="8" />
    <Button label="Lookup" @click="lookupManual" />
  </div>
</template>
