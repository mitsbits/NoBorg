<template>
<div class="content">
  <div v-if="isAuthenticated">
    Hey
    <p>Name: {{profile.firstName}}</p>
    <p>Sandwich: {{profile.favoriteSandwich}}</p>
    <button v-on:click="logout()" class="button is-primary">
        Log out
    </button>
  </div>
  <div v-else>
	<h2>Login</h2>
	<div class="field is-horizontal">
		<div class="field-label is-normal">
		  <label class="label">Username</label>
		</div>
		<div class="field-body">
		  <div class="field">
			<div class="control">
			  <input class="input" type="text"
			  placeholder="Your username" v-model="username">
			</div>
		  </div>
		</div>
	</div>
	<div class="field is-horizontal">
		<div class="field-label is-normal">
		  <label class="label">Password</label>
		</div>
		<div class="field-body">
		  <div class="field">
			<div class="control">
			  <input class="input" type="password"
			  placeholder="Your password" v-model="password">
			</div>
		  </div>
		</div>
	</div>
	<div class="field is-horizontal">
		<div class="field-label">
		  <!-- Left empty for spacing -->
		</div>
		<div class="field-body">
		  <div class="field">
			<div class="control">
			  <button class="button is-primary" v-on:click="login()">
				Login
			  </button>
			</div>
		  </div>
		</div>
	</div>
  </div>
</div>
</template>
<script>
import appService from '../app.service.js'
export default {
  data () {
    return {
      username: '',
      password: '',
      isAuthenticated: false,
      profile: {}
    }
  },
  watch: {
    isAuthenticated: function (val) {
      if (val) {
        appService.getProfile()
          .then(profile => {
            this.profile = profile
          })
      } else {
        this.password = {}
      }
    }
  },
  methods: {
    login () {
      appService.login({username: this.username, password: this.password})
        .then((data) => {
          window.localStorage.setItem('token', data.token)
          window.localStorage.setItem('tokenExpiration', data.expiration)
          this.isAuthenticated = true
          this.username = ''
          this.password = ''
        })
        .catch(() => window.alert('could not log in'))
    },
    logout () {
      window.localStorage.setItem('token', null)
      window.localStorage.setItem('tokenExpiration', null)
      this.isAuthenticated = false
    }
  },
  created () {
    let expiration = window.localStorage.getItem('tokenExpiration')
    var unixTimestamp = new Date().getTime() / 1000
    if (expiration !== null && parseInt(expiration) - unixTimestamp > 0) {
      this.isAuthenticated = true
    }
  }
}
</script>
