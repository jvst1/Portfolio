<template>
  <v-navigation-drawer app dark v-bind="$vuetify.theme.options.drawer" v-model="drawer" clipped floating
    class="elevation-2" :mini-variant.sync="mini" overflow>
    <v-list id="menulist" :dense="$vuetify.breakpoint[$vuetify.theme.options.compact.drawer]">
      <core-menulist :items="items"></core-menulist>
    </v-list>
  </v-navigation-drawer>
</template>
<script>
export default {
  computed: {
    mini: {
      get() {
        return !this.$vuetify.breakpoint.smAndDown && !this.drawer;
      },
      set() { },
    },
    items() {
      var item = this.filterAvaliables(this.$router.options.routes).map(
        function (p) {
          return p;
        }
      );
      return item;
    },
    drawer: {
      get() {
        return this.$store.getters.drawer;
      },
      set() { },
    },
    isMobile: {
      get() {
        return this.$vuetify.breakpoint.smAndDown;
      },
      set() { },
    },
  },
  methods: {
    filterAvaliables(routes) {
      const th = this;
      routes = routes.filter((p) => p.menu);
      routes.forEach(function (p) {
        if (p.children) {
          p.children = th.filterAvaliables(p.children);
        }
      });
      return routes;
    },
  },
};
</script>
