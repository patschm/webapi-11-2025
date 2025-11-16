using Microsoft.AspNetCore.Html;
using PageMaker.Interfaces;
using System.Text;

namespace PageMaker.Services
{
    public class TableServiceOptimal : ITableService
    {
        private StringBuilder table = new StringBuilder();

        public async Task<HtmlString> GenerateTableAsync(Stream? csv)
        {
            table.Append("<table>");

            using (var reader = new StreamReader(csv!))
            {
                string? line = await reader.ReadLineAsync();
                table.Append("<thead>");
                ParseHeader(line);
                table.Append("</thead>");
                table.Append("<tbody>");
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    ParseLine(line);
                }
                table.Append("</tbody>");
            }
            table.Append("</table>");
            return new HtmlString(table.ToString());
        }

        private void ParseLine(string line)
        {
            table.Append("<tr>");
            var cells = line.Split(';');
            for (var i = 0; i < cells.Length; i++)
            {
                table.Append("<td>");
                table.Append(cells[i]);
                table.Append("</td>");
            }
            table.Append("</tr>");
        }

        private void ParseHeader(string? line)
        {
            table.Append("<tr>");
            var cells = line!.Split(';');
            for (var i = 0; i < cells.Length; i++)
            {
                table.Append("<th>");
                table.Append(cells[i]);
                table.Append("</th>");
            }
            table.Append("</tr>");
        }
    }
}
