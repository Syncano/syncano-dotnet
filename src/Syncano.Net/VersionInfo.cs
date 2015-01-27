namespace Syncano.Net
{
    public static class VersionInfo
    {
        public const string Version = "1.0.5.0";

        public static readonly string Identifier;

        static VersionInfo()
        {
            Identifier = string.Format("syncano-dotnet-{0}", VersionInfo.Version);
        }

    }
}