<script setup lang="ts">
import { ChatBubbleBottomCenterTextIcon } from '@heroicons/vue/24/outline'

const emit = defineEmits<{
  suggest: [value: string]
}>()

const suggestions = [
  'Como solicitar férias?',
  'Quais benefícios a empresa oferece?',
  'Como funciona o home office?',
  'Qual o prazo para pedir reembolso?',
]
</script>

<template>
  <div class="mx-auto flex w-full max-w-3xl flex-1 flex-col items-center justify-center px-4 py-10 text-center">
    <div class="mb-4 flex h-14 w-14 items-center justify-center rounded-2xl bg-primary-100 text-primary-700">
      <ChatBubbleBottomCenterTextIcon class="h-7 w-7" />
    </div>

    <h2 class="text-2xl font-semibold text-slate-800">Corporate Copilot AI</h2>
    <p class="mt-2 max-w-md text-sm leading-relaxed text-slate-500">
      Pergunte sobre políticas internas, benefícios, férias, home office e reembolso.
      As respostas são baseadas na base de conhecimento da empresa.
    </p>

    <div class="mt-8 grid w-full gap-3 sm:grid-cols-2">
      <button
        v-for="suggestion in suggestions"
        :key="suggestion"
        type="button"
        class="rounded-xl border border-slate-200 bg-white px-4 py-3 text-left text-sm text-slate-700 shadow-sm transition hover:border-primary-300 hover:bg-primary-50 hover:text-primary-800"
        @click="emit('suggest', suggestion)"
      >
        {{ suggestion }}
      </button>
    </div>
  </div>
</template>
