import util from "../../util";

const controller = "Cardapio";
export default Object.assign({}, util.GetCrud(controller, []), {
    Delete: function (codigoUsuario, dto) {
        return util.Axios.Delete(`${controller}/${codigoUsuario}/Itens`, dto);
    },
    Get: function (codigoUsuario, dto) {
        return util.Axios.Get(`${controller}/${codigoUsuario}/Itens`, dto);
    },
    GetAll: function (codigoUsuario, dto) {
        return util.Axios.GetAll(`${controller}/${codigoUsuario}/Itens`, dto);
    },
    Persist: function (codigoUsuario, dto, key) {
        if (!dto)
            dto = {};

        if (!key)
            key = "codigo";

        if (dto[key])
            return util.Axios.Put(`${controller}/${codigoUsuario}/Itens`, dto[key], dto);
        else
            return util.Axios.Post(`${controller}/${codigoUsuario}/Itens`, dto);
    },
}); 
