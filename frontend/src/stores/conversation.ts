import { defineStore } from 'pinia'
import { computed, ref, watch } from 'vue'
import { sendChatMessage } from '@/services/chatService'
import type { ChatMessage } from '@/types/chat'

const STORAGE_KEY = 'corporate-copilot-messages'

function createId(): string {
  return crypto.randomUUID()
}

function loadMessages(): ChatMessage[] {
  try {
    const raw = sessionStorage.getItem(STORAGE_KEY)
    if (!raw) return []
    return JSON.parse(raw) as ChatMessage[]
  } catch {
    return []
  }
}

export const useConversationStore = defineStore('conversation', () => {
  const messages = ref<ChatMessage[]>(loadMessages())
  const loading = ref(false)
  const error = ref<string | null>(null)

  const hasMessages = computed(() => messages.value.length > 0)

  watch(
    messages,
    (value) => {
      sessionStorage.setItem(STORAGE_KEY, JSON.stringify(value))
    },
    { deep: true },
  )

  function clearError() {
    error.value = null
  }

  function clearConversation() {
    messages.value = []
    error.value = null
    sessionStorage.removeItem(STORAGE_KEY)
  }

  function newConversation() {
    clearConversation()
  }

  async function sendMessage(content: string) {
    const trimmed = content.trim()
    if (!trimmed || loading.value) return

    clearError()

    const userMessage: ChatMessage = {
      id: createId(),
      role: 'user',
      content: trimmed,
      createdAt: new Date().toISOString(),
      name: 'Você',
    }

    messages.value.push(userMessage)
    loading.value = true

    try {
      const answer = await sendChatMessage(trimmed)

      messages.value.push({
        id: createId(),
        role: 'assistant',
        content: answer,
        createdAt: new Date().toISOString(),
        name: 'Corporate Copilot',
      })
    } catch (err) {
      error.value =
        err instanceof Error
          ? err.message
          : 'Não foi possível obter uma resposta da IA.'
    } finally {
      loading.value = false
    }
  }

  return {
    messages,
    loading,
    error,
    hasMessages,
    sendMessage,
    clearConversation,
    newConversation,
    clearError,
  }
})
