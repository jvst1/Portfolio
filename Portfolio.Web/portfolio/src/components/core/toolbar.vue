<template>
  <v-app-bar color="custom-toolbar" app clipped-left>
    <slot name="toolbar-avatar">
      <!-- <v-avatar size="85" class="ml-3" tile><v-img src="@/assets/logo.png" width="100%" contain /></v-avatar> -->
    </slot>

    <slot name="toolbar-title">
      <v-toolbar-title color="default" class="pl-3 pr-2">Portfolio</v-toolbar-title>
    </slot>

    <slot name="toolbar-drawer">
      <v-app-bar-nav-icon v-show="true" @click.stop="$store.commit('drawer')"></v-app-bar-nav-icon>
    </slot>

    <slot name="toolbar-before-space"> </slot>

    <v-spacer></v-spacer>

    <slot name="toolbar-after-spacer"> </slot>

    <slot name="toolbar-timer">
      <core-timer></core-timer>
    </slot>

    <slot name="toolbar-options">
      <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn text tile class="text-subtitle-2" color="default" v-bind="attrs" v-on="on">
            {{ userIdentificador }}
            <v-icon class="pl-2">mdi-chevron-down</v-icon>
          </v-btn>
        </template>

        <v-list dense>
          <v-list-item link>
            <v-list-item-title>
              <v-icon small class="pr-2" color="default">mdi-account</v-icon>
              Perfil
            </v-list-item-title>
          </v-list-item>
          <v-list-item link @click="logout">
            <v-list-item-title>
              <v-icon small class="pr-2" color="red">mdi-exit-to-app</v-icon>
              Sair
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </slot>
  </v-app-bar>
</template>

<script>

export default {
  data() {
    return {
      showAppBar: true,
      userIdentificador: "",
    };
  },
  methods: {
    logout() {
      this.$store.dispatch('logout')
    }
  },
  mounted() {
    const th = this;
    const user = th.$store.getters.user;

    this.userIdentificador = user.identificador;
    this.showAppBar = !user.roles.every(role => role === 'Cliente' || role === 'Nenhum') && user.roles.length > 0;
  },
};
</script>
