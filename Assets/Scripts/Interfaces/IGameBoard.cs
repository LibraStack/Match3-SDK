using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IGameBoard : IGrid
    {
        bool IsFilled { get; }
        
        void Create(int[,] gameBoardData);
        UniTask FillAsync(IBoardFillStrategy fillStrategy);
        UniTask SwapItemsAsync(IBoardFillStrategy fillStrategy, GridPosition position1, GridPosition position2);
    }
}