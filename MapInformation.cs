// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Map.MapInformation
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DofusLibrary.Common.JsonClasses;
using DofusLibrary.Common.Map;
using DofusLibrary.Common.Repository;
using Google.Protobuf.Collections;
using JitsuriProto;
using Newtonsoft.Json;

public class MapInformation
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public JitsuriStatedElementUpdatedEvent evt;

		public _003C_003Ec__DisplayClass14_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CUpdateStatedElement_003Eb__0(JitsuriStatedElement e)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[1307](e) == _671BC22C.BF9F3D1F.FE8E0C9E[1307](_671BC22C.BF9F3D1F.FE8E0C9E[544](evt));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass24_0
	{
		public int targetSkillId;

		public Func<JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill, bool> _003C_003E9__1;

		public _003C_003Ec__DisplayClass24_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetWorkshopInteractiveElement_003Eb__0(JitsuriInteractiveElement e)
		{
			if (_671BC22C.BF9F3D1F.FE8E0C9E[853](e))
			{
				return _671BC22C.BF9F3D1F.FE8E0C9E[1251](e).Any((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == targetSkillId);
			}
			return false;
		}

		internal bool _003CGetWorkshopInteractiveElement_003Eb__1(JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill)
		{
			return skill.SkillId == targetSkillId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass27_0
	{
		public long? elementId;

		public _003C_003Ec__DisplayClass27_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetChestInteractiveElementFromCell_003Eb__0(JitsuriInteractiveElement e)
		{
			if (_671BC22C.BF9F3D1F.FE8E0C9E[855](e) == elementId && _671BC22C.BF9F3D1F.FE8E0C9E[853](e))
			{
				return _671BC22C.BF9F3D1F.FE8E0C9E[1251](e).Any((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == 104);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass28_0
	{
		public int doorElementId;

		public Func<JitsuriInteractiveElement, bool> _003C_003E9__0;

		public _003C_003Ec__DisplayClass28_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetHouseDoorElementIdAndInstanceIdByNickname_003Eb__0(JitsuriInteractiveElement e)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[855](e) == doorElementId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass29_0
	{
		public long actorId;

		public _003C_003Ec__DisplayClass29_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CIsActorIdOnMap_003Eb__0(JitsuriActorPositionInformation e)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[444](e) == actorId;
		}
	}

	private const int GRID_HEIGHT = 40;

	private const int GRID_WIDTH = 14;

	private const int ZAAP_INTERACTIVE_ID = 16;

	private const int TREASURE_INTERACTIVE_ID = -1;

	private const int TREASURE_ELEMENT_ID = 486028;

	[CompilerGenerated]
	private List<JitsuriHouse> _003CHouses_003Ek__BackingField;

	internal const int PHOENIX_SKILL_ID = 211;

	internal const int CHEST_SKILL_ID = 104;

	internal const int HDV = 355;

	internal const int CHEST_GUILD_SKILL_ID = 184;

	internal const int CHEST_GUILD_SKILL_TID = 388;

	[CompilerGenerated]
	private readonly List<JitsuriInteractiveElement> _003CInteractiveElements_003Ek__BackingField;

	[CompilerGenerated]
	private _99A5FF07 _003CMapRights_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CWorldMapX_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CWorldMapY_003Ek__BackingField;

	[CompilerGenerated]
	private List<_7A17BD0F> _003CWorldTransitions_003Ek__BackingField;

	public List<JitsuriHouse> Houses
	{
		[CompilerGenerated]
		get
		{
			sbyte b = 1;
			return _003CHouses_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CHouses_003Ek__BackingField = value;
		}
	}

	[JsonProperty("cells")]
	public List<Cell> Cells { get; set; }

	[JsonProperty("interactiveElements")]
	public List<JitsuriInteractiveElement> InteractiveElements
	{
		[CompilerGenerated]
		get
		{
			short num = -20550;
			return _003CInteractiveElements_003Ek__BackingField;
		}
	}

	[JsonProperty("actors")]
	public List<JitsuriActorPositionInformation> Actors { get; }

	[JsonProperty("statedElements")]
	public List<JitsuriStatedElement> StatedElements { get; }

	[JsonProperty("mapId")]
	public long MapId { get; set; }

	[JsonProperty("walkableCells")]
	public List<Cell> WalkableCells { get; set; }

	[JsonIgnore]
	public _99A5FF07 MapRights
	{
		[CompilerGenerated]
		get
		{
			return _003CMapRights_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 3148873663u;
			_003CMapRights_003Ek__BackingField = value;
		}
	}

	[JsonProperty("worldMapX")]
	public int WorldMapX
	{
		[CompilerGenerated]
		get
		{
			byte b = 120;
			return _003CWorldMapX_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 2338526499u;
			_003CWorldMapX_003Ek__BackingField = value;
		}
	}

	[JsonProperty("worldMapY")]
	public int WorldMapY
	{
		[CompilerGenerated]
		get
		{
			short num = 148;
			return _003CWorldMapY_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = -2;
			_003CWorldMapY_003Ek__BackingField = value;
		}
	}

	[JsonProperty("worldTransitions")]
	public List<_7A17BD0F> WorldTransitions
	{
		[CompilerGenerated]
		get
		{
			int num = 0;
			return _003CWorldTransitions_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 115261357;
			if (((-712356590 / num % num % 404217402) ^ ((int)(((num == num) ? 1u : 0u) << num + num) + -314587463 / num)) < ((num >> 12) ^ 1))
			{
				do
				{
					_003CWorldTransitions_003Ek__BackingField = value;
				}
				while (22 + (0x66318FBB & num) >= (int)((uint)num % (uint)(~num)));
			}
		}
	}

	public MapInformation()
	{
		short num2 = default(short);
		uint num3 = default(uint);
		while (true)
		{
			IL_0000:
			Houses = new List<JitsuriHouse>();
			int num = 1868227020;
			if ((0x5CAACB94 | (((num | num) + num) * -1438946921)) == 0)
			{
				break;
			}
			num = ((num / (int)((uint)num % 3316642083u) >> num) & 0xB2FE68F) ^ -939299813;
			while (true)
			{
				switch ((uint)num % 7u)
				{
				default:
					num = (((uint)num < (uint)num) ? 1 : 0) * -1 - -1868227020;
					Cells = new List<Cell>();
					InteractiveElements = new List<JitsuriInteractiveElement>();
					Actors = new List<JitsuriActorPositionInformation>();
					num <<= num;
					if ((uint)num >= (uint)(num / ~(num << 24) << ((num + -1885017665) ^ -635375425) >> (-1417266388 >> num)))
					{
						num = (int)((uint)(~(num - 121067937) * ((num == (int)((uint)num % 3492708386u)) ? 1 : 0)) / 1134565267u + 43115576);
						continue;
					}
					goto IL_01ac;
				case 1u:
					num = (int)(~((1950167570u < (uint)(226619420 * num)) ? 1u : 0u)) + -1373847550;
					goto IL_0102;
				case 2u:
					num = (num2 >>> num2 * (num2 >> (int)num2) + ((0x6F391594 & num) | -1170273512)) - 1373847567;
					MapRights = new _99A5FF07();
					num3 = (uint)(119182 << -2045508169 * ((ushort)num2 << 7));
					num = -(1430098953 * num * num) - -1501852448;
					continue;
				case 3u:
					num = (int)(0 - num3) + -1373728370;
					goto IL_01ac;
				case 4u:
					WorldMapY = ((num >> (int)(4272637954u / ~num3)) | ~(1093311259 >>> (int)num3)) - -1092754093;
					WorldTransitions = new List<_7A17BD0F>();
					if ((num3 & 0x78BC97A7) == 0)
					{
						num = ~(1453136293 * num2) % ((1267651217 % ~(-1732470354 * (int)num3)) | num2) - -1603964847;
						continue;
					}
					goto IL_0102;
				case 5u:
				{
					num = (((uint)(-(num2 + -2011512660) + ((int)num3 * -40749173 >>> 1)) < (uint)((998984102 >>> num << 23) & num)) ? 1 : 0) - 1373847552;
					_671BC22C.BF9F3D1F.FE8E0C9E[0x833 ^ (0 - ((1470891913 > num3) ? 1u : 0u) / 1100677249u >> ((num2 == 1896523570) ? 1 : 0))](this);
					ushort num4 = (ushort)(num2 | 0x61821FB5);
					num = num * (((uint)num > (uint)(-num4 & num)) ? 1 : 0) - 780224884;
					continue;
				}
				case 6u:
					break;
					IL_01ac:
					do
					{
						WorldMapX = (num2 >>> 21) ^ -1938;
						num3 = (uint)((int)(num3 & (uint)num2) / (int)((uint)((sbyte)num3 >> (int)num2) % (uint)(255726757 + (-878697051 + num))));
					}
					while (452884593 - num2 <= num2);
					continue;
				}
				break;
				IL_0102:
				StatedElements = new List<JitsuriStatedElement>();
				WalkableCells = new List<Cell>();
				num2 = (short)(384684869 * num);
				if (((sbyte)num >>> (num2 >> num)) + 1711528352 >>> (int)num2 == 0)
				{
					goto IL_0000;
				}
				num = ~(~num) ^ -670378723;
			}
			num = (int)(num3 - 0 - 1373847552);
			break;
		}
		InitializeDefaultCells();
	}

	public MapInformation(JitsuriMapComplementaryInformationEvent mapComplementaryInformation)
	{
		Houses = new List<JitsuriHouse>();
		Cells = new List<Cell>();
		InteractiveElements = new List<JitsuriInteractiveElement>();
		Actors = new List<JitsuriActorPositionInformation>();
		StatedElements = new List<JitsuriStatedElement>();
		WalkableCells = new List<Cell>();
		MapRights = new _99A5FF07();
		WorldMapX = -111;
		WorldMapY = -111;
		WorldTransitions = new List<_7A17BD0F>();
		_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		MapId = _671BC22C.BF9F3D1F.FE8E0C9E[240](mapComplementaryInformation);
		MapIdCoordinates mapIdCoordinates = _82210236.AB91771D((int)MapId, B31AE737: true);
		MapRights = D3B9EF8E._3197FD0C[(int)MapId];
		WorldMapX = mapIdCoordinates.WorldX;
		WorldMapY = mapIdCoordinates.WorldY;
		LoadCellsFromArchiveOrDefault();
		WorldTransitions = WorldGraphManager.GetAvailableTransitions((int)MapId);
		InteractiveElements = _671BC22C.BF9F3D1F.FE8E0C9E[1567](mapComplementaryInformation).ToList();
		StatedElements = _671BC22C.BF9F3D1F.FE8E0C9E[822](mapComplementaryInformation).ToList();
		Actors = _671BC22C.BF9F3D1F.FE8E0C9E[736](mapComplementaryInformation).ToList();
		Houses = _671BC22C.BF9F3D1F.FE8E0C9E[1339](mapComplementaryInformation).ToList();
		_0C91B907._649618BD(_671BC22C.BF9F3D1F.FE8E0C9E[1567](mapComplementaryInformation).ToList(), _671BC22C.BF9F3D1F.FE8E0C9E[822](mapComplementaryInformation).ToList());
		E53B74BF._772C3CB7("MapInformation: Initialisation: Successful", ConsoleColor.Yellow);
	}

	public MapInformation(JitsuriMapCurrentEvent mapcurrentEvent)
	{
		Houses = new List<JitsuriHouse>();
		Cells = new List<Cell>();
		InteractiveElements = new List<JitsuriInteractiveElement>();
		Actors = new List<JitsuriActorPositionInformation>();
		StatedElements = new List<JitsuriStatedElement>();
		WalkableCells = new List<Cell>();
		MapRights = new _99A5FF07();
		WorldMapX = -111;
		WorldMapY = -111;
		WorldTransitions = new List<_7A17BD0F>();
		_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		MapId = _671BC22C.BF9F3D1F.FE8E0C9E[1294](mapcurrentEvent);
		MapIdCoordinates mapIdCoordinates = _82210236.AB91771D((int)MapId, B31AE737: true);
		MapRights = D3B9EF8E._3197FD0C[(int)MapId];
		WorldMapX = mapIdCoordinates.WorldX;
		WorldMapY = mapIdCoordinates.WorldY;
		WorldTransitions = WorldGraphManager.GetAvailableTransitions((int)MapId);
		InteractiveElements = new List<JitsuriInteractiveElement>();
		StatedElements = new List<JitsuriStatedElement>();
		Actors = new List<JitsuriActorPositionInformation>();
		Houses = new List<JitsuriHouse>();
		LoadCellsFromArchiveOrDefault();
	}

	private void LoadCellsFromArchiveOrDefault()
	{
		try
		{
			string b68EAB8C = _671BC22C.BF9F3D1F.FE8E0C9E[280](_671BC22C.BF9F3D1F.FE8E0C9E[1277](_671BC22C.BF9F3D1F.FE8E0C9E[657]()), "data", "maps.rar");
			RootMapData result = F11F4F82._38AC0EBC(MapId, b68EAB8C).GetAwaiter().GetResult();
			if (result?.MapData?.CellsData?.Array != null)
			{
				Cells = result.MapData.CellsData.Array;
				WalkableCells = Cells.Where((Cell cell) => cell.Mov != 0).ToList();
				{
					foreach (Cell cell in Cells)
					{
						(int x, int y) tuple = _849BFB2F.A094930A(cell.CellNumber);
						int item = tuple.x;
						int item2 = tuple.y;
						cell.X = item;
						cell.Y = item2;
					}
					return;
				}
			}
			InitializeDefaultCells();
		}
		catch (Exception)
		{
			InitializeDefaultCells();
		}
	}

	private void InitializeDefaultCells()
	{
		E53B74BF._772C3CB7("MapInformation: Initialisation: DefaultCells.", ConsoleColor.Yellow);
		ushort num = 30981;
		int num2 = default(int);
		while (true)
		{
			List<Cell> list;
			switch ((uint)num % 3u)
			{
			default:
				if (Cells == null)
				{
					goto IL_0031;
				}
				goto IL_003e;
			case 1u:
				num = (ushort)(-(-1448812877 / ~(num - num)) - -1448843858);
				if (num2 >= (int)(((((uint)num < (uint)num) ? 1u : 0u) & 0xDFADFFBDu) + 560))
				{
					num = ((num > ((num ^ num) | 0x490C8D88)) ? ((ushort)1) : ((ushort)0));
					if (num * 924649489 >= 2057807122 % ~(-1249911708 % ~(1754079379 % ~(num / 1845634853))))
					{
						num = (ushort)((num ^ (1815815837 >> num / ~num)) - (101189760 + (-292526941 << num / 1923334017)) - 2007115726);
						break;
					}
					goto IL_0031;
				}
				goto IL_0063;
			case 2u:
				{
					num = (ushort)((uint)(~(num + num >>> (-610972799 << (int)num))) / (uint)num - 115169);
					WalkableCells = Cells.Where((Cell c) => c.Mov != 0).ToList();
					return;
				}
				IL_003e:
				Cells.Clear();
				num2 = ((-63108035 | num | (num << 3)) * num) ^ 0x2BF1C731;
				goto IL_00ae;
				IL_0031:
				list = (Cells = new List<Cell>());
				goto IL_003e;
				IL_00ae:
				if (-num != 0)
				{
					num = (ushort)(653477476 + 3641505199u % ~((uint)num / uint.MaxValue));
					break;
				}
				goto IL_0063;
				IL_0063:
				Cells.Add(new Cell
				{
					CellNumber = num2,
					Visible = 1,
					Mov = 0,
					Los = 1,
					NonWalkableDuringFight = 0,
					NonWalkableDuringRP = 0,
					LinkedZone = 0
				});
				num2++;
				num = 30981;
				goto IL_00ae;
			}
		}
	}

	internal async Task<bool> UpdateStatedElement(JitsuriStatedElementUpdatedEvent evt)
	{
		_003C_003Ec__DisplayClass14_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass14_0();
		CS_0024_003C_003E8__locals3.evt = evt;
		ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
		while (StatedElements == null)
		{
			ConfiguredTaskAwaitable _799EEF = _671BC22C.BF9F3D1F.FE8E0C9E[353](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000), _79058F01: false);
			ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter = _671BC22C.BF9F3D1F.FE8E0C9E[144](ref _799EEF);
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
			}
			configuredTaskAwaiter.GetResult();
		}
		int num = StatedElements.FindIndex((JitsuriStatedElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](e) == _671BC22C.BF9F3D1F.FE8E0C9E[1307](_671BC22C.BF9F3D1F.FE8E0C9E[544](CS_0024_003C_003E8__locals3.evt)));
		if (num >= 0)
		{
			StatedElements[num] = _671BC22C.BF9F3D1F.FE8E0C9E[544](CS_0024_003C_003E8__locals3.evt);
		}
		return true;
	}

	internal JitsuriInteractiveElement? GetZaapInteractiveElement()
	{
		return InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[805](e) == 16 && _671BC22C.BF9F3D1F.FE8E0C9E[853](e));
	}

	internal JitsuriInteractiveElement? GetTreasureInteractiveElement()
	{
		return InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[855](e) == -1);
	}

	internal JitsuriStatedElement? GetTreasureStatedElement()
	{
		sbyte b = -82;
		return StatedElements.FirstOrDefault((JitsuriStatedElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](e) == 486028);
	}

	internal JitsuriInteractiveElement? GetPhoenixInteractiveElement()
	{
		ushort num = 11199;
		return InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[1251](e).Any((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == 211) && _671BC22C.BF9F3D1F.FE8E0C9E[853](e));
	}

	internal JitsuriInteractiveElement? GetWorkshopInteractiveElement(List<int> playerSkills, bool specialCraft = false)
	{
		_003C_003Ec__DisplayClass24_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass24_0();
		if (playerSkills == null || playerSkills.Count == 0)
		{
			return null;
		}
		int index = ((specialCraft && playerSkills.Count > 1) ? 1 : 0);
		CS_0024_003C_003E8__locals2.targetSkillId = playerSkills[index];
		return InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[853](e) && _671BC22C.BF9F3D1F.FE8E0C9E[1251](e).Any((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == CS_0024_003C_003E8__locals2.targetSkillId));
	}

	internal JitsuriInteractiveElement? GetGuildChestInteractiveElement()
	{
		return InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[805](e) == 388 && _671BC22C.BF9F3D1F.FE8E0C9E[853](e) && _671BC22C.BF9F3D1F.FE8E0C9E[1251](e).Any((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == 184));
	}

	internal JitsuriInteractiveElement? GetHdvInteractiveElement()
	{
		return InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[1251](e).Any((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == 355) && _671BC22C.BF9F3D1F.FE8E0C9E[853](e));
	}

	internal (JitsuriInteractiveElement? element, int? skillInstanceUid) GetChestInteractiveElementFromCell(int cellId)
	{
		JitsuriInteractiveElement jitsuriInteractiveElement = default(JitsuriInteractiveElement);
		while (true)
		{
			_003C_003Ec__DisplayClass27_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass27_0();
			short num = 1;
			num = (short)(((num << (int)num) - -301922888 >> num - 408751816) - -9484);
			do
			{
				IL_002c:
				switch ((uint)num % 3u)
				{
				case 1u:
					jitsuriInteractiveElement = InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[855](e) == CS_0024_003C_003E8__locals2.elementId && _671BC22C.BF9F3D1F.FE8E0C9E[853](e) && _671BC22C.BF9F3D1F.FE8E0C9E[1251](e).Any((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == 104));
					do
					{
						if (jitsuriInteractiveElement == null)
						{
							return (element: null, skillInstanceUid: null);
						}
					}
					while (num >> 1 * num != 0);
					num = (short)(((uint)num / ((num + -1615007996 + num < 923318829 + ~num) ? 1u : 0u)) ^ 0x259C);
					goto IL_002c;
				case 2u:
					num = (short)(-((num ^ 0x6D2B6E37) - ((num >> (int)num) & 0x1A05C6AA)) + 1831553963);
					return (element: jitsuriInteractiveElement, skillInstanceUid: _671BC22C.BF9F3D1F.FE8E0C9E[0x9015EA4 ^ num ^ 0x9015A46](jitsuriInteractiveElement).FirstOrDefault((JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill skill) => skill.SkillId == 104)?.SkillInstanceUid);
				}
				num = (short)((475806397 << num / -601132637) + -475806396);
				CS_0024_003C_003E8__locals2.elementId = CB96861A._5CACA69B((int)MapId, cellId);
			}
			while (-612291812 % num == 0);
		}
	}

	internal (int? DoorElementId, int? InstanceId, int? CellId, int? SkillInstanceUid, JitsuriInteractiveElement? Element) GetHouseDoorElementIdAndInstanceIdByNickname(string accountNickname)
	{
		(int?, int?, int?, int?, JitsuriInteractiveElement) result;
		uint num2;
		sbyte b = default(sbyte);
		using (List<JitsuriHouse>.Enumerator enumerator = Houses.GetEnumerator())
		{
			JitsuriHouseInstance current2 = default(JitsuriHouseInstance);
			JitsuriInteractiveElement jitsuriInteractiveElement = default(JitsuriInteractiveElement);
			int? num4 = default(int?);
			sbyte b2 = default(sbyte);
			int? item = default(int?);
			int? item2 = default(int?);
			while (true)
			{
				IL_064b:
				short num = 28474;
				_003C_003Ec__DisplayClass28_0 CS_0024_003C_003E8__locals4;
				JitsuriHouse.Types.JitsuriHouseOnMap jitsuriHouseOnMap;
				if (enumerator.MoveNext())
				{
					object obj = _671BC22C.BF9F3D1F.FE8E0C9E[32];
					JitsuriHouse current = enumerator.Current;
					CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass28_0();
					jitsuriHouseOnMap = obj(current);
					num2 = 1u;
					if ((uint)((int)((num2 << 15) * num2) / (int)(~(num2 - num2))) > num2)
					{
						goto IL_004a;
					}
					goto IL_005a;
				}
				if (num != 0)
				{
					break;
				}
				continue;
				IL_005a:
				CS_0024_003C_003E8__locals4.doorElementId = jitsuriHouseOnMap.DoorsOnMap.First();
				if ((byte)num2 == 0)
				{
					goto IL_004a;
				}
				using (IEnumerator<JitsuriHouseInstance> enumerator2 = jitsuriHouseOnMap.HousesInformation.GetEnumerator())
				{
					if ((0x4E37558F & num2) == 0)
					{
						b = (sbyte)((num2 >> 17 << (int)num2) + num2);
						num2 = ~num2 - 3453814634u;
						goto IL_00a8;
					}
					goto IL_05b6;
					IL_00fe:
					bool num3 = _671BC22C.BF9F3D1F.FE8E0C9E[2051](current2);
					num2 = 1u;
					if (num3)
					{
						num = (short)((int)num2 >> (int)num2 >>> (int)((num2 ^ 0x2C261D25) + 69762337) / (int)(num2 * num2));
						goto IL_00a8;
					}
					goto IL_05b6;
					IL_00a8:
					while (true)
					{
						int? num5;
						switch (num2 % 12)
						{
						default:
							num2 = 0x699CEE70 ^ (930739677 + num2);
							goto IL_0611;
						case 1u:
							break;
						case 2u:
							goto IL_01ba;
						case 3u:
							goto IL_025c;
						case 4u:
							goto IL_02ea;
						case 5u:
							num2 = (uint)((ushort)num2 - 58409);
							goto IL_034d;
						case 6u:
						{
							num2 = (uint)(-1891147626 ^ ((int)(((b > num) ? 1u : 0u) / (uint)(~num)) - -(b * 1496584486 - 1839114548)));
							RepeatedField<JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill> repeatedField = _671BC22C.BF9F3D1F.FE8E0C9E[0x4E4 ^ (num2 & 0x3FA26CA2)](jitsuriInteractiveElement);
							if (repeatedField == null)
							{
								num4 = null;
								num2 = (uint)((int)(((num2 < (uint)(num * -1816613325)) ? 1u : 0u) >> 13) % (int)b);
								if ((uint)(byte)(num >>> 11) <= (uint)(~b))
								{
									num2 = ~(0 - 2592639759u / ~num2 + 850617249) + 460059829;
									continue;
								}
								goto IL_0520;
							}
							JitsuriInteractiveElement.Types.JitsuriInteractiveElementSkill? jitsuriInteractiveElementSkill = repeatedField.FirstOrDefault();
							if (jitsuriInteractiveElementSkill == null)
							{
								b2 = (sbyte)((int)num2 / 19087);
								if ((int)((uint)((int)((uint)b % 3893564950u) % (int)(~num2)) % 4111431231u) / ~b2 == 0)
								{
									num2 = (uint)(-981075472 ^ (b + 699770013));
									continue;
								}
								goto IL_034d;
							}
							num5 = jitsuriInteractiveElementSkill.SkillInstanceUid;
							goto IL_0531;
						}
						case 7u:
							num2 = (uint)(((b % -225549552 / (int)((uint)num ^ (2936153763u % (uint)b))) | b) ^ -89);
							goto IL_043a;
						case 8u:
							num2 = (uint)((int)((uint)b2 % 1176032674u) >> (605762563 >>> num / (b - 615129014)));
							num4 = null;
							b2 = (sbyte)(-818247630 | (int)((uint)num % (uint)b));
							if ((b2 >> b2 % ~num) - (-31368795 + b2 - ((b2 == 998349746) ? 1 : 0)) >> (int)((uint)num / (uint)(ushort)b2) != 0)
							{
								num2 = (uint)(-(((num > -1157566430) ? 1 : 0) + 299966699) - -822367641);
								continue;
							}
							goto IL_0578;
						case 9u:
							num2 = (uint)(num + 587807005 + -(b - -1526649938) - -938842844);
							goto IL_0520;
						case 10u:
							num2 = 1 ^ (((0 - num2) & num2) % (uint)(~(num >>> (int)(num2 * num2))) + ~((1933427632 < num2) ? 1u : 0u));
							goto IL_0578;
						case 11u:
							{
								num2 = (((uint)(-(ushort)(num2 >> (int)num2)) < (0x3BA1B086 | num2 | 0xF3B4E285u)) ? 1u : 0u) - uint.MaxValue;
								goto IL_0611;
							}
							IL_043a:
							num5 = num4;
							goto IL_0531;
							IL_0578:
							result = (CS_0024_003C_003E8__locals4.doorElementId, _671BC22C.BF9F3D1F.FE8E0C9E[1424810974 + (-1424810438 - num)](current2), item, item2, jitsuriInteractiveElement);
							goto end_IL_00a8;
							IL_0531:
							item2 = num5;
							if (1404561458 % (int)(~num2) == 0)
							{
								num2 = (uint)(((int)num2 * -955694331) ^ 0x304F61E);
								continue;
							}
							goto IL_043a;
							IL_0520:
							num5 = num4;
							goto IL_0531;
							IL_034d:
							num5 = num4;
							goto IL_0531;
							IL_0611:
							if (!_671BC22C.BF9F3D1F.FE8E0C9E[(int)((0 - num2) % (num2 | (uint)(-611914611 + (int)(0 - num2)))) + -611913229](enumerator2))
							{
								goto IL_064b;
							}
							goto IL_00f7;
						}
						JitsuriAccountTag jitsuriAccountTag = _671BC22C.BF9F3D1F.FE8E0C9E[(0x3DFE7960 ^ num) - 1040087123](current2);
						if (jitsuriAccountTag == null)
						{
							num2 &= 0xD833CFBCu;
							if ((uint)((int)((uint)(-1718443607 - num) / 2870018968u) >> (int)num) / (0x952EF0ACu | ((uint)num ^ num2)) <= ~(~(((uint)num & num2) % 1351494933)))
							{
								num2 = (uint)(-((int)num2 * (((num2 == num2) ? 1 : 0) * 18027)) ^ 0x2BBC6F86);
								continue;
							}
							goto IL_026c;
						}
						object e499E = BCAFAAB5.A830852C(jitsuriAccountTag);
						num2 ^= 1;
						goto IL_01e1;
						IL_02ea:
						num2 -= 2543361556u;
						if (jitsuriInteractiveElement == null)
						{
							num4 = null;
							num2 = (uint)(((int)num2 * -509865830 >>> (int)(num2 & num2)) | -num);
							num2 = 0xC61F9CBFu ^ (2651945110u % ~((1649077466 == 3944641846u % ~((uint)num / ~num2)) ? 1u : 0u));
							continue;
						}
						b = (sbyte)(byte)((-1048203903 >> (int)num) | 0x33B2C226);
						if ((int)(num2 + 31653) >= (int)b)
						{
							num = (short)((uint)b / uint.MaxValue);
							num2 = (uint)(-(1854991656 % (-1081270900 / b)) - -1678778414);
							continue;
						}
						goto IL_0282;
						IL_0282:
						jitsuriInteractiveElement = InteractiveElements.FirstOrDefault((JitsuriInteractiveElement e) => _671BC22C.BF9F3D1F.FE8E0C9E[855](e) == CS_0024_003C_003E8__locals4.doorElementId);
						num2 = (uint)((ushort)((int)num2 / (int)(0 - num2)) >> ~num);
						num2 = (uint)(1597547857 + ((num >> 15 << (int)num) * -1710465133 - -945813699));
						continue;
						IL_01e1:
						bool num6 = _043A6B05._1434D187((string)e499E, accountNickname);
						num2 ^= 1;
						if (num6)
						{
							num = (short)((uint)(2818 % ~(-214261226 % ~(num >>> (int)num))) % ~(((uint)(231467591 + num % -578730584) < (uint)((int)num / (int)num2) / (uint)(-678139881 ^ num)) ? 1u : 0u));
							num2 = 2215571235u / ~((uint)((-1727114747 - (int)num2) & -709802824) % (uint)((int)num2 >> (int)((uint)num % 1630346301u))) + 3852938919u;
							continue;
						}
						goto IL_05b6;
						IL_026c:
						item = CB96861A.BCAED106((int)MapId, CS_0024_003C_003E8__locals4.doorElementId);
						goto IL_0282;
						IL_025c:
						num2 = (0x8AE0AA8 ^ num2) + 318168050;
						goto IL_026c;
						IL_01ba:
						num2 = (uint)((int)(num2 << (int)num2) + -1447653481 - -1731038953);
						e499E = null;
						goto IL_01e1;
						continue;
						end_IL_00a8:
						break;
					}
					goto end_IL_0080;
					IL_05b6:
					if ((int)((0x610B5B83 & num2) * 17242279) >= (int)(0xBE2E0BB8u ^ num2))
					{
						num2 = (uint)((int)((uint)((int)num2 % (int)num2) & (0x85924E16u & num2)) * (0 / (byte)num2) - 199845341);
						goto IL_00a8;
					}
					goto IL_00fe;
					IL_00f7:
					current2 = enumerator2.Current;
					goto IL_00fe;
					end_IL_0080:;
				}
				goto end_IL_000d;
				IL_004a:
				if (!jitsuriHouseOnMap.DoorsOnMap.Any())
				{
					continue;
				}
				goto IL_005a;
			}
			goto IL_0679;
			end_IL_000d:;
		}
		num2 = 0u;
		return result;
		IL_0679:
		b = -68;
		return (DoorElementId: null, InstanceId: null, CellId: null, SkillInstanceUid: null, Element: null);
	}

	internal bool IsActorIdOnMap(long actorId)
	{
		_003C_003Ec__DisplayClass29_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass29_0();
		CS_0024_003C_003E8__locals2.actorId = actorId;
		ushort num = 20453;
		return Actors.Find((JitsuriActorPositionInformation e) => _671BC22C.BF9F3D1F.FE8E0C9E[444](e) == CS_0024_003C_003E8__locals2.actorId) != null;
	}
}
