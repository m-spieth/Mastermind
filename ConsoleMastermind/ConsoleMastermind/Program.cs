using System;

namespace ConsoleMastermind
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Mastermind mastermind = new Mastermind();
                mastermind.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unknow error occurred while running Mastermind.");
                Console.WriteLine("Error message: " + ex.Message);
            }
            
        }
    }
}
