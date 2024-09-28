using System;

namespace PCCharacterManager.Models
{
	public class DnD5eBackgroundData
	{
		public string Name { get; set; } = string.Empty;
		public string Desc { get; set; } = string.Empty;
		public string[] SkillProfs { get; set; } = Array.Empty<string>();
		public string[] OtherProfs { get; set; } = Array.Empty<string>();
		public string[] Languages { get; set; } = Array.Empty<string>();
	}
}
