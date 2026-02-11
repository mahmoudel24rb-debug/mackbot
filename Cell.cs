// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.JsonClasses.Cell
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

public class Cell
{
	[CompilerGenerated]
	private int _003CArrow_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CBlue_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CCellNumber_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CFloor_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CHavenbagCell_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CLos_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CMapChangeData_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CMov_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CMoveZone_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CNonWalkableDuringFight_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CNonWalkableDuringRP_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CRed_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CX_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CY_003Ek__BackingField;

	[JsonProperty("arrow")]
	public int Arrow
	{
		[CompilerGenerated]
		get
		{
			sbyte b = -56;
			return _003CArrow_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CArrow_003Ek__BackingField = value;
		}
	}

	[JsonProperty("blue")]
	public int Blue
	{
		[CompilerGenerated]
		get
		{
			return _003CBlue_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = 24653;
			do
			{
				_003CBlue_003Ek__BackingField = value;
			}
			while ((0x380B663D & ((uint)((int)((uint)num % (uint)num) * -550509789) % (uint)((num >>> 30) ^ -818996065))) != 0);
		}
	}

	[JsonProperty("cellNumber")]
	public int CellNumber
	{
		[CompilerGenerated]
		get
		{
			byte b = 0;
			return _003CCellNumber_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CCellNumber_003Ek__BackingField = value;
		}
	}

	[JsonProperty("farmCell")]
	public int FarmCell { get; set; }

	[JsonProperty("floor")]
	public int Floor
	{
		[CompilerGenerated]
		get
		{
			return _003CFloor_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 50530;
			_003CFloor_003Ek__BackingField = value;
		}
	}

	[JsonProperty("havenbagCell")]
	public int HavenbagCell
	{
		[CompilerGenerated]
		get
		{
			sbyte b = 49;
			return _003CHavenbagCell_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CHavenbagCell_003Ek__BackingField = value;
		}
	}

	[JsonProperty("linkedZone")]
	public int LinkedZone { get; set; }

	[JsonProperty("los")]
	public int Los
	{
		[CompilerGenerated]
		get
		{
			return _003CLos_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 0;
			_003CLos_003Ek__BackingField = value;
		}
	}

	[JsonProperty("mapChangeData")]
	public int MapChangeData
	{
		[CompilerGenerated]
		get
		{
			return _003CMapChangeData_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = -1340996608;
			_003CMapChangeData_003Ek__BackingField = value;
		}
	}

	[JsonProperty("mov")]
	public int Mov
	{
		[CompilerGenerated]
		get
		{
			uint num = 242522570u;
			return _003CMov_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CMov_003Ek__BackingField = value;
		}
	}

	[JsonProperty("moveZone")]
	public int MoveZone
	{
		[CompilerGenerated]
		get
		{
			short num = -499;
			return _003CMoveZone_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 718317279u;
			_003CMoveZone_003Ek__BackingField = value;
		}
	}

	[JsonProperty("nonWalkableDuringFight")]
	public int NonWalkableDuringFight
	{
		[CompilerGenerated]
		get
		{
			sbyte b = 74;
			return _003CNonWalkableDuringFight_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			sbyte b = 0;
			if ((int)b / (int)(~((uint)b % 1385018163u)) == 0)
			{
				_003CNonWalkableDuringFight_003Ek__BackingField = value;
			}
		}
	}

	[JsonProperty("nonWalkableDuringRP")]
	public int NonWalkableDuringRP
	{
		[CompilerGenerated]
		get
		{
			short num = 4434;
			return _003CNonWalkableDuringRP_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CNonWalkableDuringRP_003Ek__BackingField = value;
		}
	}

	[JsonProperty("red")]
	public int Red
	{
		[CompilerGenerated]
		get
		{
			return _003CRed_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			sbyte b = -76;
			_003CRed_003Ek__BackingField = value;
		}
	}

	[JsonProperty("speed")]
	public int Speed { get; set; }

	[JsonProperty("visible")]
	public int Visible { get; set; }

	[JsonProperty("x")]
	public int X
	{
		[CompilerGenerated]
		get
		{
			return _003CX_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 156;
			if (b >> (~(b - b) << (int)b) != 0)
			{
				do
				{
					_003CX_003Ek__BackingField = value;
				}
				while ((b & -139763557) == 0);
			}
		}
	}

	[JsonProperty("y")]
	public int Y
	{
		[CompilerGenerated]
		get
		{
			return _003CY_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 249963148u;
			_003CY_003Ek__BackingField = value;
		}
	}

	public Cell()
	{
		ushort num = 0;
		if ((uint)(26180389 / ((-2118173157 << (int)num) - -1308328294)) >= (uint)(num % ~num) % (uint)(~num) >> 7)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[0x833 ^ (num >>> 1)](this);
		}
	}
}
