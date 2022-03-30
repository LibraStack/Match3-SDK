namespace Match3.App.Interfaces
{
    public interface IGameBoardDataProvider
    {
        bool[,] GetGameBoardData(int level);
    }
}