import util from "../../util";

const controller = "Categoria";
export default Object.assign({}, util.GetCrud(controller, ['get', 'getall', 'persist', 'delete']), {}); 
