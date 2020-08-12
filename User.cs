using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
	[Serializable]
	class User
	{
		public string Login { get; set; }
		public string Password { get; private set; }
		public string Birthday { get; set; }
		public bool Admin = false;
		public Dictionary<string, short> Results = new Dictionary<string, short>();
		public void SetPass(string password)
		{
			using (var hash = SHA1.Create())
			{
				this.Password = string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
			}
		}
		public bool Verify(string password)
		{
			string Password;
			using (var hash = SHA1.Create())
			{
				Password = string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
			}
			if(this.Password == Password) {	return true; }
			else { return false; }
		}
	}
}