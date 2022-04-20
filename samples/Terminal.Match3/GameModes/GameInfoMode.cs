using System;
using Match3.Template.Interfaces;

namespace Terminal.Match3.GameModes
{
    public class GameInfoMode : IGameMode, IDeactivatable
    {
        public event EventHandler Finished;

        public void Activate()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine("Navigation:  Arrow keys");
            Console.WriteLine("Selection:   Spacebar key");
            Console.WriteLine("Exit:        Enter key");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");

            Console.ReadKey(true);
            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Deactivate()
        {
            Console.Clear();
        }
    }
}