<template>
  <div>
  <input :value="myProperty" />
  <h1>{{myProperty}}</h1>
  </div>
</template>
<script lang="ts">

import Vue from "vue";
import { Component, Prop } from 'vue-property-decorator'


export default {
  data(){
  return  { myProperty: 'string'};
  }
  ,


  onPropertyChanged(value: string, oldValue: string) {
    alert(value);
  }
}
</script>
