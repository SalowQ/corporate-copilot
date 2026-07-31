<script setup lang="ts">
import { ref } from 'vue'
import { Bars3Icon } from '@heroicons/vue/24/outline'
import Sidebar from '@/components/layout/Sidebar.vue'
import ChatWindow from '@/components/chat/ChatWindow.vue'
import ChatInput from '@/components/chat/ChatInput.vue'
import { useConversationStore } from '@/stores/conversation'

const store = useConversationStore()
const sidebarOpen = ref(false)

function handleSend(message: string) {
  void store.sendMessage(message)
}
</script>

<template>
  <div class="flex h-full bg-slate-50">
    <div
      v-if="sidebarOpen"
      class="fixed inset-0 z-30 bg-slate-900/40 md:hidden"
      @click="sidebarOpen = false"
    />

    <Sidebar :open="sidebarOpen" @close="sidebarOpen = false" />

    <div class="flex min-w-0 flex-1 flex-col">
      <header class="flex items-center gap-3 border-b border-slate-200 bg-white px-4 py-3 md:hidden">
        <button
          type="button"
          class="rounded-lg p-2 text-slate-600 transition hover:bg-slate-100"
          aria-label="Abrir menu"
          @click="sidebarOpen = true"
        >
          <Bars3Icon class="h-6 w-6" />
        </button>
        <div>
          <p class="text-sm font-semibold text-slate-800">Corporate Copilot AI</p>
          <p class="text-xs text-slate-500">Assistente corporativo</p>
        </div>
      </header>

      <ChatWindow />
      <ChatInput :disabled="store.loading" @send="handleSend" />
    </div>
  </div>
</template>
