import Vue from "vue";
import { Component } from 'vue-property-decorator';

@Component
export class App extends Vue {
  data() {
    return {
      msg: 'Welcome to Your Vue.js App'
    }
  }
}
