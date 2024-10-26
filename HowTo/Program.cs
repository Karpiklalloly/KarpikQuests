using HowTo._2._Linear_Quest;
using HowTo._3._Veriative_Quest;

namespace HowTo;

class Program
{
    static void Main(string[] args)
    {
        IProgram program = new VariativeQuest();
        program.Run();
    }
}