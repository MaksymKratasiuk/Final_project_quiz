namespace Final_project_quiz
{
    internal class Game
    {
        private List<Quiz_Question> questions;
        private List<Quiz_Question> UsedQuestions;


        public Game()
        {
            questions = new List<Quiz_Question>();
            UsedQuestions = new List<Quiz_Question>();
        }



        public void StartQuiz()
        {
            Player player = new Player();

            Console.WriteLine("Вітаємо, {0}! Починаємо вікторину.", player.Name);

            while (true)
            {
                Console.WriteLine("Оберіть тему вікторини:");
                Console.WriteLine("1. Географія");
                Console.WriteLine("2. Математика");
                Console.WriteLine("3. Вийти з гри");
                Console.WriteLine("4. Показати статистику гри");
                Console.WriteLine("5. Показати таблицю лідерів");

                Console.Write("Ваш вибір: ");
                int topicChoice = int.Parse(Console.ReadLine());

                string quiz_topic = "";
                switch (topicChoice)
                {
                    case 1:
                        quiz_topic = "geography";
                        break;
                    case 2:
                        quiz_topic = "math";
                        break;
                    case 3:
                        Console.WriteLine("Дякуємо за гру! До побачення.");
                        return;
                    case 4:
                        DisplayGameStats();
                        continue;
                    case 5:
                        GenerateLeaderboard();
                        continue;
                    default:
                        Console.WriteLine("Неправильний вибір, спробуйте ще раз");
                        break;
                }

                PlayQuiz(player, quiz_topic);
            }
        }


        private void PlayQuiz(Player player, string topic)
        {
            Console.WriteLine("Оберіть рівень складності:");
            Console.WriteLine("1. Легкий");
            Console.WriteLine("2. Середній");
            Console.WriteLine("3. Складний");
            Console.WriteLine("0.Повернутись");

            Console.Write("Ваш вибір: ");
            string difficultyChoice = Console.ReadLine();

            string difficulty;

            switch (difficultyChoice)
            {
                case "1":
                    difficulty = "easy";
                    break;
                case "2":
                    difficulty = "medium";
                    break;
                case "3":
                    difficulty = "hard";
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Повернення до вибору теми.");
                    return;
            }

            string FilePath = $"{topic}_{difficulty}.txt";

            questions = Quiz_Question.LoadQuestionsFromFile(FilePath);
            UsedQuestions = new List<Quiz_Question>();
            player.ZeroScore();
           

            if (questions.Count == 0)
            {
                Console.WriteLine("На жаль, немає питань за обраною темою та складністю. Повернення до вибору теми.");
                return;
            }

            Console.WriteLine("Починаємо вікторину з теми '{0}' та складності '{1}'.", topic, difficulty);



            while (true)
            {
                Quiz_Question question = new Quiz_Question();
                question = GetRandomQuestion();

                if (question == null)
                {
                    Console.WriteLine("Ви відповіли на всі питання!");
                    break;
                }

                Console.WriteLine("Питання: {0}", question.Question);

                Console.Write("Введіть вашу відповідь: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer == "0")
                {

                    Console.WriteLine("Ви відповіли правильно на {0} з {1} питань.", player.Score, questions.Count);
                    return;
                }

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

            Console.WriteLine("Ви відповіли правильно на {0} з {1} питань.", player.Score, questions.Count);
            Console.WriteLine();

            SaveGameStats(player.Name, topic, difficulty,player.Score,questions.Count);
        }

        

        private Quiz_Question GetRandomQuestion()
        {
            if (UsedQuestions.Count == questions.Count) return null;
            Quiz_Question randomQuestion = new Quiz_Question();

            randomQuestion = questions[new Random().Next(0, questions.Count)];

            while (ContainsQuestion(UsedQuestions, randomQuestion))
            {
                randomQuestion = questions[new Random().Next(0, questions.Count)];
            };

            UsedQuestions.Add(randomQuestion);

            return randomQuestion;
        }

        private bool ContainsQuestion(List<Quiz_Question> list, Quiz_Question question)
        {
            bool check;
            foreach (var item in list)
            {
                check = question.EqualQuestion(item);
                if (check) { return true; }
            }
            return false;
        }

        private void SaveGameStats(string playerName, string topic, string difficulty, int score, int totalQuestions)
        {
            string statsFilePath = "game_stats.txt";

            string stats = $"{DateTime.Now}\t{playerName}\t{topic}\t{difficulty}\t{score}\t{totalQuestions}";

            using (StreamWriter writer = new StreamWriter(statsFilePath, true))
            {
                writer.WriteLine(stats);
            }
        }

        public void DisplayGameStats()
        {
            string statsFilePath = "game_stats.txt";

            if (!File.Exists(statsFilePath))
            {
                Console.WriteLine("Немає жодної статистики гри.");
                return;
            }

            Console.WriteLine("Статистика гри:");
            Console.WriteLine("Дата\t\t\tГравець\t\tТема\t\tСкладність\tРахунок\tВсього питань");

            using (StreamReader reader = new StreamReader(statsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] stats = line.Split('\t');

                    if (stats.Length == 6)
                    {
                        string date = stats[0];
                        string playerName = stats[1];
                        string topic = stats[2];
                        string difficulty = stats[3];
                        int score = int.Parse(stats[4]);
                        int totalQuestions = int.Parse(stats[5]);

                        Console.WriteLine($"{date}\t{playerName}\t{topic}\t{difficulty}\t\t{score}/{totalQuestions}");
                    }
                }
            }
        }

        public void GenerateLeaderboard()
        {
            string statsFilePath = "game_stats.txt";

            if (!File.Exists(statsFilePath))
            {
                Console.WriteLine("Немає жодної статистики гри.");
                return;
            }

            Dictionary<string, int> leaderboard = new Dictionary<string, int>();

            using (StreamReader reader = new StreamReader(statsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] stats = line.Split('\t');

                    if (stats.Length == 6)
                    {
                        string playerName = stats[1];
                        int score = int.Parse(stats[4]);

                        if (leaderboard.ContainsKey(playerName))
                        {
                            leaderboard[playerName] += score;
                        }
                        else
                        {
                            leaderboard[playerName] = score;
                        }
                    }
                }
            }

            Console.WriteLine("Топ гравців:");
            Console.WriteLine("Гравець\t\tРахунок");

            foreach (var entry in leaderboard)
            {
                Console.WriteLine($"{entry.Key}\t\t{entry.Value}");
            }
        }
    }

}

