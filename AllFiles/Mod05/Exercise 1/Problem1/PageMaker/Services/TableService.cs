using Microsoft.AspNetCore.Html;
using PageMaker.Interfaces;

namespace PageMaker.Services
{
    public class TableService : ITableService
    {
        public async Task<HtmlString> GenerateTableAsync(Stream? csv)
        {
            var table ="<table>";
            
            using (var reader = new StreamReader(csv!))
            {
                string? line = await reader.ReadLineAsync();
                table += "<thead>";
                var first = ParseHeader(line);
                table += first;
                table += "</thead>";
                table += "<tbody>";
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var row = ParseLine(line);
                    table += row;
                }
                table += "</tbody>";
            }
            table += "</table>";
            return new HtmlString(table);
        }

        private string ParseHeader(string? line)
        {
            var tr = "<tr>";
            var cells = line.Split(';');
            for(var i=0; i < cells.Length; i++)
            {
                tr += "<th>";
                tr += cells[i];
                tr += "</th>";
            }
            tr += "</tr>";
            return tr;
        }

        private string ParseLine(string line)
        {
            var row = "<tr>";
            var cells = line.Split(';');
            for(var i = 0; i < cells.Length; i++)
            {
                row += "<td>";
                row += cells[i];
                row += "</td>";
            }
            row += "</tr>";
            return row;
        }
    }
}
