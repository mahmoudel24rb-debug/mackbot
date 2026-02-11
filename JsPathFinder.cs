// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.PathFinder.JsPathFinder
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using DofusLibrary.Common.Map;
using DofusLibrary.Common.PathFinder;

internal static class JsPathFinder
{
	internal struct GridPoint
	{
		public int X;

		public int Y;
	}

	private static readonly Dictionary<string, int> _mapPointToCellId;

	static JsPathFinder()
	{
		sbyte b = 0;
		_mapPointToCellId = new Dictionary<string, int>();
		ConstructMapPoints();
	}

	internal static GridPoint GetMapPointFromCellId(int cellId)
	{
		int num = cellId % 14 - cellId / 28;
		int x = num + 19;
		int y = num + cellId / 14;
		GridPoint result = default(GridPoint);
		sbyte b = -99;
		result.X = x;
		result.Y = y;
		ushort num2 = (ushort)(b * -214995583 * -645117556);
		return result;
	}

	private static void ConstructMapPoints()
	{
		ushort num = 65454;
		int num2 = 27173 >> (int)(short)(2365861413u / ((uint)num % 951472172u));
		GridPoint mapPointFromCellId = default(GridPoint);
		DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
		while (true)
		{
			num = (ushort)(~(3240227988u / (uint)(-1028819045 ^ num) % num));
			if (num2 < (num & 0x503366A2) - 25714)
			{
				uint num3 = 1u;
				num3 = (564826506 / (1386064646 / num3) << 520853422 / ((-1975673198 < (int)num3) ? 1 : 0) + (int)(num3 % num3)) + 691815843;
				while (true)
				{
					switch (num3 % 3)
					{
					default:
						num3 /= num3;
						mapPointFromCellId = GetMapPointFromCellId(num2);
						num3 += 2659255811u;
						num3 = (0xFFFFFE6Du & num3) + 2125126943;
						continue;
					case 1u:
					{
						num3 = 2659255812u + ((-334447726 == (int)((uint)(byte)(-1934585968 | (int)num3) % 4265515660u)) ? 1u : 0u);
						Dictionary<string, int> mapPointToCellId = _mapPointToCellId;
						_671BC22C.BF9F3D1F.FE8E0C9E[(((-2037480445 < (int)num3) ? 1u : 0u) / 4286578688u) ^ 0x48C](ref ADB0868E, (1186066694 / (int)(0 / num3 - 1673334448)) ^ 1, (byte)(0x20186023 | (-96749798 * (int)(num3 + num3))) - 241);
						ADB0868E.AppendFormatted(mapPointFromCellId.X);
						_671BC22C.BF9F3D1F.FE8E0C9E[((int)(280224770 - 3316065808u % num3) >> (sbyte)((int)num3 * -2110365171) / -1943614926) ^ -376586078](ref ADB0868E, "_");
						ADB0868E.AppendFormatted(mapPointFromCellId.Y);
						mapPointToCellId[_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)((int)num3 % -2068796760) / ~(num3 & (uint)((int)num3 / (int)num3))) % 11657 + 934](ref ADB0868E)] = num2;
						num = (ushort)(~((int)(34878 / (num3 >> (int)num3)) >> (int)num3));
						num3 = (uint)(0x4E187687 ^ ((int)(~(num3 << 29)) + (0 | (-1943271761 - (int)num3))));
						continue;
					}
					case 2u:
						break;
					}
					break;
				}
				num3 = ((num3 * ((num % num3) & num)) & 0x4C44C9FA) - 2713682894u;
				num2 += (int)(num3 >> (int)num);
				num ^= 0x51;
			}
			else if ((0x880E1893u ^ ((2710894364u > (uint)(0xEAE65B8 & (num | -315231822))) ? 1u : 0u)) != 0)
			{
				break;
			}
		}
	}

	internal static int GetCellId(int x, int y)
	{
		uint num = 180406289u;
		int value = default(int);
		if ((int)(((uint)((int)num * -619140052) % 1622733827u + num) % (uint)((int)num % ((int)num >> 5) % 36655)) <= (int)(0x73921D80 & (0 - (num - 2140472767))))
		{
			Dictionary<string, int> mapPointToCellId = _mapPointToCellId;
			DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
			_671BC22C.BF9F3D1F.FE8E0C9E[1164 + (((int)(num % 2919624459u) > (int)(~num & (uint)((int)num - -1070414529))) ? 1 : 0)](ref ADB0868E, (int)(1308026873 + (0 - (num / num % (num / num) - ((num - 1488153225) & 0xF61B182Fu)))), (int)(2128829783 + num) ^ -1985731222);
			ADB0868E.AppendFormatted(x);
			_671BC22C.BF9F3D1F.FE8E0C9E[((num > ~num) ? 1 : 0) - -852](ref ADB0868E, "_");
			ADB0868E.AppendFormatted(y);
			if (!mapPointToCellId.TryGetValue(_671BC22C.BF9F3D1F.FE8E0C9E[810 + (num + num) % 138](ref ADB0868E), out value))
			{
				short num2 = (short)(-20049568 | (int)num);
				return (int)(num - 180406290);
			}
		}
		return value;
	}

	internal static List<int> GetPath(int source, int target, MapInformation map, Dictionary<int, bool> occupiedCells, bool allowDiagonals = true, bool stopNextToTarget = false, bool isFighting = false)
	{
		return new _78A0B538(map, occupiedCells, allowDiagonals, stopNextToTarget)._362B8B29(source, target);
	}

	private static bool CheckPath(List<int> path)
	{
		uint num = 2770922590u;
		num = (39912908 + num % num >> 17) * num + 2306218737u;
		GridPoint gridPoint = default(GridPoint);
		int num4 = default(int);
		int num2 = default(int);
		short num3 = default(short);
		while (true)
		{
			switch (num % 3)
			{
			default:
				num = (uint)((short)((int)num * (int)(short)num - (int)(1420990987u / (uint)((int)num >> 13))) - 1524054467);
				goto IL_0046;
			case 1u:
				if (path.Count < 2 + (int)((num + 1556040729) % 10119428) % (int)(~(((uint)(num3 - 2035086412) < (uint)((int)num3 - (int)num)) ? 1u : 0u)))
				{
					goto IL_00a7;
				}
				gridPoint = GetMapPointFromCellId(path[(short)(num3 - num3)]);
				num3 = (short)((int)num + num3 / (0x35ACA707 ^ ((int)num + -1006385892)));
				num4 = (int)((0 - ~num) & (uint)(-(2056597920 * ((int)num / 1017922971)))) + -536879103;
				goto IL_01c2;
			case 2u:
				{
					num = (uint)(((byte)num2 / num3 - num2 << num2) + -1524044706);
					if (num4 < path.Count)
					{
						GridPoint mapPointFromCellId = GetMapPointFromCellId(path[num4]);
						if (_671BC22C.BF9F3D1F.FE8E0C9E[1803](gridPoint.X - mapPointFromCellId.X) > 1 || _671BC22C.BF9F3D1F.FE8E0C9E[1803](gridPoint.Y - mapPointFromCellId.Y) > 1)
						{
							num = 4294967294u;
							return (byte)(0 - num % num << 28) != 0;
						}
						num3 = -20884;
						if (num3 == 0)
						{
							goto IL_0046;
						}
						gridPoint = mapPointFromCellId;
						num4 += (num3 % (-1080084451 >>> (int)num3) << -num3 << (num3 << 2) + (-1128990823 - (num3 - -1055804660))) - int.MaxValue;
						num3 ^= 0x4232;
						num = 2770922590u;
						goto IL_01c2;
					}
					return (byte)(-93 + (sbyte)num3) != 0;
				}
				IL_00a7:
				return (byte)(((0x8D2E6D1Fu & ((num ^ num) - 1127873470)) >> 616077207 / (int)num) ^ 0x8C060402u) != 0;
				IL_01c2:
				num2 = (int)(((uint)(num3 >> ((230055603 == num) ? 1 : 0)) / ~(1916302633u / (uint)num3 / 3753698855u)) | (((num < num) ? 1u : 0u) >> 21));
				num = (((uint)((int)(num + 2024637500) / (-828679781 | num3)) < (uint)(-1249317851 * ((622408762u > (uint)(num3 + -1674713419)) ? 1 : 0))) ? 1u : 0u) ^ 0x6B35E287u;
				break;
				IL_0046:
				if (path != null)
				{
					num3 = (short)(-707121263 * (-1196969314 - (int)num));
					if ((int)(num & 0x16142D3E) >= (int)(((0xE8B12133u & num) << 21) & 0x31289007))
					{
						break;
					}
				}
				goto IL_00a7;
			}
		}
	}

	internal static List<int> ExpandPath(List<int> path)
	{
		sbyte b = -99;
		b = (sbyte)(0x10 ^ (0x9B967636u & (2057396663u / (uint)b)));
		byte b2 = default(byte);
		List<int> list = default(List<int>);
		int num3 = default(int);
		int num4 = default(int);
		int num2 = default(int);
		GridPoint mapPointFromCellId2 = default(GridPoint);
		uint num = default(uint);
		while (true)
		{
			switch ((uint)b % 8u)
			{
			default:
				b = (sbyte)((-139003485 - b) ^ 0x849060E);
				if (path != null)
				{
					goto IL_00a5;
				}
				if ((b >> (int)b >> (int)b) * (b / -1172873550) == 0)
				{
					b = (sbyte)((b | -770672468) % -550941317 - -76);
					break;
				}
				goto IL_0311;
			case 1u:
				b = (sbyte)(-98 + (int)(~(1300966660 % ((uint)b / (uint)b))));
				return new List<int>();
			case 2u:
				b = (sbyte)((int)((3585282186u / (uint)(b % b2) >> ((b2 ^ 0x73970A81) >>> 1259082530 * b)) % b2) ^ -84);
				if (path.Count < (((-b2 % (b ^ 0x3912C128)) | (1379596477 >> (b2 >> 23))) ^ (b2 * ~b2)) - 3303)
				{
					goto IL_0133;
				}
				goto IL_0135;
			case 3u:
				b = (sbyte)(1506636758 / (0x470F47B4 | b2) + -100);
				list = new List<int> { path[(-b2 >> b % 497321873) - -1] };
				if (-1504863258 / (int)(~(((uint)b2 & ((322514591u > (uint)b) ? 1u : 0u)) >> (b ^ 0x26910836))) > (b2 >>> 17) * ((0x915D833 ^ b) >>> ((b ^ -1031709792) & 0x2195EDB5)))
				{
					b = (sbyte)(-5 ^ ~(1882392604 % (b ^ b2) >>> (int)b2));
					break;
				}
				goto IL_0496;
			case 4u:
			{
				b = (sbyte)(-100 + b % ((int)(2097148159u % (uint)b) - (b ^ b) * (b2 + b)));
				GridPoint mapPointFromCellId = GetMapPointFromCellId(path[((int)((uint)b2 % 3875976465u + 278069765) >> 58 - b2) - (int)((uint)b2 % (uint)(b2 + 931566999)) - 17379279]);
				num3 = mapPointFromCellId.X;
				num4 = mapPointFromCellId.Y;
				num = 0u >> -532929602 % (int)((uint)(0x1AABB0B4 | b2) / (uint)b2);
				num2 = -1524604059 + (0x5ADF9CFC & b);
				if (b * -38084220 == 0)
				{
					b2 = ((-290540784 * b % (ushort)(-b2) < 1577683596) ? ((byte)1) : ((byte)0));
					break;
				}
				goto IL_059c;
			}
			case 5u:
				num = 264u;
				if (num * 1444437655 / (uint)((int)(num + num) + -1254971776) == 0)
				{
					goto IL_00a5;
				}
				mapPointFromCellId2 = GetMapPointFromCellId(path[num2]);
				goto IL_0311;
			case 6u:
				b = (sbyte)(0x2AACDFC5 ^ -(-1702493797 % (-1900853342 << -b)));
				list.Add(GetCellId(num3, num4));
				num = 264u;
				goto IL_03ce;
			case 7u:
				{
					b = (sbyte)(-94 + (int)(b2 * (num ^ 0xCCB9E482u)) * (int)b / (((-248517880 ^ b) * -945678020) | -466800987));
					num2 += 1 + (b2 >> (int)(2987101846u / (uint)(-b) % (uint)b));
					num -= 2839311403u;
					b2 = (byte)(b2 + -169);
					b ^= 0x38;
					goto IL_059c;
				}
				IL_03ce:
				do
				{
					num = (uint)(~((int)num / -1675065344) - 0);
				}
				while ((int)(((0x3291F122 | num) >> (int)((num & 0xD4BFCA83u) | num)) | 0x4F17EEB8) < (int)num % (int)(num << 6) / (int)(num + num >> (int)(3926742844u % num) % -684158023));
				if (_671BC22C.BF9F3D1F.FE8E0C9E[((-1466168569 > ~((byte)num ^ -1036739037)) ? 1 : 0) - -1803](mapPointFromCellId2.Y - num4) > (((uint)((int)(~num) + ((int)(1052259079 % num) + ((num == num) ? 1 : 0))) < num) ? 1 : 0))
				{
					goto IL_0361;
				}
				num = 2839311402u - num;
				if ((((0x1302AD24 ^ num) / num < 2794038714u / num) ? 1u : 0u) / (uint)(-84 - (int)num) != 0)
				{
					goto IL_0311;
				}
				num3 = mapPointFromCellId2.X;
				if ((((-996501340 - (int)num) / (int)num) & -489933736) != 0)
				{
					goto IL_0133;
				}
				goto IL_0496;
				IL_059c:
				if (num2 >= path.Count)
				{
					return list;
				}
				goto case 5u;
				IL_00a5:
				b2 = (byte)(short)(-1046979049 * (135653010 + b));
				b = (sbyte)((~(-1549700834 - b2) * b2) ^ -1861362448);
				break;
				IL_0135:
				b2 = (byte)(-(-1574561109 >> (-1623999063 >>> (int)b)));
				b = (sbyte)(-261787 ^ (-134033996 >> ~b2));
				break;
				IL_0133:
				return path;
				IL_0496:
				num4 = mapPointFromCellId2.Y;
				b2 = (byte)(0u - (((int)(0 & (0xDE2EFA84u ^ num)) > (int)num) ? 1u : 0u));
				if (((b2 / -727063086) ^ -73082840) < ((-1230416972 < (int)num) ? 1 : 0) % 1185357590 + (int)(b2 % (num + 504247425)))
				{
					list.Add(path[num2]);
					b = (sbyte)(-1615208268 / ((int)(b2 - (uint)b2 / 1947253004u) + (int)(short)num));
					if (((uint)b & (4113984037u / (uint)(b - 698868410))) / (uint)(-(~b)) == 0)
					{
						b = (sbyte)((uint)(0x7A904D26 | (b2 & -343921536)) / (uint)(b2 & 0x71383AB1) - 11617347);
						break;
					}
					goto IL_0135;
				}
				goto IL_0361;
				IL_0361:
				num4 += ((mapPointFromCellId2.Y > num4) ? 1 : (-1));
				b = 24;
				b = (sbyte)((61753 * (b + 1745302144) << (int)b) ^ -671088578);
				break;
				IL_0311:
				while (_671BC22C.BF9F3D1F.FE8E0C9E[0x6167 ^ ((int)num - -25956)](mapPointFromCellId2.X - num3) > (int)((uint)((925482681 >>> (int)num) | (968383120 / (int)(num << (int)num))) % (0x8EA45D98u & num) - 101))
				{
					num3 += ((mapPointFromCellId2.X > num3) ? 1 : (-1));
					list.Add(GetCellId(num3, num4));
					num = 264u;
				}
				goto IL_03ce;
			}
		}
	}

	internal static List<int> CompressPath(List<int> path)
	{
		List<int> list = new List<int>();
		sbyte b = -4;
		b = (sbyte)((0x66ACB1B5 ^ ((791330610 >> (int)b) + -518903923)) - -2017552848);
		int num9 = default(int);
		int num2 = default(int);
		int num3 = default(int);
		int num6 = default(int);
		int num7 = default(int);
		int num8 = default(int);
		sbyte b2 = default(sbyte);
		uint num = default(uint);
		sbyte b3 = default(sbyte);
		while (true)
		{
			switch ((uint)b % 5u)
			{
			default:
				b = (sbyte)((-1013778427 & b) + -4);
				num9 = path[(ushort)b - 65532];
				goto IL_0066;
			case 1u:
				b = (sbyte)((1669611014 % b) ^ -45);
				goto IL_0066;
			case 2u:
				b = (sbyte)((0x26A0400A & (b - -1089594223 - ((b << (int)b2) + 2085795142))) - 75497481);
				if (num2 < path.Count)
				{
					num3 = -115267913;
					if ((uint)((int)((uint)num3 % 2125384255u / (uint)num3) * -((num3 + num3) % num3)) < (uint)(num3 | -117417984))
					{
						int num4 = path[num2];
						GridPoint mapPointFromCellId = GetMapPointFromCellId(num4);
						int num5 = ((num2 == 0) ? (-1 + (874275128 >>> (num3 & num3) << (-181015527 + num3 >> 25)) / ~(num3 * num3 << num3)) : ((mapPointFromCellId.Y == num6) ? ((mapPointFromCellId.X > num7) ? (-(num3 >> num3 << 20) - 14680057) : (-682079549 + (-109051721 - (num3 << 13)) % (num3 >> num3) - -682079563)) : ((mapPointFromCellId.X != num7) ? ((mapPointFromCellId.X <= num7) ? ((mapPointFromCellId.Y > num6) ? ((int)((uint)((int)(0u / (uint)(num3 << num3)) - 9639 % num3) / (((uint)(5977 / num3) < 3215982367u) ? 1u : 0u)) ^ -9637) : ((int)(((uint)num3 % (uint)num3) & (((uint)num3 > (uint)num3) ? 1u : 0u) & (ushort)num3) - -1961127536 - 1961127532)) : ((mapPointFromCellId.Y > num6) ? ((int)(1937771829 % ((uint)(sbyte)num3 % ~(((uint)num3 < 227090356u) ? 1u : 0u) % (uint)(-31 ^ (750517942 >>> num3)))) + -1937771829) : ((((num3 + 377500430 + int.MinValue) / (int)((uint)(num3 >> 5) / (uint)num3)) ^ num3) + -1988132716))) : ((mapPointFromCellId.Y > num6) ? (1160572340 + ((num3 ^ -1509598831) + 1538167591)) : ((1 * num3) ^ -115267918)))));
						if (num5 != num8)
						{
							list.Add(num9 + (num5 << (-563781520 | num3) + 10131733));
							num8 = num5;
						}
						num9 = num4;
						num7 = mapPointFromCellId.X;
						b2 = (sbyte)num3;
						num6 = mapPointFromCellId.Y;
						b2 = (sbyte)(2517980336u / (((uint)((num3 >> 26) - b2) < (uint)(1361515938 / num3)) ? 1u : 0u));
						goto IL_0369;
					}
					goto IL_0448;
				}
				num = (uint)(b2 * b2);
				b = (sbyte)(751621443 + (-751621502 - b3));
				break;
			case 3u:
				b = (sbyte)((int)(0 - ((ushort)b3 ^ (num & 0x2EA58819))) - -65474);
				goto IL_0448;
			case 4u:
				{
					b = (sbyte)(~(short)(-b2) * -1490820182 - -1313326931);
					return list;
				}
				IL_0066:
				b = (sbyte)(byte)(b >> (int)b);
				num8 = -45 ^ (b & 0x2C);
				num7 = (-632420373 ^ b) / -615339051 - -1;
				num6 = b - -24415425 - 24415424;
				b2 = (sbyte)(~((b == -1851496160 - (b + 1703706398)) ? 1 : 0));
				num2 = (int)((uint)b % ~((uint)b % (uint)b) / (uint)(b - (52724753 << (int)b)) * 1478382464);
				if ((int)(4060285698u % (uint)((b - b) ^ 0x3B72C094)) * (b >> (int)b) == 0)
				{
					b2 = (sbyte)(~(-11 ^ (-1693118195 % b2)));
					b = (sbyte)(-52 ^ (b % -(b2 * 1551998602)));
					break;
				}
				goto IL_03b2;
				IL_0448:
				list.Add(num9 + (num8 << (-9 ^ (short)(-b)) - -22));
				if ((int)b3 - (int)(4290473653u % (uint)b) >= (b3 | -846156099))
				{
					b = (sbyte)(b / 1588300570 + 34);
					break;
				}
				goto IL_0369;
				IL_03b2:
				b3 = (sbyte)(((int)b + (int)(2259294783u % ~((b < b2) ? 1u : 0u))) / b);
				b = (sbyte)(789792427 * ((uint)b2 / 1u << 3) + 2023372152);
				break;
				IL_0369:
				num2 += (int)(0x108 ^ ((uint)(-(0x2888D410 | num3) << (int)((uint)(b2 << num3) | ((num3 == 1502599938) ? 1u : 0u))) & ((uint)(ushort)num3 / (uint)((95528893 % b2) & 0x1B1B59BC))));
				b2 -= -79;
				b = -1;
				goto IL_03b2;
			}
		}
	}

	internal static List<int> NormalizePath(List<int> path)
	{
		int num = -106215301;
		if ((num & 0x74168D16) != 0 && !CheckPath(path))
		{
			return ExpandPath(path);
		}
		return path;
	}

	internal static string LogPath(List<int> path)
	{
		int num2 = default(int);
		int num3 = default(int);
		StringBuilder stringBuilder = default(StringBuilder);
		int num6 = default(int);
		int num7 = default(int);
		uint num8 = default(uint);
		int num4 = default(int);
		GridPoint mapPointFromCellId = default(GridPoint);
		DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
		byte b = default(byte);
		while (true)
		{
			string[,] array = new string[33, 34];
			int num = 0;
			while (true)
			{
				if (num < 33)
				{
					num2 = 0;
					goto IL_00a1;
				}
				num3 = 0;
				goto IL_02f7;
				IL_0385:
				num4 = 1209040896;
				ushort num5 = (ushort)((((uint)num4 / 1503150878u) ^ 0xC88A7935u) | (uint)((int)((uint)num4 / 2536822183u) + -1221716723 % num4) | (uint)((num4 << 19) + (num4 << num4)));
				if ((uint)(~(num4 & 0xF0B9C12)) / (uint)(~(~num4) - (-1885532866 - num4) % num4) != 0)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[(num5 * 589133985) ^ 0x7E3CDB28](stringBuilder, array[num6, num7]);
					num4 = (byte)(-1702330481 * num5);
					num6 += 0x12 ^ (num4 % -(num4 - -551782779));
					num5 = (ushort)(num5 + -40291);
					goto IL_042d;
				}
				goto IL_04b3;
				IL_0084:
				num2 += (~b >> (b ^ -996708440)) % -66586320 + 2;
				goto IL_00a1;
				IL_00a1:
				while (num2 < 34)
				{
					array[num, num2] = ((GetCellId(num, num2) == -1) ? "    " : "[  ]");
					b = 0;
					if (((int)((uint)b / (uint)(~b)) / ~b << 8) % (int)(~((uint)(-461260914 + b) / (uint)(~(sbyte)(b * b)))) == b - 9)
					{
						continue;
					}
					goto IL_006f;
				}
				num5 = 0;
				if (num5 * 0 >= (int)((uint)(-2097152000 | (num5 - 1)) % (uint)(~(-num5 + (num5 ^ (num5 >> (int)num5))))))
				{
					num += 1 ^ num5;
					continue;
				}
				goto IL_042d;
				IL_0341:
				num8 = num8;
				num7 = (int)((num8 % 3381110970u) ^ (uint)(-225490768 << (int)((num8 << (int)num8) - num8))) - -225490768;
				goto IL_04b3;
				IL_04b3:
				if ((0x27E69D66 | ((0x5635DD8Eu | ((num8 < num8) ? 1u : 0u)) ^ (0 - (0x618C0920 & num8)))) >= (uint)(~((int)num8 / ~(((1208670364 < num8) ? 1 : 0) >> (int)num8))))
				{
					goto IL_0385;
				}
				if (num7 < (int)(((uint)((int)(num8 ^ (num8 << (int)num8)) + -120) ^ ~num8) - 85))
				{
					num5 = 24538;
					num6 = (int)(2 + ~(((int)((uint)num5 % (uint)num5) > (int)(sbyte)num5) ? 1u : 0u));
					goto IL_042d;
				}
				num4 = (int)((uint)(sbyte)num8 / ~num8);
				if (((((116746752u < (uint)(num4 ^ 0x8339106)) ? 1u : 0u) == (uint)num4 % 2719691421u / 3493671606u) ? 1 : 0) != (int)num8 - -67108865)
				{
					return stringBuilder.ToString();
				}
				goto IL_0084;
				IL_006f:
				while (true)
				{
					switch ((uint)b % 3u)
					{
					case 1u:
						goto IL_015d;
					case 2u:
						goto IL_01ec;
					}
					break;
					IL_01ec:
					b = (byte)(0x9E4BEDA5u ^ (((uint)((int)(0 - num8) * -734550525) / (uint)(short)(0x572181AB | num8)) ^ (uint)(-1639191099 >>> ((-1474149463 > b >> num4) ? 1 : 0))));
					int x = mapPointFromCellId.X;
					int y = mapPointFromCellId.Y;
					_671BC22C.BF9F3D1F.FE8E0C9E[(num4 >>> 2) ^ 0x494](ref ADB0868E, (int)((0xF5847928u & num8) << 10) + -14688254, (int)(3273678868u % num8 + 1021288429));
					_671BC22C.BF9F3D1F.FE8E0C9E[1182736943 + (0x58E9D9EA ^ num8)](ref ADB0868E, "[");
					ADB0868E.AppendFormatted(num3, "00");
					_671BC22C.BF9F3D1F.FE8E0C9E[1290995157 + (b | -1290994401)](ref ADB0868E, "]");
					array[x, y] = _671BC22C.BF9F3D1F.FE8E0C9E[((num4 | -2120914421) + (num4 >>> 7) - -728314610) ^ num4 ^ -1392599397](ref ADB0868E);
					goto IL_02e4;
					IL_015d:
					b = (byte)(-2022068328 + (int)(0xDC83113Bu ^ num8) * -274634342);
					if (mapPointFromCellId.Y >= -475850197 + b - -475850135)
					{
						goto IL_02e4;
					}
					num4 = (int)(sbyte)b + (int)(0 & (num8 | 0xBF8FD11Fu));
					if ((int)(((num8 + b) & 0xF522F2AFu) % (uint)(~num4 * -2137218156 % (int)(num8 | 0xF42FF6BEu))) > (int)(((uint)((int)num8 / 1570757141) % 3057125658u) & 0xFE96733Bu))
					{
						b = (byte)((-308486506 ^ ((int)num8 - -737786198)) + 520213357);
						continue;
					}
					goto IL_04b3;
				}
				goto IL_0084;
				IL_02e4:
				num3 += (int)((num8 ^ 0x889D35B6u) - 1777601912);
				goto IL_02f7;
				IL_02f7:
				num8 = 0u;
				if (num8 != 0)
				{
					goto IL_00a1;
				}
				if (num3 < path.Count)
				{
					num8 = 3781769423u;
					if ((num8 & (uint)(((int)num8 % (int)num8 + (int)num8) / -1710442341)) != (uint)(sbyte)(num8 / ~(2492640807u / (num8 << (int)num8))))
					{
						goto IL_0084;
					}
					mapPointFromCellId = GetMapPointFromCellId(path[num3]);
					if (mapPointFromCellId.X < (int)((num8 | (byte)num8) + 513197906))
					{
						b = (byte)(ushort)((num8 << 4) * num8 * 3196173190u);
						b = (byte)(0x3A ^ (0x47A23E81 & b));
						goto IL_006f;
					}
					goto IL_02e4;
				}
				if (0 - num8 != 0)
				{
					break;
				}
				stringBuilder = _671BC22C.BF9F3D1F.FE8E0C9E[2048772612u / (uint)(1059411497 << (int)((num8 | 0x17974128) / ~(0 - num8))) + 322]();
				goto IL_0341;
				IL_042d:
				num4 = 0 ^ ~(num5 % num5);
				if (num6 < 1077975111 + (((-1274116843 + num4) * num5 << 8) + num5))
				{
					goto IL_0385;
				}
				if ((uint)(0x58BD7D3D | num5) < (uint)num5)
				{
					goto IL_02f7;
				}
				_671BC22C.BF9F3D1F.FE8E0C9E[-30297 + num4 - -30763](stringBuilder);
				if (~num5 == 0)
				{
					goto IL_0341;
				}
				num7 += (int)(0x33966AAF ^ ((uint)(865520264 - num5) % (uint)(num5 + num4 * 1696546724 + (int)((uint)(num5 * num4) % (uint)num5))));
				num8 = 0u;
				goto IL_04b3;
			}
		}
	}
}
