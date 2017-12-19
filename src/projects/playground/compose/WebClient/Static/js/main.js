//import '../css/site.css';
//import 'bootstrap/dist/css/bootstrap.css'
//import 'jquery';
//import 'bootstrap';

//import 'vue';
//import 'vue-resource';
////import '@aspnet/signalr-client'

require('../css/site.css');
require('bootstrap/dist/css/bootstrap.css');
require('jquery');
require('bootstrap');
require('vue');
require('vue-resource');

import Vue from 'vue';
import VueResource from 'vue-resource';
//var VueResource = require('vue-resource');
//Vue.use(VueResource);
window.Vue = Vue;
Vue.use(VueResource);
//require('@aspnet/signalr-client');

//import signalR  from '@aspnet/signalr-client';