using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    public class ConsoleCommand
    {
        readonly string help;
        public string Help => help;
        readonly Action<string> CommandCallback;
        public ConsoleCommand(string help, Action<string> commandCallback)
        {
            this.help = help;
            CommandCallback = commandCallback;
        }
        public void Input(string input)
        {
            CommandCallback?.Invoke(input);
        }
    }
}
