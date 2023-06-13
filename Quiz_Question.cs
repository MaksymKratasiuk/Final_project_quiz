using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_project_quiz
{
    internal class Quiz_Question
    {
        public string Question { get; set; }
        public string Answer { get; set; }


        public void LoadRandomQuestion(string FilePath)
        {
            
            // Зчитуємо всі рядки з файлу
            string[] lines = File.ReadAllLines(FilePath);

            // Випадковим чином вибираємо питання зі списку
            string randomLine = lines[new Random().Next(0, lines.Length)];

            // Розділяємо питання та відповідь
            string[] parts = randomLine.Split(':');

            if (parts.Length >= 2)
            {
                // Перша частина - питання
                Question = parts[0].Trim();

                // Друга частина - відповідь
                Answer = parts[1].Trim();
            }
            else
            {
                // Якщо не вдалося розділити питання та відповідь, просто присвоюємо рядок у поле питання
                Question = randomLine;
            }
        }

        

        public bool EqualQuestion(Quiz_Question question)
        {
            return Question == question.Question && Answer == question.Answer;
        }
        public bool IsCorrectAnswer(string answer)
        {
            answer = answer.Replace("?", "і");
            bool bl = answer.Trim().ToLower() == Answer.ToLower();
            return bl ;
        }

        public static List<Quiz_Question> LoadQuestionsFromFile(string filePath)
        {
            // Зчитуємо всі рядки з файлу
            
            string[] lines = File.ReadAllLines(filePath);

            List<Quiz_Question> loadedQuestions = new List<Quiz_Question>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');

                if (parts.Length >= 2)
                {
                    string question = parts[0].Trim();
                    string answer = parts[1].Trim();

                    Quiz_Question quizQuestion = new Quiz_Question()
                    {
                        Question = question,
                        Answer = answer
                    };

                    loadedQuestions.Add(quizQuestion);
                }
            }

            return loadedQuestions;
        }


    }
}
