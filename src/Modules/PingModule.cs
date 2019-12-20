using System.Threading.Tasks;
using Discord.Commands;
using Raindream.Utils;

namespace Raindream.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        private EmoteService eService = new EmoteService();

        [Command("ping")]
        public async Task RunPing()
        {
            var msg = await Context.Channel.SendMessageAsync("⏱");
            await msg.ModifyAsync((m_) => {
                m_.Content = $"{eService.emotes["this"]} Latency: {(msg.CreatedAt - Context.Message.CreatedAt).Milliseconds}ms. API Latency: {Context.Client.Latency}ms.";
            });
        }
    }
}
