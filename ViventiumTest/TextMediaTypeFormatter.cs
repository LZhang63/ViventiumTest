
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace ViventiumTest
{
    //CompanyController.DataStore needs to take text/plain input. This formatter is to support that need.

    public class TextMediaTypeFormatter : InputFormatter
    {
        public TextMediaTypeFormatter() => this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        public override bool CanRead(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string? contentType = context.HttpContext.Request.ContentType;
            
            return string.IsNullOrEmpty(contentType) ||
                (contentType??"") == "text/plain";
        }
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            string? contentType = context.HttpContext.Request.ContentType;

            if (string.IsNullOrEmpty(contentType) || contentType == "text/plain")
            {
                using StreamReader reader = new(context.HttpContext.Request.Body);
                string strContent = await reader.ReadToEndAsync();
                return await InputFormatterResult.SuccessAsync(strContent);
            }
            return await InputFormatterResult.FailureAsync();
        }

    }
}
