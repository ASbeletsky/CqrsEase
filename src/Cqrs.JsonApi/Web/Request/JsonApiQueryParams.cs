namespace Cqrs.JsonApi.Web.Request
{
    internal static class JsonApiQueryParams
    {
        internal const string Include = "include";
        internal const string SparseFieldsetsTemplate = "fields[{0}]";
        internal const string FieldFilterTemplate = "filter[{0}]";
        internal const string PageSize = "page[size]";
        internal const string PageNumber = "page[number]";
        internal const string Sorting = "sort";
    }
}
