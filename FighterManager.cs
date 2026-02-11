// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.FighterManager
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Ankama.Dofus.Server.Game.Protocol;
using DofusLibrary.Common;
using DofusLibrary.Common.Character;
using DofusLibrary.Common.Fight;
using DofusLibrary.Common.JsonClasses;
using DofusLibrary.Common.Map;
using DofusLibrary.Common.PathFinder;
using DofusLibrary.Common.Repository;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using JitsuriProto;
using Newtonsoft.Json;

internal class FighterManager
{
	internal abstract class ActorFighter
	{
		[CompilerGenerated]
		private int _003CCellId_003Ek__BackingField;

		[CompilerGenerated]
		private DofusLibrary.Common.Fight.FightCharacteristics _003CCharacteristics_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CIsAlive_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CTeam_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CIsSummon_003Ek__BackingField;

		internal int Gid { get; set; }

		internal long ActorId { get; set; }

		internal int CellId
		{
			[CompilerGenerated]
			get
			{
				return _003CCellId_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				sbyte b = 0;
				do
				{
					_003CCellId_003Ek__BackingField = value;
				}
				while ((uint)(b % -364874636) >> 9 != 0);
			}
		}

		internal DofusLibrary.Common.Fight.FightCharacteristics Characteristics
		{
			[CompilerGenerated]
			get
			{
				ushort num = 38314;
				return _003CCharacteristics_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				byte b = 51;
				if ((((uint)(599681 >>> (int)b) > (uint)(-56623208 - (int)((uint)b / (uint)b))) ? 1 : 0) * (-b + 85086976) == 0)
				{
					do
					{
						_003CCharacteristics_003Ek__BackingField = value;
					}
					while ((b ^ (-502626389 | b)) == 0);
				}
			}
		}

		internal bool IsAlive
		{
			[CompilerGenerated]
			get
			{
				uint num = 2670160957u;
				return _003CIsAlive_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				int num = -704643072;
				_003CIsAlive_003Ek__BackingField = value;
			}
		}

		internal int Team
		{
			[CompilerGenerated]
			get
			{
				return _003CTeam_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				short num = -11993;
				_003CTeam_003Ek__BackingField = value;
			}
		}

		internal int TeamId
		{
			get
			{
				uint num = 0u;
				return Team;
			}
		}

		internal bool IsSummon
		{
			[CompilerGenerated]
			get
			{
				uint num = 1766582420u;
				return _003CIsSummon_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				uint num = 940837268u;
				_003CIsSummon_003Ek__BackingField = value;
			}
		}

		internal int PermanentDamage { get; set; }

		internal ActorFighter(long actorId, int cellId, DofusLibrary.Common.Fight.FightCharacteristics characteristics, bool isAlive, int team, bool isSummon)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
			ushort num = 58798;
			if ((uint)(-num + (byte)(1765677988u % (uint)num) - num) >= ((((1519471529 + num >> 30) & (0x47293F13 & num)) > -1566769765) ? 1u : 0u))
			{
				num = (ushort)((num << -838466755 % ~(num >> 21)) + -42352);
				goto IL_005a;
			}
			goto IL_01e5;
			IL_00e3:
			Unsafe.SkipInit(out short num2);
			do
			{
				CellId = cellId;
			}
			while ((-914118944 ^ ((int.MinValue | (num2 + num)) / ((num2 & -2010513994) << 20))) == 0);
			num = (ushort)(1746171711 % num - 5881);
			goto IL_005a;
			IL_005a:
			while (true)
			{
				switch ((uint)num % 6u)
				{
				default:
					num = (ushort)(0x3FF71A10 ^ (num - -1073201024));
					ActorId = actorId;
					num2 = (short)(168180112 - num);
					if ((uint)(-460678366 / (int)(3793543844u / (uint)num2)) >= (((uint)(733815995 % (num2 >> (int)num)) > (uint)num) ? 1u : 0u))
					{
						num = (ushort)(((((num >>> (int)num2) | num) << 23) / (num2 ^ -58579072)) ^ 0xADBB);
						continue;
					}
					return;
				case 1u:
					break;
				case 2u:
					num = (ushort)(((((uint)(num % num2) / 4174980442u == (uint)((-868991230 | num) / num)) ? 1 : 0) * (int)num2) ^ 0xE5AE);
					Characteristics = characteristics;
					num = ((133 < (int)(((((uint)num2 < (uint)num) ? 1u : 0u) << 1686458684 % num2) + 0)) ? ((ushort)1) : ((ushort)0));
					if (num2 * (~num2 % -1448743510) != 0)
					{
						num = (ushort)(-28287 ^ -((num2 << 14) / num2));
						continue;
					}
					return;
				case 3u:
					num = (ushort)((num2 >> (int)num) - 10736);
					do
					{
						IsAlive = isAlive;
					}
					while (num / (0x2695510E & num2) >= (int)(2719576855u / (uint)(num - -298749916)));
					num = (ushort)(num / (sbyte)(num2 << (int)num) - 1351180326 - -1351227748);
					continue;
				case 4u:
					goto IL_01cc;
				case 5u:
					num = (ushort)((short)(934319269 >>> (int)(short)num) - 24872);
					return;
				}
				break;
			}
			num += 14325;
			goto IL_00e3;
			IL_01e5:
			Team = team;
			if ((uint)(~(num2 & num) >>> 10) / ~((758334344u % (uint)(~(num2 % num2))) & (uint)(num2 << num - num2)) == 0)
			{
				num = (ushort)(-1588312579 * (num + num2) / num + -1617628720);
				goto IL_005a;
			}
			goto IL_00e3;
			IL_01cc:
			num = (ushort)(1 ^ ((uint)((num ^ num2) / 1697626516 * -735028430) % 1779832976u));
			goto IL_01e5;
		}

