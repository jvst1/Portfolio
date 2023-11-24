<template>
  <v-app id="sandbox">
    <core-toolbar></core-toolbar>
    <core-menu></core-menu>

    <v-main class="page-background">
      <v-container fluid>
        <v-breadcrumbs :items="breadcrumbs" class="ml-1 pl-2 pb-0 pt-4 pl-0">
          <template v-slot:item="{ item }">
            <v-breadcrumbs-item :href="item.href" :disabled="item.disabled" v-show="item.text != 'Home'">
              <v-icon v-if="item.icon" :color="item.disabled ? 'breadcrumb-disabled' : 'breadcrumb'" small left>{{
                item.icon }}</v-icon>&nbsp;
              <div :class="item.disabled
                ? 'breadcrumb-disabled--text'
                : 'breadcrumb--text'
                " class="text-h6 font-weight-bold">
                {{ item.text }}
              </div>
            </v-breadcrumbs-item>
          </template>
          <template v-slot:divider>
            <div class="pa-0">
              <v-icon small>mdi mdi-chevron-double-right</v-icon>
            </div>
          </template>
        </v-breadcrumbs>

        <v-row>
          <v-col cols="12">
            <router-view></router-view>
          </v-col>
        </v-row>
      </v-container>
    </v-main>

    <v-footer inset color="custom-footer" app>
      <span class="px-4">&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>
  </v-app>
</template>

<script>
/* eslint-disable */
export default {
  data() {
    return {};
  },
  computed: {
    breadcrumbs: function () {
      var route = this.$route;
      return route.meta.breadcrumbs.map(function (p, i) {
        if (i === route.meta.breadcrumbs.length - 1) {
          p.disabled = true;
        }

        return p;
      });
    },
  },
};
</script>
