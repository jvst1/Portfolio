<template>
  <v-layout justify-center>
    <v-dialog v-model="show" scrollable persistent :width="width" :max-heigth="heigth" @click.stop="close">
      <v-form enctype="multipart/form-data" ref="form" @submit.prevent="submitDefault" :lazy-validation="lazy">
        <v-card :key="controlKey">
          <v-card-title>
            <slot class="headline" name="title"></slot>
            <div class="flex-grow-1"></div>
            <v-btn icon @click="close">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-card-title>
          <v-divider></v-divider>
          <v-card-text :class="bodyClass">
            <slot name="body"></slot>
          </v-card-text>
          <v-divider></v-divider>
          <v-card-actions>
            <slot name="actions.prepend"></slot>
            <slot name="actions">
              <!--<span class="caption" color="default">{{$parent.$options._componentTag}}</span>-->
              <div class="flex-grow-1"></div>
              <v-btn color="white" @click="close">{{ textClose }}</v-btn>
              <v-btn v-if="showSubmit && saveContinue" :disabled="disableSubmit" v-on:click="submitVal = 'save'"
                color="primary" type="submit">{{ textSubmitContinue }}</v-btn>
              <v-btn v-if="showSubmit" :disabled="disableSubmit" v-on:click="submitVal = 'saveClose'" color="primary"
                type="submit">{{ textSubmitDefault }}</v-btn>
            </slot>
            <slot name="actions.append"></slot>
          </v-card-actions>
        </v-card>
      </v-form>
    </v-dialog>
  </v-layout>
</template>

<script>
/* eslint-disable */
export default {
  props: {
    value: Boolean,
    size: { default: "sm" },
    submit: Function,
    bodyClass: { type: String, default: "" },

    model: { type: String, default: "model" },
    textSubmit: { type: String, default: "Salvar" },
    textSubmitContinue: { type: String, default: "Salvar" },
    textClose: { type: String, default: "Cancelar" },
    hideFooter: { type: Boolean, default: false },
    lazy: { type: Boolean, default: false },
    disableSubmit: { type: Boolean, default: false },
    showSubmit: { type: Boolean, default: true },
    saveContinue: { type: Boolean, default: false },
  },
  data() {
    return {
      defaultModel: {},
      cleared: true,
      submitVal: null,
      controlKey: 0,
    };
  },
  methods: {
    close: function () {
      this.$parent.show = false;
      this.$parent.$emit("close");
    },
    submitDefault: function (e) {
      e.preventDefault();

      if (this.$api.UI.FormValidate(this)) {
        if (this.submit) {
          this.submit(this.reload);
        }
      }
    },
    reload: function () {
      const th = this;
      th.show = false;
      if (th.saveContinue && th.submitVal === "save") {
        th.$nextTick(function () {
          th.show = true;
        });
      } else {
        th.close();
      }
    },
  },
  computed: {
    textSubmitDefault: function () {
      if (this.saveContinue && this.textSubmit === "Salvar") {
        return "Salvar e Fechar";
      }
      return this.textSubmit;
    },
    show: {
      get() {
        const th = this;
        if (!th.value && th.$refs && th.$refs.form && th.showSubmit) {
          if (
            th.model &&
            th.$parent &&
            th.$parent[th.model] &&
            th.defaultModel
          ) {
            th.$nextTick(function () {
              if (!th.cleared) {
                var data = th.$api.UTIL.Functions.CopyObject(th.defaultModel);
                Object.keys(data).forEach((k) => {
                  th.$parent[th.model][k] = data[k];
                });
                th.$parent[th.model] = Object.assign({}, data);
                th.$set(th.$parent, th.model, data);
                th.$refs.form.reset();
                th.cleared = true;
              }
            });
          }
        } else if (this.value) {
          th.cleared = false;
        }

        return this.value;
      },
      set(value) {
        if (value === false) {
          this.controlKey++;
        }
        this.$parent.$emit("input", value);
      },
    },
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
            return "97%";
          default:
            return "500px";
        }
      },
      set() { },
    },
    heigth: {
      get() {
        return "90%"
      },
      set() { },
    },
  },
  mounted: function () {
    const th = this;
    th.$nextTick(function () {
      if (th.model && th.$parent && th.$parent[th.model]) {
        th.$nextTick(function () {
          var unWatch = th.$watch(
            "$store.getters.activeRequests",
            function (newValue, oldValue) {
              if (newValue === 0) {
                unWatch();
                th.$nextTick(function () {
                  th.defaultModel = th.$api.UTIL.Functions.CopyObject(
                    th.$parent[th.model]
                  );
                });
              }
            },
            { deep: true }
          );
        });
      }
    });
  },
};
</script>
