namespace MyNUnit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var testsRunner = new MyNUnit();
            testsRunner.Run("..\\..\\..\\..\\TestFolder\\IgnoreMethodTest\\Assembly");
            testsRunner.PrintResult();
        }
    }
}
