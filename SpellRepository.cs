// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Repository.SpellRepository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DofusLibrary.Common.Repository;
using Newtonsoft.Json;

public static class SpellRepository
{
	[CompilerGenerated]
	private static Dictionary<int, DetailedSpell> _003CRepository_003Ek__BackingField;

	public static Dictionary<int, DetailedSpell> Repository
	{
		[CompilerGenerated]
		get
		{
			return _003CRepository_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = -8142;
			do
			{
				_003CRepository_003Ek__BackingField = value;
			}
			while ((num & 0x2A) == 0);
		}
	}

	static SpellRepository()
	{
		try
		{
			Repository = JsonConvert.DeserializeObject<Dictionary<string, DetailedSpell>>(ABBEB404._69B5C318("spellParsed.json")).ToDictionary((KeyValuePair<string, DetailedSpell> kvp) => _671BC22C.BF9F3D1F.FE8E0C9E[672](kvp.Key), delegate(KeyValuePair<string, DetailedSpell> kvp)
			{
				sbyte b = 0;
				return kvp.Value;
			});
		}
		catch (Exception)
		{
		}
	}
}
