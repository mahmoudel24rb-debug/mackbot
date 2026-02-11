// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.SpellLevel
using System.Runtime.CompilerServices;

public class SpellLevel
{
	[CompilerGenerated]
	private int _003CApCost_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CId_003Ek__BackingField;

	public int ApCost
	{
		[CompilerGenerated]
		get
		{
			byte b = 0;
			return _003CApCost_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 51244;
			if (-326483169 >> (((uint)((0x1D302B9B & num) - 288030391) > (uint)num) ? 1 : 0) != ~num)
			{
				do
				{
					_003CApCost_003Ek__BackingField = value;
				}
				while (-(811745000 << (num & -1078444399)) == num);
			}
		}
	}

	public int Id
	{
		[CompilerGenerated]
		get
		{
			sbyte b = 19;
			return _003CId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CId_003Ek__BackingField = value;
		}
	}

	public SpellLevel()
	{
		sbyte b = 83;
		_671BC22C.BF9F3D1F.FE8E0C9E[~(((uint)(b ^ b) % 4223133363u) | 0x3401FF1F) + 872548179](this);
	}
}
