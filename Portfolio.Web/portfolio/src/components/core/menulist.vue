<style>
#menulist .v-list-item__icon {
  margin-right: 2px;
}
</style>
<template>
  <div>
    <template v-for="item in items">
      <router-link
        :key="item.path + item.title"
        v-if="$router.userHasPermission(item)"
        tag="div"
        :to="!item.children ? item.path : ''"
        :replace="!item.children ? replace : ''"
        :append="!item.children ? append : ''"
        :exact="!item.children ? true : ''"
        :event="!item.children ? event : ''"
      >
        <v-list-group
          color="menu"
          no-action
          :sub-group="subgroup"
          v-if="item.children && item.children.length"
          :value="isMenuOpen(item.path)"
        >
          <template v-slot:activator>
            <v-list-item-icon v-if="!subgroup">
              <v-icon size="18">{{ item.icon }}</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title
                class="pl-2 body-2"
                v-text="item.title"
              ></v-list-item-title>
            </v-list-item-content>
            <v-list-item-icon v-if="subgroup">
              <v-icon size="18">{{ item.icon }}</v-icon>
            </v-list-item-icon>
          </template>
          <core-menulist
            :subgroup="true"
            :items="item.children"
          ></core-menulist>
        </v-list-group>

        <v-list-item router :to="item.path" color="menu" v-else>
          <v-list-item-icon v-if="item.icon">
            <v-icon size="18">{{ item.icon }}</v-icon>
          </v-list-item-icon>
          <v-list-item-title class="pl-2 body-2">{{
            item.title
          }}</v-list-item-title>
        </v-list-item>
      </router-link>
    </template>
  </div>
</template>
<script>
export default {
  props: {
    items: Array,
    linkClass: {
      type: String,
      default: "",
    },
    badge: {
      default: null,
    },
    badgeClass: {
      type: String,
      default: "",
    },
    disabled: {
      type: Boolean,
      default: false,
    },

    // router props
    subgroup: {
      type: Boolean,
      default: false,
    },
    replace: {
      type: Boolean,
      default: false,
    },
    append: {
      type: Boolean,
      default: false,
    },
    event: null,
    orientation: {
      type: String,
      default: "vertical",
    },
  },
  data() {
    return {
      test: 1,
    };
  },
  methods: {
    isMenuActive(url) {
      return this.$route.path.indexOf(url) === 0;
    },
    isMenuOpen(url) {
      return (
        this.$route.path.indexOf(url) === 0 && this.orientation !== "horizontal"
      );
    },
  },
};
</script>
