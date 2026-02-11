// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Repository.MapIdCoordinates
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

public class MapIdCoordinates
{
	[CompilerGenerated]
	private int _003CMapId_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CWorldX_003Ek__BackingField;

	[JsonProperty("mapId")]
	public int MapId
	{
		[CompilerGenerated]
		get
		{
			ushort num = 0;
			return _003CMapId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CMapId_003Ek__BackingField = value;
		}
	}

	[JsonProperty("worldX")]
	public int WorldX
	{
		[CompilerGenerated]
		get
		{
			return _003CWorldX_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = 0;
			_003CWorldX_003Ek__BackingField = value;
		}
	}

	[JsonProperty("worldY")]
	public int WorldY { get; set; }

	public MapIdCoordinates(int mapId, int worldX, int worldY)
	{
		int num = 0;
		if (0 + (504610723 >>> (num >>> 14 >>> num)) != 0)
		{
			goto IL_001b;
		}
		goto IL_005b;
		IL_001b:
		while (true)
		{
			switch ((uint)num % 3u)
			{
			default:
				_671BC22C.BF9F3D1F.FE8E0C9E[2099 + -646585661 * (num << num - 1975445663)](this);
				MapId = mapId;
				break;
			case 1u:
				WorldY = worldY;
				if (~(((uint)num % (uint)(-num) > (uint)num) ? 1u : 0u) != 0)
				{
					goto IL_0080;
				}
				break;
			case 2u:
				num = -780684380 ^ (num - 1352307102);
				return;
			}
			break;
			IL_0080:
			num = 2132991487 + (~num >>> 1) % -1483650897;
		}
		goto IL_005b;
		IL_005b:
		WorldX = worldX;
		num += -45;
		goto IL_001b;
	}

	public MapIdCoordinates()
	{
		_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
	}
}
