using HowTo._5._Custom_Quest;

namespace HowTo;

class Program
{
    static void Main(string[] args)
    {
        IProgram program = new CustomQuest();
        program.Run();
    }
}