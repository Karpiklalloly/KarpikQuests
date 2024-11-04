using HowTo._2._Linear_Quest;
using HowTo._3._Veriative_Quest;
using HowTo._4._Serialization;

namespace HowTo;

class Program
{
    static void Main(string[] args)
    {
        IProgram program = new Serialization();
        program.Run();
    }
}