<script setup lang="ts">
import { nextTick, ref, watch } from 'vue'
import { useConversationStore } from '@/stores/conversation'
import ChatMessage from '@/components/chat/ChatMessage.vue'
import LoadingMessage from '@/components/chat/LoadingMessage.vue'
import EmptyState from '@/components/chat/EmptyState.vue'
import ErrorBanner from '@/components/common/ErrorBanner.vue'

const store = useConversationStore()
const scroller = ref<HTMLElement | null>(null)

async function scrollToBottom() {
  await nextTick()
  if (scroller.value) {
    scroller.value.scrollTop = scroller.value.scrollHeight
  }
}

watch(
  () => [store.messages.length, store.loading, store.error],
  () => {
    void scrollToBottom()
  },
)

function handleSuggest(value: string) {
  void store.sendMessage(value)
}
</script>

<template>
  <section ref="scroller" class="flex-1 overflow-y-auto">
    <div class="flex min-h-full flex-col">
      <div v-if="store.error" class="sticky top-0 z-10 px-4 pt-4">
        <ErrorBanner :message="store.error" @dismiss="store.clearError()" />
      </div>

      <EmptyState
        v-if="!store.hasMessages && !store.loading"
        @suggest="handleSuggest"
      />

      <div v-else class="flex flex-col gap-1 py-4">
        <TransitionGroup name="fade">
          <ChatMessage
            v-for="message in store.messages"
            :key="message.id"
            :message="message"
          />
        </TransitionGroup>

        <LoadingMessage v-if="store.loading" />
      </div>
    </div>
  </section>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: all 0.25s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(6px);
}
</style>
