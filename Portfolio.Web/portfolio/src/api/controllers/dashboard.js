import util from "../util";

const controller = "Dashboard";
export default Object.assign({}, util.GetCrud(controller, ["get", "getall"]));
