import axios, { AxiosError } from 'axios'

const baseURL = import.meta.env.VITE_API_BASE_URL

if (!baseURL) {
  console.warn('VITE_API_BASE_URL não está definida. Configure a variável de ambiente.')
}

export const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 60000,
})

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError<{ detail?: string; title?: string }>) => {
    const friendlyMessage = resolveErrorMessage(error)
    return Promise.reject(new Error(friendlyMessage))
  },
)

function resolveErrorMessage(error: AxiosError<{ detail?: string; title?: string }>): string {
  if (error.code === 'ECONNABORTED') {
    return 'A solicitação demorou demais. Tente novamente em instantes.'
  }

  if (!error.response) {
    return 'Não foi possível conectar à API. Verifique se o serviço está online.'
  }

  const detail = error.response.data?.detail || error.response.data?.title

  if (detail) {
    return detail
  }

  if (error.response.status >= 500) {
    return 'Ocorreu um erro no servidor. Tente novamente mais tarde.'
  }

  return 'Não foi possível processar sua pergunta. Verifique os dados e tente novamente.'
}
