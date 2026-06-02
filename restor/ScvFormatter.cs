using System.Collections;
using System.Text;
using Domains.DTO;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace restor;

public class ScvFormatter: TextOutputFormatter
{
    public ScvFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
        Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();
        if (context.Object is IEnumerable<CompanyDTO>)
        {
            foreach (var company in (IEnumerable<CompanyDTO>)context.Object)
            {
                FormatCsv(buffer, company);
            }
        }
        else
        {
            FormatCsv(buffer, (CompanyDTO)context.Object);
        }
        await response.WriteAsync(buffer.ToString());
    }

    private void FormatCsv(StringBuilder buffer, CompanyDTO company)
    {
        buffer.AppendLine($"{company.Name},\"{company.Address},\"{company.Country}");
    }
}