// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.FightCharacteristic
using System.Runtime.CompilerServices;
using DofusLibrary.Common.Repository;

internal class FightCharacteristic
{
	[CompilerGenerated]
	private CharacteristicKeyword _003CCharacteristicKeyword_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CId_003Ek__BackingField;

	[CompilerGenerated]
	private string _003CName_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CValue_003Ek__BackingField;

	internal CharacteristicKeyword CharacteristicKeyword
	{
		[CompilerGenerated]
		get
		{
			ushort num = 30531;
			return _003CCharacteristicKeyword_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CCharacteristicKeyword_003Ek__BackingField = value;
		}
	}

	internal int Id
	{
		[CompilerGenerated]
		get
		{
			return _003CId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = -290500312;
			_003CId_003Ek__BackingField = value;
		}
	}

	internal string Name
	{
		[CompilerGenerated]
		get
		{
			short num = 13536;
			return _003CName_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 33;
			_003CName_003Ek__BackingField = value;
		}
	}

	internal int Value
	{
		[CompilerGenerated]
		get
		{
			uint num = 738209812u;
			return _003CValue_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CValue_003Ek__BackingField = value;
		}
	}

	internal int BaseValue { get; set; }

	internal FightCharacteristic(CharacteristicKeyword characteristicKeyword, int id, string name, int value)
	{
		byte b = 50;
		b = (byte)((-1457712584 >>> (int)b) * b * (int)((uint)b / 1362900399u) + 56);
		while (true)
		{
			switch ((uint)b % 4u)
			{
			default:
				b = (byte)(((369098752 > (uint)b % (uint)b + b) ? 1 : 0) + -635818316 - -635818365);
				_671BC22C.BF9F3D1F.FE8E0C9E[(((uint)b > (uint)(b >>> (int)b)) ? 1u : 0u) ^ 0x832u](this);
				CharacteristicKeyword = characteristicKeyword;
				b = (byte)(-1427316397 + b);
				if (b != 0)
				{
					break;
				}
				goto IL_008c;
			case 1u:
				Id = id;
				goto IL_008c;
			case 2u:
				b = (byte)(133 + ~(~b >> 21 >> (int)(1310355622u % (uint)b)));
				do
				{
					Value = value;
				}
				while (-1996231773 / (int)(~(1385360640 / ~((b >> 15 == 2010907637) ? 1u : 0u))) == (int)((uint)((0x4C04E61F & (b + -964264681)) >> (int)(1535289759u % (uint)b)) / (uint)(~((-1373771862 >> (int)b << (int)b) * (int)((uint)(1285718298 << (int)b) / (uint)(-248606709 + b))))));
				b = (byte)(b + (b << (int)((uint)(b + b) / (uint)b)) - 618);
				break;
			case 3u:
				{
					b = (byte)((-1 | b) - -134);
					return;
				}
				IL_008c:
				Name = name;
				b = b;
				if (((2787315619u % (uint)(~(b >>> (922805790 >>> (int)b))) < 870185158) ? 1 : 0) > ~b >> (int)((uint)((b ^ b) * -1028963140) % (uint)((b | b) * 470270128)))
				{
					b = (byte)(12320 + (b - 12275));
					break;
				}
				return;
			}
		}
	}

	internal new string ToString()
	{
		DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
		short num;
		while (true)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 1, 2);
			num = -29969;
			if (-(-147621071 << (int)num) == 0)
			{
				break;
			}
			num = (short)((num * 65535) ^ -1964018531);
			sbyte b;
			do
			{
				IL_0039:
				switch ((uint)num % 4u)
				{
				case 1u:
					_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)(-num) / (uint)(~((num & -272845675) >>> (int)num))) - -1811628020 - 1811627169](ref ADB0868E, ":");
					num = (short)(~(-((num ^ num) * num)));
					num = (short)((int)((uint)(num * -2095843274) % 3382075279u) + -2095828784);
					goto IL_0039;
				case 2u:
					goto IL_010a;
				case 3u:
					goto end_IL_0000;
				}
				num = (short)(-36653746 + ((0x338C5018 & (num ^ -1975824472)) | (num * 253423618 >> 9)));
				_671BC22C.BF9F3D1F.FE8E0C9E[-(num * -218047175) + 2010530670](ref ADB0868E, Name);
				num = (short)(1 << num - num >> num / 61491);
				if (4294967287u / (uint)(num + num) == 0)
				{
					break;
				}
				goto IL_0039;
				IL_010a:
				num = (short)(0x6625CE89 ^ (-1713763860 ^ num));
				ADB0868E.AppendFormatted(Value);
				b = (sbyte)(0xC7B1 & (num - num));
			}
			while ((b & -1380999516) == 0);
			continue;
			end_IL_0000:
			break;
		}
		return _671BC22C.BF9F3D1F.FE8E0C9E[(-1096352889 >>> (int)num) ^ 0x3A7](ref ADB0868E);
	}
}
