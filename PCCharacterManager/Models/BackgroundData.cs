using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class BackgroundData
	{
		public string Name { get; set; } = string.Empty;
		public string Desc { get; set; } = string.Empty;
		public string[] SkillProfs { get; set; } = Array.Empty<string>();
		public string[] OtherProfs { get; set; } = Array.Empty<string>();
		public string[] Languages { get; set; } = Array.Empty<string>();
	}
}
