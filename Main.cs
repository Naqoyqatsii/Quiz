using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz
{
	class Main
	{
		QuestionCollection Collection { get; set; }
		List<Question> tmpListQuestion { get; set; }
		Dictionary<string, User> Users { get; set; }
		User User { get; set; }
		public Main()
		{
			try
			{
				using (FileStream file = new FileStream("CollectionQuestion.my", FileMode.Open, FileAccess.Read))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					Collection = (QuestionCollection)formatter.Deserialize(file);
				}
			}
			catch (System.IO.FileNotFoundException)
			{
				Collection = new QuestionCollection();
			}
			try
			{
				using (FileStream file = new FileStream("Users.my", FileMode.Open, FileAccess.Read))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					Users = (Dictionary<string, User>)formatter.Deserialize(file);
				}
			}
			catch (System.IO.FileNotFoundException)
			{
				Users = new Dictionary<string, User>();
			}
			Start();
		}
		void Autorisation()
		{
			if (Users.Count == 0)
			{
				User = new User(); User.Admin = true;
				User.Login = "Администратор"; string password;
				do
				{
					Console.Clear();
					Console.WriteLine("Вы первый пользователь и автоматически - Администратор.");
					Console.WriteLine("Ваш логин по умолчанию Администратор (позднее можете сменить).");
					Console.WriteLine("Введите пароль (не менее 5 символов)!");
					password = Console.ReadLine();
				}
				while (password.Length < 5);
				User.SetPass(password);
				Console.Clear();
				Console.WriteLine("Введите день Вашего рождения:");
				User.Birthday = Console.ReadLine();
				Users.Add(User.Login, User);
				SaveUsers();
			}
			else
			{
				User user;
				ConsoleKeyInfo key;
				while (true)
				{
					Console.Clear();
					Console.WriteLine("Введите логин:");
					string login = Console.ReadLine();
					if (Users.TryGetValue(login, out user))
					{
						bool verify = false;
						Console.Clear();
						Console.WriteLine("Введите пароль:");
						for (int i = 0; i < 5; i++)
						{
							string password = Console.ReadLine();
							if (user.Verify(password)) { verify = true; break; }
							else
							{
								Console.WriteLine($"Попытка {i = 1}");
							}
						}
						if (!verify) { return; }
						User = user;
						Menu();
						return;
					}
					else
					{
						while (true)
						{
							Console.Clear();
							Console.WriteLine("Пользователь с таким логином не найден!");
							Console.WriteLine("Хотите попробовать еще раз? (R)");
							Console.WriteLine("Зарегистрировать номого пользователя (N)");
							Console.WriteLine("Выйти (Esc)");
							key = Console.ReadKey(true);
							if (key.Key == ConsoleKey.R) { break; }
							if (key.Key == ConsoleKey.Escape) { return; }
							if (key.Key == ConsoleKey.N)
							{
								string password;
								user = new User();
								Console.Clear();
								Console.WriteLine("Введите ваш логин:");
								user.Login = Console.ReadLine();
								do
								{
									Console.Clear();
									Console.WriteLine("Введите пароль (не менее 5 символов)!");
									password = Console.ReadLine();
								}
								while (password.Length < 5);
								user.SetPass(password);
								Console.Clear();
								Console.WriteLine("Введите день Вашего рождения:");
								user.Birthday = Console.ReadLine();
								Users.Add(user.Login, user);
								SaveUsers();
								User = user;
								Menu();
								return;
							}
						}
					}
				}
			}
		}
		void Menu()
		{
			ConsoleKeyInfo key;
			while (true)
			{
				Console.Clear();
				Console.WriteLine("	Добро пожаловать в программу");
				Console.WriteLine();
				Console.WriteLine("		\"ВИКТОРИНЫ\"");
				Console.WriteLine();
				Console.WriteLine("Вы можете:");
				Console.WriteLine();
				Console.WriteLine("Выбрать викторину и проверить свой уровень знаний (S)");
				Console.WriteLine();
				Console.WriteLine("Посмотреть результаты своих прошлых викторин (R)");
				Console.WriteLine();
				Console.WriteLine("Посмотреть ТОП 20 по какой либо викторине (T)");
				Console.WriteLine();
				Console.WriteLine("Отредактировать свои данные (пароль или дату рождения) (E)");
				Console.WriteLine();
				if (User.Admin)
				{
				Console.WriteLine("Вызвать утилиту редактирования (U)");
				Console.WriteLine();
				}
				Console.WriteLine("Выйти из программы (Esc)");
				key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.S) { Select(); }
				if (key.Key == ConsoleKey.R) { ; }
				if (key.Key == ConsoleKey.T) { ; }
				if (key.Key == ConsoleKey.E) { ; }
				if (key.Key == ConsoleKey.U && User.Admin) { ; }
				if (key.Key == ConsoleKey.Escape) { Console.WriteLine(); break; }
			}
		}
		public void Start()
		{
			while (true)
			{
				ConsoleKeyInfo key;
				User = null;
				Console.Clear();
				Console.WriteLine("Авторизоваться (A)");
				Console.WriteLine("Выйти (Esc)");
				key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.A) { Autorisation(); }
				if (key.Key == ConsoleKey.Escape) { return; }
			}
		}
		void SaveUsers()
		{
			using (FileStream file = new FileStream("Users.my", FileMode.Create, FileAccess.Write))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(file, Users);
			}
		}
		void SaveColl()
		{
			using (FileStream file = new FileStream("CollectionQuestion.my", FileMode.Create, FileAccess.Write))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(file, Collection);
			}
		}
		void Select()
		{
			if (Collection == null || Collection.Subject.Count == 0)
			{
				Console.Clear();
				Console.WriteLine("Увы, еще не создано ни одной викторины!");
				Console.WriteLine("Нечего даже и выбрать :(");
				Console.WriteLine();
				Console.WriteLine("нажмите любую клавишу...");
				Console.ReadKey(true);
			}
			else
			{
				while (true)
				{
					Console.Clear();
					Console.WriteLine("	СПИСОК ВИКТОРИН:");
					Console.WriteLine();
					for (int i = 0; i < Collection.Subject.Count; i++)
					{
						Console.WriteLine($"{i + 1}. {Collection.Subject[i]}");
						Console.WriteLine();
					}
					Console.WriteLine($"{Collection.Subject.Count + 1}. Общая викторина");
					Console.WriteLine();
					Console.WriteLine("0. Возврат в главное меню");
					Console.WriteLine();
					Console.WriteLine("Введите номер выбранной викторины:");
					var tmp = Console.ReadLine();
					if (int.TryParse(tmp, out int number))
					{
						if (number == 0) { break; }
						if (number > 0 && number < (Collection.Subject.Count + 2))
						{
							{
								if (number == (Collection.Subject.Count + 1)) { UseCollection("rnd"); }
								else { UseCollection(Collection.Subject[number]); }
							}
						}
						else
						{
							Console.Clear();
							Console.WriteLine("Номер введен некорректно, введите корректный номер");
							Console.WriteLine();
							Console.WriteLine("нажмите любую клавишу...");
							Console.ReadKey(true);
						}
					}
					else
					{
						Console.Clear();
						Console.WriteLine("Номер введен некорректно, введите корректный номер");
						Console.WriteLine();
						Console.WriteLine("нажмите любую клавишу...");
						Console.ReadKey(true);
					}
				}
			}
		}
		void UseCollection(string theme)
		{
			if (theme == "rnd") { tmpListQuestion = Collection.RndQuiz(); }
			else { tmpListQuestion = Collection.ThemeOuiz(theme); }

		}
	}
}