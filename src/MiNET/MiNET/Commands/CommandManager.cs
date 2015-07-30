using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Commands
{
    public class CommandManager
    {
        Dictionary<string, List<CommandHandler>> commandHandlers;

        public CommandManager()
        {
            this.commandHandlers = new Dictionary<string, List<CommandHandler>>();
        }

        public void HandleRawCommand(string message, Player player)
        {
            string commandText = message.Split(' ')[0];
            message = message.Replace(commandText, "").Trim();
            commandText = commandText.Replace("/", "").Replace(".", "");

            List<string> arguments = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            HandleCommand(commandText, player, arguments);
        }

        public void HandleCommand(string command, Player player, List<String> args)
        {
            if (!commandHandlers.ContainsKey(command))
                return;

            CommandCall call = new CommandCall(command, player, args);
            foreach(CommandHandler handler in commandHandlers[command])
            {
                RequirementCheckResult result = handler.HasRequirements(call);
                if (!result.Success)
                    player.SendMessage("§c" + result.Reason + "!");
                else
                    handler.OnTrigger(call);
            }
        }

        public void AddCommandHandler(string command, CommandHandler handler)
        {
            if (!commandHandlers.ContainsKey(command))
                commandHandlers.Add(command, new List<CommandHandler>());
            commandHandlers[command].Add(handler);
        }

        public void RemoveCommandHandler(string command, CommandHandler handler)
        {
            if(commandHandlers.ContainsKey(command))
            {
                commandHandlers[command].Remove(handler);
                if(commandHandlers[command].Count == 0)
                    commandHandlers.Remove(command);
            }
        }

        public void ClearHandlers(string command)
        {
            if(commandHandlers.ContainsKey(command))
            {
                commandHandlers[command].Clear();
                commandHandlers.Remove(command);
            }
        }

    }
}
