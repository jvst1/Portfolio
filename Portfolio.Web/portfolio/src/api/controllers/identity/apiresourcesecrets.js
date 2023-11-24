import util from "../../util";

const controller = "ApiResourceSecrets";
export default Object.assign(
  {},
  util.GetCrud(controller, ["getall", "post", "put", "persist"])
);
