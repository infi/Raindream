using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace Raindream.Modules
{
    public class MathModule : ModuleBase<SocketCommandContext>
    {
        [Command("calc")]
        public async Task RunMath([Remainder] string input = "n")
        {
            
            if (input == "n")
            {
                await Context.Channel.SendMessageAsync("<:IconProvide:553870022125027329> Format: `rd-calc num1 [+,-,*,/,%,**,rt] num2`");
                return;
            }
            string[] args = input.Split(" ");
            decimal num1;
            string operation;
            decimal num2;

            try
            {
                num1 = System.Convert.ToDecimal(args[0]);
                operation = args[1];
                num2 = System.Convert.ToDecimal(args[2]);

                decimal result;
                string UXoperation;

                switch (operation)
                {
                    case "+":
                        result = num1 + num2;
                        UXoperation = "plus";
                        break;
                    case "-":
                        result = num1 - num2;
                        UXoperation = "minus";
                        break;
                    case "*":
                        result = num1 * num2;
                        UXoperation = "by";
                        break;
                    case "/":
                        result = num1 / num2;
                        UXoperation = "divided by";
                        break;
                    case "%":
                        result = num1 % num2;
                        UXoperation = "mod";
                        break;
                    case "**":
                        // lots of casting because we need decimals and Math.Pow needs doubles
                        result = (decimal)System.Math.Pow((double)num1, (double)num2);
                        UXoperation = "exponented by";
                        break;
                    case "rt":
                        result = (decimal)System.Math.Pow((double)num2, 1 / (double)num1);
                        UXoperation = "** 1 /";
                        break;
                    default:
                        throw new System.Exception("Unknown operation");
                }

                EmbedBuilder embedCalc = new EmbedBuilder();
                embedCalc
                    .WithColor(0x212121)
                    .WithTitle($"{num1} {UXoperation} {num2}")
                    .WithDescription($"```js\n{result}\n```");
                await Context.Channel.SendMessageAsync(null, false, embedCalc.Build());
            }
            catch
            {
                await Context.Channel.SendMessageAsync("<:IconProvide:553870022125027329> Format: `rd-calc num1 [+,-,*,/,%,**,rt] num2`");
            }
        }
    }
}