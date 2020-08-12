using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
	[Serializable]
	class QuestionCollection
	{
		List<Question> Questions { get; set; }
		public List<string> Subject { get; set; }
		Random rnd = new Random();
		public QuestionCollection()
		{
			Questions = new List<Question>();
			Subject = new List<string>();
		}
		public List<Question> RndQuiz()
		{
			List<Question> tmp = new List<Question>();
			List<Question> copy = new List<Question>();
			if (Questions.Count > 20)
			{
				//List<Question> copy = new List<Question>(Questions); // попробовать вот так еще
				foreach (var item in Questions)
				{
					copy.Add(item);
				}
				for (int i = 0; i < 20; i++)
				{
					int j = rnd.Next(0, copy.Count);
					tmp.Add(copy[j]);
					copy.RemoveAt(j);
				}
			}
			else
			{
				tmp = null;
			}
			return tmp;
		}
		public List<Question> ThemeOuiz(string subject)
		{
			if (Questions.Count >= 20)
			{
				var query = from Question qw in Questions
							where qw.Subject == subject
							select qw;
				return (List<Question>)query;
			}
			else return null;
		}
	}
}