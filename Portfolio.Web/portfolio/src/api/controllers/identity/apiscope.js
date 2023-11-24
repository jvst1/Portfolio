import util from "../../util";

const controller = "ApiScope";
export default Object.assign(
  {},
  util.GetCrud(controller, ["getall", "post", "put", "persist"]),
  {
    GetAllByClientID: function (id) {
      return util.Axios.Get(controller + "/GetAllByClientID", id);
    },
  }
);
