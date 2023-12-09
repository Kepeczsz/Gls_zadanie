using Gls_Etykiety.Domain;
using Gls_Etykiety.Models;
using Gls_Etykiety.Models.JsonResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
namespace Gls_Etykiety.azure_functions;

public class GetLabels
{
    private readonly IDbContextFactory<LabelDbContext> _contextFactory;
    private readonly static RestClient client = new RestClient("https://xxx.xxxxxxxx.xx/ade_webapi2.php?wsdl");
    public GetLabels(IDbContextFactory<LabelDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// TimeTrigger which runs every 10 minutes.
    /// Firstly, it connects with data base via dbcontextFactory
    /// We call GetSessionId which, uses "adeLogin" functionality from gls api to return session id
    /// Then we use GetPackageIdListFromGlsApi which, uses "adePreparingBox_GetConsignIDs" functionality from gls api  to return list of packages ids
    /// after first two methods, we have everything we need to get packages, so we call GetLabelsFromGlsApi, which returns labels that we need.
    /// Data in labels is formatted in Base64, so before saving labels to database, we need to convert data to string, thats why we call ConvertPackagesDataFromBase64ToString
    /// it returns list of labels, with converted data.
    /// In the end we save labels to database, so we can use with httpTrigger.
    /// </summary>
    /// <param name="myTimer"></param>
    /// <returns></returns>

    [Function("SaveLabelsToDatabase")]
    public async Task<IActionResult> Run([TimerTrigger("0 */10 * * * *")] TimerInfo myTimer)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var users = context.Users.ToList();

            foreach (var user in users)
            {
                string sessionId = await GetSessionId(user.username, user.password);

                var packageIdsList = await GetPackageIdListFromGlsApi(sessionId);

                var labels = await GetLabelsFromGlsApi(packageIdsList, sessionId);

                var convertedLabels = ConvertPackagesDataFromBase64ToString(labels, user.Id);

                context.Labels.AddRange(convertedLabels);
                await context.SaveChangesAsync();

            }
        }

        return new StatusCodeResult(200);
    }

    public static async Task<string> GetSessionId(string username, string password)
    {
        var logInRequest = new RestRequest(@"\adelogin", Method.Post)
        .AddJsonBody(new { username = username, password = password });

        var logInResponse = await client.PostAsync(logInRequest);

        if(logInResponse.IsSuccessful && !string.IsNullOrEmpty(logInResponse.Content))
        {
            SessionResponse sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(logInResponse.Content);

            string sessionId = sessionResponse.SessionData.Data;

            return sessionId;
        }

        throw new Exception(message: logInResponse.ErrorMessage);
    }

    public static async Task<List<int>> GetPackageIdListFromGlsApi(string sessionId)
    {
        var packageIdsList = new List<int>();
        var startId = 0;

        do
        {
            var packageIds = new RestRequest(@"\adePreparingBox_GetConsignIDs", Method.Post)
                .AddJsonBody(new { session = sessionId, id_start = startId });

            var packageIdsResponse = await client.PostAsync(packageIds);
            if (packageIdsResponse.IsSuccessful && !string.IsNullOrEmpty(packageIdsResponse.Content))
            {
                ConsignsIDsResponse consignsIDsResponse = JsonConvert.DeserializeObject<ConsignsIDsResponse>(packageIdsResponse.Content);
                packageIdsList.AddRange(consignsIDsResponse.ConsignsIDsArray.ConsignsIDs);

                startId = consignsIDsResponse.ConsignsIDsArray.ConsignsIDs.LastOrDefault();
            }
            else throw new Exception(message: packageIdsResponse.ErrorMessage);
            
        }
        while (startId > 0);

        return packageIdsList;
    }

    public static async Task<List<Label>> GetLabelsFromGlsApi(List<int> packageIdsList, string sessionId)
    {
        List<Label> labels = new List<Label>();

        foreach (int id in packageIdsList)
        {
            var getLabelRequest = new RestRequest(@"\adePreparingBox_GetConsignLabels")
                .AddJsonBody(new { session = sessionId, id = id, mode = "adePreparingBox_GetConsignLabels" });

            var getLabelResponse = await client.PostAsync(getLabelRequest);

            if(getLabelResponse.IsSuccessful && !string.IsNullOrEmpty(getLabelResponse.Content))
            {
                LabelResponse labelResponse = JsonConvert.DeserializeObject<LabelResponse>(getLabelResponse.Content);

                labels.Add(labelResponse.Label);
            }
            else throw new Exception(message: getLabelResponse.ErrorMessage);
        }

        return labels;
    }

    public List<Label> ConvertPackagesDataFromBase64ToString(List<Label> labelData, Guid userId)
    {
        List<Label> labels = new List<Label>();

        foreach(var label in labelData)
        {
            byte[] convertedDataToBytes = Convert.FromBase64String(label.Data);
            var dataToAppend = Encoding.UTF8.GetString(convertedDataToBytes);

            label.Data = dataToAppend;
            label.UserId = userId;
        }

        return labels;
    }
}
