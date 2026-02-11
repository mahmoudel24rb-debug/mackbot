// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Fight.Fighter
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DofusLibrary.Common;
using DofusLibrary.Common.Fight;
using DofusLibrary.Common.Fight.DofusLibrary.Common.Fight;
using DofusLibrary.Common.JsonClasses;
using DofusLibrary.Common.Map;
using DofusLibrary.Common.PathFinder;
using DofusLibrary.Common.Repository;
using JitsuriProto;

internal class Fighter
{
	internal static class InvisibilityStates
	{
		internal const int INVISIBLE = 1;

		internal const int DETECTED = 2;
	}

	internal class TackleCost
	{
		[CompilerGenerated]
		private int _003CAp_003Ek__BackingField;

		internal int Mp { get; set; }

		internal int Ap
		{
			[CompilerGenerated]
			get
			{
				ushort num = 17460;
				return _003CAp_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CAp_003Ek__BackingField = value;
			}
		}

		internal TackleCost(int mp, int ap)
		{
			uint num = uint.MaxValue;
			while (true)
			{
				switch (num % 3)
				{
				default:
					_671BC22C.BF9F3D1F.FE8E0C9E[2099 + (int)num % (int)(num >> (int)((num ^ num) >> -1474023419 % (int)num))](this);
					num = ~(0xD40F7BA7u ^ ((num | 0xF09C08AFu) + 378997020));
					if (num + 1312675755 > ((uint)(-((int)num / (int)num >> (int)num)) & (num / (uint)(1638269193 + 1695632945 % (int)num))))
					{
						num = (uint)((int)(2503805219u / (num + num)) % (int)(num - 521467948)) / ~(604176258 / num % (num + num)) - 3966110442u;
						break;
					}
					goto IL_00b7;
				case 1u:
					num = num * 424507206 + 1925402431;
					Mp = mp;
					goto IL_00b7;
				case 2u:
					{
						num = (uint)(1396014327 + ((int)num % -745316214 + ((int)num % (int)num + 104546591)));
						return;
					}
					IL_00b7:
					Ap = ap;
					if ((num ^ 0xFF15) != 0)
					{
						num = (uint)((((int)num + -1264612933) % 689845936 >>> (2014738093 >>> (int)num)) + -1215682521);
						break;
					}
					goto default;
				}
			}
		}
	}

	internal class PathNode
	{
		[CompilerGenerated]
		private readonly int _003CCellId_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CTackleMp_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CDistance_003Ek__BackingField;

		internal int CellId
		{
			[CompilerGenerated]
			get
			{
				uint num = 0u;
				return _003CCellId_003Ek__BackingField;
			}
		}

		internal int AvailableMp { get; }

		internal int AvailableAp { get; }

		internal int TackleMp
		{
			[CompilerGenerated]
			get
			{
				ushort num = 0;
				return _003CTackleMp_003Ek__BackingField;
			}
		}

		internal int TackleAp { get; }

		internal int Distance
		{
			[CompilerGenerated]
			get
			{
				uint num = 1047107412u;
				return _003CDistance_003Ek__BackingField;
			}
		}

		internal PathNode(int cellId, int availableMp, int availableAp, int tackleMp, int tackleAp, int distance)
		{
			uint num = 90u;
			num = 2552427560u + (uint)((int)num / (int)num % (int)(~(num / 1235759399))) % (uint)(87857921 << ((num < num) ? 1 : 0));
			Unsafe.SkipInit(out uint num3);
			Unsafe.SkipInit(out short num2);
			while (true)
			{
				switch (num % 4)
				{
				default:
					num = (uint)((0 % (int)(310498457 + (num ^ num))) ^ 0x5A);
					_671BC22C.BF9F3D1F.FE8E0C9E[(int)(0 - (0 - num)) - -2009](this);
					num3 = (uint)(((-1440799849 << (int)num) - -346054914 - -979866838) / ~((int)num / (int)(55868 - ~num)));
					num = (uint)(-1324834904 * (int)num + 620598385);
					break;
				case 1u:
					num = (uint)((byte)(0 >> ((int)num * -2070904285 << (int)num3)) - -90);
					CellId = cellId;
					AvailableMp = availableMp;
					num2 = (short)(((((int)num3 % 799036705) & -1984366811) >>> (int)num + -659840756) * (int)num);
					if ((int)(num3 + 1554163081) < num2 * 1)
					{
						break;
					}
					return;
				case 2u:
					AvailableAp = availableAp;
					TackleMp = tackleMp;
					TackleAp = tackleAp;
					num3 = num3 << (int)num2 << (int)(num / num);
					num = (uint)((int)(~num) / (int)num * (858395190 << 461789863 % (int)num) - ((int)(70106650 % num) >> (1932231836 >>> (int)num2)) - -1741859);
					break;
				case 3u:
					num = (uint)((((((uint)num2 < 1579397663u) ? 1 : 0) == (int)num3 / (int)num - ((int)num2 - (int)num)) ? 1 : 0) - (int)((num3 - num3) * num3) % ~(num2 >> 29) + 90);
					Distance = distance;
					return;
				}
			}
		}
	}

	internal class MoveNode
	{
		[CompilerGenerated]
		private readonly int _003CAp_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CMp_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CFrom_003Ek__BackingField;

		[CompilerGenerated]
		private readonly bool _003CReachable_003Ek__BackingField;

		[CompilerGenerated]
		private List<int> _003CPath_003Ek__BackingField;

		internal int Ap
		{
			[CompilerGenerated]
			get
			{
				ushort num = 48369;
				return _003CAp_003Ek__BackingField;
			}
		}

		internal int Mp
		{
			[CompilerGenerated]
			get
			{
				ushort num = 40000;
				return _003CMp_003Ek__BackingField;
			}
		}

		internal int From
		{
			[CompilerGenerated]
			get
			{
				byte b = 0;
				return _003CFrom_003Ek__BackingField;
			}
		}

		internal bool Reachable
		{
			[CompilerGenerated]
			get
			{
				byte b = 28;
				return _003CReachable_003Ek__BackingField;
			}
		}

		internal List<int> Path
		{
			[CompilerGenerated]
			get
			{
				byte b = 117;
				return _003CPath_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPath_003Ek__BackingField = value;
			}
		}

