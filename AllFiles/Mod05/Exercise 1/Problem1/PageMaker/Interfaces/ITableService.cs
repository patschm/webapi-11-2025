using Microsoft.AspNetCore.Html;

namespace PageMaker.Interfaces
{
    public interface ITableService
    {
        Task<HtmlString> GenerateTableAsync(Stream? csv);
    }
}
