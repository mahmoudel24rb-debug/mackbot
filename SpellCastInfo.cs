// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.SpellCastInfo
using System.Runtime.CompilerServices;
using DofusLibrary.Common.Repository;

public class SpellCastInfo
{
	[CompilerGenerated]
	private int _003CFromCellId_003Ek__BackingField;

	public double Ratio { get; set; }

	public DetailedSpell Spell { get; set; }

	public int FromCellId
	{
		[CompilerGenerated]
		get
		{
			sbyte b = 0;
			return _003CFromCellId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CFromCellId_003Ek__BackingField = value;
		}
	}

	public SpellCastInfo()
	{
		byte b = 8;
		_671BC22C.BF9F3D1F.FE8E0C9E[2098 + ((1873661119 > b) ? 1 : 0)](this);
	}
}
