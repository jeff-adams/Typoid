using System;

namespace Typoid
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            new Game().Run();
            Console.CursorVisible = true;
        }
    }
}
