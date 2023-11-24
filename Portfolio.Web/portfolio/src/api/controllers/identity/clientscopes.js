import util from "../../util";

const controller = "ClientScopes";
export default Object.assign(
  {},
  util.GetCrud(controller, ["getall", "post", "put", "persist", "delete"])
);
