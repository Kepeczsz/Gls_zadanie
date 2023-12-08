using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Gls_Etykiety.Domain;
using Gls_Etykiety.Extensions;
using Gls_Etykiety.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Gls_Etykiety.azure_functions;

public class PostLabels
{
    private readonly IDbContextFactory<LabelDbContext> _contextFactory;

    public PostLabels(IDbContextFactory<LabelDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    /// <summary>
    /// This function need Id of user which is guid in post method.
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function("PostLabels")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var client = new RestClient("https://moja-drukarka.pl");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        Guid? userId = CustomJsonExtensions.ExtractIdFromJson(requestBody);

        using (var context = _contextFactory.CreateDbContext())
        {
            var labels = context.Labels.Where(x => x.UserId == userId).ToList();

            for(int i = 0; i < labels.Count; i+=10)
            {
                var labelsToSend = labels.Skip(i).Take(10).ToList();

                PdfWriter writer = new PdfWriter("Labels.pdf");
                PdfDocument pdf = new PdfDocument(writer);

                Document document = new Document(pdf);


                foreach (var label in labelsToSend)
                {
                    document.Add(new Paragraph(label.Data));
                }

                var sendLabelRequest = new RestRequest(@"/print", Method.Post);
                sendLabelRequest.AddHeader("Content-Type", "binary");
                sendLabelRequest.AddBody(document);
            }
        }






        return new StatusCodeResult(200);
    }

}
