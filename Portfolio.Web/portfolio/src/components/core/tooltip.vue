<template>
  <v-tooltip top max-width="300">
    <template v-slot:activator="{ on }">
      <v-icon :color="color" :class="iclass" v-on="on" small>{{ icon }}</v-icon>
    </template>
    <span>{{ text }}</span>
  </v-tooltip>
</template>

<script>
export default {
  props: {
    iclass: { type: String, default: "mt-1" },
    color: { type: String, default: "primary" },
    icon: { type: String, default: "fas fa-question-circle" },
    text: { type: String, default: "" },
  },
};
</script>
