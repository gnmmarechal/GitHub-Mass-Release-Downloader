// See https://aka.ms/new-console-template for more information
using GitHub_Mass_Release_Downloader;
using Newtonsoft.Json;

Console.WriteLine("GitHub Release Downloader");

string owner = args[0];
string repo = args[1];
Console.WriteLine("Using {0}/{1}", owner, repo);
Thread.Sleep(500);
int page = 1;
List<Root> responses = new List<Root>();
try
{
    while(true)
    {
        string response = Network.DownloadString("https://api.github.com/repos/" + owner + "/" + repo + "/releases?per_page=100&page=" + page++);
        var githubResponse = JsonConvert.DeserializeObject<List<Root>>(response);
        foreach (var item in githubResponse)
            responses.Add(item);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Pages: " + (page-1));
}
if (responses.Count > 0)
{
    File.WriteAllText("test.txt", JsonConvert.SerializeObject(responses));
}
else
{
    Console.WriteLine("Loading from file...");
    responses = JsonConvert.DeserializeObject<List<Root>>(File.ReadAllText("test.txt"));
}

Console.WriteLine();
string currentDir = Directory.GetCurrentDirectory();

if (!Directory.Exists("downloads"))
    Directory.CreateDirectory("downloads");
Directory.SetCurrentDirectory("downloads");

if (!Directory.Exists(owner))
    Directory.CreateDirectory(owner);

Directory.SetCurrentDirectory(owner);

if (Directory.Exists(repo))
    Directory.Delete(repo, true);

Directory.CreateDirectory(repo);
Directory.SetCurrentDirectory(repo);

foreach (var item in responses)
{
    Console.WriteLine(item.tag_name);
    Directory.CreateDirectory(item.tag_name);
    foreach (var asset in item.assets)
    {
        Network.DownloadFile(asset.browser_download_url, item.tag_name + Path.DirectorySeparatorChar + asset.name);

        Console.WriteLine(asset.browser_download_url);
    }
}

Directory.SetCurrentDirectory(currentDir);
