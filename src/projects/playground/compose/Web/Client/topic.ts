require('./css/site.css');
require('bootstrap/dist/css/bootstrap.css');
require('jquery');
require('bootstrap');
//require('vue');
if (module['hot']) {
  module['hot'].accept();
}
import Vue from 'vue';
(<any>window).VueX = Vue;

import App from './file.vue'

new Vue({
  el: '#app',
  render: h => h(App)
});
