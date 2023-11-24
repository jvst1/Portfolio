<template>
  <input-text v-bind="$attrs" ref="campo" v-model="model" :label="label" v-mask="mask" :type="type" maxlength="20"
    v-on="$listeners">
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
    tel: { type: Boolean, default: false },
    cel: { type: Boolean, default: false },
  },
  computed: {
    mask: function () {
      const masks = [];
      if (this) {
        if (this.cel || !this.tel) {
          masks.push("(##) #####-####");
        }
        if (this.tel || !this.cel) {
          masks.push("(##) ####-####");
        }
      }
      return masks;
    },
    type: function () {
      if (this) {
        if (this.tel === this.cel) {
          return "phone";
        } else if (this.tel || !this.cel) {
          return "tel";
        } else if (this.cel || !this.tel) {
          return "cel";
        }
      }
      return "phone";
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
};
</script>
