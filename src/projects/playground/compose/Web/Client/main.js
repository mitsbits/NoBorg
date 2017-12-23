require('./css/site.css');
require('bootstrap/dist/css/bootstrap.css');
require('jquery');
require('bootstrap');
//require('vue');
import Vue from 'vue';
window.Vue = Vue;
new Vue({
    el: '#app',
    render: function (h) { return h(require('./App.vue')); }
});
////import App from './App.vue';
////new Vue({
////  el: '#app',
////  components: { App }
////})
//let v = new Vue({
//  el: "#app",
//  template: `
//  <div id="app">
//    <img src="./assets/logo.png">
//    <h1>{{ msg }}</h1>
//    <h2>Essential Links</h2>
//    <ul>
//      <li><a href="https://vuejs.org" target="_blank">Core Docs</a></li>
//      <li><a href="https://forum.vuejs.org" target="_blank">Forum</a></li>
//      <li><a href="https://chat.vuejs.org" target="_blank">Community Chat</a></li>
//      <li><a href="https://twitter.com/vuejs" target="_blank">Twitter</a></li>
//    </ul>
//    <h2>Ecosystem</h2>
//    <ul>
//      <li><a href="http://router.vuejs.org/" target="_blank">vue-router</a></li>
//      <li><a href="http://vuex.vuejs.org/" target="_blank">vuex</a></li>
//      <li><a href="http://vue-loader.vuejs.org/" target="_blank">vue-loader</a></li>
//      <li><a href="https://github.com/vuejs/awesome-vue" target="_blank">awesome-vue</a></li>
//    </ul>
//  </div>
//    `,
//  data() {
//    return {
//      msg: 'Welcome to Your Vue.js App'
//    }
//  }
//});
//# sourceMappingURL=main.js.map