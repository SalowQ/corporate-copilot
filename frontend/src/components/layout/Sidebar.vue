<script setup lang="ts">
import {
  ChatBubbleLeftRightIcon,
  PlusIcon,
  TrashIcon,
  XMarkIcon,
} from '@heroicons/vue/24/outline'
import { useConversationStore } from '@/stores/conversation'

defineProps<{
  open: boolean
}>()

const emit = defineEmits<{
  close: []
}>()

const store = useConversationStore()
const version = import.meta.env.VITE_APP_VERSION || '1.0.0'

function handleNewConversation() {
  store.newConversation()
  emit('close')
}

function handleClearConversation() {
  store.clearConversation()
  emit('close')
}
</script>

<template>
  <aside
    class="fixed inset-y-0 left-0 z-40 flex w-72 flex-col border-r border-primary-900/10 bg-primary-900 text-white transition-transform duration-300 md:static md:translate-x-0"
    :class="open ? 'translate-x-0' : '-translate-x-full'"
  >
    <div class="flex items-center justify-between px-5 py-5">
      <div class="flex items-center gap-3">
        <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-white/10">
          <ChatBubbleLeftRightIcon class="h-6 w-6 text-primary-100" />
        </div>
        <div>
          <p class="text-sm font-medium text-primary-100">Corporate</p>
          <h1 class="text-lg font-semibold leading-tight">Copilot AI</h1>
        </div>
      </div>

      <button
        type="button"
        class="rounded-lg p-2 text-primary-100 transition hover:bg-white/10 md:hidden"
        aria-label="Fechar menu"
        @click="emit('close')"
      >
        <XMarkIcon class="h-5 w-5" />
      </button>
    </div>

    <div class="flex flex-col gap-2 px-4">
      <button
        type="button"
        class="flex items-center gap-2 rounded-xl bg-primary-700 px-4 py-3 text-sm font-medium transition hover:bg-primary-600"
        @click="handleNewConversation"
      >
        <PlusIcon class="h-5 w-5" />
        Nova conversa
      </button>

      <button
        type="button"
        class="flex items-center gap-2 rounded-xl border border-white/10 px-4 py-3 text-sm font-medium text-primary-50 transition hover:bg-white/5"
        @click="handleClearConversation"
      >
        <TrashIcon class="h-5 w-5" />
        Limpar conversa
      </button>
    </div>

    <div class="mt-auto border-t border-white/10 px-5 py-4 text-xs text-primary-200">
      <p>Assistente corporativo interno</p>
      <p class="mt-1">Versão {{ version }}</p>
    </div>
  </aside>
</template>
