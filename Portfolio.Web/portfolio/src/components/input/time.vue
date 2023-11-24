<template>
  <input-text v-bind="$attrs" v-model="model" maxlength="10" :label="label" v-mask="mask" type="bmptime">
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
    mask: { default: "##:##" },
    label: String,
    "value-default": { default: "" },
  },
  data() {
    return {
      uiMask: "00:00",
    };
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
  mounted() {
    var vm = this;
    this.$nextTick(function () {
      if (vm.$el.querySelector("input")) {
        // var input = vm.$el.querySelector('input')
        let test = vm["value-default"];
        if (!test && vm.$attrs.default) {
          test = vm.$attrs.default;
        }

        switch (test) {
          case "now":
            var today = new Date();
            vm.model = today.getHours() + ":" + today.getMinutes();
            break;
          default:
            break;
        }

        switch (vm.mask) {
          case "##:##":
            this.uiMask = "00:00";
            break;
          case "##:## ##":
            this.uiMask = "00:00:00";
            break;
          default:
            break;
        }
      }
    });
  },
};
</script>
