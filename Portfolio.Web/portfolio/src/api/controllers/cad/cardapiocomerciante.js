import util from "../../util";

const controller = "CardapioComerciante";
export default Object.assign({}, util.GetCrud(controller, ['get', 'getall', 'persist', 'delete']), {}); 
