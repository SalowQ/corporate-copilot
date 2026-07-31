<script setup lang="ts">
import { computed } from 'vue'
import { marked } from 'marked'
import DOMPurify from 'dompurify'
import hljs from 'highlight.js'

const props = defineProps<{
  content: string
}>()

marked.setOptions({
  breaks: true,
  gfm: true,
})

const html = computed(() => {
  const raw = marked.parse(props.content, { async: false }) as string
  const sanitized = DOMPurify.sanitize(raw)

  return sanitized.replace(
    /<pre><code class="language-([^"]+)">([\s\S]*?)<\/code><\/pre>/g,
    (_match, language: string, code: string) => {
      const decoded = decodeHtml(code)
      const highlighted = highlightCode(decoded, language)
      return `<pre><code class="hljs language-${language}">${highlighted}</code></pre>`
    },
  ).replace(
    /<pre><code>([\s\S]*?)<\/code><\/pre>/g,
    (_match, code: string) => {
      const decoded = decodeHtml(code)
      const highlighted = hljs.highlightAuto(decoded).value
      return `<pre><code class="hljs">${highlighted}</code></pre>`
    },
  )
})

function decodeHtml(value: string): string {
  const textarea = document.createElement('textarea')
  textarea.innerHTML = value
  return textarea.value
}

function highlightCode(code: string, language: string): string {
  if (hljs.getLanguage(language)) {
    return hljs.highlight(code, { language }).value
  }

  return hljs.highlightAuto(code).value
}
</script>

<template>
  <div class="markdown-body" v-html="html" />
</template>
