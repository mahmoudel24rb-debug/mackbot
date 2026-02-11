// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.LineOfSight
using System;
using System.Collections.Generic;
using DofusLibrary.Common.JsonClasses;

internal static class LineOfSight
{
	internal const int MAP_GRID_WIDTH = 14;

	internal static bool FloatAlmostEquals(double a, double b, double epsilon = 1E-06)
	{
		short num = -15716;
		return _671BC22C.BF9F3D1F.FE8E0C9E[-(0x1D0DB13C | num | 0x5A387DAB) + 1790](a - b) < epsilon;
	}

	internal static int GetCellIdByCoord(int x, int y)
	{
		ushort num;
		int num2;
		uint num3;
		do
		{
			num = 0;
			num2 = x + (y - y % ((num >>> num % ~(num % ~num)) - (1687953413 >> (int)num) - -1687953415)) / (-272 * num + 2);
			num3 = (uint)((-1795082094 / ~num >>> 23) + num);
		}
		while (((0x67473AE7 | num3) & 0x57C6AC42) == 0);
		return y * (int)(0xFu ^ (((uint)(-602661354 - ((int)num3 >> (int)num)) > (0xB2 | num3)) ? 1u : 0u)) + num2;
	}

	internal static List<Cell> FindLosCells(Cell start, List<Cell> cellMap, int range, Dictionary<int, bool> occupiedCells)
	{
		uint num = 0u;
		List<Cell> list = default(List<Cell>);
		_9F8EAF35 _9F8EAF36 = default(_9F8EAF35);
		uint num2 = default(uint);
		if ((uint)(ushort)num <= (uint)(-1926535891 >> (int)(num / 2130163568)))
		{
			while (true)
			{
				switch (num % 4)
				{
				default:
					list = new List<Cell>();
					if (start == null)
					{
						num2 = (uint)(-((int)num * ((((int)num > (int)num) ? 1 : 0) >> 31)));
						num = (num >> (int)(num / 1480540293)) + 1219628989;
						continue;
					}
					num2 = (uint)((int)(~(num >> 22)) % ~((int)(num >> 1) % ~(((num == num) ? 1 : 0) * (int)num)));
					if ((uint)(~((int)num2 % (-1601196543 << (int)num2)) + 834132012) < (uint)(-586302824 + (-1390698212 + (int)num2) * -1991828030))
					{
						num = num2 + 1469381567 + 2524774467u;
						continue;
					}
					goto IL_0072;
				case 1u:
					num = 0x48B20FBD ^ ((uint)((int)num - -((int)num2 >> (int)num2)) ^ num2);
					goto IL_0072;
				case 2u:
					num = num2;
					_9F8EAF36 = _9F8EAF35.D6056589((uint)start.CellNumber);
					num2 = (uint)((1206572129 >> (int)(byte)(num2 / 1033576608)) + 137974676);
					if (num - 145 >= (uint)(-1482510913 * (int)num))
					{
						num = (uint)(1732363619 + (((int)num % (-1574855123 / (int)num2)) | ((int)(num2 << 4) % ((-921975387 + (int)num) & 0x17060814))));
						continue;
					}
					goto default;
				case 3u:
					break;
					IL_0072:
					return list;
				}
				break;
			}
			num = ~((num >> (int)(num2 + num)) ^ 0x6BBC50AC) - 4089703416u;
		}
		List<Cell>.Enumerator enumerator = cellMap.GetEnumerator();
		try
		{
			if (549143187 % ((1144837016 % (int)num2 < 1369705777) ? 1 : 0) == 0)
			{
				goto IL_015e;
			}
			goto IL_02ef;
			IL_015e:
			_9F8EAF35 _9F8EAF37 = default(_9F8EAF35);
			Cell current = default(Cell);
			while (true)
			{
				switch (num2 % 5)
				{
				case 1u:
					break;
				case 2u:
					goto end_IL_015e;
				case 3u:
					goto IL_02a7;
				default:
					goto IL_02ef;
				case 4u:
					num2 -= 2304836884u;
					return list;
				}
				num2 = (0x39D6F7E1 ^ num2) - 3904484444u;
				_9F8EAF37 = _9F8EAF35.D6056589((uint)current.CellNumber);
				if (3954758076u * num2 != 0)
				{
					num2 = (num2 & (num2 >> (int)num)) % (uint)(sbyte)num2 + 1159154198;
					continue;
				}
				goto IL_0262;
				continue;
				end_IL_015e:
				break;
			}
			num2 = (0x9E1305AFu & num & (uint)(~(short)num)) - 3219970059u;
			goto IL_0233;
			IL_0262:
			int num3 = _671BC22C.BF9F3D1F.FE8E0C9E[(short)num - 26105](_9F8EAF36.E8A39495 - _9F8EAF37.E8A39495);
			num2 = 0x4F000000 ^ num;
			num2 = (uint)((sbyte)(num << (int)(1636128178 + num2)) ^ 0x25120B11);
			goto IL_015e;
			IL_0233:
			int num4 = _671BC22C.BF9F3D1F.FE8E0C9E[1802 + (((~num | num2) > num2 >> (int)num2) ? 1 : 0)](_9F8EAF36.BE93B38F - _9F8EAF37.BE93B38F);
			goto IL_0262;
			IL_02d8:
			if (HasLineOfSight(start, current, cellMap, occupiedCells))
			{
				list.Add(current);
			}
			goto IL_02ef;
			IL_02a7:
			num2 = ((uint)((int)num + -356599408) ^ ~(num % 212)) - 2781768823u;
			int num5 = num4 + num3;
			num += 2949026556u;
			num2 ^= 0x4F1D4AF1;
			if (num5 <= range)
			{
				goto IL_02d8;
			}
			goto IL_02ef;
			IL_019c:
			num = 0x1D4AF1 ^ num2;
			if (-196792523 / (-1364875997 % (int)num - -272762305) != (int)((num2 >> 1) * num2 * 1024594213 + (0xD2163804u ^ num2)))
			{
				num2 = 963466536 + num * 1630312226;
				goto IL_015e;
			}
			goto IL_02d8;
			IL_02ef:
			while (enumerator.MoveNext())
			{
				current = enumerator.Current;
				int mov = current.Mov;
				num = 0u;
				num2 = 1344546805u;
				if (mov == 0)
				{
					continue;
				}
				goto IL_019c;
			}
			if ((0 - (num - num2 / 3758658696u)) % ~(3290456369u / ~num) == 0)
			{
				num2 = (uint)(~((int)num2 >> (int)((num2 | num) ^ num)) ^ 0x267AD077);
				goto IL_015e;
			}
			goto IL_0233;
		}
		finally
		{
			int num6 = 129309749;
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	internal static bool HasLineOfSight(Cell start, Cell end, List<Cell> cells, Dictionary<int, bool> occupiedCells)
	{
		sbyte b = -64;
		return HasLineOfSight(start.CellNumber, end.CellNumber, cells, occupiedCells, (byte)(((short)(b | -231863240) >> (int)((0x1B3581AB | ((uint)b % 247210369u)) % (uint)b)) ^ -1) != 0);
	}

	internal static bool HasLineOfSight(int start, int end, List<Cell> cells, Dictionary<int, bool> occupiedCells, bool countTargetAsBlocker = false)
	{
		if (start == end)
		{
			return true;
		}
		if (cells == null || cells.Count == 0)
		{
			return false;
		}
		_9F8EAF35 start2 = _9F8EAF35.D6056589((uint)start);
		_9F8EAF35 end2 = _9F8EAF35.D6056589((uint)end);
		List<_9F8EAF35> line = GetLine(start2, end2);
		if (line == null)
		{
			return false;
		}
		List<int> list = new List<int>();
		foreach (_9F8EAF35 item in line)
		{
			list.Add((int)item.A984429C);
		}
		if (list.Count == 0)
		{
			return false;
		}
		if (!countTargetAsBlocker)
		{
			_ = list[list.Count - 1];
		}
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i];
			if (num < 0 || num >= cells.Count)
			{
				return false;
			}
			Cell cell = cells[num];
			if (cell == null)
			{
				return false;
			}
			if (cell.Los == 0)
			{
				return false;
			}
		}
		int num2 = (countTargetAsBlocker ? list.Count : (list.Count - 1));
		if (occupiedCells != null && occupiedCells.Count > 0)
		{
			for (int j = 0; j < num2; j++)
			{
				int key = list[j];
				if (occupiedCells.ContainsKey(key))
				{
					return false;
				}
			}
		}
		return true;
	}

