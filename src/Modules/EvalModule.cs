using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Raindream.Modules {
    public class EvalModule : ModuleBase<SocketCommandContext> {
        [Command("eval")]
        public async Task RunEval([Remainder] string input) {
            try
            {
            var result = await CSharpScript.EvaluateAsync(input, ScriptOptions.Default.WithImports("Context"));
            await Context.Channel.SendMessageAsync($"```cs\n{result}\n```");
            }
            catch (System.Exception e)
            {
               await Context.Channel.SendMessageAsync($"Error: ```cs\n{e}\n```");
            }
            
        }
    }
}