<template>
  <modal-form v-model="show" :showSubmit="false" textClose="Fechar" :size="size">
    <template v-slot:title>
      {{ title }}
      <br />
      <span class="caption pt-1 ml-3">{{ caption }}</span>
    </template>
    <template v-slot:body>
      <core-grid mobile-breakpoint="0" modal flat ref="grid" dense :onRead="onReadDefault" :editOption="false"
        :deleteOption="false" :addOption="false" :schema="schema" :click="selecionar">
      </core-grid>
    </template>
  </modal-form>
</template>

<script>
export default {
  props: {
    title: { type: String, default: "Selecione" },
    caption: { type: String, default: "Clique na linha para selecionar" },
    itemText: { type: String, default: "nome" },
    onRead: { type: Function },
    items: { type: Array },
    schema: { type: Array },
    size: { default: "sm" },
  },
  data() {
    return {
      show: false,
    };
  },
  methods: {
    openModal() {
      this.show = true;
    },
    onReadDefault: function (options, search) {
      if (this.items && !this.onRead) {
        options.success(this.items);
      } else if (this.onRead) {
        this.onRead(options, search);
      } else {
        options.success([]);
      }
    },
    selecionar: function (curr) {
      const th = this;
      th.$api.UI.Confirmacao(
        "info",
        "Confirma a Seleção do Registro: " + curr[th.itemText] + "?",
        function () {
          th.$emit("close", curr);
          th.show = false;
        }
      );
    },
  },
};
</script>
