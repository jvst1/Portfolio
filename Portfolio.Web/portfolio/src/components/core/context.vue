<template>
  <v-menu :top="topMenu" v-model="showMenu" :position-x="x" :position-y="y" absolute min-width="200"
    transition="scale-transition" offset-yv-slot:activator="showMenu">
    <v-list>
      <slot @click="click" v-bind:item="sender"></slot>
    </v-list>
  </v-menu>
</template>

<script>
export default {
  props: {
    value: Boolean,
    action: String,
    modal: Boolean,
  },
  data: () => ({
    sender: {},
    showMenu: false,
    topMenu: false,
    x: 0,
    y: 0,
  }),
  methods: {
    show(e, sender) {
      e.preventDefault();
      this.showMenu = false;
      this.topMenu = false;
      if (window.innerHeight < e.clientY + 200) {
        this.topMenu = true;
      }

      this.x = e.clientX;
      this.y = e.clientY;
      this.$nextTick(() => {
        this.showMenu = true;
        this.sender = sender;
      });
    },
  },
  watch: {
    value(val) {
      if (val) {
        this.show();
      }
    },
  },
};
</script>
