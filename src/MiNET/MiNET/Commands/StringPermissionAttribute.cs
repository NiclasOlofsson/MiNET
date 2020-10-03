namespace MiNET.Commands
{
    // TODO: Figure a better name then StringPermission
    public class StringPermissionAttribute : CommandPermissionAttribute
    {
        public string Permission { get; set; }

        public StringPermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
