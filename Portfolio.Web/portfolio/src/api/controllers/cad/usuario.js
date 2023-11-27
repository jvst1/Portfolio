import util from "../../util";

const controller = "Usuario";
export default Object.assign({}, util.GetCrud(controller, ['get', 'getall', 'put', 'persist', 'delete']), {
  SolicitarLinkSenha: function (dto) {
    return util.Axios.Post(controller + "/SolicitarLinkSenha", dto);
  },
  RecuperarSenha: function (dto) {
    return util.Axios.Post(controller + "/RecuperarSenha", dto);
  },
  ValidaTokenSenha: function (token) {
    const params = { token: token };
    return util.Axios.GetParams(controller + "/ValidaTokenSenha", params);
  },
  Combo: function () {
    return util.Axios.Get(controller + "/GetToSelect");
  },
  GetAllComerciantes: function () {
    return util.Axios.Get(controller, "Comerciantes");
  },
  PutComerciante: function (codigoUsuario, dto) {
    return util.Axios.Put(controller, `${codigoUsuario}/Comerciante`, dto);
  },
}); 
