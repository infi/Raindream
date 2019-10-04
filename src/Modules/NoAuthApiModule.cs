using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json.Linq; // json
using System.Net.Http; // http
using Discord; // embed stuff

namespace Raindream.Modules
{

    public class NoAuthApiModule : ModuleBase<SocketCommandContext>
    {
        public string CapitalizeFirst(string input)
        {
            return // things like this is why I miss JS
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        HttpClient http = new HttpClient();

        [Command("yn")]
        public async Task RunDecision()
        {
            HttpResponseMessage res = await http.GetAsync("https://yesno.wtf/api");
            string body = await res.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(body);
            string answer = (string)json["answer"];
            string image = (string)json["image"];

            EmbedBuilder embedDecision = new EmbedBuilder();
            embedDecision.Title = CapitalizeFirst(answer) + "!";
            embedDecision.ImageUrl = image;
            embedDecision.Color = new Color(0x21221);

            await Context.Channel.SendMessageAsync(null, false, embedDecision.Build());
        }

        [Command("license")]
        public async Task RunLicense([Remainder] string spdx = "n")
        {
            if (spdx == "n")
            {
                await Context.Channel.SendMessageAsync("I need a license.");
            }
            else if (spdx == "advice")
            {
                await Context.Channel.SendMessageAsync(@"<https://developer.github.com/v3/licenses/> states: 
                >>>     GitHub is a lot of things, but it’s not a law firm. As such, GitHub does not provide legal advice. Using the Licenses API or sending us an email about it does not constitute legal advice nor does it create an attorney-client relationship. If you have any questions about what you can and can't do with a particular license, you should consult with your own legal counsel before moving forward. In fact, you should always consult with your own lawyer before making any decisions that might have legal ramifications or that may impact your legal rights.
                    GitHub created the License API to help users get information about open source licenses and the projects that use them. We hope it helps, but please keep in mind that we’re not lawyers (at least not most of us aren't) and that we make mistakes like everyone else. For that reason, GitHub provides the API on an “as-is” basis and makes no warranties regarding any information or licenses provided on or through it, and disclaims liability for damages resulting from using the API.");
            }
            else
            {
                HttpClient adHocLicenseClient = new HttpClient();
                HttpRequestMessage req = new HttpRequestMessage()
                {
                    RequestUri = new System.Uri($"https://api.github.com/licenses/{spdx}"),
                    Method = HttpMethod.Get,
                };
                req.Headers.Add("User-Agent", "Raindream");
                HttpResponseMessage res = await adHocLicenseClient.SendAsync(req);
                string body = await res.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(body);

                if (((string)json["message"]) == "Not Found")
                {
                    await Context.Channel.SendMessageAsync("Nothing found. Use the SPDX ID of the license.");
                }
                else
                {
                    string description = (string)json["description"];
                    string name = (string)json["name"];
                    bool featured = (bool)json["featured"];
                    string[] permissions = ((JArray)json["permissions"]).ToObject<string[]>();
                    string[] conditions = ((JArray)json["conditions"]).ToObject<string[]>();
                    string[] limitations = ((JArray)json["limitations"]).ToObject<string[]>();

                    EmbedBuilder embedLicense = new EmbedBuilder();
                    embedLicense
                        .WithFooter("Not legal advice - see `license advice`")
                        .WithColor(0x212121)
                        .WithDescription(description)
                        .WithTitle(name)
                        .AddField("Permissions", string.Join("\n", permissions), true)
                        .AddField("Conditions", string.Join("\n", conditions), true)
                        .AddField("Limitations", string.Join("\n", limitations), true)
                        .AddField("Common?", (featured ? "Yes" : "No"));
                    await Context.Channel.SendMessageAsync(null, false, embedLicense.Build());
                }


            }
        }
    }
}
