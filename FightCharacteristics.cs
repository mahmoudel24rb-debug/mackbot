// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.FightCharacteristics
using System.Collections.Generic;
using System.Linq;
using DofusLibrary.Common.Fight;
using DofusLibrary.Common.Repository;
using Google.Protobuf.Collections;
using JitsuriProto;

internal class FightCharacteristics : Dictionary<CharacteristicKeyword, FightCharacteristic>
{
	internal new string ToString()
	{
		return BB97372F._1E072A97(_671BC22C.BF9F3D1F.FE8E0C9E[1813](), base.Values.Select(delegate(FightCharacteristic fc)
		{
			uint num = 3763674279u;
			return fc.ToString();
		}));
	}

	internal void FromProtobufCharacteristics(RepeatedField<JitsuriCharacterCharacteristic> characterCharacteristics)
	{
		using IEnumerator<JitsuriCharacterCharacteristic> enumerator = characterCharacteristics.GetEnumerator();
		while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
		{
			JitsuriCharacterCharacteristic current = enumerator.Current;
			int num = _671BC22C.BF9F3D1F.FE8E0C9E[2025](current);
			if (A996FE3D.CABFD5B4.TryGetValue(num, out var value))
			{
				_2B16532D _2B16532D2 = A996FE3D._34B4919D[value];
				int value2 = -1;
				int baseValue = 0;
				if (_671BC22C.BF9F3D1F.FE8E0C9E[1584](current) == JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase.Detailed)
				{
					value2 = _671BC22C.BF9F3D1F.FE8E0C9E[321](_671BC22C.BF9F3D1F.FE8E0C9E[1068](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[450](_671BC22C.BF9F3D1F.FE8E0C9E[1068](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[1914](_671BC22C.BF9F3D1F.FE8E0C9E[1068](current));
					baseValue = _671BC22C.BF9F3D1F.FE8E0C9E[450](_671BC22C.BF9F3D1F.FE8E0C9E[1068](current));
				}
				else if (_671BC22C.BF9F3D1F.FE8E0C9E[1584](current) == JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase.Value)
				{
					value2 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[158](_671BC22C.BF9F3D1F.FE8E0C9E[883](current));
					baseValue = (int)_671BC22C.BF9F3D1F.FE8E0C9E[158](_671BC22C.BF9F3D1F.FE8E0C9E[883](current));
				}
				else if (_671BC22C.BF9F3D1F.FE8E0C9E[1584](current) == JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase.Usable)
				{
					value2 = _671BC22C.BF9F3D1F.FE8E0C9E[1514](_671BC22C.BF9F3D1F.FE8E0C9E[457](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[1185](_671BC22C.BF9F3D1F.FE8E0C9E[457](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[1606](_671BC22C.BF9F3D1F.FE8E0C9E[457](current));
					baseValue = _671BC22C.BF9F3D1F.FE8E0C9E[1185](_671BC22C.BF9F3D1F.FE8E0C9E[457](current));
				}
				FightCharacteristic fightCharacteristic = new FightCharacteristic(value, num, _2B16532D2._9B3B2A01, value2);
				fightCharacteristic.BaseValue = baseValue;
				base[value] = fightCharacteristic;
			}
		}
	}

	public FightCharacteristics()
	{
		ushort num = 17161;
		do
		{
			base._002Ector();
		}
		while ((-668745205 | num) == 0);
	}
}
