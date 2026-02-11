// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.InstanceContainer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using DofusLibrary.Common;

internal static class InstanceContainer
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public string characterName;

		public _003C_003Ec__DisplayClass10_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetInstanceDataByCharacterName_003Eb__0(KeyValuePair<int, InstanceData> kvp)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](kvp.Value.CharacterData.CharacterName, characterName);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0
	{
		public long characterId;

		public _003C_003Ec__DisplayClass11_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetInstanceDataByCharacterId_003Eb__0(KeyValuePair<int, InstanceData> kvp)
		{
			return kvp.Value.CharacterData.CharacterId == characterId;
		}
	}

	private static int _currentProcessId;

	internal static Dictionary<int, InstanceData> ProcessIdToInstanceData;

	[CompilerGenerated]
	private static Action<int> CurrentProcessIdChanged;

	internal static int CurrentProcessId
	{
		get
		{
			uint num = 912759912u;
			return _currentProcessId;
		}
		private set
		{
			byte b = 56;
			if (_currentProcessId == value)
			{
				return;
			}
			b = (byte)(0x7EC0CA68 & b);
			if ((short)b % ((sbyte)b | b) >> (int)((uint)b / (uint)b) - (1477226031 >> (((uint)b < 194331926u) ? 1 : 0)) == 0)
			{
				_currentProcessId = value;
				Action<int> currentProcessIdChanged = CurrentProcessIdChanged;
				if (currentProcessIdChanged != null)
				{
					currentProcessIdChanged(_currentProcessId);
					b += 16;
				}
			}
		}
	}

	internal static event Action<int> _67986817
	{
		[CompilerGenerated]
		add
		{
			Action<int> action = CurrentProcessIdChanged;
			Action<int> action2;
			byte b;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)_671BC22C.BF9F3D1F.FE8E0C9E[243](action2, value);
				b = 0;
				action = Interlocked.CompareExchange(ref CurrentProcessIdChanged, value2, action2);
			}
			while ((object)action != action2 || -545139806 / -(-827503858 + (b << 24)) == (int)(~((2020970698u > (uint)(b - b)) ? 1u : 0u)));
		}
		[CompilerGenerated]
		remove
		{
			short num = 31774;
			num = (short)(-31078 + num);
			Action<int> action2 = default(Action<int>);
			Action<int> value2 = default(Action<int>);
			Action<int> action4 = default(Action<int>);
			while (true)
			{
				switch ((uint)num % 3u)
				{
				default:
					num = (short)(-((-num >> -536259814 - num) | -142068547) + 31773);
					action2 = CurrentProcessIdChanged;
					goto IL_0045;
				case 1u:
					num = (short)(1 + ~(num % -num));
					action2 = Interlocked.CompareExchange(ref CurrentProcessIdChanged, value2, action4);
					num = (short)(num | 0x379E128C);
					break;
				case 2u:
					{
						Action<int> action = action2;
						Action<int> action3 = action4;
						num += 27026;
						if ((object)action == action3)
						{
							return;
						}
						goto IL_0045;
					}
					IL_0045:
					action4 = action2;
					value2 = (Action<int>)_671BC22C.BF9F3D1F.FE8E0C9E[0x1F844148 ^ (0x1F843DA7 ^ num)](action4, value);
					num = (short)((int)(((uint)(-1891802204 << (int)num) / 3137759246u) & 0) & (num + -1892296171 % (-794557772 % num)));
					num = (short)((num >>> (num >>> (0xA2A5E9B & num))) - -12946);
					break;
				}
			}
		}
	}

	static InstanceContainer()
	{
		_currentProcessId = -1;
		ProcessIdToInstanceData = new Dictionary<int, InstanceData>();
		AddInstanceData(-1);
	}

	internal static void AddInstanceData(int processId)
	{
		if (Contains(processId))
		{
			int num = 1;
			throw _671BC22C.BF9F3D1F.FE8E0C9E[num + 1099]("Cette ProcessId existe déjà.");
		}
		ProcessIdToInstanceData[processId] = new InstanceData(processId);
		CurrentProcessId = processId;
	}

	internal static bool Contains(int processId)
	{
		uint num = 0u;
		return ProcessIdToInstanceData.ContainsKey(processId);
	}

	internal static InstanceData GetByProcessId(int processId)
	{
		byte b = 0;
		if (!Contains(processId))
		{
			throw _671BC22C.BF9F3D1F.FE8E0C9E[(b & -341223420) ^ 0x44C]("Aucune instance n'existe pour ce processId.");
		}
		return ProcessIdToInstanceData[processId];
	}

	internal static InstanceData GetCurrentInstanceData()
	{
		uint num = 2307291192u;
		ProcessIdToInstanceData.TryGetValue(CurrentProcessId, out InstanceData value);
		return value;
	}

	internal static InstanceData GetInstanceDataByCharacterName(string characterName)
	{
		_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass10_0();
		CS_0024_003C_003E8__locals2.characterName = characterName;
		try
		{
			return ProcessIdToInstanceData.FirstOrDefault<KeyValuePair<int, InstanceData>>((KeyValuePair<int, InstanceData> kvp) => _671BC22C.BF9F3D1F.FE8E0C9E[720](kvp.Value.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.characterName)).Value;
		}
		catch
		{
			return null;
		}
	}

	internal static InstanceData GetInstanceDataByCharacterId(long characterId)
	{
		_003C_003Ec__DisplayClass11_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass11_0();
		CS_0024_003C_003E8__locals2.characterId = characterId;
		try
		{
			return ProcessIdToInstanceData.FirstOrDefault<KeyValuePair<int, InstanceData>>((KeyValuePair<int, InstanceData> kvp) => kvp.Value.CharacterData.CharacterId == CS_0024_003C_003E8__locals2.characterId).Value;
		}
		catch
		{
			return null;
		}
	}

	internal static bool RemoveInstanceData(int processId)
	{
		if (!ProcessIdToInstanceData.ContainsKey(processId))
		{
			byte b = 185;
			return (byte)(-322571826 / ~b - 1734257) != 0;
		}
		return ProcessIdToInstanceData.Remove(processId);
	}
}
