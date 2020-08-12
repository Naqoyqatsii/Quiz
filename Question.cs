using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
	[Serializable]
	class Question
	{
		public string SomeQuestion { get; set; }
		public string Subject { get; set; }
		public Dictionary<String, bool> Answer { get; set; }
		public List<short> GetTrue
		{
			get
			{
				if (Answer.Count == 0)
				{
					return null;
				}
				else
				{
					short i = 1;
					List<short> tmp = new List<short>();
					foreach (var item in Answer)
					{
						if (item.Value)
						{
							tmp.Add(i);
						}
						i++;
					}
					if (tmp.Count == 0) { return null; }
					else { return tmp; }
				}
			}
		}

		public Question()
		{
			Answer = new Dictionary<string, bool>();
		}
		public void Add()
		{
			bool exit = false;
			while (true)
			{
				if (exit) { break; }
				ConsoleKeyInfo key;
				Console.Clear();
				Console.WriteLine("Введите вариант ответа:");
				string tmp = Console.ReadLine();
				Console.Clear();
				bool verno = false;
				while (true)
				{
					Console.Clear();
					Console.WriteLine("Правильный ли это ответ? Нажмите (Y) или (N)");
					key = Console.ReadKey(true);
					if (key.Key == ConsoleKey.Y) { verno = true; break; }
					if (key.Key == ConsoleKey.N) { break; }
				}
				Answer.Add(tmp, verno);
				while (true)
				{
					Console.Clear();
					Console.WriteLine("Ввести еще ответ? (Y) или (N)");
					key = Console.ReadKey(true);
					if (key.Key == ConsoleKey.Y) { break; }
					if (key.Key == ConsoleKey.N) { exit = true; break; }
				}
			}
		}
		public void Edit()
		{
			ConsoleKeyInfo key;
			while (true)
			{
				if (Answer.Count == 0) 
				{
					while (true)
					{
						Console.Clear();
						Console.WriteLine("Еще нет ни одного ответа.");
						Console.WriteLine("Хотите ввести ответ? (Y) или (N)");
						key = Console.ReadKey(true);
						if (key.Key == ConsoleKey.Y) { Add(); break; }
						if (key.Key == ConsoleKey.N) { return; }
					}
				}
				else
				{
					foreach (var item in Answer)
					{
						bool reload = false;
						Console.Clear();
						Console.WriteLine($"Текущий вопрос \"{item.Key}\"");
						Console.WriteLine();
						Console.WriteLine(" - вы можете редактировать или добавить вопрос,");
						Console.WriteLine(" - для редактировани нажмите (Enter)");
						Console.WriteLine(" - для добавления вопроса (A)");
						Console.WriteLine(" - для удаления вопроса (Del)");
						Console.WriteLine(" - для перехода к следующему вопросу любую клавишу");
						Console.WriteLine(" - выход из редактирования (Esc)");
						key = Console.ReadKey(true);
						switch (key.Key)
						{
							case ConsoleKey.Enter:
								Console.Clear();
								Console.WriteLine("Введите новый вариант ответа:");
								string tmp = Console.ReadLine();
								Console.Clear();
								bool verno = false;
								while (true)
								{
									Console.Clear();
									Console.WriteLine("Правильный ли это ответ? Нажмите (Y) или (N)");
									key = Console.ReadKey(true);
									if (key.Key == ConsoleKey.Y) { verno = true; break; }
									if (key.Key == ConsoleKey.N) { break; }
								}
								Answer.Add(tmp, verno);
								Answer.Remove(item.Key);
								reload = true;
								break;
							case ConsoleKey.A:
								Console.Clear();
								Console.WriteLine("Введите вариант ответа:");
								string atmp = Console.ReadLine();
								Console.Clear();
								bool averno = false;
								while (true)
								{
									Console.Clear();
									Console.WriteLine("Правильный ли это ответ? Нажмите (Y) или (N)");
									key = Console.ReadKey(true);
									if (key.Key == ConsoleKey.Y) { averno = true; break; }
									if (key.Key == ConsoleKey.N) { break; }
								}
								Answer.Add(atmp, averno);
								reload = true;
								break;
							case ConsoleKey.Delete:
								bool del = false;
								while (true)
								{
									Console.Clear();
									Console.WriteLine("Вы точно хотите удалить этот ответ? Нажмите (Y) или (N)");
									key = Console.ReadKey(true);
									if (key.Key == ConsoleKey.Y) { del = true; break; }
									if (key.Key == ConsoleKey.N) { break; }
								}
								if (del)
								{
									if (Answer.Count == 1)
									{
										Answer.Clear();
										reload = true;
									}
									else
									{
										Answer.Remove(item.Key);
										reload = true;
									}
								}
								break;
							case ConsoleKey.Escape:
								return;
							default:
								break;
						}
						if (reload) { break; }
					}
				}
			}
		}
	}
}