// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Dysonity.Logic.DysonityCaseResponse
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

public class DysonityCaseResponse
{
	[CompilerGenerated]
	private int _003CRuneId_003Ek__BackingField;

	[CompilerGenerated]
	private List<C61D389B>? _003CRunesToDestroy_003Ek__BackingField;

	[JsonProperty("runeId")]
	public int RuneId
	{
		[CompilerGenerated]
		get
		{
			return _003CRuneId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = -735653331;
			if ((uint)(1656526598 / num) / 891342654u != 0)
			{
				_003CRuneId_003Ek__BackingField = value;
			}
		}
	}

	[JsonProperty("case")]
	public string Case { get; set; }

	[JsonProperty("caracId")]
	public int? CaracId { get; set; }

	[JsonProperty("runesToDestroy")]
	public List<C61D389B>? RunesToDestroy
	{
		[CompilerGenerated]
		get
		{
			uint num = 0u;
			return _003CRunesToDestroy_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			sbyte b = -64;
			_003CRunesToDestroy_003Ek__BackingField = value;
		}
	}

	public DysonityCaseResponse()
	{
		_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
	}
}
