<script setup lang="ts">
import { computed } from 'vue'
import { UserCircleIcon, SparklesIcon } from '@heroicons/vue/24/solid'
import type { ChatMessage } from '@/types/chat'
import MarkdownRenderer from '@/components/common/MarkdownRenderer.vue'
import CopyButton from '@/components/common/CopyButton.vue'

const props = defineProps<{
  message: ChatMessage
}>()

const isAssistant = computed(() => props.message.role === 'assistant')

const formattedDate = computed(() => {
  return new Intl.DateTimeFormat('pt-BR', {
    dateStyle: 'short',
    timeStyle: 'short',
  }).format(new Date(props.message.createdAt))
})
</script>

<template>
  <article
    class="group mx-auto flex w-full max-w-3xl gap-3 px-4 py-4 transition"
    :class="isAssistant ? 'bg-white' : 'bg-transparent'"
  >
    <div
      class="flex h-9 w-9 shrink-0 items-center justify-center rounded-full"
      :class="isAssistant ? 'bg-primary-100 text-primary-700' : 'bg-slate-200 text-slate-700'"
    >
      <SparklesIcon v-if="isAssistant" class="h-5 w-5" />
      <UserCircleIcon v-else class="h-5 w-5" />
    </div>

    <div class="min-w-0 flex-1">
      <div class="mb-1 flex flex-wrap items-center gap-x-3 gap-y-1">
        <span class="text-sm font-semibold text-slate-800">{{ message.name }}</span>
        <time class="text-xs text-slate-400">{{ formattedDate }}</time>
      </div>

      <div class="text-sm text-slate-700">
        <MarkdownRenderer v-if="isAssistant" :content="message.content" />
        <p v-else class="whitespace-pre-wrap leading-relaxed">{{ message.content }}</p>
      </div>

      <div v-if="isAssistant" class="mt-2 opacity-100 transition md:opacity-0 md:group-hover:opacity-100">
        <CopyButton :text="message.content" />
      </div>
    </div>
  </article>
</template>
