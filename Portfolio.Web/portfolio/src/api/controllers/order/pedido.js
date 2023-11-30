import util from "../../util";

const controller = "Pedido";
export default Object.assign({}, util.GetCrud(controller, []), {
    Post: function (codigoComerciante, dto) {
        if (!dto)
            dto = {};

        return util.Axios.Post(`${controller}/${codigoComerciante}`, dto);
    },
}); 