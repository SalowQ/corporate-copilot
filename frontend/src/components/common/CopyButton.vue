<script setup lang="ts">
import { ref } from 'vue'
import { ClipboardDocumentIcon, CheckIcon } from '@heroicons/vue/24/outline'

const props = defineProps<{
  text: string
}>()

const copied = ref(false)

async function copy() {
  try {
    await navigator.clipboard.writeText(props.text)
    copied.value = true
    window.setTimeout(() => {
      copied.value = false
    }, 1800)
  } catch {
    copied.value = false
  }
}
</script>

<template>
  <button
    type="button"
    class="inline-flex items-center gap-1 rounded-lg px-2 py-1 text-xs text-slate-500 transition hover:bg-slate-100 hover:text-primary-700"
    :title="copied ? 'Copiado' : 'Copiar resposta'"
    @click="copy"
  >
    <CheckIcon v-if="copied" class="h-4 w-4 text-primary-600" />
    <ClipboardDocumentIcon v-else class="h-4 w-4" />
    <span>{{ copied ? 'Copiado' : 'Copiar' }}</span>
  </button>
</template>
