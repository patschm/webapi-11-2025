using ControllerWeb.DTO;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace ControllerWeb.Formatters;

public class CsvFormatter : TextOutputFormatter
{
    public CsvFormatter()
    {
        SupportedMediaTypes.Add("text/csv");
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var buffer = new StringBuilder();
        buffer.AppendLine("Name");
        if (context.Object is IEnumerable<BrandDTO> brands)
        {
            foreach (var item in brands)
            {
                buffer.AppendLine(item.Name);
            }
        }
        await context.HttpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
    }
}
