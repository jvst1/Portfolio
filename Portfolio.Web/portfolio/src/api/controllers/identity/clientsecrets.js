import util from "../../util";

const controller = "ClientSecrets";
export default Object.assign(
  {},
  util.GetCrud(controller, ["getall", "post", "put", "persist", "delete"])
);
