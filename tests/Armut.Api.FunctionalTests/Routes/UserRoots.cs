namespace Armut.Api.FunctionalTests.Routes
{
    internal class UserRoots
    {
        internal static readonly string Root = $"{ApiVersion.Version}/user";

        internal static string GetUser(int userId) => $"{Root}/{userId}";
    }
}