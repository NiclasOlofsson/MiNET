namespace MiNET.Commands
{
    // TODO: Figure a better name then StringPermission
    public class DefaultPermissionChecker : CommandPermissionChecker<StringPermissionAttribute>
    {
        public override bool HasPermission(StringPermissionAttribute attr, Player.Player player)
		{
			return true;//player.Permissions.HasPermission(attr.Permission);
        }
    }
}
