<template>
  <input-text v-bind="$attrs" ref="campo" v-model="model" :label="label" v-mask="mask" maxlength="20" :type="type"
    :documento-invalido="invalido" @change="changeDefault" v-on="$listeners">
    <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>
  </input-text>
</template>

<script>
export default {
  props: {
    value: null,
    label: { type: String, default: "" },
    cpf: { type: Boolean, default: false },
    cnpj: { type: Boolean, default: false },
    validate: { type: Boolean, default: true },
  },
  data() {
    return {
      invalido: false,
    };
  },
  computed: {
    mask: function () {
      const masks = [];
      if (this) {
        if (!this.cpf || this.cnpj) {
          masks.push("##.###.###/####-##");
        }
        if (!this.cnpj || this.cpf) {
          masks.push("###.###.###-##");
        }
      }
      return masks;
    },
    type: function () {
      if (this) {
        if (this.cpf === this.cnpj) {
          return "doc";
        } else if (this.cpf || !this.cnpj) {
          return "cpf";
        } else if (this.cnpj || !this.cpf) {
          return "cnpj";
        }
      }
      return "doc";
    },
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
    changeDefault: function () {
      const th = this;
      if (th.validate) {
        th.$api.UTIL.Functions.ValidaDocumento(
          this.$api.UI.Format.SomenteNumeros(this.value)
        ).then(function (isValid) {
          th.invalido = !isValid;
        });
      }
      th.$emit("change", this.value);
    },
  },
};
</script>
