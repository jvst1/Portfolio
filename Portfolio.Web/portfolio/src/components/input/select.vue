<template>
  <v-select ref="campo" v-bind="$attrs" :placeholder="placeholderDefault" :outlined="outlinedDefault"
    :dense="denseDefault" :label="labelDefault" v-model="model" :item-value="trackBy" :item-text="text" :items="options"
    :hint="hint" :rules="rules" persistent-hint clearable v-on="$listeners">
    <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>
  </v-select>
</template>
<script>
export default {
  props: {
    options: { type: Array },
    value: null,
    hint: { Type: String, default: "" },
    text: { default: "text" },
    trackBy: { default: "value" },
    label: { Type: String, default: "" },
    multiple: { default: "" },
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
      set(value) {
        this.$emit("input", value);
      },
    },
    rules() {
      const errors = [];
      if (this.$attrs.required === "" || this.$attrs.required === true) {
        // 0 é um valor válido
        this.model === 0 ||
          !!this.model ||
          errors.push(this.label + " é obrigatório");
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
  beforeMount() {
    if (!this.$attrs["item-value"]) {
      this.$attrs["item-value"] = this.trackBy;
    }
    if (this.$attrs["item-text"]) {
      this.$attrs["item-text"] = this.text;
    }
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
        this.$emit("input", this.options[0][this.trackBy]);
      }
      const errors = this.rules;
      this.$el.setAttribute("data-message", errors ? errors[0] : "");
      this.$el.setAttribute("data-invalid", errors.length > 0);
    });
  },
};
</script>
