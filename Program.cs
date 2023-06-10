namespace Final_project_quiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Quiz_Question question = new Quiz_Question();
            question.LoadRandomQuestion();
            Console.WriteLine(question.Question);

            Console.WriteLine(question.Answer );
        
        }
    }
}