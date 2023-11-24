import util from "../../util";

const controller = "ApiResources";
export default Object.assign(
  {},
  util.GetCrud(controller, ["getall", "post", "put", "persist"])
);
