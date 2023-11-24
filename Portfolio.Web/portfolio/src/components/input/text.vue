<template>
  <v-text-field ref="campo" v-model="model" v-bind="$attrs" :label="labelDefault" :placeholder="placeholderDefault"
    :outlined="outlinedDefault" :dense="denseDefault" :rules="rules" v-on="$listeners">
    <template v-for="(index, name) in $scopedSlots" v-slot:[name]="data">
      <slot :name="name" v-bind="data"></slot>
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>
    <template v-slot:message="{ message, key }">
      <span v-html="message" :key="key"></span>
    </template>
  </v-text-field>
</template>
<script>
export default {
  props: {
    value: [String, Number],
    label: { Type: String, default: "" },
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
        !!this.model || errors.push(this.label + " é obrigatório");
      }

      if (this.$attrs.counter) {
        (this.model ? this.model : "").toString().length <=
          this.$attrs.counter ||
          errors.push(
            this.label + " deve ter até " + this.$attrs.counter + " caracteres"
          );
      }

      if (this.$attrs.customRule === false) {
        errors.push(this.$attrs.customError);
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
        } else if (this.$attrs.type === "cep") {
          // eslint-disable-next-line
          /[0-9]{2}\.?[0-9]{3}\-?[0-9]{3}$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "doc") {
          // eslint-disable-next-line
          /(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)|(^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$)/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "cpf") {
          // eslint-disable-next-line
          /^\d{3}\.\d{3}\.\d{3}\-\d{2}$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "cnpj") {
          // eslint-disable-next-line
          /^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "bmptime") {
          // eslint-disable-next-line
          /^(?:\d|[01]\d|2[0-3]):[0-5]\d$/i.test(this.model) || errors.push(this.label + ' inválido')
        } else if (this.$attrs.type === "numeric" && this.$attrs.max) {
          try {
            const max = parseFloat(this.$attrs.max);
            const val = parseFloat(this.$attrs.rawValue);
            if (val > max) {
              errors.push(
                "Valor Máximo permitido é " + this.$api.UI.Format.Decimal(max)
              );
            }
          } catch {
            // ignora
          }
        }
        if (
          this.$attrs.type === "doc" ||
          this.$attrs.type === "cpf" ||
          this.$attrs.type === "cnpj"
        ) {
          if (this.$attrs["documento-invalido"] === true) {
            errors.push(this.label + " inválido");
          }
        }
        if (this.$attrs.type === "cep") {
          if (this.$attrs["cep-invalido"] === true) {
            errors.push(this.label + " inválido");
          }
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
  methods: {},
  mounted() {
    this.$nextTick(function () {
      const errors = this.rules;
      this.$el.setAttribute("data-message", errors ? errors[0] : "");
      this.$el.setAttribute("data-invalid", errors.length > 0);
    });
  },
};
</script>
