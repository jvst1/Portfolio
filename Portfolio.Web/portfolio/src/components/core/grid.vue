<style>
  .v-application--is-ltr .v-data-table--fixed-header .v-data-footer {
    margin-right: 0px !important;
  }
</style>
<template>
  <v-data-table  v-bind="$attrs"
                ref="grid"
                v-model="selected"
                :headers="headers"
                :items="dataSource"
                :search="serverSide ? search : ''"
                fixed-header
                :dense="isDense"
                :class="
      (!flat ? 'elevation-3' : '') +
        (!modal && !hasClass ? ' pl-3 pr-3 ' : '') +
        ' custom-grid-class'
    "
                :options.sync="options"
                :items-per-page="pageSizeDefault"
                @page-count="pageCount = $event"
                :server-items-length="serverSide ? serverItemsLength : -1"
                :show-select="selectable"
                :item-key="itemKey"
                :single-select="singleSelect"
                @click:row="clickDefault"
                :item-class="row_classes"
                :footer-props="footerProps"
                loading-text="Carregando Dados... Por favor Aguarde">
    <template v-for="field in formats()" v-slot:[field.slot]="{ item }">
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'text' || field.format === 'string'">{{ item[field.value] }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'cep'">{{ $api.UI.Format.Cep(item[field.value]) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'phone'">{{ $api.UI.Format.Telefone(item[field.value]) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'boolean'">{{ item[field.value] ? "Sim" : "Não" }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'documento'">{{ $api.UI.Format.DocumentoFederal(item[field.value]) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'datetime'">{{ $api.UI.Format.DateTime(item[field.value], field.dateFormat) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'date'">{{ $api.UI.Format.Date(item[field.value], field.dateFormat) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'time'">{{ $api.UI.Format.Time(item[field.value]) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'time-seconds'">{{ $api.UI.Format.Time(item[field.value], "00:00:00") }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'decimal'">{{ $api.UI.Format.Decimal(item[field.value]) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'currency'">{{ $api.UI.Format.Currency(item[field.value]) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'array'">{{ getValue(field, item) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'template' && field.template">{{ field.template(item) }}</span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'button'">
        <v-tooltip top>
          <template v-slot:activator="{ on }">
            <v-btn v-on="field.tooltip ? on : false"
                   :fab="field.icon && !field.title ? true : false"
                   small
                   :height="isDense? '20px' : null"
                   :width="isDense? '20px' : null"
                   text
                   :color="field.title ? field.color : ''"
                   @click="field.action($event, item)">
              <v-icon v-if="field.icon" small v-bind:class="field.text? 'mr-2' : ''">{{ field.icon }}</v-icon>
              <span v-if="field.title">{{ field.title }}</span>
            </v-btn>
          </template>
          <span v-if="field.tooltip">{{ field.tooltip }}</span>
        </v-tooltip>
      </span>
      <span :key="field.value"
            :class="field.class"
            v-show="field.condition(item)"
            v-if="field.format === 'optionButton'">
        <v-menu offset-y close-on-content-click>
          <template v-slot:activator="{ on, attrs }">
            <v-btn v-on="on"
                   small
                   :class="field.icon ? '' : 'ma-0'"
                   :fab="field.icon ? true : false"
                   :text="field.icon ? true : false"
                   :color="field.color"
                   v-bind="attrs">
              <v-icon v-if="field.icon" small>{{ field.icon }}</v-icon>
              {{field.title}}
            </v-btn>
          </template>
          <v-list dense>
            <v-list-item v-for="(fItem, index) in field.items"
                         :key="index"
                         @click="fItem.action($event, item)">
              <v-list-item-icon v-if="fItem.icon" class="mr-2">
                <v-icon v-if="fItem.icon" small>{{ fItem.icon }}</v-icon>
              </v-list-item-icon>
              <v-list-item-title v-text="fItem.title"></v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </span>
    </template>

    <template slot="body.append">
      <slot name="body.append" v-bind="{ showModal, item: current }"></slot>
    </template>

    <template slot="body.prepend">
      <slot name="body.prepend" v-bind="{ showModal, item: current }"></slot>
    </template>

    <template v-slot:top>
      <v-card class="d-flex flex-wrap align-end" width="100%" flat tile>
        <v-card width="100%" tile flat>
          <v-row>
            <v-col cols="12" md="8" lg="9">
              <core-grid-button v-if="addOption"
                                :small="modal? true: false"
                                color="primary"
                                @click="incluir($event)"
                                text="Novo"
                                icon="fas fa-plus">
              </core-grid-button>

              <slot name="toolbar" v-bind="{ showModal, item: current }"></slot>

              <core-grid-button v-if="filterOption"
                                :small="modal? true: false"
                                color="grid-btn"
                                @click="filtrar"
                                text="Filtro"
                                icon="fas fa-filter">
              </core-grid-button>
              <core-grid-button v-if="refreshOption"
                                :small="modal? true: false"
                                @click="refresh"
                                text="Recarregar"
                                icon="fas fa-sync-alt">
              </core-grid-button>
              <core-grid-button v-if="exportPdfOption"
                                :small="modal? true: false"
                                text="Exportar para PDF"
                                icon="fas fa-file-pdf">
              </core-grid-button>
              <core-grid-button v-if="exportExcelOption"
                                @click="downloadExcel($event)"
                                :small="modal? true: false"
                                text="Exportar para Excel"
                                icon="fas fa-file-excel">
                <JsonExcel ref="excel"
                           v-show="false"
                           v-bind="excelOptions">
                  <v-icon small>fas fa-file-excel</v-icon>
                </JsonExcel>
              </core-grid-button>
            </v-col>

            <v-col v-if="!searchDisabled" cols="12" md="4" lg="3">
              <div class="flex-grow-0"></div>
              <v-text-field v-model="search"
                            max-width="250"
                            append-icon="search"
                            @click:append="refresh()"
                            label="Pesquisar"
                            hide-details></v-text-field>
            </v-col>
          </v-row>
        </v-card>
      </v-card>
    </template>

     <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
  </v-data-table>
</template>
<script>
import JsonExcel from 'vue-json-excel'
export default {
  components: {
    JsonExcel
  },
  props: {
    // diminui o padding em volta do grid
    modal: { type: Boolean, default: false },
    // retira a elevation
    flat: { type: Boolean, default: false },
    // não carrega o grid ao entrar na tela
    autoBind: { type: Boolean, default: true },
    // exibir botão de novo item
    addOption: { type: Boolean, default: true },
    // exibir botão de editar item
    editOption: { type: Boolean, default: true },
    // exibir botão de excluir item
    deleteOption: { type: Boolean, default: true },
    // exibir botão de filtro
    filterOption: { type: Boolean, default: false },
    // abrir o filtro ao carregar a tela
    filterOpen: { type: Boolean, default: false },
    // exibir botão de recarregar do grid
    refreshOption: { type: Boolean, default: true },
    // exibir botões de exportar para pdf
    exportPdfOption: { type: Boolean, default: false },
    // exibir botões de exportar para excel
    exportExcelOption: { type: Boolean, default: false },
    // desabilita o campo de pesquisa
    searchDisabled: { type: Boolean, default: false },
    // componente nas $refs da tela principal para exclusão, por padrão é 'excluir'
    deleteFunction: { type: String, default: 'excluir' },
    // mensagem de confirmação de exclusão padrão
    deleteConfirmMsg: {
      type: String,
      default: 'Confirma a Exclusão do Registro?'
    },
    // componente nas $refs da tela principal para edição, por padrão é 'editar'
    addFunction: { type: String, default: null },
    // componente nas $refs da tela principal para edição, por padrão é 'editar'
    editFunction: { type: String, default: 'editar' },
    // componente nas $refs da tela principal para o filtro, por padrão é filtrar
    filterFunction: { type: String, default: 'filtrar' },
    // Botões de edição da linha agrupados
    optionButton: { type: Boolean, default: false },
    // lista de colunas do grid
    schema: Array,
    // função para leitura de dados
    onRead: Function,
    // função para clique do mouse na linha
    click: Function,
    // número de linhas do grid
    pageSize: { type: Number, default: 20 },
    // exibir checkbox de seleção
    selectable: { type: Boolean, default: false },
    // campo chave/ID para grids com caixa de seleção
    itemKey: { type: String, default: 'Codigo' },
    // selecionar somente 1 item
    singleSelect: { type: Boolean, default: true },
    // paginação no servidor
    serverSide: { type: Boolean, default: false },

    // ordenável
    sortable: { type: Boolean, default: true },
    // paginável
    pageable: { type: Boolean, default: true }
  },
  data: () => ({
    search: '',
    loading: true,
    dataSource: [],
    current: {},
    serverItemsLength: 0,
    pageCount: 0,
    options: {},
    selected: [],
    parentComponent: {},
    firstLoad: true,
    gridName: 'grid',
    headers: [],
    btnOptions: [],
    pageSizeDefault: null,
    excelOptions: {}
  }),
  computed: {
    isDense: function () {
      return (this.$attrs.dense || this.$attrs.dense === '') || this.$vuetify.breakpoint[this.$vuetify.theme.options.compact.gridDense]
    },
    addFunctionDefault: function () {
      if (this.addFunction) {
        return this.addFunction
      } else {
        return this.editFunction
      }
    },
    hasClass: function () {
      if (this.$options && this.$options._parentVnode) {
        return !!this.$options._parentVnode.data.staticClass
      } else {
        return false
      }
    },
    countSelected: function () {
      return this.selected.length
    },
    footerProps: function () {
      const props = {}
      if (this.$vuetify.theme.options.grid.pageSize) {
        props['items-per-page-options'] = this.$vuetify.theme.options.grid.pageSize
      }
      return props
    }
  },
  watch: {
    options: {
      handler () {
        if (this.serverSide || this.firstLoad) {
          this.onReadDefault()
        }
      },
      deep: true
    }
  },
  methods: {
    formats: function () {
      const th = this

      if (!th.headers || th.headers.length === 0) {
        return []
      }

      th.headers
        .filter(p => p.values)
        .forEach(function (p) {
          p.format = 'array'
        })

      th.headers
        .filter(p => p.format)
        .forEach(function (p, i) {
          if (p.format === 'button') {
            p.value =
             'buttom' + i
            p.align = p.align ? p.align : 'center'
            p.sortable = 'false'
            p.color = p.color ? p.color : 'primary'
            p.width = p.icon ? (p.width ? p.width : '20') : p.width
          }

          if (th.optionButton && p.format === 'option') {
            p.order = p.order ? p.order : 2
            th.btnOptions.push(p)
          }

          if (p.format === 'boolean') {
            p.align = 'center'
          }
          if (p.format === 'decimal' || p.format === 'currency') {
            p.align = 'end'
          }
        })

      th.schema
        .filter(p => p.template)
        .forEach(function (p) {
          p.format = 'template'
        })
      th.schema
        .filter(p => p.format === 'string')
        .forEach(function (p) {
          p.sort = function (x, y) {
            return ((x < y) ? -1 : ((x > y) ? 1 : 0))
          }
        })

      if (th.optionButton) {
        th.headers = th.headers.filter(p => p.format !== 'option')
        th.headers.push({
          format: 'optionButton',
          align: 'center',
          sortable: 'false',
          color: 'info',
          items: th.$api.UTIL.Array.OrderBy(th.btnOptions, 'order')
        })
      }

      th.headers
        .filter(p => p.format === 'optionButton')
        .forEach(function (p, i) {
          if (!p.title) { p.title = p.icon ? '' : 'Opções' }
          if (!p.color) { p.color = p.icon ? '' : 'primary' }
          if (!p.width && p.icon) { p.width = p.icon ? '20' : null }

          p.value = 'optionbutton_' + i
        })

      if (th.$vuetify.theme.options.compact) {
        th.headers
          .forEach(function (p) {
            p.divider = th.$vuetify.breakpoint[th.$vuetify.theme.options.compact.gridDivider]
            p.cellClass = th.isDense ? 'pa-1 text-caption' : p.cellClass
          })
      }

      return th.headers
        .filter(p => p.format && p.format !== 'hidden')
        .map(function (p, i) {
          if (!p.condition) {
            p.condition = () => true
          }
          return Object.assign(p, {
            value: p.value ? p.value : 'item_' + i,
            slot: 'item.' + p.value
          })
        })
    },
    getValue (field, item) {
      var val = field.values.find(
        p =>
          p.value &&
            item[field.value] &&
            p.value.toString().toUpperCase() ===
            item[field.value].toString().toUpperCase()
      )
      if (val) {
        return val.text
      } else {
        return ''
      }
    },
    row_classes (item) {
      return item.selected ? 'selecionado' : ''
    },
    optionSuccess (response, length) {
      this.loading = false
      if (response === '') { response = [] }
      if (response) {
        response.forEach(p => {
          p.selected = false
        })
        this.dataSource = response
        this.pageCount = length || response.length
        this.serverItemsLength = length || response.length
      }
    },
    downloadExcel: function (e) {
      e.preventDefault()
      e.stopPropagation()
      const th = this
      const options = {}

      if (!th.dataSource || th.dataSource.length === 0) {
        th.$api.UI.ShowAlert('Não há dados para download')
        return false
      }

      options.name = th.$route.meta.title

      let fields = {}
      // eslint-disable-next-line no-unused-vars
      for (const [_, val] of Object.entries({ ...th.schema })) {
        const { value, text, format } = val
        fields = Object.assign({}, { ...fields, [text]: value })
        if (format === 'string') { options.stringifyLongNum = true }
      }
      options.fields = fields

      function fetch () {
        const excelData = th.dataSource
        // eslint-disable-next-line no-unused-vars
        for (const [_, val] of Object.entries({ ...th.schema })) {
          const { value, format } = val
          if (format) {
            excelData.map((data, i) => {
              for (const [k, v] of Object.entries(data)) {
                if (value === k) {
                  if (v) {
                    const formatValue = {
                      documento: th.$api.UI.Format.DocumentoFederal(v),
                      currency: th.$api.UI.Format.Currency(v)
                    }[format]
                    if (format === 'date' && v.length > 10) { excelData[i][k] = th.$api.UI.Format.Date(v) } else if (formatValue) { excelData[i][k] = formatValue } else { excelData[i][k] = v }
                  }
                }
              }
            })
          }
        }
        return excelData
      }

      options.fetch = fetch

      th.excelOptions = options
      th.$nextTick(() => th.$refs.excel.generate())
    },
    onReadDefault () {
      const th = this
      setTimeout(function () {
        if (th.$store.getters.loading || th.$store.getters.activeRequests > 0) {
          th.onReadDefault()
          return false
        }
        th.selected = []
        th.current = {}
        if (!th.firstLoad || th.autoBind) {
          if (th.onRead) {
            th.loading = true
            th.onRead(th.options, th.search)
          } else {
            th.options.success(null)
          }
        }
        th.firstLoad = false
      }, 10)
    },
    refresh () {
      this.onReadDefault()
    },
    incluir: function (e) {
      e.preventDefault()
      this.parentComponent.$refs[this.addFunctionDefault].openModal()
    },
    editar: function (e, item) {
      e.preventDefault()
      if (!item || typeof item !== 'object') {
        this.$api.UI.Mensagem('alerta', 'É necessário selecionar um registro')
      } else {
        var obj = item ? JSON.parse(JSON.stringify(item)) : item
        this.parentComponent.$refs[this.editFunction].openModal(obj)
      }
    },
    filtrar: function () {
      this.parentComponent.$refs[this.filterFunction].openModal()
    },
    deletar: function (e, item) {
      e.preventDefault()
      const th = this
      if (!item || typeof item !== 'object') {
        th.$api.UI.Mensagem('alerta', 'É necessário selecionar um registro')
      } else if (item) {
        th.$api.UI.Confirmacao('alerta', th.deleteConfirmMsg, function () {
          th.parentComponent[th.deleteFunction](item)
        })
      }
    },
    showModal (action, item) {
      if (!item || typeof item !== 'object') {
        this.$api.UI.Mensagem('alerta', 'É necessário selecionar um registro')
      } else {
        var componente = this.parentComponent.$refs[action]
        var obj = item ? JSON.parse(JSON.stringify(item)) : item
        componente.openModal(obj)
      }
    },
    clickDefault (item) {
      this.current.selected = null
      item.selected = true
      this.current = item
      if (this.click) {
        this.click(item)
      }
    }
  },
  beforeMount () {
    const th = this
    if (th.exportExcelOption) {
      let fields = {}
      // eslint-disable-next-line no-unused-vars
      for (const [key, val] of Object.entries({ ...th.schema })) {
        const { value, text } = val
        fields = Object.assign({}, { ...fields, [text]: value })
      }
      th.excelOptions.fields = fields
    }
  },
  mounted () {
    const th = this
    th.headers.push({ value: 'selected', align: ' d-none' })
    th.schema.forEach(p => th.headers.push(p))
    th.gridName = th.$options._parentVnode.data.ref
    th.parentComponent = th.$parent
    while (th.parentComponent) {
      if (th.parentComponent.$refs[th.gridName]) {
        break
      }
      th.parentComponent = th.parentComponent.$parent
    }
    th.options.success = th.optionSuccess
    if (th.filterOpen && th.parentComponent.$refs[th.filterFunction]) {
      th.filtrar()
    }

    if (th.optionButton) {
      if (th.editOption) {
        th.btnOptions.push({
          format: 'option',
          order: 1,
          title: 'Editar',
          icon: 'fas fa-pencil-alt',
          action: th.editar
        })
      }
      if (th.deleteOption) {
        th.btnOptions.push({
          format: 'option',
          order: 99,
          title: 'Excluir',
          icon: 'fas fa-trash-alt',
          action: th.deletar
        })
      }
    } else {
      if (th.editOption) {
        th.headers.push({
          format: 'button',
          tooltip: 'Editar',
          icon: 'fas fa-pencil-alt',
          action: th.editar,
          width: th.isDense ? '10' : '20',
          fixed: true
        })
      }
      if (th.deleteOption) {
        th.headers.push({
          format: 'button',
          tooltip: 'Excluir',
          icon: 'fas fa-trash-alt',
          action: th.deletar,
          width: th.isDense ? '10' : '20',
          fixed: true
        })
      }
    }
  },
  created () {
    this.pageSizeDefault = this.pageSize
    if (this.pageSize === 20 && this.$vuetify.theme.options.grid.rows) this.pageSizeDefault = this.$vuetify.theme.options.grid.rows
  }
}
</script>

<style>
  .v-data-table.custom-grid-class
  .v-btn:not(.v-btn--text):not(.v-btn--outlined).v-btn--active:before {
    opacity: 0;
  }
</style>
