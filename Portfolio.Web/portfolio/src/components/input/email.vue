<template>
  <input-text v-bind="$attrs" ref="campo" v-model="model" :label="label" type="email">
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
    value: String,
    label: String,
    change: Function,
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
  },
};
</script>
