using Gls_Etykiety.Domain;
using Gls_Etykiety.Exceptions;
using Gls_Etykiety.Extensions;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    /// This function needs Id of user for which method should be exectuted,
    /// Parameter we need to pass in jsonBody is " id "  in guid format for post method.
    /// If we get valid userId, we create scope, in which we create pdf file for every 10 labels
    /// we do that, because printer can take only 10 labels, in post method, then we add it as Paragraph, which creates new pdf page for every label,
    /// then we send it via mojadrukarka api.
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

            if(labels.IsNullOrEmpty())
                throw new NoDataFoundException(message: "There was no labels to retrive");
            
            for(int i = 0; i < labels.Count; i+=10)
            {
                var labelsToSend = labels.Skip(i).Take(Math.Min(10, labels.Count - i)).ToList();

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
