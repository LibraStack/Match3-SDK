namespace Common.Interfaces
{
    public interface IGameBoardDataProvider
    {
        bool[,] GetGameBoardData(int level);
    }
}