	internal static List<_9F8EAF35> GetLine(_9F8EAF35 start, _9F8EAF35 end)
	{
		byte b = 0;
		List<_9F8EAF35> list = default(List<_9F8EAF35>);
		double num5 = default(double);
		double num8 = default(double);
		double num28 = default(double);
		double num6 = default(double);
		double num9 = default(double);
		int num3 = default(int);
		int num15 = default(int);
		double num18 = default(double);
		double num26 = default(double);
		double num29 = default(double);
		int num13 = default(int);
		int num11 = default(int);
		double num31 = default(double);
		int num23 = default(int);
		double num27 = default(double);
		int num17 = default(int);
		double num7 = default(double);
		double num4 = default(double);
		double num10 = default(double);
		double num14 = default(double);
		double num16 = default(double);
		int num2 = default(int);
		double num22 = default(double);
		ushort num = default(ushort);
		int _550A2383 = default(int);
		uint num12 = default(uint);
		while (true)
		{
			int num30;
			int num21;
			double num24;
			double num25;
			int num32;
			int num33;
			int _9F9F5C;
			switch ((uint)b % 66u)
			{
			default:
				list = new List<_9F8EAF35>();
				if (1 >>> (int)b > (b >> 27) / -276873755)
				{
					b = (byte)(((b << 18 == ((b == b) ? 1 : 0)) ? 1 : 0) + (int)b - -1);
					break;
				}
				goto IL_1b2e;
			case 1u:
				b--;
				num5 = (double)start.BE93B38F + 0.5;
				num12 = (byte)(681983234u >> (int)b);
				b = (byte)(-707557029 + (0x2A2C76A7 ^ b));
				break;
			case 2u:
				b = (byte)(1437 + (-753322465 * (int)num12 >> ((b % 320894634) ^ -96947434)));
				num8 = (double)start.E8A39495 + 0.5;
				num12 = (uint)(-24108 - (-905424705 >>> (((uint)(-845413878 << (int)num12) < num12 % num12) ? 1 : 0)));
				if (37362486 - b > (int)(num12 * 448092716))
				{
					b = (byte)(num12 * 1670617642 - 1332277037);
					break;
				}
				goto IL_2739;
			case 3u:
				b = (byte)(3389566699u + (b ^ num12 ^ b));
				goto IL_0220;
			case 4u:
				b = (byte)(((uint)(0x13110CAB & b) ^ (0x78A5FE31 | ~(3693382673u % (uint)b))) - uint.MaxValue);
				num28 = (double)end.E8A39495 + 0.5;
				b = (byte)(num12 / 741848852 >> (int)(num12 ^ (num12 >> (int)b)));
				if ((0 & b) == 0)
				{
					b = (byte)(b - 672886709 + 672886845);
					break;
				}
				goto IL_0a66;
			case 5u:
				b = (byte)((-86 + num) ^ 0x628B);
				num6 = 0.0;
				if ((((uint)((int)num12 / -460362347) % 3633936386u) & 0x61A5770A) >> (int)(num - (num12 | 0x15AE403A)) >= num)
				{
					b = (byte)((17421 - (-240966772 + (1367685779 >>> (int)b))) ^ -442858574);
					break;
				}
				goto IL_26ea;
			case 6u:
				b = (byte)(-430 + (ushort)(num12 >> (int)num12));
				num9 = 0.0;
				num = (ushort)(0x5F9D8A01 ^ (b >> b % 438695102));
				num3 = (int)(0x35F75115 ^ num12);
				do
				{
					num = (ushort)((int)(num12 ^ 0xB162088) / (int)(24836 + num12));
					num15 = ((num >> 8 == num) ? 1 : 0);
				}
				while ((~b & (-854969937 + ((num >> 1) + b))) == 0);
				b = (byte)((-994406211 << (int)b) - -1988812429);
				break;
			case 7u:
				b -= 6;
				if (_671BC22C.BF9F3D1F.FE8E0C9E[(((uint)(2075659286 + (0x593EE090 ^ b)) < (uint)(~num)) ? 1u : 0u) ^ 0x73Eu](num5 - num18) == _671BC22C.BF9F3D1F.FE8E0C9E[-num - -1856](num8 - num28))
				{
					b = (byte)((uint)(1143243562 >>> 1059769737 / ~(b >> (int)b)) >> 1);
					if (num12 * (uint)(short)((b ^ 0x5431CFB0) / ~(b - b)) != 0)
					{
						b = (byte)(((int)(0x320A54B6 | ((uint)(-519327089 & num) % (uint)b)) / (int)(byte)num) ^ 0x320A543B);
						break;
					}
					goto IL_1ec7;
				}
				num12 = (uint)(-138960452 - (int)(num + (num12 ^ num12))) / (uint)(short)b;
				if ((num12 | (uint)((int)num12 >> 31)) == 0)
				{
					goto IL_1375;
				}
				if ((-1321774186 | (sbyte)((-1758115143 & b) + -148745445)) != 0)
				{
					b = (byte)(0x95C73F59u ^ ~(1782104236u / (uint)(num / b)));
					break;
				}
				goto IL_1e4d;
			case 8u:
				b = (byte)(0x44 ^ (1261070511 * b / -742523378));
				num3 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[1854 + num](num5 - num18);
				if (num18 > num5)
				{
					goto IL_04a4;
				}
				num30 = b ^ 0x788890B7 ^ -2022215924;
				goto IL_04ba;
			case 9u:
				b = (byte)((-num >>> 322831010 + (-475173578 >> (int)num) << (int)((uint)b % (uint)b) / (int)num) - -61);
				goto IL_199c;
			case 10u:
				b = (byte)(0x5F960F2C ^ (1603669806 - num));
				goto IL_061d;
			case 11u:
				b = (byte)((int)(1621292u % (uint)(~((int)num12 % (int)num))) % ~b + -7);
				num3 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[-9088 + (num ^ 0x2ABE)](num5 - num18);
				num12 += 1680339129;
				b = (byte)(((((uint)num < (uint)(ushort)num12) ? 1u : 0u) & 0x45u) + 143);
				break;
			case 12u:
				b = (byte)((0 - num12) ^ 0xA4206D8Du);
				if (!(num18 > num5))
				{
					goto IL_0735;
				}
				goto IL_0753;
			case 13u:
				b = (byte)((16u % (uint)((-399480657 >> (int)(num12 - 1602341249)) % ((238498949 >>> (int)b) + (num >>> (int)b)))) ^ 0x77);
				goto IL_0817;
			case 14u:
				b = (byte)(((uint)(b - 145) & ((0x61AEAC12 | num12) / ~num12 * ~(0 - num12))) - 4294967228u);
				goto IL_0a3c;
			case 15u:
				b = (byte)(((-2053850 & (1310809479 << (int)num12 % -124660936)) + (int)((0x61B06B9F ^ (num12 | 0x7D34ED3E)) % (num12 | num))) ^ 0x384596B9);
				num6 = _671BC22C.BF9F3D1F.FE8E0C9E[(int)num12 + -2043785941]((num18 - num5) / (double)num3 * 100.0) / 100.0;
				num15 = (short)(((uint)b & ((num12 == num) ? 1u : 0u)) >> (int)num) ^ 3;
				num12 ^= 0x79D1B95A;
				b = (byte)(b + -20);
				goto IL_0a3c;
			case 16u:
				b = (byte)(((sbyte)((b ^ 0x17B99C05) % b) * -910599745) ^ -258031429);
				goto IL_1d61;
			case 17u:
				if (_671BC22C.BF9F3D1F.FE8E0C9E[1542 + (b >>> 13)](num26) == _671BC22C.BF9F3D1F.FE8E0C9E[num12 + 936925528](num29))
				{
					b = (byte)(0 ^ ((int)(0 - num12) >> 5));
					b = (byte)((b & 0x95) + -126);
					break;
				}
				if (((((uint)b > 1915142414u) ? 1u : 0u) | ~(((uint)num % (uint)num) ^ num)) != 0)
				{
					b = (byte)(~(~((int)num12 % ~b)));
					if ((int)(num12 << (int)b) % (num << (int)num) != 0)
					{
						b = (byte)(24 + (int)(0xA60A2225u | ((uint)num % 2274327737u)) * ((((int)num12 < num % 1904423871) ? 1 : 0) / -1842958286));
						break;
					}
					goto IL_04a4;
				}
				goto IL_159f;
			case 18u:
				b = (byte)(134226083 + ((((int)(num ^ num12) < (int)b) ? 1u : 0u) ^ (0 - (0xD98311B & num12))));
				goto IL_0eed;
			case 19u:
				b = (byte)(num12 - 254 - 3358042902u);
				goto IL_0f94;
			case 20u:
				b = (byte)(4294829520u + (uint)(740951691 << (num | 0x2E1D7E2E)) / (uint)num);
				if (!(num29 < (double)num13))
				{
					num = (ushort)(num + -4883);
					goto IL_1004;
				}
				goto IL_1068;
			case 21u:
				b = (byte)(b - 735716169 - -735716190);
				if (num29 == (double)num13 && num26 < (double)num13)
				{
					num = (ushort)(num - -4883);
					b -= 20;
					goto IL_1068;
				}
				goto IL_10a6;
			case 22u:
				b = (byte)(-1019345786 - (int)num12 - -82421800);
				list.Add(_9F8EAF35._0210A7A2(num11, num13));
				if (331037733 << (((int)(59034417 * (1125579539 / num12)) < 34610739) ? 1 : 0) == 0)
				{
					num = (ushort)(-((num + (-1515416135 - num)) / -64671196));
					b = (byte)((uint)((int)(~num12) - (0x8902A5 ^ (b & num))) % (uint)((ushort)(924566050u % (uint)num) | b) + 4294967264u);
					break;
				}
				goto IL_2739;
			case 23u:
				b = (byte)((1755724041u / (uint)num >> ((2131089413 * num == b) ? 1 : 0)) / (0x4933 | (0 - num12 % b)));
				goto IL_0eed;
			case 24u:
				b = (byte)(0xFFFF53FDu ^ (0 - (uint)num % (uint)((-796007121 << (int)num12) + ((b < -686570323) ? 1 : 0))));
				if (_671BC22C.BF9F3D1F.FE8E0C9E[189391209 + (-57281482 / ((int)num12 >> 27) + (int)((((byte)num > b) ? 1u : 0u) | (uint)(num ^ -197532499)))](num26) == _671BC22C.BF9F3D1F.FE8E0C9E[((((((uint)(b >> 18) < (uint)((int)num12 >> 10)) ? 1 : 0) < ~num + num) ? 1u : 0u) / 434463828u) ^ 0x685](num29))
				{
					if (-1329702728 >>> (b ^ ((-1766775124 - b) & (num % 1426945666))) >= (int)(((num12 == b) ? 1u : 0u) / (uint)num / (0 ^ num12)) % -351982553)
					{
						b = (byte)(0x19 ^ ((num12 * (615840398u % (uint)b) == (uint)(-1090237180 << (int)b)) ? ((short)1) : ((short)0)));
						break;
					}
					goto IL_0b88;
				}
				goto IL_159f;
			case 25u:
				b = (byte)((((int)num12 < (int)(((num12 < 2802267553u) ? 1u : 0u) & (num12 >> (int)b))) ? 1 : 0) - (b >>> 2) - -179);
				goto IL_12e1;
			case 26u:
				b = (byte)((uint)(b / ((b + -1894744940) | -367402189)) ^ (429464865u % (uint)(~num)) ^ 0x19991D8F);
				goto IL_1375;
			case 27u:
				b = (byte)(-502377720 - b + 502377921);
				if (num29 == (double)num13)
				{
					b = (byte)((-341225068 | b) & (657651331 + num));
					b = (byte)((-b >>> (int)((((int)num12 > 1496822938) ? 1u : 0u) % ~(((uint)num % (uint)(~num)) | num))) + 158);
					break;
				}
				goto IL_14c9;
			case 28u:
			{
				b = (byte)(-124030792 + ((int)((num12 << 26) * 1596384317) * ((int)num12 % -1434268537) - -929337290));
				double num34 = num26;
				double num35 = num13;
				b += 44;
				if (num34 < num35)
				{
					goto IL_1428;
				}
				goto IL_14c9;
			}
			case 29u:
				b = (byte)(4294937430u + (((uint)(ushort)(0x782B1997 ^ b) / (uint)(-2034742506 * b)) ^ num));
				goto IL_149a;
			case 30u:
				b = (byte)(~(~num) ^ 0xAF);
				list.Add(_9F8EAF35._0210A7A2(num11, num13));
				b -= 174;
				num = (ushort)(num - -18470);
				if ((uint)(num - -908291770) >= num12 >> (int)b)
				{
					num = (ushort)((uint)(1149871411 + 0 / (int)(num12 | num12)) / 1572831133u);
					b = (byte)(-(b * -760515017) * b + (ushort)(b * 755497739 / -1607843033) + 163);
					break;
				}
				goto IL_2739;
			case 31u:
				b = (byte)((b ^ 0x49B9EF30) - (549317916 * num - 1294702015) + -793150990);
				goto IL_1428;
			case 32u:
				b = (byte)(-1134 + (short)(231081070 >>> (int)((num12 == 882820011) ? ((sbyte)1) : ((sbyte)0))));
				if ((int)(num31 * 100.0) > num23)
				{
					goto IL_1708;
				}
				num12 = (uint)((num - -107620036) | ~(750090006 % num));
				if (319702571 - ~b > ((((num12 - 1326916401 == (uint)num / 2634559403u % ~((uint)num / 3851338629u)) ? 1u : 0u) > (uint)(894970863 >>> (int)(1384772100 / ~(2041471763u / (uint)(~b))))) ? 1 : 0))
				{
					b = (byte)((556051745 >> ((-978048746 - (1855397015 >> (int)num12)) | b)) - 271476);
					break;
				}
				goto IL_23f9;
			case 33u:
				b = (byte)(3 + num12 - 4294964212u);
				list.Add(_9F8EAF35._0210A7A2(num11, (int)_671BC22C.BF9F3D1F.FE8E0C9E[1369 + (byte)(num >> (int)b)](num29)));
				num = (ushort)(num + -25734);
				num12 += 3358046397u;
				if (((uint)(948588678 / ~(b | b)) | num12) - (num12 << (int)num12 % 663835674) == 0)
				{
					num12 = (ushort)(0 - b / num12);
					b = (byte)((0 & b) ^ 0x22);
					break;
				}
				goto IL_2739;
			case 34u:
				b = (byte)((uint)(b >>> 26) / (uint)(~b));
				goto IL_08e6;
			case 35u:
				b = (byte)(((uint)(0x6404449E ^ (-b & 0x3BDFE0F2)) & (((num12 / num12) | 0x95318B84u) >> (int)num12)) ^ 0x204C6);
				if ((int)(num27 * 100.0) >= num17)
				{
					list.Add(_9F8EAF35._0210A7A2(num11, (int)_671BC22C.BF9F3D1F.FE8E0C9E[1542 + (short)(174603433 / (int)num12)](num26)));
					num ^= 0xE48A;
					if (b != 0)
					{
						b = (byte)((int)num12 >> (int)num);
						b = (byte)((num | -1467002054) ^ -1467000037);
						break;
					}
					goto IL_2739;
				}
				num12 = (uint)(2139102478 % ((2880666007u > (uint)b) ? 1 : 0)) & (3357691581u / num12) & num12;
				b = (byte)(~((0x55264AB & (0x40E4F18 | num12)) - 1195156783) + 3167067903u);
				break;
			case 36u:
				b = (byte)((int)(3163053245u % num12 / 3844897434u) >> (int)b >> ~(short)(num + 253063847));
				goto IL_0eed;
			case 37u:
				b = (byte)(44206 + ~(num12 ^ num));
				list.Add(_9F8EAF35._0210A7A2(num11, (int)_671BC22C.BF9F3D1F.FE8E0C9E[(0x3F301DA7 | num12) - 1060116385](num26)));
				if ((-num | 0x178B1200 | (num & 0x7B2E93BC)) >= -num)
				{
					b = (byte)((-267467640 % (1723446060 + (num | -359058680))) ^ (296873367 + b << num + num) ^ -1408318430);
					break;
				}
				goto IL_26c3;
			case 38u:
				b = (byte)((uint)b >> (b & 0x5C9FF00C) % (b % -634994298));
				goto IL_18e5;
			case 39u:
				b = (byte)(-1292326788 / (-529707284 * (int)((uint)b % 1881875381u)) - 3);
				goto IL_08e6;
			case 40u:
				b = (byte)(0x832FB0B ^ (((uint)(~num) & (0 - (num12 - 1308863807))) - 2108228998));
				num13 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(num ^ 0x48845795) - 1216631183](num7);
				goto IL_1c0c;
			case 41u:
				b = (byte)((((uint)((-1767504592 / (int)(~num12)) & num) < (uint)(-2086139743 >>> (int)(short)(num12 << (int)b))) ? 1u : 0u) - 1u);
				num11 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[-1186761225 + (0x46BC900F ^ num)](num4);
				num = (ushort)(num + 1553067433);
				b = (byte)((-1188023607 & num) ^ 0x20A3);
				break;
			case 42u:
				b = (byte)(((-2 == (int)(2486093742u % (uint)num)) ? 1u : 0u) * (uint)num);
				goto IL_1ce8;
			case 43u:
				b = (byte)(((0 - num12) * 119903884 << (int)((uint)num / 4098527432u)) % (~num12 % (b + num12) >> (int)num));
				goto IL_1d61;
			case 44u:
				b = (byte)((num + 915590657) ^ 0x3693BBAA);
				if (num10 == (double)num11)
				{
					num = (ushort)((uint)(num * (490551699 % num >> -1674805108 - (int)num12)) ^ ((uint)(1948855448 / ((3087023538u > (uint)b) ? 1 : 0)) / (uint)(num | -1786708931)));
					b = (byte)((uint)(-416476026 << -1231675595 * (num << (int)num)) % (uint)(-b - 1971395101) + 2740048266u);
					break;
				}
				goto IL_1ec7;
			case 45u:
				b = (byte)(num ^ 0);
				goto IL_1e4d;
			case 46u:
				b = (byte)(936923987 + ((0x393CBBBB | ((int)(num12 | 0xA8A55905u) % ((int)num12 % (int)num))) + (int)num12));
				list.Add(_9F8EAF35._0210A7A2(num11, num13));
				num -= 31758;
				if ((int)(num - num12 % 2678246049u / 3501942280u / num12) < b + -13)
				{
					num = (ushort)((b - 737458584) & ((((int)(1227350928u / (uint)num) > (int)(num12 & b)) ? 1 : 0) - (int)num12));
					b = (byte)((num | -10706320) ^ -10687804);
					break;
				}
				goto IL_2739;
			case 47u:
				b = (byte)(~((int)(2074042413u % (uint)b) / (num - 1529743240)) * ((int)(((uint)b / 1857570960u) & 0xBF7AAB42u) * (num >>> (int)(num12 / num))));
				goto IL_10a6;
			case 48u:
				b = (byte)num;
				num11 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[((byte)num << (int)((uint)(-309843435 * b) / ~(0x7246B66Cu & ((num == -1432660813) ? 1u : 0u)))) + 1669](num4);
				if (0 / (int)(0x73B7B419 ^ (num12 / (uint)(~num))) <= 1836273354 - num)
				{
					b ^= 0x31;
					break;
				}
				goto IL_2297;
			case 49u:
				b = ((949289999u > (uint)(num - b)) ? ((byte)1) : ((byte)0));
				goto IL_20cd;
			case 50u:
				b = (byte)(b - (num12 | (uint)((int)num12 >> (int)num12)) - 21364);
				if (!(num10 < (double)num11))
				{
					goto IL_2141;
				}
				goto IL_21fa;
			case 51u:
				b = (byte)((2318124430u % (uint)(~(1343418174 >> (int)(~((num12 > num12) ? 1u : 0u))))) ^ 0x8A2BC58Eu);
				if (num10 == (double)num11)
				{
					b = (byte)(3384491063u * ((0x81105AB1u ^ ((1402596901 < num) ? 1u : 0u)) << (int)b));
					b = (byte)(0x34 ^ ((-283311433 - (int)num / (int)num12) % (int)(~((num12 ^ num) / 3978354493u)) >> 857881758 % -b));
					break;
				}
				goto IL_2297;
			case 52u:
			{
				b = (byte)(3 ^ ((num12 + num) / 678423771));
				double num19 = num14;
				double num20 = num11;
				b ^= 7;
				if (num19 < num20)
				{
					goto IL_21fa;
				}
				goto IL_2297;
			}
			case 53u:
				b = (byte)(((num >>> (-544349031 >> (int)b)) ^ num) & -577136112);
				num11 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[109157014 + (((-1685731812 >>> (int)num) & -316382416) | -109156000)](num4);
				goto IL_2297;
			case 54u:
				b = (byte)((-1625948784 << ((b % b) | 0x35B8A881) - 855935281) + 40894464);
				list.Add(_9F8EAF35._0210A7A2(num11, num13));
				num = (ushort)(num - -18471);
				if ((b | -2045313012) == ~(num << 26))
				{
					num12 = (uint)(-481595162 / ~b) % (uint)(0 ^ num);
					b = (byte)(1799498056u / (uint)(-1450180318 >> (int)num12) - 4294967241u);
					break;
				}
				goto IL_2739;
			case 55u:
				b = (byte)(num12 * 0);
				goto IL_1c0c;
			case 56u:
				b = (byte)(num12 ^ (798982206u % (uint)(-1836632048 << (int)num)) ^ 0xE7B82C90u);
				goto IL_2739;
			case 57u:
				b = (byte)((uint)(0 % ~b) >> (((b >> 8) & num) << 1));
				if ((int)(num16 * 100.0) >= num17)
				{
					num12 = 0x963FEB0Cu | num12;
					if ((uint)(2117279667 + (-1112645111 / (int)(~num12) << (b * num >> 8))) >= (((uint)b / 2182568733u) & (1822064437 / num12 / (num12 / num12))) - 1119085491)
					{
						goto IL_08e6;
					}
					b = (byte)((-1273646548 & ((1216494738 << b % -1398782785) * ((-147207282 - (int)num12) * -1719266885))) ^ -1543204334);
					break;
				}
				num = (ushort)((uint)((byte)((uint)b / 2099721252u) >>> (1545921156 >>> 18781599 / (int)num12)) / (uint)(-((int)num12 / (int)(0 - num12))));
				b = (byte)(676664 + -((b + 70360008) / 104));
				break;
			case 58u:
				b = (byte)(1495473840 << (int)num12);
				list.Add(_9F8EAF35._0210A7A2((int)_671BC22C.BF9F3D1F.FE8E0C9E[(int)((0x175FBF8 | num12) * 51435145) - -1718288527](num14), num13));
				num12 ^= 0x17D847B1;
				num += 18471;
				if (-6896 * ((int)num12 + (int)num12 / -1542760543) != 0)
				{
					b = (byte)(191u + (((uint)((int)(0 - num12) - -427211132) % (uint)(~b) < 0 - num12) ? 1u : 0u));
					break;
				}
				goto IL_0735;
			case 59u:
				b = (byte)((b | -535715582) - -535715393);
				goto IL_2739;
			case 60u:
				b = ((1461169543 >>> -b < b) ? ((byte)1) : ((byte)0));
				list.Add(_9F8EAF35._0210A7A2((int)_671BC22C.BF9F3D1F.FE8E0C9E[0x606 ^ ((uint)(b << 12) / (uint)(~((int)((uint)b / (uint)(~b)) % ~b)))](num14), num13));
				num = (ushort)(~num12 % (uint)(~(0x7496133E & b)) + (((2417116042u < (uint)num) ? 1u : 0u) & 0x21u));
				b = (byte)((num >>> (int)(num12 & num)) - 55203);
				break;
			case 61u:
				b = (byte)((uint)(0xA8D033D | ((int)b % (int)num12)) / (uint)(~num | ((-1323246557 >>> (int)num) + 1949995572)));
				list.Add(_9F8EAF35._0210A7A2((int)_671BC22C.BF9F3D1F.FE8E0C9E[b + 1542](num10), num13));
				num ^= 0x9FC7;
				num12 -= 2175499121u;
				b = (byte)(~(-b) ^ -63);
				break;
			case 62u:
				b = (byte)(~((b & 0x45142C95) * (b % b)) - -1);
				goto IL_2739;
			case 63u:
				b = (((int)num12 * -475451725 == -1950637420) ? ((byte)1) : ((byte)0));
				num5 = (num5 * 100.0 + num6 * 100.0) / 100.0;
				num = (ushort)num12;
				b = (byte)(num12 - 3358043114u);
				break;
			case 64u:
				b = (byte)((0x430297B3 | num12) + 886587457);
				num8 = (num8 * 100.0 + num9 * 100.0) / 100.0;
				goto IL_27fe;
			case 65u:
				{
					b = (byte)((0x1999319F & num) - 1);
					if (num2 < num3)
					{
						num = 31367;
						if (~((-1154371015 & num) >> num + num >> (int)num) >= (int)((uint)(~num) ^ ((uint)(num ^ num) / ~(((uint)num > (uint)num) ? 1u : 0u))))
						{
							num4 = num5 + num6;
							num = (ushort)(num + 329567159);
							num7 = num8 + num9;
							goto IL_0b88;
						}
						goto IL_1708;
					}
					return list;
				}
				IL_1d61:
				if (!(num10 < (double)num11))
				{
					num ^= 0x3575;
					goto IL_1d74;
				}
				goto IL_1e69;
				IL_23f9:
				num12 = (uint)(0x49D3281F ^ num);
				b = (byte)((-1912976022 & num) ^ 0xBD);
				break;
				IL_1d74:
				if ((0xEF22129Fu | ((uint)((int)num12 * -276176373 + num) % (344717353 - (num12 + num)))) > 2080817085 / num12 << ~(-1632965469 % num))
				{
					b = (byte)(0xB1u ^ (((int)((0x212D01A5 & num12) ^ num) < (b & 0x420A581F) - -702326092) ? 1u : 0u));
					break;
				}
				goto IL_27fe;
				IL_086c:
				num15 = -64 + (b & 0x6043B75A);
				b ^= 0x23;
				num12 ^= 0x5BDF9274;
				b = (byte)(((int)num12 % (int)num << (int)((uint)num % 461873049u / 1080)) + (int)(num12 * ((num12 / 3271978557u) ^ 0x421C50B9)) - -146);
				break;
				IL_27fe:
				num2 += (-207956318 >> (num >>> (int)num12)) - -51989081;
				num12 -= 3358043310u;
				num -= 44205;
				goto IL_282d;
				IL_21fa:
				if ((num12 | 0xEF8264BBu) >= (uint)((b + 1083174151) ^ (num ^ -166232770)) % (uint)(-2146900439 ^ b))
				{
					b = (byte)(185 + 87757204 % (int)(~((((uint)num < 3834953273u) ? 1u : 0u) & (uint)((num >> (int)b) - -1835643126 * b))));
					break;
				}
				goto IL_0937;
				IL_0735:
				num21 = (((int)(((num12 * b) | (uint)(b >>> (int)num)) << 3) > 1477784583) ? 1 : 0) ^ -1;
				goto IL_0789;
				IL_0753:
				num = (ushort)((uint)(-((int)num12 >> (int)num)) % (uint)b);
				num21 = (int)(1043268268u / (uint)(180373057 >> (96013197 >>> (b ^ b)))) + -47381;
				num ^= 1;
				goto IL_0789;
				IL_2297:
				b = (byte)((b - -1220594902) / (int)(~((uint)num % (uint)(~num))) << ~((int)b % (int)num12 % ~num));
				if ((0x415E818 & num12) != (uint)(num * -1741189999))
				{
					b = (byte)(0xA1E6 ^ (0xA1D0 | num));
					break;
				}
				goto IL_1fc2;
				IL_0789:
				num6 = num21;
				b = (byte)(num ^ ~(b | -181247080));
				if ((((2298650044u % (uint)((int)num12 % (int)b)) ^ (uint)(sbyte)(~num12)) | 0x20214C80) >= 0 - (uint)b / 320229287u << (int)num >> 25)
				{
					b = (byte)(((1418755754 >> (int)num12 < (num >> 3) - -b) ? 1u : 0u) % (0 - num12) - 4294967283u);
					break;
				}
				goto IL_0753;
				IL_1004:
				b = (byte)(short)num12;
				if (b <= b - -1610613761)
				{
					b = (byte)((((num & num) >> (int)num) & num) - -153);
					break;
				}
				goto IL_1e4d;
				IL_1c0c:
				if (_671BC22C.BF9F3D1F.FE8E0C9E[((num12 + num12) & (uint)(num | ((num >> 1) - num))) ^ 0x606](num14) == _671BC22C.BF9F3D1F.FE8E0C9E[(int)(num12 * 0) - -1542](num10))
				{
					b = (byte)((((uint)(((int)num12 % (int)num12) & 0x2DBF1420) > 2501708041u) ? 1u : 0u) & (uint)(-b));
					b = (byte)(0x78A89B26 ^ num ^ 0x78A89B8B);
					break;
				}
				if ((b & 0x703BC2A) != (sbyte)b)
				{
					goto IL_08e6;
				}
				goto IL_1fc2;
				IL_1fc2:
				if (_671BC22C.BF9F3D1F.FE8E0C9E[(int)num12 / ((-972183775 | b) >>> (1763689022 >>> (int)num << 13)) + 1669](num14) == _671BC22C.BF9F3D1F.FE8E0C9E[num ^ 0x6C95FB98 ^ 0x6C95FD1D](num10))
				{
					if ((uint)((sbyte)((int)num12 / (int)num12) >>> (((((uint)num < (uint)num) ? 1 : 0) < 1619845423) ? 1 : 0)) < (uint)(-(ushort)(sbyte)(-1044716117 / (int)num12)))
					{
						b = (byte)(((sbyte)num12 - num << 19 >> 2047800492 * -((int)num12 + -610932220)) + 168116);
						break;
					}
					goto IL_0753;
				}
				if ((int)(num22 * 100.0) <= num23)
				{
					list.Add(_9F8EAF35._0210A7A2((int)_671BC22C.BF9F3D1F.FE8E0C9E[(int)num12 % (int)num12 - -1542](num10), num13));
					num ^= 0x4827;
					if (num12 / (uint)((-342909289 / ~b) & -1934927866) == 0)
					{
						goto IL_0eed;
					}
					b = (byte)(-16575 ^ (0x2C7BAA64 | (b - num)));
					break;
				}
				goto IL_23f9;
				IL_199c:
				if (-(-1122690158 & num) != 0)
				{
					goto IL_1068;
				}
				if (num15 == (((-272818785 >>> (int)num) & -1) ^ (num >>> (int)num12)) % (int)(~(823828249u / (uint)(~(num << 29)))) - -3)
				{
					num14 = _671BC22C.BF9F3D1F.FE8E0C9E[(int)num12 + (2098964528 >>> (int)(num12 % (uint)(~num) >> 4)) + 934875885](num5 * 100.0 + num6 * 50.0) / 100.0;
					if ((0xA7AB38A8u ^ ((uint)(num | 0x5805F630) / 28205988u)) == 0)
					{
						goto IL_0937;
					}
					goto IL_1a38;
				}
				if ((uint)(-1700526188 ^ num) > num12 - 170204967)
				{
					goto IL_18e5;
				}
				if (1619384843u / (uint)(1127361667 << (int)num12) != 0)
				{
					goto IL_0817;
				}
				goto IL_26c3;
				IL_1e4d:
				num24 = num14;
				num25 = num11;
				num = (ushort)(num - -60841);
				if (num24 < num25)
				{
					num ^= 0x3575;
					goto IL_1e69;
				}
				goto IL_1ec7;
				IL_0220:
				num18 = (double)end.BE93B38F + 0.5;
				num = (ushort)(b + 25312);
				b = (byte)((0x4D8B7224 & ((-342567489 | b) + 0 / (int)num12)) + -1233145888);
				break;
				IL_1e69:
				num = (ushort)((~b << (int)(((int)num12 > (int)num) ? ((sbyte)1) : ((sbyte)0))) & (b * num + (-936091490 * b >>> (int)num)));
				num11 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(num12 ^ (uint)(~num | 0x64145B92)) * (2687908650u / ~((num == -1365048944) ? 1u : 0u)) + 1669](num4);
				num ^= 0xEDA9;
				goto IL_1ec7;
				IL_26c3:
				_550A2383 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(-1098828004 >>> (int)num12 >>> (int)num) - 193534](num4);
				goto IL_26ea;
				IL_0817:
				num9 = _671BC22C.BF9F3D1F.FE8E0C9E[1103103620 + (((b % 394734478) & (b & num)) | ((int)num12 * (-735090931 << (int)num12) >>> (int)num12 % (int)num12))]((num28 - num8) / (double)num3 * 100.0) / 100.0;
				goto IL_086c;
				IL_10a6:
				b = (((int)num12 % 1016069286 == (int)((((uint)(~num) > (uint)num) ? 1u : 0u) / (uint)(~num))) ? ((byte)1) : ((byte)0));
				b = (byte)(154 + (uint)(-num) / 833509268u * ((uint)(1645349820 * b + (int)num12) % num12));
				break;
				IL_1068:
				num13 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[1669 + (((int)num12 - (((uint)(b >>> 30) < (uint)b) ? 1 : 0) > (ushort)(103602676 - num)) ? 1 : 0)](num7);
				b = (byte)(b - -20);
				num ^= 0x131D;
				goto IL_10a6;
				IL_1ec7:
				num = (ushort)(2081845 + b);
				b = (byte)((b << (-902047871 & (b << 15)) - -784552447 / ~b) + 46);
				break;
				IL_04a4:
				num30 = (int)(0 - (((1420771585u > (uint)num) ? 1u : 0u) ^ 0x843542B6u) - 2076884296);
				goto IL_04ba;
				IL_1708:
				if ((uint)(-1708130672 >> (int)num) / (uint)(~(b & (1084494596 - b))) << 15 == 0)
				{
					b = (byte)(((159 + b % 243499688) & -num) ^ 0xB4);
					break;
				}
				goto IL_20cd;
				IL_04ba:
				num6 = num30;
				if (!(num28 > num8))
				{
					num32 = ((1866214964 + -79702383 % b >> (int)(num12 % num)) & 0x702D56AA) - 1613497513;
				}
				else
				{
					num12 = (uint)(-9734 ^ num);
					num32 = (int)((329326117 / (num12 / b)) & ((uint)((int)num12 >> 23) / 813885325u)) + -4;
					num12 ^= 0xCA0888EEu;
				}
				num9 = num32;
				num12 = (uint)(num >> 10);
				num15 = (-1213862112 | num) - -1213862112;
				if (-b == (num - -1288828014) / ~(-368261960 / (-1743435901 - (int)num12)))
				{
					b = (byte)((813697026 + b << 16) % -1780);
					b = (byte)((-(num ^ 0x349B9D2D) % ((-1723923951 % (int)(~num12)) | -46238916)) ^ -4074135);
					break;
				}
				goto IL_0a3c;
				IL_20cd:
				if (num14 == (double)num11)
				{
					b = (byte)((uint)(b - b) / 226850944u);
					if ((b & 0) <= ~((-493265370 ^ num ^ (b % -1808397138)) + num))
					{
						b = (byte)(1364564090 + (((int)(b + num12) / (int)num12) ^ b) - 1364564041);
						break;
					}
					goto IL_12e1;
				}
				goto IL_2141;
				IL_0937:
				if (!(num28 > num8))
				{
					b = (byte)((uint)(-122441040 - b + num) >> (int)b);
					num33 = -640181628 + (int)((uint)(0x26286523 ^ b) % ~((uint)b / uint.MaxValue));
				}
				else
				{
					num33 = (sbyte)num;
					b += 87;
				}
				num9 = num33;
				num12 = (uint)(((int)(~num12 % num12) - -num) ^ (-483144130 >> (num >>> 5) >>> 1));
				b = (byte)(5 ^ (-((b >>> (int)b) | -687761742) >>> 26));
				break;
				IL_2141:
				num = (ushort)(b & 1);
				b = (byte)(~(-954419190 + (0x281CDB26 | b)) - 281439388);
				break;
				IL_08e6:
				if ((uint)(b | 0x6C836E) <= ((uint)(1151179811 / (int)(0xE018DC03u & num12)) ^ ((num12 % num12) ^ num12)))
				{
					num3 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(sbyte)(-7472767 >> (int)(num12 - 1520102675)) - -1862](num8 - num28);
					goto IL_0937;
				}
				goto IL_18e5;
				IL_2739:
				if (1884707430u % (uint)(~(b & -1482844386)) <= (num12 ^ ((1532637015u > (uint)(num % num)) ? 1u : 0u)))
				{
					b = (byte)(1121039176 + ((int)(b / num12 >> (int)num) * -(~b) + -1121039113));
					break;
				}
				goto IL_137f;
				IL_12e1:
				num13 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(int)b + (((int)(1697397513 + num12) > ~(num - 86713747)) ? 1 : 0) - -1494](num7);
				num = (ushort)((int)(num12 / (uint)(~(num >> (int)(num12 % b)))) >> 23);
				if ((b | 3) != 0)
				{
					b = (byte)(((int)(0x6AB6C208 | (0x2EA4D30E & num12)) - (num >> num / ~num) << 30) ^ -2147483490);
					break;
				}
				goto IL_1ce8;
				IL_0a3c:
				b = (byte)((sbyte)(1480193816 >> (int)num) % num);
				num23 = (-6 << (int)(0 - ~num12)) + 15 + num3 / (0 ^ num ^ 3);
				goto IL_0a66;
				IL_0b88:
				num12 = (byte)(num & 0x1793A0BD);
				num11 = 0 >> (int)(num12 << (int)num12);
				goto IL_0ba2;
				IL_0ba2:
				num = (ushort)((-283336622 % (int)num12 >> (int)(num12 % num12)) % (int)(~((-549498209 == num) ? 1u : 0u)));
				num13 = (int)(num12 / 3750569863u);
				num12 = (uint)(-936923986 | num) >> 2089554968 * (num >>> (int)(num * num12));
				if (num12 >> (int)(byte)num12 == 0)
				{
					goto IL_1375;
				}
				if (num15 != (-936935422 ^ ((int)num12 >> (int)num12 << (int)num12)))
				{
					goto IL_199c;
				}
				num26 = _671BC22C.BF9F3D1F.FE8E0C9E[(int)(~((uint)((int)num12 / -644501059) % 68966832u) | 0xAC13B3D0u) - -1671](num8 * 100.0 + num9 * 50.0) / 100.0;
				num29 = _671BC22C.BF9F3D1F.FE8E0C9E[(((1133511306 << (int)num12) / 655131962) & (num / ~num >> (1127371046 << (int)num))) - -1542](num8 * 100.0 + num9 * 150.0) / 100.0;
				if ((((int)num > (int)(num12 >> (int)(num12 | 0x58ABD121))) ? 1 : 0) / (int)(~((-1883657158 + (int)num12 == 864644745 % (int)num12) ? 1u : 0u)) == 0)
				{
					num31 = _671BC22C.BF9F3D1F.FE8E0C9E[(sbyte)(byte)num ^ 0x606](_671BC22C.BF9F3D1F.FE8E0C9E[((int)num12 >> 25) % 613371937 + 1883](_671BC22C.BF9F3D1F.FE8E0C9E[0x609 ^ (-7170625 + (-1338866502 & -num) >>> -1919314267 + (int)(3275786455u % num12))](num26) * 100.0 - num26 * 100.0)) / 100.0;
					num27 = _671BC22C.BF9F3D1F.FE8E0C9E[(int)(((num12 < num12) ? 1u : 0u) << ((num >> 1 > ((num < -1893343082) ? 1 : 0) / 924413722) ? 1 : 0)) - -1669](_671BC22C.BF9F3D1F.FE8E0C9E[((num12 / (uint)(~num) == 1236619578) ? 1 : 0) - ((-273938796 < num) ? 1 : 0) + 1856](_671BC22C.BF9F3D1F.FE8E0C9E[905264746 + ((int)(num + (0x22D64B5 ^ num12)) >> (int)num)](num29) * 100.0 - num29 * 100.0)) / 100.0;
					num = (ushort)(num12 - 1);
					if ((-67023960 + num) * -1173291752 != (((uint)(-((int)num12 / 489446462) - num % 1158864925 / (int)num12) < (uint)(num << (int)num)) ? 1 : 0))
					{
						num11 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[num - -1 - 42664](num4);
						b = (byte)(num / ~(-82158824 * ((num12 < num) ? 1 : 0)) << ((int)((uint)num / 2553769607u) >> (int)((num12 - 740414119) | 0x7D1EDA15)));
						break;
					}
					goto IL_0220;
				}
				goto IL_1ce8;
				IL_18e5:
				list.Add(_9F8EAF35._0210A7A2(num11, (int)_671BC22C.BF9F3D1F.FE8E0C9E[0x242F9FE2 ^ ((uint)(607143569 - (num - b)) % (uint)(~b))](num29)));
				num12 ^= 0xC827ACAEu;
				num ^= 0xE48A;
				if ((-729047386 | (short)num) * ((sbyte)num / ~b * -205716335) > b >>> (-896605021 << (int)b))
				{
					b = b;
					b = (byte)((int)(4053509281u % (uint)(~(-1021062499 * b))) / (int)(~((num12 | num) >> 13)) + -418);
					break;
				}
				goto IL_2739;
				IL_137f:
				if (!(num29 < (double)num13))
				{
					goto IL_1388;
				}
				goto IL_1428;
				IL_1ce8:
				if (num14 == (double)num11)
				{
					num = (ushort)(b - 85337892);
					if (-(-num) >= (int)((uint)(((3048309006u < (uint)num) ? 1 : 0) * (num ^ b)) / ((uint)(num | -1239828585) % 4053189910u)))
					{
						b = (byte)(~(1847285402 >>> (int)num12) / num - -177);
						break;
					}
					goto IL_0ba2;
				}
				goto IL_1d74;
				IL_1375:
				if (num26 == (double)num13)
				{
					goto IL_137f;
				}
				goto IL_1388;
				IL_1388:
				b = (byte)num12;
				b = (byte)(27 + (-1937245271 & num));
				break;
				IL_0a66:
				num17 = (0x61 ^ (num >>> 14)) - num3 / (int)((((uint)(b >> ((int)num12 >> (int)num12)) < 212865584 / ~num12) ? 1u : 0u) ^ 2u);
				num2 = (int)(((60359947 % ~num12 - b) | (uint)((-1802008783 >> (int)num12) % num * (num >>> (int)num))) - 60359947);
				if ((0x2227E006 ^ (b << (int)(num12 / ~num12))) << (int)((uint)(31441875 >> (int)(1828873535 - num12)) / ~(num12 >> (int)(~num12))) == (int)(((b ^ num12) >> 19) | 0xFFFFFFFEu))
				{
					num12 = (uint)(num << (int)b);
					b = (byte)(((uint)(0x46CC63EB ^ (-1551413717 / ~b + num)) / 3736299711u) ^ 0x10);
					break;
				}
				goto IL_282d;
				IL_1428:
				if (671088640 >> ((247376771 / (int)(num12 << (int)num)) & -1621818691) != 0)
				{
					num = (ushort)(3492684294u - num12);
					b = (byte)((uint)num / (uint)(-1741672946 / (772158761 >>> (int)b) >> (b << 6)) - 4294967135u);
					break;
				}
				goto IL_23f9;
				IL_0eed:
				num13 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[1641209 + ((uint)(((int)num12 >> 10) ^ 0x3A86F5A5) / 1354209056u - (num12 >> 11))](num7);
				num = (ushort)(1210516382 >> (int)num >> -1279286769 + (int)(num12 % 887743));
				if ((((int)num12 - -972742771 > -1170304359) ? 1 : 0) * (398234891 + b) % 1478850452 != 0)
				{
					b = (byte)(51u % (uint)(short)b % (uint)(~(((b & -544578140) - 382725203) / -1002847943)) - 4294967196u);
					break;
				}
				goto IL_149a;
				IL_26ea:
				_9F9F5C = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(45 + num12) ^ 0xC827AADDu](num7);
				if ((uint)((int)num12 >> (int)(num12 << 6)) < num12 - num12 >> 22)
				{
					goto IL_061d;
				}
				list.Add(_9F8EAF35._0210A7A2(_550A2383, _9F9F5C));
				num ^= 0x4827;
				b = 0;
				goto IL_2739;
				IL_149a:
				num13 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(sbyte)(633058490 + b) - -4174285 - 4172847](num7);
				num = (ushort)(num + -30040);
				goto IL_14c9;
				IL_14c9:
				num = (ushort)(-(0 - ((1032982564u > (uint)num) ? 1 : 0)) | 1);
				if ((uint)(0 + b) < (uint)num)
				{
					goto IL_0eed;
				}
				b = (byte)(659177965 + -659177803 * num);
				break;
				IL_282d:
				if (0 - num12 >> (int)(b & num12 & (byte)num) >> (int)((uint)(b / num) % uint.MaxValue) == 0)
				{
					b = (byte)((-b >>> (((sbyte)(num ^ b) < (b ^ -805091284)) ? 1 : 0)) - -197);
					break;
				}
				goto IL_1a38;
				IL_061d:
				if (!(_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)((int)(539533594 % num12) >> (int)num) / 1517679208u % (uint)(~b & -835951733)) - -1855](num5 - num18) > _671BC22C.BF9F3D1F.FE8E0C9E[~(-(-1734063670 >> (b >>> (int)num12))) - -1734065526](num8 - num28)))
				{
					goto IL_08e6;
				}
				if (num - -1449990859 != (short)((607070789 >> (851099415 << (int)num12)) % 957938725))
				{
					b = (byte)(11 + ((b - num >>> ((num > -1291491199) ? 1 : 0)) * num + num % num));
					break;
				}
				goto IL_0f94;
				IL_1b2e:
				num16 = _671BC22C.BF9F3D1F.FE8E0C9E[0x685 ^ (num & 1)](_671BC22C.BF9F3D1F.FE8E0C9E[1991190847 + (-607868817 >>> ((int)(num12 ^ num12) >> (0x28BEC324 | num)) << 9)](_671BC22C.BF9F3D1F.FE8E0C9E[0x685 ^ -((num & 0x6824A084) * 842146465)](num10) * 100.0 - num10 * 100.0)) / 100.0;
				b = (byte)(0u - ((-602389851 > num) ? 1u : 0u));
				b = (byte)(-b - -172);
				break;
				IL_1a38:
				num10 = _671BC22C.BF9F3D1F.FE8E0C9E[(464587659 % ~num >>> -1261984059 + num) ^ 0x606](num5 * 100.0 + num6 * 150.0) / 100.0;
				if ((0x598CAAA & num) * (byte)(num12 >> (int)num12) != 0)
				{
					goto IL_086c;
				}
				num22 = _671BC22C.BF9F3D1F.FE8E0C9E[((int)(num / ~(num12 / 4153901486u)) >> (int)(num12 & 0)) - -1542](_671BC22C.BF9F3D1F.FE8E0C9E[(((int)num12 / -1962934272 < (num | 0x1F2CEA1D)) ? 1 : 0) + 1854](_671BC22C.BF9F3D1F.FE8E0C9E[(((int)(num / (num12 | num12)) * (((uint)(num / ~num) > (uint)num) ? 1 : 0) == 1973510311) ? 1 : 0) - -1542](num14) * 100.0 - num14 * 100.0)) / 100.0;
				goto IL_1b2e;
				IL_0f94:
				if (num26 == (double)num13)
				{
					num = (ushort)(-1196289251 ^ num);
					if ((uint)b % (uint)num != 0)
					{
						b = (byte)(((((num12 == num12 - 1346176416) ? 1 : 0) % -1530947010 < 2101437702) ? 1u : 0u) - 4294967145u);
						break;
					}
					goto IL_0a66;
				}
				goto IL_1004;
				IL_159f:
				b = (byte)((int)num12 % (int)(~((uint)num % (uint)num)));
				b = (byte)(87341997 % ~b + 164);
				break;
			}
		}
	}
}
