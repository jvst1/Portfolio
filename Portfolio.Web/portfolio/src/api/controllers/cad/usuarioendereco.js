import util from "../../util";

const controller = "UsuarioEndereco";
export default Object.assign({}, util.GetCrud(controller, ['get', 'getall', 'persist', 'delete']), {}); 
