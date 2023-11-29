<template>
  <v-layout justify-center>
    <v-dialog v-model="show" scrollable persistent :max-width="width" :max-heigth="heigth" @click.stop="close">
      <v-card>
        <v-card-title>
          <i small class="mdi mdi-file-search-outline mr-2"></i>{{ title }}
          <div class="flex-grow-1"></div>
          <v-btn icon @click="close">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-divider></v-divider>
        <v-card-text class="mt-2">
          <v-row>
            <v-col cols="12" v-if="search">
              <input-text label="Pesquisa" v-model="pesquisa" hide-details></input-text>
            </v-col>
            <v-col cols="12">
              <v-treeview dense :items="itens" :search="pesquisa" :filter="filter" :open.sync="open"></v-treeview>
            </v-col>
          </v-row>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <slot name="actions">
            <div class="flex-grow-1"></div>
            <v-btn color="white" @click="close">Fechar</v-btn>
          </slot>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script>
export default {
  props: {
    title: { type: String, default: "Detalhes" },
    size: { default: "sm" },
    search: { type: Boolean, default: false },
    caseSensitive: { type: Boolean, default: false },
    openAll: { type: Boolean, default: false },
  },
  data() {
    return {
      show: false,
      model: null,
      itens: [],
      pesquisa: null,
      open: [],
      id: 0,
    };
  },
  methods: {
    openModal(sender) {
      this.show = true;
      if (sender) {
        this.model = sender;
        this.itens = [];
        this.id = 0;
        this.itens = this.getItens(this.model);
      }
    },
    getItens(value) {
      const th = this;
      const list = [];
      switch (typeof value) {
        case "object":
          Object.keys(value).forEach(function (p) {
            th.id++;
            const item = { name: p + ": ", id: th.id };
            if (th.hasChildren(value[p])) {
              item.children = th.getItens(value[p], item.id);
              if (th.openAll) {
                th.open.push(item.id);
              }
            } else {
              item.name += value[p];
            }

            list.push(item);
          });
          return list;
      }
    },
    hasChildren(item) {
      if (item && typeof item === "object") {
        return Object.keys(item).length > 0;
      }

      return false;
    },
    close: function () {
      this.model = null;
      this.show = false;
    },
  },
  computed: {
    filter() {
      return this.caseSensitive
        ? (item, search, textKey) => item[textKey].indexOf(search) > -1
        : undefined;
    },
    width: {
      get() {
        switch (this.size) {
          case "sm":
            return "500px";
          case "md":
            return "700px";
          case "lg":
            return "900px";
          case "xl":
            return "90%";
          default:
            return "500px";
        }
      },
      set() { },
    },
    heigth: {
      get() {
        return "90%"
      },
      set() { },
    },
  },
};
</script>
