using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_project_quiz
{
    internal class Game
    {
        private List<Quiz_Question> questions;

        public Game(string filePath)
        {
            questions = LoadQuestionsFromFile(filePath);
        }

        private List<Quiz_Question> LoadQuestionsFromFile(string filePath)
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

        public void StartQuiz(string playerName)
        {
            Player player = new Player(playerName);

            Console.WriteLine("Вітаємо, {0}! Починаємо вікторину.", player.Name);

            while (true)
            {
                Console.WriteLine("Оберіть тему вікторини:");
                Console.WriteLine("1. Географія");
                Console.WriteLine("2. Біологія");
                Console.WriteLine("3. Вийти з гри");

                Console.Write("Ваш вибір: ");
                string topicChoice = Console.ReadLine();

                if (topicChoice == "1")
                {
                    PlayQuiz(player, "географія");
                }
                else if (topicChoice == "2")
                {
                    PlayQuiz(player, "біологія");
                }
                else if (topicChoice == "3")
                {
                    Console.WriteLine("Дякуємо за гру! До побачення.");
                    break;
                }
                else
                {
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                }
            }
        }

        private void PlayQuiz(Player player, string topic)
        {
            Console.WriteLine("Оберіть рівень складності:");
            Console.WriteLine("1. Легкий");
            Console.WriteLine("2. Середній");
            Console.WriteLine("3. Складний");

            Console.Write("Ваш вибір: ");
            string difficultyChoice = Console.ReadLine();

            string difficulty;

            switch (difficultyChoice)
            {
                case "1":
                    difficulty = "легка";
                    break;
                case "2":
                    difficulty = "середня";
                    break;
                case "3":
                    difficulty = "складна";
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Повернення до вибору теми.");
                    return;
            }

            List<Quiz_Question> filteredQuestions = questions.Where(q => q.Topic == topic && q.Difficulty == difficulty).ToList();

            if (filteredQuestions.Count == 0)
            {
                Console.WriteLine("На жаль, немає питань за обраною темою та складністю. Повернення до вибору теми.");
                return;
            }

            Console.WriteLine("Починаємо вікторину з теми '{0}' та складності '{1}'.", topic, difficulty);

            while (true)
            {
                Quiz_Question question = GetRandomQuestion(filteredQuestions);

                if (question == null)
                {
                    Console.WriteLine("Ви відповіли на всі питання!");
                    break;
                }

                Console.WriteLine("Питання: {0}", question.Question);

                Console.Write("Введіть вашу відповідь: ");
                string userAnswer = Console.ReadLine();

                if (question.IsCorrectAnswer(userAnswer))
                {
                    Console.WriteLine("Відповідь правильна!");
                    player.IncreaseScore();
                }
                else
                {
                    Console.WriteLine("Відповідь неправильна!");
                }
            }

            Console.WriteLine("Ви відповіли правильно на {0} з {1} питань.", player.Score, filteredQuestions.Count);
            Console.WriteLine();
        }

        private Quiz_Question GetRandomQuestion(List<Quiz_Question> questions)
        {
            List<Quiz_Question> unusedQuestions = questions.Where(q => !q.Used).ToList();

            if (unusedQuestions.Count == 0)
                return null;

            Quiz_Question randomQuestion = unusedQuestions[new Random().Next(0, unusedQuestions.Count)];
            randomQuestion.MarkAsUsed();

            return randomQuestion;
        }
    }

    internal class Player
    {
        public string Name { get; }
        public int Score { get; private set; }

        public Player(string name)
        {
            Name = name;
            Score = 0;
        }

        public void IncreaseScore()
        {
            Score++;
        }
    }

}
