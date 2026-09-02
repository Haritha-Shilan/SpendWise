namespace SpendWise.Infrastructure.Repositories
{
    public class QueryOptions<T> where T: class,IEntity
    {

        public List<Expression<Func<T, bool>>> Filters { get; set; } = new();

        public Expression<Func<T, object>>? OrderBy { get; set; }

        public bool OrderDescending { get; set; }

        private string[] _includes = Array.Empty<string>();

        public string Includes
        {
            set=> _includes =value.Replace(" ","").Split(',',StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] GetIncludes => _includes;

        public bool HasFilter => Filters.Any();

        public bool HasOrderBy => OrderBy != null;
    }
}
