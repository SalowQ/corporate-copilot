<script setup lang="ts">
import { ref } from 'vue'
import { PaperAirplaneIcon } from '@heroicons/vue/24/solid'

const props = defineProps<{
  disabled?: boolean
}>()

const emit = defineEmits<{
  send: [value: string]
}>()

const message = ref('')

function submit() {
  const value = message.value.trim()
  if (!value || props.disabled) return

  emit('send', value)
  message.value = ''
}

function onKeydown(event: KeyboardEvent) {
  if (event.key === 'Enter' && !event.shiftKey) {
    event.preventDefault()
    submit()
  }
}
</script>

<template>
  <div class="border-t border-slate-200 bg-white/95 px-4 py-4 backdrop-blur">
    <form
      class="mx-auto flex w-full max-w-3xl items-end gap-3 rounded-2xl border border-slate-200 bg-white p-3 shadow-sm focus-within:border-primary-300 focus-within:ring-2 focus-within:ring-primary-100"
      @submit.prevent="submit"
    >
      <textarea
        v-model="message"
        rows="1"
        class="max-h-40 min-h-[44px] flex-1 resize-none bg-transparent px-2 py-2 text-sm text-slate-800 outline-none placeholder:text-slate-400"
        placeholder="Envie uma mensagem para o Corporate Copilot..."
        :disabled="disabled"
        @keydown="onKeydown"
      />

      <button
        type="submit"
        class="inline-flex h-11 w-11 shrink-0 items-center justify-center rounded-xl bg-primary-600 text-white transition hover:bg-primary-700 disabled:cursor-not-allowed disabled:bg-slate-300"
        :disabled="disabled || !message.trim()"
        aria-label="Enviar mensagem"
      >
        <PaperAirplaneIcon class="h-5 w-5" />
      </button>
    </form>

    <p class="mx-auto mt-2 max-w-3xl text-center text-xs text-slate-400">
      Enter envia · Shift+Enter quebra linha
    </p>
  </div>
</template>
