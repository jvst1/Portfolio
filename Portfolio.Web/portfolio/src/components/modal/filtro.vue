<template>
  <v-layout justify-center>
    <v-dialog v-model="show" scrollable persistent :max-width="width" :max-heigth="heigth" @click.stop="close">
      <v-form enctype="multipart/form-data" ref="form" @submit.prevent="submitDefault" :lazy-validation="lazy">
        <v-card>
          <v-card-title>
            <i small class="fas fa-filter mr-2"></i>Filtro
            <div class="flex-grow-1"></div>
            <v-btn icon @click="close">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-card-title>
          <v-divider></v-divider>
          <v-card-text>
            <slot></slot>
          </v-card-text>
          <v-divider></v-divider>
          <v-card-actions>
            <slot name="actions.prepend"></slot>
            <slot name="actions">
              <div class="flex-grow-1"></div>
              <v-btn color="white" @click="close">Fechar</v-btn>
              <v-btn v-if="showSubmit" :disabled="disableSubmit" type="submit" color="primary">Filtrar</v-btn>
            </slot>
            <slot name="actions.append"></slot>
          </v-card-actions>
        </v-card>
      </v-form>
    </v-dialog>
  </v-layout>
</template>

<script>
export default {
  props: {
    model: { default: "" },
    size: { default: "sm" },
    hideFooter: { default: false },
    submit: Function,
    lazy: { type: Boolean, default: false },
    disableSubmit: { type: Boolean, default: false },
    showSubmit: { type: Boolean, default: true },
  },
  data() {
    return {
      show: false,
    };
  },
  methods: {
    openModal() {
      this.show = true;
    },
    close: function () {
      this.show = false;
    },
    validate() {
      var isValid = this.$api.UI.FormValidate(this);
      if (!isValid) {
        this.$api.UI.HideLoading();
      }
      return isValid;
    },
    submitDefault: function (e) {
      e.preventDefault();
      if (this.validate()) {
        if (this.submit) {
          this.submit();
        } else {
          this.close();
          this.$emit("refresh");
        }
      }
    },
  },
  computed: {
    width: {
      get() {
        switch (this.size) {
          case "sm":
            return "500px";
          case "md":
            return "700px";
          case "lg":
            return "900px";
          case "xl":
            return "90%";
          default:
            return "500px";
        }
      },
      set() { },
    },
    heigth: {
      get() {
        switch (this.size) {
          case "sm":
            return "90%";
          default:
            return "90%";
        }
      },
      set() { },
    },
  },
};
</script>
