using Gls_Etykiety.Domain;
using Gls_Etykiety.Models;
using Gls_Etykiety.Models.JsonResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
namespace Gls_Etykiety.azure_functions;

public class GetLabels
{
    private readonly IDbContextFactory<LabelDbContext> _contextFactory;

    public GetLabels(IDbContextFactory<LabelDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    [Function("GetLabels")]
    public async Task<IActionResult> Run([TimerTrigger("0 */10 * * * *")] TimerInfo myTimer)
    {
        // zaloguj sie
        // zwróæ session id
        // pobierz id paczek ( potrzebne session Id )
        // pobierz etykiety ( potrzebne session Id, Id paczki )
        // zapisz Etykiety do bazy
        
        var client = new RestClient("https://xxx.xxxxxxxx.xx/ade_webapi2.php?wsdl");
        
        using(var context = _contextFactory.CreateDbContext())
        {
            var users = context.Users.ToList();

            foreach(var user in users)
            {
                var logInRequest = new RestRequest(@"\adelogin", Method.Post)
                    .AddJsonBody(new { username = user.username, password = user.password });

                var logInResponse = await client.PostAsync(logInRequest);

                
                if (logInResponse.IsSuccessful && logInResponse.Content != string.Empty)
                {
                    SessionResponse sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(logInResponse.Content);

                    string sessionId = sessionResponse.SessionData.Data;

                    var packageIdsList =  new List<int>();
                    var startId = 0;

                    do
                    {
                        var packageIds = new RestRequest(@"\adePreparingBox_GetConsignIDs", Method.Post)
                            .AddJsonBody(new { session = sessionId, id_start = startId });

                        var packageIdsResponse = await client.PostAsync(packageIds);
                        ConsignsIDsResponse consignsIDsResponse = JsonConvert.DeserializeObject<ConsignsIDsResponse>(packageIdsResponse.Content);

                        packageIdsList.AddRange(consignsIDsResponse.ConsignsIDsArray.ConsignsIDs);

                        startId = consignsIDsResponse.ConsignsIDsArray.ConsignsIDs.LastOrDefault();

                    }
                    while (startId > 0 );

                   List<Label> labels = new List<Label>();

                   foreach(int id in packageIdsList)
                    {
                        var getLabelRequest = new RestRequest(@"\adePreparingBox_GetConsignLabels")
                            .AddJsonBody(new { session = sessionId, id = id, mode = "adePreparingBox_GetConsignLabels" });

                        var getLabelResponse = await client.PostAsync(getLabelRequest);

                        LabelResponse labelResponse = JsonConvert.DeserializeObject<LabelResponse>(getLabelResponse.Content);

                        labelResponse.Label.UserId = user.Id;

                        labels.Add(labelResponse.Label);
                    }

                   context.Labels.AddRange(labels);
                   await context.SaveChangesAsync();
                }
                
            }
        }

        return new StatusCodeResult(200);
    }
}