		internal MoveNode(TackleCost tackleCost, int from, bool reachable)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
			uint num = 405194605u;
			if ((num ^ (uint)((int)num % -810495339 * (int)(num >> (int)num / -224474068))) == 0)
			{
				return;
			}
			Unsafe.SkipInit(out sbyte b);
			while (true)
			{
				switch (num % 5)
				{
				default:
					Ap = tackleCost.Ap;
					if ((int)(num >> 0) % (int)(0xDE8C221Fu ^ (num ^ num)) != (int)(num % 1772026670) % -1106697936 * ((0 - ((1528633230 > num) ? 1 : 0) > (int)num) ? 1 : 0))
					{
						num = (uint)(((int)num >> 31) - -771097437) % num - 3474494047u;
						break;
					}
					goto IL_01b6;
				case 1u:
					num = 108600585 + (num >> (int)(short)((int)num >> 29));
					Mp = tackleCost.Mp;
					b = (sbyte)((int)num & -970813169);
					if ((0xA6ADEB83u | num) * 2022170888 != 0)
					{
						num = (uint)(~(-1251922598 * b >> -668085699 - b) + 523671960);
						break;
					}
					goto IL_01b6;
				case 2u:
					num = 2912811155u / (3526606485u % num * 58798115) - 3889772693u;
					do
					{
						From = from;
					}
					while ((((int)num + -1226133512) ^ -419242689) == 0);
					num = (uint)((int)((uint)(b - 319746974) / ~((uint)b / 1866172676u)) % -2 + 109621263);
					break;
				case 3u:
					num = 2133217832u % (uint)((int)num % 1915960610) + 354780770;
					Reachable = reachable;
					if (~(num % 3367711167u) != (uint)(-725766622 * b))
					{
						num = (num | 0xD70F418Cu) ^ 0xAB3212D0u;
						break;
					}
					return;
				case 4u:
					{
						num = (uint)(-1205418131 ^ ((int)b % (int)num % 1963038911 << 23 << -1582717686 * (b * -1818565581)));
						goto IL_01b6;
					}
					IL_01b6:
					Path = null;
					return;
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass231_0
	{
		public FighterManager.Attacker mineChar;

		public _003C_003Ec__DisplayClass231_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetAlliesInZone_003Eb__0(FighterManager.ActorFighter f)
		{
			return f.ActorId == mineChar.ActorId;
		}

		internal bool _003CGetAlliesInZone_003Eb__1(FighterManager.ActorFighter f)
		{
			return f.ActorId == mineChar.ActorId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass233_0
	{
		public List<int> zone;

		public _003C_003Ec__DisplayClass233_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetFightersFromTeamInZone_003Eb__0(FighterManager.Defender f)
		{
			if (zone.Contains(f.CellId))
			{
				return f.IsAlive;
			}
			return false;
		}

		internal bool _003CGetFightersFromTeamInZone_003Eb__1(FighterManager.Attacker f)
		{
			if (zone.Contains(f.CellId))
			{
				return f.IsAlive;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass251_0
	{
		public _9F8EAF35 pt;

		public _003C_003Ec__DisplayClass251_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal int _003CGetFarthestReachableCellFromMonsters_003Eb__0(FighterManager.Defender m)
		{
			return pt.FB2C1732(new _9F8EAF35(m.CellId));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass252_0
	{
		public int minRange;

		public int maxRange;

		public Func<int, bool> _003C_003E9__2;

		public _003C_003Ec__DisplayClass252_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetSafestReachableCellWithinRangeOfMonsters_003Eb__2(int d)
		{
			if (d >= minRange)
			{
				return d <= maxRange;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass252_1
	{
		public _9F8EAF35 pt;

		public _003C_003Ec__DisplayClass252_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal int _003CGetSafestReachableCellWithinRangeOfMonsters_003Eb__1(FighterManager.ActorFighter m)
		{
			return pt.FB2C1732(new _9F8EAF35(m.CellId));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass256_0
	{
		public List<FighterManager.ActorFighter> monsters;

		public int minDistance;

		public int maxDistance;

		public _003C_003Ec__DisplayClass256_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CMoveWithinRangeOfAnyMonster_003Eb__3(int d)
		{
			if (d >= minDistance)
			{
				return d <= maxDistance;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass256_1
	{
		public _9F8EAF35 pt;

		public _003C_003Ec__DisplayClass256_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal int _003CMoveWithinRangeOfAnyMonster_003Eb__2(FighterManager.ActorFighter m)
		{
			return pt.FB2C1732(new _9F8EAF35(m.CellId));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass288_0
	{
		public List<int> adjacents;

		public _003C_003Ec__DisplayClass288_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetReachableZone_003Eb__1(FighterManager.Defender defender)
		{
			return adjacents.Contains(defender.CellId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass288_1
	{
		public int cellId;

		public _003C_003Ec__DisplayClass288_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetReachableZone_003Eb__2(FighterManager.Defender defender)
		{
			return defender.CellId == cellId;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CDoAction_003Ed__244 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public Fighter _003C_003E4__this;

		private void MoveNext()
		{
			Fighter fighter = _003C_003E4__this;
			bool result;
			try
			{
				try
				{
					FighterManager.Attacker self = fighter.FighterManager.GetSelf();
					if (fighter.OurTurn)
					{
					}
					result = true;
				}
				catch
				{
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
	private struct _003CDoTurn_003Ed__260 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public Fighter _003C_003E4__this;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			Fighter fighter = _003C_003E4__this;
			bool result;
			try
			{
				TaskAwaiter<bool> awaiter;
				if (num == 0 || num != 1)
				{
					try
					{
						if (num != 0)
						{
							goto IL_001c;
						}
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0070;
						IL_0070:
						if (awaiter.GetResult())
						{
							goto IL_001c;
						}
						goto end_IL_0019;
						IL_001c:
						awaiter = fighter.DoAction().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0070;
						end_IL_0019:;
					}
					catch
					{
					}
					awaiter = fighter.FighterManager.FightTurnFinishRequest().GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (_003C_003E1__state = 1);
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
				result = awaiter.GetResult();
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
	private struct _003CMoveCloserTo_003Ed__258 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public Fighter _003C_003E4__this;

		public int minimalMovementPoint;

		public int targetCellId;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			Fighter fighter = _003C_003E4__this;
			bool result;
			try
			{
				if (num == 0)
				{
					goto IL_0161;
				}
				FighterManager.Attacker self = fighter.FighterManager.GetSelf();
				List<int> _47A7463D = default(List<int>);
				if (self == null)
				{
					result = false;
				}
				else
				{
					minimalMovementPoint = _671BC22C.BF9F3D1F.FE8E0C9E[325](minimalMovementPoint, self.Characteristics[CharacteristicKeyword.MovementPoints].Value);
					if (minimalMovementPoint <= 0)
					{
						result = true;
					}
					else
					{
						Dictionary<int, bool> dictionary = fighter.BuildOccupiedCells(self.CellId);
						List<int> path = JsPathFinder.GetPath(self.CellId, targetCellId, fighter.FighterManager.MapInformation, dictionary, allowDiagonals: false);
						_9F8EAF35 _9F8EAF36 = _9F8EAF35.D6056589((uint)targetCellId);
						_9F8EAF35 fF9851AF = _9F8EAF35.D6056589((uint)self.CellId);
						if (_9F8EAF36.FB2C1732(fF9851AF) == 1 && dictionary.ContainsKey(targetCellId))
						{
							result = true;
						}
						else
						{
							if (_9F8EAF36.FB2C1732(fF9851AF) != 0)
							{
								List<int> list = path.Take(minimalMovementPoint + 1).ToList();
								if (list.Count > 0 && list.Last() == targetCellId && dictionary.ContainsKey(targetCellId))
								{
									list.RemoveAt(list.Count - 1);
								}
								_47A7463D = JsPathFinder.CompressPath(list);
								goto IL_0161;
							}
							result = true;
						}
					}
				}
				goto end_IL_000e;
				IL_0161:
				try
				{
					TaskAwaiter<bool> awaiter;
					if (num != 0)
					{
						awaiter = fighter.InstanceData.MessageHandler._060DCDB8(_47A7463D, (int)fighter.FighterManager.MapInformation.MapId, 1).GetAwaiter();
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
					result = awaiter.GetResult();
				}
				catch (Exception ex)
				{
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

	[CompilerGenerated]
	private InstanceData _003CInstanceData_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CSpells_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CSpellsToUse_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CEnabled_003Ek__BackingField;

	[CompilerGenerated]
	private string _003CSpeedMode_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CCastSpellMin_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CCastSpellMax_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CActionMax_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CEnterFightMin_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CEnterFightMax_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CAutoSkipTurn_003Ek__BackingField;

	[CompilerGenerated]
	private string _003CFightMode_003Ek__BackingField;

	[CompilerGenerated]
	private int? _003CTeamId_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CPlacementMoveTimeout_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CPlacementActiveTimeout_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CPlacementReadyTimeout_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CPositions_003Ek__BackingField;

	[CompilerGenerated]
	private object _003CJoinFightMap_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CJoinFightTimeout_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CTurn_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003COurTurn_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CIsLeader_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CKickUnwantedPlayers_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CBoostRatio_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CSummonRatio_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CKillRatio_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CHealRatio_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CAllowBoostSelf_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CAllowBoostSummons_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CAllowBoostAllies_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CAllowHealSelf_003Ek__BackingField;

	[CompilerGenerated]
	private bool _003CAllowHealAllies_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CEarthEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CAirEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CFireEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CNeutralEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CPushEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CDamageEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CHealEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CSummonEffects_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CCloseSummons_003Ek__BackingField;

	[CompilerGenerated]
	private List<int> _003CBoostEffects_003Ek__BackingField;

	private static Dictionary<int, string> EffectShapes = new Dictionary<int, string>
	{
		{ 88, "X" },
		{ 76, "L" },
		{ 84, "T" },
		{ 80, "P" },
		{ 68, "D" },
		{ 67, "C" },
		{ 79, "O" },
		{ 81, "Q" },
		{ 86, "V" },
		{ 87, "W" },
		{ 43, "+" },
		{ 35, "#" },
		{ 42, "*" },
		{ 47, "/" },
		{ 45, "-" },
		{ 71, "G" },
		{ 73, "I" },
		{ 85, "U" },
		{ 65, "A" }
	};

	internal const int ROOTED = 6;

	internal const int UNTACKLER = 95;

	internal const int UNTACKLABLE = 96;

	internal FighterManager FighterManager { get; set; }

	internal InstanceData InstanceData
	{
		[CompilerGenerated]
		get
		{
			short num = 14396;
			return _003CInstanceData_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 64;
			if ((uint)(-b >> 15) / (uint)(b ^ -160649806) - b != (uint)((-831850512 | (b | -1472610628)) * (857587577 >>> b % 1441596358)))
			{
				_003CInstanceData_003Ek__BackingField = value;
			}
		}
	}

	internal List<int> Spells
	{
		[CompilerGenerated]
		get
		{
			return _003CSpells_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = -6475;
			do
			{
				_003CSpells_003Ek__BackingField = value;
			}
			while ((num | -460123479) == (int)((uint)(~(num - 1706345017)) / (236454278u % (uint)num)));
		}
	}

	internal List<int> SpellsToUse
	{
		[CompilerGenerated]
		get
		{
			short num = 0;
			return _003CSpellsToUse_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 15351;
			_003CSpellsToUse_003Ek__BackingField = value;
		}
	}

	internal List<JitsuriSpellItem> SpellsUsable
	{
		get
		{
			sbyte b = 7;
			return InstanceData.CharacterData.Spells;
		}
	}

	internal bool Enabled
	{
		[CompilerGenerated]
		get
		{
			ushort num = 15701;
			return _003CEnabled_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 1728409361;
			_003CEnabled_003Ek__BackingField = value;
		}
	}

	internal string SpeedMode
	{
		[CompilerGenerated]
		get
		{
			ushort num = 2;
			return _003CSpeedMode_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CSpeedMode_003Ek__BackingField = value;
		}
	}

	internal int FinishTurnMin { get; set; }

	internal int FinishTurnMax { get; set; }

	internal int CastSpellMin
	{
		[CompilerGenerated]
		get
		{
			byte b = 0;
			return _003CCastSpellMin_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 19977;
			_003CCastSpellMin_003Ek__BackingField = value;
		}
	}

	internal int CastSpellMax
	{
		[CompilerGenerated]
		get
		{
			return _003CCastSpellMax_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = -29873;
			_003CCastSpellMax_003Ek__BackingField = value;
		}
	}

	internal int ActionMin { get; set; }

	internal int ActionMax
	{
		[CompilerGenerated]
		get
		{
			byte b = 46;
			return _003CActionMax_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 90;
			_003CActionMax_003Ek__BackingField = value;
		}
	}

	internal int EnterFightMin
	{
		[CompilerGenerated]
		get
		{
			short num = -30812;
			return _003CEnterFightMin_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 0;
			do
			{
				_003CEnterFightMin_003Ek__BackingField = value;
			}
			while ((0xAB9759ABu ^ ((uint)(num * num) % (uint)(~num))) - (uint)(num >>> num) % (uint)(~num) == 0);
		}
	}

	internal int EnterFightMax
	{
		[CompilerGenerated]
		get
		{
			ushort num = 72;
			return _003CEnterFightMax_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 3321012381u;
			_003CEnterFightMax_003Ek__BackingField = value;
		}
	}

	internal bool AutoLock { get; set; }

	internal bool AutoSkipTurn
	{
		[CompilerGenerated]
		get
		{
			return _003CAutoSkipTurn_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 0;
			if ((uint)(2100351287 >> (int)b) / ~((uint)(1628564749 % ~b >> 6) % (uint)(~b)) == 0)
			{
				do
				{
					_003CAutoSkipTurn_003Ek__BackingField = value;
				}
				while ((b ^ -2) == 0);
			}
		}
	}

	internal string FightMode
	{
		[CompilerGenerated]
		get
		{
			int num = 0;
			return _003CFightMode_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CFightMode_003Ek__BackingField = value;
		}
	}

	internal int ChickenDistance { get; set; }

	internal int? TeamId
	{
		[CompilerGenerated]
		get
		{
			sbyte b = -119;
			return _003CTeamId_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 0;
			if (~((-1464281434 ^ num) >>> ~num) != 0)
			{
				_003CTeamId_003Ek__BackingField = value;
			}
		}
	}

	internal bool IsReady { get; set; }

	internal int PlacementMoveTimeout
	{
		[CompilerGenerated]
		get
		{
			ushort num = 0;
			return _003CPlacementMoveTimeout_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 1079926566u;
			_003CPlacementMoveTimeout_003Ek__BackingField = value;
		}
	}

	internal int PlacementActiveTimeout
	{
		[CompilerGenerated]
		get
		{
			byte b = 1;
			return _003CPlacementActiveTimeout_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = -43;
			_003CPlacementActiveTimeout_003Ek__BackingField = value;
		}
	}

	internal int PlacementReadyTimeout
	{
		[CompilerGenerated]
		get
		{
			int num = -1055151812;
			return _003CPlacementReadyTimeout_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			short num = -31570;
			_003CPlacementReadyTimeout_003Ek__BackingField = value;
		}
	}

	internal List<int> Positions
	{
		[CompilerGenerated]
		get
		{
			sbyte b = 0;
			return _003CPositions_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CPositions_003Ek__BackingField = value;
		}
	}

	internal List<int> FreePositions { get; set; }

	internal object JoinFightMap
	{
		[CompilerGenerated]
		get
		{
			uint num = 1u;
			return _003CJoinFightMap_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CJoinFightMap_003Ek__BackingField = value;
		}
	}

	internal int JoinFightTimeout
	{
		[CompilerGenerated]
		get
		{
			return _003CJoinFightTimeout_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 246;
			do
			{
				_003CJoinFightTimeout_003Ek__BackingField = value;
			}
			while ((int)(((uint)(0x141A2F3F | b) / 1485398530u) & (uint)(b - -163978606)) > (int)((uint)b % (uint)(~((-351323845 % (int)(~((uint)b / 556050103u))) & -541723640))));
		}
	}

	internal int Turn
	{
		[CompilerGenerated]
		get
		{
			ushort num = 50998;
			return _003CTurn_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 0u;
			_003CTurn_003Ek__BackingField = value;
		}
	}

	internal bool AutoJoin { get; set; }

	internal bool OurTurn
	{
		[CompilerGenerated]
		get
		{
			return _003COurTurn_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 0;
			_003COurTurn_003Ek__BackingField = value;
		}
	}

	internal List<int> WaitTeammates { get; set; }

	internal List<int> Teammates { get; set; }

	internal bool IsLeader
	{
		[CompilerGenerated]
		get
		{
			ushort num = 1;
			return _003CIsLeader_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CIsLeader_003Ek__BackingField = value;
		}
	}

	internal bool KickUnwantedPlayers
	{
		[CompilerGenerated]
		get
		{
			short num = 0;
			return _003CKickUnwantedPlayers_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 1;
			if ((((((uint)(num >>> num) < 1091963137u) ? 1u : 0u) / (uint)(0x1A1AC983 | num)) | (uint)(-691109386 / num)) != (uint)num)
			{
				_003CKickUnwantedPlayers_003Ek__BackingField = value;
			}
		}
	}

	internal int AllyRatio { get; set; }

	internal int BoostRatio
	{
		[CompilerGenerated]
		get
		{
			ushort num = 0;
			return _003CBoostRatio_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CBoostRatio_003Ek__BackingField = value;
		}
	}

	internal int SummonRatio
	{
		[CompilerGenerated]
		get
		{
			sbyte b = -31;
			return _003CSummonRatio_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CSummonRatio_003Ek__BackingField = value;
		}
	}

	internal int KillRatio
	{
		[CompilerGenerated]
		get
		{
			return _003CKillRatio_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 156;
			_003CKillRatio_003Ek__BackingField = value;
		}
	}

	internal int HealRatio
	{
		[CompilerGenerated]
		get
		{
			short num = -22031;
			return _003CHealRatio_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CHealRatio_003Ek__BackingField = value;
		}
	}

	internal bool AllowBoostSelf
	{
		[CompilerGenerated]
		get
		{
			uint num = 7u;
			return _003CAllowBoostSelf_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 416253544;
			_003CAllowBoostSelf_003Ek__BackingField = value;
		}
	}

	internal bool AllowBoostSummons
	{
		[CompilerGenerated]
		get
		{
			return _003CAllowBoostSummons_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 1862787162;
			if (-num >> (num | 0xE060FA1) != 0)
			{
				_003CAllowBoostSummons_003Ek__BackingField = value;
			}
		}
	}

	internal bool AllowBoostAllies
	{
		[CompilerGenerated]
		get
		{
			return _003CAllowBoostAllies_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 43590u;
			_003CAllowBoostAllies_003Ek__BackingField = value;
		}
	}

	internal bool AllowHealSelf
	{
		[CompilerGenerated]
		get
		{
			return _003CAllowHealSelf_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 31;
			_003CAllowHealSelf_003Ek__BackingField = value;
		}
	}

	internal bool AllowHealSummons { get; set; }

	internal bool AllowHealAllies
	{
		[CompilerGenerated]
		get
		{
			uint num = 0u;
			return _003CAllowHealAllies_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 9457;
			if ((uint)((int)(2291253797u / (uint)(num >> 10)) >> (num % 574483731 >>> (num >> 5) * (int)((uint)num / 4173002919u))) < (uint)(ushort)((-80203602 & (num << 3)) ^ 0x4EBA6A83))
			{
				_003CAllowHealAllies_003Ek__BackingField = value;
			}
		}
	}

	internal List<int> WaterEffects { get; set; }

	internal List<int> EarthEffects
	{
		[CompilerGenerated]
		get
		{
			return _003CEarthEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			ushort num = 0;
			_003CEarthEffects_003Ek__BackingField = value;
		}
	}

	internal List<int> AirEffects
	{
		[CompilerGenerated]
		get
		{
			short num = 0;
			return _003CAirEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			sbyte b = 0;
			_003CAirEffects_003Ek__BackingField = value;
		}
	}

	internal List<int> FireEffects
	{
		[CompilerGenerated]
		get
		{
			int num = 1879048192;
			return _003CFireEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CFireEffects_003Ek__BackingField = value;
		}
	}

	internal List<int> NeutralEffects
	{
		[CompilerGenerated]
		get
		{
			return _003CNeutralEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			uint num = 0u;
			do
			{
				_003CNeutralEffects_003Ek__BackingField = value;
			}
			while ((0x22BF169A & num) < ((uint)((int)num >> 24) | ((num | num) >> 29)));
		}
	}

	internal List<int> PushEffects
	{
		[CompilerGenerated]
		get
		{
			byte b = 208;
			return _003CPushEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CPushEffects_003Ek__BackingField = value;
		}
	}

	internal List<int> DamageEffects
	{
		[CompilerGenerated]
		get
		{
			int num = -1055542783;
			return _003CDamageEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CDamageEffects_003Ek__BackingField = value;
		}
	}

	internal List<int> HealEffects
	{
		[CompilerGenerated]
		get
		{
			return _003CHealEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 3;
			_003CHealEffects_003Ek__BackingField = value;
		}
	}

	internal List<int> SummonEffects
	{
		[CompilerGenerated]
		get
		{
			sbyte b = -37;
			return _003CSummonEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			sbyte b = 49;
			_003CSummonEffects_003Ek__BackingField = value;
		}
	}

	internal List<int> CloseSummons
	{
		[CompilerGenerated]
		get
		{
			byte b = 164;
			return _003CCloseSummons_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CCloseSummons_003Ek__BackingField = value;
		}
	}

	internal List<int> BoostEffects
	{
		[CompilerGenerated]
		get
		{
			return _003CBoostEffects_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 213;
			_003CBoostEffects_003Ek__BackingField = value;
		}
	}

	internal Fighter(FighterManager fighterManager, InstanceData instanceData)
	{
		int num = 0;
		ushort num5 = default(ushort);
		ushort num3 = default(ushort);
		ushort num2 = default(ushort);
		short num4 = default(short);
		while (true)
		{
			switch ((uint)num % 36u)
			{
			default:
				_671BC22C.BF9F3D1F.FE8E0C9E[0x58BBE708 ^ ((uint)num / 2190203658u + 1488711483 >> (num & num))](this);
				FighterManager = fighterManager;
				num = (int)(((uint)num ^ ((981579783u > (uint)num) ? 1u : 0u)) & 0xDA985D87u) - (num >>> num);
				if (((uint)((num % -1664456588) ^ (short)num) ^ (1755682952u % (uint)num - 411492258)) != 0)
				{
					break;
				}
				goto IL_0848;
			case 1u:
				InstanceData = instanceData;
				goto IL_010a;
			case 2u:
				num = (((num >> num >>> 20) | -num) & 0x19A2F731) - 293750576;
				SpellsToUse = new List<int>();
				num5 = (ushort)(655371780 * num);
				num = -183649925 + (num << (-1870139208 * num5 >> (int)(655102099u / (uint)num)));
				break;
			case 3u:
				num = ((0x1D8D2291 ^ num ^ num5) << num * 2067196586) + -830177279;
				Enabled = (byte)((sbyte)num % num * (int)((uint)(num5 + 1689805338) / uint.MaxValue) + 1) != 0;
				if (((~(num >>> (int)num5) % -1718174438 == 75806111 - num) ? 1 : 0) != (((-2004876498 ^ num) & -618625863) << (137453968 << num)) * ((-875738917 | (num5 + num5)) + (-416239344 >> (int)num5)))
				{
					num = (num5 & int.MinValue) ^ -1406287872;
					break;
				}
				goto IL_062c;
			case 4u:
				num = (int)((uint)num / uint.MaxValue >> 2) - -1;
				SpeedMode = "normal";
				if ((num5 | 0x7CB3A32F) > num5 + (-1751953730 - num5 * -1551414123) >>> ((int)num5 - ((num5 < 1454543143) ? 1 : 0) << 9))
				{
					num = (-1165493733 / num - (-122605295 >> (0x16131EBD ^ num5))) ^ -290375286;
					break;
				}
				goto IL_0a75;
			case 5u:
				num = (-1777283438 + num5) ^ -1777271657;
				goto IL_02a3;
			case 6u:
				num = 0x8498 ^ num5;
				CastSpellMin = (sbyte)num5 + 253;
				num3 = (ushort)((((uint)num5 > ((num > ~num) ? 1u : 0u)) ? 1 : 0) / (int)num5);
				if (num * -15842 != 0)
				{
					num = -609479385 ^ (-633776604 * ((int)((uint)num5 / 4228944444u) / -1061949301));
					break;
				}
				goto IL_0a5c;
			case 7u:
				num = (num ^ -398685126) * 2075199030 - (num3 << 2) - 270793245;
				CastSpellMax = (num5 | -1721650197) - -1721649457;
				ActionMin = -1530468255 + (num | 0x5B391803);
				num = (int)(1669674267u % (uint)(num5 << (int)((uint)num3 / 589927723u)));
				num = (num5 | -29918580) - 697074457;
				break;
			case 8u:
				num = 21552 + ((num5 / 154145046) ^ (short)num3);
				ActionMax = -1539317063 + (-1608847858 + ((1873557764 << num) | (-1834929875 >>> ((num < num) ? 1 : 0))));
				num2 = (ushort)(-num3 / ((0xD33D99C ^ num) * num >>> (((uint)num3 > (uint)num3) ? 1 : 0) - (int)num3));
				if (~(128843305u / (uint)(~num2)) != 0)
				{
					num = 35694249 + 1780129792 / ~num2;
					break;
				}
				goto IL_068e;
			case 9u:
				num = 11951 + (ushort)((uint)num / (uint)num5);
				EnterFightMin = 100 + (num3 >>> (int)num2);
				if ((num3 | -29810) < (num3 | 0x4E8D1023))
				{
					num = (int)(26012u / (uint)((int)(2075052951u % (uint)(0x698B052A & num5)) % (675911 << 899569792 / ~num3))) - -2116196898;
					break;
				}
				goto IL_1630;
			case 10u:
				num = -590588752 + (0x2333FF80 | num3);
				goto IL_04c4;
			case 11u:
				num = (int)((uint)(-736452601 - num2) / (uint)(~num3) * num2 / 287517984) - -21552;
				AutoLock = (byte)((uint)num % (uint)(~num3) - 21552) != 0;
				if ((uint)num2 <= (uint)(num5 | 0x4489F68C))
				{
					num = (int)((uint)((num5 + -417727204) * -291252074 << 2) / (uint)(-(num >> (int)num3))) - -1513614324;
					break;
				}
				goto IL_08f7;
			case 12u:
				num = (byte)((uint)num3 % (uint)(num5 * 378750745) / (uint)(35 >>> (int)num3)) - -21552;
				AutoSkipTurn = (byte)((int)(1880544534u % (uint)(~(-num3))) % (int)(~(num2 & ((uint)num2 / 2293032507u))) << (int)num5) != 0;
				num5 = (ushort)(num & 0x4715C73A);
				num = 11 - num - 333956678;
				break;
			case 13u:
				num = -704621520 + (num2 / num5 + num << (-1106407399 >> (-545181947 & (num2 >>> (int)num3))));
				FightMode = "berserker";
				ChickenDistance = (num / ~(num3 >>> (int)num3) / (int)(2067117887u % (uint)(num | num5)) >> ((num3 << num == (int)((uint)num3 / 3827327395u) >> ((num2 == -1842745285) ? 1 : 0)) ? 1 : 0)) ^ -15;
				goto IL_062c;
			case 14u:
				num = ~num4 - -21553;
				IsReady = (byte)(num4 >> 1 >> 31) != 0;
				goto IL_068e;
			case 15u:
				num = (347009411 - num5 >> 8) - 1333953;
				FreePositions = new List<int>();
				goto IL_0713;
			case 16u:
				num = 21552 + (byte)((int)((uint)num4 % (uint)(~num2)) * (int)num4);
				JoinFightTimeout = -(num2 * num4 - 2013265920 % ~(-num4));
				Turn = num2 & 1 & 0x401ACE2E;
				num = -360383003 + num4;
				if ((uint)(((num4 | -207822283) - num) / ~num4) / (uint)(~(num3 / -521412957)) >= (uint)(num3 << (int)((uint)num4 % (uint)(~(num4 * -449645939)))))
				{
					num = (-1 & num3) - 1430778443;
					break;
				}
				goto IL_02c6;
			case 17u:
				num = (int)((uint)num5 % (uint)(num * -1916920063) << num2 * num2) ^ -360383102;
				goto IL_07dc;
			case 18u:
				num = -529215663 ^ -num4;
				WaitTeammates = new List<int>();
				goto IL_0848;
			case 19u:
				num = num3 + -529215663;
				BoostRatio = 0x3D3496B2 ^ (-(num2 * num3) - (num - 497640968));
				goto IL_08f7;
			case 20u:
				num = 133354060 + (-662569723 ^ ((num3 & num5) >>> (int)num4));
				HealRatio = (0 & (num + (-267110359 >> (int)num2 >> (int)num2))) + 4;
				num2 = (ushort)(0x57BB092E ^ ((num ^ num5) / (-1987181643 - num4)));
				if (num5 - -1690323184 != 0)
				{
					num = (-num4 | (num3 | 0x26B55403)) + 1299159474;
					break;
				}
				goto IL_0853;
			case 21u:
				num = (0x85 | num4) - 529215796;
				goto IL_09d2;
			case 22u:
				num = -529215731 ^ ((0x2C5C ^ (1203014580 / ~(num / 1955211304))) >>> (int)(sbyte)(-num5));
				AllowBoostAllies = (byte)((uint)(-num2) / (uint)(-1633088988 * num2)) != 0;
				goto IL_0a5c;
			case 23u:
				num = 112920359 + (1678466979 + num);
				WaterEffects = new List<int>
				{
					(ushort)(-559584230 * num2 * (num5 - 1263767898)) - 39559,
					(int)((uint)(0x53BB2008 | ((byte)num2 | -num5)) / ~((uint)num2 / 1889757595u)) - -91,
					(int)(0x60 ^ (406 / ~((3016941738u < (uint)num5) ? 1u : 0u))),
					((0x783F219D ^ (-499764832 + (num5 ^ num))) & num2) - -142,
					-30135 + num3,
					(int)(426 + (uint)(2072362249 - num >> (int)((uint)num3 % 1652734637u)) / uint.MaxValue),
					(int)((uint)(255796887 % (int)(2989140126u % (uint)(~num4) << 4)) % ((3148750771u < (uint)(num5 << (int)num2)) ? 1u : 0u)) - -1014,
					(num | 0x26126585) - -428414036,
					-num2 - -34538,
					(int)(0x86F9 ^ ((uint)num2 % (uint)((-1021099388 / num2 - num4) ^ num3))),
					(num + num4) ^ -529216720,
					((num % (1041581095 - num)) & (-197199298 - (-1836550116 ^ num2))) % (498147000 >> (num5 << (int)num5) >> 7) - 2878467,
					num5 % ((int)((uint)num ^ ((num5 < 1202002952) ? 1u : 0u)) - (0x529A2E09 | num5)) + 1029,
					-52039 + (ushort)(num + num5)
				};
				if ((uint)(-1854 / num5) % (0 - (((uint)num3 / 3584024766u) | num5)) != 0)
				{
					num = -1999775475 ^ (1885800942 - num);
					break;
				}
				goto IL_06ce;
			case 24u:
				num = ((sbyte)((num5 | num2) << (num3 >> num)) + -289746920) ^ 0xECE1F46;
				goto IL_0cb0;
			case 25u:
				num = (num4 & -1399193210) ^ -529215663;
				AirEffects = new List<int>
				{
					((~num3 << num + num3) | -num2) - -33557,
					(int)((uint)(-458785406 * -(-num)) % (uint)num2 - 5433),
					(byte)(num3 * num5) ^ 0x18,
					(int)((uint)(-810802502 >>> ((-1858584178 + num4) ^ num3)) / 2285414934u + 277),
					(int)(((uint)num5 % (uint)(~num4)) ^ 0xFE55),
					(-90238541 >> (((uint)(num4 >> (int)num5 << (num3 ^ 0x5975FBF)) < (uint)num5) ? 1 : 0)) ^ -45118676,
					(int)((uint)(-num4) % (uint)num) - -1064,
					((num4 << num2 + num) ^ num3) - (num5 ^ 0x401C1619) - -1075607361,
					((((ushort)num4 < -2105283535 % num3) ? 1 : 0) + (int)((uint)(num3 * -1698740837) % (uint)(-248175594 - num))) ^ 0x2010F7F,
					(num2 ^ 0x7CD4) + -64011,
					(-1658239612 | num) - -42087568,
					1131 + ((868043185 << (int)num5) & 0xA6),
					(-846560489 + num >>> (((uint)num < (uint)(num3 ^ 0x6EB7E7B4)) ? 1 : 0)) + 1375777288
				};
				goto IL_0f5d;
			case 26u:
				num = (((uint)(((num5 < num2) ? 1 : 0) / (int)num2) < (uint)(num / (int)(4052332935u / (uint)num2) >> 19)) ? 1 : 0) - 529215664;
				NeutralEffects = new List<int>
				{
					(((num2 == (0x44A69F00 & (num | -2110999527))) ? 1 : 0) >> (int)(((uint)num % 4245353241u) & (uint)(num2 * num3))) + 82,
					-1092602825 ^ ~(1092602769 % (-1950675072 + (sbyte)num3)),
					(int)((uint)(-num5 ^ (sbyte)num5 ^ num2) % ~((uint)(num2 ^ num2) % (uint)(0x20054873 | (num3 - num4))) - 16165),
					num - (1286798004 + num5 >>> 28) - -529215767,
					((-1686383364 ^ num4) >>> 16 << (-71820382 | (1921375489 - num3 >> (num ^ 0x6537C82E)))) - 159069,
					(int)(((uint)(num5 - -552672222) % 3719247488u) | 0xE27DEA7Du) - -486540433,
					((num5 & ((num3 | -2037543362) & num4)) ^ (num5 & num2)) + -48993,
					(641704193 >> (num3 & num5)) / ~(num * (int)((uint)num5 / 2401659827u)) + num3 - -2476677,
					670 + num4,
					(int)((uint)num4 / (uint)(-num5) + 671),
					num5 - 64857,
					num3 - 29398,
					(1469330220 >>> (int)((uint)num5 % (uint)(sbyte)(~num))) - -1028,
					-529216747 ^ num,
					num2 + -48165,
					(int)(529257398 + ((uint)num ^ ((uint)num5 % (uint)(num % -224110935)))),
					(num4 + -762356812 >>> (-2078300274 + (num5 ^ -1692044747)) % ~(-num4 % (0x499B390F | num4))) - -762357936,
					-7071 + ((num >>> 8) & -1676171251),
					(int)((uint)(~num4) / (uint)(0x4BB38C0D | num3)) / ~(num2 >> 17) + 549736505 + -549735363
				};
				if ((((uint)num < (uint)num) ? 1u : 0u) <= (uint)(1779194903 >>> (int)num2) % (uint)(~(num5 / -484211304 << 29)))
				{
					num = (int)((uint)(num5 | -num3) / (uint)((-num4 - num) | num)) ^ -1976750582;
					break;
				}
				goto IL_0713;
			case 27u:
				num = -529221221 ^ (num3 << (int)(513322171u / (uint)(~num4)));
				PushEffects = new List<int> { (int)((num3 ^ (((num2 == num) ? 1u : 0u) / 3165631899u)) + 285582509) + -285612914 };
				if ((ushort)(((num2 * -552110184) | (0x4F279595 & num)) - (num2 - 1185331733)) >= ~num3)
				{
					num = -1985927880 ^ ((394248u < (uint)num3) ? 1 : 0);
					break;
				}
				goto IL_06a3;
			case 28u:
				num = -529215663 + (int)((uint)num2 % (uint)num3) / (int)num3;
				DamageEffects = new List<int>();
				if (((num * 1678319293) | -64909366) != 0)
				{
					num = (int)((uint)(~(~num3 | 0x448AA004)) % 553500138u - 274090265);
					break;
				}
				goto IL_04c4;
			case 29u:
				num = ((num2 & num3) - (num4 >> (int)num3) - num2) ^ 0x1F8BB081;
				DamageEffects.AddRange(WaterEffects);
				DamageEffects.AddRange(EarthEffects);
				if (715238168 / ~num4 != 0)
				{
					num = (num + (407798542 >> num4 * num5)) ^ 0x4D630BA1;
					break;
				}
				goto IL_07dc;
			case 30u:
				num = (num >>> 2) - 1291011886;
				goto IL_1527;
			case 31u:
				num = -529215663 + (int)((uint)(num >> 18) / (uint)(~num4) >> (-1682003522 & num4));
				DamageEffects.AddRange(FireEffects);
				goto IL_15a4;
			case 32u:
				num = (short)(1875963134u / (uint)(~num4)) + -529215663;
				HealEffects = new List<int> { -48002 + (int)(428280125u % (uint)num5) };
				num3 = num3;
				num = ((0x3B24EE10 & (num2 + ~num5)) >>> (int)num4) ^ 0x362E4AB5;
				break;
			case 33u:
				num = -310491676 ^ num;
				goto IL_1630;
			case 34u:
				num = num - (byte)(num3 + num4 / num) - 521109105;
				CloseSummons = new List<int>
				{
					-346847736 + (int)(~((num3 == 1662476683) ? 1u : 0u) + 346847778),
					num5 - 868091700 - -868026231,
					0xFF78 ^ num5,
					num4 ^ 0xB6,
					(-1643099895 | (0x6288BB15 ^ num5)) ^ 0x434400AC ^ 0x1A7F9A65 ^ -1482434658,
					num - 0 - -529215850,
					187 + (-399176958 >>> (int)((uint)num4 / 1152586771u + 850386334)),
					((247057834 >>> (-80926280 << (int)num4) < num3) ? 1 : 0) ^ 0x16E
				};
				BoostEffects = new List<int>
				{
					((num + -110) ^ 0x1A0F5702) - -92563080,
					(-num2 << (int)num2) - 402653077,
					(int)((((uint)num2 < (uint)num4) ? 1u : 0u) & (uint)num3) - -356964318 % ((num5 % num) | num) + -356964208,
					num5 + 2 - 65420,
					0x785CF82B ^ (num4 - -2019358811),
					~(num & -122630453) / (int)((uint)(num3 & 0x228B218E) % 278339514u) - 66708933,
					-1217933231 + (-(num2 * num + num4) & -608256990),
					(sbyte)((num4 + 1638965273 + 864882599) ^ 0x3200) ^ -75,
					0x588682C1 ^ (0x588682B7 | num4),
					num4 - -119,
					0x798F7EF9 ^ (num4 ^ 0x798F7E81),
					-571706195 ^ (num ^ 0x3D98BB85),
					(short)(~(num4 >> (int)num4) ^ num5) + 117,
					(~(num4 - num3) << -1 - (int)((uint)num5 % (uint)num)) + ~num4 + -2101571,
					((num / ~num4 / (num2 + -633654246) == 806008864) ? 1 : 0) + 125,
					(int)((uint)(-7675510 + (num - 1697076246)) % 1327190150u - 733809601),
					0x3497C7D2 ^ (353146532 + ~num),
					0x88 ^ ((num4 & num) / ~num4 % -2051930446 * ((319637164 << (num & num5)) % num3)),
					num5 - 0 - 65392,
					(num2 | num5) - ~(num3 * -1587823654) + -1069208770,
					((-2139075554 < ((num5 / 1704302366) | 0x16B47027)) ? 1 : 0) - -141,
					(int)(0x101DE572 ^ (3852943120u / (uint)num2 * ((uint)(~num % (num5 ^ num4)) % (uint)(~((2134276244 % num) & (num4 & 0x659D301E)))))),
					(num << (int)num2) - 1140850530,
					(0x646134CF | num2) + -1684141151,
					-1783041365 + (num | ((byte)num3 >>> (num5 | num5))) * -536072938,
					(num3 << (int)num2) + -536870160,
					(int)(753 + ((((uint)num > (uint)(num2 - 76060431)) ? 1u : 0u) << (int)num5)),
					num2 + -49109,
					(short)(num4 << (int)num3) - -174,
					(short)((uint)num3 / 1217762186u) ^ 0xB0,
					(int)((0x8907F4B4u ^ (0x76804113u & ((4172801316u < (uint)num) ? 1u : 0u))) & (uint)(~num3 - num2)) ^ -1996049274,
					(num ^ num5) % -2136408018 / ~(num5 / -1097521399) + -529256098,
					(int)(0x46C6474B ^ ((uint)(-1187399677 ^ ~num4) % (uint)(~(num4 << ((num2 > 2107213702) ? 1 : 0))))),
					-32656 + (int)((uint)(num3 % 503601076) % (uint)((num5 ^ -619573482) | (num4 | -650890184))),
					(num5 >> num5 + 839843643) * (136277303 << (num4 >>> 1)) + (int)((uint)((2042422562 >> (int)num4) % num2) % 2023858223u) - 14474,
					(int)(4124426158u / (uint)num) % ~(~num3 * (num3 / (num2 + -357626966))) - -210,
					0xD3 ^ (num2 % ~num4),
					216 + 1755952180 / (int)((uint)(num2 >>> (int)num3) % (uint)(num5 + 243787015) * 713734662),
					(num2 & -354482476) - -133,
					(int)(209 + ~((uint)(540739756 << (int)num4) / (uint)(~num)) / (uint)(num3 + 834057660)),
					(int)(1295807930u % (uint)(~((num + -259927373) & -num4)) - 1295807690),
					((815410750 > num5 % -54675318 / ~(num4 >> 13)) ? 1 : 0) * ((num2 & -910456410) + ((ushort)num << 2)) - 244853,
					0xA91 ^ ((-492830529 ^ (-894419953 % (-1526227042 * num))) >>> num),
					(2005871502 * num) ^ -1797482211,
					(int)(((1362137144 * ((uint)num5 / (uint)(~num4) % num5)) | (((uint)(~num >> 25) > (uint)(num ^ (-1799950028 | num))) ? 1u : 0u)) ^ 0xF4),
					(int)(0u % (uint)(~num4) + 250),
					0x80B3 ^ (0x19AEAC6F & num3),
					(int)(0xFC ^ ((0x2923E421u & ((984381193u > (uint)num) ? 1u : 0u)) << num2 * num4)),
					(int)((((uint)(num3 * 286129461 + 498714767) < 1857416595u) ? 1u : 0u) % 1727166462u) - -253,
					(int)((uint)((-1517774550 >> (1661293959 >>> (int)num3)) + num) / (uint)(1839702790 >>> (int)num4)) - -252,
					(int)(0x104 ^ ((uint)(num5 << (int)num4) % 1u - (uint)(-num) / (uint)(-251195222 | (num % 302161305)))),
					-num2 ^ -49533,
					0x106 ^ (0 - (((uint)(1227511842 >> (int)num5) > (uint)((num2 + num3) * (num5 % 33855770))) ? 1 : 0)),
					(((num ^ -164997990) << 30) & num2) + 263,
					(num2 | 0x5C) - 49014,
					((num4 >> 16) + 590683800 >> (int)num5) - -1022,
					0x8458 ^ num3,
					-32559 + num3 % -498954531,
					0x7A244632 ^ (2049197864 + (num2 << (int)num3 >> num + num5 >>> 23)),
					0x11B ^ (ushort)(num5 >>> 5 << (int)num2),
					(num4 ^ num2) - 48990,
					-num2 ^ -49509,
					(int)(4027325612u / (uint)(-879965565 << (int)num5 << (int)num4)) % ~(short)(num3 & -1818943686) + 282,
					536871199 + -1581993801 * (-307595896 << (int)num2 >> 0),
					(-272 << -2027405142 - (312384055 - (-1458098032 + num2))) ^ 0x120,
					(int)(0x121 ^ ((((uint)(-601435592 ^ num) < (uint)((num << 12) % ~num4)) ? 1u : 0u) << (num | 0x252F3B9B))),
					529215953 + num,
					(~(num3 >>> num) | ((int)(4052721557u / (uint)num3) + (num2 + num5) - -(num3 + -1173586381))) + 292,
					292 + ((num5 >>> 16) & 0),
					~(num3 >>> 2033587382 / ~(num2 >> 25)) ^ -262,
					1434791868 + (num4 - 1434790814),
					(int)((uint)num3 % 376248005u - 32075)
				};
				if ((-1442268509 << (-836816739 << (int)num2) / num3) / (num + num2) != 0)
				{
					num = 1192069141 + ((-1665884928 + num4) % 46984906 >>> (int)num3);
					break;
				}
				goto IL_0cb0;
			case 35u:
				{
					num = ~(num2 / 1479244856) % (num2 % num3) + 1946893205 + 1818858429;
					return;
				}
				IL_0848:
				Teammates = new List<int>();
				goto IL_0853;
				IL_0a5c:
				AllowHealSelf = (byte)((ushort)((int)num2 + ((num4 * num4 == num3) ? 1 : 0)) - 33470) != 0;
				goto IL_0a75;
				IL_0cb0:
				EarthEffects = new List<int>
				{
					(byte)(1720694666u % (uint)(~num4)) - 52,
					1753700642 / ~(num5 & (num5 / num3)) * num2 - -1337420696,
					(int)((uint)(-num5 >>> (int)num4) & ((uint)num % (uint)num)) - -97,
					-1559477246 + num2 - -1559443933,
					(num2 & -543439219) ^ 0x8398,
					-1435729786 ^ (-1435729632 ^ num4),
					((num4 / -1970359153 / num5) | -1842552415) * (num5 << (int)num5) - 123985694,
					(num5 << (0x248A2086 ^ num) * num * (int)(((uint)num2 % (uint)num3) & 0x60BB58E)) + -1879047064,
					((num5 | 0x7115415) >> ((num >>> 22 == -1406388474) ? 1 : 0)) - 118575107
				};
				num5 = (ushort)((uint)(ushort)(-(0x162FD51F & num5)) >> (int)((uint)((-268303734 | num3) & (num2 + num5)) % 2602635039u));
				num = num3 ^ 0x7989A3EF;
				break;
				IL_0853:
				IsLeader = (byte)(6927379 + ((num4 | num4) / (int)(1460902844u % (uint)(~num4)) + num3 + -6927379)) != 0;
				KickUnwantedPlayers = (byte)num4 != 0;
				AllyRatio = -267065716 ^ (-1735868284 - ~(1468802564 + num3));
				if ((num2 ^ (1258701723 >>> (int)num2)) - (-1650489697 << (num5 - -1852882146 << (int)num5)) != 0)
				{
					num = (ushort)(1 - num4) + -1960636126;
					break;
				}
				goto IL_02a3;
				IL_0a75:
				AllowHealSummons = (byte)((0x28638D5 | num) + 487129132) != 0;
				goto IL_0a89;
				IL_07dc:
				AutoJoin = (byte)(((int)(4294966357u / (uint)(~num3)) * (num2 / ~(num3 * num3))) & (1451252241 >> (int)num4)) != 0;
				OurTurn = (byte)num2 != 0;
				num = num2 - 529215663;
				num = (int)(((num3 | (3761638534u / (uint)num5)) << (int)(2610793857u % (uint)num5) << 8) - 1122420454);
				break;
				IL_0a89:
				AllowHealAllies = (byte)((((uint)(0x44EAD2C2 | num3) < (uint)num2) ? 1u : 0u) - uint.MaxValue) != 0;
				num3 = (ushort)(0x179DB99B ^ num);
				num = 0 - ((30516401 == (num3 ^ ((uint)num4 % 990509730u))) ? 1 : 0) - -1974364295;
				break;
				IL_0713:
				JoinFightMap = null;
				if ((((uint)(num4 * 2108001706) > (uint)(-(-num4))) ? 1 : 0) / 1 == 0)
				{
					num = (((uint)num4 < 3970640689u) ? 1 : 0) - -213025551;
					break;
				}
				goto IL_1527;
				IL_02c6:
				FinishTurnMax = (int)(0x224 ^ ((uint)((int)(1831095090u / (uint)num) * (-539123603 & (1262473259 + num))) % 1152u));
				num5 = (ushort)(num5 + (2057660752 >>> num5 - (num << 19)));
				num = -1882181474 ^ (-222653934 << (int)(1303986838u / (uint)(0x563C9C00 & (-1976737016 ^ num))));
				break;
				IL_08f7:
				SummonRatio = 5 + (int)num3 % (int)(~((uint)(1 & num4) / (uint)(num3 | 0x3BB1FBBD)));
				KillRatio = -529215662 ^ num;
				if (-392050125 / (num3 | -1312929731) == (int)((uint)((byte)(num4 - 2117114407) ^ (num5 % ~num2)) / 2073807011u))
				{
					num = (num3 | -1355466691) - -919687931;
					break;
				}
				goto IL_15a4;
				IL_062c:
				TeamId = null;
				num4 = (short)((int)((uint)(num3 >>> ~num3) % (uint)(~((((uint)num2 > (uint)num) ? 1 : 0) * (num5 >> (int)num3)))) - (0 << ((-1768027348 + num2) ^ num)));
				num = (-418658511 | num) + 723289453;
				break;
				IL_15a4:
				DamageEffects.AddRange(NeutralEffects);
				if (35673511 << (int)(731390722u % (uint)num) != 0)
				{
					num = ~num5 - 1793575546;
					break;
				}
				goto IL_010a;
				IL_04c4:
				EnterFightMax = -384077664 * ((-843933052 < ~num2 >>> 23) ? 1 : 0) + 384077864;
				if (~(num & -1170659326) != 0)
				{
					num = (-694507608 | (-1625783767 / -num)) ^ -1358673496;
					break;
				}
				goto IL_1527;
				IL_1527:
				DamageEffects.AddRange(AirEffects);
				num3 = (ushort)(num2 & -26443704);
				if ((uint)(num4 - num5) % (uint)(-829940826 & num5) % (uint)((num + 1636375737) ^ 0x23A0DFA6) != (uint)(num + 654374928))
				{
					num = -1606728419 + ~(~num2);
					break;
				}
				goto IL_0f5d;
				IL_010a:
				Spells = new List<int>();
				num ^= ((num == 1319893782) ? 1 : 0);
				if ((uint)(num * 1301761280) >= ((uint)num ^ (((uint)(sbyte)(-759936216 - num) < (uint)num) ? 1u : 0u)))
				{
					num = 46673586 + ((((uint)(-num + num) > (uint)num) ? 1 : 0) - 2023193216);
					break;
				}
				goto IL_06a3;
				IL_0f5d:
				FireEffects = new List<int>
				{
					(num3 ^ -1794231887) - -1794226397,
					~(378649 % num5) - -51099,
					((-15634632 & (num2 + (num2 - num4))) >> 10) ^ 0x22,
					(int)(((uint)(0x67BB7B32 ^ (num4 + 1822279808)) / (uint)(num + 1303443640)) ^ (2183775934u % ((uint)num5 % (uint)num2)) ^ 0x328B),
					(0x6A09618B ^ (1276252088 >> (num3 ^ num2))) + num3 + -1779030905,
					((-609357943 / (int)((uint)num % 311229583u) * (int)(((uint)num4 % (uint)num3) & 0x3B2C0CA9)) ^ (num + -1498653049 - (int)(((num5 < num5) ? 1u : 0u) / (uint)(~num4)))) + 2027868990,
					(((num5 ^ 0x69B02E3E) + -num >>> 24) | num5) - 65105,
					1170107995 + (1370687622 * num - num3),
					(num & 0xBF) + 1020,
					-29344 + (int)((num3 ^ (num2 / ~((uint)num3 / (uint)(~num4)))) % (uint)(~((byte)num2 / 855797300))),
					(int)((uint)(num >>> 25 >>> 0 >>> 0) % (uint)(-(num5 ^ -2068650716)) + 957),
					(num3 | 0x4F9E4233) ^ 0x4F9E72BD,
					-106578 + (int)(3275114636u / (uint)((num4 | num3) << (int)((uint)num2 / uint.MaxValue))),
					0xBB4F1AD ^ (num3 + 196378369),
					(num4 % num) ^ 0x46D,
					num2 + -861 - 31471
				};
				num2 = (ushort)((num3 + 0) * (num4 ^ num5));
				if (((2049112348 >>> 1008189313 % num3 - 688034590) & ((num3 - -643171570) ^ num2)) != ((-1560226881 | num3) - (-669862616 >> num4 * num5)) / 613365913)
				{
					num = (num5 ^ -390093553) - 539032516;
					break;
				}
				goto IL_0a89;
				IL_02a3:
				FinishTurnMin = -1004608520 + (int)((uint)(2009217101 >> num) % (uint)(-518378213 + -num << 18));
				goto IL_02c6;
				IL_06a3:
				PlacementActiveTimeout = ~(num5 + num) - 74475090 - -74514199;
				PlacementReadyTimeout = (num3 ^ -1164156744) - -1164156844;
				goto IL_06ce;
				IL_1630:
				SummonEffects = new List<int> { (int)(181 + (uint)(-(sbyte)(num2 >>> 26)) / (uint)(~(num4 << -num5))) };
				if (num5 - 1435227306 != -469417688 * num3 % ((0x39A56FB6 | num4) - num5))
				{
					num = 1073972446 + ((num3 & (short)((uint)num2 / (uint)num2)) ^ -1082078932);
					break;
				}
				goto IL_09d2;
				IL_06ce:
				Positions = new List<int>();
				num5 = (byte)((num5 + ~num) * 1946668441);
				num = ~(num4 >>> 17) - 133018476;
				break;
				IL_09d2:
				AllowBoostSelf = (byte)((num3 * (short)num5 >>> 800257457 - num3 / -172640613) ^ 1) != 0;
				AllowBoostSummons = (byte)(1u + ((num2 > (ushort)((uint)num5 / (uint)num5 + 1536015001)) ? 1u : 0u)) != 0;
				num2 = (ushort)(-num4 | -417758530);
				num = -1710086885 + ~((sbyte)(num + num3) >> 23);
				break;
				IL_068e:
				PlacementMoveTimeout = -((42 % num5) | num2) - -142;
				goto IL_06a3;
			}
		}
	}

	internal void SetSpeed(string speed)
	{
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[720](speed, "slow"))
		{
			goto IL_001b;
		}
		FinishTurnMin = 300;
		ushort num = 49426;
		num = (ushort)((2080588314 << (int)num) % num + 31900);
		goto IL_004a;
		IL_03ae:
		int num2 = default(int);
		EnterFightMax = (-(320435973 >>> (int)num) & (496150931 / (num ^ (num2 ^ -2012004294)))) - -2000;
		return;
		IL_001b:
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[720](speed, "ultraFast"))
		{
			num = 57628;
			num = (ushort)(57986 + ~((sbyte)(~num) << 0));
			goto IL_004a;
		}
		goto IL_03d7;
		IL_0716:
		EnterFightMin = 0x64 ^ (~(0x7086CD1D & (num % num2)) / (0x338A33A1 ^ (53517472 >> (int)num)));
		EnterFightMax = ((1671171354 + ((num2 < -1139601990) ? 1 : 0)) | num) - 1671220819;
		return;
		IL_004a:
		while (true)
		{
			switch ((uint)num % 18u)
			{
			case 1u:
				goto IL_014e;
			case 2u:
				goto IL_0191;
			case 3u:
				num = (ushort)(0xC112 ^ -num2);
				CastSpellMin = num2 * 1738115200 - -603114563 - 603113063;
				if (-num2 - (sbyte)(num2 >> 27) * (-675725178 / ~(num2 - num2)) > num2 - 926324640)
				{
					num = (ushort)(num2 ^ 0xD72C);
					continue;
				}
				goto IL_07eb;
			case 4u:
				num = (ushort)(((sbyte)(3299303214u % (uint)num) << 3) - -49986);
				CastSpellMax = (num >>> (int)(3635002541u / (uint)num) >>> (int)((uint)num2 % 2097874481u)) - 1092984723 + 1092987530;
				num2 = -138466021 + (int)((2419573405u % (uint)(~(num2 << num2))) ^ (uint)(-1724580592 >> -num2));
				num = (ushort)((-82375930 ^ ((1134789797 + num - (num | num2)) / ((num & -477912790) ^ (num >> num2)))) + 82398756);
				continue;
			case 5u:
				num = (ushort)(1266978977 + ((num * num) ^ num2));
				ActionMin = num2 / ((byte)(num2 / 1947875876) - -14018943) + 1000;
				num2 = -97 * num2;
				num = (ushort)(-num2 + -1235949590);
				continue;
			case 6u:
				num = (ushort)((int)((uint)(num % 498091276) / 2097457284u) * (num >> num2 % num) + 49426);
				ActionMax = 664618350 + (num - 664665776);
				num2 = ((-1447228367 > num2) ? 1 : 0);
				num = (ushort)(((uint)(-1132005063 % (num >> ((num < num2) ? 1 : 0))) & ((((num2 | num) < num) ? 1u : 0u) / 1965236244u)) - 4294914045u);
				continue;
			case 7u:
				goto end_IL_004a;
			case 8u:
				num = (ushort)(((int)((((uint)num > (uint)num2) ? 1u : 0u) & ((uint)num % 8596373u >> 0)) / 525471286) ^ 0x92CC);
				EnterFightMin = (-812447965 ^ num2) - -998459223;
				if (-937380437 * num != 0)
				{
					num = (ushort)(-112506873 ^ num ^ -112524972);
					continue;
				}
				goto IL_07c1;
			case 9u:
				goto IL_04eb;
			case 10u:
				num = (ushort)(0xBEF2D5C ^ num2);
				return;
			case 11u:
				num = (ushort)((389153285 * ((num + num) / 2081301823)) ^ 0x84);
				FinishTurnMin = 138909095 * num + -1156131326;
				num = (ushort)(906152705u % (uint)(~(num / 1461049790 << (int)num)));
				num = (ushort)((uint)((num >> 4 << 6) % num) / (uint)(~((int)num / (int)(~((-231079423 == num) ? 1u : 0u)) * (187530937 % (int)(~((uint)num % (uint)num))))) + 29442);
				continue;
			case 12u:
				num = (ushort)((1892453960u / (uint)(974434615 + (num >> (int)num)) >> (int)num) - 4294915327u);
				FinishTurnMax = num + -51909;
				CastSpellMin = (int)((uint)(num + -818123486 >>> 14) / (uint)num + 146);
				CastSpellMax = 1618576520 + (-1618628189 + num);
				num2 = (int)((((uint)num / (uint)num) & 0xC13D1CA1u) % 2326167843u);
				num = (ushort)((1780668989 >>> ((-81119611 * num) | -373632474)) % ((2072258842 * num % num2) ^ ((377938956 - num2) * -2004010987)) - 13890421);
				continue;
			case 13u:
				goto IL_0679;
			case 14u:
				goto IL_0706;
			case 15u:
				num = (ushort)(0 - 749431597u % (uint)(~(num >>> num2)) / 4111173505u + 57628);
				FinishTurnMin = 0x96 ^ (num >> (int)num << (num2 >>> 9 << (int)num) % -1564999680);
				goto IL_07c1;
			case 16u:
				goto IL_0848;
			case 17u:
				{
					num = (ushort)(0x8009 ^ (3822773700u / (uint)((0x1185E693 ^ num) | (num >>> 25))));
					EnterFightMin = (0x38A57838 ^ num) - 950400584;
					EnterFightMax = (int)(((uint)(num / num) % 1261869826u) ^ 0x3E9);
					return;
				}
				IL_07eb:
				CastSpellMax = 1500 + ((num % num) & -536078329);
				ActionMin = (num2 >> num2) % -1 + num + -57128;
				num = (ushort)((-669017434 + (int)(169694118u / (uint)num2 >> num * 1427822349)) & num);
				num = (ushort)(-177086462 ^ (278357543 * num));
				continue;
				IL_07c1:
				FinishTurnMax = -(-2077678076 / num2) - 35758;
				CastSpellMin = (num2 >> (int)(byte)(~num2)) + 722;
				goto IL_07eb;
			}
			num = (ushort)(-1186135101 / ((int)(2240831912u / (uint)(num | -1123579595)) - ~(num * 806715966)) - -57628);
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[(((uint)(-930232769 >> (((uint)num > (uint)num) ? 1 : 0) >> (-1765914709 | (0x62B90E13 & num))) < 3985135623u) ? 1u : 0u) ^ 0x2D0u](speed, "fast"))
			{
				if (-1812079283 - (sbyte)(-805063017 << (int)num) >>> 2 == 0)
				{
					num2 = (-1992398148 & (num / 1880520222)) + (num | 0x92A1880);
					num = (ushort)(23037 + (int)((uint)num % (uint)(-1020408270 - (sbyte)num)) % (-644967676 % num));
					continue;
				}
				goto IL_0759;
			}
			num = (byte)(-1156767356 | (num >>> (int)num));
			num = (ushort)(0x3ACA2D1 ^ ((1073975354 >>> ((-num == num) ? 1 : 0)) * (num + num)));
			continue;
			IL_0848:
			num = (ushort)(189112039 + (-189095907 | num2));
			ActionMax = (num >> num2 + num) + 1000;
			num2 = (int)((uint)num2 % (uint)(num2 >> (int)((uint)(1143419542 % num2) & ((num > num2) ? 1u : 0u))));
			if ((uint)((((num2 | num) ^ 0) + ((num2 == (int)((uint)num % 3910179217u)) ? 1 : 0)) * -1433342678) <= (uint)(-((-91855870 * num) | num2)))
			{
				num = (ushort)((422881800 << (int)((uint)(-736286191 | num2) % 2241983495u)) + 760765357);
				continue;
			}
			goto IL_03d7;
			IL_014e:
			num = (ushort)(~(num >> 4) ^ (num % -290815051) ^ -35568);
			goto IL_0759;
			IL_0679:
			num = (ushort)(73025 + ((1659521 / -num >> num2) & ~num));
			goto IL_0691;
			IL_04eb:
			num = (ushort)(0 - (uint)(478303140 >> num % num2) / (uint)num2 - 4294929716u);
			EnterFightMax = (short)num2 - -16506;
			if (num >= -num)
			{
				num = (ushort)(~(1454207794u / (uint)(-num)) - 4294944353u);
				continue;
			}
			goto IL_0691;
			IL_0191:
			num = (ushort)(0x256F9D6A ^ (1437953084 * ((num << 20) + num) << (num ^ num) * num - ((num > 1394809786) ? 1 : 0)));
			FinishTurnMax = num + -48826;
			num2 = ((-1996045405 * num > 227735863) ? 1 : 0);
			if (~(648909004 / ~num2) != 0)
			{
				num = (ushort)(-(1646445583 - (((num2 ^ -1825376628) > (-1800688500 & num)) ? 1 : 0)) - -1646454604);
				continue;
			}
			goto IL_001b;
			IL_0691:
			ActionMin = (int)((uint)(num2 * 613349168 + 1840929847) / (uint)((num * num - num) | (num | -1070070013))) - -100;
			ActionMax = 500 + (((num2 / num - num) | 1) & (-642927462 % ~((num2 ^ num2) / (int)(4127655986u / (uint)num2))));
			if ((uint)(897846693 - num >>> 26 >> 27 >> 0) < (uint)num2)
			{
				num = (ushort)((-1882498064 | num) + 1882501273);
				continue;
			}
			goto IL_03e7;
			IL_0759:
			num2 = -43 & num;
			num = (ushort)(40893 + ((byte)((int)((uint)num2 % (uint)num2) * (num2 << 4)) >> (num2 & 0xAA)));
			continue;
			end_IL_004a:
			break;
		}
		num = (ushort)((sbyte)((byte)num2 << 16) * num - -49426);
		EnterFightMin = ((num | ((1996578868 + num) | num2)) << 3) + 1206450488;
		goto IL_03ae;
		IL_03d7:
		FinishTurnMin = 10;
		FinishTurnMax = 10;
		goto IL_03e7;
		IL_03e7:
		CastSpellMin = 10;
		CastSpellMax = 10;
		num2 = -78;
		if ((-957234359 & num2) != 0)
		{
			ActionMin = (num2 >> 13) - -11;
			num2 &= 0xBEFBFD0;
			if (((822939799 * num2 == 680272420) ? 1u : 0u) % 3868420509u / ((uint)num2 / (uint)num2) == 0)
			{
				ActionMax = (int)(0xA ^ (0x64AADF24u & ((3129020334u > (uint)num2) ? 1u : 0u)));
				num = (ushort)(89880576 / ((uint)(-num2) / (uint)num2));
				if ((((uint)(-2139075582 << num2) < (uint)(~num)) ? 1 : 0) - -634678252 > (((8 >>> num2) & 0x2F801717) ^ (num % 92579360)))
				{
					num = (ushort)(0x7A2E ^ (num >> 11));
					goto IL_004a;
				}
				goto IL_03ae;
			}
		}
		goto IL_0716;
		IL_0706:
		num = (ushort)(((num2 == 445275415) ? 1u : 0u) ^ 0xCB01u);
		goto IL_0716;
	}

	internal List<int> GetSpellEffectZone(DetailedSpellLevel spell, int castCell, int targetCell)
	{
		if (spell == null || spell.Effects == null || spell.Effects._1A183688 == null || spell.Effects._1A183688.Count() == 0 || spell.Effects._1A183688[0] == null || spell.Effects._1A183688[0].D2A3B72E == null || InstanceData?.MapInformation?.Cells == null)
		{
			return new List<int>();
		}
		CF005D89 cF005D = spell.Effects._1A183688[0].D2A3B72E;
		if (!EffectShapes.TryGetValue(cF005D.FF9F4D25, out string value) || _671BC22C.BF9F3D1F.FE8E0C9E[744](value))
		{
			return new List<int>();
		}
		List<Cell> cells = InstanceData.FighterManager.MapInformation.Cells;
		if (cells.Count == 0)
		{
			return new List<int>();
		}
		SpellZoneEffect _260CD59E = new SpellZoneEffect
		{
			ZoneSize = cF005D._12303992,
			ZoneMinSize = cF005D._48388C98,
			ZoneShape = value
		};
		Dictionary<int, Cell> dictionary = cells.ToDictionary((Cell c) => c.CellNumber);
		if (dictionary == null || dictionary.Count == 0)
		{
			return new List<int>();
		}
		Dictionary<int, _9F8EAF35> dictionary2 = D98DE637.E937108E(dictionary, castCell, targetCell, _260CD59E);
		if (dictionary2 == null || dictionary2.Count == 0)
		{
			return new List<int>();
		}
		return dictionary2.Keys.ToList();
	}

	internal double CalculateDistanceRatio(int extraParam, int castCell)
	{
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		FighterManager.ActorFighter closestFighterOfCell = default(FighterManager.ActorFighter);
		double num6 = default(double);
		FighterManager.ActorFighter closestFighterOfCell2 = default(FighterManager.ActorFighter);
		while (true)
		{
			FighterManager.ActorFighter self = FighterManager.GetSelf();
			while (true)
			{
				byte b;
				if (extraParam == 0)
				{
					b = 1;
					if ((byte)((uint)(b * ~b) % 4089203845u) == 0)
					{
						continue;
					}
					b = (byte)(48 + (((b == 1762808488) ? 1u : 0u) << (~b ^ -27973) * -215292418));
				}
				else
				{
					b = 129;
					if ((b & (b + b)) != 0)
					{
						goto IL_010d;
					}
					b = (byte)((2038512829 >>> ~(~(b ^ b))) - 2038512660);
				}
				goto IL_0049;
				IL_039d:
				if (num < ChickenDistance)
				{
					int num2 = num3;
					int num4 = num;
					b = (byte)(b - -254);
					if (num2 >= num4)
					{
						goto IL_04df;
					}
					num5 &= -69929730;
					if (num5 * 1208380810 != 0)
					{
						goto IL_015c;
					}
					b = (byte)((b | -518745952) - -518745926);
				}
				else
				{
					b = (byte)((int)((uint)b / 2080588058u + (0 - (uint)b / 84514457u)) >> b % -499181139);
					b = (byte)(((b % ~(num5 % ~num5) >> (int)b < b) ? 1u : 0u) - 4294967263u);
				}
				goto IL_0049;
				IL_04df:
				if ((uint)(157459580 << num5) < (uint)(187 * num5))
				{
					break;
				}
				b = (byte)((int)((uint)(507082758 >>> (int)(sbyte)b) % 1309960481u) % (int)(1974336 % (b * (545627428u / (uint)b))) - -47);
				goto IL_0049;
				IL_0049:
				while (true)
				{
					double num7;
					switch ((uint)b % 12u)
					{
					default:
						b = (byte)(uint.MaxValue + (uint)((sbyte)b * b * -2037658857) / (uint)(b ^ 0x548ACBB0));
						num7 = 0.0;
						goto IL_010c;
					case 1u:
						b = (byte)(2073081757 - (-708904150 & b) - 2073081588);
						num7 = (double)extraParam / 50.0;
						b -= 128;
						goto IL_010c;
					case 2u:
						break;
					case 3u:
						goto IL_01be;
					case 4u:
						goto IL_0238;
					case 5u:
						b = (byte)(884615084 * ((uint)num5 % (uint)b));
						closestFighterOfCell = GetClosestFighterOfCell(self.CellId, GetAliveEnemies());
						num5 = b;
						b = (byte)((num5 - num5 * -1642655102 << (-1509515641 << num5) * (0x3332E098 | b)) + 138);
						continue;
					case 6u:
						goto IL_0317;
					case 7u:
						goto IL_0392;
					case 8u:
						goto IL_03eb;
					case 9u:
						goto IL_0462;
					case 10u:
						goto IL_04b4;
					case 11u:
						{
							b = (byte)(-18776 + b - -18983);
							return num6;
						}
						IL_010c:
						num6 = num7;
						goto IL_010d;
					}
					break;
					IL_04b4:
					b ^= 0xD0;
					num6 += (double)ChickenDistance - ((double)num3 - 8.0 / (double)ChickenDistance);
					goto IL_04df;
					IL_0462:
					b = (byte)(((1530624296 % b << 31 >>> num5) + 1171006873) ^ 0x45CC2599);
					int num8 = num3;
					int chickenDistance = ChickenDistance;
					b += 254;
					if (num8 < chickenDistance)
					{
						if (~(b + -num5) != 0)
						{
							b = (byte)((byte)num5 - -46);
							continue;
						}
						goto IL_03fc;
					}
					goto IL_04df;
					IL_01be:
					b = (byte)(((uint)((int)((uint)(b | -1959763150) % (uint)(b + b)) / (b - b + -1072907725)) | ((uint)b / (uint)(-1365882833 & (1746988045 * b)))) - 4294967042u);
					num3 = _4CAC0493._52884196.E230AC25(castCell, closestFighterOfCell2.CellId);
					num5 = (int)((uint)b % (uint)b / b);
					if ((uint)(-(num5 - -1381352570)) > (uint)(0x3336BE19 | num5))
					{
						b = (byte)((b ^ 0x101FC7) - 1056565);
						continue;
					}
					goto IL_04df;
					IL_0238:
					b = (byte)(0 * num5 + num5 + 254);
					if (_671BC22C.BF9F3D1F.FE8E0C9E[0x2D0 ^ (((uint)(0 >> b % -1079418199) % (uint)(~(-803975782 & num5))) & 0)](FightMode, "chicken"))
					{
						b = (byte)(num5 * num5 / ~((short)(num5 >> num5) >> (int)b));
						b = (byte)(0xB9 ^ ((int)((uint)(1797926048 << num5) % 3364739092u) / -2079621089));
						continue;
					}
					goto IL_04df;
					IL_03eb:
					b = (byte)(0x3816CA1A ^ (0x3816CAA0 | b));
					goto IL_03fc;
					IL_03fc:
					num6 += (double)ChickenDistance - ((double)num3 - 8.0 / (double)ChickenDistance);
					goto IL_04df;
					IL_0317:
					b = (byte)(-1890690133 ^ (-1890690271 ^ b));
					num = _4CAC0493._52884196.E230AC25(self.CellId, closestFighterOfCell.CellId);
					if ((int)((uint)b / 2391058568u) / ~((b & num5) % ~num5) << ~num5 == 0)
					{
						b = (byte)(0x2B ^ (((int)((uint)b / 4128450708u + b) - (num5 >>> (num5 ^ -1246853328))) * (num5 >> b % ~num5 - -128448409)));
						continue;
					}
					goto IL_04df;
				}
				b ^= 0x27;
				goto IL_015c;
				IL_015c:
				if (closestFighterOfCell2 == null)
				{
					return num6;
				}
				b = (byte)((uint)(~b) >> (int)(69215017u % (uint)b));
				if ((byte)((b ^ 0x272BF01D) % ~(byte)(b >>> (int)b)) != 0)
				{
					continue;
				}
				b = (byte)((((uint)((-240405699 + (b - 161787454)) % (int)(~((uint)b / 1880557998u))) < (uint)(-835213633 + b)) ? 1u : 0u) ^ 0x26u);
				goto IL_0049;
				IL_010d:
				closestFighterOfCell2 = GetClosestFighterOfCell(castCell, GetAliveEnemies());
				if ((uint)(731354411 * b * (-1574512215 + b) - -1591383283) <= (uint)(-b))
				{
					b = (byte)(~(-(-952991312 + b)) - -952991350);
					goto IL_0049;
				}
				goto IL_039d;
				IL_0392:
				b = (byte)(num5 & 0x3A9F9E19);
				goto IL_039d;
			}
		}
	}

	internal List<FighterManager.ActorFighter> GetAlliesInZone(List<int> zone, bool flag)
	{
		_003C_003Ec__DisplayClass231_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass231_0();
		CS_0024_003C_003E8__locals5.mineChar = FighterManager.GetSelf();
		uint num = default(uint);
		bool flag2 = default(bool);
		List<FighterManager.ActorFighter> fightersFromTeamInZone = default(List<FighterManager.ActorFighter>);
		byte b2 = default(byte);
		int team = default(int);
		while (true)
		{
			int? teamId = TeamId;
			sbyte b = -15;
			b = (sbyte)((-1554923678 ^ (b >>> 20)) + 1554927486);
			while (true)
			{
				int num3;
				switch ((uint)b % 9u)
				{
				default:
					b = (sbyte)((int)(1255297489u / (uint)(-1819338583 * b)) + (int)b - 42);
					if (!teamId.HasValue)
					{
						num3 = (int)((uint)b % 3131910409u % 1049719452) * (1721449615 >> 691751228 + b >>> (int)((uint)b / 3069251665u)) - -1808267248;
						goto IL_010e;
					}
					teamId = TeamId;
					num = ~((2097131119 > b) ? 1u : 0u) | (uint)(b | (b % (b & b)));
					b = (sbyte)((int)((uint)(1401665669 * ((1553449495 + b) * (int)num)) | num) ^ -47);
					continue;
				case 1u:
					b = (sbyte)((b ^ -1) - -32);
					num3 = teamId.Value;
					goto IL_010e;
				case 2u:
					b = (sbyte)(0x52552656 ^ b ^ -1381312127);
					goto IL_017e;
				case 3u:
					b = (sbyte)(-462283141 ^ ((int)num - -462283145));
					if (flag2)
					{
						if ((uint)(-257602375 >> (int)((num / 4139591590u) | 0x3167DB8)) >= ((1350825244 % num % (uint)((b << (int)num) % -1975868500)) & 0xE289659Du))
						{
							b = (sbyte)(-303996870 ^ (int)((0xF12B82A2u & num) - (0x121E9F87 ^ (num / 1308857994))));
							continue;
						}
						goto IL_017e;
					}
					goto IL_02b9;
				case 4u:
					b = (sbyte)(-15 ^ (int)((uint)(1736429538 % (((int)((uint)b % 805916731u) > (int)(num << (int)num)) ? 1 : 0)) ^ (((int)(~((-376133216 > (int)num) ? 1u : 0u)) > (int)num % -961385343 << 13) ? 1u : 0u)));
					if (flag)
					{
						goto IL_02b9;
					}
					if ((0x60004B02 ^ (1771906737u % (uint)b - 2049459842)) != 0)
					{
						b = (sbyte)(65 + -b * (int)(num / 3408162579u - num));
						continue;
					}
					goto IL_03d2;
				case 5u:
					b = (sbyte)(-2039289143 - ((ushort)num | 0x539A76B3) + -853046309);
					fightersFromTeamInZone.RemoveAll((FighterManager.ActorFighter f) => f.ActorId == CS_0024_003C_003E8__locals5.mineChar.ActorId);
					goto IL_03d2;
				case 6u:
					b = (sbyte)((sbyte)(551545600u / (uint)(~((int)num >> 20 << 10))) - 15);
					if ((flag2 ? 1 : 0) == 1 + ((int)b / ((1587235608u < (uint)b) ? 1 : 0) >> b % ~(b2 >> 22) >> (int)b2) && flag)
					{
						num ^= 0xFFFE;
						continue;
					}
					goto IL_03d2;
				case 7u:
				{
					bool num2 = fightersFromTeamInZone.Any((FighterManager.ActorFighter f) => f.ActorId == CS_0024_003C_003E8__locals5.mineChar.ActorId);
					num ^= 0xFFFE;
					if (!num2)
					{
						if (((0 - num >> (int)num + -1382407011) & 0x6E0BA981) / ~((uint)(b2 << (int)b << (int)b) / 839258129u) <= ((uint)((866812600 >>> (int)(0 - num)) * ((b2 - -2062811602) | b2)) ^ num))
						{
							b = (sbyte)((b2 << (int)b2) + -2071986159);
							continue;
						}
						break;
					}
					goto IL_03d2;
				}
				case 8u:
					{
						b = (sbyte)(((byte)num << (b2 | 0x2198)) - -2147483633);
						fightersFromTeamInZone.Add(CS_0024_003C_003E8__locals5.mineChar);
						goto IL_03d2;
					}
					IL_02b9:
					b2 = (byte)(0x49AE4F36 | b | 0);
					b = (sbyte)(-16 ^ (int)(~(1136024080 * ((uint)b % (uint)b >> 3))));
					continue;
					IL_017e:
					fightersFromTeamInZone = GetFightersFromTeamInZone(zone, team);
					flag2 = zone.Contains(CS_0024_003C_003E8__locals5.mineChar.CellId);
					if (num != 0)
					{
						b = (sbyte)(-17 ^ b);
						continue;
					}
					goto IL_03d2;
					IL_010e:
					team = num3;
					num = ((((85202576 < (int)(4279698105u / (uint)b)) ? 1 : 0) < 13) ? 1u : 0u);
					if (1656496898 / ~((uint)b / (uint)(~((int)b % (int)num >> (int)b))) >= (uint)((int)(3935898666u / num) * ((int)num >> b * -2020582854 % 381327787)))
					{
						b = (sbyte)(-41 ^ (b << (b & 0)));
						continue;
					}
					break;
					IL_03d2:
					return fightersFromTeamInZone;
				}
				break;
			}
		}
	}

	internal List<FighterManager.ActorFighter> GetEnemiesInZone(List<int> zone)
	{
		int? teamId = TeamId;
		short num = default(short);
		if (teamId.HasValue)
		{
			num = -8230;
			teamId = TeamId;
			uint num2 = (uint)(num | ((short)(num / -1929108605) | (-759138106 / -num)));
			goto IL_0034;
		}
		goto IL_0040;
		IL_0040:
		int num3 = 1737763;
		int num4 = (num3 >>> (num3 >>> 24)) - 1737763;
		goto IL_0073;
		IL_0034:
		if (teamId.Value != 0)
		{
			goto IL_0040;
		}
		num4 = num + 703510866 - 703502635;
		num3 = 1737763;
		goto IL_0073;
		IL_0073:
		int team = num4;
		if (((num3 - 754653691) ^ 0x1F798D3) != 0)
		{
			return GetFightersFromTeamInZone(zone, team);
		}
		goto IL_0034;
	}

	internal List<FighterManager.ActorFighter> GetFightersFromTeamInZone(List<int> zone, int team)
	{
		_003C_003Ec__DisplayClass233_0 CS_0024_003C_003E8__locals3;
		while (true)
		{
			CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass233_0();
			CS_0024_003C_003E8__locals3.zone = zone;
			if (team == 0)
			{
				int num = 0;
				if ((int)(((((uint)num > 923797273u) ? 1u : 0u) << 6) % 437855320) >> (int)((uint)(num | 0x1F9B363E) / (uint)(~(num % ~num % ~num))) != 0)
				{
					continue;
				}
			}
			else
			{
				int num = -715777561;
				if (((-1977401596 % (num | num)) | -176702814) >>> -(227658665 * num - num * -577598812) >= ((7366 >>> (-82164080 ^ (num + num)) == -861907907) ? 1 : 0))
				{
					break;
				}
			}
			return FighterManager.Defenders.Where((FighterManager.Defender f) => CS_0024_003C_003E8__locals3.zone.Contains(f.CellId) && f.IsAlive).Cast<FighterManager.ActorFighter>().ToList();
		}
		return FighterManager.Attackers.Where((FighterManager.Attacker f) => CS_0024_003C_003E8__locals3.zone.Contains(f.CellId) && f.IsAlive).Cast<FighterManager.ActorFighter>().ToList();
	}

	internal FighterManager.ActorFighter GetClosestFighterOfCell(int cellId, List<FighterManager.ActorFighter> fighters)
	{
		short num = 20629;
		FighterManager.ActorFighter actorFighter = null;
		int num3;
		ushort num2;
		do
		{
			num2 = (ushort)num;
			num3 = (int)(((num > 1945952838) ? 1u : 0u) >> (int)((uint)num2 / 2023225750u)) - -2147483647;
			num2 = (ushort)(-819317607 * (-1551806403 >>> (int)num) - 1947048632);
		}
		while (-1834626298 % (int)(((num >>> (int)num > 5410438) ? 1u : 0u) | ((uint)num2 % 4164934168u)) == 0);
		using (List<FighterManager.ActorFighter>.Enumerator enumerator = fighters.GetEnumerator())
		{
			if (num2 >> (num2 >> num % num2) == 0)
			{
				goto IL_00c6;
			}
			goto IL_0167;
			IL_0167:
			byte b = (((0x185B506 | (num - num2)) > (int)(((num < -1212227665) ? 1u : 0u) | 0x9E4677E3u)) ? ((byte)1) : ((byte)0));
			FighterManager.ActorFighter current = default(FighterManager.ActorFighter);
			if (enumerator.MoveNext())
			{
				b = 161;
				current = enumerator.Current;
				b = (byte)(195077812 >> -b % (int)(~((uint)b % 3204909456u % b)));
				if (b >> -b <= 947089503)
				{
					goto IL_00c6;
				}
				goto IL_0104;
			}
			goto end_IL_006a;
			IL_0104:
			b = (byte)((1085990040u % (uint)b) | (uint)(-1181884897 + (0x7C36C11D & b)) | 0xBB220000u);
			int num4 = default(int);
			if ((((-1884797013 < 1378256033 >>> (int)b) ? 1u : 0u) ^ (uint)b) != 0)
			{
				actorFighter = current;
				if (1208304777u / (uint)b > (uint)(230788281 * b))
				{
					goto IL_00c6;
				}
				num3 = num4;
				num2 = 8964;
				num = 20629;
			}
			goto IL_0167;
			IL_00c6:
			num4 = _4CAC0493._52884196.E230AC25(cellId, current.CellId);
			b = (byte)(~(b % ((int)(3350729637u / (uint)b) % -172308607)));
			if (actorFighter != null)
			{
				int num5 = num4;
				int num6 = num3;
				num = 20629;
				num2 = 8964;
				if (num5 >= num6)
				{
					goto IL_0167;
				}
			}
			goto IL_0104;
			end_IL_006a:;
		}
		num = 9226;
		return actorFighter;
	}

	internal List<FighterManager.ActorFighter> GetAliveEnemies()
	{
		return FighterManager.Defenders.Where((FighterManager.Defender f) => f.IsAlive && !f.IsSummon).Cast<FighterManager.ActorFighter>().ToList();
	}

	internal bool IsAffectedByTargetMask(string effectMask, string fighterMask)
	{
		char _1E1568BE = default(char);
		short num4 = default(short);
		while (true)
		{
			uint num = 4274683741u;
			int num2 = (int)(2 + (num ^ (0 - num)));
			while ((1 ^ num) != 0)
			{
				if (num2 < _671BC22C.BF9F3D1F.FE8E0C9E[(int)(((-356757488 == (int)(0x7B3B5314 ^ (598951228 % num))) ? 1u : 0u) & num) - -69](effectMask))
				{
					uint num3 = 2753466047u;
					num3 = (0x6639E7AB ^ num3) / num3 - 3418717283u;
					while (true)
					{
						switch (num3 % 3)
						{
						case 1u:
							num3 = 0x7E93E49A ^ num3;
							if (_671BC22C.BF9F3D1F.FE8E0C9E[1541502976 + (num3 | ((0x46BA2638 & num3) % num3))](fighterMask, _1E1568BE))
							{
								return (byte)(0xA41E8EBEu ^ num3) != 0;
							}
							num4 = (short)(num3 ^ 0x43C66FE);
							continue;
						case 2u:
							goto end_IL_002e;
						}
						num3 = 0x90240D21u ^ num3;
						_1E1568BE = _671BC22C.BF9F3D1F.FE8E0C9E[(num3 ^ 0x9B2E78BAu) - 1060171246](effectMask, num2);
						if (0 - ~num3 >= (uint)((int)num3 / 1519775160 >>> 26 >>> ((int)num3 >> (int)num3)))
						{
							num3 = (uint)((1956316715 >>> (0xAD4BAFD ^ (-1384002652 - (int)num3))) ^ -628266415);
							continue;
						}
						goto IL_013d;
						continue;
						end_IL_002e:
						break;
					}
					num2 += (int)((uint)(-961907277 | num4) / (uint)((int)num4 % (int)num3));
					num = 4274683741u;
					continue;
				}
				goto IL_013d;
				IL_013d:
				return (byte)(286366251 + (0x10241A88 ^ num)) != 0;
			}
		}
	}

	internal string GetFighterTargetMasks(FighterManager.ActorFighter fighter)
	{
		bool flag = fighter.TeamId == (TeamId.HasValue ? TeamId.Value : 0);
		if (flag)
		{
			goto IL_0042;
		}
		object obj = "A";
		goto IL_00af;
		IL_004e:
		short num2 = default(short);
		string text = default(string);
		byte b = default(byte);
		int num;
		while (true)
		{
			switch ((uint)num % 11u)
			{
			case 1u:
				num = -2067092779 + (0xB32C694 | (num % (num / (num * -1971157708))));
				goto IL_00e1;
			case 2u:
				num = -2067082736 + (-49 & num2);
				if (fighter.IsSummon)
				{
					goto IL_012e;
				}
				num = num2 | 0x13C5D5D2;
				if ((uint)(0 >>> (num2 >>> 0) + (0x3D2325CD | num)) > ((1025746939u < (uint)num) ? 1u : 0u))
				{
					goto IL_04ce;
				}
				num = (byte)(-(num % -569694072)) ^ -32261504;
				continue;
			case 3u:
				num = (int)((uint)((num >>> (int)num2 >>> -524628033 * num2) | num2) / 243982621u - 8731);
				if (fighter.ActorId == FighterManager.GetSelf().ActorId)
				{
					num = -(num % 421839931 - (num >>> 18));
					if ((num ^ 0) / (int)(~((uint)(0 * (num2 - -2121769806)) % (uint)num2)) != 0)
					{
						num = (int)((uint)(num2 - 0) / (uint)(~(~num2 / (num << (int)num2))) - 676024690);
						continue;
					}
					goto IL_048c;
				}
				num2 = (short)((int)(1061433772u / (uint)(num2 - 580458172)) % ((sbyte)num2 << 5 >> num % num));
				num = (-1607213423 & num) ^ 0x5096D921;
				continue;
			case 4u:
				goto IL_020c;
			case 5u:
				num = -1449570867 + (-617521913 ^ ((int)((uint)num / 3037131398u) >> 24 << (0x552B4029 | (69578499 + num2))));
				goto IL_04ce;
			case 6u:
				num = (num2 + 1585780532) ^ -1585788222;
				if (fighter.ActorId <= num2 << (((uint)(num2 & -519555277) > 773372815u) ? 1 : 0) >> (int)((uint)(-101 >> (num | -636580968)) % (uint)(num - (num2 - num))))
				{
					goto IL_03d6;
				}
				num = num;
				if (-2 >> (num2 << (num & num2)) == 0)
				{
					goto IL_04ce;
				}
				num = -844230264 ^ (-281306194 & num);
				continue;
			case 7u:
				num = -8714 + num2;
				text = _1C1E6834._86854E2F(text, flag ? "h" : "H");
				num2 ^= -9996;
				num ^= 0x7B357722;
				if (num2 + -871715287 > ~((-182668275 ^ num2) | num) * num2)
				{
					num = -407848058 * num - 1575489643;
					continue;
				}
				goto IL_012e;
			case 8u:
				num = -2067092779 + (num2 >> 15);
				goto IL_04ce;
			case 9u:
				num = -211 + (int)((uint)(1954701754 + (num << 14 << 23)) % (uint)(-172297317 >>> ~num));
				goto IL_048c;
			case 10u:
				{
					num = (int)((uint)(num2 >> 14) / (uint)(~(228268037 << num << (num >> 6)))) ^ -2558975;
					return text;
				}
				IL_048c:
				text = _671BC22C.BF9F3D1F.FE8E0C9E[0x705 ^ ((197085696 + b * num) * (b << num) % ~b)](text, "X");
				num2 ^= -9996;
				num += -2067092780;
				goto IL_04ce;
				IL_00e1:
				_ = fighter.IsSummon;
				num2 = (short)(-1204277728 + num);
				if (num2 != 0)
				{
					num = (0 << (num2 ^ 0x40C2AAD3) >>> num) + -837769451;
					continue;
				}
				goto IL_04ce;
				IL_04ce:
				num = num2 << (((num + num) % num) & (num2 >>> -num));
				if (~(num2 / ~(num % num)) != 0)
				{
					num = (int)(0x39A64812 ^ ((uint)(-38500203 << (488264715 >>> -num2)) / (uint)(1460734230 >> (int)num2)));
					continue;
				}
				goto IL_00e1;
				IL_012e:
				text = _1C1E6834._86854E2F(text, flag ? "i" : "I");
				goto IL_04ce;
			}
			break;
			IL_03d6:
			num = (int)num2 ^ (((uint)num2 > (uint)(num2 + 1101375637)) ? 1 : 0);
			if ((int)((uint)(1342489279 + (num | num)) / (uint)(~num2)) <= 128153861 % (((num > -920070480) ? 1 : 0) >> (int)num2))
			{
				b = (byte)((uint)(453206145 + num2) / (uint)(~(num * 756329399 << (int)num2)) * (uint)(339012784 / (int)(~((uint)num / (uint)(~num2))) / 1529039105));
				num = -73306613 ^ ((((int)((uint)num % (uint)(~(num2 + num))) > 1848821946) ? 1 : 0) / ~((554951452 << num2 - 1501820589) & -b));
				continue;
			}
			goto IL_0224;
			IL_0224:
			text = _671BC22C.BF9F3D1F.FE8E0C9E[num * -1000 + 25098797](text, "cC");
			num -= 2067117877;
			if ((uint)(num2 << num) + (((uint)(num2 / -1833477953) < (uint)num2) ? 1u : 0u) % (uint)(~(num2 / -1875182795)) != 0)
			{
				num = 0x6425B107 ^ ((num2 + (-num & -802164419)) * (int)((uint)num2 / (uint)num2));
				continue;
			}
			goto IL_0042;
			IL_020c:
			num = ((-3 & num) + (int)(696563721u % (uint)(num / num))) * num2 - 1559239527;
			goto IL_0224;
		}
		num = (2005515838 / (num << 26) * 1856775052 >> ((-1851393261 >>> num) ^ num)) - -234446921;
		obj = "a";
		goto IL_00af;
		IL_00af:
		text = (string)obj;
		num = -2067092780;
		num = (0x2E127BA1 & num) - 895850085;
		goto IL_004e;
		IL_0042:
		num = 63;
		num ^= 0x826B5BC;
		goto IL_004e;
	}

	internal double GetHealEffectOnFighter(EffectInstance effect, FighterManager.ActorFighter target)
	{
		double num = (double)(effect.DiceNum + effect.DiceSide) / 2.0;
		FighterManager.Attacker self = FighterManager.GetSelf();
		return num * (double)(100 + self.Characteristics[CharacteristicKeyword.Intelligence].Value) / 100.0 + (double)self.Characteristics[CharacteristicKeyword.HealBonus].Value;
	}

	internal double GetDamageEffectOnFighter(EffectInstance effect, FighterManager.ActorFighter target)
	{
		double num = (double)(effect.DiceNum + effect.DiceSide) / 2.0;
		FightCharacteristics characteristics = FighterManager.GetSelf().Characteristics;
		double num2 = 0.0;
		sbyte b = -91;
		if (0 - ((1686943236u % (uint)b) & (uint)b) < (uint)b)
		{
			b = (sbyte)(0xCA3C ^ (51848 + b));
			goto IL_005e;
		}
		goto IL_093e;
		IL_02ca:
		ushort num3 = default(ushort);
		sbyte b2 = default(sbyte);
		int value = default(int);
		num2 = num * (double)(-26894 + ((short)num3 + b2 % -317276230) + characteristics[(CharacteristicKeyword)(b2 - -4)].Value + value) / 100.0 + (double)characteristics[(CharacteristicKeyword)((int)((uint)(((b2 + b) ^ 0x41124625) + -1072828381 / (-215743042 >> (int)b2)) % (uint)(-1107291246 ^ (num3 >>> 12))) + -15574450)].Value;
		goto IL_0339;
		IL_005e:
		sbyte b3 = default(sbyte);
		while (true)
		{
			switch ((uint)b % 17u)
			{
			case 1u:
				goto IL_0177;
			case 2u:
				goto IL_01f2;
			case 3u:
				goto IL_0251;
			case 4u:
				goto end_IL_005e;
			case 5u:
				b = (sbyte)((((uint)b > (uint)(-4200001 << (int)b2)) ? 1 : 0) / 29526 + -91);
				goto IL_0568;
			case 6u:
				goto IL_0403;
			case 7u:
				b = (sbyte)((uint)(b2 >> 27 >>> (((uint)b / (uint)b2 == (uint)(-860478186 ^ b2)) ? 1 : 0)) / (uint)(~(~b2 % ~(b - b) % ~b2)) - 92);
				num2 = num * (double)((((1 > (-1566226391 | b)) ? 1 : 0) >> ~b2) - -100 + characteristics[(CharacteristicKeyword)(((b2 ^ 0x72B32582 ^ 0x5B9BE110) | -1279489466) % (537308315 % ~b) + 27)].Value + value) / 100.0 + (double)characteristics[(CharacteristicKeyword)(b % -650962130 - -164)].Value;
				if ((int)((uint)(b2 + b - b2) | ((uint)b % 3643704746u)) > (0 & b))
				{
					goto IL_0568;
				}
				b = (sbyte)(b % b2 / (-1685177329 / (int)(~((uint)b / 2776968224u))) - -25);
				continue;
			case 8u:
				b = (sbyte)((-1290767436 / (int)((uint)b2 % (uint)b * 1941487285) << (((int)((uint)b2 % 1939495858u) > -1104064886 << (-1170852086 >> (int)b2)) ? 1 : 0)) - 91);
				goto IL_0568;
			case 9u:
				goto IL_05cc;
			case 10u:
				goto IL_062f;
			case 11u:
				goto IL_068a;
			case 12u:
				goto IL_073c;
			case 13u:
				b = (sbyte)((uint)((int)((uint)num3 / 850996514u) * (num3 >>> 1) / (b2 << num3 - 69817003)) % 1954913582u - 91);
				goto IL_093e;
			case 14u:
			{
				b = (sbyte)((uint)b / (uint)(~(((b2 < -241214718) ? 1 : 0) * 1376385712 >>> (int)b)));
				bool num4 = NeutralEffects.Contains(effect.EffectId);
				b += -91;
				if (num4)
				{
					b3 = (sbyte)(b2 >> (int)(ushort)b);
					b = (sbyte)((int)((((uint)(b ^ 0x33AB4016) > (uint)b2) ? 1u : 0u) << (int)b2) + (b ^ b2) % -116154839 - -139);
					continue;
				}
				goto IL_093e;
			}
			case 15u:
				goto IL_0899;
			case 16u:
				{
					b = (sbyte)(b % (0x219E7BE & b2) - 124);
					return _671BC22C.BF9F3D1F.FE8E0C9E[((b + -1760940913 * b2) * b2) ^ -44508878](0.0, num2);
				}
				IL_0568:
				num2 = ApplyFighterResistance(num2, target, "air");
				if ((uint)(sbyte)((uint)b % (uint)(-412835874 + -b)) == (0xFAA47481u | ((b2 == (b & b2)) ? 1u : 0u)) / (uint)b2)
				{
					b2 = (sbyte)(~(0x238E9D2Du | (((int)(279727545u % (uint)b2) < 1136274099) ? 1u : 0u)));
					b = (sbyte)(16828228 * b * -1376218248 - 1271123748);
					continue;
				}
				goto IL_093e;
			}
			b = (sbyte)(-108 + b);
			value = characteristics[(CharacteristicKeyword)(110 + b)].Value;
			goto IL_00c8;
			IL_0899:
			b = (sbyte)(((958392493 - (0x3B | b3) > -1045921511) ? 1 : 0) ^ -92);
			goto IL_08b1;
			IL_073c:
			b = (sbyte)((uint)(num3 ^ 0x1A32A1C) % (uint)(~(num3 >>> ((-1920434121 > num3) ? 1 : 0))) - 27470364);
			goto IL_075b;
			IL_068a:
			b = (sbyte)(b2 % -b2 >> (-660614869 >>> (((uint)b2 > 96174215u) ? 1 : 0)));
			num2 = num * (double)(-b - -100 + characteristics[(CharacteristicKeyword)(9 + b2)].Value + value) / 100.0 + (double)characteristics[(CharacteristicKeyword)(1328982888 + ~(0x4F36AB20 & (~b % 1638916413)))].Value;
			num3 = (ushort)(2317363977u / (uint)(~((b2 / -759616719) | b)) << 8);
			if (num3 < (0x558A | b2))
			{
				b = (sbyte)(0xCD414FD ^ (215225538 >>> 0 % (b | (-561768547 | b2))));
				continue;
			}
			goto IL_00c8;
			IL_075b:
			do
			{
				num2 = ApplyFighterResistance(num2, target, "fire");
				b ^= -91;
			}
			while (~b % -1651 == 0);
			b = (sbyte)((int)((uint)(((0x5C8D5182 ^ b2) - -2112282831) * -b) % (uint)(~(b2 >> (-1256794447 ^ num3) / b))) - -1488067849);
			continue;
			IL_062f:
			b = (sbyte)((ushort)(277589636 >> ~b) - 60321);
			if (FireEffects.Contains(effect.EffectId) && (uint)b2 < (uint)(1612791825 * b2))
			{
				b = (sbyte)(45 + (b >>> ((b2 * b) ^ 0x1A96FD0F)) / 1933366969);
				continue;
			}
			b3 = (sbyte)(b | 0x6EBDB79B);
			if ((0x1C4EA79 | b) != 0)
			{
				b = (sbyte)(0x33CE0434 ^ ((((1511217343 >> (int)b3) / ~(b & -1760643313)) & (b3 >>> 22)) - (((ushort)b3 >>> (int)b) | -869179862)));
				continue;
			}
			goto IL_03b9;
			IL_08b1:
			num2 = num * (double)(-67 + (byte)(((byte)b2 >>> (int)b) | -1324580441) + characteristics[(CharacteristicKeyword)(((ushort)(b - (b - b2)) ^ (-717444072 ^ ((1275774743 - b3) ^ b3))) + 1724486903)].Value + value) / 100.0 + (double)characteristics[(CharacteristicKeyword)(-(918411556 % (int)((1454053909u % (uint)(~b3)) ^ 0x9CBB669Fu)) - -13522424)].Value;
			goto IL_092e;
			IL_01f2:
			b = (sbyte)(-1701410171 * (ushort)b - 19574244);
			goto IL_093e;
			IL_03b9:
			if ((uint)(-1811829974 + b) < (uint)((-1531037639 | b2) + -1665895034))
			{
				goto IL_093e;
			}
			b = (sbyte)((((631325463 < (((uint)b % 3306619057u) ^ 0xB112C480u)) ? 1u : 0u) << b % (short)(b - -1146735345)) ^ 8);
			continue;
			IL_0177:
			b = (sbyte)((0 * ((b * b) | (b << -1103740141 / b))) ^ -91);
			num2 = ApplyFighterResistance(num2, target, "water");
			if ((((uint)(~b) < (uint)b) ? 1 : 0) - (b + b) == 0)
			{
				b2 = (sbyte)(-1204260576 >>> (-1895633279 >>> (-1684952823 & b) * ((b < -595241974) ? 1 : 0)));
				b = (sbyte)((int)(68270729u / (uint)(-b)) + -750174);
				continue;
			}
			goto IL_093e;
			IL_05cc:
			b = (sbyte)(-91 ^ ((b / -1902823623) & (b * (0x7C2B7312 & (b * 2075090691)))));
			goto IL_092e;
			IL_0403:
			b = (sbyte)((b & (b2 << 28)) % (int)(~((2075775919 == (b2 ^ b)) ? 1u : 0u)) * -1905401275 + -91);
			if (AirEffects.Contains(effect.EffectId))
			{
				b2 = (sbyte)(-1110052544 + b);
				if (b2 != (b2 & 1))
				{
					b = (sbyte)(b2 - 2067777316 - -2067777401);
					continue;
				}
				goto IL_093e;
			}
			b = (sbyte)((b | -2010646865) * (b2 >> 6));
			if (b2 >= (sbyte)((int)b % (int)(~(((b < b) ? 1u : 0u) >> (int)b))))
			{
				b = (sbyte)(61 + ((~b == (b & b)) ? 1 : 0) % (int)b2);
				continue;
			}
			goto IL_075b;
			IL_092e:
			num2 = ApplyFighterResistance(num2, target, "neutral");
			goto IL_093e;
			IL_00c8:
			if (WaterEffects.Contains(effect.EffectId))
			{
				num2 = num * (double)(b + 191 + characteristics[(CharacteristicKeyword)(((ushort)b + b) ^ 0xFF42)].Value + value) / 100.0 + (double)characteristics[(CharacteristicKeyword)(~b - 18)].Value;
				if (1545803130 >>> ((3475171876u < (uint)(b >>> (int)b)) ? 1 : 0) > 1225203226 / (b - b + 856579002) << 15)
				{
					b = (sbyte)((((uint)b > 4047978662u) ? 1 : 0) % (int)(~(239913 + (uint)b % (uint)b)));
					continue;
				}
				goto IL_0339;
			}
			b2 = (sbyte)(b / b);
			if ((uint)(766608061 << 75904 % b2) > (uint)(-b))
			{
				b = (sbyte)(-1836483359 + -(-1836483271 + b >>> (689429 >>> -b2)));
				continue;
			}
			goto IL_08b1;
			IL_0251:
			b = (sbyte)((int)((uint)(b & (-1292361788 << (int)b2)) % 214958891u) ^ -91);
			if (EarthEffects.Contains(effect.EffectId))
			{
				num3 = (ushort)(-(-893702881 >> (int)b2));
				if ((((uint)b % 1050241845u) & (uint)(1788013224 << (int)b2)) - 703556562 != 0)
				{
					b = (sbyte)(55 + (uint)b2 / (uint)b % num3);
					continue;
				}
			}
			goto IL_03b9;
			continue;
			end_IL_005e:
			break;
		}
		b = (sbyte)(-91 + (int)((uint)num3 % (uint)b2 >> (int)b2));
		goto IL_02ca;
		IL_093e:
		num3 = (ushort)(648274049 % ~((uint)(-929859694 >> (int)b) % (uint)b));
		b2 = (sbyte)((b >> 19 >> 23 >> (num3 << (int)num3)) ^ (1619652767 / b));
		if ((uint)(0 - (((sbyte)(0x2C045B3F & b2) < b - 1245169474) ? 1 : 0)) <= (uint)(-((b2 + b) * -b)))
		{
			b = (sbyte)(0 + b + 124);
			goto IL_005e;
		}
		goto IL_02ca;
		IL_0339:
		num2 = ApplyFighterResistance(num2, target, "earth");
		if ((uint)(1878511526 >>> (int)((uint)(b2 << (int)((uint)b / 1765464624u)) / (uint)(b + -189392465 >>> 0))) > ~((uint)(b2 % -913307458 >> (b2 | b)) % 402074945u))
		{
			num3 = (ushort)(~num3);
			b = (sbyte)((0x640AD1 & b2) + 4);
			goto IL_005e;
		}
		goto IL_093e;
	}

	internal double GetPushEffectOnFighter(EffectInstance effect, FighterManager.ActorFighter target, int castCell)
	{
		return 10.0;
	}

	internal double GetBoostEffectOnFighter(EffectInstance effect, FighterManager.ActorFighter target)
	{
		ushort num = 55216;
		return 1.0;
	}

	internal double ApplyFighterResistance(double damage, FighterManager.ActorFighter target, string element)
	{
		byte b2 = default(byte);
		byte b = default(byte);
		while (true)
		{
			double num = 0.0;
			while (true)
			{
				IL_000a:
				double num2 = 0.0;
				while (true)
				{
					IL_0014:
					FightCharacteristics characteristics = target.Characteristics;
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[720](element, "water"))
					{
						b = 246;
						b = (byte)(-b + 1915526411 + b - ~b + -1915526488);
						goto IL_0056;
					}
					b2 = 184;
					if (((-1607234628 - 680193691 % b2 / 1134738203) | b2) != 0)
					{
						num = characteristics[(CharacteristicKeyword)((int)(((b2 < b2) ? 1u : 0u) >> (int)((uint)b2 | ((b2 < b2) ? 1u : 0u))) - -49)].Value;
						goto IL_03a4;
					}
					goto IL_0643;
					IL_070b:
					num2 = characteristics[(CharacteristicKeyword)(0x93 ^ (byte)(-(-100061308 ^ b)))].Value;
					b ^= 1;
					b2 = 0;
					if ((((b ^ b2) % -1626807539) ^ ((int)((uint)b2 / (uint)(~b2)) % -558086142)) != 0)
					{
						b = (byte)(0xA5 ^ ((int)((uint)b2 / 4159110476u) * -499849597));
						goto IL_0056;
					}
					goto IL_086b;
					IL_0901:
					if ((0x40502746 & b) > ~b2)
					{
						b = (byte)((~b2 % ~b2 >>> (-2039831438 << -467333116 + b2) - b2) + 203);
						goto IL_0056;
					}
					goto IL_0504;
					IL_086b:
					b2 = (byte)(b2 - (b ^ (621807006 * b2)) << 3);
					if ((int)((uint)(b2 * b) & (((uint)b2 < (uint)b) ? 1u : 0u)) >> -(~(b2 >>> (int)b)) != 0)
					{
						goto IL_0901;
					}
					b = (byte)(-80 ^ (~b % -960689354));
					goto IL_0056;
					IL_0643:
					b = (byte)(~(-392585208 & (-752017399 + (b << 9))));
					b = (byte)(0xEC ^ ((uint)b % uint.MaxValue % (uint)(~(-97 & ((b >>> (int)b) & (b << (int)b))))));
					goto IL_0056;
					IL_0056:
					while (true)
					{
						switch ((uint)b % 17u)
						{
						case 1u:
							goto IL_0125;
						case 2u:
							goto IL_01e6;
						case 3u:
							goto IL_02af;
						case 4u:
							goto IL_0346;
						case 5u:
							goto IL_042e;
						case 6u:
							goto IL_0495;
						case 7u:
							goto IL_0559;
						case 8u:
							goto IL_05cd;
						case 9u:
							goto IL_062d;
						case 10u:
							goto IL_0681;
						case 11u:
							goto end_IL_0056;
						case 12u:
							goto IL_076d;
						case 13u:
							b = (byte)(0xE8 ^ (b % (b2 % b / 1554413954 + b2)));
							num2 = characteristics[(CharacteristicKeyword)(((uint)b % (uint)(b2 * (b2 >>> (int)((uint)b / (uint)b2)))) ^ 0xE9)].Value;
							b2 -= 246;
							b = (byte)(201 + (uint)(0x5933E333 ^ (b2 % -734368977)) / ~((uint)(b * 403428925 << (int)b2) / 1734818762u));
							continue;
						case 14u:
							goto IL_084e;
						case 15u:
							goto IL_08b2;
						case 16u:
							b = (byte)((0x7D9702BB | ((uint)(short)(1141956644 * b2) % (uint)b)) - 2107048389);
							return damage - num - num2 / 100.0 * (damage - num);
						}
						b = (byte)((((uint)(-((b + b) / b)) > ((b >> 16 < 884238624) ? 1u : 0u)) ? 1u : 0u) - 4294967051u);
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[720 + b % -1 * (~(b & b) << (int)b)](element, "fire"))
						{
							if (b - -737350781 != 0)
							{
								b = (byte)((0 % (int)((uint)((-702094405 << (int)b) * (-1272055500 * b)) / 1995813894u)) ^ 1);
								continue;
							}
							goto IL_0504;
						}
						b = (byte)(((689557136 - b) & 0x61BD58C) >> (~b | (b >>> (b & 0xC0D57BC))));
						if ((uint)b / 2393409168u == 0)
						{
							b = (byte)((((2442813326u < (uint)(b * (b % b))) ? 1 : 0) >> 9) + 6);
							continue;
						}
						goto IL_0901;
						IL_08b2:
						b = (byte)(0x9112 ^ ((-1847266085 | b2) >>> (b2 - (b2 << 20)) * (-86540132 | b)));
						num = characteristics[(CharacteristicKeyword)((int)(0 % ((uint)(-820480763 + b2) / 1034922294u)) - -67)].Value;
						b2 ^= 0x50;
						goto IL_0901;
						IL_084e:
						b = (byte)(1409826816u % (uint)((int)((uint)b % (uint)(~b2)) >> (int)b2) - 4294967203u);
						goto IL_0901;
						IL_076d:
						b = (byte)((sbyte)(~b << ((b2 % 1639625401) | 0x3F8E9130)) + 246);
						goto IL_0901;
						IL_0681:
						b = (byte)((((b - -1959895265) | b) & (-1733528155 >> (b & b))) ^ 0x74D1A004);
						num = characteristics[(CharacteristicKeyword)(-b - -297)].Value;
						if (-238977827 - b == 0)
						{
							goto IL_0014;
						}
						b = (byte)(((-15458 / ~b >> 18) % (b * 74685518)) ^ 0x1C);
						continue;
						IL_01e6:
						b = (byte)(1057304462 % (-1204643825 + b) + b2 - 1057304462);
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[-2106149485 + (b2 % ~(b ^ b2) - (b2 & -32701677) + (int)((uint)(2106150575 - b2) ^ (((uint)b % 957280687u) | b2)))](element, "neutral"))
						{
							b2 = (byte)((b >> 5) / -1809264510 % ((b2 % b) ^ (b2 | -473956569)));
							if ((uint)((int)(0 - (0xB011DC03u | (3860853525u / (uint)(~b2)))) + -1372826988) < (uint)(-1341594326 * b))
							{
								goto IL_000a;
							}
							b = (byte)(3 ^ (((0x1F3957A0 ^ (-132011624 | b2)) << 29) % (-845841680 / ~b2 / ~(b2 & b))));
							continue;
						}
						goto IL_0791;
						IL_062d:
						b = (byte)(246 + (uint)b2 % 1586497076u);
						goto IL_0901;
						IL_05cd:
						num2 = characteristics[(CharacteristicKeyword)(0x54 ^ (0xB9 ^ b))].Value;
						b2 = 0;
						if (b2 > (0xB123D23 | (b2 + 1042779557)) * b2 * (int)(((2610210051u > (uint)b % 1409354268u) ? 1u : 0u) % 1905556767u))
						{
							goto end_IL_0014;
						}
						b = (byte)(949614778 - b + -949614387);
						continue;
						IL_0125:
						b = (byte)(((-89694702 * b / b) ^ ((int)((uint)b % (uint)b) * (732637576 - b))) + 89694948);
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[~(((b >> 21) | b) - b) + 721](element, "earth"))
						{
							if (!_671BC22C.BF9F3D1F.FE8E0C9E[(int)((((b >> 28) - (b >> 19) > -1415746768) ? 1u : 0u) << (int)((uint)(-b) / ~(((582624694 == b) ? 1u : 0u) / 289397815u))) - -719](element, "air"))
							{
								b2 = b;
								b = (byte)(((-330694174 & b2) + (b >> 965549068 + b2)) % (int)(~((uint)b % (uint)b)) + 53);
								continue;
							}
							goto IL_0643;
						}
						num = characteristics[(CharacteristicKeyword)(((1906699318 << (int)((uint)b % (uint)(b + b))) & (-426604742 >>> -13437 * b)) ^ 0x980002F)].Value;
						if ((int)b > (int)((uint)((1689127729 + b) ^ -868722507) / 168671124u))
						{
							continue;
						}
						goto IL_03a4;
						IL_0559:
						b = (byte)(((3635580693u % (uint)(~b2)) & 0xC687F8A3u) - 3229779723u);
						goto IL_0901;
						IL_0495:
						b = (byte)(1630681542 + (-1800755066 >> ((1924067083 > b) ? 1 : 0) >> (int)(short)b) * ((b & b) + 1079098966));
						num = characteristics[(CharacteristicKeyword)(48 + ((int)((uint)b / (uint)(b & 0xD123106)) * (b >>> (-1424867049 ^ b)) - (int)(((-231098616 + b < -568578902) ? 1u : 0u) >> (int)b)))].Value;
						goto IL_0504;
						IL_042e:
						b = (byte)(0xF7 ^ ((b2 % ~b2) | 1));
						goto IL_0643;
						IL_0346:
						b = (byte)(2135281253 + (-2135280867 - b));
						goto IL_0791;
						IL_02af:
						b = (byte)((((2139616673 - (b - -1071512013)) | (b2 % b + b)) / b) ^ 0x1538A9B0);
						if (_671BC22C.BF9F3D1F.FE8E0C9E[(b & (((-166727127 + b) | 0x3E0B1F92) >>> (int)b2)) ^ 0x246](element, "push"))
						{
							goto IL_086b;
						}
						if (b * -485619308 == 0)
						{
							ushort num3 = (ushort)(((uint)(1614213409 / b) / (uint)(~(-871308652 & b2))) | 0xFFFFFFFFu);
							b = (byte)((uint)(1821366960 - b) / (uint)b % (uint)(~b2) + 4287563507u);
							continue;
						}
						goto IL_0901;
						IL_0791:
						num = characteristics[(CharacteristicKeyword)(b2 ^ -419029595 ^ -419029664)].Value;
						if (((b < 1426589209) ? 1 : 0) * (b2 & b2) == 0)
						{
							goto IL_000a;
						}
						b = (byte)(0x13 ^ (byte)(3 - b2));
						continue;
						end_IL_0056:
						break;
					}
					b = (byte)((-1618728900 | (-1265184092 ^ (-1509832135 | (-1584420591 | b)))) ^ -1617086134);
					goto IL_070b;
					IL_0504:
					num2 = characteristics[(CharacteristicKeyword)((int)((uint)(b | b) % (uint)(b & -1198852811)) - -26)].Value;
					b = (byte)(b - -20);
					b2 = 0;
					if ((0x1A53704E ^ b2) != 0)
					{
						b = (byte)(194 + 2u / (uint)(b * 1704937887 << (int)b2));
						goto IL_0056;
					}
					goto IL_070b;
					IL_03a4:
					num2 = characteristics[(CharacteristicKeyword)((uint)(b2 >> 8 << ~(b2 << (int)b2)) / (uint)((-b2 & 0x785AF9F7) ^ (b2 % -2088011104)) + 49)].Value;
					b2 ^= 0xB8;
					b = 246;
					if (-(-2144160326 / (int)(~(1051490458u / (uint)(~b2)))) == 0)
					{
						ushort num3 = (ushort)(b2 ^ 0x3D);
						b = (byte)((b / -174096490 << ((~b2 | b) >> (int)b)) - -158);
						goto IL_0056;
					}
					goto IL_0901;
					continue;
					end_IL_0014:
					break;
				}
				break;
			}
		}
	}

	internal SpellCastInfo GetWhatSpellToCast()
	{
		uint num = 3207790492u;
		return new SpellCastInfo();
	}

	[AsyncStateMachine(typeof(_003CDoAction_003Ed__244))]
	internal Task<bool> DoAction()
	{
		_003CDoAction_003Ed__244 stateMachine = default(_003CDoAction_003Ed__244);
		byte b;
		do
		{
			stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
			stateMachine._003C_003E4__this = this;
			stateMachine._003C_003E1__state = -1;
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			b = 0;
		}
		while ((sbyte)b != 0);
		return stateMachine._003C_003Et__builder.Task;
	}

	internal Dictionary<int, bool> BuildOccupiedCells(int selfCellId, int? destCellIdToAllow = null)
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		using (IEnumerator<FighterManager.ActorFighter> enumerator = FighterManager.Defenders.Cast<FighterManager.ActorFighter>().Concat(FighterManager.Attackers.Cast<FighterManager.ActorFighter>()).GetEnumerator())
		{
			while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
			{
				FighterManager.ActorFighter current = enumerator.Current;
				if (current.IsAlive)
				{
					int cellId = current.CellId;
					if (cellId != selfCellId && (!destCellIdToAllow.HasValue || cellId != destCellIdToAllow.Value))
					{
						dictionary[cellId] = true;
					}
				}
			}
		}
		foreach (int turnOccupiedCell in FighterManager.TurnOccupiedCells)
		{
			if (turnOccupiedCell != selfCellId && (!destCellIdToAllow.HasValue || turnOccupiedCell != destCellIdToAllow.Value))
			{
				dictionary[turnOccupiedCell] = true;
			}
		}
		return dictionary;
	}

	internal void Verbose(string message)
	{
		FighterManager.Verbose(message);
	}

	internal void Verbose(string label, object value, int maxChars = 10000)
	{
		FighterManager.Verbose(label, value, maxChars);
	}

	internal void Verbose(object value, int maxChars = 10000)
	{
		Verbose("data", value, maxChars);
	}

	internal void Verbose(Func<string> messageFactory)
	{
		FighterManager.Verbose(messageFactory);
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

	internal int GetFarthestReachableCellFromMonsters(int currentCellId, int maxMp)
	{
		MapInformation mapInformation = FighterManager.MapInformation;
		if (mapInformation?.Cells == null || mapInformation.Cells.Count == 0)
		{
			Verbose("FRRF.earlyReturn.noMap", new global::_7EA8EA2C<int, int>(currentCellId, maxMp));
			return currentCellId;
		}
		List<FighterManager.Defender> defenders = FighterManager.Defenders;
		if (defenders == null || defenders.Count == 0)
		{
			Verbose("FRRF.earlyReturn.noMonsters", new global::FA24CC05<int, int, int>(currentCellId, maxMp, mapInformation.Cells.Count));
			return currentCellId;
		}
		Dictionary<int, bool> dictionary = BuildOccupiedCells(currentCellId);
		Verbose("FRRF.start", new global::_94AA8691<int, int, int, int, int>(currentCellId, maxMp, mapInformation.Cells.Count, defenders.Count, dictionary.Count));
		int num = currentCellId;
		double num2 = -1.0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		foreach (Cell cell in mapInformation.Cells)
		{
			_003C_003Ec__DisplayClass251_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass251_0();
			int cellNumber = cell.CellNumber;
			if (cell.NonWalkableDuringFight == 1)
			{
				num4++;
				continue;
			}
			if (dictionary.ContainsKey(cellNumber))
			{
				num5++;
				continue;
			}
			List<int> path = JsPathFinder.GetPath(currentCellId, cellNumber, mapInformation, dictionary, allowDiagonals: false);
			if (path == null || path.Count == 0)
			{
				num6++;
				continue;
			}
			int num9 = path.Count - 1;
			if (num9 > maxMp)
			{
				num7++;
				continue;
			}
			num3++;
			CS_0024_003C_003E8__locals2.pt = _9F8EAF35.D6056589((uint)cellNumber);
			double num10 = defenders.Min((FighterManager.Defender m) => CS_0024_003C_003E8__locals2.pt.FB2C1732(new _9F8EAF35(m.CellId)));
			if (num10 > num2)
			{
				num2 = num10;
				num = cellNumber;
				num8++;
				Verbose("FRRF.bestUpdate", new global::ADB76789<int, double, int, int>(num, _671BC22C.BF9F3D1F.FE8E0C9E[1919](num2, 3), num9, path.Count));
			}
		}
		Verbose("FRRF.summary", new global::BEA97700<int, int, int, int, int, int, int, int, int, double?>(currentCellId, maxMp, num3, num4, num5, num6, num7, num8, num, (num2 < 0.0) ? ((double?)null) : new double?(_671BC22C.BF9F3D1F.FE8E0C9E[1919](num2, 3))));
		return num;
	}

	internal int GetSafestReachableCellWithinRangeOfMonsters(int currentCellId, int maxMp, int minRange, int maxRange)
	{
		_003C_003Ec__DisplayClass252_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass252_0();
		CS_0024_003C_003E8__locals15.minRange = minRange;
		CS_0024_003C_003E8__locals15.maxRange = maxRange;
		MapInformation mapInformation = FighterManager.MapInformation;
		if (mapInformation?.Cells == null || mapInformation.Cells.Count == 0)
		{
			Verbose("GSRCR.earlyReturn.noMap", new global::_3B07969B<int, int, int, int>(currentCellId, maxMp, CS_0024_003C_003E8__locals15.minRange, CS_0024_003C_003E8__locals15.maxRange));
			return currentCellId;
		}
		List<FighterManager.ActorFighter> list = FighterManager.Defenders?.Where((FighterManager.Defender m) => m.IsAlive).Cast<FighterManager.ActorFighter>().ToList();
		if (list == null || list.Count == 0)
		{
			Verbose("GSRCR.earlyReturn.noMonsters", new global::_001D1D21<int, int, int, int, int>(currentCellId, maxMp, CS_0024_003C_003E8__locals15.minRange, CS_0024_003C_003E8__locals15.maxRange, mapInformation.Cells.Count));
			return currentCellId;
		}
		Dictionary<int, bool> dictionary = BuildOccupiedCells(currentCellId);
		Verbose("GSRCR.start", new global::_9B18D426<int, int, int, int, int, int, int>(currentCellId, maxMp, mapInformation.Cells.Count, list.Count, dictionary.Count, CS_0024_003C_003E8__locals15.minRange, CS_0024_003C_003E8__locals15.maxRange));
		int num = currentCellId;
		double num2 = -1.0;
		int num3 = int.MaxValue;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		foreach (Cell cell in mapInformation.Cells)
		{
			_003C_003Ec__DisplayClass252_1 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass252_1();
			int cellNumber = cell.CellNumber;
			if (cell.NonWalkableDuringFight == 1)
			{
				num5++;
				continue;
			}
			if (dictionary.ContainsKey(cellNumber))
			{
				num6++;
				continue;
			}
			List<int> path = JsPathFinder.GetPath(currentCellId, cellNumber, mapInformation, dictionary, allowDiagonals: false);
			if (path == null || path.Count == 0)
			{
				num7++;
				continue;
			}
			int num11 = path.Count - 1;
			if (num11 > maxMp)
			{
				num8++;
				continue;
			}
			CS_0024_003C_003E8__locals13.pt = _9F8EAF35.D6056589((uint)cellNumber);
			List<int> list2 = list.Select((FighterManager.ActorFighter m) => CS_0024_003C_003E8__locals13.pt.FB2C1732(new _9F8EAF35(m.CellId))).ToList();
			if (!list2.Any((int d) => d >= CS_0024_003C_003E8__locals15.minRange && d <= CS_0024_003C_003E8__locals15.maxRange))
			{
				num9++;
				continue;
			}
			num4++;
			double num12 = _671BC22C.BF9F3D1F.FE8E0C9E[1774](list2);
			if (num12 > num2 || (_671BC22C.BF9F3D1F.FE8E0C9E[1855](num12 - num2) < 0.001 && num11 < num3))
			{
				num2 = num12;
				num = cellNumber;
				num3 = num11;
				num10++;
				Verbose("GSRCR.bestUpdate", new global::ADB76789<int, double, int, int>(num, _671BC22C.BF9F3D1F.FE8E0C9E[1919](num2, 3), num11, path.Count));
			}
		}
		Verbose("GSRCR.summary", new global::_673E5731<int, int, int, int, int, int, int, int, int, int, int, int, double?>(currentCellId, maxMp, CS_0024_003C_003E8__locals15.minRange, CS_0024_003C_003E8__locals15.maxRange, num4, num5, num6, num7, num8, num9, num10, num, (num2 < 0.0) ? ((double?)null) : new double?(_671BC22C.BF9F3D1F.FE8E0C9E[1919](num2, 3))));
		return num;
	}

	internal int GetFarthestReachableCellFromCellId(int currentCellId, int awayCellId, int maxMp)
	{
		MapInformation mapInformation = FighterManager.MapInformation;
		if (mapInformation?.Cells == null || mapInformation.Cells.Count == 0)
		{
			return currentCellId;
		}
		Dictionary<int, bool> dictionary = BuildOccupiedCells(currentCellId);
		int result = currentCellId;
		double num = -1.0;
		_9F8EAF35 fF9851AF = new _9F8EAF35(awayCellId);
		foreach (Cell cell in mapInformation.Cells)
		{
			int cellNumber = cell.CellNumber;
			if (cell.NonWalkableDuringFight == 1 || dictionary.ContainsKey(cellNumber))
			{
				continue;
			}
			List<int> path = JsPathFinder.GetPath(currentCellId, cellNumber, mapInformation, dictionary, allowDiagonals: false);
			if (path != null && path.Count != 0 && path.Count - 1 <= maxMp)
			{
				double num2 = _9F8EAF35.D6056589((uint)cellNumber).FB2C1732(fF9851AF);
				if (num2 > num)
				{
					num = num2;
					result = cellNumber;
				}
			}
		}
		return result;
	}

	internal async Task<bool> MoveAwayFromAllMonsters(int minimalMovementPoint)
	{
		FighterManager.Attacker self = FighterManager.GetSelf();
		if (self == null)
		{
			Verbose("MAFM.earlyReturn.selfNull", true);
			return false;
		}
		int value = self.Characteristics[CharacteristicKeyword.MovementPoints].Value;
		int num = _671BC22C.BF9F3D1F.FE8E0C9E[325](minimalMovementPoint, value);
		Verbose("MAFM.start", new global::_13110031<long, int, int, int, int>(self.ActorId, self.CellId, minimalMovementPoint, value, num));
		if (num <= 0)
		{
			Verbose("MAFM.earlyReturn.noMPBudget", true);
			return false;
		}
		int farthestReachableCellFromMonsters = GetFarthestReachableCellFromMonsters(self.CellId, num);
		_9F8EAF35 obj = _9F8EAF35.D6056589((uint)farthestReachableCellFromMonsters);
		_9F8EAF35 fF9851AF = _9F8EAF35.D6056589((uint)self.CellId);
		int num2 = obj.FB2C1732(fF9851AF);
		Verbose("MAFM.target", new global::BBB89A1D<int, int>(farthestReachableCellFromMonsters, num2));
		if (num2 <= 1)
		{
			Verbose("MAFM.earlyReturn.alreadyFarEnough", true);
			return true;
		}
		Dictionary<int, bool> dictionary = BuildOccupiedCells(self.CellId);
		Verbose("MAFM.occupied", new global::_79824BA3<int>(dictionary.Count));
		List<int> path = JsPathFinder.GetPath(self.CellId, farthestReachableCellFromMonsters, FighterManager.MapInformation, dictionary, allowDiagonals: false);
		if (path == null || path.Count == 0)
		{
			Verbose("MAFM.path.none", new global::D1132D2A<int, int>(self.CellId, farthestReachableCellFromMonsters));
			return false;
		}
		Verbose("MAFM.path.full", new global::_15AE4129<int>(path.Count));
		List<int> list = path.Take(num + 1).ToList();
		if (list.Count > 0 && list.Last() == farthestReachableCellFromMonsters)
		{
			list.RemoveAt(list.Count - 1);
		}
		Verbose("MAFM.path.sliced", new global::_7B1C811C<int, int>(list.Count, num));
		List<int> list2 = JsPathFinder.CompressPath(list);
		Verbose("MAFM.path.compressed", new global::_15AE4129<int>(list2.Count));
		try
		{
			int num3 = (int)FighterManager.MapInformation.MapId;
			Verbose("MAFM.move.request", new global::_229BF015<int, int>(num3, list2.Count));
			bool flag = await InstanceData.MessageHandler._060DCDB8(list2, num3, 1);
			Verbose("MAFM.move.result", flag);
			return flag;
		}
		catch (Exception ex)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[376]();
			_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex.ToString());
			Verbose("MAFM.exception", new global::_1D01AD97<string, string>(_671BC22C.BF9F3D1F.FE8E0C9E[2117](ex), _671BC22C.BF9F3D1F.FE8E0C9E[605](ex)));
			return false;
		}
	}

	internal async Task<bool> MoveAwayFrom(int targetCellId, int minimalMovementPoint)
	{
		FighterManager.Attacker self = FighterManager.GetSelf();
		if (self == null)
		{
			return false;
		}
		minimalMovementPoint = _671BC22C.BF9F3D1F.FE8E0C9E[325](minimalMovementPoint, self.Characteristics[CharacteristicKeyword.MovementPoints].Value);
		if (minimalMovementPoint <= 0)
		{
			return false;
		}
		int farthestReachableCellFromCellId = GetFarthestReachableCellFromCellId(self.CellId, targetCellId, minimalMovementPoint);
		_9F8EAF35 obj = _9F8EAF35.D6056589((uint)farthestReachableCellFromCellId);
		_9F8EAF35 fF9851AF = _9F8EAF35.D6056589((uint)self.CellId);
		if (obj.FB2C1732(fF9851AF) <= 1)
		{
			return true;
		}
		Dictionary<int, bool> occupiedCells = BuildOccupiedCells(self.CellId);
		List<int> list = JsPathFinder.GetPath(self.CellId, farthestReachableCellFromCellId, FighterManager.MapInformation, occupiedCells, allowDiagonals: false).Take(minimalMovementPoint + 1).ToList();
		if (list.Count > 0 && list.Last() == farthestReachableCellFromCellId)
		{
			list.RemoveAt(list.Count - 1);
		}
		List<int> _47A7463D = JsPathFinder.CompressPath(list);
		try
		{
			return await InstanceData.MessageHandler._060DCDB8(_47A7463D, (int)FighterManager.MapInformation.MapId, 1);
		}
		catch (Exception ex)
		{
			_1E3B359C._6E3CC3BA(ex.ToString());
			return false;
		}
	}

	internal async Task<bool> MoveWithinRangeOfAnyMonster(int minDistance, int maxDistance, int minimalMovementPoint)
	{
		_003C_003Ec__DisplayClass256_0 CS_0024_003C_003E8__locals22 = new _003C_003Ec__DisplayClass256_0();
		CS_0024_003C_003E8__locals22.minDistance = minDistance;
		CS_0024_003C_003E8__locals22.maxDistance = maxDistance;
		FighterManager.Attacker self = FighterManager.GetSelf();
		if (self == null)
		{
			Verbose("MWRA.earlyReturn.selfNull", true);
			return false;
		}
		if (CS_0024_003C_003E8__locals22.maxDistance < CS_0024_003C_003E8__locals22.minDistance)
		{
			Verbose("MWRA.earlyReturn.invalidRange", new global::F13B3520<int, int>(CS_0024_003C_003E8__locals22.minDistance, CS_0024_003C_003E8__locals22.maxDistance));
			return false;
		}
		CS_0024_003C_003E8__locals22.monsters = FighterManager.Defenders?.Where((FighterManager.Defender m) => m.IsAlive).Cast<FighterManager.ActorFighter>().ToList();
		if (CS_0024_003C_003E8__locals22.monsters == null || CS_0024_003C_003E8__locals22.monsters.Count == 0)
		{
			Verbose("MWRA.earlyReturn.noMonsters", true);
			return false;
		}
		if (CellSatisfiesBand(self.CellId))
		{
			Verbose("MWRA.earlyReturn.alreadyInBand", new global::DD1C081E<int, int, int>(self.CellId, CS_0024_003C_003E8__locals22.minDistance, CS_0024_003C_003E8__locals22.maxDistance));
			return true;
		}
		int value = self.Characteristics[CharacteristicKeyword.MovementPoints].Value;
		int num = _671BC22C.BF9F3D1F.FE8E0C9E[325](minimalMovementPoint, value);
		Verbose("MWRA.start", new global::A92F2634<long, int, int, int, int, int, int>(self.ActorId, self.CellId, CS_0024_003C_003E8__locals22.minDistance, CS_0024_003C_003E8__locals22.maxDistance, minimalMovementPoint, value, num));
		if (num <= 0)
		{
			Verbose("MWRA.earlyReturn.noMPBudget", true);
			return false;
		}
		int safestReachableCellWithinRangeOfMonsters = GetSafestReachableCellWithinRangeOfMonsters(self.CellId, num, CS_0024_003C_003E8__locals22.minDistance, CS_0024_003C_003E8__locals22.maxDistance);
		_9F8EAF35 obj = _9F8EAF35.D6056589((uint)safestReachableCellWithinRangeOfMonsters);
		_9F8EAF35 fF9851AF = _9F8EAF35.D6056589((uint)self.CellId);
		int num2 = obj.FB2C1732(fF9851AF);
		Verbose("MWRA.target", new global::_769EDFB6<int, int>(safestReachableCellWithinRangeOfMonsters, num2));
		if (!CellSatisfiesBand(safestReachableCellWithinRangeOfMonsters))
		{
			Verbose("MWRA.earlyReturn.noCandidate", new global::AD399A0C<int>(safestReachableCellWithinRangeOfMonsters));
			return false;
		}
		if (num2 <= 0)
		{
			Verbose("MWRA.earlyReturn.alreadyThere", true);
			return true;
		}
		Dictionary<int, bool> dictionary = BuildOccupiedCells(self.CellId);
		Verbose("MWRA.occupied", new global::_79824BA3<int>(dictionary.Count));
		List<int> path = JsPathFinder.GetPath(self.CellId, safestReachableCellWithinRangeOfMonsters, FighterManager.MapInformation, dictionary, allowDiagonals: false);
		if (path == null || path.Count == 0)
		{
			Verbose("MWRA.path.none", new global::AB2F563F<int, int>(self.CellId, safestReachableCellWithinRangeOfMonsters));
			return false;
		}
		Verbose("MWRA.path.full", new global::_15AE4129<int>(path.Count));
		List<int> list = path.Take(num + 1).ToList();
		if (list.Count > 0 && list.Last() == safestReachableCellWithinRangeOfMonsters)
		{
			list.RemoveAt(list.Count - 1);
		}
		Verbose("MWRA.path.sliced", new global::_7B1C811C<int, int>(list.Count, num));
		List<int> list2 = JsPathFinder.CompressPath(list);
		Verbose("MWRA.path.compressed", new global::_15AE4129<int>(list2.Count));
		try
		{
			int num3 = (int)FighterManager.MapInformation.MapId;
			Verbose("MWRA.move.request", new global::_229BF015<int, int>(num3, list2.Count));
			bool flag = await InstanceData.MessageHandler._060DCDB8(list2, num3, 1);
			Verbose("MWRA.move.result", flag);
			return flag;
		}
		catch (Exception ex)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[376]();
			_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex.ToString());
			Verbose("MWRA.exception", new global::_1D01AD97<string, string>(_671BC22C.BF9F3D1F.FE8E0C9E[2117](ex), _671BC22C.BF9F3D1F.FE8E0C9E[605](ex)));
			return false;
		}
		bool CellSatisfiesBand(int cellId)
		{
			_003C_003Ec__DisplayClass256_1 CS_0024_003C_003E8__locals20 = new _003C_003Ec__DisplayClass256_1();
			CS_0024_003C_003E8__locals20.pt = _9F8EAF35.D6056589((uint)cellId);
			List<int> list3 = CS_0024_003C_003E8__locals22.monsters.Select((FighterManager.ActorFighter m) => CS_0024_003C_003E8__locals20.pt.FB2C1732(new _9F8EAF35(m.CellId))).ToList();
			if (list3.Count == 0)
			{
				return false;
			}
			bool flag2 = list3.Any((int d) => d >= CS_0024_003C_003E8__locals22.minDistance && d <= CS_0024_003C_003E8__locals22.maxDistance);
			double num4 = _671BC22C.BF9F3D1F.FE8E0C9E[1774](list3);
			if (flag2)
			{
				return num4 >= (double)CS_0024_003C_003E8__locals22.minDistance;
			}
			return false;
		}
	}

	internal async Task<bool> MoveTo(int targetCellId, int minimalMovementPoint)
	{
		FighterManager.Attacker self = FighterManager.GetSelf();
		if (self == null)
		{
			Verbose("MoveTo.early.selfNull", true);
			return false;
		}
		int value = self.Characteristics[CharacteristicKeyword.MovementPoints].Value;
		int num = _671BC22C.BF9F3D1F.FE8E0C9E[325](minimalMovementPoint, value);
		Verbose("MoveTo.start", new global::_9A21D42D<int, int, int, int, int>(targetCellId, minimalMovementPoint, value, num, self.CellId));
		if (num <= 0)
		{
			Verbose("MoveTo.early.noMPBudget", true);
			return false;
		}
		Dictionary<int, bool> dictionary = BuildOccupiedCells(self.CellId);
		Verbose("MoveTo.occupied.initial", dictionary);
		List<int> path = JsPathFinder.GetPath(self.CellId, targetCellId, FighterManager.MapInformation, dictionary, allowDiagonals: false);
		Verbose("mapinformation", FighterManager.MapInformation, 10000000);
		int num2 = _9F8EAF35.D6056589((uint)targetCellId).FB2C1732(_9F8EAF35.D6056589((uint)self.CellId));
		Verbose("MoveTo.distance", new global::_54105B9F<int>(num2));
		if (num2 == 0)
		{
			Verbose("MoveTo.early.alreadyAtTarget", true);
			return true;
		}
		if (path == null || path.Count == 0)
		{
			Verbose("MoveTo.path.none", new global::EC104E38<int, int>(self.CellId, targetCellId));
			return false;
		}
		Verbose("MoveTo.path.full", path);
		List<int> list = path.Take(num + 1).ToList();
		Verbose("MoveTo.path.sliced", list);
		List<int> list2 = JsPathFinder.CompressPath(list);
		Verbose("MoveTo.path.compressed", list2);
		try
		{
			int num3 = (int)FighterManager.MapInformation.MapId;
			Verbose("MoveTo.request", new global::_229BF015<int, int>(num3, list2.Count));
			bool flag = await InstanceData.MessageHandler._060DCDB8(list2, num3, 1);
			Verbose("MoveTo.result", flag);
			self = FighterManager.GetSelf();
			if (self == null)
			{
				Verbose("MoveTo.end.selfNull", true);
				return false;
			}
			Verbose("new Cell id :", new global::_16ACC587<int>(self.CellId));
			return flag;
		}
		catch (Exception ex)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1676](ex.ToString());
			Verbose("MoveTo.exception", new global::_1D01AD97<string, string>(_671BC22C.BF9F3D1F.FE8E0C9E[2117](ex), _671BC22C.BF9F3D1F.FE8E0C9E[605](ex)));
			return false;
		}
	}

	[AsyncStateMachine(typeof(_003CMoveCloserTo_003Ed__258))]
	internal Task<bool> MoveCloserTo(int targetCellId, int minimalMovementPoint)
	{
		ushort num = 0;
		_003CMoveCloserTo_003Ed__258 stateMachine = default(_003CMoveCloserTo_003Ed__258);
		if (4137371574u / (uint)(~num) >> (-1968727291 | num | 0x319B2B3F) * 496146565 == 0)
		{
			uint num2 = default(uint);
			while (true)
			{
				switch ((uint)num % 3u)
				{
				default:
					stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
					num2 = (uint)((ushort)(num * 144717963 >> 15) * num);
					num = (ushort)(0xDFAB ^ ((int)(0 - num2) >> (int)num2));
					continue;
				case 1u:
					break;
				case 2u:
					goto end_IL_0026;
				}
				num = (ushort)(0x7BBE96B7 ^ ~(-2034412487 - ((int)num2 - -41675505)));
				stateMachine._003C_003E4__this = this;
				stateMachine.targetCellId = targetCellId;
				stateMachine.minimalMovementPoint = minimalMovementPoint;
				stateMachine._003C_003E1__state = (int)((((num2 ^ (824842022 % ~num2)) | 0xFFFF98A6u) >> (int)((num2 << (num ^ num)) / (uint)(~(num / -1673927498 - -1826382810 * num)))) ^ 0x6059);
				if (num2 == 0)
				{
					num = (ushort)(~(2812326704u % (uint)(~num)) + 2812385099u);
					continue;
				}
				goto IL_0120;
				continue;
				end_IL_0026:
				break;
			}
			num = (((0x7239FE85 | (num * -1667684083 - 1168624284)) == (int)((((uint)num > 2560496667u) ? 1u : 0u) / 267156828u)) ? ((ushort)1) : ((ushort)0));
		}
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		goto IL_0120;
		IL_0120:
		return stateMachine._003C_003Et__builder.Task;
	}

	internal List<int> FindReachableCells(int startCellId, int maxMovementPoints, IDictionary<int, bool> occupiedCells)
	{
		List<int> list = new List<int>();
		int count = FighterManager.MapInformation.Cells.Count;
		list.Add(startCellId);
		for (int i = 0; i < count; i++)
		{
			if (i != startCellId && FighterManager.MapInformation.Cells[i].Mov != 0 && !occupiedCells.ContainsKey(i))
			{
				List<int> path = JsPathFinder.GetPath(startCellId, i, FighterManager.MapInformation, occupiedCells.ToDictionary(), allowDiagonals: false);
				if (path != null && path.Count > 0 && _671BC22C.BF9F3D1F.FE8E0C9E[1937](0, path.Count - 1) <= maxMovementPoints)
				{
					list.Add(i);
				}
			}
		}
		return list;
	}

	[AsyncStateMachine(typeof(_003CDoTurn_003Ed__260))]
	internal Task<bool> DoTurn()
	{
		_003CDoTurn_003Ed__260 stateMachine = default(_003CDoTurn_003Ed__260);
		sbyte b;
		short num;
		do
		{
			stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
			b = -28;
			if ((byte)(2819682340u * (1923440288u % (uint)b)) != 0)
			{
				stateMachine._003C_003E4__this = this;
				stateMachine._003C_003E1__state = -273003217 % b / b * (ushort)b / (b | -550786018 | 0x2DBA5D13) - 1;
			}
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			num = (short)(((-1659737343 > (int)(4022336923u / (uint)b)) ? 1u : 0u) % (uint)b);
		}
		while (((3945484421u > (uint)(2082939964 >>> (int)b)) ? 1 : 0) > (b >>> (b & (byte)b)) - num);
		return stateMachine._003C_003Et__builder.Task;
	}

	internal void GameFightStartingMessage()
	{
	}

	internal void GameFightEndMessage()
	{
	}

	internal void GameFightPlacementPossiblePositionsMessage()
	{
	}

	internal void GameEntitiesDispositionMessage()
	{
	}

	internal void GameEntityDispositionErrorMessage()
	{
	}

	internal void GameFightTurnStartMessage()
	{
	}

	internal void GameActionFightNoSpellCastMessage()
	{
	}

	internal void GameFightNewRoundMessage()
	{
	}

	internal void GameFightSynchronizeMessage()
	{
	}

	internal void GameFightTurnResumeMessage()
	{
	}

	internal void GameFightShowFighterMessage()
	{
	}

	internal void GameFightUpdateTeamMessage()
	{
	}

	internal void TextInformationMessage()
	{
	}

	internal void GameMapNoMovementMessage()
	{
	}

	internal void MapComplementaryInformationsDataMessage()
	{
	}

	internal void PartyMemberInFightMessage()
	{
	}

	internal static bool CanBeTackler(FighterManager.Defender tackler, FighterManager.Attacker actor)
	{
		int num = 1494919673;
		return (byte)(~(0xA31B652Fu & (((uint)num > 1301590054u) ? 1u : 0u)) * (uint)(num / num / (-num | 0x5ABBC7A7))) != 0;
	}

	internal static bool CanBeTackled(FighterManager.Attacker attacker)
	{
		return true;
	}

	internal static double GetTackleRatio(FighterManager.Attacker attacker, FighterManager.Defender tackler)
	{
		ushort num = 36127;
		int num2 = _671BC22C.BF9F3D1F.FE8E0C9E[num + 1342177280 - 1342211470]((-2052933739 ^ num) - -2052965750, attacker.Characteristics[(CharacteristicKeyword)((int)(((0 > num / -575357685) ? 1u : 0u) ^ 0x40264FA2u ^ 0xFC31158Fu) - -1139320335)].Value);
		int num3 = _671BC22C.BF9F3D1F.FE8E0C9E[(int)(ushort)(num >> (int)num >> (int)num) % ((2133695874 % ~num < num) ? 1 : 0) - -1937]((sbyte)((uint)num / (uint)(-273837781 & num)) + -1, tackler.Characteristics[(CharacteristicKeyword)(((uint)(-num) / 1174929924u * num) ^ (uint)(num * num) ^ 0x4DCA8EA1)].Value);
		return (double)(num2 + (int)(800809618u / (uint)num - 22164)) / (double)(num3 + ((int)(((uint)num ^ (((uint)num > 2461333014u) ? 1u : 0u)) | 0x38735977) % (int)(~((uint)(-num) / (uint)(~(21659684 << (int)num)))) - -2)) / 2.0;
	}

	internal static TackleCost GetTackleCost(FighterManager.Attacker actor, List<FighterManager.Defender> tacklers, int mp, int ap)
	{
		sbyte b = -85;
		b -= -125;
		TackleCost tackleCost = default(TackleCost);
		uint num = default(uint);
		while (true)
		{
			switch ((uint)b % 5u)
			{
			default:
				b = (sbyte)(((-2028815200 ^ b) | (b - b << 26)) - -2028815139);
				goto IL_0044;
			case 1u:
				b = (sbyte)(b * 389163785 - 721664856);
				ap = _671BC22C.BF9F3D1F.FE8E0C9E[(((uint)(b % 1865660939) > (uint)b) ? 1u : 0u) ^ 0x791u](-(b >>> -b), ap);
				b = (sbyte)(~(((3996618906u < (uint)b) ? 1u : 0u) | 0x36176D21u | (uint)b));
				tackleCost = new TackleCost((-72190797 - b) ^ -72190693, 104 + (b | b));
				if (!CanBeTackled(actor))
				{
					num = (uint)(b << 17);
					if ((-558557256 & (ushort)(b >>> (int)num)) != (b >> (int)b) % (0x92 | (b & -82348257)))
					{
						continue;
					}
					break;
				}
				num = (uint)(-1027945569 ^ (b / (b | -1272877389)));
				if ((uint)(~((b / 1209379624) | 0x6886A702)) >> (int)num != 0)
				{
					b = (sbyte)((int)num - -1027945623);
					continue;
				}
				goto IL_0044;
			case 2u:
				return tackleCost;
			case 3u:
				b = (sbyte)((int)(((2827566730u > ((-879999220 == (int)(num | 0xD68EBF8Fu)) ? 1u : 0u)) ? 1u : 0u) % (((0 - num) ^ (859014443 * num)) << 15)) + -105);
				if (tacklers.Count == 0)
				{
					b = (sbyte)(-1380498762 * (int)num >> (964 >>> (int)(num << -1650829154 % (int)num)));
					continue;
				}
				break;
			case 4u:
				{
					return tackleCost;
				}
				IL_0044:
				mp = _671BC22C.BF9F3D1F.FE8E0C9E[b ^ -1990]((b % -1243306054 - 428671409) ^ -428671494, mp);
				b = (sbyte)(~(2132658310 >>> (int)b));
				b = (sbyte)((b - -1573628392) ^ 0x5DCBAA00);
				continue;
			}
			break;
		}
		using (List<FighterManager.Defender>.Enumerator enumerator = tacklers.GetEnumerator())
		{
			if ((((int)num * -911503338 == -671088640 * b) ? 1u : 0u) - (uint)(-b) != 0)
			{
				b = (sbyte)(0x24 ^ (num >> 19 >> 13));
				goto IL_0211;
			}
			goto IL_02e8;
			IL_0388:
			int num2 = default(int);
			b = (sbyte)((num2 >>> 8) - 1052291003 - -1047664450);
			double tackleRatio = default(double);
			tackleCost.Ap += (int)((double)ap * (1.0 - tackleRatio) + 0.5);
			goto IL_03c0;
			IL_0211:
			while (true)
			{
				switch ((uint)b % 6u)
				{
				case 1u:
					goto IL_02a7;
				case 2u:
					goto IL_02e8;
				case 3u:
					b += -119;
					tackleCost.Mp += (int)((double)mp * (1.0 - tackleRatio) + 0.5);
					if (137 - (((0 - num) / 3781074842u) & num) != 0)
					{
						b = (sbyte)((int)b / (int)(~(3052640350u / (uint)((int)num + -1933877875 + (int)(num << (int)b)))) + -64);
						continue;
					}
					goto IL_02e8;
				case 4u:
					goto IL_0388;
				case 5u:
					goto IL_0442;
				}
				break;
			}
			b = (sbyte)(-103 ^ (int)(((uint)(715695549 - (-1718907340 - (int)num)) | num) / 2930251137u));
			goto IL_03c0;
			IL_02a7:
			b = (sbyte)(-104 + (int)((uint)(b >> 23) % ((uint)b ^ num)));
			goto IL_02b7;
			IL_026c:
			FighterManager.Defender current = default(FighterManager.Defender);
			if (CanBeTackler(current, actor))
			{
				num2 = (int)((uint)b & num) >> ~((b / -1803112052) ^ 0x442EFE8C);
				b = (sbyte)(4286573311u % (uint)b + 8394016);
				goto IL_0211;
			}
			goto IL_03c0;
			IL_02e8:
			if (!(tackleRatio >= 1.0))
			{
				num2 = (int)(0 - ~num * ((uint)b ^ num));
				if ((0 & num2) == 0)
				{
					b = (sbyte)((((num2 < b) ? 1u : 0u) ^ 0xE383683u) - 238564980);
					goto IL_0211;
				}
				goto IL_026c;
			}
			goto IL_03c0;
			IL_0442:
			b = (sbyte)((((2109518265u > (uint)((b << 4) / b)) ? 1 : 0) >> (int)(((num > (uint)b) ? 1u : 0u) | num)) + -64);
			if (enumerator.MoveNext())
			{
				current = enumerator.Current;
				FighterManager.Defender defender = current;
				b = -104;
				num = 3267021726u;
				if (defender != null)
				{
					goto IL_026c;
				}
				goto IL_03c0;
			}
			goto end_IL_01ea;
			IL_03c0:
			b = (sbyte)((int)(1402867356u % (uint)b) * -31728 >>> 104645999 % (sbyte)num / (int)((uint)b / (uint)b - 733811340));
			if ((num & 0x8AB43D01u) < (uint)((75811736 >> (int)((1536450048 / num) | (uint)(-1825338195 | b))) / ~((b ^ b) % 1588980765)))
			{
				b = (sbyte)(~((uint)(-477520607 % b) % (uint)(0x4738BDBE ^ (1094300038 - b))) + 64222305);
				goto IL_0211;
			}
			goto IL_02b7;
			IL_02b7:
			do
			{
				tackleRatio = GetTackleRatio(actor, current);
			}
			while ((uint)((b << 3) / b << (num2 * (num2 * b) << (int)num)) > (uint)(-2145307604 * num2));
			goto IL_0211;
			end_IL_01ea:;
		}
		b = 0;
		return tackleCost;
	}

	internal Dictionary<int, MoveNode> GetReachableZone(FighterManager.Attacker fighterData)
	{
		ushort num = ushort.MaxValue;
		if ((uint)(536904850 - num >>> -2067917263 * num << (((uint)num < (uint)(num - -1724238161)) ? 1 : 0)) < ~((num >> 9 > num) ? 1u : 0u) << (int)num)
		{
			num = (ushort)(((num == 971892294) ? 1u : 0u) / ~((1831266584 + -156238553 / num < -1853642335) ? 1u : 0u) - 4294963796u);
			goto IL_0069;
		}
		goto IL_018d;
		IL_018d:
		FightCharacteristics characteristics = default(FightCharacteristics);
		int value = characteristics[(CharacteristicKeyword)(0x11 ^ ((int)((uint)(-(num << 20)) / 3918959911u) / (-165384047 | (-1824590166 - num))))].Value;
		sbyte b = (sbyte)(-1145626310 & (-821668034 * num));
		num = (ushort)(0x7AC8 ^ (num % 151033751));
		goto IL_0069;
		IL_0069:
		Dictionary<int, MoveNode> dictionary2 = default(Dictionary<int, MoveNode>);
		sbyte b3 = default(sbyte);
		int value3 = default(int);
		sbyte b2 = default(sbyte);
		Dictionary<int, PathNode> dictionary = default(Dictionary<int, PathNode>);
		PathNode pathNode = default(PathNode);
		int cellId = default(int);
		List<PathNode> list = default(List<PathNode>);
		_003C_003Ec__DisplayClass288_0 _003C_003Ec__DisplayClass288_2 = default(_003C_003Ec__DisplayClass288_0);
		PathNode pathNode2 = default(PathNode);
		int cellId2 = default(int);
		PathNode value2 = default(PathNode);
		int current = default(int);
		PathNode pathNode3 = default(PathNode);
		while (true)
		{
			List<int> list2;
			List<FighterManager.Defender> tacklers;
			TackleCost tackleCost;
			int num3;
			int num4;
			int tackleMp;
			int tackleAp;
			int distance;
			bool reachable;
			List<int>.Enumerator enumerator;
			switch ((uint)num % 10u)
			{
			default:
				num = (ushort)((0x688E1 | num) + -364014);
				goto IL_00ac;
			case 1u:
				num = (ushort)(((int)(~(339060655u / (uint)(num / num))) >> 29) - -65536);
				dictionary2 = new Dictionary<int, MoveNode>();
				characteristics = fighterData.Characteristics;
				if ((uint)num <= 3475369384u)
				{
					num = (ushort)(0x332129B4 ^ (~(-1467382503 * num) + (263287188 + (num >> (int)num))));
					continue;
				}
				goto IL_02bb;
			case 2u:
				num = (ushort)(1719912992 % (int)(~(((1033886854 > num) ? 1u : 0u) / 101800594u)) * 1519931285 + 65535);
				break;
			case 3u:
				num = (ushort)(~((uint)num % (uint)(-b)) + 99639);
				if (value <= ((sbyte)(17606674u / (uint)(num - num + num)) ^ 0xC))
				{
					b3 = (sbyte)((uint)b / (uint)b >> 17);
					num = (ushort)(-b3 - -17844);
					continue;
				}
				b = (sbyte)((int)b + (int)(2762412674u / (574043065u / (uint)num) - num));
				if (b != 0)
				{
					continue;
				}
				goto IL_00ac;
			case 4u:
				num = (ushort)(0xFFFF ^ ((uint)((b3 % ~b3) & b3) % (uint)(~b3) << (int)num));
				return dictionary2;
			case 5u:
				value3 = characteristics[(CharacteristicKeyword)(((num + b < -1215298662) ? 1 : 0) - -1)].Value;
				b = (sbyte)(-1455446560 | b);
				num = (ushort)((0x527C67C5 & num) ^ 0xDCCB);
				continue;
			case 6u:
				num = (ushort)((1050169887 - b) / -1 + 1050235432);
				goto IL_02bb;
			case 7u:
				num = (ushort)(((int)num % (int)((uint)b % 1043229465u) >> ((b2 + 563778618) & 0x2BBD181F)) ^ 0xFFFF);
				dictionary = new Dictionary<int, PathNode>();
				pathNode = new PathNode(cellId, value, value3, (num ^ -1814880722) - -1814830639, (int)((num ^ ((uint)((b >>> 15) % num) / 1938885277u)) - 65535), 0xDFFFE ^ ((b + -558120053 / (num / b2)) | num));
				b2 = (sbyte)(876171394 >> -1008241546 / (int)((uint)num % (uint)(b | -79814855)));
				num = (ushort)(-33029 ^ (-1937 | num));
				continue;
			case 8u:
				num = (ushort)(b - -65545);
				list.Add(pathNode);
				dictionary[cellId] = pathNode;
				goto IL_03cf;
			case 9u:
				{
					num = (ushort)((0xC2E8830 ^ b) - -204441657);
					goto IL_058d;
				}
				IL_03cf:
				BuildOccupiedCells(cellId);
				if (-((0 | (b2 & 0x7F386F1E)) - -1530959217) == 0)
				{
					int num2 = (int)(0 - (uint)b % (uint)b);
					num = (ushort)((-458158936 % num) ^ -36268);
					continue;
				}
				goto IL_0b44;
				IL_0b44:
				num = 55612;
				if (list.Count > ((-(-208715749 ^ num) > num + ((246850832 * num) ^ 0x680C5490)) ? 1 : 0))
				{
					_003C_003Ec__DisplayClass288_2 = new _003C_003Ec__DisplayClass288_0();
					b2 = -46;
					if ((int)(((uint)b2 / (uint)(b2 >>> 20)) ^ ((1572446212 > (b2 ^ 0x498EBF82) >>> ~b2) ? 1u : 0u)) > (int)((uint)b2 / 2769269167u % (uint)(17487415 % b2)))
					{
						pathNode2 = list[list.Count - (1 + (b2 << 31 << (int)((uint)(-1608925568 + b2) / 1510888755u) >>> (-123917940 >>> (int)b2) * -(b2 - 1670002945)))];
						if (1281718864 - (b2 >>> 15 >> (int)(~((uint)b2 % (uint)b2))) <= ((sbyte)((uint)(b2 >>> 28) % 3607366699u) ^ ((int)((uint)(b2 * b2) / 470061188u) >> (int)b2)))
						{
							goto IL_03cf;
						}
						list.RemoveAt(list.Count - (1 ^ (((b2 * b2) & b2) << ~((b2 | b2) ^ b2))));
						b2 = (sbyte)((-b2 >>> b2 / b2) - (~b2 + (-1675390159 | (b2 | b2))));
					}
					do
					{
						cellId2 = pathNode2.CellId;
						b3 = (sbyte)(-(697719976 * b2) / (((b2 > b2) ? 1 : 0) + (b2 << 5)));
					}
					while (b2 - -2018544149 < (b2 ^ (-1398780656 >>> (int)b2)));
					_003C_003Ec__DisplayClass288_2.adjacents = InstanceData.GetAdjacentsCellId(cellId2, (byte)((b2 - 142196736) ^ -142196745) != 0);
					goto IL_058d;
				}
				return dictionary2;
				IL_02bb:
				list = new List<PathNode>();
				b2 = (sbyte)((num >>> b * num - 1721947571) - num / b);
				num = (ushort)(0x276F ^ ((int)(2055681060u % (uint)(b * b2)) >> b2 / 1773482297 % 60));
				continue;
				IL_00ac:
				cellId = fighterData.CellId;
				if ((num | -(num * 993280685 << 20)) > ~((int)(0xF00C898Cu & ((uint)num / 3381484553u)) / ((-1632611197 % num) ^ 0x4974AAE0)))
				{
					num = (ushort)(((1443831057 + 991294592 / num) & num) - (0x199F47B7 & num) - -5643);
					continue;
				}
				break;
				IL_058d:
				list2 = _003C_003Ec__DisplayClass288_2.adjacents.Where(delegate(int cellId3)
				{
					_003C_003Ec__DisplayClass288_1 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass288_1();
					CS_0024_003C_003E8__locals2.cellId = cellId3;
					return FighterManager.Defenders.Any((FighterManager.Defender defender) => defender.CellId == CS_0024_003C_003E8__locals2.cellId);
				}).ToList();
				tacklers = FighterManager.Defenders.Where(_003C_003Ec__DisplayClass288_2._003CGetReachableZone_003Eb__1).ToList();
				tackleCost = GetTackleCost(fighterData, tacklers, pathNode2.AvailableMp, pathNode2.AvailableAp);
				num3 = pathNode2.AvailableMp - tackleCost.Mp - ((int)(1117001919 % (3800771617u % (uint)(b2 & b2) / (uint)b3)) + -31067174);
				num4 = pathNode2.AvailableAp - tackleCost.Ap;
				tackleMp = pathNode2.TackleMp + tackleCost.Mp;
				tackleAp = pathNode2.TackleAp + tackleCost.Ap;
				distance = pathNode2.Distance + (int)(1 + (((1812480533 < b3) ? 1u : 0u) / 700385433u << (int)(~(((uint)b3 % (uint)b2) ^ (uint)b2))));
				reachable = ((num3 < (int)((uint)((int)(1554930080 * ((uint)b3 / (uint)b3)) >> 19) % 2769613741u - 2965)) ? 1 : 0) == -105 / ~((int)((uint)b2 / (uint)b2) / ~(b2 + 565595579)) + -105;
				enumerator = list2.GetEnumerator();
				try
				{
					if ((((0x5F07900A | b2) >> (-241249239 >> (int)b2) >>> (int)b2) & 0x9AA1B32) < -821131127 >>> b2 - 775072926)
					{
						b2 = (sbyte)((1102744849 >> (int)b3) * -1224757011 % b2 << (int)b2);
						goto IL_06ff;
					}
					goto IL_0a7d;
					IL_08c2:
					int availableAp = value2.AvailableAp;
					b2 ^= -49;
					if (availableAp < num4)
					{
						b2 ^= -49;
						goto IL_08e0;
					}
					goto IL_0a7d;
					IL_06ff:
					while (true)
					{
						switch ((uint)b3 % 7u)
						{
						case 1u:
							break;
						case 2u:
							goto end_IL_06ff;
						case 3u:
							b3 = (sbyte)((b3 ^ 0xAE56) - 44558);
							goto IL_09b4;
						default:
							goto IL_09b4;
						case 4u:
							goto IL_0a37;
						case 5u:
							goto IL_0acf;
						case 6u:
							b3 = (sbyte)(0x6EB7AAFC ^ (0xEEB7AAA8u ^ (2147483648u % (uint)(~(b3 >>> (int)(byte)b3)))));
							goto end_IL_06a9;
						}
						b3 = (sbyte)(b3 - -112 + -64);
						while (true)
						{
							int availableMp = value2.AvailableMp;
							b2 ^= -49;
							if (availableMp != num3)
							{
								break;
							}
							if ((uint)(((b2 - 1687039125) ^ (288488847 + b3 >>> 7)) + 1754803507) <= (uint)(((int)((uint)b3 / 1568728103u) % (int)b3) & -1901511806))
							{
								continue;
							}
							goto IL_088e;
						}
						goto IL_08e0;
						IL_0acf:
						b3 = (sbyte)(131 + -b3);
						if (enumerator.MoveNext())
						{
							goto IL_0729;
						}
						if (b2 / (-157709209 * (byte)(b3 | b3)) == 0)
						{
							b3 = (sbyte)((b2 % b3) ^ 6);
							continue;
						}
						goto IL_07a4;
						IL_088e:
						b3 = (sbyte)((0xE26294FFu & ((uint)(-778166604 & -b3) % 6884893u)) - 4194524);
						continue;
						end_IL_06ff:
						break;
					}
					b3 = (sbyte)(-94340278 + (b3 + 94340360));
					goto IL_08c2;
					IL_08e0:
					int nonWalkableDuringFight = FighterManager.MapInformation.Cells[current].NonWalkableDuringFight;
					uint num5 = 1 + (2636904253u / (uint)(b2 ^ 0x77B1986E) >> ((b2 + 515732239 != 0) ? 1 : 0));
					b3 = 84;
					b2 ^= -49;
					if (nonWalkableDuringFight != (int)num5)
					{
						b3 = (sbyte)((uint)b3 % ~((b2 + (b2 & -1709154113) == (int)((uint)(0x431268A4 & b2) / 4294967292u)) ? 1u : 0u));
						if ((uint)(b2 - -b3) > (uint)((((uint)b3 < (uint)b3) ? 1 : 0) % (b2 + 1200214797) >> 1795308599 * (b2 - 657956115) >> (int)b3))
						{
							b3 = (sbyte)((int)b3 / (int)(~((uint)b2 / (uint)(~((b3 >>> (int)b2) & 0x1120A095)))) + 136);
							goto IL_06ff;
						}
						goto IL_09c6;
					}
					goto IL_0a7d;
					IL_0a7d:
					b2 = (sbyte)((int)(((uint)b2 % (uint)b3) | 0x14B36B12) ^ (b3 * (b3 >>> 11)));
					if ((b2 ^ 0xA844E08) != 0)
					{
						b3 = (sbyte)(((983044489 >> b2 * b3 >>> b2 / (b2 << 7)) ^ (1 - -b2)) - 240050);
						goto IL_06ff;
					}
					goto IL_08e0;
					IL_09c6:
					pathNode3 = new PathNode(current, num3, num4, tackleMp, tackleAp, distance);
					num = (ushort)b2;
					if ((uint)(~(0 & (b3 / -22794067)) % b3) >= (uint)((873206285 >>> (int)num << (int)b3 >>> (int)((uint)b2 | ((uint)b3 % 2047433993u))) | (num >>> 21 >>> b3 / 1863603647)))
					{
						b3 = (sbyte)(-829370457 + (829436009 - num));
						goto IL_06ff;
					}
					goto IL_0729;
					IL_07a4:
					int availableMp2 = value2.AvailableMp;
					b3 = 84;
					b2 ^= -49;
					if (availableMp2 <= num3)
					{
						num = (ushort)((b3 ^ b3) + ((b2 ^ 0x29233609) >> (int)b3) / ~(b3 / b3 * (int)((uint)b2 % (uint)b2)));
						if ((int)((uint)(1536186511 >>> (int)num) % (uint)(((1218610608u > (uint)num) ? 1 : 0) - -1044924389)) + -399939428 < ((186967194u % (uint)(-516539715 + (b3 << 1)) == (uint)(num & 0x62900138)) ? 1 : 0))
						{
							b3 = (sbyte)(0x3CD ^ ((1059098389 >>> (int)b3) + b2));
							goto IL_06ff;
						}
						goto IL_09b4;
					}
					goto IL_0a7d;
					IL_0729:
					while (true)
					{
						b2 = 56;
						if ((((uint)(-125718770 / b2) > (uint)((1555929780 - b2) & b2)) ? 1u : 0u) / ((uint)((1695462926 * b2) & (-1055453816 + b2)) % 4021255219u) != 0)
						{
							break;
						}
						current = enumerator.Current;
						if (dictionary.TryGetValue(current, out value2))
						{
							if (1409155018 + b2 == b2 * ((int)((uint)b2 % 2101110042u) >> (int)b2) * (int)(982999094u / (uint)(~(b2 ^ b2))))
							{
								continue;
							}
							goto IL_07a4;
						}
						goto IL_08e0;
					}
					goto IL_08c2;
					IL_09b4:
					dictionary2[current] = new MoveNode(tackleCost, cellId2, reachable);
					goto IL_09c6;
					IL_0a37:
					b3 = (sbyte)((num & -1468853027) + (int)(((num > b3) ? 1u : 0u) << (int)b2) * -409919080 + 872409983);
					dictionary[current] = pathNode3;
					if (pathNode2.Distance < value)
					{
						list.Add(pathNode3);
					}
					goto IL_0a7d;
					end_IL_06a9:;
				}
				finally
				{
					num = 32849;
					((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
				goto IL_0b44;
			}
			break;
		}
		goto IL_018d;
	}
}
