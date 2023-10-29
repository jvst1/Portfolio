namespace Portfolio.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static List<TOut> SafeForeach<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> action)
        {
            var ret = new List<TOut>();
            var errorList = new List<string>();

            foreach (var src in source)
            {
                try
                {
                    var reg = action(src);
                    ret.Add(reg);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        errorList.Add(ex.InnerException.Message);
                    else
                        errorList.Add(ex.Message);
                }
            }

            if (errorList.Count > 0)
                throw new Exception(string.Join("<br />", errorList));

            return ret;
        }

        public static void SafeForeach<TIn>(this IEnumerable<TIn> source, Action<TIn> action)
        {
            var errorList = new List<string>();

            foreach (var src in source)
            {
                try
                {
                    action(src);
                }
                catch (Exception ex)
                {
                    errorList.Add(ex.Message);
                }
            }

            if (errorList.Count > 0)
                throw new Exception(string.Join(Environment.NewLine, errorList));
        }
    }
}
