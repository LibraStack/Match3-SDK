using Implementation.Common.Interfaces;

namespace Implementation.Common.Extensions
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