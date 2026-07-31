<script setup lang="ts">
import { SparklesIcon } from '@heroicons/vue/24/solid'
</script>

<template>
  <article class="mx-auto flex w-full max-w-3xl gap-3 bg-white px-4 py-4">
    <div class="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-primary-100 text-primary-700">
      <SparklesIcon class="h-5 w-5 animate-pulse" />
    </div>

    <div class="flex flex-col justify-center gap-2">
      <p class="text-sm font-semibold text-slate-800">Corporate Copilot</p>
      <div class="flex items-center gap-2 text-sm text-slate-500">
        <span class="flex gap-1">
          <span class="h-1.5 w-1.5 animate-bounce rounded-full bg-primary-500 [animation-delay:-0.2s]" />
          <span class="h-1.5 w-1.5 animate-bounce rounded-full bg-primary-500 [animation-delay:-0.1s]" />
          <span class="h-1.5 w-1.5 animate-bounce rounded-full bg-primary-500" />
        </span>
        <span>A IA está pensando...</span>
      </div>
    </div>
  </article>
</template>
