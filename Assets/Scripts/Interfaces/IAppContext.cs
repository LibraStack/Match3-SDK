namespace Interfaces
{
    public interface IAppContext
    {
        T Resolve<T>();
    }
}