using MiNET.Plugins.Attributes;

namespace MiNET.Commands
{
    public class CommandData
    {
        public CommandAttribute Attribute { get; set; }
        public CommandPermissionAttribute PermissionAttribute { get; set; }
        public object Instance { get; set; }

        public CommandData(CommandAttribute attribute, object instance)
        {
            Attribute = attribute;
            Instance = instance;
        }

        public CommandData(CommandAttribute attribute, object instance, CommandPermissionAttribute permissionAttribute)
        {
            Attribute = attribute;
            Instance = instance;
            PermissionAttribute = permissionAttribute;
        }
    }
}