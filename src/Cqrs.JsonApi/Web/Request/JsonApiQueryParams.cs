namespace Cqrs.JsonApi.Web.Request
{
    public static class JsonApiQueryParams
    {
        public const string Include = "include";
        public const string SparseFieldsetsTemplate = "fields[{0}]";
        public const string FieldFilterTemplate = "filter[{0}]";
        public const string PageSize = "page[size]";
        public const string PageNumber = "page[number]";
        public const string Sorting = "sort";
    }
}
