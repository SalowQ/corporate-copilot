<script setup lang="ts">
import { XMarkIcon, ExclamationTriangleIcon } from '@heroicons/vue/24/outline'

defineProps<{
  message: string
}>()

const emit = defineEmits<{
  dismiss: []
}>()
</script>

<template>
  <div
    class="mx-auto flex w-full max-w-3xl items-start gap-3 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700 shadow-sm"
    role="alert"
  >
    <ExclamationTriangleIcon class="mt-0.5 h-5 w-5 shrink-0" />
    <p class="flex-1 leading-relaxed">{{ message }}</p>
    <button
      type="button"
      class="rounded-md p-1 text-red-500 transition hover:bg-red-100"
      aria-label="Fechar aviso de erro"
      @click="emit('dismiss')"
    >
      <XMarkIcon class="h-4 w-4" />
    </button>
  </div>
</template>
