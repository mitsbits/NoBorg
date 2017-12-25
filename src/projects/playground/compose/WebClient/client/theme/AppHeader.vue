<<template>
      <nav class="nav has-shadow">
      <div class="container">
      <router-link to="/category/front-end" exact>
        <img src="http://bit.ly/vue-img"
          alt="Vue SPA" />
      </router-link>

      <router-link class="nav-item is-tab" to="/category/front-end">Front end</router-link>
      <router-link class="nav-item is-tab"  :to="{ name: 'category', params: { id: 'mobile' } }">Mobile</router-link>
      <router-link class="nav-item is-tab" to="/topic">Topic</router-link>
      <router-link class="nav-item is-tab" to="/login">
        <span v-if="isAuthenticated">Log out</span>
        <span v-else>Log in</span>
    </router-link>
    <a class="nav-item is-tab" href="#">{{counter}}</a>
    </div>
    </nav>
</template>
<script>
import { mapGetters, mapActions } from 'vuex'
export default{
  data () {
    return {
      connection: null
    }
  },
  computed: {
    ...mapGetters(['isAuthenticated', 'counter'])
  },
  methods: {
    ...mapActions({
      setCounter: 'setCounter'
    })
  },
  created: function () {
    this.connection = new this.$signalR.HubConnection('/count')
  },
  mounted: function () {
    this.connection.start()

    this.connection.on('increment', data => {
      this.setCounter(data)
    })
  }
}
</script>
