// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.JsonClasses.RootMapData
using System.Runtime.CompilerServices;
using DofusLibrary.Common.JsonClasses;
using Newtonsoft.Json;

public class RootMapData
{
	[CompilerGenerated]
	private MapData _003CMapData_003Ek__BackingField;

	[JsonProperty("mapData")]
	public MapData MapData
	{
		[CompilerGenerated]
		get
		{
			return _003CMapData_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 439132322u;
			_003CMapData_003Ek__BackingField = value;
		}
	}

	public RootMapData()
	{
		_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
	}
}
