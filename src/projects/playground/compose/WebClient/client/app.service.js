import axios from 'axios'

axios.defaults.baseURL = 'http://localhost:1921/v1'

axios.interceptors.request.use(function (config) {
  if (typeof window === 'undefined') {
    return config
  }
  const token = window.localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

const appService = {
  getPosts (categoryId) {
    return new Promise((resolve) => {
      axios.get(`/posts?categories=${categoryId}&per_page=6`)
        .then(response => {
          resolve(response.data)
        })
    })
  },
  login (credentials) {
    return new Promise((resolve, reject) => {
      axios.post('/token', credentials)
        .then(response => {
          resolve(response.data)
        }).catch(response => {
          reject(response.status)
        })
    })
  },
  getProfile () {
    return new Promise((resolve) => {
      axios.get('/token/profile')
        .then(response => {
          resolve(response.data)
        })
    })
  },
  slugify (input) {
    return new Promise((resolve) => {
      axios.get('/slug/' + input)
        .then(response => {
          resolve(response.data)
        })
    })
  }
}

export default appService
