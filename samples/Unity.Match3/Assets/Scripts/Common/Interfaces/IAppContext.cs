namespace Common.Interfaces
{
    public interface IAppContext
    {
        T Resolve<T>();
    }
}