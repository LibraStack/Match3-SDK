using Common.Interfaces;

namespace Common.Extensions
{
    public static class AppModeExtensions
    {
        public static void Deactivate(this IAppMode appMode)
        {
            if (appMode is IDeactivatable deactivatable)
            {
                deactivatable.Deactivate();
            }
        }
    }
}