		internal float GetLifePointPercentage()
		{
			int num = Characteristics[CharacteristicKeyword.HitPoints].Value + Characteristics[CharacteristicKeyword.Vitality].Value - PermanentDamage;
			int value = Characteristics[CharacteristicKeyword.HitPointLoss].Value;
			float num2 = (float)(num + value) / (float)num * 100f;
			if (num2 > 100f)
			{
				return 100f;
			}
			return num2;
		}
	}

	internal class Attacker : ActorFighter
	{
		internal Attacker(long actorId, int cellId, DofusLibrary.Common.Fight.FightCharacteristics characteristics, bool isAlive, bool isSummon)
			: base(actorId, cellId, characteristics, isAlive, 0, isSummon)
		{
			base.Gid = (int)actorId;
		}
	}

	internal class Defender : ActorFighter
	{
		internal new bool IsSummon { get; set; }

		internal int MonsterGrade { get; set; }

		internal int MonsterLevel { get; set; }

		internal Defender(long actorId, int cellId, DofusLibrary.Common.Fight.FightCharacteristics characteristics, bool isAlive, int monsterGid, int monsterGrade, int monsterLevel, bool isSummon = false)
		{
			short num = -9045;
			if (174 * num != 0)
			{
				num = (short)((-651776234 | ~(0 << (1998423739 >> (int)num))) ^ -8973);
				Unsafe.SkipInit(out byte b);
				while (true)
				{
					switch ((uint)num % 4u)
					{
					default:
						num = (short)((-1132170211 >>> num / -58334818 >>> -141111379 + (0xC17CE0F | (num / -1615785830))) + -9056);
						base._002Ector(actorId, cellId, characteristics, isAlive, (0x55EAE1F0 ^ num) + 1441448614, isSummon);
						if (num < 827755871 + num)
						{
							num += 29654;
							continue;
						}
						goto end_IL_0030;
					case 1u:
						num = (short)(-29654 ^ num);
						base.Gid = monsterGid;
						b = (byte)(((0x523C5800 & num) >> 2050518844 % num) % (num & (-1760830933 >> (int)num)) >> (747965373 << (num / -1600433528 >>> (int)num)));
						if ((uint)num >= (uint)(byte)(589206155 / ~b) / (uint)((num | num) << 0))
						{
							num = (short)((((uint)num % 539181495u) ^ (uint)((-1717756782 + b >> (int)b) - (-1005049545 | num))) + 2037147413);
							continue;
						}
						return;
					case 2u:
						break;
					case 3u:
						goto end_IL_0030;
					}
					num = (short)((num | 0x78A04134) - 2023790867);
					MonsterGrade = monsterGrade;
					int num2 = num << num * b;
					if ((uint)(1302031402 * -num2) < (uint)num)
					{
						continue;
					}
					goto IL_0173;
					continue;
					end_IL_0030:
					break;
				}
			}
			MonsterLevel = monsterLevel;
			goto IL_0173;
			IL_0173:
			IsSummon = isSummon;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass24_0
	{
		public IEnumerable<long> attackerIds;

		public _003C_003Ec__DisplayClass24_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CWaitForAllMembersInFight_003Eb__1(long id)
		{
			return attackerIds.Contains(id);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass27_0
	{
		public long actorId;

		public _003C_003Ec__DisplayClass27_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleEntitiesDispositionEvent_003Eb__0(Attacker actor)
		{
			return actor.ActorId == actorId;
		}

		internal bool _003CHandleEntitiesDispositionEvent_003Eb__1(Defender actor)
		{
			return actor.ActorId == actorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass30_0
	{
		public JitsuriActorPositionInformation actor;

		public _003C_003Ec__DisplayClass30_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFightSynchronizeEvent_003Eb__0(Attacker attacker)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[444](actor) == attacker.ActorId;
		}

		internal bool _003CHandleFightSynchronizeEvent_003Eb__1(Defender defender)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[444](actor) == defender.ActorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass31_0
	{
		public JitsuriFightRefreshCharacterStatsEvent ev;

		public _003C_003Ec__DisplayClass31_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFightRefreshCharacterStatsEvent_003Eb__0(Attacker attacker)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[46](ev) == attacker.ActorId;
		}

		internal bool _003CHandleFightRefreshCharacterStatsEvent_003Eb__1(Defender defender)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[46](ev) == defender.ActorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass34_0
	{
		public Dictionary<int, bool> occupiedCells;

		public _003C_003Ec__DisplayClass34_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFightPlacementPossiblePositionsEvent_003Eb__0(int e)
		{
			return !occupiedCells.ContainsKey(e);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass38_0
	{
		public IReadOnlyCollection<int> wantedTargetCells;

		public Func<_9F8EAF35, bool> _003C_003E9__0;

		public _003C_003Ec__DisplayClass38_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CFindBestRepositionForCastByMP_003Eb__0(_9F8EAF35 mp)
		{
			return wantedTargetCells.Contains(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass39_0
	{
		public Attacker attacker;

		public _003C_003Ec__DisplayClass39_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CUpdateMapInformation_003Eb__0(JitsuriActorPositionInformation act)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[444](act) == attacker.ActorId;
		}

		internal bool _003CUpdateMapInformation_003Eb__1(JitsuriActorPositionInformation actor)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[444](actor) == attacker.ActorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass39_1
	{
		public Defender defender;

		public _003C_003Ec__DisplayClass39_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CUpdateMapInformation_003Eb__2(JitsuriActorPositionInformation actor)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[444](actor) == defender.ActorId;
		}

		internal bool _003CUpdateMapInformation_003Eb__3(JitsuriActorPositionInformation act)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[444](act) == defender.ActorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass40_0
	{
		public UserDefinedSpell userDefinedSpell;

		public Func<ActorFighter, bool> _003C_003E9__7;

		public _003C_003Ec__DisplayClass40_0()
		{
			ushort num = 2954;
			_671BC22C.BF9F3D1F.FE8E0C9E[-(num + ((-73352519 * num) ^ (num - 891006372))) + 1179573017](this);
		}

		internal bool _003CHandleFightTurnEvent_003Eb__6(JitsuriSpellItem e)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[84](e) == userDefinedSpell.SpellId;
		}

		internal bool _003CHandleFightTurnEvent_003Eb__7(ActorFighter t)
		{
			return userDefinedSpell.TargetHpCondition.IsValid(t.GetLifePointPercentage());
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass40_1
	{
		public DetailedSpellLevel spellLevel;

		public Dictionary<int, int> spellUsedOnActors;

		public Func<Defender, bool> _003C_003E9__12;

		public Func<Defender, bool> _003C_003E9__13;

		public Func<Defender, bool> _003C_003E9__14;

		public Func<Attacker, bool> _003C_003E9__16;

		public Func<Attacker, bool> _003C_003E9__17;

		public _003C_003Ec__DisplayClass40_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFightTurnEvent_003Eb__12(Defender e)
		{
			if (e.IsAlive)
			{
				if (spellLevel.MaxCastPerTarget != 0)
				{
					return spellUsedOnActors.GetValueOrDefault((int)e.ActorId) < spellLevel.MaxCastPerTarget;
				}
				return true;
			}
			return false;
		}

		internal bool _003CHandleFightTurnEvent_003Eb__13(Defender e)
		{
			if (e.IsAlive && e.IsSummon)
			{
				if (spellLevel.MaxCastPerTarget != 0)
				{
					return spellUsedOnActors.GetValueOrDefault((int)e.ActorId) < spellLevel.MaxCastPerTarget;
				}
				return true;
			}
			return false;
		}

		internal bool _003CHandleFightTurnEvent_003Eb__14(Defender e)
		{
			if (e.IsAlive && !e.IsSummon)
			{
				if (spellLevel.MaxCastPerTarget != 0)
				{
					return spellUsedOnActors.GetValueOrDefault((int)e.ActorId) < spellLevel.MaxCastPerTarget;
				}
				return true;
			}
			return false;
		}

		internal bool _003CHandleFightTurnEvent_003Eb__16(Attacker a)
		{
			if (a.IsAlive && a.IsSummon)
			{
				if (spellLevel.MaxCastPerTarget != 0)
				{
					return spellUsedOnActors.GetValueOrDefault((int)a.ActorId) < spellLevel.MaxCastPerTarget;
				}
				return true;
			}
			return false;
		}

		internal bool _003CHandleFightTurnEvent_003Eb__17(Attacker a)
		{
			if (a.IsAlive && !a.IsSummon)
			{
				if (spellLevel.MaxCastPerTarget != 0)
				{
					return spellUsedOnActors.GetValueOrDefault((int)a.ActorId) < spellLevel.MaxCastPerTarget;
				}
				return true;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass40_2
	{
		public Attacker myself;

		public Dictionary<int, bool> occupiedCells;

		public HashSet<int> wantedTargetCells;

		public ActorFighter ennemyToTarget;

		public _003C_003Ec__DisplayClass40_1 CS_0024_003C_003E8__locals1;

		public _003C_003Ec__DisplayClass40_2()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFightTurnEvent_003Eb__15(Attacker a)
		{
			if (a.IsAlive && a.ActorId != myself.ActorId)
			{
				if (CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget != 0)
				{
					return CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)a.ActorId) < CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget;
				}
				return true;
			}
			return false;
		}

		internal bool _003CHandleFightTurnEvent_003Eb__8(_9F8EAF35 e)
		{
			return !occupiedCells.ContainsKey((int)e.A984429C);
		}

		internal bool _003CHandleFightTurnEvent_003Eb__9(_9F8EAF35 mp)
		{
			return wantedTargetCells.Contains(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495));
		}

		internal int _003CHandleFightTurnEvent_003Eb__10(_9F8EAF35 mp)
		{
			return _4CAC0493._52884196.E230AC25(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495), ennemyToTarget.CellId);
		}

		internal bool _003CHandleFightTurnEvent_003Eb__27(_9F8EAF35 mp)
		{
			return wantedTargetCells.Contains(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495));
		}

		internal int _003CHandleFightTurnEvent_003Eb__28(_9F8EAF35 mp)
		{
			return _4CAC0493._52884196.E230AC25(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495), ennemyToTarget.CellId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass40_3
	{
		public HashSet<long> forceIds;

		public _003C_003Ec__DisplayClass40_3()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFightTurnEvent_003Eb__21(ActorFighter t)
		{
			return forceIds.Contains(t.Gid);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass40_4
	{
		public HashSet<long> excludeIds;

		public _003C_003Ec__DisplayClass40_4()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFightTurnEvent_003Eb__25(ActorFighter t)
		{
			return !excludeIds.Contains(t.Gid);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass42_0
	{
		public int spellId;

		public Dictionary<int, bool> occupiedCells;

		public _003C_003Ec__DisplayClass42_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetSpellPossibleCellsOnCellId_003Eb__0(JitsuriSpellItem e)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[84](e) == spellId;
		}

		internal bool _003CGetSpellPossibleCellsOnCellId_003Eb__2(int e)
		{
			return !occupiedCells.ContainsKey(e);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass43_0
	{
		public int spellId;

		public _003C_003Ec__DisplayClass43_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetSpellZoneOnCellId_003Eb__0(JitsuriSpellItem e)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[84](e) == spellId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass45_0
	{
		public long actorId;

		public _003C_003Ec__DisplayClass45_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CMoveActorIdTo_003Eb__0(Attacker a)
		{
			return a.ActorId == actorId;
		}

		internal bool _003CMoveActorIdTo_003Eb__1(Defender d)
		{
			return d.ActorId == actorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass46_0
	{
		public JitsuriGameActionFightEvent ev;

		public _003C_003Ec__DisplayClass46_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleGameActionFightEvent_003Eb__0(Defender d)
		{
			return d.ActorId == _671BC22C.BF9F3D1F.FE8E0C9E[257](ev).TargetId;
		}

		internal bool _003CHandleGameActionFightEvent_003Eb__1(Attacker a)
		{
			return a.ActorId == _671BC22C.BF9F3D1F.FE8E0C9E[257](ev).TargetId;
		}

		internal bool _003CHandleGameActionFightEvent_003Eb__2(Defender d)
		{
			return d.ActorId == _671BC22C.BF9F3D1F.FE8E0C9E[1775](ev).TargetId;
		}

		internal bool _003CHandleGameActionFightEvent_003Eb__3(Attacker a)
		{
			return a.ActorId == _671BC22C.BF9F3D1F.FE8E0C9E[1775](ev).TargetId;
		}

		internal bool _003CHandleGameActionFightEvent_003Eb__4(Defender d)
		{
			return d.ActorId == _671BC22C.BF9F3D1F.FE8E0C9E[1314](ev).TargetId;
		}

		internal bool _003CHandleGameActionFightEvent_003Eb__5(Attacker a)
		{
			return a.ActorId == _671BC22C.BF9F3D1F.FE8E0C9E[1314](ev).TargetId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass48_0
	{
		public long actorId;

		public _003C_003Ec__DisplayClass48_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleMapMovementEvent_003Eb__0(Attacker actor)
		{
			return actor.ActorId == actorId;
		}

		internal bool _003CHandleMapMovementEvent_003Eb__1(Defender actor)
		{
			return actor.ActorId == actorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass66_0
	{
		public int cellId;

		public _003C_003Ec__DisplayClass66_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CIsValidPositionForTreasure_003Eb__1(Attacker attacker)
		{
			return attacker.CellId == cellId;
		}

		internal bool _003CIsValidPositionForTreasure_003Eb__2(Defender defender)
		{
			return defender.CellId == cellId;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CFightTurnFinishRequest_003Ed__55 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		private void MoveNext()
		{
			FighterManager fighterManager = _003C_003E4__this;
			bool result;
			try
			{
				try
				{
					FightTurnFinishRequest _642D = _671BC22C.BF9F3D1F.FE8E0C9E[1485]();
					Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
					_5E33BC89._67B8C522(any, fighterManager._messageHandler.F1A6D798[_79AC42A1.FightTurnFinishRequest]);
					_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
					Any _83237E3A = any;
					fighterManager._messageHandler._051BFF9F(_83237E3A);
					result = true;
				}
				catch (TaskCanceledException ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
					result = false;
				}
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForgeAndSendChallengeReadyRequest_003Ed__57 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		private void MoveNext()
		{
			FighterManager fighterManager = _003C_003E4__this;
			bool result;
			try
			{
				ChallengeReadyRequest _642D = _671BC22C.BF9F3D1F.FE8E0C9E[137]();
				Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
				_5E33BC89._67B8C522(any, fighterManager._messageHandler.F1A6D798[_79AC42A1.ChallengeReadyRequest]);
				_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
				Any _83237E3A = any;
				fighterManager._messageHandler._051BFF9F(_83237E3A);
				try
				{
					result = true;
				}
				catch (TaskCanceledException ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
					result = false;
				}
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForgeAndSendChallengeValidateRequest_003Ed__58 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		public int challengeId;

		private TaskAwaiter<GameMessage> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			bool result;
			try
			{
				Task<GameMessage> task = default(Task<GameMessage>);
				if (num != 0)
				{
					Func<GameMessage, bool> e586CB2B = delegate(GameMessage msg)
					{
						if (_671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) != GameMessage.ContentOneofCase.Event)
						{
							return false;
						}
						return _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](msg))), CS_0024_003C_003E8__locals5._messageHandler.F1A6D798[_79AC42A1.SequenceEndEvent]) ? true : false;
					};
					task = CS_0024_003C_003E8__locals5._messageHandler._8E0F0412(e586CB2B, 7000);
					JitsuriChallengeValidateRequest jitsuriChallengeValidateRequest = _671BC22C.BF9F3D1F.FE8E0C9E[1624]();
					_6F0DDB3D.DA39809C(jitsuriChallengeValidateRequest, challengeId);
					JitsuriChallengeValidateRequest _642D = jitsuriChallengeValidateRequest;
					Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
					_5E33BC89._67B8C522(any, CS_0024_003C_003E8__locals5._messageHandler.F1A6D798[_79AC42A1.ChallengeValidateRequest]);
					_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
					Any _83237E3A = any;
					CS_0024_003C_003E8__locals5._messageHandler._051BFF9F(_83237E3A);
				}
				try
				{
					TaskAwaiter<GameMessage> awaiter;
					if (num != 0)
					{
						awaiter = task.GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
					}
					else
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<GameMessage>);
						num = (_003C_003E1__state = -1);
					}
					result = _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](awaiter.GetResult()))), CS_0024_003C_003E8__locals5._messageHandler.F1A6D798[_79AC42A1.SequenceEndEvent]);
				}
				catch (TaskCanceledException ex)
				{
					_1E3B359C._6E3CC3BA(ex.ToString());
					result = false;
				}
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForgeAndSendFightPlacementPositionRequest_003Ed__59 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		public int cellId;

		public long actorId;

		private TaskAwaiter<GameMessage> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			bool result;
			try
			{
				Task<GameMessage> task = default(Task<GameMessage>);
				if (num != 0)
				{
					Func<GameMessage, bool> e586CB2B = delegate(GameMessage msg)
					{
						if (_671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) != GameMessage.ContentOneofCase.Event)
						{
							return false;
						}
						Event _99874684 = _671BC22C.BF9F3D1F.FE8E0C9E[160](msg);
						if (_671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_99874684)), CS_0024_003C_003E8__locals6._messageHandler.F1A6D798[_79AC42A1.EntityDispositionErrorEvent]))
						{
							return true;
						}
						return _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_99874684)), CS_0024_003C_003E8__locals6._messageHandler.F1A6D798[_79AC42A1.EntitiesDispositionEvent]) ? true : false;
					};
					task = CS_0024_003C_003E8__locals6._messageHandler._8E0F0412(e586CB2B);
					JitsuriFightPlacementPositionRequest jitsuriFightPlacementPositionRequest = _671BC22C.BF9F3D1F.FE8E0C9E[713]();
					B00AF19B._6DBF4B86(jitsuriFightPlacementPositionRequest, cellId);
					_5C09EA99.B5075598(jitsuriFightPlacementPositionRequest, actorId);
					FightPlacementPositionRequest _642D = _018E928B._7C324B00<FightPlacementPositionRequest, JitsuriFightPlacementPositionRequest>(jitsuriFightPlacementPositionRequest);
					Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
					_5E33BC89._67B8C522(any, CS_0024_003C_003E8__locals6._messageHandler.F1A6D798[_79AC42A1.FightPlacementPositionRequest]);
					_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
					Any _83237E3A = any;
					CS_0024_003C_003E8__locals6._messageHandler._051BFF9F(_83237E3A);
				}
				try
				{
					TaskAwaiter<GameMessage> awaiter;
					if (num != 0)
					{
						awaiter = task.GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
					}
					else
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<GameMessage>);
						num = (_003C_003E1__state = -1);
					}
					result = _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](awaiter.GetResult()))), CS_0024_003C_003E8__locals6._messageHandler.F1A6D798[_79AC42A1.EntitiesDispositionEvent]);
				}
				catch (TaskCanceledException ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
					result = false;
				}
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForgeAndSendFightReadyRequest_003Ed__60 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		private void MoveNext()
		{
			FighterManager fighterManager = _003C_003E4__this;
			bool result;
			try
			{
				FightReadyRequest fightReadyRequest = _671BC22C.BF9F3D1F.FE8E0C9E[1171]();
				_7039C8AA.D62B5FBA(fightReadyRequest, FAB6B02E: true);
				FightReadyRequest _642D = fightReadyRequest;
				Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
				_5E33BC89._67B8C522(any, fighterManager._messageHandler.F1A6D798[_79AC42A1.FightReadyRequest]);
				_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
				Any _83237E3A = any;
				fighterManager._messageHandler._051BFF9F(_83237E3A);
				try
				{
					result = true;
				}
				catch (TaskCanceledException ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
					result = false;
				}
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForgeAndSendFightTurnReadyRequest_003Ed__61 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		private void MoveNext()
		{
			FighterManager fighterManager = _003C_003E4__this;
			bool result;
			try
			{
				FightTurnReadyRequest _642D = _671BC22C.BF9F3D1F.FE8E0C9E[1420]();
				Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
				_5E33BC89._67B8C522(any, fighterManager._messageHandler.F1A6D798[_79AC42A1.FightTurnReadyRequest]);
				_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
				Any _83237E3A = any;
				fighterManager._messageHandler._051BFF9F(_83237E3A);
				try
				{
					result = true;
				}
				catch (TaskCanceledException ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
					result = false;
				}
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForgeAndSendGameActionFightCastRequest_003Ed__63 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public int spellId;

		public FighterManager _003C_003E4__this;

		public int cellId;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private GameMessage _003CreceivedMessage_003E5__2;

		private TaskAwaiter<GameMessage> _003C_003Eu__2;

		private TaskAwaiter _003C_003Eu__3;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			bool result;
			try
			{
				TaskAwaiter<bool> awaiter;
				if (num != 0)
				{
					if ((uint)(num - 1) <= 2u)
					{
						goto IL_0160;
					}
					if (spellId != 413)
					{
						goto IL_00be;
					}
					awaiter = _598ED02C.D5BBC0B8("can_useArchimonster").GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = awaiter;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						return;
					}
				}
				else
				{
					awaiter = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
				}
				if (awaiter.GetResult())
				{
					goto IL_00be;
				}
				CS_0024_003C_003E8__locals15.Log(new global::ED33D63D<string, global::_620DAB8E<string>>("permission.required", new global::_620DAB8E<string>("can_useArchimonster")), "gathering", D32842AE._543E5320.B3B830AD);
				result = true;
				goto end_IL_000e;
				IL_00be:
				Func<GameMessage, bool> e586CB2B = delegate(GameMessage msg)
				{
					if (_671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) != GameMessage.ContentOneofCase.Event)
					{
						return false;
					}
					Event _99874684 = _671BC22C.BF9F3D1F.FE8E0C9E[160](msg);
					if (_671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_99874684)), CS_0024_003C_003E8__locals15._messageHandler.F1A6D798[_79AC42A1.TextInformationEvent]))
					{
						TextInformationEvent textInformationEvent = _671BC22C.BF9F3D1F.FE8E0C9E[976]().ParseFrom(_671BC22C.BF9F3D1F.FE8E0C9E[1343](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_99874684)));
						if (B50EBF90.A926AFA5(textInformationEvent) == 171)
						{
							CS_0024_003C_003E8__locals15.Verbose("Probleme de portée....");
						}
						if (B50EBF90.A926AFA5(textInformationEvent) == 174)
						{
							CS_0024_003C_003E8__locals15.Verbose("Probleme de ldv....");
						}
						return A18A8DA8._1587CB27(textInformationEvent) == 1;
					}
					return _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_99874684)), CS_0024_003C_003E8__locals15._messageHandler.F1A6D798[_79AC42A1.SequenceEndEvent]) && _671BC22C.BF9F3D1F.FE8E0C9E[1319](_671BC22C.BF9F3D1F.FE8E0C9E[1724]().ParseFrom(_671BC22C.BF9F3D1F.FE8E0C9E[1343](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_99874684)))) == 0;
				};
				Task<GameMessage> task = CS_0024_003C_003E8__locals15._messageHandler._8E0F0412(e586CB2B, 2000);
				JitsuriGameActionFightCastRequest jitsuriGameActionFightCastRequest = _671BC22C.BF9F3D1F.FE8E0C9E[1682]();
				_243B9334.AE85E815(jitsuriGameActionFightCastRequest, cellId);
				_7A023D81.F0AAA72E(jitsuriGameActionFightCastRequest, spellId);
				GameActionFightCastRequest _642D = _018E928B._7C324B00<GameActionFightCastRequest, JitsuriGameActionFightCastRequest>(jitsuriGameActionFightCastRequest);
				Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
				_5E33BC89._67B8C522(any, CS_0024_003C_003E8__locals15._messageHandler.F1A6D798[_79AC42A1.GameActionFightCastRequest]);
				_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
				Any _83237E3A = any;
				CS_0024_003C_003E8__locals15._messageHandler._051BFF9F(_83237E3A);
				goto IL_0160;
				IL_0160:
				DefaultInterpolatedStringHandler CEA3933A;
				try
				{
					TaskAwaiter<GameMessage> awaiter2;
					TaskAwaiter _6D28EB9F;
					GameMessage result2;
					switch (num)
					{
					default:
						awaiter2 = task.GetAwaiter();
						if (!awaiter2.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = awaiter2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
							return;
						}
						goto IL_01ca;
					case 1:
						awaiter2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<GameMessage>);
						num = (_003C_003E1__state = -1);
						goto IL_01ca;
					case 2:
						_6D28EB9F = _003C_003Eu__3;
						_003C_003Eu__3 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_02fa;
					case 3:
						{
							_6D28EB9F = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter);
							num = (_003C_003E1__state = -1);
							goto IL_0392;
						}
						IL_0392:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						break;
						IL_01ca:
						result2 = awaiter2.GetResult();
						_003CreceivedMessage_003E5__2 = result2;
						CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](82, 2);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "ForgeAndSendGameActionFightCastRequest -> SequenceEnd received for spell ");
						CEA3933A.AppendFormatted(spellId);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, " on cell ");
						CEA3933A.AppendFormatted(cellId);
						CS_0024_003C_003E8__locals15.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
						switch (CS_0024_003C_003E8__locals15._instanceData.CharacterParameters.CombatSpeed)
						{
						case CombatSpeed.Rapide:
							break;
						case CombatSpeed.Human:
							goto IL_0312;
						default:
							goto end_IL_0163;
						}
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__3 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
						goto IL_02fa;
						IL_0312:
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 3);
							_003C_003Eu__3 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
						goto IL_0392;
						IL_02fa:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						break;
						end_IL_0163:
						break;
					}
					result = _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](_003CreceivedMessage_003E5__2))), CS_0024_003C_003E8__locals15._messageHandler.F1A6D798[_79AC42A1.SequenceEndEvent]);
				}
				catch (TaskCanceledException ex)
				{
					if (!CS_0024_003C_003E8__locals15._instanceData.CharacterData.IsFighting)
					{
						CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](64, 1);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "CastRequest timeout but IsFighting=false -> return true (spell ");
						CEA3933A.AppendFormatted(spellId);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ")");
						CS_0024_003C_003E8__locals15.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
						result = true;
					}
					else
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[376]();
						_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex.ToString());
						CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](55, 3);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "CastRequest TaskCanceledException for spell ");
						CEA3933A.AppendFormatted(spellId);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, " on cell ");
						CEA3933A.AppendFormatted(cellId);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ": ");
						_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, _671BC22C.BF9F3D1F.FE8E0C9E[2117](ex));
						CS_0024_003C_003E8__locals15.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
						result = false;
					}
				}
				catch (Exception ex2)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex2.ToString());
					CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](43, 3);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "CastRequest EXCEPTION for spell ");
					CEA3933A.AppendFormatted(spellId);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, " on cell ");
					CEA3933A.AppendFormatted(cellId);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ": ");
					CEA3933A.AppendFormatted(ex2);
					CS_0024_003C_003E8__locals15.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
					result = false;
				}
				end_IL_000e:;
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForgeAndSendSurrend_003Ed__64 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		private TaskAwaiter<GameMessage> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			bool result;
			try
			{
				if (num == 0)
				{
					goto IL_00b5;
				}
				Task<GameMessage> task = default(Task<GameMessage>);
				if (CS_0024_003C_003E8__locals5._instanceData.CharacterData.IsFighting)
				{
					Func<GameMessage, bool> e586CB2B = delegate(GameMessage msg)
					{
						if (_671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) != GameMessage.ContentOneofCase.Event)
						{
							return false;
						}
						return _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](msg))), CS_0024_003C_003E8__locals5._messageHandler.F1A6D798[_79AC42A1.FightEndEvent]) ? true : false;
					};
					task = CS_0024_003C_003E8__locals5._messageHandler._8E0F0412(e586CB2B, 10000);
					ContextQuitRequest _642D = _671BC22C.BF9F3D1F.FE8E0C9E[1873]();
					Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
					_5E33BC89._67B8C522(any, CS_0024_003C_003E8__locals5._messageHandler.F1A6D798[_79AC42A1.ContextQuitRequest]);
					_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](_642D));
					Any _83237E3A = any;
					CS_0024_003C_003E8__locals5._messageHandler._051BFF9F(_83237E3A);
					goto IL_00b5;
				}
				result = true;
				goto end_IL_000e;
				IL_00b5:
				try
				{
					TaskAwaiter<GameMessage> awaiter;
					if (num != 0)
					{
						awaiter = task.GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
					}
					else
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<GameMessage>);
						num = (_003C_003E1__state = -1);
					}
					awaiter.GetResult();
					result = true;
				}
				catch (TaskCanceledException ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
					result = false;
				}
				end_IL_000e:;
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CHandleChallengeAddEvent_003Ed__23 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncVoidMethodBuilder _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		private TaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				_ = 2;
				try
				{
					TaskAwaiter _6D28EB9F;
					TaskAwaiter<bool> awaiter;
					switch (num)
					{
					case 0:
						_6D28EB9F = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_00a4;
					default:
						if (!fighterManager.IsPlaced)
						{
							_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](50));
							if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = _6D28EB9F;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
								return;
							}
							goto IL_00a4;
						}
						awaiter = fighterManager.WaitForAllMembersInFight().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0119;
					case 1:
						awaiter = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0119;
					case 2:
						{
							awaiter = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_00a4:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						goto default;
						IL_0119:
						if (awaiter.GetResult())
						{
							awaiter = fighterManager.ForgeAndSendFightReadyRequest().GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 2);
								_003C_003Eu__2 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							break;
						}
						goto end_IL_0013;
					}
					awaiter.GetResult();
					end_IL_0013:;
				}
				catch (Exception ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
				}
			}
			catch (Exception _52914E)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[545](ref _003C_003Et__builder, _52914E);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[993](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[875](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CHandleChallengeListEvent_003Ed__25 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncVoidMethodBuilder _003C_003Et__builder;

		private void MoveNext()
		{
			try
			{
			}
			catch (Exception _52914E)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[545](ref _003C_003Et__builder, _52914E);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[993](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[875](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CHandleChallengeProposalEvent_003Ed__26 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncVoidMethodBuilder _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		public ByteString value;

		private JitsuriChallengeProposalEvent _003Cev_003E5__2;

		private TaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				TaskAwaiter _6D28EB9F;
				TaskAwaiter<bool> awaiter;
				switch (num)
				{
				case 0:
					_6D28EB9F = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_00a6;
				default:
					if (!fighterManager.IsPlaced)
					{
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
						goto IL_00a6;
					}
					_003Cev_003E5__2 = _018E928B._29B6D927<ChallengeProposalEvent, JitsuriChallengeProposalEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[1234]()).map;
					if (fighterManager._instanceData.PartyManager._8B821C87)
					{
						awaiter = fighterManager.WaitForAllMembersInFight().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_015b;
					}
					goto IL_016c;
				case 1:
					awaiter = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_015b;
				case 2:
					_6D28EB9F = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_01e9;
				case 3:
					{
						awaiter = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						break;
					}
					IL_00a6:
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					goto default;
					IL_015b:
					if (awaiter.GetResult())
					{
						goto IL_016c;
					}
					goto end_IL_000e;
					IL_016c:
					_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__1 = _6D28EB9F;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
						return;
					}
					goto IL_01e9;
					IL_01e9:
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					awaiter = fighterManager.ForgeAndSendChallengeValidateRequest(_671BC22C.BF9F3D1F.FE8E0C9E[1421](_671BC22C.BF9F3D1F.FE8E0C9E[990](_003Cev_003E5__2)[0])).GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (_003C_003E1__state = 3);
						_003C_003Eu__2 = awaiter;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						return;
					}
					break;
				}
				awaiter.GetResult();
				end_IL_000e:;
			}
			catch (Exception _52914E)
			{
				_003C_003E1__state = -2;
				_003Cev_003E5__2 = null;
				_671BC22C.BF9F3D1F.FE8E0C9E[545](ref _003C_003Et__builder, _52914E);
				return;
			}
			_003C_003E1__state = -2;
			_003Cev_003E5__2 = null;
			_671BC22C.BF9F3D1F.FE8E0C9E[993](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[875](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CHandleEntitiesDispositionEvent_003Ed__27 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncVoidMethodBuilder _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		public ByteString value;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				TaskAwaiter<bool> awaiter;
				if (num != 0)
				{
					if (num == 1)
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0100;
					}
					if (fighterManager._instanceData.CharacterData.CharacterLevel >= 5 || !fighterManager._instanceData.CharacterParameters.AutoFightReadyEnabled)
					{
						goto IL_0108;
					}
					awaiter = fighterManager.WaitForAllMembersInFight().GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = awaiter;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						return;
					}
				}
				else
				{
					awaiter = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
				}
				if (awaiter.GetResult())
				{
					awaiter = fighterManager.ForgeAndSendFightReadyRequest().GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__1 = awaiter;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						return;
					}
					goto IL_0100;
				}
				goto IL_0108;
				IL_0108:
				IEnumerator<JitsuriEntityDisposition> enumerator = _671BC22C.BF9F3D1F.FE8E0C9E[1589](_018E928B._29B6D927<EntitiesDispositionEvent, JitsuriEntitiesDispositionEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[1220]()).map).GetEnumerator();
				try
				{
					while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
					{
						JitsuriEntityDisposition current = enumerator.Current;
						_003C_003Ec__DisplayClass27_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass27_0();
						if (_671BC22C.BF9F3D1F.FE8E0C9E[2052](current) && _671BC22C.BF9F3D1F.FE8E0C9E[2052](current))
						{
							CS_0024_003C_003E8__locals3.actorId = _671BC22C.BF9F3D1F.FE8E0C9E[662](current);
							int cellId = _671BC22C.BF9F3D1F.FE8E0C9E[1466](current);
							Attacker attacker = fighterManager.Attackers.Find((Attacker actor) => actor.ActorId == CS_0024_003C_003E8__locals3.actorId);
							if (attacker != null)
							{
								attacker.CellId = cellId;
							}
							Defender defender = fighterManager.Defenders.Find((Defender actor) => actor.ActorId == CS_0024_003C_003E8__locals3.actorId);
							if (defender != null)
							{
								defender.CellId = cellId;
							}
						}
					}
				}
				finally
				{
					if (num < 0)
					{
						enumerator?.Dispose();
					}
				}
				fighterManager.UpdateMapInformation();
				goto end_IL_000e;
				IL_0100:
				awaiter.GetResult();
				goto IL_0108;
				end_IL_000e:;
			}
			catch (Exception _52914E)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[545](ref _003C_003Et__builder, _52914E);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[993](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[875](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CHandleFightPlacementPossiblePositionsEvent_003Ed__34 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncVoidMethodBuilder _003C_003Et__builder;

		public ByteString value;

		public FighterManager _003C_003E4__this;

		private _003C_003Ec__DisplayClass34_0 _003C_003E8__1;

		private JitsuriFightPlacementPossiblePositionsEvent _003Cev_003E5__2;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private TaskAwaiter _003C_003Eu__2;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				_ = 12;
				try
				{
					TaskAwaiter _6D28EB9F;
					TaskAwaiter<bool> awaiter;
					List<int> possiblePositions;
					List<ActorFighter>.Enumerator enumerator;
					List<int>.Enumerator enumerator2;
					switch (num)
					{
					default:
						_003C_003E8__1 = new _003C_003Ec__DisplayClass34_0();
						_003Cev_003E5__2 = _018E928B._29B6D927<FightPlacementPossiblePositionsEvent, JitsuriFightPlacementPossiblePositionsEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[1409]()).map;
						if (!fighterManager._instanceData.CharacterParameters.AutoFightReadyEnabled)
						{
							awaiter = fighterManager.ForgeAndSendChallengeBonusChoiceRequest().GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_00f0;
						}
						goto IL_018d;
					case 0:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_00f0;
					case 1:
						_6D28EB9F = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_017a;
					case 2:
						_6D28EB9F = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_034a;
					case 3:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_03b7;
					case 4:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0419;
					case 5:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_047b;
					case 6:
						_6D28EB9F = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0528;
					case 7:
						_6D28EB9F = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_05b2;
					case 8:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_061f;
					case 9:
						_6D28EB9F = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_06ab;
					case 10:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0719;
					case 11:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_077c;
					case 12:
						{
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							goto IL_07df;
						}
						IL_05c5:
						awaiter = fighterManager.WaitForAllMembersInFight().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 8);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_061f;
						IL_0540:
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](fighterManager.PlaceNearMonster(possiblePositions));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 7);
							_003C_003Eu__2 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
						goto IL_05b2;
						IL_00f0:
						awaiter.GetResult();
						goto end_IL_000e;
						IL_018d:
						if (fighterManager.IsLoadingMap)
						{
							_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](50));
							if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__2 = _6D28EB9F;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
								return;
							}
							goto IL_017a;
						}
						_003C_003E8__1.occupiedCells = new Dictionary<int, bool>();
						enumerator = fighterManager.Defenders.Cast<ActorFighter>().Concat(fighterManager.Attackers.Cast<ActorFighter>()).ToList()
							.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								ActorFighter current = enumerator.Current;
								if (current.ActorId != fighterManager._instanceData.CharacterData.CharacterId)
								{
									_003C_003E8__1.occupiedCells[current.CellId] = true;
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
							}
						}
						enumerator2 = fighterManager.TurnOccupiedCells.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								int current2 = enumerator2.Current;
								_003C_003E8__1.occupiedCells[current2] = true;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
							}
						}
						possiblePositions = (from e in _671BC22C.BF9F3D1F.FE8E0C9E[1442](_671BC22C.BF9F3D1F.FE8E0C9E[472](_003Cev_003E5__2))
							where !_003C_003E8__1.occupiedCells.ContainsKey(e)
							select e).ToList();
						if (fighterManager.IsFightingForTreasure)
						{
							_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](fighterManager.PlaceWithFreeAdjacents(possiblePositions));
							if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
							{
								num = (_003C_003E1__state = 2);
								_003C_003Eu__2 = _6D28EB9F;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
								return;
							}
							goto IL_034a;
						}
						switch (fighterManager._instanceData.CharacterParameters.CombatAction)
						{
						case CombatAction.Ranged:
						case CombatAction.Hybride:
							break;
						case CombatAction.Melee:
							goto IL_0540;
						default:
							goto IL_05c5;
						}
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](fighterManager.PlaceFarFromMonster(possiblePositions));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 6);
							_003C_003Eu__2 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
						goto IL_0528;
						IL_07df:
						awaiter.GetResult();
						break;
						IL_061f:
						if (!awaiter.GetResult())
						{
							break;
						}
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](300));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 9);
							_003C_003Eu__2 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
						goto IL_06ab;
						IL_017a:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						goto IL_018d;
						IL_0528:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						goto IL_05c5;
						IL_077c:
						awaiter.GetResult();
						awaiter = fighterManager.ForgeAndSendFightReadyRequest().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 12);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_07df;
						IL_034a:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						awaiter = fighterManager.ForgeAndSendChallengeBonusChoiceRequest().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 3);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_03b7;
						IL_05b2:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						goto IL_05c5;
						IL_03b7:
						awaiter.GetResult();
						awaiter = fighterManager.ForgeAndSendChallengeReadyRequest().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 4);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0419;
						IL_0719:
						awaiter.GetResult();
						awaiter = fighterManager.ForgeAndSendChallengeReadyRequest().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 11);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_077c;
						IL_0419:
						awaiter.GetResult();
						awaiter = fighterManager.ForgeAndSendFightReadyRequest().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 5);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_047b;
						IL_06ab:
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
						awaiter = fighterManager.ForgeAndSendChallengeBonusChoiceRequest().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 10);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0719;
						IL_047b:
						awaiter.GetResult();
						break;
					}
					_003C_003E8__1 = null;
					_003Cev_003E5__2 = null;
				}
				catch (Exception ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
				}
				fighterManager.IsPlaced = true;
				end_IL_000e:;
			}
			catch (Exception _52914E)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[545](ref _003C_003Et__builder, _52914E);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[993](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[875](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CHandleFightTurnEvent_003Ed__40 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncVoidMethodBuilder _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		public ByteString value;

		private _003C_003Ec__DisplayClass40_1 _003C_003E8__1;

		private _003C_003Ec__DisplayClass40_0 _003C_003E8__2;

		private _003C_003Ec__DisplayClass40_2 _003C_003E8__3;

		private object _003C_003E7__wrap1;

		private int _003C_003E7__wrap2;

		private int _003CcurrentRound_003E5__4;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private TaskAwaiter _003C_003Eu__2;

		private Defender _003Cdefender_003E5__5;

		private List<JitsuriSpellItem>.Enumerator _003C_003E7__wrap5;

		private JitsuriSpellItem _003Cspell_003E5__7;

		private int _003CapCost_003E5__8;

		private int _003CnumberOfCast_003E5__9;

		private Dictionary<long, int> _003CspellUsedOnActors_003E5__10;

		private int _003Ci_003E5__11;

		private int _003CremainingAp_003E5__12;

		private object _003C_003E7__wrap12;

		private List<UserDefinedSpell>.Enumerator _003C_003E7__wrap13;

		private object _003C_003E7__wrap14;

		private int _003Ci_003E5__16;

		private int _003CmaxRange_003E5__17;

		private int _003CminRange_003E5__18;

		private int? _003CcellToTargetId_003E5__19;

		private int _003CmaxMovementPoint_003E5__20;

		private int? _003CrepositionCell_003E5__21;

		private TaskAwaiter<int?> _003C_003Eu__3;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager CS_0024_003C_003E8__locals200 = _003C_003E4__this;
			try
			{
				DefaultInterpolatedStringHandler D6A007B;
				TaskAwaiter _6D28EB9F;
				TaskAwaiter<bool> awaiter;
				int _4AB61FB;
				Exception ex6;
				switch (num)
				{
				default:
					CS_0024_003C_003E8__locals200.VerboseClear();
					CS_0024_003C_003E8__locals200.Verbose("===== HandleFightTurnEvent: START =====");
					_003C_003E7__wrap2 = 0;
					goto case 0;
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 21:
				case 22:
				case 23:
				case 24:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
				case 30:
					try
					{
						Exception ex4;
						switch (num)
						{
						default:
							CS_0024_003C_003E8__locals200._instanceData.CharacterData.IsFighting = true;
							try
							{
								JitsuriFightTurnEvent item = _018E928B._29B6D927<FightTurnEvent, JitsuriFightTurnEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[1200]()).map;
								D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](37, 1);
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "FightTurnEvent received. CharacterId=");
								D6A007B.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[596](item));
								CS_0024_003C_003E8__locals200.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
								CS_0024_003C_003E8__locals200.IsMyTurn = _671BC22C.BF9F3D1F.FE8E0C9E[596](item) == CS_0024_003C_003E8__locals200._instanceData.CharacterData.CharacterId;
							}
							catch
							{
								_671BC22C.BF9F3D1F.FE8E0C9E[376]();
							}
							CS_0024_003C_003E8__locals200._instanceData.CharacterData.IsFighting = true;
							D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](9, 1);
							_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "IsMyTurn=");
							D6A007B.AppendFormatted(CS_0024_003C_003E8__locals200.IsMyTurn);
							CS_0024_003C_003E8__locals200.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
							if (!CS_0024_003C_003E8__locals200.IsMyTurn)
							{
								CS_0024_003C_003E8__locals200.Verbose("Not my turn -> return");
							}
							else
							{
								CS_0024_003C_003E8__locals200.Round++;
								D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](21, 1);
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "Round incremented -> ");
								D6A007B.AppendFormatted(CS_0024_003C_003E8__locals200.Round);
								CS_0024_003C_003E8__locals200.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
								try
								{
									CS_0024_003C_003E8__locals200.Verbose("STATE/Attackers", CS_0024_003C_003E8__locals200.Attackers);
									CS_0024_003C_003E8__locals200.Verbose("STATE/Defenders", CS_0024_003C_003E8__locals200.Defenders);
									CS_0024_003C_003E8__locals200.Verbose("STATE/TurnOccupiedCells", CS_0024_003C_003E8__locals200.TurnOccupiedCells);
									CS_0024_003C_003E8__locals200.Verbose("STATE/SpellIdOnCooldown", CS_0024_003C_003E8__locals200.SpellIdOnCooldown);
									CS_0024_003C_003E8__locals200.Verbose((object)"STATE/MapInformation.ActorsCount", (CS_0024_003C_003E8__locals200.MapInformation?.Actors?.Count).GetValueOrDefault());
								}
								catch (Exception fEBDE)
								{
									_671BC22C.BF9F3D1F.FE8E0C9E[376]();
									CS_0024_003C_003E8__locals200.Verbose("STATE_DUMP_ERROR", _671BC22C.BF9F3D1F.FE8E0C9E[2117](fEBDE));
								}
								_003CcurrentRound_003E5__4 = CS_0024_003C_003E8__locals200.Round;
								if (CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.AutoPassTurnEnabled)
								{
									CS_0024_003C_003E8__locals200.Verbose("AutoPassTurnEnabled", true);
									awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
									if (!awaiter.IsCompleted)
									{
										num = (_003C_003E1__state = 0);
										_003C_003Eu__1 = awaiter;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
										return;
									}
									goto IL_045a;
								}
								if (CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.AutoFightEnabled)
								{
									CS_0024_003C_003E8__locals200.Verbose("CombatSpeed", CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.CombatSpeed);
									switch (CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.CombatSpeed)
									{
									case CombatSpeed.Rapide:
										break;
									case CombatSpeed.Human:
										goto IL_057e;
									default:
										goto IL_0611;
									}
									_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
									if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
									{
										num = (_003C_003E1__state = 1);
										_003C_003Eu__2 = _6D28EB9F;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
										return;
									}
									goto IL_0566;
								}
								CS_0024_003C_003E8__locals200.Verbose("AutoFightEnabled", false);
							}
							goto end_IL_00b1;
						case 0:
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							goto IL_045a;
						case 1:
							_6D28EB9F = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter);
							num = (_003C_003E1__state = -1);
							goto IL_0566;
						case 2:
							_6D28EB9F = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter);
							num = (_003C_003E1__state = -1);
							goto IL_05fe;
						case 3:
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							goto IL_06b9;
						case 4:
						case 5:
						case 6:
						case 7:
							try
							{
								bool result4;
								int? f8A;
								switch (num)
								{
								case 4:
									awaiter = _003C_003Eu__1;
									_003C_003Eu__1 = default(TaskAwaiter<bool>);
									num = (_003C_003E1__state = -1);
									goto IL_0bba;
								case 5:
									awaiter = _003C_003Eu__1;
									_003C_003Eu__1 = default(TaskAwaiter<bool>);
									num = (_003C_003E1__state = -1);
									goto IL_0cce;
								case 6:
									_6D28EB9F = _003C_003Eu__2;
									_003C_003Eu__2 = default(TaskAwaiter);
									num = (_003C_003E1__state = -1);
									goto IL_0d80;
								case 7:
									awaiter = _003C_003Eu__1;
									_003C_003Eu__1 = default(TaskAwaiter<bool>);
									num = (_003C_003E1__state = -1);
									break;
								default:
									{
										if (!_003C_003E7__wrap5.MoveNext())
										{
											goto end_IL_079d;
										}
										_003Cspell_003E5__7 = _003C_003E7__wrap5.Current;
										CS_0024_003C_003E8__locals200.Verbose("spell", _003Cspell_003E5__7);
										if (!CS_0024_003C_003E8__locals200._instanceData.CharacterData.DetailedSpells.ContainsKey(_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7)))
										{
											CS_0024_003C_003E8__locals200.Verbose((object)"MissingDetailedSpell", _671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7));
										}
										else
										{
											DetailedSpell detailedSpell2 = CS_0024_003C_003E8__locals200._instanceData.CharacterData.DetailedSpells[_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7)];
											CS_0024_003C_003E8__locals200.Verbose("detailedSpell", detailedSpell2);
											int num8 = _671BC22C.BF9F3D1F.FE8E0C9E[835](_003Cspell_003E5__7) - 1;
											if (num8 < 0 || num8 >= detailedSpell2.SpellLevels.Levels.Count())
											{
												CS_0024_003C_003E8__locals200.Verbose("InvalidSpellLevel", new global::_2A1293B5<int, int>(_671BC22C.BF9F3D1F.FE8E0C9E[835](_003Cspell_003E5__7), detailedSpell2.SpellLevels.Levels.Count()));
											}
											else
											{
												int num9 = detailedSpell2.SpellLevels.Levels[num8];
												if (SpellLevelRepository.Repository.ContainsKey(num9))
												{
													DetailedSpellLevel detailedSpellLevel = SpellLevelRepository.Repository[num9];
													CS_0024_003C_003E8__locals200.Verbose("spellLevel", detailedSpellLevel);
													_003CapCost_003E5__8 = detailedSpellLevel.ApCost;
													_003CnumberOfCast_003E5__9 = ((detailedSpellLevel.MaxCastPerTarget != 0) ? detailedSpellLevel.MaxCastPerTarget : detailedSpellLevel.MaxCastPerTurn);
													CS_0024_003C_003E8__locals200.Verbose("CastingPlan", new global::FD0A22A6<int, int, int>(_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7), _003CapCost_003E5__8, _003CnumberOfCast_003E5__9));
													_003CspellUsedOnActors_003E5__10 = new Dictionary<long, int>();
													_003Ci_003E5__11 = 0;
													goto IL_0eb2;
												}
												CS_0024_003C_003E8__locals200.Verbose((object)"MissingSpellLevelId", num9);
											}
										}
										goto end_IL_00b1;
									}
									IL_0cce:
									result4 = awaiter.GetResult();
									CS_0024_003C_003E8__locals200.Verbose("CastResult", result4);
									if (!result4)
									{
										CS_0024_003C_003E8__locals200.Verbose("CastRequestFailed", true);
										_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](CS_0024_003C_003E8__locals200.SendDiscordWebhookSafeAsync("CastRequest FALSE (Treasure)"));
										if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
										{
											num = (_003C_003E1__state = 6);
											_003C_003Eu__2 = _6D28EB9F;
											_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
											return;
										}
										goto IL_0d80;
									}
									_003CspellUsedOnActors_003E5__10.TryAdd(_003Cdefender_003E5__5.ActorId, 0);
									_003CspellUsedOnActors_003E5__10[_003Cdefender_003E5__5.ActorId]++;
									f8A = CS_0024_003C_003E8__locals200.GetSelf()?.Characteristics[CharacteristicKeyword.ActionPoints].Value;
									CS_0024_003C_003E8__locals200.Verbose("PostCast", new global::D61989A4<int, int?, int>(_003CremainingAp_003E5__12, f8A, _003CspellUsedOnActors_003E5__10[_003Cdefender_003E5__5.ActorId]));
									_003Ci_003E5__11++;
									goto IL_0eb2;
									IL_0eb2:
									if (_003Ci_003E5__11 >= _003CnumberOfCast_003E5__9)
									{
										_003CspellUsedOnActors_003E5__10 = null;
										_003Cspell_003E5__7 = null;
										goto default;
									}
									CS_0024_003C_003E8__locals200.Verbose("CastIteration", new global::D2A73A83<int, int, int, int>(_003Ci_003E5__11 + 1, _003CnumberOfCast_003E5__9, _003CcurrentRound_003E5__4, CS_0024_003C_003E8__locals200.Round));
									if (_003CcurrentRound_003E5__4 != CS_0024_003C_003E8__locals200.Round)
									{
										CS_0024_003C_003E8__locals200.Verbose("RoundChanged", true);
									}
									else
									{
										Attacker self2 = CS_0024_003C_003E8__locals200.GetSelf();
										if (self2 == null)
										{
											CS_0024_003C_003E8__locals200.Verbose("Self", "null");
										}
										else
										{
											CS_0024_003C_003E8__locals200.Verbose("Self", new global::_6110EF30<long, int, int, int?>(self2.ActorId, self2.CellId, self2.Characteristics[CharacteristicKeyword.ActionPoints].Value, self2.Characteristics.ContainsKey(CharacteristicKeyword.MovementPoints) ? new int?(self2.Characteristics[CharacteristicKeyword.MovementPoints].Value) : ((int?)null)));
											_003Cdefender_003E5__5 = CS_0024_003C_003E8__locals200.Defenders.Find((Defender d) => d.ActorId == CS_0024_003C_003E8__locals200.TreasureTargetId);
											if (_003Cdefender_003E5__5 == null)
											{
												CS_0024_003C_003E8__locals200.Verbose("DefenderMissing", true);
											}
											else
											{
												CS_0024_003C_003E8__locals200.Verbose("Defender", _003Cdefender_003E5__5);
												_003CremainingAp_003E5__12 = self2.Characteristics[CharacteristicKeyword.ActionPoints].Value;
												CS_0024_003C_003E8__locals200.Verbose((object)"RemainingAp", _003CremainingAp_003E5__12);
												if (_003CremainingAp_003E5__12 < _003CapCost_003E5__8)
												{
													CS_0024_003C_003E8__locals200.Verbose("ApInsufficient", new global::FD0F4D25<int, int>(_003CremainingAp_003E5__12, _003CapCost_003E5__8));
													awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
													if (!awaiter.IsCompleted)
													{
														num = (_003C_003E1__state = 4);
														_003C_003Eu__1 = awaiter;
														_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
														return;
													}
													goto IL_0bba;
												}
												if (!CS_0024_003C_003E8__locals200._instanceData.CharacterData.IsFighting)
												{
													CS_0024_003C_003E8__locals200.Verbose("IsFighting", false);
												}
												else
												{
													if (_003CcurrentRound_003E5__4 == CS_0024_003C_003E8__locals200.Round)
													{
														CS_0024_003C_003E8__locals200.Verbose("CastRequest", new global::_2425EA1F<int, int>(_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7), _003Cdefender_003E5__5.CellId));
														awaiter = CS_0024_003C_003E8__locals200.ForgeAndSendGameActionFightCastRequest(_003Cdefender_003E5__5.CellId, _671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7)).GetAwaiter();
														if (!awaiter.IsCompleted)
														{
															num = (_003C_003E1__state = 5);
															_003C_003Eu__1 = awaiter;
															_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
															return;
														}
														goto IL_0cce;
													}
													CS_0024_003C_003E8__locals200.Verbose("RoundChangedPostChecks", true);
												}
											}
										}
									}
									goto end_IL_00b1;
									IL_0d80:
									_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
									awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
									if (!awaiter.IsCompleted)
									{
										num = (_003C_003E1__state = 7);
										_003C_003Eu__1 = awaiter;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
										return;
									}
									break;
									IL_0bba:
									awaiter.GetResult();
									goto end_IL_00b1;
								}
								awaiter.GetResult();
								goto end_IL_00b1;
								end_IL_079d:;
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)_003C_003E7__wrap5/*cast due to .constrained prefix*/).Dispose();
								}
							}
							_003C_003E7__wrap5 = default(List<JitsuriSpellItem>.Enumerator);
							if (_003CcurrentRound_003E5__4 == CS_0024_003C_003E8__locals200.Round)
							{
								CS_0024_003C_003E8__locals200.Verbose("TreasureLoopDone", true);
								awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
								if (!awaiter.IsCompleted)
								{
									num = (_003C_003E1__state = 8);
									_003C_003Eu__1 = awaiter;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
									return;
								}
								goto IL_0fa7;
							}
							CS_0024_003C_003E8__locals200.Verbose("RoundChangedAfterLoop", true);
							goto end_IL_00b1;
						case 8:
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							goto IL_0fa7;
						case 9:
						case 10:
						case 11:
						case 12:
						case 13:
						case 14:
						case 15:
						case 16:
						case 17:
						case 18:
						case 19:
						case 20:
						case 21:
						case 22:
						case 23:
						case 24:
						case 25:
						case 26:
						case 27:
						case 28:
							try
							{
								Attacker self;
								CombatSpeed combatSpeed;
								int num7;
								int maxRange;
								int safestReachableCellWithinRangeOfMonsters;
								switch (num)
								{
								default:
								{
									CS_0024_003C_003E8__locals200.Verbose("Flow", "Standard combat flow");
									CS_0024_003C_003E8__locals200.UpdateSpellCooldown();
									List<UserDefinedSpell> list = (from e in CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.UserDefinedSpells
										where e.Delay < CS_0024_003C_003E8__locals200.Round && !CS_0024_003C_003E8__locals200.SpellIdOnCooldown.ContainsKey(e.SpellId)
										orderby e.Priority
										select e).ToList();
									CS_0024_003C_003E8__locals200.Verbose("UserDefinedSpells", list);
									_003C_003E7__wrap13 = list.GetEnumerator();
									goto case 9;
								}
								case 9:
								case 10:
								case 11:
								case 12:
								case 13:
								case 14:
								case 15:
								case 16:
								case 17:
								case 18:
								case 19:
								case 20:
									try
									{
										switch (num)
										{
										case 9:
										case 10:
										case 11:
										case 12:
										case 13:
										case 14:
										case 15:
										case 16:
										case 17:
										case 18:
											try
											{
												TaskAwaiter<int?> awaiter2;
												int? result;
												object obj2;
												bool result2;
												bool result3;
												List<_9F8EAF35> list2;
												switch (num)
												{
												default:
												{
													_003C_003E8__1 = new _003C_003Ec__DisplayClass40_1();
													CS_0024_003C_003E8__locals200.Verbose("UDS.current", _003C_003E8__2.userDefinedSpell);
													_003Cspell_003E5__7 = CS_0024_003C_003E8__locals200._instanceData.CharacterData.Spells.Find((JitsuriSpellItem e) => _671BC22C.BF9F3D1F.FE8E0C9E[84](e) == _003C_003E8__2.userDefinedSpell.SpellId);
													if (_003Cspell_003E5__7 == null)
													{
														_671BC22C.BF9F3D1F.FE8E0C9E[376]();
														CS_0024_003C_003E8__locals200.Verbose((object)"Spell.notFoundOnCharacter", _003C_003E8__2.userDefinedSpell.SpellId);
														break;
													}
													CS_0024_003C_003E8__locals200.Verbose("Spell", _003Cspell_003E5__7);
													if (!CS_0024_003C_003E8__locals200._instanceData.CharacterData.DetailedSpells.TryGetValue(_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7), out DetailedSpell detailedSpell) || detailedSpell == null)
													{
														_671BC22C.BF9F3D1F.FE8E0C9E[376]();
														CS_0024_003C_003E8__locals200.Verbose((object)"DetailedSpell.missingFor", _671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7));
														break;
													}
													CS_0024_003C_003E8__locals200.Verbose("DetailedSpell", detailedSpell);
													int num2 = detailedSpell.SpellLevels.Levels[_671BC22C.BF9F3D1F.FE8E0C9E[835](_003Cspell_003E5__7) - 1];
													if (!SpellLevelRepository.Repository.TryGetValue(num2, out _003C_003E8__1.spellLevel))
													{
														_671BC22C.BF9F3D1F.FE8E0C9E[376]();
														CS_0024_003C_003E8__locals200.Verbose((object)"SpellLevel.missingId", num2);
														break;
													}
													CS_0024_003C_003E8__locals200.Verbose("SpellLevel", _003C_003E8__1.spellLevel);
													_003Ci_003E5__11 = _003C_003E8__1.spellLevel.ApCost;
													_003CremainingAp_003E5__12 = _003C_003E8__2.userDefinedSpell.PerTurn;
													CS_0024_003C_003E8__locals200.Verbose("CastingPlan", new global::_2E2D52A7<int, int, int>(_003C_003E8__2.userDefinedSpell.SpellId, _003CremainingAp_003E5__12, _003Ci_003E5__11));
													_003C_003E8__1.spellUsedOnActors = new Dictionary<int, int>();
													_003Ci_003E5__16 = 0;
													goto IL_2841;
												}
												case 9:
													awaiter = _003C_003Eu__1;
													_003C_003Eu__1 = default(TaskAwaiter<bool>);
													num = (_003C_003E1__state = -1);
													goto IL_15d5;
												case 10:
													_6D28EB9F = _003C_003Eu__2;
													_003C_003Eu__2 = default(TaskAwaiter);
													num = (_003C_003E1__state = -1);
													goto IL_1698;
												case 11:
													awaiter = _003C_003Eu__1;
													_003C_003Eu__1 = default(TaskAwaiter<bool>);
													num = (_003C_003E1__state = -1);
													goto IL_1706;
												case 12:
													awaiter2 = _003C_003Eu__3;
													_003C_003Eu__3 = default(TaskAwaiter<int?>);
													num = (_003C_003E1__state = -1);
													goto IL_21d9;
												case 13:
													awaiter = _003C_003Eu__1;
													_003C_003Eu__1 = default(TaskAwaiter<bool>);
													num = (_003C_003E1__state = -1);
													goto IL_22b4;
												case 14:
													_6D28EB9F = _003C_003Eu__2;
													_003C_003Eu__2 = default(TaskAwaiter);
													num = (_003C_003E1__state = -1);
													goto IL_2393;
												case 15:
													_6D28EB9F = _003C_003Eu__2;
													_003C_003Eu__2 = default(TaskAwaiter);
													num = (_003C_003E1__state = -1);
													goto IL_242c;
												case 16:
													awaiter = _003C_003Eu__1;
													_003C_003Eu__1 = default(TaskAwaiter<bool>);
													num = (_003C_003E1__state = -1);
													goto IL_261a;
												case 17:
													_6D28EB9F = _003C_003Eu__2;
													_003C_003Eu__2 = default(TaskAwaiter);
													num = (_003C_003E1__state = -1);
													goto IL_26dd;
												case 18:
													{
														awaiter = _003C_003Eu__1;
														_003C_003Eu__1 = default(TaskAwaiter<bool>);
														num = (_003C_003E1__state = -1);
														goto IL_278c;
													}
													IL_242c:
													_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
													goto IL_243f;
													IL_21d9:
													result = awaiter2.GetResult();
													_003CrepositionCell_003E5__21 = result;
													if (!_003CrepositionCell_003E5__21.HasValue)
													{
														obj2 = "none";
													}
													else
													{
														_4AB61FB = _003CrepositionCell_003E5__21.Value;
														obj2 = _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref _4AB61FB);
													}
													CS_0024_003C_003E8__locals200.Verbose("Reposition.cell", obj2);
													if (_003CrepositionCell_003E5__21.HasValue)
													{
														awaiter = CS_0024_003C_003E8__locals200.Fighter.MoveTo(_003CrepositionCell_003E5__21.Value, _003CmaxMovementPoint_003E5__20).GetAwaiter();
														if (!awaiter.IsCompleted)
														{
															num = (_003C_003E1__state = 13);
															_003C_003Eu__1 = awaiter;
															_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
															return;
														}
														goto IL_22b4;
													}
													goto IL_2522;
													IL_243f:
													list2 = (from mp in D98DE637._811AB8B8(CS_0024_003C_003E8__locals200.MapInformation.Cells, _003CrepositionCell_003E5__21.Value, _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel, _003CminRange_003E5__18, _003CmaxRange_003E5__17, _003C_003E8__3.occupiedCells)
														where _003C_003E8__3.wantedTargetCells.Contains(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495))
														select mp).ToList();
													CS_0024_003C_003E8__locals200.Verbose("ViableCastPoints.afterReposition", list2);
													if (list2.Count > 0)
													{
														_003CcellToTargetId_003E5__19 = (from mp in list2
															orderby _4CAC0493._52884196.E230AC25(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495), _003C_003E8__3.ennemyToTarget.CellId)
															select _8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495)).First();
														CS_0024_003C_003E8__locals200.Verbose((object)"CastCell.selected.afterReposition", _003CcellToTargetId_003E5__19.Value);
													}
													goto IL_2522;
													IL_15d5:
													result2 = awaiter.GetResult();
													CS_0024_003C_003E8__locals200.Verbose("Cast.result.self", result2);
													if (!result2)
													{
														_671BC22C.BF9F3D1F.FE8E0C9E[376]();
														CS_0024_003C_003E8__locals200.Verbose("Cast.fail.self", true);
														_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](CS_0024_003C_003E8__locals200.SendDiscordWebhookSafeAsync("CastRequest FALSE (self-cast)"));
														if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
														{
															num = (_003C_003E1__state = 10);
															_003C_003Eu__2 = _6D28EB9F;
															_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
															return;
														}
														goto IL_1698;
													}
													_003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors[(int)_003C_003E8__3.myself.ActorId] = _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)_003C_003E8__3.myself.ActorId) + 1;
													if (_003C_003E8__2.userDefinedSpell.Cooldown > 0)
													{
														CS_0024_003C_003E8__locals200.SpellIdOnCooldown[_003C_003E8__2.userDefinedSpell.SpellId] = _003C_003E8__2.userDefinedSpell.Cooldown;
													}
													goto IL_282f;
													IL_2828:
													_003C_003E8__3 = null;
													goto IL_282f;
													IL_261a:
													result3 = awaiter.GetResult();
													CS_0024_003C_003E8__locals200.Verbose("Cast.result", result3);
													if (!result3)
													{
														_671BC22C.BF9F3D1F.FE8E0C9E[376]();
														CS_0024_003C_003E8__locals200.Verbose("Cast.fail.standard", true);
														_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](CS_0024_003C_003E8__locals200.SendDiscordWebhookSafeAsync("CastRequest FALSE (standard)"));
														if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
														{
															num = (_003C_003E1__state = 17);
															_003C_003Eu__2 = _6D28EB9F;
															_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
															return;
														}
														goto IL_26dd;
													}
													_003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors[(int)_003C_003E8__3.ennemyToTarget.ActorId] = _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)_003C_003E8__3.ennemyToTarget.ActorId) + 1;
													if (_003C_003E8__2.userDefinedSpell.Cooldown > 0)
													{
														CS_0024_003C_003E8__locals200.SpellIdOnCooldown[_003C_003E8__2.userDefinedSpell.SpellId] = _003C_003E8__2.userDefinedSpell.Cooldown;
													}
													goto IL_2828;
													IL_2841:
													if (_003Ci_003E5__16 < _003CremainingAp_003E5__12)
													{
														_003C_003E8__3 = new _003C_003Ec__DisplayClass40_2();
														_003C_003E8__3.CS_0024_003C_003E8__locals1 = _003C_003E8__1;
														CS_0024_003C_003E8__locals200.Verbose("UDS.Iteration", new global::_5528403F<int, int, int>(_003Ci_003E5__16 + 1, _003CremainingAp_003E5__12, CS_0024_003C_003E8__locals200.Round));
														_003C_003E8__3.myself = CS_0024_003C_003E8__locals200.GetSelf();
														if (_003C_003E8__3.myself == null)
														{
															_671BC22C.BF9F3D1F.FE8E0C9E[376]();
															CS_0024_003C_003E8__locals200.Verbose("Self.null", true);
															goto end_IL_10aa;
														}
														CS_0024_003C_003E8__locals200.Verbose("Self", _003C_003E8__3.myself);
														bool flag = _003C_003E8__2.userDefinedSpell.SelfHpCondition.IsValid(_003C_003E8__3.myself.GetLifePointPercentage());
														CS_0024_003C_003E8__locals200.Verbose("SelfHpCondition.valid", flag);
														if (!flag)
														{
															goto IL_282f;
														}
														int num3 = _003C_003E8__3.myself.Characteristics[CharacteristicKeyword.ActionPoints].Value;
														CS_0024_003C_003E8__locals200.Verbose("AP.check", new global::_7D188C89<int, int>(num3, _003Ci_003E5__11));
														if (num3 < _003Ci_003E5__11)
														{
															goto IL_282f;
														}
														if (!CS_0024_003C_003E8__locals200._instanceData.CharacterData.IsFighting)
														{
															_671BC22C.BF9F3D1F.FE8E0C9E[376]();
															CS_0024_003C_003E8__locals200.Verbose("IsFighting", false);
															goto end_IL_10aa;
														}
														if (_003C_003E8__2.userDefinedSpell.OnMe)
														{
															if (_003CcurrentRound_003E5__4 != CS_0024_003C_003E8__locals200.Round)
															{
																_671BC22C.BF9F3D1F.FE8E0C9E[376]();
																CS_0024_003C_003E8__locals200.Verbose("RoundChanged.beforeSelfCast", true);
																goto end_IL_10aa;
															}
															CS_0024_003C_003E8__locals200.Verbose("Cast.request.self", new global::_3C18FD9B<int, int>(_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7), _003C_003E8__3.myself.CellId));
															awaiter = CS_0024_003C_003E8__locals200.ForgeAndSendGameActionFightCastRequest(_003C_003E8__3.myself.CellId, _671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7)).GetAwaiter();
															if (!awaiter.IsCompleted)
															{
																num = (_003C_003E1__state = 9);
																_003C_003Eu__1 = awaiter;
																_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
																return;
															}
															goto IL_15d5;
														}
														_003C_003E8__3.ennemyToTarget = null;
														List<ActorFighter> source = _003C_003E8__2.userDefinedSpell.Target switch
														{
															TargetOption.Me => new List<ActorFighter> { _003C_003E8__3.myself }, 
															TargetOption.Enemy => CS_0024_003C_003E8__locals200.Defenders.Where((Defender e) => e.IsAlive && (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget == 0 || _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)e.ActorId) < _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget)).Cast<ActorFighter>().ToList(), 
															TargetOption.EnemySummon => CS_0024_003C_003E8__locals200.Defenders.Where((Defender e) => e.IsAlive && e.IsSummon && (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget == 0 || _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)e.ActorId) < _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget)).Cast<ActorFighter>().ToList(), 
															TargetOption.EnemyNoSummon => CS_0024_003C_003E8__locals200.Defenders.Where((Defender e) => e.IsAlive && !e.IsSummon && (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget == 0 || _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)e.ActorId) < _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget)).Cast<ActorFighter>().ToList(), 
															TargetOption.Ally => CS_0024_003C_003E8__locals200.Attackers.Where((Attacker a) => a.IsAlive && a.ActorId != _003C_003E8__3.myself.ActorId && (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget == 0 || _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)a.ActorId) < _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget)).Cast<ActorFighter>().ToList(), 
															TargetOption.AllySummon => CS_0024_003C_003E8__locals200.Attackers.Where((Attacker a) => a.IsAlive && a.IsSummon && (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget == 0 || _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)a.ActorId) < _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget)).Cast<ActorFighter>().ToList(), 
															TargetOption.AllyNoSummon => CS_0024_003C_003E8__locals200.Attackers.Where((Attacker a) => a.IsAlive && !a.IsSummon && (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget == 0 || _003C_003E8__3.CS_0024_003C_003E8__locals1.spellUsedOnActors.GetValueOrDefault((int)a.ActorId) < _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MaxCastPerTarget)).Cast<ActorFighter>().ToList(), 
															_ => new List<ActorFighter>(), 
														};
														if (_003C_003E8__2.userDefinedSpell.ForceTarget != null && _003C_003E8__2.userDefinedSpell.ForceTarget.Any())
														{
															_003C_003Ec__DisplayClass40_3 CS_0024_003C_003E8__locals195 = new _003C_003Ec__DisplayClass40_3();
															CS_0024_003C_003E8__locals195.forceIds = new HashSet<long>(from id in _003C_003E8__2.userDefinedSpell.ForceTarget.Select((string s) =>
																{
																	long C = default(long);
																	return (!_671BC22C.BF9F3D1F.FE8E0C9E[1932](s, ref C)) ? ((long?)null) : new long?(C);
																})
																where id.HasValue
																select id.Value);
															source = source.Where((ActorFighter t) => CS_0024_003C_003E8__locals195.forceIds.Contains(t.Gid)).ToList();
														}
														if (_003C_003E8__2.userDefinedSpell.ExcludeTarget != null && _003C_003E8__2.userDefinedSpell.ExcludeTarget.Any())
														{
															_003C_003Ec__DisplayClass40_4 CS_0024_003C_003E8__locals196 = new _003C_003Ec__DisplayClass40_4();
															CS_0024_003C_003E8__locals196.excludeIds = new HashSet<long>(from id in _003C_003E8__2.userDefinedSpell.ExcludeTarget.Select((string s) =>
																{
																	long C = default(long);
																	return (!_671BC22C.BF9F3D1F.FE8E0C9E[1932](s, ref C)) ? ((long?)null) : new long?(C);
																})
																where id.HasValue
																select id.Value);
															source = source.Where((ActorFighter t) => !CS_0024_003C_003E8__locals196.excludeIds.Contains(t.Gid)).ToList();
														}
														source = source.Where((ActorFighter t) => _003C_003E8__2.userDefinedSpell.TargetHpCondition.IsValid(t.GetLifePointPercentage())).ToList();
														CS_0024_003C_003E8__locals200.Verbose("ValidTargets", source);
														if (_003C_003E8__2.userDefinedSpell.Target != TargetOption.Me)
														{
															switch (_003C_003E8__2.userDefinedSpell.Focus)
															{
															case FocusOption.Closest:
																_003C_003E8__3.ennemyToTarget = CS_0024_003C_003E8__locals200.Fighter.GetClosestFighterOfCell(_003C_003E8__3.myself.CellId, source);
																break;
															case FocusOption.Weakest:
																_003C_003E8__3.ennemyToTarget = source.MinBy((ActorFighter f) => f.GetLifePointPercentage());
																break;
															}
														}
														else
														{
															_003C_003E8__3.ennemyToTarget = _003C_003E8__3.myself;
														}
														if (_003C_003E8__3.ennemyToTarget == null)
														{
															CS_0024_003C_003E8__locals200.Verbose("Target.selected", "none");
														}
														else
														{
															CS_0024_003C_003E8__locals200.Verbose("Target.selected", _003C_003E8__3.ennemyToTarget);
															int num4 = (_003C_003E8__3.myself.Characteristics.ContainsKey(CharacteristicKeyword.Range) ? _003C_003E8__3.myself.Characteristics[CharacteristicKeyword.Range].Value : 0);
															_003CmaxRange_003E5__17 = FA823390.FB0CEC18(_003C_003E8__2.userDefinedSpell.MaxCastCell, _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.CanRangeBeBoosted ? (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.Range + num4) : _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.Range);
															_003CminRange_003E5__18 = _671BC22C.BF9F3D1F.FE8E0C9E[1937](_003C_003E8__2.userDefinedSpell.MinCastCell, _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.MinRange);
															CS_0024_003C_003E8__locals200.Verbose("SpellRange", new global::_389B179D<int, int, int, int, int>(_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7), num4, _003CminRange_003E5__18, _003CmaxRange_003E5__17, _003C_003E8__3.ennemyToTarget.CellId));
															_003C_003E8__3.occupiedCells = CS_0024_003C_003E8__locals200.Fighter.BuildOccupiedCells(_003C_003E8__3.myself.CellId);
															int num5 = _671BC22C.BF9F3D1F.FE8E0C9E[1937](0, _003C_003E8__2.userDefinedSpell.CastAtDistTargetMin);
															int castAtDistTargetMax = _003C_003E8__2.userDefinedSpell.CastAtDistTargetMax;
															_003C_003E8__3.wantedTargetCells = new HashSet<int>();
															List<Cell>.Enumerator enumerator = CS_0024_003C_003E8__locals200.MapInformation.Cells.GetEnumerator();
															try
															{
																while (enumerator.MoveNext())
																{
																	Cell current = enumerator.Current;
																	if (current.Mov == 1 && current.Los != 0 && current.Visible != 0)
																	{
																		int num6 = _4CAC0493._52884196.E230AC25(current.CellNumber, _003C_003E8__3.ennemyToTarget.CellId);
																		if (num6 >= num5 && num6 <= castAtDistTargetMax)
																		{
																			_003C_003E8__3.wantedTargetCells.Add(current.CellNumber);
																		}
																	}
																}
															}
															finally
															{
																if (num < 0)
																{
																	((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
																}
															}
															CS_0024_003C_003E8__locals200.Verbose("WantedTargetCells", _003C_003E8__3.wantedTargetCells);
															List<_9F8EAF35> source2 = D98DE637._811AB8B8(CS_0024_003C_003E8__locals200.MapInformation.Cells, _003C_003E8__3.myself.CellId, _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel, _003CminRange_003E5__18, _003CmaxRange_003E5__17, _003C_003E8__3.occupiedCells);
															if (_003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel.NeedFreeCell)
															{
																source2 = source2.Where((_9F8EAF35 e) => !_003C_003E8__3.occupiedCells.ContainsKey((int)e.A984429C)).ToList();
															}
															CS_0024_003C_003E8__locals200.Verbose("CastableCells of current position", source2);
															list2 = source2.Where((_9F8EAF35 mp) => _003C_003E8__3.wantedTargetCells.Contains(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495))).ToList();
															CS_0024_003C_003E8__locals200.Verbose("ViableCastPoints", list2);
															_003CcellToTargetId_003E5__19 = null;
															_003CmaxMovementPoint_003E5__20 = _003C_003E8__3.myself.Characteristics[CharacteristicKeyword.MovementPoints].Value;
															if (list2.Count > 0)
															{
																_003CcellToTargetId_003E5__19 = (from mp in list2
																	orderby _4CAC0493._52884196.E230AC25(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495), _003C_003E8__3.ennemyToTarget.CellId)
																	select _8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495)).First();
																CS_0024_003C_003E8__locals200.Verbose((object)"CastCell.selected.noMove", _003CcellToTargetId_003E5__19.Value);
																goto IL_2522;
															}
															if (_003CmaxMovementPoint_003E5__20 != 0)
															{
																awaiter2 = CS_0024_003C_003E8__locals200.FindBestRepositionForCastByMP(_003C_003E8__3.myself.CellId, _003C_003E8__3.CS_0024_003C_003E8__locals1.spellLevel, _003CminRange_003E5__18, _003CmaxRange_003E5__17, _003C_003E8__3.wantedTargetCells, _003C_003E8__3.occupiedCells, _003CmaxMovementPoint_003E5__20).GetAwaiter();
																if (!awaiter2.IsCompleted)
																{
																	num = (_003C_003E1__state = 12);
																	_003C_003Eu__3 = awaiter2;
																	_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
																	return;
																}
																goto IL_21d9;
															}
															_671BC22C.BF9F3D1F.FE8E0C9E[376]();
															CS_0024_003C_003E8__locals200.Verbose("CastCell.noViableAndNoMP", true);
														}
														goto IL_282f;
													}
													_003C_003E8__1 = null;
													_003Cspell_003E5__7 = null;
													goto IL_287b;
													IL_1698:
													_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
													awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
													if (!awaiter.IsCompleted)
													{
														num = (_003C_003E1__state = 11);
														_003C_003Eu__1 = awaiter;
														_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
														return;
													}
													goto IL_1706;
													IL_26dd:
													_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
													if (_003C_003E8__2.userDefinedSpell.Cooldown > 0)
													{
														CS_0024_003C_003E8__locals200.SpellIdOnCooldown[_003C_003E8__2.userDefinedSpell.SpellId] = _003C_003E8__2.userDefinedSpell.Cooldown;
													}
													awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
													if (!awaiter.IsCompleted)
													{
														num = (_003C_003E1__state = 18);
														_003C_003Eu__1 = awaiter;
														_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
														return;
													}
													goto IL_278c;
													IL_1706:
													awaiter.GetResult();
													goto end_IL_110d;
													IL_22b4:
													if (!awaiter.GetResult())
													{
														_671BC22C.BF9F3D1F.FE8E0C9E[376]();
														CS_0024_003C_003E8__locals200.Verbose("MoveTo.failed", true);
														goto IL_282f;
													}
													combatSpeed = CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.CombatSpeed;
													if (combatSpeed != CombatSpeed.Rapide)
													{
														if (combatSpeed != CombatSpeed.Human)
														{
															goto IL_243f;
														}
														_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000));
														if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
														{
															num = (_003C_003E1__state = 15);
															_003C_003Eu__2 = _6D28EB9F;
															_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
															return;
														}
														goto IL_242c;
													}
													_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
													if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
													{
														num = (_003C_003E1__state = 14);
														_003C_003Eu__2 = _6D28EB9F;
														_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
														return;
													}
													goto IL_2393;
													IL_278c:
													awaiter.GetResult();
													goto end_IL_110d;
													IL_282f:
													_003Ci_003E5__16++;
													goto IL_2841;
													IL_2393:
													_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
													goto IL_243f;
													IL_2522:
													if (!_003CcellToTargetId_003E5__19.HasValue)
													{
														goto IL_2828;
													}
													if (_003CcurrentRound_003E5__4 != CS_0024_003C_003E8__locals200.Round)
													{
														CS_0024_003C_003E8__locals200.Verbose("RoundChanged.beforeCast", true);
														goto end_IL_10aa;
													}
													CS_0024_003C_003E8__locals200.Verbose("Cast.request", new global::BC3154A6<int, int, long>(_671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7), _003CcellToTargetId_003E5__19.Value, _003C_003E8__3.ennemyToTarget.ActorId));
													awaiter = CS_0024_003C_003E8__locals200.ForgeAndSendGameActionFightCastRequest(_003CcellToTargetId_003E5__19.Value, _671BC22C.BF9F3D1F.FE8E0C9E[84](_003Cspell_003E5__7)).GetAwaiter();
													if (!awaiter.IsCompleted)
													{
														num = (_003C_003E1__state = 16);
														_003C_003Eu__1 = awaiter;
														_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
														return;
													}
													goto IL_261a;
												}
												goto default;
												end_IL_110d:;
											}
											catch (Exception ex)
											{
												_003C_003E7__wrap14 = ex;
												_003CapCost_003E5__8 = 1;
												goto IL_287b;
											}
											break;
										case 19:
											_6D28EB9F = _003C_003Eu__2;
											_003C_003Eu__2 = default(TaskAwaiter);
											num = (_003C_003E1__state = -1);
											goto IL_296e;
										case 20:
											awaiter = _003C_003Eu__1;
											_003C_003Eu__1 = default(TaskAwaiter<bool>);
											num = (_003C_003E1__state = -1);
											goto IL_29dc;
										default:
											{
												if (_003C_003E7__wrap13.MoveNext())
												{
													_003C_003E8__2 = new _003C_003Ec__DisplayClass40_0();
													_003C_003E8__2.userDefinedSpell = _003C_003E7__wrap13.Current;
													_003CapCost_003E5__8 = 0;
													goto case 9;
												}
												goto end_IL_10a6;
											}
											IL_287b:
											_4AB61FB = _003CapCost_003E5__8;
											if (_4AB61FB == 1)
											{
												Exception ex2 = (Exception)_003C_003E7__wrap14;
												_671BC22C.BF9F3D1F.FE8E0C9E[376]();
												_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex2.ToString());
												CS_0024_003C_003E8__locals200.Verbose("EXCEPTION.innerUserDefinedSpellLoop", new global::_1D01AD97<string, string>(_671BC22C.BF9F3D1F.FE8E0C9E[2117](ex2), _671BC22C.BF9F3D1F.FE8E0C9E[605](ex2)));
												_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](CS_0024_003C_003E8__locals200.SendDiscordWebhookSafeAsync("Exception in userDefinedSpell loop"));
												if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
												{
													num = (_003C_003E1__state = 19);
													_003C_003Eu__2 = _6D28EB9F;
													_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
													return;
												}
												goto IL_296e;
											}
											goto IL_29e4;
											IL_29dc:
											awaiter.GetResult();
											goto IL_29e4;
											IL_29e4:
											_003C_003E7__wrap14 = null;
											_003C_003E8__2 = null;
											goto default;
											IL_296e:
											_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
											awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
											if (!awaiter.IsCompleted)
											{
												num = (_003C_003E1__state = 20);
												_003C_003Eu__1 = awaiter;
												_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
												return;
											}
											goto IL_29dc;
											end_IL_10aa:
											break;
										}
										goto end_IL_0fc3;
										end_IL_10a6:;
									}
									finally
									{
										if (num < 0)
										{
											((IDisposable)_003C_003E7__wrap13/*cast due to .constrained prefix*/).Dispose();
										}
									}
									_003C_003E7__wrap13 = default(List<UserDefinedSpell>.Enumerator);
									if (!CS_0024_003C_003E8__locals200._instanceData.CharacterData.IsFighting)
									{
										CS_0024_003C_003E8__locals200.Verbose("IsFighting.afterSpells", false);
									}
									else
									{
										self = CS_0024_003C_003E8__locals200.GetSelf();
										if (self == null)
										{
											_671BC22C.BF9F3D1F.FE8E0C9E[376]();
											CS_0024_003C_003E8__locals200.Verbose("Self.afterSpells", "null");
										}
										else
										{
											switch (CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.CombatAction)
											{
											case CombatAction.Idle:
												CS_0024_003C_003E8__locals200.Verbose("CombatAction", "Idle");
												goto IL_307c;
											case CombatAction.Melee:
												break;
											case CombatAction.Ranged:
												goto IL_2d8c;
											case CombatAction.Hybride:
												goto IL_2f78;
											default:
												goto IL_307c;
											}
											ActorFighter closestFighterOfCell = CS_0024_003C_003E8__locals200.Fighter.GetClosestFighterOfCell(self.CellId, CS_0024_003C_003E8__locals200.Defenders.Where((Defender e) => e.IsAlive).Cast<ActorFighter>().ToList());
											if (closestFighterOfCell == null)
											{
												CS_0024_003C_003E8__locals200.Verbose("Melee.closestDefender", "none");
											}
											else
											{
												if (_003CcurrentRound_003E5__4 == CS_0024_003C_003E8__locals200.Round)
												{
													CS_0024_003C_003E8__locals200.Verbose((object)"MoveCloserTo.cell", closestFighterOfCell.CellId);
													awaiter = CS_0024_003C_003E8__locals200.Fighter.MoveCloserTo(closestFighterOfCell.CellId, 100).GetAwaiter();
													if (!awaiter.IsCompleted)
													{
														num = (_003C_003E1__state = 21);
														_003C_003Eu__1 = awaiter;
														_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
														return;
													}
													goto IL_2bf0;
												}
												CS_0024_003C_003E8__locals200.Verbose("RoundChanged.beforeMeleeMove", true);
											}
										}
									}
									goto end_IL_0fc3;
								case 21:
									awaiter = _003C_003Eu__1;
									_003C_003Eu__1 = default(TaskAwaiter<bool>);
									num = (_003C_003E1__state = -1);
									goto IL_2bf0;
								case 22:
									_6D28EB9F = _003C_003Eu__2;
									_003C_003Eu__2 = default(TaskAwaiter);
									num = (_003C_003E1__state = -1);
									goto IL_2cb4;
								case 23:
									_6D28EB9F = _003C_003Eu__2;
									_003C_003Eu__2 = default(TaskAwaiter);
									num = (_003C_003E1__state = -1);
									goto IL_2d4d;
								case 24:
									awaiter = _003C_003Eu__1;
									_003C_003Eu__1 = default(TaskAwaiter<bool>);
									num = (_003C_003E1__state = -1);
									goto IL_2e03;
								case 25:
									_6D28EB9F = _003C_003Eu__2;
									_003C_003Eu__2 = default(TaskAwaiter);
									num = (_003C_003E1__state = -1);
									goto IL_2ec7;
								case 26:
									_6D28EB9F = _003C_003Eu__2;
									_003C_003Eu__2 = default(TaskAwaiter);
									num = (_003C_003E1__state = -1);
									goto IL_2f60;
								case 27:
									awaiter = _003C_003Eu__1;
									_003C_003Eu__1 = default(TaskAwaiter<bool>);
									num = (_003C_003E1__state = -1);
									goto IL_3060;
								case 28:
									{
										awaiter = _003C_003Eu__1;
										_003C_003Eu__1 = default(TaskAwaiter<bool>);
										num = (_003C_003E1__state = -1);
										break;
									}
									IL_2ec7:
									_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
									goto IL_307c;
									IL_2d8c:
									CS_0024_003C_003E8__locals200.Verbose("Ranged.move", "MoveAwayFromAllMonsters");
									awaiter = CS_0024_003C_003E8__locals200.Fighter.MoveAwayFromAllMonsters(100).GetAwaiter();
									if (!awaiter.IsCompleted)
									{
										num = (_003C_003E1__state = 24);
										_003C_003Eu__1 = awaiter;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
										return;
									}
									goto IL_2e03;
									IL_2e03:
									if (!awaiter.GetResult())
									{
										_671BC22C.BF9F3D1F.FE8E0C9E[376]();
									}
									combatSpeed = CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.CombatSpeed;
									if (combatSpeed != CombatSpeed.Rapide)
									{
										if (combatSpeed != CombatSpeed.Human)
										{
											goto IL_307c;
										}
										_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000));
										if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
										{
											num = (_003C_003E1__state = 26);
											_003C_003Eu__2 = _6D28EB9F;
											_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
											return;
										}
										goto IL_2f60;
									}
									_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
									if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
									{
										num = (_003C_003E1__state = 25);
										_003C_003Eu__2 = _6D28EB9F;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
										return;
									}
									goto IL_2ec7;
									IL_2bf0:
									if (!awaiter.GetResult())
									{
										_671BC22C.BF9F3D1F.FE8E0C9E[376]();
									}
									combatSpeed = CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.CombatSpeed;
									if (combatSpeed != CombatSpeed.Rapide)
									{
										if (combatSpeed != CombatSpeed.Human)
										{
											goto IL_2d60;
										}
										_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000));
										if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
										{
											num = (_003C_003E1__state = 23);
											_003C_003Eu__2 = _6D28EB9F;
											_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
											return;
										}
										goto IL_2d4d;
									}
									_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
									if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
									{
										num = (_003C_003E1__state = 22);
										_003C_003Eu__2 = _6D28EB9F;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
										return;
									}
									goto IL_2cb4;
									IL_2d4d:
									_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
									goto IL_2d60;
									IL_2f60:
									_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
									goto IL_307c;
									IL_2d60:
									if (_003CcurrentRound_003E5__4 == CS_0024_003C_003E8__locals200.Round)
									{
										goto IL_307c;
									}
									CS_0024_003C_003E8__locals200.Verbose("RoundChanged.afterMeleeMove", true);
									goto end_IL_0fc3;
									IL_2f78:
									num7 = _671BC22C.BF9F3D1F.FE8E0C9E[1937](1, CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.MinHybride);
									maxRange = _671BC22C.BF9F3D1F.FE8E0C9E[1937](num7, CS_0024_003C_003E8__locals200._instanceData.CharacterParameters.MaxHybride);
									safestReachableCellWithinRangeOfMonsters = CS_0024_003C_003E8__locals200.Fighter.GetSafestReachableCellWithinRangeOfMonsters(self.CellId, self.Characteristics[CharacteristicKeyword.MovementPoints].Value, num7, maxRange);
									awaiter = CS_0024_003C_003E8__locals200.Fighter.MoveTo(safestReachableCellWithinRangeOfMonsters, self.Characteristics[CharacteristicKeyword.MovementPoints].Value).GetAwaiter();
									if (!awaiter.IsCompleted)
									{
										num = (_003C_003E1__state = 27);
										_003C_003Eu__1 = awaiter;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
										return;
									}
									goto IL_3060;
									IL_307c:
									CS_0024_003C_003E8__locals200.Verbose("FinishTurn.afterMovement", true);
									awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
									if (!awaiter.IsCompleted)
									{
										num = (_003C_003E1__state = 28);
										_003C_003Eu__1 = awaiter;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
										return;
									}
									break;
									IL_3060:
									if (!awaiter.GetResult())
									{
										_671BC22C.BF9F3D1F.FE8E0C9E[376]();
									}
									goto IL_307c;
									IL_2cb4:
									_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
									goto IL_2d60;
								}
								awaiter.GetResult();
								goto IL_3110;
								end_IL_0fc3:;
							}
							catch (Exception ex3)
							{
								_003C_003E7__wrap12 = ex3;
								_003CnumberOfCast_003E5__9 = 1;
								goto IL_3110;
							}
							goto end_IL_00b1;
						case 29:
							_6D28EB9F = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter);
							num = (_003C_003E1__state = -1);
							goto IL_3203;
						case 30:
							{
								awaiter = _003C_003Eu__1;
								_003C_003Eu__1 = default(TaskAwaiter<bool>);
								num = (_003C_003E1__state = -1);
								goto IL_3271;
							}
							IL_05fe:
							_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
							goto IL_0611;
							IL_3203:
							_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
							awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 30);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_3271;
							IL_0566:
							_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
							goto IL_0611;
							IL_0611:
							if (!CS_0024_003C_003E8__locals200.IsFightingForTreasure)
							{
								_003CnumberOfCast_003E5__9 = 0;
								goto case 9;
							}
							CS_0024_003C_003E8__locals200.Verbose("TreasureMode", true);
							if (CS_0024_003C_003E8__locals200.Round >= 20)
							{
								CS_0024_003C_003E8__locals200.Verbose("Round", new global::B4298CA6<int, string>(CS_0024_003C_003E8__locals200.Round, "Surrender"));
								awaiter = CS_0024_003C_003E8__locals200.ForgeAndSendSurrend().GetAwaiter();
								if (!awaiter.IsCompleted)
								{
									num = (_003C_003E1__state = 3);
									_003C_003Eu__1 = awaiter;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
									return;
								}
								goto IL_06b9;
							}
							_003Cdefender_003E5__5 = CS_0024_003C_003E8__locals200.Defenders.Find((Defender d) => d.ActorId == CS_0024_003C_003E8__locals200.TreasureTargetId);
							if (_003Cdefender_003E5__5 == null)
							{
								CS_0024_003C_003E8__locals200.Verbose("TreasureDefender", "not_found");
							}
							else
							{
								CS_0024_003C_003E8__locals200.Verbose("TreasureDefender", _003Cdefender_003E5__5);
								List<JitsuriSpellItem> list3 = CS_0024_003C_003E8__locals200._instanceData.CharacterData.Spells.Where((JitsuriSpellItem e) => CS_0024_003C_003E8__locals200.TreasureSpell.ContainsKey(_671BC22C.BF9F3D1F.FE8E0C9E[84](e))).OrderBy(delegate(JitsuriSpellItem spell)
								{
									DetailedSpell detailedSpell3 = CS_0024_003C_003E8__locals200._instanceData.CharacterData.DetailedSpells[_671BC22C.BF9F3D1F.FE8E0C9E[84](spell)];
									return SpellLevelRepository.Repository[detailedSpell3.SpellLevels.Levels[_671BC22C.BF9F3D1F.FE8E0C9E[835](spell) - 1]].ApCost;
								}).ToList();
								if (list3.Count != 0)
								{
									CS_0024_003C_003E8__locals200.Verbose("Spells", list3);
									_003C_003E7__wrap5 = list3.GetEnumerator();
									goto case 4;
								}
								CS_0024_003C_003E8__locals200.Verbose("TreasureCompatibleSpells", "none");
							}
							goto end_IL_00b1;
							IL_045a:
							awaiter.GetResult();
							goto end_IL_00b1;
							IL_0fa7:
							awaiter.GetResult();
							_003Cdefender_003E5__5 = null;
							goto IL_329b;
							IL_3110:
							_4AB61FB = _003CnumberOfCast_003E5__9;
							if (_4AB61FB != 1)
							{
								break;
							}
							ex4 = (Exception)_003C_003E7__wrap12;
							_671BC22C.BF9F3D1F.FE8E0C9E[376]();
							_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex4.ToString());
							CS_0024_003C_003E8__locals200.Verbose("EXCEPTION.standardFlow", new global::_1D01AD97<string, string>(_671BC22C.BF9F3D1F.FE8E0C9E[2117](ex4), _671BC22C.BF9F3D1F.FE8E0C9E[605](ex4)));
							_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](CS_0024_003C_003E8__locals200.SendDiscordWebhookSafeAsync("Exception in standard combat flow"));
							if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
							{
								num = (_003C_003E1__state = 29);
								_003C_003Eu__2 = _6D28EB9F;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
								return;
							}
							goto IL_3203;
							IL_06b9:
							awaiter.GetResult();
							goto end_IL_00b1;
							IL_057e:
							_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000));
							if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
							{
								num = (_003C_003E1__state = 2);
								_003C_003Eu__2 = _6D28EB9F;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
								return;
							}
							goto IL_05fe;
							IL_3271:
							awaiter.GetResult();
							break;
						}
						_003C_003E7__wrap12 = null;
						goto IL_329b;
						end_IL_00b1:;
					}
					catch (Exception ex5)
					{
						_003C_003E7__wrap1 = ex5;
						_003C_003E7__wrap2 = 1;
						goto IL_329b;
					}
					goto end_IL_000e;
				case 31:
					_6D28EB9F = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_33a1;
				case 32:
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_340f;
					}
					IL_329b:
					_4AB61FB = _003C_003E7__wrap2;
					if (_4AB61FB != 1)
					{
						break;
					}
					ex6 = (Exception)_003C_003E7__wrap1;
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex6.ToString());
					D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](40, 1);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "EXCEPTION (outer HandleFightTurnEvent): ");
					D6A007B.AppendFormatted(ex6);
					CS_0024_003C_003E8__locals200.Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
					_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](CS_0024_003C_003E8__locals200.SendDiscordWebhookSafeAsync("Exception in HandleFightTurnEvent"));
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
					{
						num = (_003C_003E1__state = 31);
						_003C_003Eu__2 = _6D28EB9F;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
						return;
					}
					goto IL_33a1;
					IL_33a1:
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					awaiter = CS_0024_003C_003E8__locals200.FightTurnFinishRequest().GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (_003C_003E1__state = 32);
						_003C_003Eu__1 = awaiter;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						return;
					}
					goto IL_340f;
					IL_340f:
					awaiter.GetResult();
					break;
				}
				_003C_003E7__wrap1 = null;
				CS_0024_003C_003E8__locals200.Verbose("===== HandleFightTurnEvent: END =====");
				end_IL_000e:;
			}
			catch (Exception _52914E)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[545](ref _003C_003Et__builder, _52914E);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[993](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[875](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlaceFarFromMonster_003Ed__49 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public List<int> possiblePositions;

		public FighterManager _003C_003E4__this;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				try
				{
					TaskAwaiter<bool> awaiter;
					if (num == 0)
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0177;
					}
					int num2 = possiblePositions[0];
					int num3 = -1;
					List<int>.Enumerator enumerator = possiblePositions.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							int current = enumerator.Current;
							int num4 = int.MaxValue;
							_9F8EAF35 fF9851AF = _9F8EAF35.D6056589((uint)current);
							List<Defender>.Enumerator enumerator2 = fighterManager.Defenders.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									int num5 = _9F8EAF35.D6056589((uint)enumerator2.Current.CellId).FB2C1732(fF9851AF);
									if (num5 < num4)
									{
										num4 = num5;
									}
									if (num4 == 0)
									{
										break;
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
								}
							}
							if (num4 > num3)
							{
								num3 = num4;
								num2 = current;
							}
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
						}
					}
					Attacker self = fighterManager.GetSelf();
					if (self == null || self.CellId != num2)
					{
						awaiter = fighterManager.ForgeAndSendFightPlacementPositionRequest(fighterManager._instanceData.CharacterData.CharacterId, num2).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0177;
					}
					goto end_IL_0011;
					IL_0177:
					awaiter.GetResult();
					end_IL_0011:;
				}
				catch (Exception ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
				}
			}
			catch (Exception _341F081F)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[1494](ref _003C_003Et__builder, _341F081F);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[810](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[298](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlaceNearMonster_003Ed__50 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public List<int> possiblePositions;

		public FighterManager _003C_003E4__this;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				try
				{
					TaskAwaiter<bool> awaiter;
					if (num == 0)
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0160;
					}
					int num2 = possiblePositions[0];
					int num3 = 111111;
					List<int>.Enumerator enumerator = possiblePositions.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							int current = enumerator.Current;
							_9F8EAF35 fF9851AF = _9F8EAF35.D6056589((uint)current);
							List<Defender>.Enumerator enumerator2 = fighterManager.Defenders.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									int num4 = _9F8EAF35.D6056589((uint)enumerator2.Current.CellId).FB2C1732(fF9851AF);
									if (num4 < num3)
									{
										num3 = num4;
										num2 = current;
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
								}
							}
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
						}
					}
					Attacker self = fighterManager.GetSelf();
					if (self == null || self.CellId != num2)
					{
						awaiter = fighterManager.ForgeAndSendFightPlacementPositionRequest(fighterManager._instanceData.CharacterData.CharacterId, num2).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0160;
					}
					goto end_IL_0011;
					IL_0160:
					awaiter.GetResult();
					end_IL_0011:;
				}
				catch (Exception ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
				}
			}
			catch (Exception _341F081F)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[1494](ref _003C_003Et__builder, _341F081F);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[810](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[298](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlaceWithFreeAdjacents_003Ed__52 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public List<int> possiblePositions;

		public FighterManager _003C_003E4__this;

		private List<int>.Enumerator _003C_003E7__wrap1;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				try
				{
					if (num != 0)
					{
						_003C_003E7__wrap1 = possiblePositions.GetEnumerator();
					}
					try
					{
						TaskAwaiter<bool> awaiter;
						if (num == 0)
						{
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							goto IL_00d5;
						}
						while (_003C_003E7__wrap1.MoveNext())
						{
							int current = _003C_003E7__wrap1.Current;
							if (!fighterManager.IsValidPositionForTreasure(current))
							{
								continue;
							}
							Attacker self = fighterManager.GetSelf();
							if (self == null || self.CellId != current)
							{
								awaiter = fighterManager.ForgeAndSendFightPlacementPositionRequest(fighterManager._instanceData.CharacterData.CharacterId, current).GetAwaiter();
								if (!awaiter.IsCompleted)
								{
									num = (_003C_003E1__state = 0);
									_003C_003Eu__1 = awaiter;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
									return;
								}
								goto IL_00d5;
							}
							goto end_IL_0011;
						}
						goto end_IL_0029;
						IL_00d5:
						awaiter.GetResult();
						goto end_IL_0011;
						end_IL_0029:;
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
						}
					}
					_003C_003E7__wrap1 = default(List<int>.Enumerator);
					end_IL_0011:;
				}
				catch (Exception ex)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					_1E3B359C._6E3CC3BA(ex.ToString());
				}
			}
			catch (Exception _341F081F)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[1494](ref _003C_003Et__builder, _341F081F);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[810](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[298](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSendDiscordWebhookSafeAsync_003Ed__21 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public FighterManager _003C_003E4__this;

		public string reason;

		private string _003CtempPath_003E5__2;

		private MultipartFormDataContent _003Cmultipart_003E5__3;

		private ByteArrayContent _003CfileContent_003E5__4;

		private TaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<byte[]> _003C_003Eu__2;

		private TaskAwaiter<HttpResponseMessage> _003C_003Eu__3;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			FighterManager fighterManager = _003C_003E4__this;
			try
			{
				_ = 2;
				try
				{
					TaskAwaiter _6D28EB9F;
					if (num != 0)
					{
						if ((uint)(num - 1) <= 1u)
						{
							goto IL_0174;
						}
						string text = fighterManager._fightVerbose.ToString();
						if (_671BC22C.BF9F3D1F.FE8E0C9E[490](text))
						{
							text = "(no verbose data)";
						}
						object obj = _671BC22C.BF9F3D1F.FE8E0C9E[1899];
						string c1A5E80D = _671BC22C.BF9F3D1F.FE8E0C9E[2]();
						object obj2 = _671BC22C.BF9F3D1F.FE8E0C9E[806];
						Guid E419FA0D = _671BC22C.BF9F3D1F.FE8E0C9E[169]();
						_003CtempPath_003E5__2 = obj(c1A5E80D, obj2("verbose-log-", _671BC22C.BF9F3D1F.FE8E0C9E[1378](ref E419FA0D, "N"), ".txt"));
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1663](_003CtempPath_003E5__2, text, _671BC22C.BF9F3D1F.FE8E0C9E[236](), default(CancellationToken)));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
					}
					else
					{
						_6D28EB9F = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					_003Cmultipart_003E5__3 = _671BC22C.BF9F3D1F.FE8E0C9E[196]();
					goto IL_0174;
					IL_0174:
					try
					{
						TaskAwaiter<byte[]> awaiter;
						if (num != 1)
						{
							if (num == 2)
							{
								goto IL_0244;
							}
							_671BC22C.BF9F3D1F.FE8E0C9E[1469](_003Cmultipart_003E5__3, _671BC22C.BF9F3D1F.FE8E0C9E[185](reason), "content");
							awaiter = _671BC22C.BF9F3D1F.FE8E0C9E[365](_003CtempPath_003E5__2, default(CancellationToken)).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__2 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
						}
						else
						{
							awaiter = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter<byte[]>);
							num = (_003C_003E1__state = -1);
						}
						byte[] result = awaiter.GetResult();
						_003CfileContent_003E5__4 = _671BC22C.BF9F3D1F.FE8E0C9E[103](result);
						goto IL_0244;
						IL_0244:
						try
						{
							TaskAwaiter<HttpResponseMessage> awaiter2;
							if (num != 2)
							{
								_671BC22C.BF9F3D1F.FE8E0C9E[225](_671BC22C.BF9F3D1F.FE8E0C9E[1362](_003CfileContent_003E5__4), _671BC22C.BF9F3D1F.FE8E0C9E[40]("text/plain"));
								_671BC22C.BF9F3D1F.FE8E0C9E[105](_003Cmultipart_003E5__3, _003CfileContent_003E5__4, "file", _671BC22C.BF9F3D1F.FE8E0C9E[1233](_003CtempPath_003E5__2));
								awaiter2 = _671BC22C.BF9F3D1F.FE8E0C9E[1595](_httpClient, "https://discord.com/api/webhooks/1425624414883676324/87L-g5hrvfBYN8U5D-ysyv9nApEXaX7FDeFORFxs2v2BTODsX7gTG1bR_W0ECmjfjKEF", _003Cmultipart_003E5__3).GetAwaiter();
								if (!awaiter2.IsCompleted)
								{
									num = (_003C_003E1__state = 2);
									_003C_003Eu__3 = awaiter2;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
									return;
								}
							}
							else
							{
								awaiter2 = _003C_003Eu__3;
								_003C_003Eu__3 = default(TaskAwaiter<HttpResponseMessage>);
								num = (_003C_003E1__state = -1);
							}
							HttpResponseMessage result2 = awaiter2.GetResult();
							try
							{
								_671BC22C.BF9F3D1F.FE8E0C9E[1198](_003CtempPath_003E5__2);
							}
							catch
							{
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)result2)?.Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && _003CfileContent_003E5__4 != null)
							{
								((IDisposable)_003CfileContent_003E5__4).Dispose();
							}
						}
					}
					finally
					{
						if (num < 0 && _003Cmultipart_003E5__3 != null)
						{
							((IDisposable)_003Cmultipart_003E5__3).Dispose();
						}
					}
					_003CtempPath_003E5__2 = null;
					_003Cmultipart_003E5__3 = null;
					_003CfileContent_003E5__4 = null;
				}
				catch
				{
				}
			}
			catch (Exception _341F081F)
			{
				_003C_003E1__state = -2;
				_671BC22C.BF9F3D1F.FE8E0C9E[1494](ref _003C_003Et__builder, _341F081F);
				return;
			}
			_003C_003E1__state = -2;
			_671BC22C.BF9F3D1F.FE8E0C9E[810](ref _003C_003Et__builder);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[298](ref _003C_003Et__builder, stateMachine);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	internal List<Cell> cells = new List<Cell>();

	internal const int TreasureGid = 3724;

	internal InstanceData _instanceData;

	internal _668B0EAA _messageHandler;

	internal List<Attacker> Attackers;

	internal List<Defender> Defenders;

	internal static readonly HttpClient _httpClient = _671BC22C.BF9F3D1F.FE8E0C9E[598]();

	internal const string DiscordWebhookUrl = "https://discord.com/api/webhooks/1425624414883676324/87L-g5hrvfBYN8U5D-ysyv9nApEXaX7FDeFORFxs2v2BTODsX7gTG1bR_W0ECmjfjKEF";

	internal readonly StringBuilder _fightVerbose;

	private static readonly JsonSerializerSettings JsonLogSettings;

	internal static bool _verboseIncludeTimestamp;

	internal const int MaxLogChars = 1000000;

	public List<int> TurnOccupiedCells;

	internal int Round;

	private Dictionary<int, int> SpellIdOnCooldown;

	internal Dictionary<int, bool> TreasureSpell;

	[CompilerGenerated]
	private bool _003CIsFightingForTreasure_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CIsMyTurn_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CIsPlaced_003Ek__BackingField;

	[CompilerGenerated]
	private MapInformation _003CMapInformation_003Ek__BackingField;

	[CompilerGenerated]
	private long _003CTreasureTargetId_003Ek__BackingField;

	[CompilerGenerated]
	private Fighter _003CFighter_003Ek__BackingField;

	internal bool IsFightingForTreasure
	{
		[CompilerGenerated]
		get
		{
			ushort num = 26230;
			return _003CIsFightingForTreasure_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CIsFightingForTreasure_003Ek__BackingField = value;
		}
	}

	internal bool IsMyTurn
	{
		[CompilerGenerated]
		get
		{
			int num = -238254302;
			return _003CIsMyTurn_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 22832;
			_003CIsMyTurn_003Ek__BackingField = value;
		}
	}

	internal bool IsPlaced
	{
		[CompilerGenerated]
		get
		{
			short num = 0;
			return _003CIsPlaced_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CIsPlaced_003Ek__BackingField = value;
		}
	}

	internal MapInformation MapInformation
	{
		[CompilerGenerated]
		get
		{
			short num = 64;
			return _003CMapInformation_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = -492388203;
			do
			{
				_003CMapInformation_003Ek__BackingField = value;
			}
			while (((num + num) ^ num) * (1486364977 / num) >= num);
		}
	}

	internal long TreasureTargetId
	{
		[CompilerGenerated]
		get
		{
			byte b = 0;
			return _003CTreasureTargetId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CTreasureTargetId_003Ek__BackingField = value;
		}
	}

	internal bool IsLoadingMap { get; set; }

	internal Fighter Fighter
	{
		[CompilerGenerated]
		get
		{
			int num = 1;
			return _003CFighter_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = 11032;
			_003CFighter_003Ek__BackingField = value;
		}
	}

	internal void VerboseClear()
	{
		try
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1275](_fightVerbose);
		}
		catch
		{
		}
	}

	internal void Verbose(string message)
	{
		try
		{
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[744](message))
			{
				string text;
				if (!_verboseIncludeTimestamp)
				{
					text = message;
				}
				else
				{
					DefaultInterpolatedStringHandler D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](3, 2);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "[");
					D6A007B.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[1548](), "HH:mm:ss.fff");
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "] ");
					_671BC22C.BF9F3D1F.FE8E0C9E[311](ref D6A007B, message);
					text = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B);
				}
				string _2D88278F = text;
				_671BC22C.BF9F3D1F.FE8E0C9E[1063](_fightVerbose, _2D88278F);
				if (_671BC22C.BF9F3D1F.FE8E0C9E[701](_fightVerbose) > 1000000)
				{
					int num = 500000;
					string f01484B = _671BC22C.BF9F3D1F.FE8E0C9E[1141](_fightVerbose, _671BC22C.BF9F3D1F.FE8E0C9E[701](_fightVerbose) - num, num);
					_671BC22C.BF9F3D1F.FE8E0C9E[1275](_fightVerbose);
					_671BC22C.BF9F3D1F.FE8E0C9E[1063](_fightVerbose, "...[truncated]...");
					_671BC22C.BF9F3D1F.FE8E0C9E[117](_fightVerbose, f01484B);
				}
			}
		}
		catch
		{
		}
	}

	private static object ProjectForLog(object value, int maxDepth = 2, int maxItems = 64, int maxString = 200)
	{
		HashSet<int> seen = new HashSet<int>();
		return ProjectNode(value, maxDepth, maxItems, maxString, seen);
	}

	private static bool IsPrimitiveLike(System.Type t)
	{
		t = _671BC22C.BF9F3D1F.FE8E0C9E[1002](t) ?? t;
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[1016](t) && !_671BC22C.BF9F3D1F.FE8E0C9E[1137](t) && !_671BC22C.BF9F3D1F.FE8E0C9E[157](t, _671BC22C.BF9F3D1F.FE8E0C9E[180](typeof(string).TypeHandle)) && !_671BC22C.BF9F3D1F.FE8E0C9E[157](t, _671BC22C.BF9F3D1F.FE8E0C9E[180](typeof(decimal).TypeHandle)) && !_671BC22C.BF9F3D1F.FE8E0C9E[157](t, _671BC22C.BF9F3D1F.FE8E0C9E[180](typeof(DateTime).TypeHandle)) && !_671BC22C.BF9F3D1F.FE8E0C9E[157](t, _671BC22C.BF9F3D1F.FE8E0C9E[180](typeof(DateTimeOffset).TypeHandle)) && !_671BC22C.BF9F3D1F.FE8E0C9E[157](t, _671BC22C.BF9F3D1F.FE8E0C9E[180](typeof(TimeSpan).TypeHandle)))
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[157](t, _671BC22C.BF9F3D1F.FE8E0C9E[180](typeof(Guid).TypeHandle));
		}
		return true;
	}

	private static object ProjectNode(object obj, int depth, int maxItems, int maxString, HashSet<int> seen)
	{
		if (obj == null)
		{
			return null;
		}
		System.Type type = _671BC22C.BF9F3D1F.FE8E0C9E[869](obj);
		if (IsPrimitiveLike(type))
		{
			if (obj is string text)
			{
				if (_671BC22C.BF9F3D1F.FE8E0C9E[69](text) <= maxString)
				{
					return text;
				}
				return _671BC22C.BF9F3D1F.FE8E0C9E[1797](_671BC22C.BF9F3D1F.FE8E0C9E[2035](text, 0, maxString), "...(truncated)");
			}
			return obj;
		}
		int item = _671BC22C.BF9F3D1F.FE8E0C9E[1037](obj);
		if (!seen.Add(item))
		{
			return "<cyclic-ref>";
		}
		if (depth <= 0)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[806]("<", _671BC22C.BF9F3D1F.FE8E0C9E[1488](type), " …>");
		}
		if (obj is IDictionary _738AF)
		{
			List<object> list = new List<object>();
			int num = 0;
			{
				IDictionaryEnumerator dictionaryEnumerator = _671BC22C.BF9F3D1F.FE8E0C9E[357](_738AF);
				try
				{
					while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](dictionaryEnumerator))
					{
						DictionaryEntry B795D = (DictionaryEntry)_671BC22C.BF9F3D1F.FE8E0C9E[107](dictionaryEnumerator);
						if (num++ >= maxItems)
						{
							list.Add("...(truncated)");
							break;
						}
						list.Add(new global::_3D39A214<string, object>(_671BC22C.BF9F3D1F.FE8E0C9E[200](ref B795D)?.ToString(), ProjectNode(_671BC22C.BF9F3D1F.FE8E0C9E[381](ref B795D), depth - 1, maxItems, maxString, seen)));
					}
				}
				finally
				{
					IDisposable disposable = dictionaryEnumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			return list;
		}
		if (obj is IEnumerable _23314F9D)
		{
			List<object> list2 = new List<object>();
			int num2 = 0;
			{
				IEnumerator enumerator = _671BC22C.BF9F3D1F.FE8E0C9E[98](_23314F9D);
				try
				{
					while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
					{
						object obj2 = _671BC22C.BF9F3D1F.FE8E0C9E[107](enumerator);
						if (num2++ >= maxItems)
						{
							list2.Add("...(truncated)");
							break;
						}
						list2.Add(ProjectNode(obj2, depth - 1, maxItems, maxString, seen));
					}
				}
				finally
				{
					IDisposable disposable2 = enumerator as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
				}
			}
			return list2;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		FieldInfo[] array = _671BC22C.BF9F3D1F.FE8E0C9E[977](type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in array)
		{
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[769](fieldInfo) && !_671BC22C.BF9F3D1F.FE8E0C9E[2005](_671BC22C.BF9F3D1F.FE8E0C9E[1488](fieldInfo), "<"))
			{
				object obj3 = null;
				try
				{
					obj3 = _671BC22C.BF9F3D1F.FE8E0C9E[368](fieldInfo, obj);
				}
				catch
				{
					obj3 = "<unreadable>";
				}
				dictionary[_671BC22C.BF9F3D1F.FE8E0C9E[1488](fieldInfo)] = ProjectNode(obj3, depth - 1, maxItems, maxString, seen);
			}
		}
		PropertyInfo[] array2 = _671BC22C.BF9F3D1F.FE8E0C9E[1761](type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (PropertyInfo propertyInfo in array2)
		{
			if (_671BC22C.BF9F3D1F.FE8E0C9E[16](propertyInfo).Length != 0)
			{
				continue;
			}
			MethodInfo methodInfo = _671BC22C.BF9F3D1F.FE8E0C9E[1809](propertyInfo, _630F2096: true);
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[43](methodInfo, null))
			{
				object obj5 = null;
				try
				{
					obj5 = _671BC22C.BF9F3D1F.FE8E0C9E[943](methodInfo, obj, null);
				}
				catch
				{
					obj5 = "<unreadable>";
				}
				dictionary[_671BC22C.BF9F3D1F.FE8E0C9E[1488](propertyInfo)] = ProjectNode(obj5, depth - 1, maxItems, maxString, seen);
			}
		}
		if (dictionary.Count == 0)
		{
			return obj.ToString();
		}
		return dictionary;
	}

	internal void Verbose(string label, object value, int maxChars = 10000)
	{
		try
		{
			string text = ((!(value is string text2)) ? _671BC22C.BF9F3D1F.FE8E0C9E[1601](ProjectForLog(value, 4, 1024, 16192), JsonLogSettings) : text2);
			if (text != null && _671BC22C.BF9F3D1F.FE8E0C9E[69](text) > maxChars)
			{
				text = _671BC22C.BF9F3D1F.FE8E0C9E[1797](_671BC22C.BF9F3D1F.FE8E0C9E[2035](text, 0, maxChars), "...(truncated)");
			}
			Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[806](label, " => ", text));
		}
		catch (Exception ex)
		{
			DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
			_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 24, 3);
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref ADB0868E, label);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " => <projection_error:");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref ADB0868E, _671BC22C.BF9F3D1F.FE8E0C9E[1488](_671BC22C.BF9F3D1F.FE8E0C9E[1243](ex)));
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ":");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref ADB0868E, _671BC22C.BF9F3D1F.FE8E0C9E[2117](ex));
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ">");
			Verbose(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E));
		}
	}

	internal void Verbose(object value, int maxChars = 10000)
	{
		Verbose("data", value, maxChars);
	}

	internal void Verbose(Func<string> messageFactory)
	{
		try
		{
			Verbose(messageFactory());
		}
		catch
		{
		}
	}

	internal void Verbose(string label, Func<object> objFactory, int maxChars = 10000)
	{
		try
		{
			Verbose(label, objFactory(), maxChars);
		}
		catch
		{
		}
	}

	[AsyncStateMachine(typeof(_003CSendDiscordWebhookSafeAsync_003Ed__21))]
	internal Task SendDiscordWebhookSafeAsync(string reason = "HandleFightTurnEvent alert")
	{
		_003CSendDiscordWebhookSafeAsync_003Ed__21 stateMachine = default(_003CSendDiscordWebhookSafeAsync_003Ed__21);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
		stateMachine._003C_003E4__this = this;
		stateMachine.reason = reason;
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
	}

	internal FighterManager(_668B0EAA messageHandler, InstanceData instanceData)
	{
		sbyte b = -69;
		if ((uint)(-1809907007 * b) < (uint)(-(1385133604 % b)))
		{
			b = (sbyte)((uint)(b << (b << (int)b) + (b >> 24)) / (uint)b);
			goto IL_0036;
		}
		goto IL_0952;
		IL_0952:
		MapInformation = new MapInformation();
		sbyte b2 = default(sbyte);
		_671BC22C.BF9F3D1F.FE8E0C9E[(-1336371936 >>> ((b | ~b2) & 0x1FF9B77)) - 3544](this);
		if ((b & 0xE1E4B86) != 0)
		{
			b = (sbyte)(-(0x7320A988 & (-1212616830 << 1722013609 + b)) + 1929422900);
			goto IL_0036;
		}
		return;
		IL_0036:
		short num2 = default(short);
		ushort num = default(ushort);
		while (true)
		{
			switch ((uint)b % 8u)
			{
			default:
				b = (sbyte)((b % -1978337268) ^ -69);
				Attackers = new List<Attacker>();
				if ((uint)(b | 0x6C1D1C93) % ((uint)(-585196913 | b) % (uint)(b << (int)b)) != 0)
				{
					b = (sbyte)((int)((((uint)b % (uint)b > 3056319792u) ? 1u : 0u) & (uint)((int)(2677114687u / (uint)b) / (int)b)) % 1764335106 - -41);
					continue;
				}
				goto IL_0a7c;
			case 1u:
				b = (sbyte)(((2432991542u > (uint)(((b < b) ? 1 : 0) >> (int)b)) ? 1u : 0u) % (uint)(b + b + (1402953480 + (b ^ b))) - 70);
				Defenders = new List<Defender>();
				_fightVerbose = _671BC22C.BF9F3D1F.FE8E0C9E[217 + (-1756 ^ b)](-290652 ^ (b - 28439));
				b2 = (sbyte)((uint)b / (uint)b - (uint)b % (uint)(byte)b);
				b = (sbyte)((ushort)b2 + -65501);
				continue;
			case 2u:
				b = (sbyte)(-548586199 + (b2 + 548586163));
				TurnOccupiedCells = new List<int>();
				SpellIdOnCooldown = new Dictionary<int, int>();
				TreasureSpell = new Dictionary<int, bool>
				{
					{
						(b ^ b2) + 13006,
						(byte)(1 + 2752797583u / (uint)(~(-1836271314 & (b / -440410323)))) != 0
					},
					{
						((b / (-838130904 >>> (int)b) % 1731224720) ^ ((800456110 << (int)b2) / b2)) - -13141,
						(byte)(0xE880001 ^ (((2u / (uint)(b << (int)b2)) | (uint)(b & 0x76B103A6)) << 69046295 + b)) != 0
					},
					{
						(0 | b) - -13086,
						(byte)((-1146579163 / (int)(((-1138900163 >>> (int)b2 > b2) ? 1u : 0u) >> (1335549964 >> (int)b2))) ^ -1146579164) != 0
					},
					{
						b + 13052,
						(byte)((b ^ 0) + 70) != 0
					},
					{
						-12581 ^ b2,
						(byte)(-40146 + (ushort)(2114536714u / (uint)(-b))) != 0
					},
					{
						b2 ^ -14054,
						(byte)((0 - (uint)(1293373320 % b) % 3456532757u) * 56 + 3361) != 0
					},
					{
						13305 + (-b2 << (int)((uint)((b == b) ? ((sbyte)1) : ((sbyte)0)) / 405447096u)),
						(byte)(b2 % 1404526496 + 34) != 0
					},
					{
						(int)(((uint)(~(b ^ b2) / (202059168 << (1711558925 >>> (int)b2))) | ((uint)(0x3F978488 ^ b2) / (uint)b2)) + 13353),
						(byte)(((839362081 - (0x312B2B0B ^ ((uint)b % 1345006103u))) | 0x549AE8A8) - 4156292011u) != 0
					},
					{
						(int)((uint)((b % b * b) | -1220903928) | (2436149529u / (uint)b + 1328146361 << (b2 % -870097120 >> (int)b))) - -1220916830,
						(byte)((~b2 >> 28) ^ 1) != 0
					},
					{
						~(-103821072 + (b - b)) - 103808139,
						(byte)(-5 + (((765036428 - b2) % (1010 / b)) & (-888695751 - (b % -1004828740 + (590345757 >>> (int)b2))))) != 0
					},
					{
						b2 + 13278,
						(byte)((b2 - (-1182681457 >> (b2 >>> (int)b))) ^ -31) != 0
					},
					{
						(int)((0x2DC32582 ^ (3587042034u / (uint)(b & 0x1A972737)) ^ (uint)b2) + 767777164),
						(byte)(-649068614 ^ (0x26B00000 ^ b)) != 0
					},
					{
						(int)(13254 + (0 - 3700389811u / ~((uint)b / (uint)b2))),
						(byte)((byte)(1930706099u / (uint)b2) + 1) != 0
					},
					{
						(1496800018 >>> (0x63D2 ^ b2)) ^ 0x2FB93,
						(byte)(b2 - 521657747 + 521657781) != 0
					},
					{
						b2 - -1 - -12910,
						(byte)(0x4607BEBA ^ (b + 1174912768)) != 0
					},
					{
						b2 + 0 - -25922,
						(byte)(1u + (((int)((uint)(b >> b2 % -1591860608) / 2492425014u) < (int)b) ? 1u : 0u)) != 0
					},
					{
						0x650E ^ (sbyte)(1026030243u % ((1948568859u < (uint)b2) ? 1u : 0u) >> b * (b + b)),
						(byte)(-b2 + b + (b ^ (220927276 << (int)b)) + 1610612842) != 0
					},
					{
						(-1977704057 * (-5224032 & (b2 - b)) - (ushort)(1319355571u % (uint)b2)) ^ 0x43D3093D,
						(byte)(-(-(-2087942264 * b * -1371574217)) + -370236135) != 0
					},
					{
						((b + ~b == b2) ? 1 : 0) ^ 0x6523,
						(byte)((b >>> ((985347644u < (uint)b) ? 1 : 0)) ^ 0x7FFFFFDC) != 0
					},
					{
						(b * 1742164692) ^ 0x2F69E25,
						(byte)(649089 + (-1163358805 / b2 << b * b / b2 >> 9)) != 0
					},
					{
						(int)((uint)(-b ^ (b2 >> (b >>> 23))) / (uint)(~(b2 % b / -1073741824)) + 13063),
						(byte)(4 + (int)((((uint)b2 < (uint)(~b2)) ? 1u : 0u) ^ (uint)b) % (int)b2) != 0
					},
					{
						589489033 - b2 - 589475492,
						(byte)(-134217736 + (-(b2 << 27) + 92512770 % b2)) != 0
					},
					{
						(int)(0x349D ^ ((uint)(-b) % (uint)((-1154661576 | b) >> (int)b2))),
						(byte)(1683947369 + ((-1321502953 >>> ((int)((uint)b2 % (uint)b) >> 19)) + -362444415)) != 0
					},
					{
						12735 + (b2 >> b - (b - 806316588) + ((2030557483 - b) ^ (-1397315674 * b2))),
						(byte)(3 + ~((uint)b2 / (uint)b)) != 0
					},
					{
						~b - -12655,
						(byte)(1 + -((-626909050 >>> (int)b2) / b)) != 0
					},
					{
						(int)(~((uint)b % (uint)b2) + 12714),
						(byte)(ushort)((uint)(b & b) / 2994457347u) != 0
					},
					{
						12785 + ((~(2920251392u / (uint)b) != 0) ? 1 : 0),
						(byte)((218985272 - b2) ^ 0xD0D7358) != 0
					},
					{
						-13446 ^ (int)((uint)b2 / (uint)(b / b)),
						(byte)(-493395053 * b2 + 897832436) != 0
					},
					{
						(0xF09F & b) - 48107,
						(byte)(1 ^ (10496u / (uint)b)) != 0
					},
					{
						(int)(0x3467 ^ ((uint)(b2 >>> (((uint)b > 3886623416u) ? 1 : 0)) & (0xB1E4E8B6u & ((2215851908u < (uint)(-2012036068 << (int)b2)) ? 1u : 0u)))),
						(byte)(0x3FFE ^ (b >>> 18)) != 0
					},
					{
						(int)((((b > b2) ? 1u : 0u) | (uint)(b & (-122483265 >>> (int)b))) + 13381),
						(byte)((50802856 - ~b2 >> 0) - 50802823) != 0
					},
					{
						0x360C ^ ((2067723406 > ((328215574 > (b2 ^ -417369211)) ? 1 : 0)) ? 1 : 0),
						(byte)(0x57228 ^ (-234524119 & ((b2 & (b % 1789994172)) >>> 7))) != 0
					},
					{
						(b2 >> ~((int)(byte)b % (int)(~((b > b) ? 1u : 0u)))) - -13867,
						(byte)((int)((uint)(0 >>> (int)b2) / 3993658627u) - (0 + b2) * (b & 0x2F901AB3) - 563310866) != 0
					},
					{
						-14636 ^ b2,
						(byte)((int)(((3543444511u < (uint)(0x77237618 ^ b)) ? 1u : 0u) << (int)b2) / ~((b2 / b2) | (b & b)) - -1) != 0
					},
					{
						0x26106301 ^ ((b2 * 720752304) & -1346864507),
						(byte)((uint)(b2 >>> (int)b) % (uint)(629131720 % (int)((uint)b % (uint)b2)) + 1) != 0
					},
					{
						b * 178772918 - 549556880,
						(byte)((int)((uint)b % (uint)(b2 & b2)) % (int)b2 - -4) != 0
					},
					{
						(int)(0x358A ^ ((uint)(-1835273430 >> (int)(short)(-1752595065 / b2)) / 697098641u)),
						(byte)((1992290111 << -16988 - (sbyte)((uint)b2 / (uint)b)) ^ -1241548295) != 0
					},
					{
						(int)((uint)(926349239 % (-1356831844 & b) << 17) / (uint)(b % 27794372)) - -13755,
						(byte)(0x251A68 ^ (-b * -1808655231 / -(b ^ b2))) != 0
					},
					{
						(int)((uint)(b << (b >> (int)b)) % 3275412748u - 2147469844),
						(byte)(b - 734 - -804) != 0
					},
					{
						(370193184 << (int)b << 15 >> (int)b) ^ (1863409194 / (b2 >> (int)b2)) ^ -1863386441,
						(byte)(((ushort)((uint)(b & -787791182) / 1u) - -1630155392) ^ 0x612A7533) != 0
					},
					{
						-194965606 ^ (0xB9EAA9A ^ b2),
						(uint)((b & b) % (1580075532 + b)) > (uint)((b & b2) << (1914432303 >> (int)b))
					},
					{
						348 + (int)((uint)(b2 % b) % 3317093935u) / 412281622,
						(uint)b > ((((b < b2) ? 1u : 0u) < (uint)(b2 << (int)b2)) ? 1u : 0u) / 3465616133u
					},
					{
						b + 1538217256 - 1538193181,
						(byte)(70 + b) != 0
					}
				};
				num2 = (short)(b + -178336804);
				if (-1813377842 - (num2 - ((-1925282680 << (int)num2) | num2)) != 0)
				{
					continue;
				}
				goto IL_0afc;
			case 3u:
				break;
			case 4u:
				b = (sbyte)((int)((((b ^ b) > 379525655) ? 1u : 0u) & 0xFFFF8276u) + -69);
				goto IL_09c8;
			case 5u:
				b = (sbyte)(-1980 ^ (((ushort)b2 >>> (int)b) | b));
				_instanceData = instanceData;
				if ((num >>> (num2 | num)) / (int)(2288953850u % (uint)(470590489 + b2)) - -234465280 < (int)((uint)(num2 >> (b2 >> 5)) % 3946763250u))
				{
					b = (sbyte)((int)(0 ^ (0u % (uint)num)) - -30);
					continue;
				}
				goto IL_09c8;
			case 6u:
				b = (sbyte)(-(num % ((1908076332 >>> (int)b) - b)) ^ 0x44);
				goto IL_0a7c;
			case 7u:
				{
					b = (sbyte)(0x900 ^ (short)num);
					goto IL_0afc;
				}
				IL_09c8:
				do
				{
					_messageHandler = messageHandler;
					num = (ushort)((b2 - -811267804) % (((b2 ^ 0x60AD979A) + b) & b2));
				}
				while ((uint)(num2 * 514029472) < (uint)num);
				b = (sbyte)((num2 * -1298767095 >> (int)num2) + 136054785 - 136054852);
				continue;
				IL_0a7c:
				_messageHandler._7898AEBB += HandleMessage;
				b2 = (sbyte)(((num * b2) & (num2 << (int)(1488019628u % (uint)b))) - -1626254842);
				if ((int)((uint)(((b2 << 12) & -1) - (num / b2 + b)) / 682581288u) <= b2 * -890930952)
				{
					b = (sbyte)((((uint)(1034294915 / b2 - b % -164871254 / 363995296) > (uint)(-b ^ -877920339)) ? 1 : 0) - -22);
					continue;
				}
				break;
				IL_0afc:
				Fighter = new Fighter(this, _instanceData);
				return;
			}
			break;
		}
		goto IL_0952;
	}

	[AsyncStateMachine(typeof(_003CHandleChallengeAddEvent_003Ed__23))]
	internal void HandleChallengeAddEvent()
	{
		byte b = 78;
		b = (byte)((short)((short)b - b % 478708153) - ~b - -93);
		_003CHandleChallengeAddEvent_003Ed__23 stateMachine = default(_003CHandleChallengeAddEvent_003Ed__23);
		while (true)
		{
			switch ((uint)b % 4u)
			{
			case 1u:
				b = (byte)((((-261873009 & -b) > -1363314561) ? 1u : 0u) ^ 0xBBu);
				stateMachine._003C_003E1__state = (b ^ -1584901730) + 1584901851;
				b = (byte)(~b);
				b = (byte)(((b | 0x72EFD058) >> 35 * b) % (int)(b | (1269556141u % (uint)b)) - -95);
				continue;
			case 2u:
				b = (byte)((int)b + ((b > b) ? 1 : 0) - 97);
				do
				{
					stateMachine._003C_003Et__builder.Start(ref stateMachine);
				}
				while ((uint)b % 2475957937u % 1681705349 == 0);
				b = (byte)(0xAE ^ (b & -1027770085));
				continue;
			case 3u:
				b = (byte)((((b > (uint)b % (uint)(~(b % b))) ? 1u : 0u) << (b << (int)b) + (0x4E0779A1 ^ b)) - 4294967227u);
				return;
			}
			b = (byte)(((int)b / (int)(~(0 - (uint)b / 213331104u)) << b / ~(-(b >>> (int)b))) - -180355150);
			stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[b - -1724]();
			do
			{
				stateMachine._003C_003E4__this = this;
				b = (byte)(((uint)(-1547074439 ^ b) % (uint)(-1660807922 ^ (b / b))) | 0x2D9983B2);
			}
			while (-291772109 + (b << 1) == 0);
			b = (byte)(977953150 / ~(b / 1404844986) + 977953303);
		}
	}

	internal async Task<bool> WaitForAllMembersInFight()
	{
		if (!_instanceData.PartyManager._8B821C87)
		{
			return true;
		}
		TaskAwaiter taskAwaiter = default(TaskAwaiter);
		while (true)
		{
			_003C_003Ec__DisplayClass24_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass24_0();
			List<long> e5BCD62A = _instanceData.PartyManager.E5BCD62A;
			CS_0024_003C_003E8__locals2.attackerIds = Attackers.Select((Attacker a) => a.ActorId);
			if (e5BCD62A.All((long id) => CS_0024_003C_003E8__locals2.attackerIds.Contains(id)))
			{
				return true;
			}
			if (Round > 0)
			{
				break;
			}
			TaskAwaiter _6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](50));
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
			{
				await _6D28EB9F;
				_6D28EB9F = taskAwaiter;
				taskAwaiter = default(TaskAwaiter);
			}
			_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
		}
		return false;
	}

	[AsyncStateMachine(typeof(_003CHandleChallengeListEvent_003Ed__25))]
	internal void HandleChallengeListEvent(ByteString value)
	{
		_003CHandleChallengeListEvent_003Ed__25 stateMachine = default(_003CHandleChallengeListEvent_003Ed__25);
		uint num;
		do
		{
			stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1802]();
			num = 114358289u;
		}
		while (num == 0);
		stateMachine._003C_003E1__state = ((int)num >> (int)((num ^ num) << (int)((num + num) ^ num))) - 114358290;
		int num2 = (int)(16496463 - num);
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
	}

	[AsyncStateMachine(typeof(_003CHandleChallengeProposalEvent_003Ed__26))]
	internal void HandleChallengeProposalEvent(ByteString value)
	{
		_003CHandleChallengeProposalEvent_003Ed__26 stateMachine = default(_003CHandleChallengeProposalEvent_003Ed__26);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1802]();
		ushort num = 0;
		ushort num2 = default(ushort);
		while (true)
		{
			switch ((uint)num % 3u)
			{
			default:
				stateMachine._003C_003E4__this = this;
				num2 = (ushort)(num * -1569233441);
				num = (ushort)(-538983904 + (539019782 >> (int)((uint)(num2 << 23 >>> 2) ^ ((num2 << (int)num < num + -1170641763) ? 1u : 0u))));
				break;
			case 1u:
				num = (ushort)(-(num - (num2 + num)) << 27);
				do
				{
					stateMachine.value = value;
					stateMachine._003C_003E1__state = ((0x61AF16BA ^ num) - num2) ^ -1638864571;
					stateMachine._003C_003Et__builder.Start(ref stateMachine);
				}
				while (~(3451772713u % (uint)(~((-1643437543 % ~num) | num2))) == 0);
				num = (ushort)((num & -(num + -29737693)) ^ 0x7CB5);
				break;
			case 2u:
				num = (ushort)((-28367 >>> (int)num >> 5) + -63);
				return;
			}
		}
	}

	[AsyncStateMachine(typeof(_003CHandleEntitiesDispositionEvent_003Ed__27))]
	internal void HandleEntitiesDispositionEvent(ByteString value)
	{
		_003CHandleEntitiesDispositionEvent_003Ed__27 stateMachine = default(_003CHandleEntitiesDispositionEvent_003Ed__27);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1802]();
		sbyte b = -6;
		b = (sbyte)(4 + (b ^ b));
		ushort num = default(ushort);
		while (true)
		{
			switch ((uint)b % 4u)
			{
			default:
				b = (sbyte)((((byte)b / b) ^ (b % b)) - 7);
				stateMachine._003C_003E4__this = this;
				b = (sbyte)((uint)(b << (-528175333 >>> (int)b)) / (uint)b);
				b = (sbyte)(0x1B3D7591 ^ (0x1B3D7584 ^ b));
				break;
			case 1u:
				b = (sbyte)((uint)(b >>> 13 >> 1) % (uint)(0x1C84C31A | (b - 1671717397)) >> 14);
				do
				{
					stateMachine.value = value;
				}
				while (~b * (-574783189 % (17443 / ~b)) <= (int)((uint)b % ~(3309268014u / (uint)(~b))));
				b = (sbyte)(b * (b + b) + 46);
				break;
			case 2u:
				b = (sbyte)(-1 + (b >>> 5));
				stateMachine._003C_003E1__state = -1 ^ (b << (int)b);
				num = (ushort)(-(-(-435534303 * b) * -460855390));
				b = (sbyte)(((b + 13 + (int)((uint)b / 3231808480u)) ^ ((1275217801 / ~b) ^ ~b)) + -1275217794);
				break;
			case 3u:
				b = (sbyte)(b / ~(num << 22) * (1670198149 + (b - num) + (-527777740 >>> (int)num)) - 867706060);
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				return;
			}
		}
	}

	internal void HandleEvent(Event gameEvent)
	{
		_79AC42A1 value;
		uint num2 = default(uint);
		short num4 = default(short);
		byte b = default(byte);
		short num3 = default(short);
		while (_messageHandler._8401EA01.TryGetValue(_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](gameEvent)), out value))
		{
			int num = -2070166902;
			if (num % (int)(~((420614017 / num < (-768914253 >>> num) % ((num < 87378468) ? 1 : 0)) ? 1u : 0u)) == 0)
			{
				num = ((num * num * 1471496705 == 336857100) ? 1 : 0) + 454148100;
				goto IL_0081;
			}
			goto IL_09ab;
			IL_09ab:
			num2 = ~num2;
			if ((((uint)num < (uint)(774804666 >> num)) ? 1 : 0) > (int)(0x98273812u | ((uint)(-77 & num) % ((uint)(num ^ num) | (0xE831CC33u ^ num2)))))
			{
				num = 0x4B4933A8 ^ ((num + (int)(((uint)num ^ num2) & 0x922B4BF)) % (-551102333 >>> (num << num)));
				goto IL_0081;
			}
			break;
			IL_0081:
			while (true)
			{
				switch ((uint)num % 36u)
				{
				default:
					num = (int)(0 - (uint)num % 2729658906u / (uint)((num & num) | num)) + -2070166901;
					goto IL_0135;
				case 1u:
					num = (int)(((((uint)num % 809450687u) | 0x7513) & (uint)(-(866211256 % num))) - 2096625286);
					if ((int)value <= (0x11E ^ ((int)(243959996u % (uint)(num >>> 2)) % num >> -1952364376 % ((short)num - -727027404))))
					{
						num = 0xE09DE9F & (num - (int)(1216420286u % (uint)num) - (((int)num2 > 489215384 - num) ? 1 : 0));
						num = (849467295 % (int)(772305971 % ~num2 << (num & 0x287F1B70))) ^ 0x13A3F7DE;
						continue;
					}
					if (num / 622089136 != 0)
					{
						num = (0x1D29B5B7 & ((0x1527F427 | (num << num)) ^ (num >>> num))) - 704447253;
						continue;
					}
					goto IL_12a3;
				case 2u:
					num = (int)(num2 % ~(num2 & num2) + 0) - -201885835;
					if (value != (_79AC42A1)(0x1DE ^ ((int)num2 * -1610242048)))
					{
						goto IL_0262;
					}
					goto IL_0d0b;
				case 3u:
					num = -(~num) ^ -508521213;
					goto IL_029c;
				case 4u:
					num = -18027477 * num4 + 2082749622;
					if (value != (_79AC42A1)(1442358 + (num4 << 6)))
					{
						if (555280276 >>> num < (int)(0 - 2325652874u % (uint)(0x6FACACA8 ^ num4)) >> (int)(num2 | (uint)(-1850319092 + (int)num2) | num2))
						{
							num = (int)(289064080 - num2 - 488483199);
							continue;
						}
						goto IL_029c;
					}
					Reset();
					b = (byte)(num % (int)(3718583324u % (uint)(num4 ^ num)));
					num = (b ^ ((((int)(~num2) < -1389743470) ? 1 : 0) >> num)) - 1489472977;
					continue;
				case 5u:
					num = 201885835 + (int)((uint)(num4 % num4) % 1234475048u) * (num % 1461138182);
					return;
				case 6u:
					num = (int)(((0xBE0F8383u ^ num2) * 353259302) ^ 0x57055B2E) / (-542253562 << 6 / (int)(~num2)) + -2070166899;
					if ((int)value <= (int)(513 + (uint)(0x63402862 & ((int)num2 / -199313878 - (int)num2)) / (uint)num))
					{
						num2 -= 3811107487u;
						num = -676253789 ^ ((int)num2 >> (num % (1150814265 - num) >>> 25));
						continue;
					}
					if (value != (_79AC42A1)((num2 | 0x2134BEAB) ^ 0x2134BCA4))
					{
						break;
					}
					if (((uint)(num - -1837039556 << 21 << 6) | ((2727150626u > (uint)(0xFB52D1D & (1627601034 * num))) ? 1u : 0u)) != 0)
					{
						num = (int)(num2 / (uint)(~((num >> 7) * (int)num2 << (int)num2 * -1061187671))) - -1326545336;
						continue;
					}
					goto IL_0a5c;
				case 7u:
					num = -687391926 + 436144576 * num * (int)(num2 << (-844645600 & ((int)num2 + num)));
					if (value != (_79AC42A1)((int)(~num2) / (int)num2 + 508))
					{
						if ((int)((0 - (num2 << (num << 28))) & 0x9A9F5DA1u) < -909 * (int)(3745186066u / (uint)(~((int)(num2 >> num) / -1398212987))))
						{
							num = (int)(227929528 + (0xA400 & num2));
							continue;
						}
						goto IL_06d8;
					}
					goto IL_0a5c;
				case 8u:
					num = -2070166646 ^ (int)(0x6388 & num2);
					if (value != (_79AC42A1)(((522976274 < 1468913 * ((uint)num / 14u)) ? 1 : 0) - -512))
					{
						if ((((int)num2 + 320614058 / (-902087801 | num)) | (-452353493 >> num)) >= (num - num) % (num + num - -1587775118) >>> -249622094 / (-2026093040 & (-810299604 >> (int)num2)))
						{
							return;
						}
						num = (((int)(short)((sbyte)num2 >>> num) < (int)num2) ? 1 : 0) + 1612398860;
					}
					else
					{
						num4 = (short)((uint)(-449237958 / (num >>> (int)((uint)num | num2))) / 312136890u);
						num = ((1074491054 << num) & (50 >> -num4)) - 720106714;
					}
					continue;
				case 9u:
					num = ((-num ^ 0) << 1 + num) + 1423901322;
					return;
				case 10u:
					num = (sbyte)(-1875684214 - num << num) + -2070166902;
					if ((int)value <= num + 2070167668)
					{
						num2 = (uint)(num % 1 % 244301574) & ((uint)num % (uint)(-155378688 * num));
						if ((0xC0EA383Du & num2) != (uint)(-737352401 & (((num == 0) ? 1 : 0) - (num + (short)num2))))
						{
							num = -788172560 + (num + 2106022197);
							continue;
						}
						goto IL_0135;
					}
					if ((int)value <= 1215 + (-301704016 + num >> (num / (num << num) >>> 2)))
					{
						num3 = (short)(((num == num) ? 1 : 0) >> (int)((uint)(-1480884183 + num) / uint.MaxValue) << num + num);
						num = (int)((uint)(num3 * (num3 - num - (num >> num))) % (((uint)(num >> 21) % (uint)num) ^ (uint)(num3 | (num >>> (int)num3)))) ^ -435152905;
					}
					else if (value != (_79AC42A1)(2147255356 + ((75675936 / (414955413 << num) * (-1540221306 | (num >> num))) | (num & -1475893060))))
					{
						if ((int)((uint)num / (uint)num) < ((~num + -2010416206 > (588152249 % num >> 18) / num) ? 1 : 0))
						{
							return;
						}
						num = 1745787042 + ~(num >>> 11 >> 28 << -num * num);
					}
					else
					{
						num2 = ((1871026963 == (num ^ num)) ? 1u : 0u);
						num = -1534284846 ^ (1638068876 - num);
					}
					continue;
				case 11u:
					num = (int)((uint)(short)num2 % (uint)(~((int)(sbyte)num2 % (int)(~num2)))) + -2070166902;
					goto IL_0622;
				case 12u:
					num = (int)((uint)(582284836 % (num >> 11) << 739129631 * (((int)num2 < 992182410) ? 1 : 0)) % (uint)(num | num) - 2070166902);
					goto IL_0691;
				case 13u:
					num = ((int)(0xEE2DD988u & num2) - -num >> 28) ^ 0x322E6403;
					if (value != (_79AC42A1)(((num == (byte)num) ? 1 : 0) - -764))
					{
						if (value != (_79AC42A1)(-18958 + num % ((ushort)num >>> 0)))
						{
							if ((uint)(352533393 % (-383257284 | num)) % (uint)(~(num % num * (int)num2)) / 3978141486u >= 1200903558u / (uint)(num & 0x33D572D & -341825509))
							{
								return;
							}
							num = (int)((uint)((-745145325 / num) & -264743283) % (((1470810808u > (uint)(ushort)num2) ? 1u : 0u) % 2106548493u) - 510902126);
							continue;
						}
						num4 = (short)((short)num ^ num);
						if ((uint)(1842341332 >>> (int)num2) - (~((uint)num4 % 1723361981u) >> (~num >>> 19)) != 0)
						{
							num = -363625638 + -(num4 ^ 0xE3A1085);
							continue;
						}
						goto IL_0a5c;
					}
					num2 = (uint)(num | -275859676);
					if (((((uint)num | num2) << (int)num2) | ((num2 & 0x8F8E1F10u) - 578903049)) >> (int)(0x6C29F79Bu & ((-2020162758 * num < num) ? 1u : 0u)) != 0)
					{
						num = (int)((uint)(1610612736 - num) / 1967672275u + 286114132);
						continue;
					}
					goto IL_0135;
				case 14u:
					num = 0x322E6407 ^ ((int)(321338008 / ~num2) / num >> (int)((uint)(-1304959347 >>> (int)num2) & num2) << 8);
					return;
				case 15u:
					num = -2070166902 + (int)((uint)((num | (num3 * -2078931043)) % num) / (uint)(~((num3 << (int)num3) * (int)(((num < -821193184) ? 1u : 0u) ^ 0x2Bu))));
					goto IL_0873;
				case 16u:
					num = (sbyte)(268668844 % ((num3 > -1777764477) ? 1 : 0)) ^ -2070166902;
					return;
				case 17u:
					num = -317374630 + num * -1953053392;
					goto IL_0995;
				case 18u:
					num = (sbyte)num - 2070166804;
					HandleFightPlacementPossiblePositionsEvent(_671BC22C.BF9F3D1F.FE8E0C9E[0x53E ^ (((num2 % 1762381967) ^ 0xCA108188u) % num2 >> (int)num2)](_671BC22C.BF9F3D1F.FE8E0C9E[((511734719 > num) ? 1 : 0) - -1382](gameEvent)));
					return;
				case 19u:
					num = ((1271354967 > (uint)num / num2) ? 1 : 0) ^ 1;
					HandleFightTurnEvent(_671BC22C.BF9F3D1F.FE8E0C9E[-((int)(~num2) % (0x5D82A49D | ((int)num2 / -1498873411))) ^ 0x1CD7185D](_671BC22C.BF9F3D1F.FE8E0C9E[((uint)((int)num2 % ~(num >> (int)(722285628 + num2))) | ((num2 & 0x28B19030) + 1141314078)) - 1285037271](gameEvent)));
					return;
				case 20u:
					num = (int)((uint)(num | (num / (int)(~((uint)num % (uint)num)))) % 757502518u) - -1717345696;
					Reset();
					if ((uint)((sbyte)num * -1935719898) <= (uint)(-(-1256870392 >>> (int)(((num2 < (uint)num) ? 1u : 0u) | (uint)(715491121 << (int)num2)))))
					{
						num = (int)(num2 ^ 0x360F27A5);
						continue;
					}
					goto IL_0995;
				case 21u:
					num = (int)(((uint)((int)num2 * (num - num)) | num2) - 2070166902);
					_instanceData.CharacterData.IsFighting = (byte)((ushort)((32137 + num) % (314176673 / (int)(~num2))) - 64472) != 0;
					return;
				case 22u:
					num = -1606588626 - num4 - -1808451932;
					if (!_instanceData.CharacterParameters.NoAnimMode)
					{
						return;
					}
					goto IL_0c36;
				case 23u:
					num = -2070176401 + (ushort)num;
					HandleEntitiesDispositionEvent(_671BC22C.BF9F3D1F.FE8E0C9E[(~num + (-432864599 >> num)) ^ 0x7B5DCF0A](_671BC22C.BF9F3D1F.FE8E0C9E[(864950918 << (int)(0 - num2) - -1928340306 * ((int)num2 * num)) - -735053159](gameEvent)));
					return;
				case 24u:
					num = 1635390003 + num * (int)(1547693067 % ~num2);
					HandleMapMovementEvent(_671BC22C.BF9F3D1F.FE8E0C9E[(int)(0 ^ num2) - -1343](_671BC22C.BF9F3D1F.FE8E0C9E[0x567 ^ (num2 & 0xDCA93798u)](gameEvent)));
					if (((uint)(num + -771626193) | ((uint)(short)(-659868381 | (int)num2) % 2736452872u)) >= (uint)num)
					{
						num = 0x3516D236 ^ b;
						continue;
					}
					goto IL_0c36;
				case 25u:
					num = (-(1983138356 + (((int)num2 < (int)b) ? 1 : 0)) >>> (-b >> (int)(byte)num)) - -201885834;
					return;
				case 26u:
					num = -222678136 ^ ((num / (int)(~num2) - 1044013187) % 278335904);
					HandleMapCurrentEvent(_671BC22C.BF9F3D1F.FE8E0C9E[583770373 + (-784536399 ^ num)](_671BC22C.BF9F3D1F.FE8E0C9E[1383 + num % num](gameEvent)));
					return;
				case 27u:
					num = ((-282150 >> (num << 9)) ^ ((int)num2 % -1189663188)) + (int)(1913696649 + (num2 >> 11) / ~num2) - -311385895;
					HandleChallengeListEvent(_671BC22C.BF9F3D1F.FE8E0C9E[((num < (sbyte)(8329 / (int)(4087714445u % ~num2))) ? 1 : 0) - -1342](_671BC22C.BF9F3D1F.FE8E0C9E[(short)(~num2) ^ -1384](gameEvent)));
					return;
				case 28u:
					num = -841901137 ^ (-(num ^ ((int)num2 * -1716057821)) % ((int)num2 + num >>> (num | -164092159)));
					HandleChallengeProposalEvent(_671BC22C.BF9F3D1F.FE8E0C9E[(num2 * 741654441) ^ 0x3CD34480](_671BC22C.BF9F3D1F.FE8E0C9E[num / (int)(num2 ^ 0xE526DA9Du) - -1382](gameEvent)));
					return;
				case 29u:
					num = (int)((num2 * 1843221202) ^ 0x322E6407);
					HandleChallengeAddEvent();
					return;
				case 30u:
					num = -2070166902 ^ ((((int)((uint)((int)num2 * (int)num4) / (uint)num4) < ~num4) ? 1 : 0) / num);
					HandleFightRefreshCharacterStatsEvent(_671BC22C.BF9F3D1F.FE8E0C9E[((-1302541472 | (-1816263664 << (int)num2)) >> (int)num4 - (int)num2) ^ -298253](_671BC22C.BF9F3D1F.FE8E0C9E[0x542 ^ ((sbyte)(num2 / 3481579663u + 134977199) - (sbyte)num)](gameEvent)));
					if (~num != 0)
					{
						num = ((int)num2 >> 13) - 1165019757;
						continue;
					}
					goto IL_0873;
				case 31u:
					num = (1 >>> (num << (num | num4))) - 2070166903;
					return;
				case 32u:
					num = (int)(~(((uint)(sbyte)num2 | num2) & num2)) + -2070166901;
					HandleFightSynchronizeEvent(_671BC22C.BF9F3D1F.FE8E0C9E[-(1495830200 - (num + 331039)) - 729299890](_671BC22C.BF9F3D1F.FE8E0C9E[(0 >>> (1488326147 << (int)num2 >> 30) % (int)(~((uint)(-40208490 / num) / (uint)num))) - -1383](gameEvent)));
					return;
				case 33u:
					num = (0x4CB9ADA0 & (num >>> (num3 + -156491630) * -256386760)) - 2070199670;
					HandleGameActionFightEvent(_671BC22C.BF9F3D1F.FE8E0C9E[0x910735 ^ (num & 0x6BF50E4A)](_671BC22C.BF9F3D1F.FE8E0C9E[-102482 ^ ~(num3 - -106224091 >>> 10)](gameEvent)));
					if (((((num3 >> 17 >> (int)num3) | (-1079919059 - num3)) > (num ^ num)) ? 1u : 0u) != (uint)num4)
					{
						return;
					}
					continue;
				case 34u:
					return;
				case 35u:
					{
						num = (int)(28738462 + (num2 ^ 0x8AE53EECu));
						HandleGameActionUpdateEffectTriggerCountEvent(_671BC22C.BF9F3D1F.FE8E0C9E[((~num3 << 10) & -1522985834) - -1522987327](_671BC22C.BF9F3D1F.FE8E0C9E[((0 - num2 == (num2 | 0xE99BA685u)) ? ((sbyte)1) : ((sbyte)0)) + 1383](gameEvent)));
						return;
					}
					IL_0c36:
					_671BC22C.BF9F3D1F.FE8E0C9E[num4 - -24095](FB299931.B8047F90(_instanceData.ProcessId, (byte)(1 + -1715919210 % (int)(~((uint)(b - 1980313270) / ~(((int)num2 < (int)num2) ? 1u : 0u)))) != 0));
					return;
					IL_12a3:
					num2 = (uint)(-1299826942 << (int)(1177510298u % (uint)num) % 536887824);
					num = ~(num3 >> num) ^ 0x5354A958;
					continue;
					IL_0995:
					if (value != (_79AC42A1)(-2070165765 + (num ^ -186)))
					{
						return;
					}
					goto IL_125a;
					IL_0873:
					if (value != (_79AC42A1)(-((0x49BF789 ^ num3) + -1798788302) + -1721456775))
					{
						if (value != (_79AC42A1)(num + (int)(~((uint)num3 / (uint)(~num3))) - -2070168118))
						{
							if ((uint)(((num3 < 2075744302) ? 1 : 0) / (num >>> 18 >>> num) >>> num3 - (1008045239 >> num)) >= (uint)num3)
							{
								num = (((uint)(140 + (-1414835420 >> (-1844551265 ^ num3))) > (uint)num3) ? 1 : 0) ^ -644016767;
								continue;
							}
							goto IL_0d0b;
						}
						num4 = (((0x3D31BC83 ^ (932060 + num)) == (int)num3 + (int)((uint)(0x3407981A ^ num) % ~((num3 < num3) ? 1u : 0u))) ? ((short)1) : ((short)0));
						num = -1250599557 + 594525118 / (int)(~((((uint)(num << 4) > 3877298617u) ? 1u : 0u) & (uint)(num / ~(num4 >> 26))));
						continue;
					}
					goto IL_12a3;
					IL_029c:
					if (value != (_79AC42A1)(0 - 1829056443u % (uint)((-1355295723 >> (int)num2) - num) + 1829056923))
					{
						num4 = (short)(~((num << num) - (int)num2 * -11));
						num = (int)(num2 - num2) * num + 654469168;
						continue;
					}
					if (2245154143u / (uint)(num + ((num | num) ^ 0x4DA3633A)) < (uint)(num >>> num))
					{
						num = (int)(0x16063C99 ^ (3u | (((int)num2 > (int)(0x3C377405 ^ num2) / 41976320) ? 1u : 0u)));
						continue;
					}
					goto IL_0622;
					IL_125a:
					HandleFightFighterShowEvent(_671BC22C.BF9F3D1F.FE8E0C9E[-(num - -930153550 % num) + (num & -2136140886) + 998492557](_671BC22C.BF9F3D1F.FE8E0C9E[(-415827029 & num) ^ -2079143955](gameEvent)));
					return;
					IL_0d0b:
					b = (byte)((num >>> (int)num2) | ((int)(num2 | num2) + num >> (num & -1006213738)));
					num = -1434780169 ^ (int)(~((num2 & 0x41EBD37) ^ (num2 & ((2141826853 > b) ? 1u : 0u))));
					continue;
					IL_0622:
					if ((int)value > ((1351649807 - num) ^ -873151107))
					{
						goto IL_06d8;
					}
					num2 = 920495828 / ~num2;
					if ((uint)(num ^ ((int)num2 % (int)(~(num2 * num2)))) >= (uint)num)
					{
						num = -1538325076 ^ (int)((num2 ^ 0x49AE4034) << 5);
						continue;
					}
					goto IL_12a3;
					IL_0135:
					if ((int)value <= (-1383021774 & -num) - 687865633)
					{
						num2 = (uint)((-1824702846 - (int)((uint)(num >>> 13) % (uint)(num & 0x191D5401))) & (num >>> 22));
						if (((98738254u / (uint)num == (uint)((int)num2 >> ((327457830 > num * (int)num2) ? 1 : 0))) ? 1 : 0) >= -63410635 + (int)num2)
						{
							num = ((~((481563444 < num2) ? 1u : 0u) > (uint)(num * (int)(num2 & (uint)num))) ? 1 : 0) - 214975444;
							continue;
						}
						goto IL_0691;
					}
					if ((byte)(~num) != -(154431376 % num % 38457) % num)
					{
						num = (int)(((-718035582 / num < -1734627905 * (int)((uint)num / 581486731u)) ? 1u : 0u) % (uint)((0x13B2410E | num) << (int)(byte)num)) - -925270282;
						continue;
					}
					return;
					IL_0a5c:
					num = (sbyte)(num2 << ~num);
					if (((short)(0 - num2) & ((-1507715792 / (int)(num2 | 0x2235BD3A)) & 0x4B245A0D)) != 0)
					{
						num = (int)((num2 | 0x871AD295u) ^ 0x3F43C0FE);
						continue;
					}
					return;
					IL_0691:
					if (value != (_79AC42A1)((-1691625965 | num) - -1615080435))
					{
						if (value != (_79AC42A1)(((int)(~(num2 >> (int)num2)) >> (int)(3666916742u / (uint)(~(num * (int)num2))) >>> (int)(488794283 + (num2 >> 25))) - 2096391) || num2 * 0 != 0)
						{
							return;
						}
						num = (((358299503 >> (int)num2 % 1796936110) * num < ~(num + 188631712)) ? 1 : 0) + -888959858;
					}
					else
					{
						num2 = (uint)(-1884038133 | (sbyte)(num2 / 800117907)) % ((uint)(num >>> (int)num2) | num2);
						num = -69844449 ^ ((1689858946 << (int)(short)num2) - (num << (int)((uint)num % num2)));
					}
					continue;
					IL_06d8:
					num = (int)(num2 ^ 0x322E6407);
					if ((int)(num2 << 6) * -509543398 / num / 791924523 == 0)
					{
						num = -1087386950 + -1 * (int)(1971953305 - num2);
						continue;
					}
					goto IL_125a;
				}
				break;
				IL_0262:
				if ((int)(num2 - (num2 << -1699352947 / (int)(~num2))) > num / num)
				{
					goto IL_0000;
				}
				num = -552702216 + (num | 0xAA12E8F);
			}
			if (value != (_79AC42A1)((byte)(num << (int)num2) ^ 0x29B))
			{
				break;
			}
			goto IL_09ab;
			IL_0000:;
		}
	}

	internal void HandleGameActionUpdateEffectTriggerCountEvent(ByteString value)
	{
	}

	internal void HandleFightSynchronizeEvent(ByteString value)
	{
		IEnumerator<JitsuriActorPositionInformation> enumerator = _671BC22C.BF9F3D1F.FE8E0C9E[1047](_018E928B._29B6D927<FightSynchronizeEvent, JitsuriFightSynchronizeEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[1973]()).map).GetEnumerator();
		try
		{
			JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriFightFighterInformation fighter = default(JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriFightFighterInformation);
			int num2 = default(int);
			Attacker attacker = default(Attacker);
			int value4 = default(int);
			JitsuriCharacterCharacteristic current = default(JitsuriCharacterCharacteristic);
			FightCharacteristic value2 = default(FightCharacteristic);
			CharacteristicKeyword value3 = default(CharacteristicKeyword);
			int num3 = default(int);
			_2B16532D _2B16532D2 = default(_2B16532D);
			_2B16532D _2B16532D3 = default(_2B16532D);
			CharacteristicKeyword value6 = default(CharacteristicKeyword);
			int value5 = default(int);
			JitsuriCharacterCharacteristic current2 = default(JitsuriCharacterCharacteristic);
			int num8 = default(int);
			while (true)
			{
				sbyte b = 1;
				b = (sbyte)(1261783736u / (uint)(~(b * b - b)) >> (int)b);
				while (!_671BC22C.BF9F3D1F.FE8E0C9E[((uint)b | (471183799u / (uint)(~(b << 6)) % 1342306102)) + 1381](enumerator))
				{
					if ((b * b + b) * b * -1013050980 == 0)
					{
						return;
					}
				}
				while (true)
				{
					_003C_003Ec__DisplayClass30_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass30_0();
					short num = 1844;
					if (!(-1077695236 > (num & 0x43AFF290)))
					{
						goto IL_0059;
					}
					goto IL_007f;
					IL_007f:
					fighter = _671BC22C.BF9F3D1F.FE8E0C9E[((num >>> 22) ^ (num >>> 0 >> (int)num)) - -147](CS_0024_003C_003E8__locals4.actor).Fighter;
					num2 = num >> num / ~(num & 3);
					if ((num2 | -1034974095) == 0)
					{
						continue;
					}
					num = (short)((((uint)(num >> 16) < (uint)num) ? 1 : 0) + 7476);
					goto IL_0059;
					IL_0059:
					while (true)
					{
						switch ((uint)num % 4u)
						{
						case 1u:
							goto IL_00e3;
						case 2u:
							goto IL_015c;
						case 3u:
							goto IL_0198;
						}
						break;
						IL_0198:
						num = (short)(-26732 ^ (0 ^ b));
						goto IL_01a4;
						IL_015c:
						num = (short)(~(-num2) - -1845);
						if (attacker != null)
						{
							num = (short)(num + -614363040);
							num = (short)(15007 + ((-b == num2) ? 1 : 0) / (-1596338992 | num2));
							continue;
						}
						num2 = ((num2 > b) ? 1 : 0) | (76278667 % num);
						Defender defender = Defenders.Find((Defender defender2) => _671BC22C.BF9F3D1F.FE8E0C9E[444](CS_0024_003C_003E8__locals4.actor) == defender2.ActorId);
						if (defender == null)
						{
							goto end_IL_0036;
						}
						b = (sbyte)(num2 | 0x27736205);
						IEnumerator<JitsuriCharacterCharacteristic> enumerator2 = _671BC22C.BF9F3D1F.FE8E0C9E[((uint)(1764875547 - num2 >> (b ^ num)) | ((uint)b | ((uint)b / 1169022486u))) * 1908384556 - 2137250898](fighter.Stats).GetEnumerator();
						try
						{
							if (-846745077 << (int)(0 - 907302153 / (2284941838u % (uint)b)) == 0)
							{
								num2 = num2;
								num = (short)((int)((1245365u / (uint)b) ^ (uint)(((num & 0x7AABA189) ^ -1179712739) / ((num / num2) ^ -753578090))) + -5607);
								goto IL_0b35;
							}
							goto IL_127e;
							IL_12c5:
							while (!_671BC22C.BF9F3D1F.FE8E0C9E[1380 + ((-1131123764 * (-2145682912 & b) > -510160723) ? 1 : 0)](enumerator2))
							{
								if (-1708758348 + b == 1618691153 - b)
								{
									continue;
								}
								goto IL_1304;
							}
							goto IL_0b8e;
							IL_0b35:
							while (true)
							{
								switch ((uint)num % 13u)
								{
								default:
									num = (short)(~(((num | 0x71BD0893) == num2) ? 1u : 0u) + 1845);
									goto IL_0ed9;
								case 1u:
									break;
								case 2u:
									goto IL_0c6c;
								case 3u:
									num = (short)(-66 ^ (b | num));
									value4 = _671BC22C.BF9F3D1F.FE8E0C9E[(num >>> (int)b) - ~num2 - 1287](_671BC22C.BF9F3D1F.FE8E0C9E[0xD583 ^ (ushort)(-804400849 ^ (num << num2))](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[((int)((uint)((b >>> 23) ^ 0x410D979B) / (uint)(~(num2 / -2137656045))) >> 1) - -450](_671BC22C.BF9F3D1F.FE8E0C9E[1186 + b](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[((uint)b % 3316025138u) ^ 0x3A597F22](_671BC22C.BF9F3D1F.FE8E0C9E[(-1507509993 << (int)num) + ((((922767491 == b) ? 1u : 0u) > 3126992212u) ? 1 : 0) + -1279946242](current));
									goto IL_1188;
								case 4u:
									goto IL_0e36;
								case 5u:
									num = (short)(2015477263 - b + -2015477381);
									goto IL_0ed9;
								case 6u:
									num = (short)((num2 & num2) + -1606);
									goto IL_1188;
								case 7u:
								{
									num = (short)((uint)num2 % (uint)(~((num >>> 30) % -1)));
									JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase num4 = _671BC22C.BF9F3D1F.FE8E0C9E[b + -633448294 - -633449996](current);
									int num5 = num2 / ~(short)(num2 >> 15) - -1610;
									num -= 1606;
									if (num4 == (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)num5)
									{
										num = (short)(-(-130367192 * -num));
										num = (short)(23058 + (0 - ((b * 1250602802 < num) ? 1 : 0) + ((num2 << 13 == 833279765) ? 1 : 0)));
										continue;
									}
									goto IL_1188;
								}
								case 8u:
									num = (short)(-927221205 % (int)(~((uint)((num2 ^ num) / -560340318) % (uint)num)) - 16088);
									value4 = _671BC22C.BF9F3D1F.FE8E0C9E[0x192C05EA ^ ((uint)(((b < -1573286864) ? 1 : 0) >> (0x4B9A3024 & b)) | ((uint)b % 238041130u << 17))](_671BC22C.BF9F3D1F.FE8E0C9E[((-num * 1496469536 - ((b == -265133284) ? 1 : 0) < b) ? 1 : 0) + 457](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[(b + num2 - 513598129) / -1357225316 / num - -1185](_671BC22C.BF9F3D1F.FE8E0C9E[3042 + (short)(ushort)(230589321 + num + -1994936010)](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[(0 >> 1159856284 / b) ^ 0x646](_671BC22C.BF9F3D1F.FE8E0C9E[(~(((1470840222 > num2) ? 1 : 0) * -1424526941) - (sbyte)(~(b / 723297817))) ^ 0x54E88F94](current));
									num += 16089;
									goto IL_1188;
								case 9u:
									num = (short)(-2016566707 ^ num ^ -2016560064);
									value2 = new FightCharacteristic(value3, num3, _2B16532D2._9B3B2A01, value4);
									num = (short)(1346198311 + -953315573 % b);
									num = (short)(17678 + (b >> (int)(~((uint)num / 2433395714u) - (ushort)(1319479699 % num2))));
									continue;
								case 10u:
									goto end_IL_0b35;
								case 11u:
									goto IL_12c5;
								case 12u:
									{
										num = (short)((b >> 27) - -1844);
										goto end_IL_0aea;
									}
									IL_1188:
									num2 = ((((num == -1524103014) ? 1 : 0) + -280676653) | (num2 >> 8)) % -868721389;
									if ((uint)((64814 >> ((3876325532u < (uint)b) ? 1 : 0)) | 0x6D4754CE) > (uint)((num >> -(-num)) | num))
									{
										num = (short)(1596971138 + 1906231041 * b);
										continue;
									}
									goto IL_0ed9;
									IL_0ed9:
									value4 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(-1801193594 * num2) ^ ((ushort)(1429162529 >>> (int)num) ^ num2) ^ 0x114686D2](_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)(num2 - num2) / (uint)(~(1514127275 % ~(num2 % num2))) - 526511322) - -526512205](current));
									num -= -1;
									num = (short)(17594 + ((2878096796u / (uint)(221976453 - b) % ~((uint)num2 / (uint)b) < (uint)(num % num2 * -1603170462)) ? 1 : 0));
									continue;
								}
								goto IL_0bf4;
								IL_0e36:
								num = (short)(0xECF ^ ((uint)(num >>> 1) % (uint)((-1568130769 & num) | num)));
								if (_671BC22C.BF9F3D1F.FE8E0C9E[((b & 0x38F1133) * (int)((uint)num2 % (uint)(((b > -276142304) ? 1 : 0) - 833668353))) ^ 0x5713C5BE](current) == (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)((((int)((uint)(num & 0x280AE402) / 3451368994u) - (-1389171172 % b >> 19)) & -2) + 4))
								{
									if (b != 0)
									{
										num = (short)((int)((uint)((b << 25) + b) % (uint)(1694637339 / ~num >>> 2) - 63) + -335541937);
										continue;
									}
									goto IL_0b8e;
								}
								if (((num << (num2 >>> (int)b)) ^ -1811253452) >> 26 <= num >> ~(num ^ 0x60F1A32) % ((num - -1163360862 > -1235251503) ? 1 : 0))
								{
									num = (short)(num2 + b * ((int)((uint)b % (uint)b) >> 7));
									num = (short)(0x13AFC6 ^ (1301467 >> (int)(3372884633u / ((uint)(-876711392 + num2) % 4169426327u))));
									continue;
								}
								goto IL_0bf4;
								IL_0bf4:
								_2B16532D2 = A996FE3D._34B4919D[value3];
								goto IL_0c02;
								IL_0c6c:
								num = (short)((((0x514D364C | num2) & -1667737665) ^ ((int)((uint)num / (uint)num2) % ((0x773C9397 & b) >> num2))) + -268965327);
								if (_671BC22C.BF9F3D1F.FE8E0C9E[1583 + ((((num2 > -81363189) ? 1u : 0u) < (uint)num % 4119302055u) ? 1 : 0)](current) == (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)(((num ^ (num2 + b)) | (b % num2 / 1673440529)) ^ 0x5E6))
								{
									num = (((-65485805 ^ (num2 | -1726220768)) > -1198330472) ? ((short)1) : ((short)0));
									if (num2 - (num2 + b) != 0)
									{
										num = (short)((int)((0 - (2 + (uint)num2 % (uint)b)) % (uint)(num | (num >> (int)b))) - -8245);
										continue;
									}
									goto IL_0c02;
								}
								num = (short)(56718961 % ~(num2 - num2 >>> (int)(sbyte)num2));
								num = (short)((-2025174 - num << (int)b / (int)(~((num < num2 % num2) ? 1u : 0u))) - 1249895009);
								continue;
								end_IL_0b35:
								break;
							}
							num = (short)((num2 & (num2 & -1072624497)) - -1073698401);
							defender.Characteristics[value3] = value2;
							num ^= 0x5DDC;
							num2 -= -280678256;
							b = (sbyte)(b - -189);
							goto IL_127e;
							IL_0b8e:
							current = enumerator2.Current;
							num3 = _671BC22C.BF9F3D1F.FE8E0C9E[2025](current);
							num2 = 0;
							goto IL_0bad;
							IL_0c02:
							value4 = ~(num2 ^ b) - -1536;
							b = (sbyte)((int)((uint)(-1943047126 + -num2) / 781973164u) ^ (-1524493943 >> num * (b << (int)b)));
							if ((num2 - num) * num != 0)
							{
								num = (short)(((0u < (uint)(num * -1447068504 / (-611903977 - num2 - (int)(1753614508u % (uint)num2)))) ? 1 : 0) - -19228);
								goto IL_0b35;
							}
							goto IL_12c5;
							IL_1304:
							num = (short)((int)((uint)b % 3994842406u) - -24230);
							goto IL_0b35;
							IL_0bad:
							bool num6 = A996FE3D.CABFD5B4.TryGetValue(num3, out value3);
							num2 ^= 0x647;
							b = 71;
							num = 1844;
							if (num6)
							{
								num = (short)(2491114360u % (uint)b);
								if (num2 + (int)((uint)b % (uint)b % (uint)num) + num2 != 0)
								{
									goto IL_0b35;
								}
								goto IL_0c02;
							}
							goto IL_127e;
							IL_127e:
							b = (sbyte)(((581618291u > (uint)(num2 / b)) ? 1 : 0) % -526984495);
							if (b % ((num - 1940280638) / (num | -1241418576)) != 1444436528 % ((b - num2) % -1933721198 >>> (int)num))
							{
								goto IL_0b35;
							}
							goto IL_0bad;
							end_IL_0aea:;
						}
						finally
						{
							if (enumerator2 != null)
							{
								num = 0;
								enumerator2.Dispose();
							}
						}
						goto end_IL_0036;
						IL_01a4:
						enumerator2 = _671BC22C.BF9F3D1F.FE8E0C9E[1634 + (uint)num2 % (uint)(~((b & 0x6018CB83) >> 16))](fighter.Stats).GetEnumerator();
						try
						{
							if (-382381648 - (int)((uint)(554086335 << num2) % 3996899760u * 696633649) == 0)
							{
								num2 = (int)((uint)(num * -63465558) / 8u);
								goto IL_01ff;
							}
							goto IL_0925;
							IL_070c:
							b = (sbyte)(((b + (byte)num) / -9) | 0x6D04B7B8);
							if ((((2183141642u < (uint)(19476270 / (b >>> 21))) ? 1u : 0u) ^ (uint)(-1044682178 >>> num2 - 1241314382)) == 0)
							{
								goto IL_08ab;
							}
							b = (sbyte)((int)(0x3F105023u & ((num < (byte)((uint)num2 / (uint)(~num))) ? 1u : 0u)) - -35);
							goto IL_01ff;
							IL_01ff:
							while (true)
							{
								switch ((uint)b % 13u)
								{
								case 1u:
									break;
								case 2u:
									goto IL_02ff;
								case 3u:
									goto IL_035c;
								case 4u:
									b = (sbyte)(0x28 ^ (0xF8FBE3E & b));
									_2B16532D3 = A996FE3D._34B4919D[value6];
									value5 = -1 + (int)((uint)(-1232946287 + (int)(3408718981u % (uint)(19180467 << num2))) / 3979607612u);
									num2 = b & -485576142;
									b = (sbyte)((1133331477 % b << num - num) ^ 0x2C);
									continue;
								case 5u:
									goto IL_045f;
								case 6u:
									goto IL_05c9;
								case 7u:
									b = (sbyte)(-16 + ((num2 >> 3) & b));
									value5 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)num2 % (uint)(~b)) - -943902997](_671BC22C.BF9F3D1F.FE8E0C9E[(num2 << (num2 & (num2 >>> (int)b))) - 2053049997](current2));
									num2 -= -943902889;
									num -= 26732;
									b -= -54;
									b = (sbyte)((int)((uint)(-1280879476 | (num & num)) % (uint)(~(short)(num / 179429273))) - -10400);
									continue;
								case 8u:
									goto IL_06eb;
								case 9u:
									goto IL_0760;
								case 10u:
									goto IL_08eb;
								case 11u:
									b = (sbyte)((0x59384439 & (b << 7)) + -1024);
									goto IL_095f;
								default:
									goto IL_095f;
								case 12u:
									b = (sbyte)(-808250482 + (((-1592679036 >> num2) | (b / 777214628)) - -(967385997 * b + num2)));
									goto end_IL_01ff;
								}
								b = (sbyte)(((b - (b << (int)b)) * b << 3) + 1073736027);
								current2 = enumerator2.Current;
								if ((uint)(b * 885031328) > (uint)(sbyte)(0x6D272B31 ^ ((b - -1834850127) | b)))
								{
									b = (sbyte)((((((1429684363u < (uint)b) ? 1 : 0) < (int)b) ? 1u : 0u) << -2104844925 - 834278335 % (b - 1127640972)) ^ 0x26);
									continue;
								}
								goto IL_04c9;
								IL_095f:
								if (_671BC22C.BF9F3D1F.FE8E0C9E[1381 + (uint)num / (uint)((num2 ^ (num / 1452965004)) | (0x6BC94AD | num))](enumerator2))
								{
									b = 35;
									if (856799257 << (b << b + 1286056763) > (int)((uint)(0x8B561A7 & b) % (uint)((b & -2035447515) % 1847757571 << (int)b)))
									{
										b = (sbyte)(((1227713805u % (uint)b) | (uint)(~(~b) >> (int)((uint)b % 1897376318u) * (int)b)) ^ 0x11);
										continue;
									}
									goto IL_0925;
								}
								if ((int)(566108677u % (uint)(b | 0x7F39941C)) / ((-273251054 / ~b) | 0x43BA3427) - (int)((((uint)(num2 & num2) > 40u) ? 1u : 0u) ^ 0x1389970Au) != (num2 >> (int)num) + -2085186166)
								{
									b = (sbyte)((((b / ~num2) & (-266274918 << (int)num)) / ~(num2 >>> 28) % ~(-(-num2))) ^ 0x26);
									continue;
								}
								goto IL_070c;
								IL_045f:
								b = (sbyte)(b + 1718108484 - 1718108461);
								if (_671BC22C.BF9F3D1F.FE8E0C9E[(num | (byte)(((3826362115u < (uint)b) ? 1 : 0) + num2)) - -28282](current2) != (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)((int)(((uint)(ushort)num | ((b >> num2 < 520970243) ? 1u : 0u)) << -1761340749 / num / (int)(1413210545u % (uint)(b + -1816949843))) + -38803))
								{
									num = (short)((uint)(-835437516 * (num2 | num2)) / 1201147054u);
									b = (sbyte)(-(short)(~num) << num2);
									b = (sbyte)(-33 ^ (~num | 0x1BBD));
									continue;
								}
								goto IL_04c9;
								IL_035c:
								b = (sbyte)(~num2 ^ -840981893);
								bool num7 = A996FE3D.CABFD5B4.TryGetValue(num8, out value6);
								b -= 35;
								num2 ^= 0x32205DA7;
								num = -26732;
								if (num7)
								{
									b = (sbyte)(num2 + 833962550);
									if ((uint)(0x1910 ^ ((num | -786786899) << (int)(short)num2 << num + 1552297878)) >= (uint)((num2 >> (num << (int)num)) + b) / (uint)(b << num % num % -1987051516))
									{
										b = (sbyte)((b % (b << (num2 - num) * (0x1BA8061F | b))) ^ 0x28);
										continue;
									}
									goto IL_08ab;
								}
								goto IL_0925;
								IL_04c9:
								value5 = _671BC22C.BF9F3D1F.FE8E0C9E[(num2 << 1) - -221](_671BC22C.BF9F3D1F.FE8E0C9E[1122 + (int)b / (int)(~((uint)(sbyte)num2 / (uint)((-181185512 >>> num2) | 0x66048698)))](current2)) + _671BC22C.BF9F3D1F.FE8E0C9E[(((uint)b < (uint)(0x18A47001 ^ b)) ? 1 : 0) + 449](_671BC22C.BF9F3D1F.FE8E0C9E[num / 1866150800 + (((uint)(-79142707 / (b - 655377413)) < 781011675u) ? 1 : 0) - -1067](current2)) + _671BC22C.BF9F3D1F.FE8E0C9E[num2 * 901721567 - 2136403476](_671BC22C.BF9F3D1F.FE8E0C9E[(1 & num2) - -1068](current2));
								goto IL_08ab;
								IL_02ff:
								b = (sbyte)((0x406F | b) - 16476);
								num8 = _671BC22C.BF9F3D1F.FE8E0C9E[0x320 ^ (b * b)](current2);
								num2 = 0x32205D84 | (b % 999516693);
								if ((uint)(-num2 >>> (int)b) / ~((-1021830763 % b == b) ? 1u : 0u) >> 8 == 0)
								{
									b -= -20;
									continue;
								}
								goto IL_070c;
								IL_08eb:
								b = (sbyte)((0 * num) ^ 0x36);
								FightCharacteristic value7 = new FightCharacteristic(value6, num8, _2B16532D3._9B3B2A01, value5);
								attacker.Characteristics[value6] = value7;
								num2 += -50;
								b += -54;
								goto IL_0925;
								IL_0760:
								b += -107;
								JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase num9 = _671BC22C.BF9F3D1F.FE8E0C9E[135633920 + -b - 135632408](current2);
								int num10 = ((((int)((uint)(0x3F9930AE ^ num) % ((uint)num2 / (uint)num2)) < -891592995 / ~num) ? 1 : 0) / (int)b) ^ 3;
								b -= -126;
								num += -26732;
								if (num9 == (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)num10)
								{
									goto IL_07b6;
								}
								goto IL_08ab;
								IL_06eb:
								b = (sbyte)((int)(0xE10EF819u | ((uint)(0x16A15B00 ^ num) % 3755339586u)) - -376505051);
								goto IL_08ab;
								IL_05c9:
								b = (sbyte)(-1184 + b * (byte)(1486296113 >> num2));
								if (_671BC22C.BF9F3D1F.FE8E0C9E[(num2 | (1807549184 + 716589098 % ~(-838022771 & num))) - 1807547650](current2) != (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)(-454830574 + (-454830578 << (int)num) / ~(num / 13)))
								{
									goto IL_070c;
								}
								num2 = (num2 << (int)b) ^ -943902789;
								if ((num2 & 0x37220333) != 0)
								{
									b = (sbyte)(-(-47629003 / ~(-1249012322 % ~num)) - -47629023);
									continue;
								}
								goto IL_07b6;
								IL_07b6:
								value5 = _671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)(0x4FB8E506 | b) % ~((uint)((num2 + b) & -1657422051) / 2494746514u)) + -1337515852](_671BC22C.BF9F3D1F.FE8E0C9E[425 + (b & -347901728)](current2)) + _671BC22C.BF9F3D1F.FE8E0C9E[(uint)(-2104946400 >>> num2) / 1951533694u % 3543403318u + 1185](_671BC22C.BF9F3D1F.FE8E0C9E[(byte)(((1369745209 / num2) | 0x47AEE82C) >>> -560438135 / b) - -338](current2)) + _671BC22C.BF9F3D1F.FE8E0C9E[497157441 + ((19972 - (uint)num % 1898882383u) ^ (uint)(((-678490467 >>> num2) | 0x6C1D4C2E) % num2))](_671BC22C.BF9F3D1F.FE8E0C9E[457 + (uint)(-328634763 ^ num2) / (uint)num](current2));
								goto IL_08ab;
								continue;
								end_IL_01ff:
								break;
							}
							goto end_IL_01d1;
							IL_0925:
							if (num - 560990326 << 16 != 0)
							{
								b = (sbyte)(((0x710B823E | (b ^ (b >>> 19))) & 8) ^ 0x10);
								goto IL_01ff;
							}
							goto IL_070c;
							IL_08ab:
							while ((int)((uint)num ^ ((1016772119 == num) ? 1u : 0u)) + (num | b | (byte)b) >= 1 << (int)(1862340148 * (2981989809u / (uint)b)))
							{
							}
							b = (sbyte)((-4 & (ushort)num) + -38755);
							goto IL_01ff;
							end_IL_01d1:;
						}
						finally
						{
							num2 = -2063472717;
							if (enumerator2 != null)
							{
								num = (short)(-821015146 | num2);
								if ((uint)((1392619062 + ((num2 + -1390815428) & -1961283026)) % (~(num - num) ^ (num2 << (int)((uint)num / (uint)num)))) < (uint)num)
								{
									enumerator2.Dispose();
								}
							}
						}
						goto end_IL_0036;
						IL_00e3:
						num = (short)(-1297317860 + num - -1297312227);
						attacker = Attackers.Find((Attacker attacker2) => _671BC22C.BF9F3D1F.FE8E0C9E[444](CS_0024_003C_003E8__locals4.actor) == attacker2.ActorId);
						b = ((num2 == -200011095 || (uint)num < (uint)(num >>> 12)) ? ((sbyte)1) : ((sbyte)0));
						if ((num2 << 31) % ~(num2 % 1837451697 + (num >>> 12)) == 0)
						{
							num = (short)(((146113550 / ~((num >> 13) & b)) | num2) ^ -146117904);
							continue;
						}
						goto IL_01a4;
					}
					CS_0024_003C_003E8__locals4.actor = enumerator.Current;
					goto IL_007f;
					continue;
					end_IL_0036:
					break;
				}
			}
		}
		finally
		{
			while (true)
			{
				short num = 0;
				if (enumerator != null)
				{
					if ((((uint)(-536479862 - num) % (uint)(~num) >> num % ~num) & (uint)num) == 0)
					{
						enumerator.Dispose();
						break;
					}
					continue;
				}
				break;
			}
		}
	}

	internal void HandleFightRefreshCharacterStatsEvent(ByteString value)
	{
		_003C_003Ec__DisplayClass31_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass31_0();
		(FightRefreshCharacterStatsEvent, JitsuriFightRefreshCharacterStatsEvent) tuple = _018E928B._29B6D927<FightRefreshCharacterStatsEvent, JitsuriFightRefreshCharacterStatsEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[710]());
		uint num = 2920912273u;
		num = 3692694043u + ((uint)(0x1D055DB6 | ((byte)num >>> 15)) ^ ((0x6D9E993D ^ num) % ~((num < 4279242032u) ? 1u : 0u)));
		Attacker attacker = default(Attacker);
		_2B16532D _2B16532D2 = default(_2B16532D);
		CharacteristicKeyword value3 = default(CharacteristicKeyword);
		int value4 = default(int);
		JitsuriCharacterCharacteristic current = default(JitsuriCharacterCharacteristic);
		FightCharacteristic value2 = default(FightCharacteristic);
		int num3 = default(int);
		uint num2 = default(uint);
		uint num6 = default(uint);
		while (true)
		{
			switch (num % 3)
			{
			default:
				num = 0xAE199591u ^ ((0 - num == num) ? 1u : 0u);
				goto IL_0069;
			case 1u:
				if (attacker != null)
				{
					if (1018941731 % (int)(~num2) != 0)
					{
						num = (uint)(0x4D6DBF1F ^ (1038519 + ((int)(num * num2) >> (int)(num2 / 4294967230u))));
						break;
					}
					goto IL_0069;
				}
				num = (uint)((int)(num | 0x460F5F3A) % -685993209);
				Defenders.Find((Defender defender) => _671BC22C.BF9F3D1F.FE8E0C9E[46](CS_0024_003C_003E8__locals4.ev) == defender.ActorId);
				return;
			case 2u:
				{
					num = (uint)(-((int)(num + num2) / -1475099877) - 1374055024);
					IEnumerator<JitsuriCharacterCharacteristic> enumerator = _671BC22C.BF9F3D1F.FE8E0C9E[(~num2 + (0x8B3EF59Du & (num2 & num))) ^ num ^ 0x662](_671BC22C.BF9F3D1F.FE8E0C9E[(0x95A5EB1Cu ^ num2) + 1002210890](CS_0024_003C_003E8__locals4.ev)).GetEnumerator();
					try
					{
						if ((num - ((num2 & 0x9C0CD40Cu) << (int)num)) / 3348220810u != 631691432 / (1739309619 * num + num2 - num2))
						{
							num2 = num;
							num2 = 2188754824u + ~((uint)((int)num2 >> (int)num >>> (int)num) ^ num2);
							goto IL_0162;
						}
						goto IL_082d;
						IL_082d:
						num2 = (uint)((short)((uint)((int)num / (int)num2) / 897972379u) - -32488);
						num2 = num;
						num2 = (uint)((int)num * -24341076 - 0 / (int)(345923235 % num) + 462948314);
						goto IL_0162;
						IL_0162:
						while (true)
						{
							bool num7;
							switch (num2 % 12)
							{
							default:
								num2 = (uint)((356273096 % (int)num2 << 0) - -1017781927);
								goto IL_080e;
							case 1u:
								num2 = (uint)((((short)num2 << (int)(num2 << (int)num2)) * 791376935 >>> (int)(25u / (uint)((int)num2 % (int)num))) - -493516932);
								_2B16532D2 = A996FE3D._34B4919D[value3];
								num2 = (((uint)(1687031471 << (int)(num2 * 42)) > (uint)(-((int)num % (int)num * (int)num))) ? 1u : 0u);
								value4 = -39689 ^ (39688 * (short)num2);
								if ((uint)(-((int)num2 % (int)(~(num2 >> (int)num2)))) < (uint)((1721320981 >> (int)(num2 / (1782084099 - num2))) / (int)(~((num2 % ((num2 == num2) ? 1u : 0u)) ^ (uint)((int)num / -1684906843)))))
								{
									num2 = (uint)((int)num2 - -2053587662 - -1129283543);
									continue;
								}
								goto IL_01be;
							case 2u:
								num2 = (num2 | (0xFFFFFE1Eu ^ ((uint)((int)num2 >> 19) % 3290496157u))) - 4256679846u;
								if (_671BC22C.BF9F3D1F.FE8E0C9E[-1925968187 % (int)(~((0 - num2 % num2) % (uint)(~((int)num2 / 2058114317)))) + 1584](current) == (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)(num2 ^ 3))
								{
									goto IL_032e;
								}
								num = 1374046423 + num2;
								if ((0 ^ num) > ((num2 - num2) | num2))
								{
									num2 += 3165659163u;
									continue;
								}
								goto IL_080e;
							case 3u:
								num2 = 0xAE077DBAu ^ (num2 | (uint)((int)num % -1976419790));
								goto IL_074e;
							case 4u:
								num2 = 3165659112u + (53 - num2);
								goto IL_0472;
							case 5u:
								num2 = (uint)((-1254949460 ^ (607517569 >>> (int)num6)) * (int)num2 + 1531840980);
								value4 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[0x9E ^ -(-427707717 * (int)num6)](_671BC22C.BF9F3D1F.FE8E0C9E[(((1201880469 < num) ? 1 : 0) - -(-1985001457 % (int)(num + num6))) ^ -610954341](current));
								num -= 2883924936u;
								if ((int)((((num + num6) | num2) << 2) % (uint)(~((int)num6 / (int)(0 - num) >> (int)num6))) <= (-(1385301161 / (int)num) << ((1921162808 / (int)(~num6)) ^ -2035091529)) * 506263337)
								{
									num2 = (uint)(-937432298 ^ ((int)num6 - ((int)num + ((int)num >> 31)) << (int)(0xB52D839Cu | (num6 | 0xFD0AC414u))));
									continue;
								}
								goto IL_0472;
							case 6u:
								num2 = ((num2 > 800705684) ? 1u : 0u);
								goto IL_074e;
							case 7u:
							{
								num2 = (num2 - (num | 0x72197CBC) << 16) ^ 0x5CAB0000;
								JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase num4 = _671BC22C.BF9F3D1F.FE8E0C9E[917680285 / (int)((~num2 | (num & 0x92A699A)) ^ (num | 0x5C389D1D)) + 1584](current);
								int num5 = 3 + -1380981994 % (int)(~((uint)((int)num2 % 260744590) & num));
								num2++;
								num += 1411042360;
								if (num4 == (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)num5)
								{
									goto IL_0688;
								}
								goto IL_074e;
							}
							case 8u:
								num2 = (uint)(((-1399059176 + (int)(num2 / 2214849716u)) | (-2099249152 & (-812280659 % (int)(num + num2)))) ^ -1365504744);
								value2 = new FightCharacteristic(value3, num3, _2B16532D2._9B3B2A01, value4);
								num2 = num - ((num % num2) ^ num2);
								if ((uint)((int)num >> 12) * num != 0)
								{
									num2 = (uint)(0 - ((-1093951711 > (int)(((605723546 < num2) ? 1u : 0u) >> 4)) ? 1 : 0) - 1869115771);
									continue;
								}
								goto IL_032e;
							case 9u:
								num2 = (uint)(-1616343785 / (int)((num2 >> 31) % 1519064712) + (int)num2 + 1975581043);
								goto IL_080e;
							case 10u:
								num2 = (uint)(-990282904 + (int)num2 % -1191063523);
								goto IL_086c;
							case 11u:
								{
									num2 = (num | 0x66A84FBE) - 1084246574;
									return;
								}
								IL_0688:
								value4 = _671BC22C.BF9F3D1F.FE8E0C9E[(int)(0 - num2 << ((-1044616018 << (int)num) / (int)(num2 % 1435380620) >>> 7)) - -1515](_671BC22C.BF9F3D1F.FE8E0C9E[(int)((num & num) >> (int)(num ^ 0x906A4BC) * -479945654) - -1937562207 + -1937561916](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[~((789701542 % (num >> (int)num2)) | (uint)(sbyte)num) ^ 0xD0EE18E8u](_671BC22C.BF9F3D1F.FE8E0C9E[(num2 >> 23) + 457](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[0x21FC0427 ^ (0x87FD1771u ^ num)](_671BC22C.BF9F3D1F.FE8E0C9E[(int)num2 - -456](current));
								goto IL_074e;
								IL_0472:
								if (_671BC22C.BF9F3D1F.FE8E0C9E[(((uint)(((int)(num2 - num) >> (int)num2) % (int)(~((1519165579 < num) ? 1u : 0u))) > 4294967288u) ? 1 : 0) + 1584](current) == (JitsuriCharacterCharacteristic.CharacterCharacteristicOneofCase)(num2 - 786124880 + 786124883))
								{
									num6 = (num2 + num2 >> (int)num) / ~(num / 2981758512u);
									num2 = (0 - ~num2 * (0 - num6)) ^ 0xEC9CB001u;
									continue;
								}
								num2 = 220732694u >> (int)(0x3989AC & (8392983 + num / num));
								if ((int)num % (int)(~(num2 >> (int)num2)) % ((int)num >> (int)num) == 0)
								{
									num2 = (num2 & num) / (1688329771 + num) << (233807331 << (int)num);
									if ((num & 0x6512) != (num2 & 0x5BEC3010))
									{
										num2 = 2156102420u + (((num ^ (num >> 4)) & (uint)((int)num % (int)(~num2))) | 0x30905693);
										continue;
									}
									goto IL_01b0;
								}
								goto IL_086c;
								IL_032e:
								value4 = _671BC22C.BF9F3D1F.FE8E0C9E[(short)((int)num - ((int)(0 - num2) >> (int)num)) - 5072](_671BC22C.BF9F3D1F.FE8E0C9E[(int)(num + 1794) - -1509877786](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[0x1C2 ^ (num & 1)](_671BC22C.BF9F3D1F.FE8E0C9E[(((int)num - -333353820) & ((int)(num2 - num2) % 825112624)) - -1068](current)) + _671BC22C.BF9F3D1F.FE8E0C9E[1914 + ~(~((int)num / 2023828655))](_671BC22C.BF9F3D1F.FE8E0C9E[-895971042 + (-1889116674 + (int)num)](current));
								if ((uint)((int)((((int)num2 < (int)num2) ? 1u : 0u) & (uint)((int)num >> (int)num2)) / -2144828631 + ((int)(0x45A51485u & ((-182897895 > (int)num) ? 1u : 0u)) >> (int)((num - 77535156) / 4094312845u))) <= (num ^ 0xF14B3256u))
								{
									num2 = 772177322 + ~num / (uint)(721770761 << (int)num2);
									continue;
								}
								goto IL_020a;
								IL_080e:
								attacker.Characteristics[value3] = value2;
								num += 135823489;
								num2 -= 1411033760;
								break;
								IL_01be:
								num3 = _671BC22C.BF9F3D1F.FE8E0C9E[2025 + (~num >> 31) / ((uint)((int)num * (-310223311 << (int)num)) % 3846034739u)](current);
								if (((uint)(~(-13449682 << (int)num)) | ((num % 3695081732u) & ((num ^ num) * 0))) == 0)
								{
									break;
								}
								goto IL_020a;
								IL_086c:
								if (_671BC22C.BF9F3D1F.FE8E0C9E[(byte)(sbyte)(1462975417 >> (int)num) + 1228](enumerator))
								{
									goto IL_01b0;
								}
								if (((int)((0 - num) / num) >> (int)num) * (int)(num2 ^ 0x3D) < 1286027283 << (447285152 << (int)((num * 119639055) & 0xA21E8095u)))
								{
									num2 = (uint)(0x26D2AB32 ^ (sbyte)((int)num2 >> (int)(num2 / 2963982097u)));
									continue;
								}
								goto IL_01be;
								IL_020a:
								num7 = A996FE3D.CABFD5B4.TryGetValue(num3, out value3);
								num ^= 0x9AE687E;
								num2 = 1374055023u;
								if (num7)
								{
									num &= 0xF6015F36u;
									num2 = 0x4AA35A9D ^ (0xC824 & (num % num));
									continue;
								}
								break;
								IL_01b0:
								current = enumerator.Current;
								num = 2813853167u;
								goto IL_01be;
								IL_074e:
								if (~((int)(num & 0x811C1F30u) / (int)num >>> (int)num2) < (int)num + -1582895816)
								{
									num2 = 0x75856C8C ^ (num % ~((num2 >> (int)num2) / (uint)(-1375326532 % (int)num - 1410817675)));
									continue;
								}
								goto IL_0688;
							}
							break;
						}
						goto IL_082d;
					}
					finally
					{
						while (true)
						{
							IL_08ee:
							num6 = 2153778289u;
							num6 = 0xF9AA8A8Bu ^ (891095997 % num6);
							while (true)
							{
								switch (num6 % 3)
								{
								default:
									num6 = 3014167355u + num6;
									if (enumerator != null)
									{
										num = num6;
										if ((uint)(-1467371078 >>> (int)num6) % ~(((uint)(-904769105 >> (int)num6) < (uint)(((3702820636u > num) ? 1 : 0) + (int)(num ^ 0xC820E027u))) ? 1u : 0u) <= (0xDA2EB41Du ^ (((num % 3173893542u) & (num6 + num6)) >> (int)num % 129871133 + -1215167938)))
										{
											continue;
										}
									}
									goto IL_098c;
								case 1u:
									enumerator.Dispose();
									goto IL_098c;
								case 2u:
									num6 = 894106598 + num6;
									break;
								}
								break;
								IL_098c:
								if (0 - num6 + 660588543 > (uint)(-1274258967 & (-1711104368 % (int)(num6 >> ((int)num6 >> (int)num6)))))
								{
									goto IL_08ee;
								}
								num6 = ((1975021737 == (419488458 >>> (int)(3682631810u / num6)) * (-1088957543 + -333589606 * (int)num6)) ? 1u : 0u) - 3035295605u;
							}
							break;
						}
					}
				}
				IL_0069:
				CS_0024_003C_003E8__locals4.ev = tuple.Item2;
				attacker = Attackers.Find((Attacker attacker2) => _671BC22C.BF9F3D1F.FE8E0C9E[46](CS_0024_003C_003E8__locals4.ev) == attacker2.ActorId);
				num2 = 0 - num;
				break;
			}
		}
	}

	internal void HandleFightFighterShowEvent(ByteString value)
	{
		JitsuriFightFighterShowEvent item = _018E928B._29B6D927<FightFighterShowEvent, JitsuriFightFighterShowEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[639]()).map;
		byte b = default(byte);
		int monsterGid = default(int);
		int creatureGrade = default(int);
		int creatureLevel = default(int);
		RepeatedField<JitsuriCharacterCharacteristic> characterCharacteristics = default(RepeatedField<JitsuriCharacterCharacteristic>);
		DofusLibrary.Common.Fight.FightCharacteristics fightCharacteristics = default(DofusLibrary.Common.Fight.FightCharacteristics);
		DofusLibrary.Common.Fight.FightCharacteristics fightCharacteristics2 = default(DofusLibrary.Common.Fight.FightCharacteristics);
		RepeatedField<JitsuriCharacterCharacteristic> characterCharacteristics2 = default(RepeatedField<JitsuriCharacterCharacteristic>);
		DofusLibrary.Common.Fight.FightCharacteristics fightCharacteristics3 = default(DofusLibrary.Common.Fight.FightCharacteristics);
		RepeatedField<JitsuriCharacterCharacteristic> characterCharacteristics3 = default(RepeatedField<JitsuriCharacterCharacteristic>);
		int num = default(int);
		while (true)
		{
			long actorId = _671BC22C.BF9F3D1F.FE8E0C9E[444](_671BC22C.BF9F3D1F.FE8E0C9E[1720](item));
			int cellId = _671BC22C.BF9F3D1F.FE8E0C9E[1466](_671BC22C.BF9F3D1F.FE8E0C9E[799](_671BC22C.BF9F3D1F.FE8E0C9E[1720](item)));
			if (_671BC22C.BF9F3D1F.FE8E0C9E[1207](_671BC22C.BF9F3D1F.FE8E0C9E[147](_671BC22C.BF9F3D1F.FE8E0C9E[1720](item)).Fighter.SpawnInformation) == 0)
			{
				num = 1;
				num = 1035298082 + (num / num >>> 11) * num;
			}
			else
			{
				b = 53;
				if (_671BC22C.BF9F3D1F.FE8E0C9E[147 + ((uint)(3466 << (((int)((uint)b / 3583659306u) > 1872562057) ? 1 : 0)) & (((uint)b > 432878180u) ? 1u : 0u))](_671BC22C.BF9F3D1F.FE8E0C9E[(((-802949206 ^ (b ^ 0x40BE77A6)) << -69 * b) ^ (-b * b >> 0)) + 478155185](item)).Fighter.FighterInformationCase == (JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriFightFighterInformation.FighterInformationOneofCase)(3 + (-(b >> 19) >> 29)))
				{
					b = (byte)(b + b << (int)((uint)(ushort)(sbyte)b / (uint)b));
					if ((0x4485BDB7 | (b % (b % -743882204))) >>> 31 == 0)
					{
						JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriFightFighterInformation.Types.JitsuriAIFighterInformation.Types.JitsuriMonsterFighter monsterFighterInformation = _671BC22C.BF9F3D1F.FE8E0C9E[0x1211019F ^ ((1980855596 + (b >> (int)b >>> (int)b)) & (b + (b + b) * 370492171))](_671BC22C.BF9F3D1F.FE8E0C9E[~(b ^ b) - -1721](item)).Fighter.AiFighter.MonsterFighterInformation;
						monsterGid = monsterFighterInformation.MonsterGid;
						creatureGrade = monsterFighterInformation.CreatureGrade;
						creatureLevel = monsterFighterInformation.CreatureLevel;
						if ((sbyte)(b % ~((uint)b / 3140436147u)) == 0)
						{
							goto IL_0126;
						}
						characterCharacteristics = _671BC22C.BF9F3D1F.FE8E0C9E[0x8AB9B3D ^ b ^ 0x8AB9D8B](_671BC22C.BF9F3D1F.FE8E0C9E[-65 + b](_671BC22C.BF9F3D1F.FE8E0C9E[0x66C ^ b](item)).Fighter.Stats);
						fightCharacteristics = new DofusLibrary.Common.Fight.FightCharacteristics();
						b = (byte)(280501377 * (b / b));
						if ((((uint)b % (uint)b - b) & (uint)(b * b)) <= (uint)((int)b * ((b - b > -954578141) ? 1 : 0) + (int)(3342400431u % (uint)b)))
						{
							goto IL_02fe;
						}
						goto IL_0544;
					}
					goto IL_090a;
				}
				num = (sbyte)(((uint)(b ^ -1408219866) ^ ((uint)b / (uint)b)) * b);
				num = (int)(((-1209135421 == (((uint)num > 4172552990u) ? 1 : 0)) ? 1u : 0u) / (268108288 * ((uint)num / 1957912494u))) * ((num / num) & -853908949);
				num = -1196975690 + (int)(4160749568u / (uint)(~num));
			}
			goto IL_00c7;
			IL_07e7:
			fightCharacteristics2.FromProtobufCharacteristics(characterCharacteristics2);
			b = (byte)(707125695 << num / ~num >> (~b & (-240618434 << num) & (-796757610 % b >> 15)));
			if ((num | 0x40AE19A0) - 344205722 == 0)
			{
				goto IL_090a;
			}
			num = -2073466431 - b + -131940214;
			goto IL_00c7;
			IL_02fe:
			_671BC22C.BF9F3D1F.FE8E0C9E[0x68Cu ^ (((uint)b > 1570255926u) ? 1u : 0u)](fightCharacteristics3.ToString());
			if (num >> (-1416904171 >> (num >> 10)) << num != 0)
			{
				num = (((uint)b % 646091787u == 532492973) ? 1 : 0);
				num = 272019893 * ((short)b * b) - 415080226;
				goto IL_00c7;
			}
			goto IL_090a;
			IL_05ab:
			if (monsterGid == (int)(0x8BE8C ^ ((uint)((-232822122 >> (int)b) & 0x4A8BA94) % (uint)num)))
			{
				IsFightingForTreasure = (byte)(~((num == 696413491) ? 1u : 0u) + 2) != 0;
			}
			Defenders.Add(new Defender(actorId, cellId, fightCharacteristics, (byte)(num + -15368642 - -368130872) != 0, monsterGid, creatureGrade, creatureLevel, (byte)(-(~(b << (int)b)) - 259) != 0));
			num -= -352762229;
			b ^= 0x2C;
			if ((uint)(b >> (int)b >> (num << (int)b)) >= (uint)(0 + num))
			{
				num = -1843902166 + (num / 1956294460 << ((b < b) ? 1 : 0));
				goto IL_00c7;
			}
			break;
			IL_0544:
			fightCharacteristics.FromProtobufCharacteristics(characterCharacteristics);
			num = -1634156277 * b;
			if ((uint)((int)(61940516u % (uint)(-608631918 / num)) >> -1145252422 + (0x7B2D97A3 ^ num)) < (uint)(b ^ 0))
			{
				num = (((uint)(b * (b >> 5)) > (uint)b) ? 1 : 0) - 1148216918;
				goto IL_00c7;
			}
			goto IL_07e7;
			IL_090a:
			UpdateMapInformation();
			if (num > -1994642416 >>> (int)(((uint)b / (uint)(0x29086D1E ^ b)) & (uint)(-267812595 % b)))
			{
				num = (((num ^ ~num) | 0x6005D9A) & num) + 387221522;
				goto IL_00c7;
			}
			goto IL_05ab;
			IL_0126:
			characterCharacteristics3 = _671BC22C.BF9F3D1F.FE8E0C9E[(((num << 19 << (int)(950545685u % (uint)num)) / num) | ((-1700268269 & num) >>> 25)) - 522654](_671BC22C.BF9F3D1F.FE8E0C9E[(((num - 394479878) % num) ^ -1279660649) - ((((1350833192 > num) ? 1u : 0u) > (uint)num / (uint)num) ? 1 : 0) - -1279660796](_671BC22C.BF9F3D1F.FE8E0C9E[1720 + (0x754DDAC8 & num)](item)).Fighter.Stats);
			if (num / -481037145 == 0)
			{
				goto IL_00c7;
			}
			goto IL_0544;
			IL_00c7:
			while (true)
			{
				switch ((uint)num % 13u)
				{
				case 1u:
					fightCharacteristics3 = new DofusLibrary.Common.Fight.FightCharacteristics();
					b = (byte)(((byte)num & -(-1339464562 ^ num)) * -576378195);
					num = (int)(~((uint)((int)((uint)b / (uint)b) / 60440) % (uint)(-(b * 1066113434))) + 2075331989);
					continue;
				case 2u:
					goto IL_01f7;
				case 3u:
					goto IL_024c;
				case 4u:
					goto end_IL_00c7;
				case 5u:
					goto IL_0363;
				case 6u:
					goto IL_0593;
				case 7u:
					goto IL_065f;
				case 8u:
					num = 1 ^ ((-660781255 < b) ? 1 : 0);
					characterCharacteristics2 = _671BC22C.BF9F3D1F.FE8E0C9E[(b & -1820430867) - -1597](_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)(b ^ num) / 934614068u) * (-1316028752 | b | (b - 456229415)) - -147](_671BC22C.BF9F3D1F.FE8E0C9E[b % -1410832939 * -b - -4529](item)).Fighter.Stats);
					num %= b % 647665933 << (int)b;
					num = (int)(0x1C625240 ^ ((uint)(((num % -516988865) ^ -178862612) >>> (num << (int)b)) ^ ((uint)b / 506u)));
					continue;
				case 9u:
					goto IL_078a;
				case 10u:
					goto IL_07d9;
				case 11u:
					goto IL_0844;
				case 12u:
					num = (-1188423106 + (int)((uint)num / (uint)num) + num >>> (int)b) - 426485;
					return;
				}
				num = -272384 + (int)(0x430CBEB1 & ((uint)(num >> (num & 0x501F29A3)) / (uint)((sbyte)num % -1098260087)));
				goto IL_0126;
				IL_0844:
				num = ((num % 1511924118 << num) + ((num < num) ? 1 : 0) >> -b) ^ -13;
				goto IL_0860;
				IL_07d9:
				num = (-1311477279 | num) - -36408851;
				goto IL_07e7;
				IL_078a:
				num = (((uint)b > 666714786u) ? 1 : 0);
				fightCharacteristics2 = new DofusLibrary.Common.Fight.FightCharacteristics();
				b = (byte)(num * b + 79);
				if ((uint)(0 * num) >= ((uint)(num | b) & (3936777005u / (uint)(~num) - (uint)b / (uint)(~num))))
				{
					num = (int)(((uint)num & ((uint)(b * 655297834) / (uint)(~num))) - 847236948);
					continue;
				}
				goto IL_0126;
				IL_01f7:
				num = -15541619 + (num ^ 0x7B5E28E0);
				fightCharacteristics3.FromProtobufCharacteristics(characterCharacteristics3);
				if (b != 0 - 1393122949u % (uint)((num - 1233496509) | (162319108 + b)))
				{
					num = (661366994 << (-1694311497 << num) * -1) - 902280924;
					continue;
				}
				goto IL_07e7;
				IL_065f:
				num = (int)((uint)b % (uint)(-num * (num & b))) * -776442871 + 1180630507;
				goto IL_090a;
				IL_024c:
				num = (((uint)(-b * 1986237662) < (uint)(-627758042 * (539117496 * num))) ? 1 : 0) - -1;
				Attackers.Add(new Attacker(actorId, cellId, fightCharacteristics3, (byte)(2416904714u >> num * -num) != 0, (byte)((num + (ushort)(0x121C9D9E | b)) ^ 0x9DC0) != 0));
				num &= -1811069914;
				if (((1378877107 * ~b) ^ num) != (num ^ (-1433406658 | b)) * num)
				{
					num = (num | -1902820353) - 1869345361;
					continue;
				}
				goto IL_0860;
				IL_0860:
				Defenders.Add(new Defender(actorId, cellId, fightCharacteristics2, (byte)((((uint)(746866079 << (int)b) < (uint)(420479766 % b >>> 8) % 900971275u) ? 1u : 0u) + 1u) != 0, (-15340792 + (int)((uint)(-b) / (uint)(~num))) ^ 0xEA14F7, ((byte)(~(1303206414 >> (int)b)) | (33866274 * (~num & (b + 2125523747)))) + -585879488, -1 + (num - -num << num), (byte)((int)((uint)b / (uint)(~num * -1879419776)) - ((sbyte)(-1364065622 >> num) ^ 0x3287F697) - 847771331) != 0));
				b += 136;
				goto IL_090a;
				continue;
				end_IL_00c7:
				break;
			}
			num = -1475908991 / b + (int)((uint)(-1449872194 >> num + 1672800187) % ((uint)b % (uint)(num ^ -693119342))) + 8531114;
			goto IL_02fe;
			IL_0593:
			num = (-1486746297 / (b ^ 0x4E987317) << 20) - 351713653;
			goto IL_05ab;
			IL_0363:
			num = ((1208603442 % num % b + num) ^ (ushort)num) + -1905262653;
		}
	}

	[AsyncStateMachine(typeof(_003CHandleFightPlacementPossiblePositionsEvent_003Ed__34))]
	internal void HandleFightPlacementPossiblePositionsEvent(ByteString value)
	{
		_003CHandleFightPlacementPossiblePositionsEvent_003Ed__34 stateMachine = default(_003CHandleFightPlacementPossiblePositionsEvent_003Ed__34);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1802]();
		stateMachine._003C_003E4__this = this;
		stateMachine.value = value;
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
	}

	internal async Task<bool> ForgeAndSendChallengeBonusChoiceRequest()
	{
		ChallengeBonusChoiceRequest req = _671BC22C.BF9F3D1F.FE8E0C9E[1998]();
		TaskAwaiter _6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](50));
		TaskAwaiter taskAwaiter = default(TaskAwaiter);
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
		{
			await _6D28EB9F;
			_6D28EB9F = taskAwaiter;
			taskAwaiter = default(TaskAwaiter);
		}
		_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
		if (_instanceData.CharacterParameters.ChallengeChoice == ChallengeChoice.Xp)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1728](req, 0);
		}
		else
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1728](req, 1);
		}
		Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
		_5E33BC89._67B8C522(any, _messageHandler.F1A6D798[_79AC42A1.ChallengeBonusChoiceRequest]);
		_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](req));
		Any _83237E3A = any;
		_messageHandler._051BFF9F(_83237E3A);
		_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](50));
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
		{
			await _6D28EB9F;
			_6D28EB9F = taskAwaiter;
		}
		_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
		try
		{
			return true;
		}
		catch (TaskCanceledException ex)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[376]();
			_1E3B359C._6E3CC3BA(ex.ToString());
			return false;
		}
	}

	internal List<int> FindReachableCells()
	{
		Attacker? self = GetSelf();
		int cellId = self.CellId;
		int value = self.Characteristics[CharacteristicKeyword.MovementPoints].Value;
		Dictionary<int, bool> occupiedCells = Fighter.BuildOccupiedCells(cellId);
		return Fighter.FindReachableCells(cellId, value, occupiedCells);
	}

	internal async Task<int?> FindBestRepositionForCastByMP(int startCellId, DetailedSpellLevel spellLevel, int minRange, int maxRange, IReadOnlyCollection<int> wantedTargetCells, Dictionary<int, bool> occupiedCells, int maxMovementPoints)
	{
		_003C_003Ec__DisplayClass38_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass38_0();
		CS_0024_003C_003E8__locals2.wantedTargetCells = wantedTargetCells;
		int? result = null;
		int num = int.MaxValue;
		foreach (int item in Fighter.FindReachableCells(startCellId, maxMovementPoints, occupiedCells))
		{
			List<int> path = JsPathFinder.GetPath(startCellId, item, MapInformation, occupiedCells, allowDiagonals: false);
			if (path != null && path.Count != 0)
			{
				int num2 = _671BC22C.BF9F3D1F.FE8E0C9E[1937](0, path.Count - 1);
				if (num2 < num && D98DE637._811AB8B8(MapInformation.Cells, item, spellLevel, minRange, maxRange, occupiedCells).Any((_9F8EAF35 mp) => CS_0024_003C_003E8__locals2.wantedTargetCells.Contains(_8F12C8BC._5198AA84(mp.BE93B38F, mp.E8A39495))))
				{
					result = item;
					num = num2;
				}
			}
		}
		return result;
	}

	private void UpdateMapInformation()
	{
		if (!_instanceData.CharacterData.IsFighting)
		{
			return;
		}
		Attacker self = GetSelf();
		if (self == null)
		{
			return;
		}
		_instanceData.CharacterData.CharacterCellId = self.CellId;
		using (List<Attacker>.Enumerator enumerator = Attackers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				_003C_003Ec__DisplayClass39_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass39_0();
				CS_0024_003C_003E8__locals15.attacker = enumerator.Current;
				if (!_instanceData.CharacterData.IsFighting)
				{
					return;
				}
				if (CS_0024_003C_003E8__locals15.attacker.ActorId == _instanceData.CharacterData.CharacterCellId)
				{
					continue;
				}
				if (!CS_0024_003C_003E8__locals15.attacker.IsAlive)
				{
					MapInformation.Actors.RemoveAll((JitsuriActorPositionInformation act) => _671BC22C.BF9F3D1F.FE8E0C9E[444](act) == CS_0024_003C_003E8__locals15.attacker.ActorId);
					continue;
				}
				JitsuriActorPositionInformation jitsuriActorPositionInformation = MapInformation.Actors.Find((JitsuriActorPositionInformation actor) => _671BC22C.BF9F3D1F.FE8E0C9E[444](actor) == CS_0024_003C_003E8__locals15.attacker.ActorId);
				if (jitsuriActorPositionInformation != null)
				{
					JitsuriEntityDisposition jitsuriEntityDisposition = _671BC22C.BF9F3D1F.FE8E0C9E[1934]();
					_3114D522.CAA9F80B(jitsuriEntityDisposition, CS_0024_003C_003E8__locals15.attacker.CellId);
					_2618CDBA.B01EDCBD(jitsuriActorPositionInformation, jitsuriEntityDisposition);
					continue;
				}
				JitsuriActorPositionInformation jitsuriActorPositionInformation2 = _671BC22C.BF9F3D1F.FE8E0C9E[503]();
				_671BC22C.BF9F3D1F.FE8E0C9E[882](jitsuriActorPositionInformation2, CS_0024_003C_003E8__locals15.attacker.ActorId);
				JitsuriEntityDisposition jitsuriEntityDisposition2 = _671BC22C.BF9F3D1F.FE8E0C9E[1934]();
				_3114D522.CAA9F80B(jitsuriEntityDisposition2, CS_0024_003C_003E8__locals15.attacker.CellId);
				_2618CDBA.B01EDCBD(jitsuriActorPositionInformation2, jitsuriEntityDisposition2);
				MapInformation.Actors.Add(jitsuriActorPositionInformation2);
			}
		}
		using (List<Defender>.Enumerator enumerator2 = Defenders.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				_003C_003Ec__DisplayClass39_1 CS_0024_003C_003E8__locals17 = new _003C_003Ec__DisplayClass39_1();
				CS_0024_003C_003E8__locals17.defender = enumerator2.Current;
				if (!_instanceData.CharacterData.IsFighting)
				{
					return;
				}
				JitsuriActorPositionInformation jitsuriActorPositionInformation3 = MapInformation.Actors.Find((JitsuriActorPositionInformation actor) => _671BC22C.BF9F3D1F.FE8E0C9E[444](actor) == CS_0024_003C_003E8__locals17.defender.ActorId);
				if (!CS_0024_003C_003E8__locals17.defender.IsAlive)
				{
					MapInformation.Actors.RemoveAll((JitsuriActorPositionInformation act) => _671BC22C.BF9F3D1F.FE8E0C9E[444](act) == CS_0024_003C_003E8__locals17.defender.ActorId);
					continue;
				}
				if (jitsuriActorPositionInformation3 != null)
				{
					JitsuriEntityDisposition jitsuriEntityDisposition3 = _671BC22C.BF9F3D1F.FE8E0C9E[1934]();
					_3114D522.CAA9F80B(jitsuriEntityDisposition3, CS_0024_003C_003E8__locals17.defender.CellId);
					_2618CDBA.B01EDCBD(jitsuriActorPositionInformation3, jitsuriEntityDisposition3);
					continue;
				}
				JitsuriActorPositionInformation jitsuriActorPositionInformation4 = _671BC22C.BF9F3D1F.FE8E0C9E[503]();
				_671BC22C.BF9F3D1F.FE8E0C9E[882](jitsuriActorPositionInformation4, CS_0024_003C_003E8__locals17.defender.ActorId);
				JitsuriEntityDisposition jitsuriEntityDisposition4 = _671BC22C.BF9F3D1F.FE8E0C9E[1934]();
				_3114D522.CAA9F80B(jitsuriEntityDisposition4, CS_0024_003C_003E8__locals17.defender.CellId);
				_2618CDBA.B01EDCBD(jitsuriActorPositionInformation4, jitsuriEntityDisposition4);
				_671BC22C.BF9F3D1F.FE8E0C9E[2121](jitsuriActorPositionInformation4, new JitsuriActorPositionInformation.Types.JitsuriActorInformation());
				_671BC22C.BF9F3D1F.FE8E0C9E[147](jitsuriActorPositionInformation4).RolePlayActor = new JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriRolePlayActor();
				_671BC22C.BF9F3D1F.FE8E0C9E[147](jitsuriActorPositionInformation4).RolePlayActor.NpcActor = new JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriRolePlayActor.Types.JitsuriNpcStaticInformation();
				_671BC22C.BF9F3D1F.FE8E0C9E[147](jitsuriActorPositionInformation4).RolePlayActor.NpcActor.NpcId = CS_0024_003C_003E8__locals17.defender.Gid;
				MapInformation.Actors.Add(jitsuriActorPositionInformation4);
			}
		}
		if (_instanceData.CharacterData.IsFighting)
		{
			string c9B2633F = _671BC22C.BF9F3D1F.FE8E0C9E[1563](MapInformation);
			_499DDEB5._3C11A432("MapInformation", c9B2633F, _instanceData.ProcessId);
		}
	}

	[AsyncStateMachine(typeof(_003CHandleFightTurnEvent_003Ed__40))]
	internal void HandleFightTurnEvent(ByteString value)
	{
		_003CHandleFightTurnEvent_003Ed__40 stateMachine = default(_003CHandleFightTurnEvent_003Ed__40);
		byte b = default(byte);
		while (true)
		{
			stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1802]();
			short num = -18370;
			num = (short)(((-658814710 << -1106414686 % num) & 0x2833AB99) - 537932358);
			while (true)
			{
				switch ((uint)num % 5u)
				{
				default:
					num = (short)((int)((uint)(-197236470 >>> (int)num) ^ ((2703525951u < (uint)(num ^ -1874871545)) ? 1u : 0u)) + -18431);
					stateMachine._003C_003E4__this = this;
					num = (short)((int)num / (int)(~(((uint)num < 1085630484u) ? 1u : 0u)) * (-669863371 >> num * num) << 8);
					num = (short)((421463572 << (int)((uint)num % (uint)num)) % num - (int)((uint)(1603040912 + num * 874067767) % (uint)(-num)) - -15408);
					break;
				case 1u:
					num = (short)(-1211600468 ^ (-1031785929 * num));
					goto IL_00d7;
				case 2u:
					num = (short)(-b - 30977);
					stateMachine._003C_003E1__state = (ushort)((sbyte)b << (num & -1716487375)) ^ -65536;
					if (((uint)((-896464732 & num) | -1089736149) ^ (((uint)b / (uint)num > (uint)(num % b)) ? 1u : 0u)) + b > (uint)((int)(3492318214u / (uint)(~(num / 2082179501 - 0))) >> (1465171 << num - num) * (-num >>> (int)num)))
					{
						num = (short)(0x16B1 ^ (((uint)b > ((1494082879 > 1646966396 << (int)num) ? 1u : 0u)) ? 1 : 0));
						break;
					}
					goto IL_00d7;
				case 3u:
					num = (short)((int)((uint)(456047872 >>> -1011630304 * num >>> (int)(b - (uint)num / (uint)b)) | (((uint)b > (uint)((412429749 - num) & (b - b))) ? 1u : 0u)) ^ -913057);
					do
					{
						stateMachine._003C_003Et__builder.Start(ref stateMachine);
					}
					while ((int)b + ((-768440173 > -b) ? 1 : 0) >> (int)((uint)(num + -1299547759) % (uint)(~(byte)num)) != 0);
					break;
				case 4u:
					return;
				}
				continue;
				IL_00d7:
				stateMachine.value = value;
				b = (byte)(0 - (uint)num / 3422588172u);
				if ((-637190885 >>> (int)b) / b >>> (-1448229224 | b) != (0 & (b % (2134636441 * b))))
				{
					break;
				}
				num = (short)((ushort)num ^ 0x8AA0);
			}
		}
	}

	internal List<int> GetSpellPossibleCells(int spellId)
	{
		Attacker self = GetSelf();
		return GetSpellPossibleCellsOnCellId(spellId, self.CellId);
	}

	internal List<int> GetSpellPossibleCellsOnCellId(int spellId, int cellId)
	{
		_003C_003Ec__DisplayClass42_0 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass42_0();
		CS_0024_003C_003E8__locals7.spellId = spellId;
		List<int> result = new List<int>();
		JitsuriSpellItem _1A377322 = _instanceData.CharacterData.Spells.Find((JitsuriSpellItem e) => _671BC22C.BF9F3D1F.FE8E0C9E[84](e) == CS_0024_003C_003E8__locals7.spellId);
		if (!_instanceData.CharacterData.DetailedSpells.ContainsKey(CS_0024_003C_003E8__locals7.spellId))
		{
			return result;
		}
		DetailedSpell detailedSpell = _instanceData.CharacterData.DetailedSpells[CS_0024_003C_003E8__locals7.spellId];
		int num = _671BC22C.BF9F3D1F.FE8E0C9E[835](_1A377322) - 1;
		if (num < 0 || num >= detailedSpell.SpellLevels.Levels.Count())
		{
			return result;
		}
		int key = detailedSpell.SpellLevels.Levels[num];
		if (!SpellLevelRepository.Repository.ContainsKey(key))
		{
			return result;
		}
		DetailedSpellLevel detailedSpellLevel = SpellLevelRepository.Repository[key];
		Attacker self = GetSelf();
		int num2 = (self.Characteristics.ContainsKey(CharacteristicKeyword.Range) ? self.Characteristics[CharacteristicKeyword.Range].Value : 0);
		int _8A3D0B = (detailedSpellLevel.CanRangeBeBoosted ? (detailedSpellLevel.Range + num2) : detailedSpellLevel.Range);
		int minRange = detailedSpellLevel.MinRange;
		CS_0024_003C_003E8__locals7.occupiedCells = Fighter.BuildOccupiedCells(self.CellId);
		result = (from e in D98DE637._811AB8B8(MapInformation.Cells, self.CellId, detailedSpellLevel, minRange, _8A3D0B, CS_0024_003C_003E8__locals7.occupiedCells)
			select (int)e.A984429C).ToList();
		if (detailedSpellLevel.NeedFreeCell)
		{
			result = result.Where((int e) => !CS_0024_003C_003E8__locals7.occupiedCells.ContainsKey(e)).ToList();
		}
		return result;
	}

	internal List<int> GetSpellZoneOnCellId(int spellId, int cellId)
	{
		_003C_003Ec__DisplayClass43_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass43_0();
		CS_0024_003C_003E8__locals4.spellId = spellId;
		List<int> result = new List<int>();
		JitsuriSpellItem _1A377322 = _instanceData.CharacterData.Spells.Find((JitsuriSpellItem e) => _671BC22C.BF9F3D1F.FE8E0C9E[84](e) == CS_0024_003C_003E8__locals4.spellId);
		if (!_instanceData.CharacterData.DetailedSpells.ContainsKey(CS_0024_003C_003E8__locals4.spellId))
		{
			return result;
		}
		DetailedSpell detailedSpell = _instanceData.CharacterData.DetailedSpells[CS_0024_003C_003E8__locals4.spellId];
		int num = _671BC22C.BF9F3D1F.FE8E0C9E[835](_1A377322) - 1;
		if (num < 0 || num >= detailedSpell.SpellLevels.Levels.Count())
		{
			return result;
		}
		int key = detailedSpell.SpellLevels.Levels[num];
		if (!SpellLevelRepository.Repository.ContainsKey(key))
		{
			return result;
		}
		DetailedSpellLevel spell = SpellLevelRepository.Repository[key];
		Attacker self = GetSelf();
		return Fighter.GetSpellEffectZone(spell, self.CellId, cellId);
	}

	internal void UpdateSpellCooldown()
	{
		ushort num = 51956;
		List<int> list = default(List<int>);
		if (-(num / num + 2) != 0)
		{
			list = new List<int>();
			num = (ushort)(-(num / -1146759423) | (num >>> -(num + -1768403419)));
		}
		List<int>.Enumerator enumerator = SpellIdOnCooldown.Keys.ToList().GetEnumerator();
		ushort num2;
		try
		{
			ushort num3 = default(ushort);
			int current = default(int);
			Dictionary<int, int> spellIdOnCooldown = default(Dictionary<int, int>);
			int key = default(int);
			while (true)
			{
				num2 = (ushort)(num + -1585016301);
				if ((int)((uint)num2 / 893489019u) <= (int)(0 - 1009576969u / (uint)(~num)))
				{
					if (!enumerator.MoveNext())
					{
						break;
					}
					num3 = 23041;
					if (0 - (0xD3988023u ^ (2878459137u / (uint)num3) ^ (uint)(num3 - -num3)) >= (uint)((0 & num3) >>> 5))
					{
						num3 = (ushort)(84251 + ~num3);
						while (true)
						{
							switch ((uint)num3 % 3u)
							{
							default:
								num3 = (ushort)(-126851373 - num3 - -126935623);
								current = enumerator.Current;
								num2 = (ushort)(0x83004111u ^ (((uint)(~(596611644 >> (int)num3)) < (uint)num3) ? 1u : 0u));
								continue;
							case 1u:
								break;
							case 2u:
								goto end_IL_0080;
							}
							spellIdOnCooldown = SpellIdOnCooldown;
							num3 = (ushort)(((397111856 - (-1717472603 << (int)num2)) % num3) ^ 0x3DAE1310);
							if (1756611134u % (uint)num3 != 0)
							{
								num3 = (ushort)(0x9992 ^ ((uint)(1628833438 / num3) / (uint)(num3 - num2)));
								continue;
							}
							goto IL_015e;
							continue;
							end_IL_0080:
							break;
						}
						num3 = (ushort)(-(-147266408 - -num3) ^ 0x8C6B822);
						key = current;
					}
				}
				spellIdOnCooldown[key] -= 0x1E5FBF13 ^ ((int)((uint)(-1582018930 % num2) % 3785372284u) % -1929173199);
				goto IL_015e;
				IL_015e:
				int num4 = SpellIdOnCooldown[current];
				int num5 = (-695590596 << (0x45196490 ^ ((num2 * num3) | (num2 >> (int)num2)))) ^ 0x68A213C0;
				num = 405;
				if (num4 <= num5)
				{
					list.Add(current);
				}
			}
		}
		finally
		{
			num = 32575;
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		num2 = 16384;
		using List<int>.Enumerator enumerator2 = list.GetEnumerator();
		while (true)
		{
			num2 = (ushort)(932930350 / num2);
			if (enumerator2.MoveNext())
			{
				ushort num3 = 25128;
				int current2 = enumerator2.Current;
				num2 = (ushort)((num3 % num3) & ((num3 + 1) ^ (1612296624 % num3)));
				SpellIdOnCooldown.Remove(current2);
				num2 = (ushort)(num2 - -16384);
				continue;
			}
			break;
		}
	}

	internal void MoveActorIdTo(long actorId, int cellId)
	{
		Attacker attacker = default(Attacker);
		Defender defender = default(Defender);
		while (true)
		{
			_003C_003Ec__DisplayClass45_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass45_0();
			int num = 276522;
			num = 1328570160 + -(num * -1735420760) / (int)(1119192350 % (((uint)num / 2686588337u) ^ (uint)(~num)));
			do
			{
				IL_002c:
				switch ((uint)num % 5u)
				{
				default:
					num = ~num - -1328846683;
					goto IL_0051;
				case 1u:
					num = (((0 & num) << ((num * num) | -num)) * ((num << num) / (num >>> num) << ((2971309852u < (uint)num) ? 1 : 0))) ^ 0x4382A;
					if (attacker != null)
					{
						continue;
					}
					goto IL_00f7;
				case 2u:
					attacker.CellId = cellId;
					goto IL_00f7;
				case 3u:
					num = -(0xF95FFBA & num) ^ -59079942;
					defender = Defenders.Find((Defender d) => d.ActorId == CS_0024_003C_003E8__locals3.actorId);
					num = (((short)num > (num >>> 14) - (num | (num * 1527698365))) ? 1 : 0);
					if ((uint)(1 - num % (byte)num) / (uint)num != 0)
					{
						num = ((-885927271 << num) - num) ^ -395178827;
						break;
					}
					goto IL_0051;
				case 4u:
					{
						num = -3 ^ (num | -(byte)(num & -829427180));
						if (defender != null)
						{
							defender.CellId = cellId;
						}
						return;
					}
					IL_0051:
					CS_0024_003C_003E8__locals3.actorId = actorId;
					do
					{
						attacker = Attackers.Find((Attacker a) => a.ActorId == CS_0024_003C_003E8__locals3.actorId);
					}
					while ((~(num | (num << 25)) | 0x1A59BBE4) == 0);
					num = 1459499274 + (328360374 - num);
					break;
					IL_00f7:
					num = (int)((uint)num / (uint)(num & -839128248));
					num = (int)((((int)(2034216450u % (uint)num) < -num) ? 1u : 0u) ^ (uint)(-108643955 >>> ((num < num - num) ? 1 : 0))) ^ -636795731;
					break;
				}
				goto IL_002c;
			}
			while (((11245240u / (uint)num) ^ ((uint)num / 3608023040u)) % 698416899 * (uint)(-1389004611 * num) != 0);
		}
	}

	internal void HandleGameActionFightEvent(ByteString value)
	{
		ushort num = 0;
		_003C_003Ec__DisplayClass46_0 _003C_003Ec__DisplayClass46_1 = default(_003C_003Ec__DisplayClass46_0);
		(GameActionFightEvent, JitsuriGameActionFightEvent) tuple = default((GameActionFightEvent, JitsuriGameActionFightEvent));
		JitsuriGameActionFightEvent.ActionOneofCase actionOneofCase = default(JitsuriGameActionFightEvent.ActionOneofCase);
		uint num3 = default(uint);
		Defender defender2 = default(Defender);
		int num5 = default(int);
		Attacker attacker3 = default(Attacker);
		int num4 = default(int);
		Defender defender = default(Defender);
		Attacker attacker = default(Attacker);
		Attacker attacker2 = default(Attacker);
		JitsuriGameActionFightEvent.Types.JitsuriSummons.Types.JitsuriSummonsByContextInformation.Types.JitsuriSummonContextInformation current = default(JitsuriGameActionFightEvent.Types.JitsuriSummons.Types.JitsuriSummonsByContextInformation.Types.JitsuriSummonContextInformation);
		RepeatedField<JitsuriCharacterCharacteristic> characterCharacteristics = default(RepeatedField<JitsuriCharacterCharacteristic>);
		JitsuriSpawnInformation current2 = default(JitsuriSpawnInformation);
		RepeatedField<JitsuriCharacterCharacteristic> characterCharacteristics2 = default(RepeatedField<JitsuriCharacterCharacteristic>);
		DofusLibrary.Common.Fight.FightCharacteristics fightCharacteristics2 = default(DofusLibrary.Common.Fight.FightCharacteristics);
		byte b = default(byte);
		Defender defender3 = default(Defender);
		ushort num2 = default(ushort);
		while (true)
		{
			switch ((uint)num % 29u)
			{
			default:
				_003C_003Ec__DisplayClass46_1 = new _003C_003Ec__DisplayClass46_0();
				num = (ushort)(3802810173u % (uint)(~(-(num / ~num))) / (uint)(~num));
				if (0 - (((uint)num > (uint)(1168639631 >>> (num ^ -542033757))) ? 1 : 0) == 0)
				{
					num = (ushort)(63395 + ((2023774260u < (uint)(num >> 15)) ? ((short)1) : ((short)0)));
					break;
				}
				goto IL_0233;
			case 1u:
				num = (ushort)(((num > num) ? 1 : 0) >> (int)((0xEB00E20Cu & ((uint)num % (uint)num)) + num));
				tuple = _018E928B._29B6D927<GameActionFightEvent, JitsuriGameActionFightEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)(1375941097 << (int)num) / (uint)(~num) << (((146190482 % ~num) ^ -1795060440) >>> 29)) - -1055]());
				b = (byte)(((-1263847807 * num) | num) * 0);
				num = (ushort)((((int)b > ((-1968067527 < num) ? 1 : 0)) ? 1u : 0u) - 4294947284u);
				break;
			case 2u:
				num = (ushort)((((uint)(b | num) < (uint)(-687037908 & num) / 3392101796u) ? 1 : 0) >> (~num >> (int)num << (int)((uint)b / 899286316u)));
				_003C_003Ec__DisplayClass46_1.ev = tuple.Item2;
				b = (byte)(-1516949465 - num);
				num = (ushort)(1865307317 - (b | 0x5FB78110) - 259393282);
				break;
			case 3u:
				num = (ushort)(((((num & num) >> 25) + b) | -(b % b)) - 39);
				actionOneofCase = _671BC22C.BF9F3D1F.FE8E0C9E[(uint)b % 1u + 1154](_003C_003Ec__DisplayClass46_1.ev);
				if ((1u ^ ((-b == (b | -551968076) >> (int)b) ? 1u : 0u)) != 0)
				{
					num = (ushort)((b | 0x7B7C3AEC) + -2071697384);
					break;
				}
				goto IL_045d;
			case 4u:
				num = (ushort)(~(num / -1) + -41734);
				goto IL_0233;
			case 5u:
				num = (ushort)((0x501EED97 | (-1758819930 / num2)) + 69705);
				switch (actionOneofCase - (num + 3))
				{
				case JitsuriGameActionFightEvent.ActionOneofCase.ReflectSpell:
					goto IL_04bf;
				case JitsuriGameActionFightEvent.ActionOneofCase.Vanish:
					goto IL_0b3b;
				case JitsuriGameActionFightEvent.ActionOneofCase.UnmarkCells:
					goto IL_18b4;
				case JitsuriGameActionFightEvent.ActionOneofCase.TeleportOnSameMap:
					goto IL_194d;
				case JitsuriGameActionFightEvent.ActionOneofCase.Ejcr:
					goto IL_1bc2;
				case JitsuriGameActionFightEvent.ActionOneofCase.Ejda:
					goto IL_1c7a;
				case JitsuriGameActionFightEvent.ActionOneofCase.None:
					goto IL_1d0f;
				case JitsuriGameActionFightEvent.ActionOneofCase.CarryCharacter:
				case (JitsuriGameActionFightEvent.ActionOneofCase)2:
				case JitsuriGameActionFightEvent.ActionOneofCase.StealKama:
				case JitsuriGameActionFightEvent.ActionOneofCase.ExchangePositions:
				case JitsuriGameActionFightEvent.ActionOneofCase.DropCharacter:
				case JitsuriGameActionFightEvent.ActionOneofCase.SpellRemove:
				case JitsuriGameActionFightEvent.ActionOneofCase.ModifyEffectsDuration:
				case JitsuriGameActionFightEvent.ActionOneofCase.ChangeLook:
					return;
				}
				goto IL_02d8;
			case 6u:
				num = (ushort)((-33059709 - (int)((uint)(b & b) / (uint)b) << (0x768107AB & b) - (int)((uint)num / (uint)(b << (int)b))) - 1073741824);
				if (actionOneofCase != (JitsuriGameActionFightEvent.ActionOneofCase)(22 + num))
				{
					if ((num2 & ((1988538634 + 1686870313u / (uint)(~num)) % 1914065923)) != 0)
					{
						num = (ushort)((num >>> (((uint)(-(-num2)) > 800528520u) ? 1 : 0)) - -42637);
						break;
					}
					goto IL_0233;
				}
				return;
			case 7u:
				num = (ushort)(~(num2 << (int)(short)(0x4B8C8A9B | num2)) - 1879048191);
				goto IL_03b2;
			case 8u:
				num = (ushort)(858999297 / num + -36022);
				return;
			case 9u:
				num = (ushort)(4278728753u + (~(0xB43CBC3Du & num3) >> (int)b));
				if (actionOneofCase != (JitsuriGameActionFightEvent.ActionOneofCase)((120427211 / (int)(~((b < b) ? 1u : 0u)) << 28 >> (int)num3) - 163808))
				{
					goto IL_045d;
				}
				num3 = (uint)((int)num3 % -4659808 >> (int)b) % ~((uint)((int)(4279044248u / num3) >> 30) / (uint)(~num));
				if ((0x723E9115 & ((int)num3 >> (int)(~(num3 << 7)))) != 0)
				{
					num = (ushort)((0xBCA22688u | num3 | 0x13A9E0) + (num3 ^ 0x763C2507) - 2311258345u);
					break;
				}
				goto IL_0b07;
			case 10u:
				num = (ushort)(b * 1079688935 - -841804495);
				goto IL_04a8;
			case 11u:
				num = (ushort)((809080355 + b) ^ 0x3039964A);
				if (IsFightingForTreasure)
				{
					b = (byte)(-num);
					num = (ushort)(((int)(1781962914u / (uint)(784293797 % num2)) * (b >>> 5) << (-1237841491 + num2 / 1572217012) % -174016112) - -18949);
					break;
				}
				return;
			case 12u:
				num = (ushort)((num2 & -137) - 21618);
				if (_671BC22C.BF9F3D1F.FE8E0C9E[num2 - b - 19922](_003C_003Ec__DisplayClass46_1.ev).TargetId < (-(num % num2) ^ -1))
				{
					TreasureTargetId = _671BC22C.BF9F3D1F.FE8E0C9E[-1009226088 - num2 + 1009249402](_003C_003Ec__DisplayClass46_1.ev).TargetId;
					if ((uint)num2 > ((-num2 < 0) ? 1u : 0u))
					{
						num = (ushort)(-706457125 + ((num2 ^ 0x71387699) - 1193014948));
						break;
					}
					goto IL_0233;
				}
				return;
			case 13u:
				num = (ushort)(0 / (int)(~(((uint)(num2 / 1653930407) / 2562387358u == (uint)(num2 + -8376)) ? 1u : 0u)));
				return;
			case 14u:
				num = (ushort)(65 + ((-1220620114 >> ((b >>> 0) ^ 0x78557B7)) | (num | 0x34985B8D)));
				defender2 = Defenders.Find(_003C_003Ec__DisplayClass46_1._003CHandleGameActionFightEvent_003Eb__0);
				if (defender2 == null)
				{
					goto IL_087a;
				}
				if (~num3 != 0)
				{
					num = (ushort)(num3 - 4294880981u);
					break;
				}
				goto IL_0fde;
			case 15u:
				num = (ushort)((~((uint)(-468148073 + num) / (uint)num) | (uint)(num + -1357632719)) - 4294964211u);
				defender2.PermanentDamage += _671BC22C.BF9F3D1F.FE8E0C9E[(~num3 / (uint)(b ^ (num + -568555256))) ^ 0x101](_003C_003Ec__DisplayClass46_1.ev).PermanentDamages;
				num5 = _671BC22C.BF9F3D1F.FE8E0C9E[257 + (0 - (num3 - num3 << (int)b << (int)(2900983010u / (uint)(~num))))](_003C_003Ec__DisplayClass46_1.ev).PermanentDamages - _671BC22C.BF9F3D1F.FE8E0C9E[((278536500 << ~num >>> (int)b) - (-961110639 + (int)((0x9B9D77B1u & num3) ^ 0x4631C5A4))) ^ 0x5B9C9FDB](_003C_003Ec__DisplayClass46_1.ev).Loss;
				if (num5 > (int)(498673422u % (uint)(~(sbyte)num) >> 25) + -14)
				{
					num3 = (uint)(-113 - num);
					if ((0xFB389B6 | ~num3) != ((uint)((int)num3 * -1129760619 + -1798010081 - (int)(3333466385u % num3 % b)) | ((uint)(num & b) % 4103581735u)))
					{
						num = (ushort)(((-2011735540 & (-1629772723 << (int)((uint)num % 781663420u))) << (int)num) + 2012070272);
						break;
					}
					goto IL_087a;
				}
				return;
			case 16u:
				num = (ushort)(780889269 + (((num - (0x42940C3D & num)) & num) ^ -780905781));
				defender2.Characteristics[(CharacteristicKeyword)(((num ^ (num - num >> (int)num3)) >>> ((num % 437444133) | 0xBA4B96)) - -79)].Value += num5;
				return;
			case 17u:
				num = (ushort)(((short)b % -1147960688 / ~num2) ^ 0x3C ^ -27);
				attacker3 = Attackers.Find(_003C_003Ec__DisplayClass46_1._003CHandleGameActionFightEvent_003Eb__1);
				num = (ushort)((int)num3 / 28990 + (1780022315 << (int)((((uint)num < 2081123712u) ? 1u : 0u) % num3)));
				num = (ushort)((uint)(-1121962088 ^ ((int)(0xA2280405u | num3) / (-1861029604 / num))) / num3 + 64949);
				break;
			case 18u:
				num = (ushort)((num3 | 0x3B36) + 80094);
				if (attacker3 != null && (uint)(20 / (-752602709 & num)) < 1896423713 / ((num3 << 6) + 668976068))
				{
					num = (ushort)(2220628329u + ((0 - num3) ^ 0x7BA38E9A ^ b));
					break;
				}
				return;
			case 19u:
				num = (ushort)((-1704594050 + (num >> 23)) ^ -1704653525);
				attacker3.PermanentDamage += _671BC22C.BF9F3D1F.FE8E0C9E[257 + 850246145 * (((num2 >>> 19 < (int)num3) ? 1 : 0) % (int)(short)num)](_003C_003Ec__DisplayClass46_1.ev).PermanentDamages;
				num3 = (uint)(b ^ num);
				num = (ushort)(1688594330 * num3 - 2319330300u);
				break;
			case 20u:
				num = (ushort)((0x23106009 ^ num3) - 588226598);
				num4 = _671BC22C.BF9F3D1F.FE8E0C9E[0xD1 ^ (((1216792711u % (uint)num) ^ 0x3A05EEA8) >> (int)(~num3 % 2342909592u))](_003C_003Ec__DisplayClass46_1.ev).PermanentDamages - _671BC22C.BF9F3D1F.FE8E0C9E[(-598219422 * (int)(((-1650335697 > b) ? 1u : 0u) ^ 0xC0015B6u) >>> (byte)(num ^ -2010301663) - num2) - 1603](_003C_003Ec__DisplayClass46_1.ev).Loss;
				num = (ushort)((int)((0 - num3) * 318849827) % 37);
				num = (ushort)((-1904152838 >> (num2 & 0)) ^ -1904178843);
				break;
			case 21u:
				num = (ushort)((((int)num3 + -742) & ((int)((uint)(num ^ num) & num3) / (num | 0x2995C72D))) - -9);
				goto IL_0ade;
			case 22u:
				num = (ushort)((uint)num / uint.MaxValue >> 470134959 - num + -1888602393);
				defender = Defenders.Find(_003C_003Ec__DisplayClass46_1._003CHandleGameActionFightEvent_003Eb__2);
				if ((num | 0x60107486) >= b)
				{
					num = (ushort)((uint)num2 / 724248239u + 52426);
					break;
				}
				goto IL_0ade;
			case 23u:
				num = (ushort)(b - 39);
				if (defender != null)
				{
					b = (((uint)(~num >>> 18) % (uint)(num2 & num2) < b) ? ((byte)1) : ((byte)0));
					num = (ushort)((0xA050019 ^ (num ^ -b)) - 168040797);
					break;
				}
				attacker = Attackers.Find(_003C_003Ec__DisplayClass46_1._003CHandleGameActionFightEvent_003Eb__3);
				num2 = (ushort)(~(b - num2) << -num2);
				if ((-560671603 >> (-796856426 | num2)) / 1997772821 * ((~num2 == -10) ? 1 : 0) >= (int)(3943764770u / (uint)(~(1 - (((uint)num < 406069123u) ? 1 : 0)))) >> (int)b)
				{
					num = (ushort)((num2 >> 16) * ((b - 361025043) * (int)((((uint)num2 < (uint)num) ? 1u : 0u) | (uint)(422072836 - num))) + 7624);
					break;
				}
				goto IL_0b07;
			case 24u:
				num = (ushort)((-124822466 << (int)num2) * num2 * ((b & b) >>> 18) + -599609522 - -599609522);
				defender.Characteristics[(CharacteristicKeyword)(-((b % ~(b / 504302901)) | -2069638989) ^ 0x7B5C2F02)].Value += _671BC22C.BF9F3D1F.FE8E0C9E[(-1288364101 % ~num * b >> ~num) + 1775](_003C_003Ec__DisplayClass46_1.ev).Delta;
				num3 = (uint)(ushort)(-493408503 >>> (-1540360438 | num)) / (uint)(~(-num));
				num = (ushort)((-36 | num) - -43590);
				break;
			case 25u:
				num = (ushort)(0xF0E7C27 ^ ((int)((uint)(-577737792 | (b / num2)) | (0 - num3)) - -830345319));
				defender.Characteristics[(CharacteristicKeyword)(0x2584F83 ^ (num + 39342028))].Value = _671BC22C.BF9F3D1F.FE8E0C9E[(b % 800713898 >> (int)num3) - -325](defender.Characteristics[(CharacteristicKeyword)(79 + (((uint)(-37751875 / (num2 ^ b) % 598415505) < (uint)(1265291082 >> (int)b << (int)(((b < 126234890) ? 1u : 0u) % ~num3))) ? 1 : 0))].Value, b + 0);
				return;
			case 26u:
				num = (ushort)(-(num2 % ~(b >>> 30) >>> b + 717100735));
				if (attacker != null)
				{
					attacker.Characteristics[(CharacteristicKeyword)((b & (short)b) - -40)].Value += _671BC22C.BF9F3D1F.FE8E0C9E[((1351594017 == b) ? 1 : 0) * (-1415033454 * num2) - -1775](_003C_003Ec__DisplayClass46_1.ev).Delta;
					if ((0x40A3DD80 | (b / ~num)) % (short)(b % num2) != 0 + ((b * -273096300 * 1208025089) ^ num))
					{
						num = (ushort)((((int)num2 - ((~num2 == b) ? 1 : 0)) / 1554415772) ^ 0x968B);
						break;
					}
					goto IL_0b07;
				}
				return;
			case 27u:
				num = (ushort)(0x65801A4 ^ ((4152082428u / (uint)b) ^ num2));
				attacker.Characteristics[(CharacteristicKeyword)((byte)((0x4B87A896 ^ b) << (1447032212 >> (int)b)) + 79)].Value = _671BC22C.BF9F3D1F.FE8E0C9E[((b ^ b) << 4 >> 20 % (int)(~((1127589161 < -b) ? 1u : 0u))) + 325](attacker.Characteristics[(CharacteristicKeyword)(-(-1162731636 | num2) ^ 0x454D603B)].Value, (int)((uint)(-(num ^ num2)) / (uint)(~((int)(((num2 > -2054075117) ? 1u : 0u) ^ 0x4FB15B84u) / -2027336828))));
				if (((130 << (int)num2 > (int)(504260157u / ((num > -836649845) ? 1u : 0u))) ? 1 : 0) >= ((1076891648u > (uint)((b * -b) | (-2011437906 << (int)((uint)num / 2628896783u)))) ? 1 : 0))
				{
					num = (ushort)(0x8724 ^ ((uint)(~(num2 | 0x2119923B) * 1864440473) / 4294948055u));
					break;
				}
				goto IL_0400;
			case 28u:
				{
					num = (ushort)(998298135 + num - 998332731);
					return;
				}
				IL_1d0f:
				num = (ushort)((748371008 >> (int)num) / -806050196);
				if ((ushort)(199807092 - 4257217039u / (uint)(~num)) != 0)
				{
					num2 = (ushort)(4294967205u / (uint)(~((b >>> (int)b >>> 6) * 458551599)) + 12318);
					goto IL_1643;
				}
				goto IL_17c8;
				IL_1c7a:
				b = (byte)num;
				num2 = (ushort)(-(-(b + num2)) ^ 0x4349);
				goto IL_1643;
				IL_194d:
				num = (ushort)(-(-num2) / 521502385);
				if ((uint)(~b) < (uint)(1929295904 - b))
				{
					return;
				}
				_671BC22C.BF9F3D1F.FE8E0C9E[(-1568198623 >>> (int)num2) - 9619](_003C_003Ec__DisplayClass46_1.ev);
				b = (byte)(-736364271 + (-391268705 % ~num << 13));
				if ((uint)((int)((uint)(-22564470 << (int)b) / 3426231343u) % (int)num2) < (uint)(185081858 % num2) != (int)(29u % (uint)(1672972045 << (int)num)) > (int)(num2 | ((uint)num / (uint)(num2 | 0x7CA3869F))))
				{
					num2 = (ushort)(num - -14091);
					goto IL_1643;
				}
				goto IL_1898;
				IL_17c8:
				attacker2 = Attackers.Find(_003C_003Ec__DisplayClass46_1._003CHandleGameActionFightEvent_003Eb__5);
				num2 = (ushort)(((uint)(num / -2925) ^ (446760633u / (uint)(~num))) >> (int)num);
				num2 = (ushort)(21143u + ((1301553428 > (int)(num3 % num3) % (int)b) ? 1u : 0u));
				goto IL_1643;
				IL_1a04:
				MoveActorIdTo(_671BC22C.BF9F3D1F.FE8E0C9E[((uint)(ushort)(b & (102290465 >> (int)num2)) % 3935938730u) ^ 0x30E](_003C_003Ec__DisplayClass46_1.ev), _671BC22C.BF9F3D1F.FE8E0C9E[0xA851 ^ (((b * b) ^ (0xF931 ^ num2)) >>> (631582084 * ((b > 742163201) ? 1 : 0) >>> (b >> ((1939437866u < (uint)b) ? 1 : 0))))](_003C_003Ec__DisplayClass46_1.ev).CasterCellId);
				b = (byte)(((b & -1287357231) / ~num) ^ 0x98);
				if ((uint)((int)((uint)(num + -1900327116) / ((b < num2) ? 1u : 0u)) / 416145671) % (uint)(num2 * (0 ^ b)) != 0)
				{
					num2 = (ushort)(((((uint)(1629628556 >>> (int)num) < (uint)num) ? 1 : 0) + ((-num2 > 328215204) ? 1 : 0)) ^ 0xF10A);
					goto IL_1643;
				}
				goto IL_1bc2;
				IL_18b4:
				if ((uint)(914460694 >> (int)((((uint)b < (uint)num2) ? 1u : 0u) << b / -969852406)) / (uint)(~(num2 * num)) == 0)
				{
					num2 = (ushort)(64280 + num);
					goto IL_1643;
				}
				return;
				IL_0400:
				num3 = (uint)(-2044978291 + num);
				num = (ushort)(1635882529 * num3 - 282092194);
				break;
				IL_0b3b:
				if (1015836033u % (uint)(-(-(num2 << 26))) != (uint)(~(num - num2) ^ ((0x719AB68A & num) % 1228050751)))
				{
					num = (ushort)(8777 + (-1181424377 / (b * b) >>> 30));
					break;
				}
				goto IL_04a8;
				IL_1bc2:
				num2 = (ushort)(0 & num);
				if ((uint)(0x377C ^ (-693499642 * (num2 & b))) >> 1 != 0)
				{
					num2 = (ushort)(-2005518985 ^ ((((-64662219 == num2) ? 1 : 0) * 0 - 2005493554) ^ ((int)((uint)(-b) / 1302840171u) - -1328750388 * (int)((uint)num2 % (uint)b))));
					goto IL_1643;
				}
				goto IL_1a04;
				IL_04a8:
				if (actionOneofCase != (JitsuriGameActionFightEvent.ActionOneofCase)(689354152 * b - 1115008113))
				{
					return;
				}
				goto IL_0fde;
				IL_0fde:
				try
				{
					IEnumerator<JitsuriGameActionFightEvent.Types.JitsuriSummons.Types.JitsuriSummonsByContextInformation.Types.JitsuriSummonContextInformation> enumerator = _671BC22C.BF9F3D1F.FE8E0C9E[(byte)(1 & (4280844676u / (uint)(~num))) - -1769](_003C_003Ec__DisplayClass46_1.ev).SummonsByContextInformation.Summons.GetEnumerator();
					try
					{
						if (-35267399 / (int)(~((num == 1998630656 - num) ? 1u : 0u)) == 0)
						{
							num2 = (ushort)(0x202ED3AC | ((b & (num / -1372563018)) | num));
							goto IL_1058;
						}
						goto IL_15ac;
						IL_15ac:
						if (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
						{
							current = enumerator.Current;
							num2 = 56888;
							goto IL_1058;
						}
						return;
						IL_1058:
						IEnumerator<JitsuriSpawnInformation> enumerator2 = current.Summons.GetEnumerator();
						try
						{
							if ((num2 | -1933791960) > num2 - ((byte)(3246520498u / (uint)num2) - (0x113AA0A4 | num2)))
							{
								num2 = (ushort)(num2 + 1429432099);
								num2 = (ushort)(-970011125 ^ (-249087104 * num2));
								goto IL_10a4;
							}
							goto IL_14c0;
							IL_1314:
							characterCharacteristics = _671BC22C.BF9F3D1F.FE8E0C9E[num2 ^ 0x359D](current.Characteristics);
							goto IL_1330;
							IL_10a4:
							while (true)
							{
								switch ((uint)num2 % 7u)
								{
								case 1u:
									goto IL_1285;
								case 2u:
									goto end_IL_10a4;
								case 3u:
									goto IL_1361;
								case 4u:
									goto IL_13a4;
								case 5u:
									goto IL_150d;
								case 6u:
									goto end_IL_1066;
								}
								num2 = (ushort)((short)(-(-400485759 % num2)) - -76131);
								goto IL_14c0;
								IL_150d:
								num2 = (ushort)(-56889 ^ (((int)(~((uint)num2 / (uint)b)) >> (int)num2) + (int)(0x321B81AAu & ((b < num2) ? 1u : 0u))));
								if (_671BC22C.BF9F3D1F.FE8E0C9E[1750633823 + 2544334854u % (uint)(short)num2](enumerator2))
								{
									current2 = enumerator2.Current;
									if (_671BC22C.BF9F3D1F.FE8E0C9E[1207](current2) != 1)
									{
										num2 = 13311;
										if ((num2 >>> (int)num2) * 2 == 0)
										{
											num2 = (ushort)(0xF215 ^ ((uint)(num2 % num2 / num2) % (uint)num2));
											continue;
										}
										goto IL_14c0;
									}
									b = 167;
									if (2 * b == 0)
									{
										goto IL_1330;
									}
									characterCharacteristics2 = _671BC22C.BF9F3D1F.FE8E0C9E[b - (b >> 0) - -1634](current.Characteristics);
									num = (ushort)(b / b);
								}
								else if (((-1509443397 >>> (-920407268 ^ num2)) | ((b << (int)num2) * ~num2)) != (b ^ -582043582))
								{
									continue;
								}
								DofusLibrary.Common.Fight.FightCharacteristics fightCharacteristics = new DofusLibrary.Common.Fight.FightCharacteristics();
								fightCharacteristics.FromProtobufCharacteristics(characterCharacteristics2);
								b = (byte)(b ^ num);
								Defenders.Add(new Defender(_671BC22C.BF9F3D1F.FE8E0C9E[2116535690 + num - 2116535247](_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)b | (((uint)b > (uint)(b + 1042853428)) ? 1u : 0u)) - -1620](current2)), _671BC22C.BF9F3D1F.FE8E0C9E[(byte)(num % ~((uint)num % (uint)num)) + (num | b) * ((-642477535 ^ num) >> (int)num) - -2107268169](_671BC22C.BF9F3D1F.FE8E0C9E[(uint)(num * 1605939205 >> (int)(byte)num >>> (int)b) / (uint)(0x6BA3419C ^ ((num >>> 20) / (b | 0x6CBB2218))) + 799](_671BC22C.BF9F3D1F.FE8E0C9E[b - -1581748165 - 1581746545](current2))), fightCharacteristics, (byte)(-num + 2) != 0, -1187805667 ^ num ^ 0x46CC79E3, -167 ^ b, -167 + (b + (num >>> (int)num)), (byte)(1215046494 + (-1215046494 ^ num)) != 0));
								num2 = 56888;
								num2 = (ushort)((1392591038 << (int)num2) + 1107360146);
								continue;
								IL_1361:
								num2 = (ushort)(-13312 ^ (0 - (((num ^ num2) < num) ? 1 : 0)));
								fightCharacteristics2.FromProtobufCharacteristics(characterCharacteristics);
								if (-(num2 ^ -692234624) << (int)(0x1582B089 ^ (3215537967u % (uint)num2 % num)) == 0)
								{
									goto IL_14c0;
								}
								continue;
								IL_1285:
								num2 = (ushort)(56888u + ((~(-281525885 << (int)((uint)b % (uint)b)) == ((1900409210 == num2) ? 1 : 0) % -1885455831) ? 1u : 0u));
								goto IL_14c0;
								IL_13a4:
								Attackers.Add(new Attacker(_671BC22C.BF9F3D1F.FE8E0C9E[-33032 ^ ((int)((uint)((int)(111627276u / (uint)num) * (num2 - num)) / (uint)(num + num)) / ~(((2745287577u < (uint)num2) ? 1 : 0) % 573516198))](_671BC22C.BF9F3D1F.FE8E0C9E[(num >>> (int)num2) - -1786](current2)), _671BC22C.BF9F3D1F.FE8E0C9E[((uint)(num2 << 14) | ((uint)num2 / 1727915754u)) - 218085958](_671BC22C.BF9F3D1F.FE8E0C9E[((sbyte)(-266372301 >>> (num2 | 0x7B2C1F27)) ^ 0x4B9F14A4) - 1268715910](_671BC22C.BF9F3D1F.FE8E0C9E[-1613 ^ (((num & num2) + -547396123 + num) % (num & 0x6306C135 & num2))](current2))), fightCharacteristics2, (byte)((num2 * -677104612) ^ -2098103323) != 0, (byte)((((uint)(num2 >> (int)num2) % (uint)(byte)num2) & (((2652672905u < (uint)num) ? 1u : 0u) << (int)((uint)(num % num2) / (uint)num2))) - uint.MaxValue) != 0));
								num2 = (ushort)(num2 - -43577);
								goto IL_14c0;
								continue;
								end_IL_10a4:
								break;
							}
							num2 = (ushort)(((num2 - -2110992496) % -325598205 >>> ((891547275u > ((430916864 > (int)((uint)num2 % 4043508537u)) ? 1u : 0u)) ? 1 : 0)) + -78719308);
							goto IL_1314;
							IL_1330:
							fightCharacteristics2 = new DofusLibrary.Common.Fight.FightCharacteristics();
							num = (ushort)(-((num2 & 0x673A49AD) << 2));
							num2 = (ushort)((-1103768290 << num2 - num) ^ -1460611177);
							goto IL_10a4;
							IL_14c0:
							b = (byte)(1837966394 >> (int)((uint)(num2 % (1787922703 + num2)) / 1371387961u));
							if (-1270783025 / (-457367803 / b >> (int)num2) != 0)
							{
								num2 = (ushort)(10800 * num2 - 614350565);
								goto IL_10a4;
							}
							goto IL_1314;
							end_IL_1066:;
						}
						finally
						{
							if (enumerator2 != null)
							{
								num2 = 45579;
								if ((byte)(1613359389 % (sbyte)num2) != 0)
								{
									enumerator2.Dispose();
								}
							}
						}
						goto IL_15ac;
					}
					finally
					{
						if (enumerator != null)
						{
							b = 53;
							enumerator.Dispose();
						}
					}
				}
				catch
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[376]();
					return;
				}
				IL_04bf:
				if ((b ^ 0x3236704) - ((int)((uint)(-b) / 806080435u) - -969098354) != 0)
				{
					num = (ushort)(-1416696424 + (num2 << ~(0x5133380D | b) + (num2 >>> 18)));
					break;
				}
				goto IL_02d8;
				IL_087a:
				num2 = (ushort)((ushort)(num >> 8) >> (int)(num3 % num3));
				if ((int)(byte)(num - num2 >> 16) >= (int)((b ^ num3) * num2 / 1578083713))
				{
					num = (ushort)(((b << (int)num3) & (1335848092 - num2)) + -58675057);
					break;
				}
				goto IL_03b2;
				IL_0233:
				if ((int)actionOneofCase <= (0x559EFF ^ (num ^ 0x559EE2)))
				{
					num2 = (ushort)(b - 1865067445);
					num = (ushort)(~(((0u < (uint)b) ? 1 : 0) >> (((uint)(-383472633 & b) < (uint)b) ? 1 : 0)) + 64560);
					break;
				}
				goto IL_0400;
				IL_02d8:
				if ((((0x7AB02510 ^ num) - -1985615308) | (0 | ((-805229137 >> (int)b) + 322260775))) != 0)
				{
					num = (ushort)((1871017630 << ((b % ~num > num2) ? 1 : 0)) - 1870985869);
					break;
				}
				goto IL_0400;
				IL_1898:
				attacker2.IsAlive = (byte)(14 + (~b >> (int)((uint)(short)num2 & num3 & num2))) != 0;
				return;
				IL_03b2:
				if (actionOneofCase != (JitsuriGameActionFightEvent.ActionOneofCase)((num2 & 0) - -29))
				{
					if ((sbyte)((uint)num2 % 2643062966u) != 0)
					{
						num = (ushort)(-2392 ^ (short)(-num2 >>> -1708726382 % ~num));
						break;
					}
					goto IL_0ade;
				}
				num >>= 17;
				num2 = (ushort)((1570284317 >> (int)num) * (num2 - ~b) * (b / 1670930863 >> ((497199886 >> (int)num2) | 0x4446)) - -32927);
				goto IL_1643;
				IL_045d:
				if (actionOneofCase != (JitsuriGameActionFightEvent.ActionOneofCase)((int)(~num3 ^ (((uint)b / (uint)(~num) < ((1252361529 < num3) ? 1u : 0u)) ? 1u : 0u)) + -2044978255))
				{
					b = b;
					num = (ushort)(64690 + -b);
					break;
				}
				goto IL_15f7;
				IL_15f7:
				num2 = (ushort)(2319651777u % (uint)(-(((b > b) ? 1 : 0) + (int)num3)));
				if ((uint)(-((int)num3 / (-1642175610 ^ (53698073 % b)))) >= ((((uint)num > 265314u) ? 1u : 0u) ^ (uint)((int)(num * num3) >> 30 >> ((int)b / (int)num3 << (int)num3))))
				{
					goto IL_1643;
				}
				goto IL_1898;
				IL_0ade:
				if (num4 >= (int)((uint)num / 671643936u) % (((uint)(b & -2096772095) < (uint)(-995214836 % (b - -1917140302))) ? 1 : 0))
				{
					return;
				}
				goto IL_0b07;
				IL_0b07:
				attacker3.Characteristics[(CharacteristicKeyword)((byte)(-961913538 >> 495871143 + b * b) + -119)].Value += num4;
				return;
				IL_1643:
				while (true)
				{
					switch ((uint)num2 % 14u)
					{
					case 1u:
						goto IL_16e7;
					case 2u:
						num2 = (ushort)(12110 + -(num2 % -1365295597 / 1278483853));
						defender3.IsAlive = (byte)(96717 + (-96717 ^ num)) != 0;
						if (num / 1304612309 * num * -426178271 == 0)
						{
							num2 = (ushort)((short)(0 - (num3 + 1924312092)) + -16324);
							continue;
						}
						return;
					case 3u:
						num2 = (ushort)((-1881553085 + ((num | -1876649466) ^ 0x7F80A7BB)) * (b & (-2019144164 >>> (b << 31))) + 34085710);
						return;
					case 4u:
						goto IL_181a;
					case 5u:
						goto end_IL_1643;
					case 6u:
						num2 = (ushort)(0x5473 ^ ((uint)b / (uint)b));
						MoveActorIdTo(_671BC22C.BF9F3D1F.FE8E0C9E[(b | 0x5170F8A) - 85395955](_003C_003Ec__DisplayClass46_1.ev).TargetId, _671BC22C.BF9F3D1F.FE8E0C9E[(-1783185238 & b) - -1434](_003C_003Ec__DisplayClass46_1.ev).Cell);
						return;
					case 7u:
						goto IL_19f9;
					case 8u:
						goto IL_1aea;
					case 9u:
						num2 = (ushort)(20957 + (1402384794 >> (int)((uint)num2 % (uint)b) - ((-1179184599 == num) ? 1 : 0)) % num2);
						return;
					case 10u:
						num2 -= 44474;
						MoveActorIdTo(_671BC22C.BF9F3D1F.FE8E0C9E[(b | -418556134) - -418556941](_003C_003Ec__DisplayClass46_1.ev).TargetId, _671BC22C.BF9F3D1F.FE8E0C9E[((num2 - b * num2) * ~(-b)) ^ 0x34C](_003C_003Ec__DisplayClass46_1.ev).Cell);
						return;
					case 11u:
						num2 = (ushort)(((uint)num % (uint)(num2 / num2) % (uint)(((436479626 >> (int)b) ^ b) | (b * (0x690ABC82 ^ num2)))) ^ 0x5472);
						MoveActorIdTo(_671BC22C.BF9F3D1F.FE8E0C9E[-255862649 + b + 255862953](_003C_003Ec__DisplayClass46_1.ev).TargetId, _671BC22C.BF9F3D1F.FE8E0C9E[~b + 305](_003C_003Ec__DisplayClass46_1.ev).Cell);
						return;
					case 12u:
						num2 = (ushort)(b - -21579);
						MoveActorIdTo(_671BC22C.BF9F3D1F.FE8E0C9E[((num | b) ^ (((num2 & b) >>> (int)b) / ~num)) - -624](_003C_003Ec__DisplayClass46_1.ev).TargetId, _671BC22C.BF9F3D1F.FE8E0C9E[((933719428 << (num2 << (int)b)) + -2079772624) ^ -1146052829](_003C_003Ec__DisplayClass46_1.ev).Cell);
						return;
					case 13u:
						num2 = (ushort)(1088792687 + (uint)(-1088803684 ^ num2) % (uint)(~num2));
						MoveActorIdTo(_671BC22C.BF9F3D1F.FE8E0C9E[559704661 + (num2 + -559724792)](_003C_003Ec__DisplayClass46_1.ev).TargetId, _671BC22C.BF9F3D1F.FE8E0C9E[(b >>> (0x6B975203 | ((num2 >>> (int)num) / 1335914257))) + 1483](_003C_003Ec__DisplayClass46_1.ev).EndCell);
						return;
					}
					defender3 = Defenders.Find(_003C_003Ec__DisplayClass46_1._003CHandleGameActionFightEvent_003Eb__4);
					if (((0x37A9413B & num2) ^ -1750953182 ^ num) << (int)(~num3 >> 10) != 0)
					{
						num2 = (ushort)(-43410480 ^ (-1322767081 % (int)(51584696 * ((3499164 % num3) ^ (uint)(num2 >> 29)))));
						continue;
					}
					goto IL_1a04;
					IL_1aea:
					num2 = (ushort)((1 & ((1 - (1736651710 >>> (int)num2)) / -316654966)) + 21618);
					MoveActorIdTo(_671BC22C.BF9F3D1F.FE8E0C9E[(int)((((uint)num2 > (uint)(b * 453236670)) ? 1u : 0u) >> 6) - -1075](_003C_003Ec__DisplayClass46_1.ev).TargetId, _671BC22C.BF9F3D1F.FE8E0C9E[-1059 ^ (~(b | num2) % 26)](_003C_003Ec__DisplayClass46_1.ev).TargetCellId);
					if ((uint)((b | -1676153820) * num2 >> (int)b) > (uint)(byte)num2)
					{
						num2 = (ushort)((ushort)((num2 >>> 17) * (num2 >> (num >> (int)b))) ^ 0x29B);
						continue;
					}
					goto IL_15f7;
					IL_16e7:
					num2 = (ushort)(0x2F4E ^ ((num & ((-1467743585 % ~num) | b)) >> (int)num2));
					if (defender3 != null)
					{
						num3 = (uint)b >> (int)((uint)(-980273784 << (int)num2) | ((uint)(num2 << 22) % (uint)(~num)));
						num2 = (ushort)((int)num3 / (int)num3 - -63267);
						continue;
					}
					goto IL_17c8;
					IL_19f9:
					num2 = (ushort)(num + 21618);
					goto IL_1a04;
					IL_181a:
					num2 = (ushort)(((int)num3 % ((-955804528 << (int)num3) / b) >> 8) - -21911);
					if (attacker2 != null)
					{
						b = (byte)((-400898803 << ((num2 * -164022979 == -692141378) ? 1 : 0)) | -(num2 % ~num2 >> (int)(num3 | num3)));
						if (~((1 > (int)(~num3)) ? 1u : 0u) >= num2)
						{
							num2 = (ushort)(0x106B6F5D ^ (0x106BDAFE | num2));
							continue;
						}
						goto IL_1c7a;
					}
					return;
					continue;
					end_IL_1643:
					break;
				}
				num2 = (ushort)(num3 - 2249989005u);
				goto IL_1898;
			}
		}
	}

	internal void HandleMapCurrentEvent(ByteString value)
	{
		IsLoadingMap = true;
		short num = -23234;
		if ((uint)((-1935880396 ^ num) >> ((-1491082720 < num) ? 1 : 0) >>> -323651137 * num) >= (uint)(num ^ -1852768611) / (uint)(num | num) << (int)(2159735192u % (uint)(75625633 + num << num % num)))
		{
			num = (short)(((num / (0x7405D5AE & num)) | num) ^ -28110);
			goto IL_0066;
		}
		goto IL_00f2;
		IL_008d:
		JitsuriMapCurrentEvent item = _018E928B._29B6D927<MapCurrentEvent, JitsuriMapCurrentEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[0xBFA56D ^ (num + 12582912)]()).map;
		if ((int)((uint)num % (uint)num >> (int)num) >> 30 == 0)
		{
			num = (short)(27303 + (-1564899169 >>> (int)num));
			goto IL_0066;
		}
		goto IL_00fe;
		IL_0066:
		sbyte b = default(sbyte);
		while (true)
		{
			switch ((uint)num % 4u)
			{
			case 1u:
				goto IL_00d9;
			case 2u:
				do
				{
					UpdateMapInformation();
				}
				while ((uint)(-113 / ~b) <= (uint)(b + 44));
				num = (short)((b | 0) - -22075);
				continue;
			case 3u:
				num = (short)((((2152454007u / (uint)(~(b & 0x2721BA37)) < (uint)num) ? 1u : 0u) ^ (uint)((num << (int)((uint)b / (uint)(~b))) | b)) - 45308);
				return;
			}
			break;
		}
		num = (short)((-2111397317 - num) ^ 0x7DD9CE11);
		goto IL_008d;
		IL_00d9:
		num = (short)(num * num - ((0u > (uint)(1150373255 >>> (int)num)) ? 1 : 0) + -745586259);
		goto IL_00f2;
		IL_00f2:
		MapInformation = new MapInformation(item);
		goto IL_00fe;
		IL_00fe:
		IsLoadingMap = (byte)((num ^ (byte)(-962491352 >> (int)num)) + 23103) != 0;
		b = (sbyte)((num - num) % 1194193333 >>> (int)num);
		if ((uint)(num / 1082231171) <= (uint)(-257811952 & num))
		{
			goto IL_0066;
		}
		goto IL_008d;
	}

	internal void HandleMapMovementEvent(ByteString value)
	{
		uint num = 4190027631u;
		if ((int)(~num) >= (int)num * (((int)num / -860431571 << (int)(num ^ 0x309A8613)) + (int)num))
		{
			num = (num >> (int)(num % num)) + 2774707985u;
			goto IL_003c;
		}
		goto IL_00d5;
		IL_008b:
		_003C_003Ec__DisplayClass48_0 _003C_003Ec__DisplayClass48_1 = new _003C_003Ec__DisplayClass48_0();
		num = ((num + (num >> 29)) & (uint)((int)num + (((int)num < (int)num) ? 1 : 0))) | 0xA6EB82E5u;
		num = 0x30DA5749 ^ (0 - ((byte)num ^ (num >> 18)));
		goto IL_003c;
		IL_003c:
		Attacker attacker = default(Attacker);
		short num3 = default(short);
		short num2 = default(short);
		Defender defender = default(Defender);
		int cellId = default(int);
		JitsuriMapMovementEvent item = default(JitsuriMapMovementEvent);
		while (true)
		{
			switch (num % 10)
			{
			case 1u:
				goto IL_00c2;
			case 2u:
				num = (((0x120 ^ ((int)num / 976673684)) == (int)num) ? 1u : 0u);
				goto IL_01bd;
			case 3u:
				num = (uint)(0x602E ^ ((int)num >> 14));
				attacker = Attackers.Find(_003C_003Ec__DisplayClass48_1._003CHandleMapMovementEvent_003Eb__0);
				if ((int)(num % 1543857557 >> (((int)num3 * (int)num) & -1197030985)) % -1054877785 == 0)
				{
					num = (uint)(((int)((uint)num3 ^ num) % -946255079 + 163) ^ 0x13AE185D);
					continue;
				}
				goto IL_0329;
			case 4u:
				num = (num | 0x21) ^ 0x13AE283B;
				if (attacker != null)
				{
					num *= (uint)((int)num + (num3 >>> (int)num3));
					num = 1799940096 + (0x35B43894 & num) - 4231681497u;
					continue;
				}
				goto IL_0332;
			case 5u:
				num = 4294967280u + ((uint)((int)num3 * (int)num) & (((3617173564u > 1873780002 - num) ? 1u : 0u) << (int)num3));
				goto IL_0329;
			case 6u:
				num = ((9 > num2) ? 1u : 0u);
				defender = Defenders.Find(_003C_003Ec__DisplayClass48_1._003CHandleMapMovementEvent_003Eb__1);
				if (num - 1637336837 != 0)
				{
					num = (uint)(-1139191278 % ((-594100701 & num2) * (-1876541779 ^ num3) + 10338453) - -1957933985);
					continue;
				}
				goto IL_0421;
			case 7u:
				num = (uint)(1789754931 + -(num2 ^ ((num3 ^ 0x6F24D5A2) + -74959472 % (int)num)));
				if (defender != null)
				{
					num3 = (short)(num2 & -1922039825);
					num = (uint)(num3 - 1496448180 - 1446031059);
					continue;
				}
				goto IL_0432;
			case 8u:
				num -= 1352508958;
				goto IL_0421;
			case 9u:
				{
					num = (uint)((((-1589504429 + 1907013636 / ~num2) ^ num3) >>> (int)num) + -24951695);
					UpdateMapInformation();
					return;
				}
				IL_0329:
				attacker.CellId = cellId;
				goto IL_0332;
				IL_0332:
				num2 = (short)(num + 151736741);
				if (-1170991590 % ~(num2 / -1215672914 << (int)(num >> -num3)) <= -(~((int)num3 + (((int)num2 < (int)num) ? 1 : 0))))
				{
					num = (uint)((int)(3501984524u % (uint)(num3 & num2)) * (int)num3 - 1474456664);
					continue;
				}
				goto IL_01bd;
				IL_0421:
				defender.CellId = cellId;
				num3 -= 8705;
				goto IL_0432;
				IL_0432:
				num2 = (short)(629415689 % ~(0 >>> -453345440 % num3));
				num = 4194061188u + (num | 0xC6227201u);
				continue;
				IL_01bd:
				do
				{
					cellId = _671BC22C.BF9F3D1F.FE8E0C9E[(num - (num | num) << (int)((0 - num) | (ushort)num) - -155722581) ^ 0xBF](item).Last();
					num3 = (short)((int)((uint)((int)num >> (int)num) ^ (num + 445919140)) >> (int)(num / (uint)(~((int)num % (int)(~num)))));
				}
				while ((int)(18 * num + 252432655 >> (num3 - -316180221) * (int)((uint)num3 / (uint)(1217530797 / num3))) >= -189255907 >>> (int)((uint)(num3 + 0) % 180116285u));
				num = (uint)(403351949 + ((num3 >>> (int)num >>> (int)num3 << 10) & -468392815));
				continue;
			}
			break;
		}
		num = 1410541874 + (((num & 0xB2255C10u) ^ 0x378ADA3D) | ((num == num) ? 1u : 0u));
		goto IL_008b;
		IL_00c2:
		num = (uint)(((int)(1879048192 - num) % (int)num) ^ 0x2E4B6F59);
		goto IL_00d5;
		IL_00d5:
		item = _018E928B._29B6D927<MapMovementEvent, JitsuriMapMovementEvent>(value, _671BC22C.BF9F3D1F.FE8E0C9E[-1772854443 % (int)((num % 2319089939u) ^ 0x6985723) - -1772855888]()).map;
		_003C_003Ec__DisplayClass48_1.actorId = _671BC22C.BF9F3D1F.FE8E0C9E[-146255427 * ((int)(~num) * -939290224 / -1740811517) - -727](item);
		num = (uint)((((int)num / 2073581721) & ((int)num >> 19)) % (int)(~(num % 1008876945)));
		if (((uint)((int)num >> -1660839531 * (-1902342243 * (int)num)) ^ (num >> (int)num)) == 3549668239u / ~(0x52A11CA2 & num))
		{
			num = (uint)(((-627655507 - (int)(num ^ 0xF1023125u)) | (1948182699 + ((0 - num > num) ? 1 : 0))) - 1893675895);
			goto IL_003c;
		}
		goto IL_008b;
	}

	[AsyncStateMachine(typeof(_003CPlaceFarFromMonster_003Ed__49))]
	internal Task PlaceFarFromMonster(List<int> possiblePositions)
	{
		_003CPlaceFarFromMonster_003Ed__49 stateMachine = default(_003CPlaceFarFromMonster_003Ed__49);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
		stateMachine._003C_003E4__this = this;
		stateMachine.possiblePositions = possiblePositions;
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
	}

	[AsyncStateMachine(typeof(_003CPlaceNearMonster_003Ed__50))]
	internal Task PlaceNearMonster(List<int> possiblePositions)
	{
		_003CPlaceNearMonster_003Ed__50 stateMachine = default(_003CPlaceNearMonster_003Ed__50);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
		stateMachine._003C_003E4__this = this;
		stateMachine.possiblePositions = possiblePositions;
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
	}

	internal Attacker? GetSelf()
	{
		return Attackers.Find((Attacker e) => e.ActorId == _instanceData.CharacterData.CharacterId);
	}

	[AsyncStateMachine(typeof(_003CPlaceWithFreeAdjacents_003Ed__52))]
	internal Task PlaceWithFreeAdjacents(List<int> possiblePositions)
	{
		_003CPlaceWithFreeAdjacents_003Ed__52 stateMachine = default(_003CPlaceWithFreeAdjacents_003Ed__52);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
		stateMachine._003C_003E4__this = this;
		stateMachine.possiblePositions = possiblePositions;
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
	}

	internal void Reset()
	{
		Attackers.Clear();
		Defenders.Clear();
		short num = -17920;
		if (((4100950631u / (uint)(~(num & (byte)num))) ^ ((uint)num % 2921382548u)) != 0)
		{
			goto IL_0034;
		}
		goto IL_01e5;
		IL_0034:
		while (true)
		{
			switch ((uint)num % 6u)
			{
			default:
				do
				{
					IsMyTurn = (byte)(-262221220 - num - -262203300) != 0;
					num = num;
				}
				while ((uint)(num / (num >>> (int)(2499955503u / (uint)num)) >>> num + (ushort)num) < (uint)(short)((num ^ num) | (num - num)));
				num = (short)(25250 + ~(((uint)num % 1856619055u > 1471492610) ? 1u : 0u));
				continue;
			case 1u:
				num = (short)(-10079 ^ num);
				do
				{
					IsFightingForTreasure = (byte)(-1030067018 - num - -1030049098) != 0;
				}
				while ((uint)(0x13A4A6A5 | num) <= (uint)num);
				num = (short)(~num - -12293);
				continue;
			case 2u:
				break;
			case 3u:
				goto end_IL_0034;
			case 4u:
				goto IL_01c7;
			case 5u:
				num = (short)(45754374 % (num & (2091931151 - num)) - 18534);
				return;
			}
			num = (short)((int)(~(1128174365u / (uint)num)) - -19422);
			TreasureTargetId = (int)(0x2E922B9C | ((uint)num % (uint)(num >> (int)num))) - (int)num - 780238165;
			num = (short)(num >> (int)num);
			if ((-num >>> 227658015 / num) % ((-1424066684 ^ num) | -561841264) >> (int)(3694141959u / (uint)num) != 0)
			{
				num = (short)((~num % num >> 9) - -2561);
				continue;
			}
			goto IL_01e5;
			IL_01c7:
			num = (short)((0x7D13E633 & (1010641571 % num * num - (num >> (int)num))) ^ -152265216);
			goto IL_01e5;
			continue;
			end_IL_0034:
			break;
		}
		num = (short)((((int)((uint)(num % 1596694547) / 3534345089u) / (-450798957 << (int)num) > -1720009128) ? 1 : 0) - 17921);
		goto IL_017f;
		IL_01e5:
		Round = (int)(2 ^ ((uint)num / 1627800207u));
		SpellIdOnCooldown = new Dictionary<int, int>();
		if (101165594 * ((uint)((num | num) >> (int)num) / 3659923263u) <= (uint)(-1808947954 << -num))
		{
			num = (short)(((-40583802 < num) ? 1 : 0) - -18850);
			goto IL_0034;
		}
		goto IL_017f;
		IL_017f:
		do
		{
			IsPlaced = (byte)((num ^ (-1 ^ num)) - -1) != 0;
		}
		while ((-1036670968 ^ num) == 0);
		num = (short)((num >>> num * num >>> ~(ushort)num >> (int)((uint)((num << (int)num) * -718299115) / (uint)num)) - -14907);
		goto IL_0034;
	}

	[AsyncStateMachine(typeof(_003CFightTurnFinishRequest_003Ed__55))]
	internal Task<bool> FightTurnFinishRequest()
	{
		byte b = 147;
		if (((uint)(-94 / b) & (((uint)(b * b) < (uint)b) ? 1u : 0u)) == 0)
		{
			b = (byte)(-2147483628 ^ -(b << (~b >>> (int)b)));
			goto IL_002b;
		}
		goto IL_009f;
		IL_009f:
		_003CFightTurnFinishRequest_003Ed__55 stateMachine = default(_003CFightTurnFinishRequest_003Ed__55);
		stateMachine._003C_003E1__state = -1 + (b - b) / -265544674;
		sbyte b2 = (sbyte)((uint)((-b >>> 25) % (int)((uint)(b - 1353792319) % (uint)b)) % (uint)(~(34 * (b >> 8))));
		b = (byte)((90402 << (548274111 >> (int)(short)b)) - 608174030);
		goto IL_002b;
		IL_002b:
		while (true)
		{
			switch ((uint)b % 4u)
			{
			default:
				b = (byte)(0x90 ^ (b - 17));
				stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
				goto IL_005b;
			case 1u:
				break;
			case 2u:
				b = (byte)(-(b2 << ((b2 & b2) >> (int)b2)) - -148);
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				if ((b2 | 0x71A53E15) != 0)
				{
					continue;
				}
				goto IL_005b;
			case 3u:
				{
					return stateMachine._003C_003Et__builder.Task;
				}
				IL_005b:
				do
				{
					stateMachine._003C_003E4__this = this;
				}
				while (b >> 2948 / b != 0);
				b = (byte)((b | (((b > b) ? 1u : 0u) >> (int)b)) + 4294967294u);
				continue;
			}
			break;
		}
		b = (byte)(1648597267 + ((b / b) ^ (0x1DBC6781 | (b << 27))));
		goto IL_009f;
	}

	internal async Task<bool> ForgeAndSendFightOptionToggleRequest(FightOption? option = null)
	{
		FightOptionToggleRequest fightOptionToggleRequest = _671BC22C.BF9F3D1F.FE8E0C9E[761]();
		if (option.HasValue)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1959](fightOptionToggleRequest, (int)option.Value);
		}
		Any any = _671BC22C.BF9F3D1F.FE8E0C9E[2101]();
		_5E33BC89._67B8C522(any, _messageHandler.F1A6D798[_79AC42A1.FightOptionToggleRequest]);
		_9DBE6F24._6C98D201(any, _671BC22C.BF9F3D1F.FE8E0C9E[1791](fightOptionToggleRequest));
		Any _83237E3A = any;
		_messageHandler._051BFF9F(_83237E3A);
		try
		{
			return true;
		}
		catch (TaskCanceledException ex)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[376]();
			_1E3B359C._6E3CC3BA(ex.ToString());
			return false;
		}
	}

	[AsyncStateMachine(typeof(_003CForgeAndSendChallengeReadyRequest_003Ed__57))]
	internal Task<bool> ForgeAndSendChallengeReadyRequest()
	{
		_003CForgeAndSendChallengeReadyRequest_003Ed__57 stateMachine = default(_003CForgeAndSendChallengeReadyRequest_003Ed__57);
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
		byte b = 0;
		stateMachine._003C_003E4__this = this;
		stateMachine._003C_003E1__state = -2142000558 + (0x7FAC55AD | b);
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		short num = (short)(~b | -1147802740);
		return stateMachine._003C_003Et__builder.Task;
	}

	[AsyncStateMachine(typeof(_003CForgeAndSendChallengeValidateRequest_003Ed__58))]
	internal Task<bool> ForgeAndSendChallengeValidateRequest(int challengeId)
	{
		int num = 849714780;
		_003CForgeAndSendChallengeValidateRequest_003Ed__58 stateMachine = default(_003CForgeAndSendChallengeValidateRequest_003Ed__58);
		int num2 = default(int);
		byte b = default(byte);
		while (true)
		{
			switch ((uint)num % 5u)
			{
			default:
				do
				{
					stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
					num2 = num | -550508404;
				}
				while ((uint)(((num + num2) & -568083153) / num >> (num >>> 25)) > (uint)(num2 >>> 13 >>> (num >> num)));
				num = (((uint)((0x7D2CF832 | num2) / num2) > (uint)(~(-num))) ? 1 : 0) - -1906578206;
				break;
			case 1u:
				num = 0x32A59E5C ^ ((int)((uint)(-189412844 >>> num) / 2894362779u) * num);
				goto IL_008f;
			case 2u:
				num = 849714780 + -1895160415 * (b * num2 >>> -267454804 / ~b);
				goto IL_0114;
			case 3u:
				num = (0x75 | b) + -116;
				stateMachine._003C_003E1__state = (0x2A99AABD | b) + -714713790;
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				b = (byte)((uint)(-b << (int)((uint)num / (uint)num2 >> 16)) * ((uint)(1989885609 * num) / (uint)(-633602428 + num2) >> num));
				if ((int)((uint)(1388787807 << (int)b) | ((uint)(b << num) / ~((1964451864 < num) ? 1u : 0u))) > (num | b) + num)
				{
					num = (int)(~((965151258u % (uint)(~b)) ^ 0x772F0821) << 16) + -1144035435;
					break;
				}
				goto IL_0114;
			case 4u:
				{
					num = ((0x2798A710 | num) >>> num >> (int)((uint)num % 1822790154u) >> 31) - -1;
					return stateMachine._003C_003Et__builder.Task;
				}
				IL_008f:
				stateMachine._003C_003E4__this = this;
				b = (byte)((((uint)num % 3913536854u == (uint)num) ? 1u : 0u) << (num2 / num2 - (int)(866608431u % (uint)num) >>> num2));
				if ((int)((uint)(num / (short)num >>> ((num2 < num) ? 1 : 0)) / (uint)(num / 1)) >= num2 + (num2 ^ 0x73BBFEA0) + ((num == ((~num2 == 110625826) ? 1 : 0)) ? 1 : 0))
				{
					num = num % 1268230190 - -341968597;
					break;
				}
				goto default;
				IL_0114:
				stateMachine.challengeId = challengeId;
				num = (int)((uint)(num2 - (b | 0x6EA6681C) >> (int)(short)(3543584063u % (uint)(~b))) / (uint)((num2 << 13) | b));
				if ((uint)num2 >> 17 != 0)
				{
					num = (int)(1u % (uint)(~(ushort)(num2 - num2)) - 921223264);
					break;
				}
				goto IL_008f;
			}
		}
	}

	[AsyncStateMachine(typeof(_003CForgeAndSendFightPlacementPositionRequest_003Ed__59))]
	internal Task<bool> ForgeAndSendFightPlacementPositionRequest(long actorId, int cellId)
	{
		_003CForgeAndSendFightPlacementPositionRequest_003Ed__59 stateMachine = default(_003CForgeAndSendFightPlacementPositionRequest_003Ed__59);
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
		stateMachine._003C_003E4__this = this;
		uint num;
		do
		{
			stateMachine.actorId = actorId;
			sbyte b = 0;
			stateMachine.cellId = cellId;
			stateMachine._003C_003E1__state = (b << (int)b) * (int)(2821554832u % (uint)(992974339 / ~b >> (int)b)) - 1;
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			num = ~((-1 > 1603733664 >> (int)b) ? 1u : 0u);
		}
		while ((byte)num * (num | num) >> (int)(num * (num & 0xC6AB42A0u) / (num >> 7)) == 0);
		return stateMachine._003C_003Et__builder.Task;
	}

	[AsyncStateMachine(typeof(_003CForgeAndSendFightReadyRequest_003Ed__60))]
	internal Task<bool> ForgeAndSendFightReadyRequest()
	{
		ushort num = 0;
		if ((uint)((((uint)num > 3467430559u) ? 1 : 0) % ((num + 1536929455 >>> 14) - 537002770)) >= (uint)(0x1968 & num))
		{
			goto IL_0027;
		}
		goto IL_009f;
		IL_0027:
		ushort num2 = default(ushort);
		_003CForgeAndSendFightReadyRequest_003Ed__60 stateMachine = default(_003CForgeAndSendFightReadyRequest_003Ed__60);
		while (true)
		{
			switch ((uint)num % 3u)
			{
			default:
				goto IL_003b;
			case 1u:
				break;
			case 2u:
				num = (ushort)((0x43048303 | num) & (14483 / ~(byte)num2 % (-13858 / (num2 ^ num))));
				return stateMachine._003C_003Et__builder.Task;
			}
			break;
			IL_003b:
			stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
			stateMachine._003C_003E4__this = this;
			num2 = (ushort)(~(~num));
			num = (ushort)(num2 - 235286669 - -235345092);
		}
		num = (ushort)(((((0x270AFB94 | num) - 1610612736) | (num2 ^ 0x663D40B3)) / ~(833956201 % (((uint)num < 3391174078u) ? 1 : 0))) ^ 0x18C00049);
		goto IL_0095;
		IL_009f:
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		if (1394750341 + num2 != 0)
		{
			num += 7817;
			goto IL_0027;
		}
		goto IL_0095;
		IL_0095:
		stateMachine._003C_003E1__state = ~num2;
		goto IL_009f;
	}

	[AsyncStateMachine(typeof(_003CForgeAndSendFightTurnReadyRequest_003Ed__61))]
	internal Task<bool> ForgeAndSendFightTurnReadyRequest()
	{
		_003CForgeAndSendFightTurnReadyRequest_003Ed__61 stateMachine = default(_003CForgeAndSendFightTurnReadyRequest_003Ed__61);
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
		stateMachine._003C_003E4__this = this;
		uint num = 4294967294u;
		num = 0 - num * 1549773995 + 3210051258u;
		while (true)
		{
			switch (num % 3)
			{
			case 1u:
				num = (num ^ 0x771FBE0B) - 1721227160;
				do
				{
					stateMachine._003C_003Et__builder.Start(ref stateMachine);
				}
				while ((uint)((int)num * -75911225) > (uint)((-705698836 << (1335073322 / (int)num << (int)num)) - (-1448388290 >> (short)num / (-1439133170 + (int)num))));
				continue;
			case 2u:
				return stateMachine._003C_003Et__builder.Task;
			}
			num = (uint)(-2014631955 + ((int)num / 1812663336 + (int)num));
			do
			{
				stateMachine._003C_003E1__state = (int)((1032132372 / num) ^ ((0 - num) & num)) ^ -3;
			}
			while ((uint)((int)num - (((int)num < (int)num) ? 1 : 0) - 413) % (0 - num) == 0);
			num = (uint)((short)(((int)(num * num) + (int)num % -1758696263) ^ (int)num) + 294152609);
		}
	}

	[AsyncStateMachine(typeof(_003CForgeAndSendGameActionFightCastRequest_003Ed__63))]
	internal Task<bool> ForgeAndSendGameActionFightCastRequest(int cellId, int spellId)
	{
		_003CForgeAndSendGameActionFightCastRequest_003Ed__63 stateMachine = default(_003CForgeAndSendGameActionFightCastRequest_003Ed__63);
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
		stateMachine._003C_003E4__this = this;
		stateMachine.cellId = cellId;
		stateMachine.spellId = spellId;
		int num = 1924805014;
		if (1518374925u / (uint)(-num) == 0)
		{
			num = (-123970374 & (num ^ (-1279571066 + (num << num)))) - 2018268163;
			ushort num2 = default(ushort);
			while (true)
			{
				switch ((uint)num % 3u)
				{
				default:
					num = -2104609381 ^ num;
					goto IL_006e;
				case 1u:
					stateMachine._003C_003Et__builder.Start(ref stateMachine);
					num += 1248340802;
					if ((uint)num / uint.MaxValue % ((uint)(num2 >> num) / 1427941664u - 793339998) <= (((1026699067u % (uint)num) & 0x87EF1B6Bu) << num2 % (num2 | num2)) / 2463125176u)
					{
						num = ((2049881371 - num) * (num << 31) * (int)((uint)num / (uint)(num << num)) << (int)((uint)num2 / (uint)(-num2))) - -1308984125;
						continue;
					}
					goto IL_006e;
				case 2u:
					break;
					IL_006e:
					stateMachine._003C_003E1__state = (-1192353441 + (num >> num)) ^ 0x4711DCD6;
					num2 = (ushort)(num * (int)(1375804092u / (uint)num - 1553426590) % (num - (0x4331590F | (-1892201038 << num))));
					continue;
				}
				break;
			}
			num = -1121821480 + (int)(byte)(0xA & num2) / ((1327894402u > (uint)(num2 / 923188282 % num2)) ? 1 : 0);
		}
		return stateMachine._003C_003Et__builder.Task;
	}

	[AsyncStateMachine(typeof(_003CForgeAndSendSurrend_003Ed__64))]
	internal Task<bool> ForgeAndSendSurrend()
	{
		byte b = 238;
		b = (byte)(5 + (-1 << ((-2017635232 > (0x2E1375A5 & (b | b))) ? 1 : 0)));
		byte b2 = default(byte);
		_003CForgeAndSendSurrend_003Ed__64 stateMachine = default(_003CForgeAndSendSurrend_003Ed__64);
		while (true)
		{
			switch ((uint)b % 4u)
			{
			default:
				b = (byte)(((b == b) ? 1u : 0u) ^ 0xEFu);
				goto IL_0043;
			case 1u:
			{
				b = (byte)(238 + ((b2 & 0x35661400) << (-407874537 & b2) - b) % ~(sbyte)((uint)b2 % (uint)b2));
				stateMachine._003C_003E1__state = (int)((uint)b % (uint)b / ~(b2 / (b - (uint)b2 / (uint)b2))) ^ -1;
				sbyte b3 = (sbyte)((b2 & 0x473C0F0A) / ((b > (0x10 | (b ^ -326086614))) ? 1 : 0));
				if ((uint)b3 % (uint)(-1711218531 - (0x194954FE | b2)) <= (uint)b3)
				{
					break;
				}
				goto IL_0043;
			}
			case 2u:
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				b2 = (byte)(-2018278123 * b2);
				if (((b * (b2 >>> (int)b) > 42) ? 1u : 0u) / 2344407087u == 0)
				{
					b = (byte)(b2 - b - -299);
					break;
				}
				goto IL_0043;
			case 3u:
				{
					b = (byte)(238 + (uint)((b2 & -1313350213) / b) % 1260297760u);
					return stateMachine._003C_003Et__builder.Task;
				}
				IL_0043:
				stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
				stateMachine._003C_003E4__this = this;
				b2 = (byte)(-b);
				if (824936759 / (int)(~((1 == -400548958 << (b2 ^ b2)) ? 1u : 0u)) <= (int)((uint)((-114784591 | b) * 2067087360) / (uint)b2))
				{
					b = (byte)(-21160143 ^ (0x36A906BC | (-633595478 - b2)));
					break;
				}
				goto case 2u;
			}
		}
	}

	internal void HandleMessage(GameMessage message)
	{
		if (_671BC22C.BF9F3D1F.FE8E0C9E[1647](message) == GameMessage.ContentOneofCase.Event && _671BC22C.BF9F3D1F.FE8E0C9E[160](message) != null)
		{
			ushort num = 0;
			if (_671BC22C.BF9F3D1F.FE8E0C9E[((0 - ((num == num) ? 1 : 0)) % ~(byte)num << (int)num) - -1383](_671BC22C.BF9F3D1F.FE8E0C9E[((short)(-228187803 + ~num) << (int)num) - 8388](message)) != null)
			{
				short num2 = (short)(((num * num) ^ -1683681055) & 0x637269F);
				HandleEvent(_671BC22C.BF9F3D1F.FE8E0C9E[(-(-200900962 ^ (num / 1177191306)) << (int)num2) - 401801764](message));
			}
		}
	}

	internal bool IsValidPositionForTreasure(int cellId)
	{
		List<int> adjacentsCellId = _instanceData.GetAdjacentsCellId(cellId, getDiagonals: false);
		ushort num = 0;
		if ((2528738838u % (uint)(~(num | num)) << 1 << (int)((uint)(0x58F4BED8 & num) / (uint)(~num)) != 0 && adjacentsCellId.Count < -1426637619 + (num | 0x5508C337)) || num * 0 != 0)
		{
			return (byte)(-(num >> 30)) != 0;
		}
		return adjacentsCellId.All(delegate(int cellId2)
		{
			_003C_003Ec__DisplayClass66_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass66_0();
			CS_0024_003C_003E8__locals4.cellId = cellId2;
			Cell cell = MapInformation.Cells[CS_0024_003C_003E8__locals4.cellId];
			Attacker attacker = Attackers.Find((Attacker attacker2) => attacker2.CellId == CS_0024_003C_003E8__locals4.cellId);
			Defender defender = Defenders.Find((Defender defender2) => defender2.CellId == CS_0024_003C_003E8__locals4.cellId);
			return cell.Los != 0 && cell.Visible != 0 && cell.Mov == 1 && attacker == null && defender == null;
		});
	}

	private void Log(object message, string category, D32842AE._543E5320 level)
	{
		string aEB611B = ((message is string text) ? text : _671BC22C.BF9F3D1F.FE8E0C9E[1563](message));
		D32842AE._0700BABF(category, aEB611B, _instanceData.ProcessId, level);
	}

	static FighterManager()
	{
		JsonSerializerSettings jsonSerializerSettings = _671BC22C.BF9F3D1F.FE8E0C9E[992]();
		_09B897AF._14971E95(jsonSerializerSettings, NullValueHandling.Ignore);
		_229D9712._099A1120(jsonSerializerSettings, DefaultValueHandling.Ignore);
		D8AFB18D._94A3AB14(jsonSerializerSettings, ReferenceLoopHandling.Ignore);
		_35BD43A5._64BBAA97(jsonSerializerSettings, Formatting.None);
		JsonLogSettings = jsonSerializerSettings;
		_verboseIncludeTimestamp = false;
	}
}
