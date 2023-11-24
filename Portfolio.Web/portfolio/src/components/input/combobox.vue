<template>
  <v-combobox ref="campo" v-bind="$attrs" :placeholder="placeholderDefault" :outlined="outlinedDefault"
    :dense="denseDefault" :label="labelDefault" v-model="model" :items="options" :rules="rules" v-on="$listeners">
    <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>

    <template v-slot:item="p">
      <slot name="item" v-bind="p" />
    </template>
  </v-combobox>
</template>
<script>
export default {
  props: {
    options: { type: Array },
    value: null,
    label: { Type: String, default: "" },
  },
  methods: {
    open(timeout) {
      setTimeout(() => {
        this.$refs.campo.focus();
        this.$refs.campo.activateMenu();
      }, timeout || 0);
    },
  },
  computed: {
    labelDefault: function () {
      return (
        this.label +
        (this.$attrs.required === "" || this.$attrs.required === true
          ? " *"
          : "")
      );
    },
    placeholderDefault: function () {
      return this.$attrs.placeholder;
    },
    outlinedDefault: function () {
      return this.$attrs.outlined;
    },
    denseDefault: function () {
      return this.$attrs.dense;
    },
    model: {
      get() {
        return this.value;
      },
      set(val) {
        this.$emit("input", val);
      },
    },
    rules() {
      const errors = [];
      if (this.$attrs.required === "" || this.$attrs.required === true) {
        !!this.model || errors.push(this.label + " é obrigatório");
      }

      if (this.$refs && this.$refs.campo && this.$refs.campo.$el) {
        this.$refs.campo.$el.setAttribute(
          "data-message",
          errors ? errors[0] : ""
        );
        this.$refs.campo.$el.setAttribute("data-invalid", errors.length > 0);
      }

      return errors;
    },
  },
  mounted() {
    this.$nextTick(function () {
      // Selecionar a primeira se houver somente 1 opção e for required
      if (
        !this.value &&
        this.options &&
        this.options.length === 1 &&
        (this.$attrs.required === "" || this.$attrs.required === true)
      ) {
        this.$emit("input", this.options[0]);
      }
      const errors = this.rules;
      this.$el.setAttribute("data-message", errors ? errors[0] : "");
      this.$el.setAttribute("data-invalid", errors.length > 0);
    });
  },
};
</script>
