using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Infrastructure.Helpers
{
    [ModelBinder(typeof(RequestBinder<Request>))]
    public class Request
    {
        public List<CustomParameter> parameters { get; set; }
        public string[] groupBy { get; set; }
        public string[] groupDesc { get; set; }
        public int page { get; set; }
        public int itemsPerPage { get; set; }
        public bool multiSort { get; set; }
        public bool mustSort { get; set; }
        public string[] sortBy { get; set; }
        public bool[] sortDesc { get; set; }

        public string this[string referencia]
        {
            get { return this != null && parameters != null ? parameters.Where(result => result.name == referencia).FirstOrDefault()?.value : null; }
        }

        public string[] this[string referencia, bool isArray]
        {
            get { return this != null && parameters != null ? parameters.Where(result => result.name == referencia).FirstOrDefault()?.values : null; }
        }
    }

    public class CustomParameter
    {
        public string name { get; set; }
        public string value { get; set; }
        public string[] values { get; set; }
    }
}