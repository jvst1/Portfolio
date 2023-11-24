<template>
  <v-file-input ref="campo" v-bind="$attrs" v-model="model" :placeholder="placeholderDefault" :outlined="outlinedDefault"
    :dense="denseDefault" small-chips prepend-inner-icon="mdi-paperclip" prepend-icon="" :rules="rules"
    :label="labelDefault" v-on="$listeners">
    <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>
  </v-file-input>
</template>
<script>
export default {
  props: {
    value: null,
    label: { Type: String, default: "" },
    change: Function,
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

      if (this.$attrs["max-size"] && this.value) {
        if (this.value.size) {
          try {
            const max = parseInt(
              this.$attrs["max-size"].replace(/[aA-zZ]/g, "")
            );
            const unit = this.$attrs["max-size"].replace(/\d/g, "");
            let val = this.value.size;
            if (unit.toUpperCase() === "KB") {
              val = val / 1024;
            }
            if (unit.toUpperCase() === "MB") {
              val = val / (1024 * 1024);
            }
            if (unit.toUpperCase() === "GB") {
              val = val / (1024 * 1024 * 1024);
            }
            if (val > max) {
              errors.push(
                this.label +
                ": Tamanho máximo do arquivo é de " +
                this.$attrs["max-size"]
              );
            }
          } catch {
            // continue regardless of error
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
  mounted() {
    this.$nextTick(function () {
      const errors = this.rules;
      this.$el.setAttribute("data-message", errors ? errors[0] : "");
      this.$el.setAttribute("data-invalid", errors.length > 0);
    });
  },
};
</script>
