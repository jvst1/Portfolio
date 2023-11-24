import util from "../../util";

const controller = "Clients";
export default Object.assign(
  {},
  util.GetCrud(controller, ["getall", "post", "put", "persist"])
);
