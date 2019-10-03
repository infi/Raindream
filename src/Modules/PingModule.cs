using System.Threading.Tasks;
using Discord.Commands;
using Raindream;

namespace Raindream.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task RunPing()
        {
            var msg = await Context.Channel.SendMessageAsync("⏱");
            await msg.ModifyAsync((m_) => {
                m_.Content = $"<:IconThis:553869005820002324> Latency: {(Context.Message.CreatedAt - msg.CreatedAt).Milliseconds}ms. API Latency: {Context.Client.Latency}ms.";
            });
        }
    }
}
