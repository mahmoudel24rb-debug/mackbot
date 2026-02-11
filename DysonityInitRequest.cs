// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Dysonity.Logic.DysonityInitRequest
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dysonity.Config;
using Dysonity.JsonClass;
using Newtonsoft.Json;

public class DysonityInitRequest
{
	[CompilerGenerated]
	private Dictionary<string, _1CB1C715> _003CPossibleEffectsMap_003Ek__BackingField;

	[CompilerGenerated]
	private OptionsJson _003COptions_003Ek__BackingField;

	[CompilerGenerated]
	private Dictionary<string, EffectJson> _003CRuneByEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<string> _003CMacAddresses_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CIsPc_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CCharacterId_003Ek__BackingField;

	[JsonProperty("wantedJet")]
	public Dictionary<string, int> WantedJet { get; set; }

	[JsonProperty("possibleEffectsMap")]
	public Dictionary<string, _1CB1C715> PossibleEffectsMap
	{
		[CompilerGenerated]
		get
		{
			return _003CPossibleEffectsMap_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 50;
			_003CPossibleEffectsMap_003Ek__BackingField = value;
		}
	}

	[JsonProperty("options")]
	public OptionsJson Options
	{
		[CompilerGenerated]
		get
		{
			return _003COptions_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 4294967272u;
			if (num - (0 - num % num) << ((int)num >> 16 << (int)num) != 0)
			{
				_003COptions_003Ek__BackingField = value;
			}
		}
	}

	[JsonProperty("runeByEffects")]
	public Dictionary<string, EffectJson> RuneByEffects
	{
		[CompilerGenerated]
		get
		{
			return _003CRuneByEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 2930987008u;
			_003CRuneByEffects_003Ek__BackingField = value;
		}
	}

	[JsonProperty("macAddresses")]
	public List<string> MacAddresses
	{
		[CompilerGenerated]
		get
		{
			ushort num = 0;
			return _003CMacAddresses_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CMacAddresses_003Ek__BackingField = value;
		}
	}

	[JsonProperty("isPc")]
	public bool IsPc
	{
		[CompilerGenerated]
		get
		{
			ushort num = 17191;
			return _003CIsPc_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 294931682u;
			if (num * num != (uint)(((int)num >> 24) * 1990697236 - (int)num))
			{
				do
				{
					_003CIsPc_003Ek__BackingField = value;
				}
				while (((((int)num > -1221660414) ? 1u : 0u) & 0x47AEu) != 0);
			}
		}
	}

	[JsonProperty("characterId")]
	public int CharacterId
	{
		[CompilerGenerated]
		get
		{
			return _003CCharacterId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 177;
			_003CCharacterId_003Ek__BackingField = value;
		}
	}

	public DysonityInitRequest()
	{
		short num = 1;
		do
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[(int)num + ((4164554938u > (uint)num) ? 1 : 0) + 2097](this);
		}
		while (((2302018564u % ((uint)num / (uint)num)) ^ ((((uint)num > 698201484u) ? 1u : 0u) << (num & -1743547334))) != 0);
	}
}
