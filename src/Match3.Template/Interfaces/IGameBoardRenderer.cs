using System;

namespace Match3.Template.Interfaces
{
    public interface IGameBoardRenderer : IDisposable
    {
        void CreateGridTiles(int[,] data);
        void ResetGridTiles();
    }
}