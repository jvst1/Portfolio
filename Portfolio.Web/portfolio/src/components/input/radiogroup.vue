<template>
  <div>
    <v-radio-group ref="campo" v-bind="$attrs" v-model="model" :rules="rules" v-on="$listeners">
      <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
        <slot :name="name" v-bind="slotData" />
      </template>
      <template v-for="(_, name) in $slots" v-slot:[name]>
        <slot :name="name" />
      </template>
    </v-radio-group>
  </div>
</template>
<script>
export default {
  props: {
    value: [Number, String],
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
    rules() {
      let name = this.$attrs.name;
      if (!name) {
        name = this.$attrs.label;
      }

      const errors = [];
      if (this.$attrs.required === "" || this.$attrs.required === true) {
        !!this.model || errors.push(name + " é obrigatório");
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
};
</script>
