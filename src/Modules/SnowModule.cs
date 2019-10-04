using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace Raindream.Modules
{
    public class SnowModule : ModuleBase<SocketCommandContext>
    {
        [Command("snow")]
        public Task RunSnow([Remainder] string args = "n")
        {
            if (args == "n")
            {
                ulong newSnowflake = Discord.SnowflakeUtils.ToSnowflake(System.DateTime.Now);
                Context.Channel.SendMessageAsync($"`{newSnowflake}` made `{System.DateTime.Now}`");
            }
            else
            {
                ulong snowflake = System.Convert.ToUInt64(args);
                System.DateTimeOffset snow = Discord.SnowflakeUtils.FromSnowflake(snowflake);
                EmbedBuilder embedSnow = new EmbedBuilder();
                embedSnow.Title = args;
                embedSnow.Color = new Discord.Color(0x212121);
                embedSnow.AddField("Binary", System.Convert.ToString((long)snowflake, 2));
                embedSnow.AddField("Date", snow);
                Context.Channel.SendMessageAsync(null, false, embedSnow.Build());
            }
            return Task.CompletedTask;
        }
    }
}
