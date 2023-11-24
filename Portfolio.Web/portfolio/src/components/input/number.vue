<template>
  <input-text ref="campo" autocomplete="off" v-bind="$attrs" v-model="inputVal" :label="label" @change="changeFunction"
    type="numeric" :rawValue="autoNumeric.rawValue">
    <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>
  </input-text>
</template>
<script>
import AutoNumeric from "autonumeric";
export default {
  props: {
    value: [String, Number],
    label: String,
    change: Function,
    changeRaw: Function,
  },
  data() {
    return {
      autoNumeric: {},
    };
  },
  computed: {
    inputVal: {
      get() {
        if (
          this.autoNumeric.rawValue &&
          parseFloat(this.autoNumeric.rawValue) === this.value
        ) {
          return this.$api.UI.Format.Decimal(this.value);
        }
        return this.value;
      },
      set(value) {
        this.$emit("input", value);
      },
    },
  },
  methods: {
    changeFunction() {
      if (this.inputVal && !this.$refs.campo.disabled) {
        if (this.change) {
          this.change();
        }
        if (this.changeRaw) {
          this.changeRaw(this.autoNumeric.rawValue);
        }
      }
    },
  },
  mounted() {
    var vm = this;
    this.$nextTick(function () {
      var options = {
        alwaysAllowDecimalCharacter: true,
        decimalCharacter: AutoNumeric.options.decimalCharacter.comma,
        digitGroupSeparator: AutoNumeric.options.digitGroupSeparator.dot,
        maximumValue: vm.$attrs.maximumValue
          ? vm.$attrs.maximumValue
          : "9999999999",
        minimumValue: vm.$attrs.minimumValue
          ? vm.$attrs.minimumValue
          : AutoNumeric.options.minimumValue.zero,
        outputFormat: AutoNumeric.options.outputFormat.string,
        decimalPlaces: vm.$attrs.decimalPlaces
          ? vm.$attrs.decimalPlaces
          : AutoNumeric.options.decimalPlaces.two,
      };

      var input = vm.$el.querySelector("input");
      if (input) {
        switch (input.getAttribute("n-type")) {
          case "currency":
            options.currencySymbol =
              /* AutoNumeric.options.currencySymbol.real + */ " ";
            options.emptyInputBehavior =
              AutoNumeric.options.emptyInputBehavior.always;
            break;
          case "decimal":
            options.emptyInputBehavior =
              AutoNumeric.options.emptyInputBehavior.always;
            break;
          case "percent":
            options.maximumValue = 100;
            options.currencySymbol = " %";
            options.currencySymbolPlacement =
              AutoNumeric.options.currencySymbolPlacement.suffix;
            options.emptyInputBehavior =
              AutoNumeric.options.emptyInputBehavior.always;
            break;
          default:
            options.decimalPlaces = AutoNumeric.options.decimalPlaces.none;
            break;
        }
        this.autoNumeric = new AutoNumeric(input, options);
        return this.autoNumeric;
      }
    });
  },
};
</script>
