namespace src.Program;
using src.Fork;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        Task.WaitAll(ExecuteAsync());
    }

    public static async Task ExecuteAsync()
    {   
        
        //Sets up the connection to Github API
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://api.github.com");

        //to Generate a github token under your github account settings
        //Token Very SECRET use the following commands to not leak access to you github xDDD
        //Need to implement secrets "Git-Token" = Your Git Token

        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var token = "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";

        client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);
        
        //takes the "/repos/:Organization name:/:Repo Name:/forks" as an argument
        var gitForks = await client.GetAsync("/repos/Divik-kid/BDSA00/forks");
        gitForks.EnsureSuccessStatusCode();

        string responseBody = await gitForks.Content.ReadAsStringAsync();
        var forkList = JsonConvert.DeserializeObject<List<Fork>>(responseBody);

        //prints username of the forks' creators
        foreach(var f in forkList!){
            Console.WriteLine(f.owner!.login);
        }  

        client.Dispose();       
    }
   
}