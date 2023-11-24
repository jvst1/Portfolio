import util from "../util";

const controller = "Authentication";
export default Object.assign({}, util.GetCrud(controller, null), {
  Authenticate: function (login, senha) {
    var dto = { Login: login, Senha: senha };
    return util.Axios.Post(controller + "/Login", dto);
  },
  Logout: function () {
    return util.Axios.Delete(controller);
  },
  RefreshToken: function (token) {
    var dto = { RefreshToken: token };
    return util.Axios.Post(controller + "/RefreshToken", dto);
  },
  RevokeRefreshToken: function (token) {
    return util.Axios.Delete(controller, "/RefreshToken/" + token + "/Revoke");
  },
});
