namespace Implementation.Common.Interfaces
{
    public interface IAppContext
    {
        T Resolve<T>();
    }
}