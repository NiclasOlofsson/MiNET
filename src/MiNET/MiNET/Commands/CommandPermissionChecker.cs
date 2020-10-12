using System;

namespace MiNET.Commands
{
    public abstract class CommandPermissionChecker
    {
        public abstract bool HasPermission(CommandPermissionAttribute attr, Player.Player player);
    }
    
    public class CommandPermissionChecker<TType> : CommandPermissionChecker where TType : CommandPermissionAttribute
    {
        public virtual bool HasPermission(TType attr, Player.Player player)
        {
            throw new NotImplementedException();
        }

        public override bool HasPermission(CommandPermissionAttribute attr, Player.Player player)
        {
            if (attr is TType cmdAttribute)
                return HasPermission(cmdAttribute, player);
            
            throw new InvalidOperationException("The attribute type does not match the expected type!");
        }
    }
}
