<template>
  <v-navigation-drawer v-model="show" absolute bottom temporary :width="width">
    <v-list nav dense>
      <v-list-item-group active-class="primary--text" :value="model">
        <template v-for="(p, i) in itens">
          <v-list-group no-action :key="i" :value="(model || model === 0) && (model / 100) >> 0 === i"
            v-if="p.itens && p.itens.length">
            <template v-slot:activator>
              <v-list-item-icon v-if="!!p.icon">
                <v-icon size="18">{{ p.icon }}</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title class="pl-2 body-2" v-text="p.name"></v-list-item-title>
              </v-list-item-content>
            </template>

            <v-list-item :value="o.value" @click="click($event, o)" v-for="(o, j) in p.itens" :key="j">
              <v-list-item-title class="pl-2 body-2">
                {{ o.name }}
              </v-list-item-title>
            </v-list-item>
          </v-list-group>

          <v-list-item :value="p.value" @click="click($event, p)" :key="i" v-else>
            <v-list-item-icon v-if="!!p.icon">
              <v-icon size="18">{{ p.icon }}</v-icon>
            </v-list-item-icon>
            <v-list-item-title class="pl-2 body-2">
              {{ p.name }}
            </v-list-item-title>
          </v-list-item>
        </template>
      </v-list-item-group>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
export default {
  props: {
    value: null,
    itens: { type: Array },
    width: { type: String, default: "400px" },
  },
  data() {
    return {
      show: false,
      group: null,
    };
  },
  computed: {
    model: {
      get() {
        return this.value;
      },
      set(value) {
        this.$emit("input", value);
      },
    },
  },
  methods: {
    toggle() {
      this.show = !this.show;
    },
    click(e, item) {
      e.preventDefault();
      this.model = item.value;
      if (item.click) {
        item.click();
      }
      this.$emit("select", item);
      this.toggle();
    },
  },
};
</script>
