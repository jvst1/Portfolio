<template>
  <v-textarea ref="campo" v-model="model" v-bind="$attrs" :label="labelDefault" :placeholder="placeholderDefault"
    :outlined="outlinedDefault" :dense="denseDefault" :rules="rules" :rows="rows" @change="changeFunction">
    <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>
  </v-textarea>
</template>
<script>
export default {
  props: {
    value: String,
    label: { Type: String, default: "" },
    change: Function,
    rows: { Type: String, default: "2" },
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
      if (!this.$attrs.placeholder && this.$attrs.placeholder !== "") {
        return this.$empresa.placeholder;
      }
      return this.$attrs.placeholder;
    },
    outlinedDefault: function () {
      if (!this.$attrs.outlined && this.$attrs.outlined !== "") {
        return this.$empresa.outlined;
      }
      return this.$attrs.outlined;
    },
    denseDefault: function () {
      if (!this.$attrs.dense && this.$attrs.dense !== "") {
        return this.$empresa.dense;
      }
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
        !!this.model || errors.push(this.label + " é obrigatório");
      }

      if (this.$attrs.counter) {
        (this.model ? this.model : "").toString().length <=
          this.$attrs.counter ||
          errors.push(
            this.label + " deve ter até " + this.$attrs.counter + " caracteres"
          );
      }

      if (this.model) {
        if (this.$attrs.type === "email") {
          // eslint-disable-next-line
          /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "phone") {
          // eslint-disable-next-line
          /(?:\()[0-9]{2}(?:\))\s?[0-9]{4,5}(?:-)[0-9]{4}$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "tel") {
          // eslint-disable-next-line
          /(?:\()[0-9]{2}(?:\))\s?[0-9]{4}(?:-)[0-9]{4}$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "cel") {
          // eslint-disable-next-line
          /(?:\()[0-9]{2}(?:\))\s?[0-9]{5}(?:-)[0-9]{4}$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "doc") {
          // eslint-disable-next-line
          /(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)|(^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$)/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "cpf") {
          // eslint-disable-next-line
          /^\d{3}\.\d{3}\.\d{3}\-\d{2}$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "cnpj") {
          // eslint-disable-next-line
          /^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/i.test(this.model) || errors.push(this.label + ' inválido')
        }
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
  methods: {
    changeFunction(val) {
      if (val && !this.$refs.campo.disabled) {
        if (this.change) {
          this.change(val);
        }
      }
    },
  },
  mounted() {
    this.$nextTick(function () {
      const errors = this.rules;
      this.$el.setAttribute("data-message", errors ? errors[0] : "");
      this.$el.setAttribute("data-invalid", errors.length > 0);
    });
  },
};
</script>
