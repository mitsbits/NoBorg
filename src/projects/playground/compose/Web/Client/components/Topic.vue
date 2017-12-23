<template>
  <div>
    <p>Long-form v-model example</p>
    <input :value="name"/>
  </div>
</template>

<script lang="ts">
  import Vue from 'vue'

  export default {
    // Data property
    data() {
      return {
        name:'xxx',
        hash: 'ooo'
      }
    },


    // Lifecycle hook
    mounted() {
      this.name = 'Boop';
      alert(this.name);
    },

    // Component method
    updateMyProperty($event) {
      this.hash = $event.target.value
    }
  }
</script>
