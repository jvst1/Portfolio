<template>
  <v-list v-bind="$attrs">
    <v-list-item-group multiple v-model="model" ref="group">
      <template v-for="(item, i) in Items">
        <v-divider v-if="!item" :key="`divider-${i}`"></v-divider>
        <v-list-item v-else :key="`item-${i}`" :value="item.value" active-class="primary--text text--accent-4">
          <template v-slot:default="{ active }">
            <v-list-item-action>
              <v-checkbox :input-value="active" color="primary accent-4"></v-checkbox>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title v-text="item.text"></v-list-item-title>
            </v-list-item-content>
          </template>
        </v-list-item>
      </template>
    </v-list-item-group>
  </v-list>
</template>
<script>
export default {
  name: "multiselect-list",
  props: {
    Items: { type: Array, required: true },
    value: null,
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
