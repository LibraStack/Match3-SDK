using System;

namespace Match3.Infrastructure.Interfaces
{
    public interface IGameBoardRenderer : IDisposable
    {
        void CreateGridTiles(int[,] data);
        void ResetGridTiles();
    }
}