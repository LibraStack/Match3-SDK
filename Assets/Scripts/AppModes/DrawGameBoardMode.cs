namespace AppModes
{
    public class DrawGameBoardMode : AppMode<int[,]>
    {
        private readonly int[,] _gameBoardData =
        {
            {1, 0, 1, 1, 1, 1, 1, 0, 1},
            {1, 0, 1, 1, 1, 1, 1, 0, 1},
            {1, 0, 1, 1, 1, 1, 1, 0, 1},
            {1, 0, 1, 1, 1, 1, 1, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 0, 0, 0, 1, 1, 1}
        };

        public override void Activate()
        {
            // TODO: Draw game board by mouse.
            RaiseFinished(_gameBoardData);
        }

        public override void Deactivate()
        {
        }
    }
}