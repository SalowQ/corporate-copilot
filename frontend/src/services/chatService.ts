import { api } from '@/services/api'
import type { ChatRequest, ChatResponse } from '@/types/chat'

export async function sendChatMessage(message: string): Promise<string> {
  const payload: ChatRequest = { message }
  const { data } = await api.post<ChatResponse>('/chat', payload)
  return data.answer
}
