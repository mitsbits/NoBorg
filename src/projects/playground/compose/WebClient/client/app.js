import Vue from 'vue'
import store from './vuex/index.js'
import AppLayout from './theme/Layout.vue'
import router from './router'
var signalR = require('@aspnet/signalr-client')

Vue.prototype.$signalR = signalR

const app = new Vue({
  router,
  ...AppLayout,
  store
})

export {app, router, store}
