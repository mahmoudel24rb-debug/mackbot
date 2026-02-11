// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.DofusLibrary.Common.Fight.HexGridHelper
using System.Collections.Generic;

internal static class HexGridHelper
{
	internal static List<_9F8EAF35> GetHexDirections(int y)
	{
		if (y % 2 == 0)
		{
			int num = 0;
			return new List<_9F8EAF35>
			{
				new _9F8EAF35(-271462961 + (0x102E3232 | num), (num >> num) / ~(num & (num ^ 0x53FF9557)), ((-1991889676 < -(1074721983 % ~(num * -1331242610))) ? 1 : 0) - 1),
				new _9F8EAF35((1661249430 + (-2122051793 - (0x344122EF | ~num))) ^ -460802362, (int)((((uint)(~num) > 455797646u) ? 1u : 0u) | (uint)num) - (num << num + -242392773) * num - 2, 423448983 % ~(num << 10) * -1884694601 + num),
				new _9F8EAF35(-1 ^ (num * 334854780), (num << ((num > num) ? 1 : 0)) * (758127155 << num) - 1, num ^ (num >>> 6)),
				new _9F8EAF35((num >> 7) % (int)((uint)(num | 0x49042D0A) % (uint)(~(num >> 25)) << num) + -1, -1 + ((-207120857 < ~(-1030016458 + (-383216484 & num))) ? 1 : 0), num),
				new _9F8EAF35((((num | num) ^ 0x595054A3) & num) + (int)((uint)num / 6160u) + -1, (num * 45) ^ 1, (int)((uint)(num & num) % 1386553902u)),
				new _9F8EAF35(17 - num - 17, ((num > (-567326679 ^ (1520017305 / ~num))) ? 1 : 0) ^ 1, (num >> 18) / ~num >> (num | num) % (int)(~((1595553317 < num) ? 1u : 0u)) * (2084007699 % ~num >> num))
			};
		}
		sbyte b = -26;
		return new List<_9F8EAF35>
		{
			new _9F8EAF35(((int)((uint)b / (uint)b / (uint)(0x2FA1BB83 ^ b)) >> (int)(sbyte)(b << (int)b)) - -1, (b >> 12) + 1, (int)((uint)(b - b) % 4273468211u)),
			new _9F8EAF35((b - 4 / (-511678041 >> (int)b)) ^ -25, (((((uint)b / 1773454486u) & (uint)((1571879702 + b) % (1455035412 >> (int)b))) > (uint)b) ? 1 : 0) + -1, ((b & b) >>> (b >>> (int)b)) - 1),
			new _9F8EAF35(618203075 + -618203075 % ((-1459012320 & b) ^ ((b & b) * (int)((uint)b / (uint)b))), (-860355538 & (b ^ (b >> 31))) * (-1928644690 >>> (int)b) + -295790321, (int)((uint)b / (uint)(-877550417 % (b >>> 29) << (int)((uint)b / 1544146970u)))),
			new _9F8EAF35(-1674 + ((int)((uint)(-249759603 | b) ^ ((uint)b / (uint)b >> 22)) - ((b << (int)b) + b)), (int)((((1983218850u % (uint)b) | (uint)(b | (b & 0x478B07B2))) >> (int)((uint)(~b) % 335544340u)) - 127), (byte)b - (314041358 >>> (b & -836106240)) + (int)(((uint)b % 2704878523u) & (3608424374u % (uint)b)) - 1129062650),
			new _9F8EAF35(b - (b - b - ((b == b) ? 1 : 0) % (654925061 / b)) - -25, 0x6E3AF8EF ^ ((b + -1062275449) * b), (-b * (613350700 >> (int)b)) ^ 0xEDA16C8),
			new _9F8EAF35((int)(4294967285u % (uint)(b % -1 + (-1451207911 >>> (int)b)) - 29328244), -18612345 + (1191190161 >> (-509159793 >> 70039596 % b)), (917057181 < (((0x3E2AF9A5 & b) < b) ? 1 : 0)) ? 1 : 0)
		};
	}

	internal static int ManhattanDistance(_9F8EAF35 a, _9F8EAF35 b)
	{
		ushort num = 16406;
		return _671BC22C.BF9F3D1F.FE8E0C9E[(int)((((uint)num % (uint)(-num)) | 0x84C0BDB0u) / (uint)(num / -139984599 + (~num << (int)num))) - -1803](a.BE93B38F - b.BE93B38F) + _671BC22C.BF9F3D1F.FE8E0C9E[(0 - ((3586037509u > (uint)num) ? 1 : 0) + 0) % (int)(~((uint)num % (uint)num)) + 1803](a.E8A39495 - b.E8A39495);
	}
}
