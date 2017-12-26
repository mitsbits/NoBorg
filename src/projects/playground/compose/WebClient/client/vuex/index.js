import Vue from 'vue'
import Vuex from 'vuex'
import appService from '../app.service.js'
import postsModule from './posts'

Vue.use(Vuex)

const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

const state = {
  isAuthenticated: false,
  counter: 0,
  profile: ''
}
const store = new Vuex.Store({
  modules: {
    postsModule
  },
  state,
  getters: {
    isAuthenticated: (state) => {
      return state.isAuthenticated
    },
    counter: (state) => {
      return state.counter
    },
    profile: (state) => {
      return state.profile
    }
  },
  actions: {
    logout (context) {
      context.commit('logout')
    },
    login (context, credentials) {
      return new Promise((resolve) => {
        appService.login(credentials)
          .then((data) => {
            context.commit('login', data, credentials.username)
            resolve()
          })
          .catch(() => {
            if (typeof window !== 'undefined') { window.alert('Could not login!') }
          })
      })
    },
    setCounter (context, obj) {
      console.log(obj)
      context.commit(MAIN_SET_COUNTER, obj)
    }
  },
  mutations: {
    logout (state) {
      if (typeof window !== 'undefined') {
        window.localStorage.setItem('token', null)
        window.localStorage.setItem('tokenExpiration', null)
      }
      state.isAuthenticated = false
    },
    login (state, token, profile) {
      if (typeof window !== 'undefined') {
        window.localStorage.setItem('token', token.token)
        window.localStorage.setItem('tokenExpiration', token.expiration)
      }
      state.isAuthenticated = true
      state.profile = profile
    },
    [MAIN_SET_COUNTER] (state, obj) {
      state.counter = obj
    }
  }
})

if (typeof window !== 'undefined') {
  document.addEventListener('DOMContentLoaded', function (event) {
    let expiration = window.localStorage.getItem('tokenExpiration')
    var unixTimestamp = new Date().getTime() / 1000
    if (expiration !== null && parseInt(expiration) - unixTimestamp > 0) {
      store.state.isAuthenticated = true
    }
  })
}
export default store
