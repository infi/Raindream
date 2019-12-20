using System.Threading.Tasks;
using Discord.Commands;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Discord;
using System.Linq;

namespace Raindream.Modules
{
    public class PackageManagerApiModule : ModuleBase<SocketCommandContext>
    {
        public static bool ArrayIsNullOrEmpty<T>(T[] array) where T : class
        {
            if (array == null || array.Length == 0)
                return true;
            else
                return array.All(item => item == null);
        }

        HttpClient http = new HttpClient();

        [Command("mvn")]
        public async Task RunMaven([Remainder] string query = "n")
        {
            if (query == "n")
            {
                await Context.Channel.SendMessageAsync("<:IconProvide:553870022125027329> I need a query");

            }
            else
            {
                HttpResponseMessage res = await http.GetAsync($"http://search.maven.org/solrsearch/select?q={query}&wt=json");
                string body = await res.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(body);

                JObject mvnResponse = (JObject)json["response"];
                int numFound = (int)mvnResponse["numFound"];
                JObject[] mvnDocs = ((JArray)mvnResponse["docs"]).ToObject<JObject[]>();
                if (numFound > 0)
                {
                    JObject mvnFirst = mvnDocs[0];

                    string g = (string)mvnFirst["g"];
                    string a = (string)mvnFirst["a"];
                    string latest = (string)mvnFirst["latestVersion"];
                    string repo = (string)mvnFirst["repositoryId"];

                    EmbedBuilder embedMaven = new EmbedBuilder();
                    embedMaven
                        .WithColor(0xdc6328)
                        .WithTitle("Result")
                        .AddField("Group ID", $"`{g}`", true)
                        .AddField("Artifact ID", a, true)
                        .AddField("Current Version", latest, true)
                        .AddField("Repository", repo, true);
                    await Context.Channel.SendMessageAsync(null, false, embedMaven.Build());
                }
                else
                {
                    EmbedBuilder embedMaven = new EmbedBuilder();
                    embedMaven
                        .WithColor(0xdc6328)
                        .WithTitle("<:IconProvide:553870022125027329> Nothing found")
                        .WithDescription("Try something different.");
                    await Context.Channel.SendMessageAsync(null, false, embedMaven.Build());
                }
            }
        }

        [Command("npm")]
        public async Task RunYarn([Remainder] string query = "n")
        {
            if (query == "n")
            {
                await Context.Channel.SendMessageAsync("<:IconProvide:553870022125027329> I need a package name");
            }
            else
            {
                HttpResponseMessage res = await http.GetAsync($"https://registry.yarnpkg.com/{query}");
                string body = await res.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(body);

                string error = (string)json["error"];
                if (error == "Not found")
                {
                    EmbedBuilder embedYarn = new EmbedBuilder();
                    embedYarn
                        .WithColor(0xfb3b49)
                        .WithTitle("<:IconProvide:553870022125027329> Nothing found")
                        .WithDescription("Try something different.");
                    await Context.Channel.SendMessageAsync(null, false, embedYarn.Build());
                }
                else
                {
                    // Parse the JSON
                    string name = (string)json["name"];
                    string description = (string)json["description"];
                    JObject distTags = (JObject)json["dist-tags"];
                    string latestVer = (string)distTags["latest"];
                    JObject versions = (JObject)json["versions"];
                    JObject latestRelease = (JObject)versions[latestVer];
                    string[] keywords = ((JArray)latestRelease["keywords"]).ToObject<string[]>();
                    JObject authorInfo = (JObject)latestRelease["author"];
                    string jsonAuthor = (string)authorInfo["name"];
                    JObject[] maintainers = ((JArray)json["maintainers"]).ToObject<JObject[]>();
                    string maintainer = (string)maintainers[0]["name"];

                    EmbedBuilder embedYarn = new EmbedBuilder();
                    embedYarn
                        .WithColor(0xfb3b49)
                        .WithTitle("Result")
                        .AddField("Name", $"{name}")
                        .AddField("Description", $"{description}")
                        .AddField("Current Version", latestVer)
                        .AddField("Keywords", $"`{string.Join(",", keywords)}`")
                        .AddField("Author", $"NPM says `{maintainer}` | package.json says {jsonAuthor}");
                    await Context.Channel.SendMessageAsync(null, false, embedYarn.Build());
                }
            }
        }

        [Command("nuget")]
        public async Task RunNuget([Remainder] string query = "n")
        {
            if (query == "n")
            {
                await Context.Channel.SendMessageAsync("<:IconProvide:553870022125027329> I need a package name");
            }
            else
            {
                HttpResponseMessage res = await http.GetAsync($"https://azuresearch-usnc.nuget.org/query?q={query}&take=1");
                string body = await res.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(body);

                uint totalHits = (uint)json["totalHits"];

                if (totalHits > 0)
                {
                    JObject package = (((JArray)json["data"]).ToObject<JObject[]>())[0];
                    string nspace = (string)package["id"];
                    string version = (string)package["version"];
                    string description = (string)package["description"];
                    string name = (string)package["title"];
                    string iconUri = (string)package["iconUrl"];
                    string[] tags = ((JArray)package["tags"]).ToObject<string[]>();
                    string[] authors = ((JArray)package["authors"]).ToObject<string[]>();
                    bool verified = (bool)package["verified"];
                    uint downloads = (uint)package["totalDownloads"];

                    EmbedBuilder embedNuget = new EmbedBuilder();
                    embedNuget
                        .WithColor(0x004980)
                        .WithTitle("Result")
                        .AddField("Name", name, true)
                        .AddField("Description", description, true)
                        .AddField("Namespace", $"`{nspace}`", true)
                        .AddField("Current Version", version, true)
                        .AddField("Author", string.Join(", ", authors), true)
                        .AddField("Tags", $"`{string.Join(",", tags)}`", true)
                        .AddField("Verified", verified, true)
                        .AddField("Downloads", downloads, true)
                        .WithThumbnailUrl(iconUri);
                    await Context.Channel.SendMessageAsync(null, false, embedNuget.Build());
                }
                else
                {
                    EmbedBuilder embedNuget = new EmbedBuilder();
                    embedNuget
                        .WithColor(0x004980)
                        .WithTitle("<:IconProvide:553870022125027329> Nothing found")
                        .WithDescription("Try something different.");
                    await Context.Channel.SendMessageAsync(null, false, embedNuget.Build());
                }
            }
        }
    }
}