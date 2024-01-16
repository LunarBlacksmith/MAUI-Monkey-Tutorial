using MonkeyFinder.Model;
using System.Net.Http.Json; //<-- for the JSON reading capabilities

namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient _httpClient;
    public MonkeyService()
    {
        _httpClient = new HttpClient();
    }

    List<Monkey> _monkeyList = new();
    public async Task<List<Monkey>> GetMonkeys()
    {
        if (_monkeyList?.Count > 0)
        { return _monkeyList; }

        string url = "https://montemagno.com/monkeys.json";
        // asychronous task performs in the background, meaning it won't lock up the UI
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            // read the list of JSON and deserialise it and return it as a list of Monkeys.
            _monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
        }

        /* #region Package JSON Deserialiser
        // Use this if you are having issues with the above code connecting to the internet,
        // VPN issues, etc.
        using Stream stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using StreamReader reader = new StreamReader(stream);
        string contents = await reader.ReadToEndAsync();
        _monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
        #endregion
        */

        return _monkeyList;
    }
}
