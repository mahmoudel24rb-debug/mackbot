// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.LuaManager.ScriptManager
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Com.Ankama.Dofus.Server.Game.Protocol;
using DofusLibrary.Common;
using DofusLibrary.Common.Character;
using DofusLibrary.Common.Fight;
using DofusLibrary.Common.LuaManager;
using DofusLibrary.Common.Repository;
using Google.Protobuf.Collections;
using JitsuriProto;
using NLua;

internal class ScriptManager
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public int currentMapId;

		public string currentPosition;

		public _003C_003Ec__DisplayClass14_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleActions_003Eb__0(LuaTable action)
		{
			return _043A6B05._1434D187(_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "map")?.ToString(), _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref currentMapId));
		}

		internal bool _003CHandleActions_003Eb__1(LuaTable action)
		{
			return _043A6B05._1434D187(_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "map")?.ToString(), currentPosition);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_1
	{
		public int x;

		public int y;

		public _003C_003Ec__DisplayClass14_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleActions_003Eb__3(InstanceData member)
		{
			return TravelToAsync(member, x, y);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_2
	{
		public int mapId;

		public _003C_003Ec__DisplayClass14_2()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleActions_003Eb__6(InstanceData member)
		{
			return TravelToAsync(member, mapId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass18_0
	{
		public int[] elements;

		public ScriptManager _003C_003E4__this;

		public _9F8EAF35 characterMp;

		public _003C_003Ec__DisplayClass18_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleGather_003Eb__1(JitsuriInteractiveElement e)
		{
			_003C_003Ec__DisplayClass18_1 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass18_1();
			CS_0024_003C_003E8__locals6.e = e;
			if (Enumerable.Contains(elements, _671BC22C.BF9F3D1F.FE8E0C9E[805](CS_0024_003C_003E8__locals6.e)) && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals6.e) != null && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals6.e).Count != 0)
			{
				JitsuriStatedElement? jitsuriStatedElement = _003C_003E4__this.InstanceData.MapInformation.StatedElements.Find((JitsuriStatedElement statedElement) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals6.e));
				if (jitsuriStatedElement != null && A98A1B8C._31A2CD33(jitsuriStatedElement) == 0)
				{
					return _671BC22C.BF9F3D1F.FE8E0C9E[853](CS_0024_003C_003E8__locals6.e);
				}
			}
			return false;
		}

		internal bool _003CHandleGather_003Eb__2(JitsuriInteractiveElement e)
		{
			_003C_003Ec__DisplayClass18_2 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass18_2();
			CS_0024_003C_003E8__locals7.e = e;
			if (!_003C_003E4__this.ElementIdErrorsOnGatherOnCurrentMap.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals7.e)) && Enumerable.Contains(elements, _671BC22C.BF9F3D1F.FE8E0C9E[805](CS_0024_003C_003E8__locals7.e)) && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals7.e) != null && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals7.e).Count != 0)
			{
				JitsuriStatedElement? jitsuriStatedElement = _003C_003E4__this.InstanceData.MapInformation.StatedElements.Find((JitsuriStatedElement statedElement) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals7.e));
				if (jitsuriStatedElement != null && A98A1B8C._31A2CD33(jitsuriStatedElement) == 0)
				{
					return _671BC22C.BF9F3D1F.FE8E0C9E[853](CS_0024_003C_003E8__locals7.e);
				}
			}
			return false;
		}

		internal int _003CHandleGather_003Eb__3(JitsuriInteractiveElement e)
		{
			_003C_003Ec__DisplayClass18_3 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass18_3();
			CS_0024_003C_003E8__locals2.e = e;
			_9F8EAF35 fF9851AF = new _9F8EAF35(_671BC22C.BF9F3D1F.FE8E0C9E[1284](_003C_003E4__this.InstanceData.MapInformation.StatedElements.Find((JitsuriStatedElement statedElement) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals2.e))));
			return characterMp.FB2C1732(fF9851AF);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass18_1
	{
		public JitsuriInteractiveElement e;

		public _003C_003Ec__DisplayClass18_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleGather_003Eb__4(JitsuriStatedElement statedElement)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](e);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass18_2
	{
		public JitsuriInteractiveElement e;

		public _003C_003Ec__DisplayClass18_2()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleGather_003Eb__5(JitsuriStatedElement statedElement)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](e);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass18_3
	{
		public JitsuriInteractiveElement e;

		public _003C_003Ec__DisplayClass18_3()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleGather_003Eb__6(JitsuriStatedElement statedElement)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](e);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass19_0
	{
		public ScriptManager _003C_003E4__this;

		public int minMonsters;

		public int maxMonsters;

		public HashSet<int> forbiddenMonsters;

		public HashSet<int> forceMonsters;

		public Func<JitsuriMonsterInGroupInformation, bool> _003C_003E9__6;

		public _003C_003Ec__DisplayClass19_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CHandleFight_003Eb__2(JitsuriActorPositionInformation group)
		{
			JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriRolePlayActor.Types.JitsuriMonsterGroupActor jitsuriMonsterGroupActor = _671BC22C.BF9F3D1F.FE8E0C9E[147](group).RolePlayActor?.MonsterGroupActor;
			if (jitsuriMonsterGroupActor == null)
			{
				return false;
			}
			JitsuriMonsterInGroupInformation _8BB571AF = _671BC22C.BF9F3D1F.FE8E0C9E[423](jitsuriMonsterGroupActor.Identification);
			RepeatedField<JitsuriMonsterInGroupInformation> repeatedField = _671BC22C.BF9F3D1F.FE8E0C9E[1415](jitsuriMonsterGroupActor.Identification);
			int num = 1 + (repeatedField?.Count ?? 0);
			ScriptManager scriptManager = _003C_003E4__this;
			DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
			_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 39, 3);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "debug.fight.monsterGroupContent (");
			ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[444](group));
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " + ");
			ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF));
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " + ");
			D4B768B4._3DBED533(ref ADB0868E, BB97372F._1E072A97(", ", repeatedField?.Select(delegate(JitsuriMonsterInGroupInformation u)
			{
				int _4AB61FB = _671BC22C.BF9F3D1F.FE8E0C9E[1320](u);
				return _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref _4AB61FB);
			}) ?? new List<string>()));
			scriptManager.Log(new global::ED33D63D<string, global::_2788C194<long, int, string>>(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E), new global::_2788C194<long, int, string>(_671BC22C.BF9F3D1F.FE8E0C9E[444](group), _671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF), BB97372F._1E072A97(", ", repeatedField?.Select(delegate(JitsuriMonsterInGroupInformation u)
			{
				int _4AB61FB = _671BC22C.BF9F3D1F.FE8E0C9E[1320](u);
				return _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref _4AB61FB);
			}) ?? new List<string>()))), "gathering", D32842AE._543E5320._703EA0A0);
			if (num < minMonsters || num > maxMonsters)
			{
				return false;
			}
			if (forbiddenMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF)))
			{
				_003C_003E4__this.Log(new global::ED33D63D<string, global::_78311E8C<int, long>>("info.fight.skippedForbiddenMonster", new global::_78311E8C<int, long>(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF), _671BC22C.BF9F3D1F.FE8E0C9E[444](group))), "gathering", D32842AE._543E5320._703EA0A0);
				return false;
			}
			if (repeatedField != null)
			{
				using IEnumerator<JitsuriMonsterInGroupInformation> enumerator = repeatedField.GetEnumerator();
				while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
				{
					JitsuriMonsterInGroupInformation current = enumerator.Current;
					if (forbiddenMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](current)))
					{
						_003C_003E4__this.Log(new global::ED33D63D<string, global::_78311E8C<int, long>>("info.fight.skippedForbiddenMonster", new global::_78311E8C<int, long>(_671BC22C.BF9F3D1F.FE8E0C9E[1320](current), _671BC22C.BF9F3D1F.FE8E0C9E[444](group))), "gathering", D32842AE._543E5320._703EA0A0);
						return false;
					}
				}
			}
			if (forceMonsters.Count > 0 && !forceMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF)) && (repeatedField == null || !repeatedField.Any((JitsuriMonsterInGroupInformation u) => forceMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](u)))))
			{
				return false;
			}
			return true;
		}

		internal bool _003CHandleFight_003Eb__6(JitsuriMonsterInGroupInformation u)
		{
			return forceMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](u));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass20_0
	{
		public int leaderMapId;

		public ScriptManager _003C_003E4__this;

		public Stopwatch sw;

		public int i;

		public _003C_003Ec__DisplayClass20_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CWaitForTeamToBeInSameMap_003Eb__0(InstanceData e)
		{
			return e.MapInformation.MapId == leaderMapId;
		}

		internal bool _003CWaitForTeamToBeInSameMap_003Eb__1(InstanceData e)
		{
			return e.MapInformation.MapId != leaderMapId;
		}

		internal void _003CWaitForTeamToBeInSameMap_003Eb__2(InstanceData e)
		{
			ScriptManager scriptManager = _003C_003E4__this;
			string _970C = e.CharacterData.CharacterName;
			long mapId = e.MapInformation.MapId;
			TimeSpan CE12F = _671BC22C.BF9F3D1F.FE8E0C9E[1619](sw);
			scriptManager.Log(new global::ED33D63D<string, global::_53B198A0<string, long, double, double, int>>("info.team.memberNotOnLeaderMap", new global::_53B198A0<string, long, double, double, int>(_970C, mapId, _671BC22C.BF9F3D1F.FE8E0C9E[903](ref CE12F), 20.0 - (double)(i % 40) * 0.5, leaderMapId)), "gathering", D32842AE._543E5320._703EA0A0);
		}

		internal Task<bool> _003CWaitForTeamToBeInSameMap_003Eb__3(InstanceData ctx)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1566](ctx.GoToNearestZaap(_82210236.AB91771D(leaderMapId, B31AE737: true)));
			return ctx.MovePlayerToWorldMapId(leaderMapId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_0
	{
		public ScriptManager _003C_003E4__this;

		public Lua lua;

		public _003C_003Ec__DisplayClass21_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal void _003CInitializeLuaFunctions_003Eb__0(string message, string color)
		{
			string a11A83A = _671BC22C.BF9F3D1F.FE8E0C9E[235](color);
			D32842AE._543E5320 _543E = ((!_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "yellow")) ? (_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "green") ? D32842AE._543E5320._5D225697 : (_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "red") ? D32842AE._543E5320.B3B830AD : ((!_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "blue")) ? D32842AE._543E5320._703EA0A0 : D32842AE._543E5320._703EA0A0))) : D32842AE._543E5320._7C2AA78D);
			D32842AE._543E5320 dB93A = _543E;
			D32842AE._0700BABF("gathering", message, _003C_003E4__this.InstanceData.ProcessId, dB93A);
		}

		internal void _003CInitializeLuaFunctions_003Eb__2()
		{
			_003C_003E4__this.StopScript();
		}

		internal string _003CInitializeLuaFunctions_003Eb__3()
		{
			return _003C_003E4__this.GetPosition();
		}

		internal int _003CInitializeLuaFunctions_003Eb__4()
		{
			return (int)_003C_003E4__this.InstanceData.MapInformation.MapId;
		}

		internal int _003CInitializeLuaFunctions_003Eb__5()
		{
			return _003C_003E4__this.InstanceData.CharacterData.MaxPods;
		}

		internal int _003CInitializeLuaFunctions_003Eb__6()
		{
			return _003C_003E4__this.InstanceData.CharacterData.Pods;
		}

		internal int _003CInitializeLuaFunctions_003Eb__7()
		{
			return _003C_003E4__this.InstanceData.CharacterData.CharacterLevel;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__8(int gid)
		{
			Task<bool> maxItemByGid = _003C_003E4__this.GetMaxItemByGid(gid);
			_680DAE05._7422EE95(maxItemByGid);
			if (!maxItemByGid.Result)
			{
				_003C_003E4__this.StopScript(isStoppedManually: true);
			}
			return maxItemByGid.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__9(int doorMapId, string nickname, string code)
		{
			Task<bool> task = _003C_003E4__this.GoToAndEnterHouse(doorMapId, nickname, code);
			_680DAE05._7422EE95(task);
			if (!task.Result)
			{
				_003C_003E4__this.StopScript(isStoppedManually: true);
			}
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__10(int chestMapId, int chestCellId, string code)
		{
			Task<bool> task = _003C_003E4__this.MoveAndOpenChest(chestMapId, chestCellId, code);
			_680DAE05._7422EE95(task);
			if (!task.Result)
			{
				_003C_003E4__this.StopScript(isStoppedManually: true);
			}
			return task.Result;
		}

		internal int _003CInitializeLuaFunctions_003Eb__11(int gid)
		{
			if (_003C_003E4__this.InstanceData.Inventory.GetObjectByGid(gid) == null)
			{
				return 0;
			}
			return _671BC22C.BF9F3D1F.FE8E0C9E[289](_003C_003E4__this.InstanceData.Inventory.GetObjectByGid(gid));
		}

		internal bool _003CInitializeLuaFunctions_003Eb__12(int gid, int quantity)
		{
			_003C_003Ec__DisplayClass21_2 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass21_2();
			CS_0024_003C_003E8__locals4.gid = gid;
			CS_0024_003C_003E8__locals4.quantity = quantity;
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MessageHandler._368F20BF(CS_0024_003C_003E8__locals4.gid, CS_0024_003C_003E8__locals4.quantity));
		}

		internal int _003CInitializeLuaFunctions_003Eb__13()
		{
			return _003C_003E4__this.InstanceData.CharacterData.CharacterBreedId;
		}

		internal object _003CInitializeLuaFunctions_003Eb__14()
		{
			List<object> list = new List<object>();
			foreach (InstanceData item in _003C_003E4__this.TeamInstanceDataWithLeader)
			{
				list.Add(new Dictionary<string, object>
				{
					["breedId"] = item.CharacterData.CharacterBreedId,
					["name"] = item.CharacterData.CharacterName,
					["level"] = item.CharacterData.CharacterLevel,
					["id"] = item.CharacterData.CharacterId
				});
			}
			return list.ToArray();
		}

		internal bool _003CInitializeLuaFunctions_003Eb__15(string name, int agility, int strength, int vitality, int chance, int intelligence, int wisdom)
		{
			_003C_003Ec__DisplayClass21_3 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_3();
			CS_0024_003C_003E8__locals2.name = name;
			InstanceData instanceData = _003C_003E4__this.TeamInstanceDataWithLeader?.FirstOrDefault((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[155](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name, StringComparison.OrdinalIgnoreCase));
			if (instanceData == null)
			{
				return false;
			}
			Task<bool> task = instanceData.MessageHandler._251DD3AE(agility, strength, vitality, chance, intelligence, wisdom);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__16(int agility, int strength, int vitality, int chance, int intelligence, int wisdom)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.MessageHandler._251DD3AE(agility, strength, vitality, chance, intelligence, wisdom);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__17(int gid, int quantity)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.MessageHandler._368F20BF(gid, quantity);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__18(int uid, int position)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.MessageHandler._7C107FBB(uid, position);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__19(int uid)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.MessageHandler._7C107FBB(uid, 63);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal object _003CInitializeLuaFunctions_003Eb__20(int gid)
		{
			RawInventory rawInventory = _003C_003E4__this.InstanceData?.Inventory;
			if (rawInventory == null)
			{
				return null;
			}
			JitsuriObjectItem objectByGid = rawInventory.GetObjectByGid(gid);
			if (objectByGid == null)
			{
				return null;
			}
			LuaTable obj = (LuaTable)_671BC22C.BF9F3D1F.FE8E0C9E[468](lua, "return {}", "chunk")[0];
			F7BDF799.EB1D3905(obj, "uid", _671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid));
			F7BDF799.EB1D3905(obj, "gid", (int)_671BC22C.BF9F3D1F.FE8E0C9E[1827](objectByGid));
			F7BDF799.EB1D3905(obj, "quantity", _671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid));
			return obj;
		}

		internal object _003CInitializeLuaFunctions_003Eb__21(int position)
		{
			EquippedItem value = null;
			Dictionary<int, EquippedItem> dictionary = _003C_003E4__this.InstanceData.CharacterData.EquippedItems;
			if (dictionary != null)
			{
				dictionary.TryGetValue(position, out value);
			}
			else if (_003C_003E4__this.InstanceData.CharacterData.EquippedItems is IList<EquippedItem> list && position >= 0 && position < list.Count)
			{
				value = list[position];
			}
			if (value == null)
			{
				return null;
			}
			LuaTable obj = (LuaTable)_671BC22C.BF9F3D1F.FE8E0C9E[468](lua, "return {}", "chunk")[0];
			F7BDF799.EB1D3905(obj, "uid", (int)value.Uid);
			F7BDF799.EB1D3905(obj, "gid", (int)value.Gid);
			F7BDF799.EB1D3905(obj, "quantity", value.Quantity);
			return obj;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__22(int itemGid, int mapIdBank, int mapIdWorkshop, bool specialCraft, bool returnBank)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.CraftManager._97103C08(itemGid, mapIdBank, mapIdWorkshop, specialCraft, returnBank);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal void _003CInitializeLuaFunctions_003Eb__23()
		{
			_003C_003E4__this.InstanceData.NpcManager._37BBC281();
		}

		internal void _003CInitializeLuaFunctions_003Eb__24()
		{
			_003C_003E4__this.InstanceData.NpcManager._241B7A21();
		}

		internal void _003CInitializeLuaFunctions_003Eb__25()
		{
			_003C_003E4__this.InstanceData.NpcManager._5CBB2910();
		}

		internal void _003CInitializeLuaFunctions_003Eb__26()
		{
			_003C_003E4__this.InstanceData.NpcManager._4401C30C();
		}

		internal bool _003CInitializeLuaFunctions_003Eb__27(int gid, int quantity)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.MessageHandler._56B395BF(gid, quantity);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__28(int cellId)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.MovePlayerOnCellId(cellId);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__29(int cellId)
		{
			_003C_003Ec__DisplayClass21_4 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_4();
			CS_0024_003C_003E8__locals2.cellId = cellId;
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MovePlayerAndChangeMap(CS_0024_003C_003E8__locals2.cellId, -1));
		}

		internal bool _003CInitializeLuaFunctions_003Eb__30(int cellId)
		{
			_003C_003Ec__DisplayClass21_5 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_5();
			CS_0024_003C_003E8__locals2.cellId = cellId;
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MovePlayerOnCellId(CS_0024_003C_003E8__locals2.cellId));
		}

		internal bool _003CInitializeLuaFunctions_003Eb__31(int mapId)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1566](_003C_003E4__this.InstanceData.GoToNearestZaap(_82210236.AB91771D(mapId, B31AE737: true)));
			Task<bool> task = _003C_003E4__this.InstanceData.MovePlayerToWorldMapId(mapId);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__32(int npcActorId)
		{
			_003C_003Ec__DisplayClass21_6 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_6();
			CS_0024_003C_003E8__locals2.npcActorId = npcActorId;
			_671BC22C.BF9F3D1F.FE8E0C9E[1566](_003C_003E4__this.WaitForTeamToBeInSameMap());
			_680DAE05._7422EE95(D1380F8D.D62C4B25([AsyncStateMachine(typeof(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_60_003Ed))] () =>
			{
				_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_60_003Ed stateMachine = default(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_60_003Ed);
				stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
				stateMachine._003C_003E1__state = -1;
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
			}));
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager.CC0805BE(ctx.MapInformation.MapId, CS_0024_003C_003E8__locals2.npcActorId, 3));
		}

		internal bool _003CInitializeLuaFunctions_003Eb__33()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1566](_003C_003E4__this.WaitForTeamToBeInSameMap());
			_680DAE05._7422EE95(D1380F8D.D62C4B25([AsyncStateMachine(typeof(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_62_003Ed))] () =>
			{
				_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_62_003Ed stateMachine = default(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_62_003Ed);
				stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
				stateMachine._003C_003E1__state = -1;
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
			}));
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MessageHandler.FA2C3C86());
		}

		internal bool _003CInitializeLuaFunctions_003Eb__34(int npcId)
		{
			_003C_003Ec__DisplayClass21_7 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_7();
			CS_0024_003C_003E8__locals2.npcId = npcId;
			_671BC22C.BF9F3D1F.FE8E0C9E[1566](_003C_003E4__this.WaitForTeamToBeInSameMap());
			_680DAE05._7422EE95(D1380F8D.D62C4B25([AsyncStateMachine(typeof(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_64_003Ed))] () =>
			{
				_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_64_003Ed stateMachine = default(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_64_003Ed);
				stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
				stateMachine._003C_003E1__state = -1;
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
			}));
			return _003C_003E4__this.ExecuteForTeam(delegate(InstanceData ctx)
			{
				JitsuriActorPositionInformation jitsuriActorPositionInformation = ctx.MapInformation?.Actors?.FirstOrDefault((JitsuriActorPositionInformation a) => a != null && _671BC22C.BF9F3D1F.FE8E0C9E[148](a)?.RolePlayActor?.NpcActor?.NpcId == CS_0024_003C_003E8__locals2.npcId);
				return (jitsuriActorPositionInformation == null) ? Task.FromResult(result: false) : ctx.NpcManager.CC0805BE(ctx.MapInformation.MapId, (int)_671BC22C.BF9F3D1F.FE8E0C9E[444](jitsuriActorPositionInformation), 3);
			});
		}

		internal bool _003CInitializeLuaFunctions_003Eb__35(int replyIndex)
		{
			_003C_003Ec__DisplayClass21_8 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_8();
			CS_0024_003C_003E8__locals2.replyIndex = replyIndex;
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager._0399D809(CS_0024_003C_003E8__locals2.replyIndex));
		}

		internal bool _003CInitializeLuaFunctions_003Eb__36(int replyIndex)
		{
			_003C_003Ec__DisplayClass21_9 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_9();
			CS_0024_003C_003E8__locals2.replyIndex = replyIndex;
			return _003C_003E4__this.ExecuteForTeam(delegate(InstanceData ctx)
			{
				_003C_003Ec__DisplayClass21_10 obj = new _003C_003Ec__DisplayClass21_10();
				obj.ctx = ctx;
				Func<GameMessage, bool> e586CB2B = (GameMessage msg) => _671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) == GameMessage.ContentOneofCase.Event && _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](msg))), obj.ctx.MessageHandler.F1A6D798[_79AC42A1.MapComplementaryInformationEvent]);
				Task<GameMessage> result = obj.ctx.MessageHandler._8E0F0412(e586CB2B, 2000);
				obj.ctx.NpcManager._0399D809(CS_0024_003C_003E8__locals2.replyIndex);
				return result;
			});
		}

		internal bool _003CInitializeLuaFunctions_003Eb__37()
		{
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager.CC17DB97());
		}

		internal bool _003CInitializeLuaFunctions_003Eb__38(int cellId)
		{
			Task<bool> task = _003C_003E4__this.InstanceData.MoveAndUseInteractive(cellId);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__39(int cellId)
		{
			_003C_003Ec__DisplayClass21_11 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_11();
			CS_0024_003C_003E8__locals2.cellId = cellId;
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MoveAndUseInteractive(CS_0024_003C_003E8__locals2.cellId));
		}

		internal bool _003CInitializeLuaFunctions_003Eb__40(int cellId)
		{
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.HdvManager._4000929B(ctx.MapInformation.MapId));
		}

		internal bool _003CInitializeLuaFunctions_003Eb__41()
		{
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager.CC17DB97());
		}

		internal bool _003CInitializeLuaFunctions_003Eb__42()
		{
			return _003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.HdvManager._10232BBF());
		}

		internal bool _003CInitializeLuaFunctions_003Eb__43()
		{
			return _003C_003E4__this.InstanceData.CharacterData.IsFighting;
		}

		internal object _003CInitializeLuaFunctions_003Eb__44()
		{
			return _003C_003E4__this.InstanceData.FighterManager.Attackers.Select(FighterToLua).ToList();
		}

		internal object _003CInitializeLuaFunctions_003Eb__45()
		{
			return _003C_003E4__this.InstanceData.FighterManager.Defenders.Select(FighterToLua).ToList();
		}

		internal object _003CInitializeLuaFunctions_003Eb__46()
		{
			InstanceData instanceData = _003C_003E4__this.TeamInstanceDataWithLeader?.FirstOrDefault((InstanceData x) => x != null && x.FighterManager?.IsMyTurn == true);
			if (instanceData == null)
			{
				return null;
			}
			return new Dictionary<string, object>
			{
				["breedId"] = instanceData.CharacterData.CharacterBreedId,
				["name"] = instanceData.CharacterData.CharacterName,
				["level"] = instanceData.CharacterData.CharacterLevel,
				["id"] = instanceData.CharacterData.CharacterId
			};
		}

		internal object _003CInitializeLuaFunctions_003Eb__47(string name)
		{
			_003C_003Ec__DisplayClass21_12 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_12();
			CS_0024_003C_003E8__locals2.name = name;
			return _003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name))?.FighterManager.FindReachableCells();
		}

		internal bool _003CInitializeLuaFunctions_003Eb__48(string name, int cellId)
		{
			_003C_003Ec__DisplayClass21_13 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_13();
			CS_0024_003C_003E8__locals2.name = name;
			InstanceData instanceData = _003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name));
			if (instanceData == null)
			{
				return false;
			}
			int value = instanceData.FighterManager.GetSelf().Characteristics[CharacteristicKeyword.MovementPoints].Value;
			Task<bool> task = instanceData.FighterManager.Fighter.MoveTo(cellId, value);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__49(string name, int spellId, int cellId)
		{
			_003C_003Ec__DisplayClass21_14 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_14();
			CS_0024_003C_003E8__locals2.name = name;
			InstanceData instanceData = _003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name));
			if (instanceData == null)
			{
				return false;
			}
			Task<bool> task = instanceData.FighterManager.ForgeAndSendGameActionFightCastRequest(spellId, cellId);
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal object _003CInitializeLuaFunctions_003Eb__50(string name, int spellId)
		{
			_003C_003Ec__DisplayClass21_15 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_15();
			CS_0024_003C_003E8__locals2.name = name;
			return _003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name))?.FighterManager.GetSpellPossibleCells(spellId);
		}

		internal object _003CInitializeLuaFunctions_003Eb__51(string name, int spellId, int cellId)
		{
			_003C_003Ec__DisplayClass21_16 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_16();
			CS_0024_003C_003E8__locals2.name = name;
			return _003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name))?.FighterManager.GetSpellPossibleCellsOnCellId(spellId, cellId);
		}

		internal object _003CInitializeLuaFunctions_003Eb__52(string name, int spellId, int cellId)
		{
			_003C_003Ec__DisplayClass21_17 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_17();
			CS_0024_003C_003E8__locals2.name = name;
			return _003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name))?.FighterManager.GetSpellZoneOnCellId(spellId, cellId);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__53(string name)
		{
			_003C_003Ec__DisplayClass21_18 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass21_18();
			CS_0024_003C_003E8__locals2.name = name;
			InstanceData instanceData = _003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals2.name));
			if (instanceData == null)
			{
				return false;
			}
			Task<bool> task = instanceData.FighterManager.ForgeAndSendFightTurnReadyRequest();
			_680DAE05._7422EE95(task);
			return task.Result;
		}

		internal bool _003CInitializeLuaFunctions_003Eb__54(int gid)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[968](gid);
			JitsuriObjectItem objectByGid = _003C_003E4__this.InstanceData.Inventory.GetObjectByGid(gid);
			if (objectByGid != null)
			{
				ScriptManager scriptManager = _003C_003E4__this;
				DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
				_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 60, 2);
				_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "[DEBUG] Objet trouvé dans l'inventaire : UID = ");
				ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid));
				_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ", quantité = ");
				ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid));
				scriptManager.Log(new global::E5A6EF93<string>(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E)), "gathering", D32842AE._543E5320._5D225697);
				if (_671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid) <= 0)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 58, 1);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "[INFO] Objet GID ");
					ADB0868E.AppendFormatted(gid);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " présent mais quantité nulle ou invalide.");
					_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E));
					return false;
				}
				int num = _671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid);
				Task<bool> task = _003C_003E4__this.InstanceData.MessageHandler.DB02459F(_671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid), num);
				_671BC22C.BF9F3D1F.FE8E0C9E[1566](task);
				bool result = task.Result;
				_17348D39._53192AA0(result ? ConsoleColor.Green : ConsoleColor.Red);
				if (result)
				{
					ScriptManager scriptManager2 = _003C_003E4__this;
					_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 51, 3);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "[SUCCÈS] Objet GID ");
					ADB0868E.AppendFormatted(gid);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " (UID: ");
					ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid));
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ") supprimé avec succès x");
					ADB0868E.AppendFormatted(num);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ".");
					scriptManager2.Log(new global::E5A6EF93<string>(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E)), "gathering", (!result) ? D32842AE._543E5320.B3B830AD : D32842AE._543E5320._5D225697);
					return task.Result;
				}
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_1
	{
		[StructLayout(LayoutKind.Auto)]
		private struct _003C_003CInitializeLuaFunctions_003Eb__55_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public _003C_003Ec__DisplayClass21_1 _003C_003E4__this;

			private TaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				_003C_003Ec__DisplayClass21_1 _003C_003Ec__DisplayClass21_19 = _003C_003E4__this;
				try
				{
					TaskAwaiter _6D28EB9F;
					if (num != 0)
					{
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](_003C_003Ec__DisplayClass21_19.milliseconds));
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

		public int milliseconds;

		public _003C_003Ec__DisplayClass21_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		[AsyncStateMachine(typeof(_003C_003CInitializeLuaFunctions_003Eb__55_003Ed))]
		internal Task? _003CInitializeLuaFunctions_003Eb__55()
		{
			_003C_003CInitializeLuaFunctions_003Eb__55_003Ed stateMachine = default(_003C_003CInitializeLuaFunctions_003Eb__55_003Ed);
			stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
			stateMachine._003C_003E4__this = this;
			stateMachine._003C_003E1__state = -1;
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_10
	{
		public InstanceData ctx;

		public _003C_003Ec__DisplayClass21_10()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__69(GameMessage msg)
		{
			if (_671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) != GameMessage.ContentOneofCase.Event)
			{
				return false;
			}
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](msg))), ctx.MessageHandler.F1A6D798[_79AC42A1.MapComplementaryInformationEvent]);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_11
	{
		public int cellId;

		public _003C_003Ec__DisplayClass21_11()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CInitializeLuaFunctions_003Eb__71(InstanceData ctx)
		{
			return ctx.MoveAndUseInteractive(cellId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_12
	{
		public string name;

		public _003C_003Ec__DisplayClass21_12()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__76(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, name);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_13
	{
		public string name;

		public _003C_003Ec__DisplayClass21_13()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__77(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, name);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_14
	{
		public string name;

		public _003C_003Ec__DisplayClass21_14()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__78(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, name);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_15
	{
		public string name;

		public _003C_003Ec__DisplayClass21_15()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__79(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, name);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_16
	{
		public string name;

		public _003C_003Ec__DisplayClass21_16()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__80(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, name);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_17
	{
		public string name;

		public _003C_003Ec__DisplayClass21_17()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__81(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, name);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_18
	{
		public string name;

		public _003C_003Ec__DisplayClass21_18()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__82(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, name);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_2
	{
		public int gid;

		public int quantity;

		public _003C_003Ec__DisplayClass21_2()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CInitializeLuaFunctions_003Eb__56(InstanceData ctx)
		{
			return ctx.MessageHandler._368F20BF(gid, quantity);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_3
	{
		public string name;

		public _003C_003Ec__DisplayClass21_3()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__57(InstanceData it)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[155](it.CharacterData.CharacterName, name, StringComparison.OrdinalIgnoreCase);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_4
	{
		public int cellId;

		public _003C_003Ec__DisplayClass21_4()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CInitializeLuaFunctions_003Eb__58(InstanceData ctx)
		{
			return ctx.MovePlayerAndChangeMap(cellId, -1);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_5
	{
		public int cellId;

		public _003C_003Ec__DisplayClass21_5()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CInitializeLuaFunctions_003Eb__59(InstanceData ctx)
		{
			return ctx.MovePlayerOnCellId(cellId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_6
	{
		public int npcActorId;

		public _003C_003Ec__DisplayClass21_6()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CInitializeLuaFunctions_003Eb__61(InstanceData ctx)
		{
			return ctx.NpcManager.CC0805BE(ctx.MapInformation.MapId, npcActorId, 3);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_7
	{
		public int npcId;

		public Func<JitsuriActorPositionInformation, bool> _003C_003E9__66;

		public _003C_003Ec__DisplayClass21_7()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CInitializeLuaFunctions_003Eb__65(InstanceData ctx)
		{
			JitsuriActorPositionInformation jitsuriActorPositionInformation = ctx.MapInformation?.Actors?.FirstOrDefault((JitsuriActorPositionInformation a) => a != null && _671BC22C.BF9F3D1F.FE8E0C9E[148](a)?.RolePlayActor?.NpcActor?.NpcId == npcId);
			if (jitsuriActorPositionInformation == null)
			{
				return Task.FromResult(result: false);
			}
			return ctx.NpcManager.CC0805BE(ctx.MapInformation.MapId, (int)_671BC22C.BF9F3D1F.FE8E0C9E[444](jitsuriActorPositionInformation), 3);
		}

		internal bool _003CInitializeLuaFunctions_003Eb__66(JitsuriActorPositionInformation a)
		{
			if (a == null)
			{
				return false;
			}
			return _671BC22C.BF9F3D1F.FE8E0C9E[148](a)?.RolePlayActor?.NpcActor?.NpcId == npcId;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_8
	{
		public int replyIndex;

		public _003C_003Ec__DisplayClass21_8()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CInitializeLuaFunctions_003Eb__67(InstanceData ctx)
		{
			return ctx.NpcManager._0399D809(replyIndex);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_9
	{
		public int replyIndex;

		public _003C_003Ec__DisplayClass21_9()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<GameMessage> _003CInitializeLuaFunctions_003Eb__68(InstanceData ctx)
		{
			_003C_003Ec__DisplayClass21_10 obj = new _003C_003Ec__DisplayClass21_10();
			obj.ctx = ctx;
			Func<GameMessage, bool> e586CB2B = (GameMessage msg) => _671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) == GameMessage.ContentOneofCase.Event && _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](msg))), obj.ctx.MessageHandler.F1A6D798[_79AC42A1.MapComplementaryInformationEvent]);
			Task<GameMessage> result = obj.ctx.MessageHandler._8E0F0412(e586CB2B, 2000);
			obj.ctx.NpcManager._0399D809(replyIndex);
			return result;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass23_0
	{
		public ScriptManager _003C_003E4__this;

		public int maxPodsPercentage;

		public _003C_003Ec__DisplayClass23_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CIsInventoryFull_003Eb__0(InstanceData instanceData)
		{
			return _003C_003E4__this.IsCharacterFull(maxPodsPercentage, instanceData);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass24_0
	{
		public int gid;

		public _003C_003Ec__DisplayClass24_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal bool _003CGetMaxItemByGid_003Eb__0(JitsuriObjectItem i)
		{
			return _671BC22C.BF9F3D1F.FE8E0C9E[1827](i) == gid;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass29_0
	{
		public int gid;

		public _003C_003Ec__DisplayClass29_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CExecuteScript_003Eb__0(InstanceData memberData)
		{
			return DeleteOneItemAsync(memberData, gid);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public Func<InstanceData, Task<bool>> action;

		public _003C_003Ec__DisplayClass5_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CExecuteForTeam_003Eb__0(InstanceData e)
		{
			return action(e);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public Func<InstanceData, Task<GameMessage>> action;

		public _003C_003Ec__DisplayClass6_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<GameMessage> _003CExecuteForTeam_003Eb__0(InstanceData e)
		{
			return action(e);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public ScriptManager _003C_003E4__this;

		public string path;

		public string cell;

		public bool crafting;

		public int craftItemGid;

		public int craftBankMapId;

		public int craftWorkshopMapId;

		public bool craftSpecialWorkshop;

		public bool craftReturnBank;

		public int thresholdKamas;

		public int kamasToTake;

		public string lockedHouse;

		public _003C_003Ec__DisplayClass7_0()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleAction_003Eb__9(InstanceData member)
		{
			return member.MoveToDirection(_003C_003E4__this.PathToDirection(_671BC22C.BF9F3D1F.FE8E0C9E[235](path)));
		}

		internal Task<bool> _003CHandleAction_003Eb__11(InstanceData member)
		{
			return member.MovePlayerAndChangeMap(_671BC22C.BF9F3D1F.FE8E0C9E[1176](cell), -1);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_1
	{
		public int x;

		public int y;

		public _003C_003Ec__DisplayClass7_1()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleAction_003Eb__0(InstanceData member)
		{
			return TravelToAsync(member, x, y);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_2
	{
		public int mapId;

		public _003C_003Ec__DisplayClass7_2()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleAction_003Eb__3(InstanceData member)
		{
			return TravelToAsync(member, mapId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_3
	{
		public int zaapId;

		public _003C_003Ec__DisplayClass7_3()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleAction_003Eb__6(InstanceData member)
		{
			return TravelToAsync(member, zaapId);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_4
	{
		public InstanceData instanceData;

		public _003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals1;

		public _003C_003Ec__DisplayClass7_4()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleAction_003Eb__13(InstanceData member)
		{
			return CS_0024_003C_003E8__locals1._003C_003E4__this.NpcBankForInstanceData(instanceData, CS_0024_003C_003E8__locals1.crafting, CS_0024_003C_003E8__locals1.craftItemGid, CS_0024_003C_003E8__locals1.craftBankMapId, CS_0024_003C_003E8__locals1.craftWorkshopMapId, CS_0024_003C_003E8__locals1.craftSpecialWorkshop, CS_0024_003C_003E8__locals1.craftReturnBank, CS_0024_003C_003E8__locals1.thresholdKamas, CS_0024_003C_003E8__locals1.kamasToTake);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_5
	{
		public InstanceData instanceData;

		public _003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals2;

		public _003C_003Ec__DisplayClass7_5()
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[2099](this);
		}

		internal Task<bool> _003CHandleAction_003Eb__15(InstanceData member)
		{
			return CS_0024_003C_003E8__locals2._003C_003E4__this.LockedHouseForInstanceData(instanceData, CS_0024_003C_003E8__locals2.lockedHouse);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CHandleActions_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public ScriptManager _003C_003E4__this;

		public List<LuaTable> actions;

		private _003C_003Ec__DisplayClass14_0 _003C_003E8__1;

		public Lua lua;

		private int _003CoffsetIndex_003E5__2;

		private LuaTable _003CmatchingAction_003E5__3;

		private TaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<bool[]> _003C_003Eu__2;

		private TaskAwaiter<bool> _003C_003Eu__3;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			ScriptManager scriptManager = _003C_003E4__this;
			bool result;
			try
			{
				TaskAwaiter<bool[]> awaiter2;
				TaskAwaiter _6D28EB9F;
				LuaFunction luaFunction = default(LuaFunction);
				TaskAwaiter<bool> awaiter;
				bool[] result2;
				string text2;
				string cell;
				string door;
				bool npcBank;
				bool crafting;
				int craftItemGid;
				int craftBankMapId;
				int craftWorkshopMapId;
				bool craftSpecialWorkshop;
				bool craftReturnBank;
				string lockedHouse;
				string lockedStorage;
				bool chestGuild;
				int chestGuildNumber;
				int thresholdKamas;
				int kamasToTake;
				bool[] result3;
				bool flag;
				switch (num)
				{
				default:
					_003C_003E8__1 = new _003C_003Ec__DisplayClass14_0();
					flag = true;
					if (!scriptManager.isRunning)
					{
						result = true;
					}
					else
					{
						if (actions.Count != 0)
						{
							_003C_003E8__1.currentPosition = scriptManager.GetPosition();
							_003C_003E8__1.currentMapId = (int)scriptManager.InstanceData.MapInformation.MapId;
							scriptManager._lastMapId = _003C_003E8__1.currentMapId;
							if (scriptManager._lastMapId == _003C_003E8__1.currentMapId && scriptManager.InstanceData.NpcManager._29920FA6 != null && scriptManager.InstanceData.NpcManager._29920FA6.Count == 0)
							{
								_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
								if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
								{
									num = (_003C_003E1__state = 0);
									_003C_003Eu__1 = _6D28EB9F;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
									return;
								}
								goto IL_017c;
							}
							goto IL_018f;
						}
						result = true;
					}
					goto end_IL_000e;
				case 0:
					_6D28EB9F = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_017c;
				case 1:
					awaiter2 = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<bool[]>);
					num = (_003C_003E1__state = -1);
					goto IL_0540;
				case 2:
					awaiter = _003C_003Eu__3;
					_003C_003Eu__3 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_0673;
				case 3:
					awaiter2 = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<bool[]>);
					num = (_003C_003E1__state = -1);
					goto IL_073c;
				case 4:
					awaiter = _003C_003Eu__3;
					_003C_003Eu__3 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_0868;
				case 5:
					awaiter2 = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<bool[]>);
					num = (_003C_003E1__state = -1);
					goto IL_090b;
				case 6:
					awaiter = _003C_003Eu__3;
					_003C_003Eu__3 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_099e;
				case 7:
					_6D28EB9F = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_0a49;
				case 8:
					awaiter = _003C_003Eu__3;
					_003C_003Eu__3 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_0b27;
				case 9:
					awaiter = _003C_003Eu__3;
					_003C_003Eu__3 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_0ba1;
				case 10:
					try
					{
						if (num == 10)
						{
							awaiter = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							goto IL_0d03;
						}
						object[] array = _671BC22C.BF9F3D1F.FE8E0C9E[1888](luaFunction, Array.Empty<object>());
						if (scriptManager.isRunning)
						{
							if (array != null && array.Length != 0 && array[0] is LuaTable _2484EA)
							{
								awaiter = scriptManager.HandleActions(lua, _671BC22C.BF9F3D1F.FE8E0C9E[1291](_2484EA).OfType<LuaTable>().ToList(), isCustom: true).GetAwaiter();
								if (!awaiter.IsCompleted)
								{
									num = (_003C_003E1__state = 10);
									_003C_003Eu__3 = awaiter;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
									return;
								}
								goto IL_0d03;
							}
							goto IL_0d27;
						}
						result = false;
						goto end_IL_000e;
						IL_0d03:
						if (!awaiter.GetResult())
						{
							scriptManager.Log(new global::E5A6EF93<string>("error.script.customError"), "gathering", D32842AE._543E5320.B3B830AD);
						}
						goto IL_0d27;
						IL_0d27:
						if (_003C_003E8__1.currentMapId == (int)scriptManager.InstanceData.MapInformation.MapId)
						{
							goto end_IL_0c31;
						}
						result = true;
						goto end_IL_000e;
						end_IL_0c31:;
					}
					catch (Exception)
					{
						scriptManager.Log(new global::E5A6EF93<string>("error.script.customError"), "gathering", D32842AE._543E5320.B3B830AD);
						result = false;
						goto end_IL_000e;
					}
					goto IL_0d72;
				case 11:
					{
						awaiter = _003C_003Eu__3;
						_003C_003Eu__3 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						break;
					}
					IL_099e:
					flag = awaiter.GetResult();
					goto IL_09a6;
					IL_0b27:
					if (!awaiter.GetResult())
					{
						awaiter = scriptManager.HandleFight(lua, _003CmatchingAction_003E5__3).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 9);
							_003C_003Eu__3 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0ba1;
					}
					result = true;
					goto end_IL_000e;
					IL_09a6:
					if (flag)
					{
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](2000));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							num = (_003C_003E1__state = 7);
							_003C_003Eu__1 = _6D28EB9F;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
							return;
						}
						goto IL_0a49;
					}
					scriptManager.Log(new global::E5A6EF93<string>("error.script.cantHavenbag"), "gathering", D32842AE._543E5320.B3B830AD);
					result = flag;
					goto end_IL_000e;
					IL_0ba1:
					if (awaiter.GetResult())
					{
						result = true;
					}
					else
					{
						scriptManager.ElementIdErrorsOnGatherOnCurrentMap = new List<int>();
						LuaTable item = _003CmatchingAction_003E5__3;
						int num2 = actions.ToList().IndexOf(item);
						scriptManager.LastActionIndex = num2 + _003CoffsetIndex_003E5__2;
						if (!scriptManager.InstanceData.CharacterData.IsFighting)
						{
							luaFunction = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "custom") as LuaFunction;
							if (luaFunction != null)
							{
								goto case 10;
							}
							goto IL_0d72;
						}
						result = true;
					}
					goto end_IL_000e;
					IL_0868:
					result = awaiter.GetResult();
					goto end_IL_000e;
					IL_090b:
					flag = awaiter2.GetResult().All((bool r) => r);
					goto IL_09a6;
					IL_0673:
					result = awaiter.GetResult();
					goto end_IL_000e;
					IL_017c:
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					goto IL_018f;
					IL_018f:
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[744](_003C_003E8__1.currentPosition) && _003C_003E8__1.currentMapId != 0)
					{
						_003CoffsetIndex_003E5__2 = 0;
						if (scriptManager.LastActionIndex != -1 && scriptManager.LastActionIndex < actions.Count - 1)
						{
							string a11A83A = _671BC22C.BF9F3D1F.FE8E0C9E[1090](actions[scriptManager.LastActionIndex + 1], "map")?.ToString();
							if (_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, _003C_003E8__1.currentPosition) || _671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref _003C_003E8__1.currentMapId)))
							{
								actions = actions.ToList().Skip(scriptManager.LastActionIndex + 1).ToList();
								_003CoffsetIndex_003E5__2 = scriptManager.LastActionIndex + 1;
							}
						}
						_003CmatchingAction_003E5__3 = actions.OfType<LuaTable>().FirstOrDefault((LuaTable action) => _043A6B05._1434D187(_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "map")?.ToString(), _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref _003C_003E8__1.currentMapId)));
						if (_003CmatchingAction_003E5__3 == null)
						{
							_003CmatchingAction_003E5__3 = actions.OfType<LuaTable>().FirstOrDefault((LuaTable action) => _043A6B05._1434D187(_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "map")?.ToString(), _003C_003E8__1.currentPosition));
						}
						LuaTable luaTable = actions.OfType<LuaTable>().FirstOrDefault((LuaTable action) => _671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "map") != null);
						if (_003CmatchingAction_003E5__3 == null)
						{
							scriptManager.LastActionIndex = -1;
							string _563DC = ((luaTable == null) ? "none..." : _671BC22C.BF9F3D1F.FE8E0C9E[1090](luaTable, "map").ToString());
							scriptManager.Log(new global::ED33D63D<string, global::_6B08BC8D<string, int, string>>("warning.script.noAction", new global::_6B08BC8D<string, int, string>(_003C_003E8__1.currentPosition, _003C_003E8__1.currentMapId, _563DC)), "gathering", D32842AE._543E5320._7C2AA78D);
							if (luaTable != null)
							{
								string text = _671BC22C.BF9F3D1F.FE8E0C9E[1090](luaTable, "map")?.ToString();
								if (_671BC22C.BF9F3D1F.FE8E0C9E[744](text) || !_671BC22C.BF9F3D1F.FE8E0C9E[1130](text, ","))
								{
									_003C_003Ec__DisplayClass14_2 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass14_2();
									CS_0024_003C_003E8__locals8.mapId = _671BC22C.BF9F3D1F.FE8E0C9E[672](text);
									if (scriptManager.IsInTeam)
									{
										awaiter2 = Task.WhenAll(scriptManager.TeamInstanceDataWithLeader.Select((InstanceData member) => TravelToAsync(member, CS_0024_003C_003E8__locals8.mapId)).ToArray()).GetAwaiter();
										if (!awaiter2.IsCompleted)
										{
											num = (_003C_003E1__state = 3);
											_003C_003Eu__2 = awaiter2;
											_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
											return;
										}
										goto IL_073c;
									}
									awaiter = TravelToAsync(scriptManager.InstanceData, CS_0024_003C_003E8__locals8.mapId).GetAwaiter();
									if (!awaiter.IsCompleted)
									{
										num = (_003C_003E1__state = 4);
										_003C_003Eu__3 = awaiter;
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
										return;
									}
									goto IL_0868;
								}
								string[] array2 = _671BC22C.BF9F3D1F.FE8E0C9E[1565](text, ',', StringSplitOptions.None);
								if (array2.Length == 2)
								{
									_003C_003Ec__DisplayClass14_1 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass14_1();
									string c7B31F = _671BC22C.BF9F3D1F.FE8E0C9E[232](array2[0]);
									string c7B31F2 = _671BC22C.BF9F3D1F.FE8E0C9E[232](array2[1]);
									if (_671BC22C.BF9F3D1F.FE8E0C9E[422](c7B31F, ref CS_0024_003C_003E8__locals10.x) && _671BC22C.BF9F3D1F.FE8E0C9E[422](c7B31F2, ref CS_0024_003C_003E8__locals10.y))
									{
										if (scriptManager.IsInTeam)
										{
											awaiter2 = Task.WhenAll(scriptManager.TeamInstanceDataWithLeader.Select((InstanceData member) => TravelToAsync(member, CS_0024_003C_003E8__locals10.x, CS_0024_003C_003E8__locals10.y)).ToArray()).GetAwaiter();
											if (!awaiter2.IsCompleted)
											{
												num = (_003C_003E1__state = 1);
												_003C_003Eu__2 = awaiter2;
												_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
												return;
											}
											goto IL_0540;
										}
										awaiter = TravelToAsync(scriptManager.InstanceData, CS_0024_003C_003E8__locals10.x, CS_0024_003C_003E8__locals10.y).GetAwaiter();
										if (!awaiter.IsCompleted)
										{
											num = (_003C_003E1__state = 2);
											_003C_003Eu__3 = awaiter;
											_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
											return;
										}
										goto IL_0673;
									}
									scriptManager.Log(new global::E5A6EF93<string>("error.script.unknownPosition"), "gathering", D32842AE._543E5320.B3B830AD);
								}
							}
							if (scriptManager.IsInTeam)
							{
								awaiter2 = Task.WhenAll(scriptManager.TeamInstanceDataWithLeader.Select((InstanceData member) => member.GoToHavenBag()).ToArray()).GetAwaiter();
								if (!awaiter2.IsCompleted)
								{
									num = (_003C_003E1__state = 5);
									_003C_003Eu__2 = awaiter2;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
									return;
								}
								goto IL_090b;
							}
							awaiter = scriptManager.InstanceData.GoToHavenBag().GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 6);
								_003C_003Eu__3 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_099e;
						}
						awaiter = scriptManager.HandleGather(lua, _003CmatchingAction_003E5__3).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 8);
							_003C_003Eu__3 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0b27;
					}
					scriptManager.Log(new global::E5A6EF93<string>("error.script.unknownPosition"), "gathering", D32842AE._543E5320.B3B830AD);
					result = false;
					goto end_IL_000e;
					IL_0a49:
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					_003C_003E8__1.currentPosition = scriptManager.GetPosition();
					_003C_003E8__1.currentMapId = (int)scriptManager.InstanceData.MapInformation.MapId;
					scriptManager.Log(new global::ED33D63D<string, global::_6494D781<string, int>>("info.script.updatedPosition", new global::_6494D781<string, int>(_003C_003E8__1.currentPosition, _003C_003E8__1.currentMapId)), "gathering", D32842AE._543E5320._703EA0A0);
					result = false;
					goto end_IL_000e;
					IL_073c:
					result2 = awaiter2.GetResult();
					if (result2.All((bool r) => r))
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Green);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[SUCCÈS] Tous les membres sont arrivés.");
					}
					else
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[ERREUR] Au moins un membre n'a pas pu se déplacer.");
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[759]();
					result = result2.All((bool r) => r);
					goto end_IL_000e;
					IL_0d72:
					text2 = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "changeMap")?.ToString();
					if (text2 == null || _671BC22C.BF9F3D1F.FE8E0C9E[720](text2, _671BC22C.BF9F3D1F.FE8E0C9E[1733]()))
					{
						text2 = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "path")?.ToString();
					}
					cell = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "cell")?.ToString();
					door = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "door")?.ToString();
					npcBank = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "npcBank") != null && _671BC22C.BF9F3D1F.FE8E0C9E[788](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "npcBank"));
					crafting = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "crafting") != null && _671BC22C.BF9F3D1F.FE8E0C9E[788](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "crafting"));
					craftItemGid = ((_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftItemGid") != null) ? _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftItemGid")) : 0);
					craftBankMapId = ((_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftBankMapId") != null) ? _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftBankMapId")) : 0);
					craftWorkshopMapId = ((_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftWorkshopMapId") != null) ? _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftWorkshopMapId")) : 0);
					craftSpecialWorkshop = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftSpecialWorkshop") != null && _671BC22C.BF9F3D1F.FE8E0C9E[788](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "crafting"));
					craftReturnBank = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftReturnBank") != null && _671BC22C.BF9F3D1F.FE8E0C9E[788](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "craftReturnBank"));
					lockedHouse = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "lockedHouse")?.ToString();
					lockedStorage = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "lockedStorage")?.ToString();
					chestGuild = _671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "chestGuild") != null && _671BC22C.BF9F3D1F.FE8E0C9E[788](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "chestGuild"));
					chestGuildNumber = ((_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "chestGuildNumber") == null) ? 1 : _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "chestGuildNumber")));
					thresholdKamas = ((_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "thresholdKamas") != null) ? _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "thresholdKamas")) : 0);
					kamasToTake = ((_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "kamasToTake") != null) ? _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[1090](_003CmatchingAction_003E5__3, "kamasToTake")) : 0);
					awaiter = scriptManager.HandleAction(text2, cell, door, npcBank, crafting, craftItemGid, craftBankMapId, craftWorkshopMapId, craftSpecialWorkshop, craftReturnBank, lockedHouse, lockedStorage, chestGuild, chestGuildNumber, thresholdKamas, kamasToTake).GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (_003C_003E1__state = 11);
						_003C_003Eu__3 = awaiter;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						return;
					}
					break;
					IL_0540:
					result3 = awaiter2.GetResult();
					if (result3.All((bool r) => r))
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Green);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[SUCCÈS] Tous les membres sont arrivés.");
					}
					else
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[ERREUR] Au moins un membre n'a pas pu se déplacer.");
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[759]();
					result = result3.All((bool r) => r);
					goto end_IL_000e;
				}
				flag = awaiter.GetResult();
				result = flag;
				end_IL_000e:;
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003E8__1 = null;
				_003CmatchingAction_003E5__3 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003E8__1 = null;
			_003CmatchingAction_003E5__3 = null;
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
	private struct _003CHandleFight_003Ed__19 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public ScriptManager _003C_003E4__this;

		public LuaTable action;

		public Lua lua;

		private _003C_003Ec__DisplayClass19_0 _003C_003E8__1;

		private bool _003Cresult_003E5__2;

		private bool _003ClockSpec_003E5__3;

		private bool _003ClockJoin_003E5__4;

		private bool _003ClockParty_003E5__5;

		private List<JitsuriActorPositionInformation> _003CmonsterGroups_003E5__6;

		private JitsuriActorPositionInformation _003CgroupToFight_003E5__7;

		private int _003CtotalMonsters_003E5__8;

		private long _003CtargetMonsterGroupId_003E5__9;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private int _003Cattempt_003E5__10;

		private int _003CcellId_003E5__11;

		private int _003CfinalCellId_003E5__12;

		private TaskAwaiter<(bool SpectatorLocked, bool JoinLocked, bool joinParty)> _003C_003Eu__2;

		private TaskAwaiter _003C_003Eu__3;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			ScriptManager scriptManager = _003C_003E4__this;
			bool result;
			try
			{
				TaskAwaiter<bool> awaiter2;
				TaskAwaiter<(bool, bool, bool)> awaiter;
				TaskAwaiter _6D28EB9F;
				(bool, bool, bool) result3;
				bool result2;
				switch (num)
				{
				default:
					_003C_003E8__1 = new _003C_003Ec__DisplayClass19_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					if (scriptManager.InstanceData.CharacterData.IsFighting)
					{
						result = true;
					}
					else
					{
						if (_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "fight") != null && (bool)_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "fight"))
						{
							_003Cresult_003E5__2 = true;
							_003C_003E8__1.minMonsters = ((_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "MIN_MONSTERS") == null) ? 1 : _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "MIN_MONSTERS")));
							_003C_003E8__1.maxMonsters = ((_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "MAX_MONSTERS") != null) ? _671BC22C.BF9F3D1F.FE8E0C9E[1225](_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "MAX_MONSTERS")) : 8);
							_003ClockSpec_003E5__3 = _671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FIGHT_LOCK_SPEC") != null && (bool)_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FIGHT_LOCK_SPEC");
							_003ClockJoin_003E5__4 = _671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FIGHT_LOCK_JOIN") != null && (bool)_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FIGHT_LOCK_JOIN");
							_003ClockParty_003E5__5 = _671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FIGHT_LOCK_PARTY") != null && (bool)_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FIGHT_LOCK_PARTY");
							_003CmonsterGroups_003E5__6 = scriptManager.InstanceData.MapInformation.Actors;
							_003C_003E8__1.forbiddenMonsters = ((_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FORBIDDEN_MONSTERS") is LuaTable _2484EA) ? (from object v in _671BC22C.BF9F3D1F.FE8E0C9E[1291](_2484EA)
								select _671BC22C.BF9F3D1F.FE8E0C9E[1225](v)).ToHashSet() : new HashSet<int>());
							_003C_003E8__1.forceMonsters = ((_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "FORCE_MONSTERS") is LuaTable _2484EA2) ? (from object v in _671BC22C.BF9F3D1F.FE8E0C9E[1291](_2484EA2)
								select _671BC22C.BF9F3D1F.FE8E0C9E[1225](v)).ToHashSet() : new HashSet<int>());
							scriptManager.Log(new global::ED33D63D<string, global::_57A1C735<string>>(_671BC22C.BF9F3D1F.FE8E0C9E[1797]("debug.fight.forceMonstersLoaded: ", string.Join(", ", _003C_003E8__1.forceMonsters)), new global::_57A1C735<string>(string.Join(", ", _003C_003E8__1.forceMonsters))), "gathering", D32842AE._543E5320._703EA0A0);
							awaiter2 = scriptManager.WaitForTeamToBeInSameMap().GetAwaiter();
							if (!awaiter2.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter2;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
								return;
							}
							goto IL_041c;
						}
						result = false;
					}
					goto end_IL_000e;
				case 0:
					awaiter2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_041c;
				case 1:
					awaiter2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_05cb;
				case 2:
					awaiter2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_06c8;
				case 3:
					awaiter2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_07d0;
				case 4:
					awaiter = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<(bool, bool, bool)>);
					num = (_003C_003E1__state = -1);
					goto IL_089e;
				case 5:
					{
						_6D28EB9F = _003C_003Eu__3;
						_003C_003Eu__3 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_09be;
					}
					IL_09e3:
					if (_003Cattempt_003E5__10 > 3)
					{
						break;
					}
					_003CcellId_003E5__11 = _671BC22C.BF9F3D1F.FE8E0C9E[1466](_671BC22C.BF9F3D1F.FE8E0C9E[799](_003CgroupToFight_003E5__7));
					scriptManager.Log(new global::ED33D63D<string, global::_79B4883B<long, int, int>>("success.fight.moveToGroup", new global::_79B4883B<long, int, int>(_003CtargetMonsterGroupId_003E5__9, _003CcellId_003E5__11, _003CtotalMonsters_003E5__8)), "gathering", D32842AE._543E5320._5D225697);
					awaiter2 = scriptManager.InstanceData.MovePlayerOnCellId(_003CcellId_003E5__11).GetAwaiter();
					if (!awaiter2.IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__1 = awaiter2;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
						return;
					}
					goto IL_05cb;
					IL_07d0:
					result2 = awaiter2.GetResult();
					_003Cresult_003E5__2 = result2;
					if (_003Cresult_003E5__2)
					{
						scriptManager.Log(new global::E5A6EF93<string>("success.fight.startFightSuccessfully"), "gathering", D32842AE._543E5320._5D225697);
						if (!(_003ClockSpec_003E5__3 | _003ClockJoin_003E5__4 | _003ClockParty_003E5__5) || scriptManager.hasHandledLock)
						{
							break;
						}
						scriptManager.hasHandledLock = true;
						awaiter = HandleLockSettings(_003ClockSpec_003E5__3, _003ClockJoin_003E5__4, _003ClockParty_003E5__5).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 4);
							_003C_003Eu__2 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_089e;
					}
					scriptManager.Log(new global::CE3BCE30<string, int>("warning.fight.errorWhenAttacking", _003Cattempt_003E5__10), "gathering", D32842AE._543E5320._7C2AA78D);
					_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](200));
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
					{
						num = (_003C_003E1__state = 5);
						_003C_003Eu__3 = _6D28EB9F;
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref _6D28EB9F, ref this);
						return;
					}
					goto IL_09be;
					IL_070d:
					scriptManager.Log(new global::E5A6EF93<string>("success.fight.startFight"), "gathering", D32842AE._543E5320._5D225697);
					if (scriptManager.InstanceData.MapInformation.IsActorIdOnMap(_003CtargetMonsterGroupId_003E5__9))
					{
						awaiter2 = scriptManager.InstanceData.MessageHandler._5B3DDA34(_003CtargetMonsterGroupId_003E5__9).GetAwaiter();
						if (!awaiter2.IsCompleted)
						{
							num = (_003C_003E1__state = 3);
							_003C_003Eu__1 = awaiter2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
							return;
						}
						goto IL_07d0;
					}
					scriptManager.Log(new global::ED33D63D<string, global::_5A807736<long>>("info.fight.monsterGroupAlreadyGone", new global::_5A807736<long>(_003CtargetMonsterGroupId_003E5__9)), "gathering", D32842AE._543E5320._703EA0A0);
					result = true;
					goto end_IL_000e;
					IL_09be:
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					_003Cattempt_003E5__10++;
					goto IL_09e3;
					IL_089e:
					result3 = awaiter.GetResult();
					if (!result3.Item1 && !result3.Item2)
					{
						scriptManager.Log(new global::E5A6EF93<string>("error.fight.lockSettingsFailed"), "fight", D32842AE._543E5320.B3B830AD);
						break;
					}
					if (result3.Item1)
					{
						scriptManager.Log(new global::E5A6EF93<string>("info.fight.spectatorLockActivated"), "gathering", D32842AE._543E5320._703EA0A0);
					}
					if (result3.Item2)
					{
						scriptManager.Log(new global::E5A6EF93<string>("info.fight.joinLockActivated"), "gathering", D32842AE._543E5320._703EA0A0);
					}
					break;
					IL_041c:
					awaiter2.GetResult();
					_003CgroupToFight_003E5__7 = _003CmonsterGroups_003E5__6.FirstOrDefault(delegate(JitsuriActorPositionInformation group)
					{
						JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriRolePlayActor.Types.JitsuriMonsterGroupActor jitsuriMonsterGroupActor2 = _671BC22C.BF9F3D1F.FE8E0C9E[147](group).RolePlayActor?.MonsterGroupActor;
						if (jitsuriMonsterGroupActor2 == null)
						{
							return false;
						}
						JitsuriMonsterInGroupInformation _8BB571AF = _671BC22C.BF9F3D1F.FE8E0C9E[423](jitsuriMonsterGroupActor2.Identification);
						RepeatedField<JitsuriMonsterInGroupInformation> repeatedField = _671BC22C.BF9F3D1F.FE8E0C9E[1415](jitsuriMonsterGroupActor2.Identification);
						int num2 = 1 + (repeatedField?.Count ?? 0);
						ScriptManager scriptManager2 = _003C_003E8__1._003C_003E4__this;
						DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
						_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 39, 3);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "debug.fight.monsterGroupContent (");
						ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[444](group));
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " + ");
						ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF));
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " + ");
						D4B768B4._3DBED533(ref ADB0868E, BB97372F._1E072A97(", ", repeatedField?.Select(delegate(JitsuriMonsterInGroupInformation u)
						{
							int _4AB61FB = _671BC22C.BF9F3D1F.FE8E0C9E[1320](u);
							return _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref _4AB61FB);
						}) ?? new List<string>()));
						scriptManager2.Log(new global::ED33D63D<string, global::_2788C194<long, int, string>>(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E), new global::_2788C194<long, int, string>(_671BC22C.BF9F3D1F.FE8E0C9E[444](group), _671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF), BB97372F._1E072A97(", ", repeatedField?.Select(delegate(JitsuriMonsterInGroupInformation u)
						{
							int _4AB61FB = _671BC22C.BF9F3D1F.FE8E0C9E[1320](u);
							return _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref _4AB61FB);
						}) ?? new List<string>()))), "gathering", D32842AE._543E5320._703EA0A0);
						if (num2 < _003C_003E8__1.minMonsters || num2 > _003C_003E8__1.maxMonsters)
						{
							return false;
						}
						if (_003C_003E8__1.forbiddenMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF)))
						{
							_003C_003E8__1._003C_003E4__this.Log(new global::ED33D63D<string, global::_78311E8C<int, long>>("info.fight.skippedForbiddenMonster", new global::_78311E8C<int, long>(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF), _671BC22C.BF9F3D1F.FE8E0C9E[444](group))), "gathering", D32842AE._543E5320._703EA0A0);
							return false;
						}
						if (repeatedField != null)
						{
							using IEnumerator<JitsuriMonsterInGroupInformation> enumerator = repeatedField.GetEnumerator();
							while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
							{
								JitsuriMonsterInGroupInformation current = enumerator.Current;
								if (_003C_003E8__1.forbiddenMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](current)))
								{
									_003C_003E8__1._003C_003E4__this.Log(new global::ED33D63D<string, global::_78311E8C<int, long>>("info.fight.skippedForbiddenMonster", new global::_78311E8C<int, long>(_671BC22C.BF9F3D1F.FE8E0C9E[1320](current), _671BC22C.BF9F3D1F.FE8E0C9E[444](group))), "gathering", D32842AE._543E5320._703EA0A0);
									return false;
								}
							}
						}
						return (_003C_003E8__1.forceMonsters.Count <= 0 || _003C_003E8__1.forceMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](_8BB571AF)) || (repeatedField != null && repeatedField.Any((JitsuriMonsterInGroupInformation u) => _003C_003E8__1.forceMonsters.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[1320](u))))) ? true : false;
					});
					if (_003CgroupToFight_003E5__7 == null)
					{
						result = false;
					}
					else
					{
						JitsuriActorPositionInformation.Types.JitsuriActorInformation.Types.JitsuriRolePlayActor.Types.JitsuriMonsterGroupActor jitsuriMonsterGroupActor = _671BC22C.BF9F3D1F.FE8E0C9E[147](_003CgroupToFight_003E5__7).RolePlayActor?.MonsterGroupActor;
						if (jitsuriMonsterGroupActor != null)
						{
							_671BC22C.BF9F3D1F.FE8E0C9E[423](jitsuriMonsterGroupActor.Identification);
							_003CtotalMonsters_003E5__8 = 1 + (_671BC22C.BF9F3D1F.FE8E0C9E[1415](jitsuriMonsterGroupActor.Identification)?.Count ?? 0);
							_003CtargetMonsterGroupId_003E5__9 = _671BC22C.BF9F3D1F.FE8E0C9E[444](_003CgroupToFight_003E5__7);
							_003Cattempt_003E5__10 = 1;
							goto IL_09e3;
						}
						result = false;
					}
					goto end_IL_000e;
					IL_06c8:
					result2 = awaiter2.GetResult();
					_003Cresult_003E5__2 = result2;
					if (_003Cresult_003E5__2)
					{
						_003CcellId_003E5__11 = _003CfinalCellId_003E5__12;
						goto IL_070d;
					}
					scriptManager.Log(new global::E5A6EF93<string>("error.fight.cellChangedAndMove"), "gathering", D32842AE._543E5320._7C2AA78D);
					result = true;
					goto end_IL_000e;
					IL_05cb:
					result2 = awaiter2.GetResult();
					_003Cresult_003E5__2 = result2;
					if (_003Cresult_003E5__2)
					{
						_003CfinalCellId_003E5__12 = _671BC22C.BF9F3D1F.FE8E0C9E[1466](_671BC22C.BF9F3D1F.FE8E0C9E[799](_003CgroupToFight_003E5__7));
						if (_003CfinalCellId_003E5__12 != _003CcellId_003E5__11)
						{
							scriptManager.Log(new global::ED33D63D<string, global::_5200DA3B<int>>("info.fight.cellChanged", new global::_5200DA3B<int>(_003CfinalCellId_003E5__12)), "gathering", D32842AE._543E5320._703EA0A0);
							awaiter2 = scriptManager.InstanceData.MovePlayerOnCellId(_003CfinalCellId_003E5__12).GetAwaiter();
							if (!awaiter2.IsCompleted)
							{
								num = (_003C_003E1__state = 2);
								_003C_003Eu__1 = awaiter2;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
								return;
							}
							goto IL_06c8;
						}
						goto IL_070d;
					}
					scriptManager.Log(new global::E5A6EF93<string>("error.fight.moveToGroup"), "gathering", D32842AE._543E5320._7C2AA78D);
					result = true;
					goto end_IL_000e;
				}
				if (!_003Cresult_003E5__2)
				{
					scriptManager.Log(new global::E5A6EF93<string>("error.fight.allRetriesFailed"), "gathering", D32842AE._543E5320.B3B830AD);
				}
				result = true;
				end_IL_000e:;
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003E8__1 = null;
				_003CmonsterGroups_003E5__6 = null;
				_003CgroupToFight_003E5__7 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003E8__1 = null;
			_003CmonsterGroups_003E5__6 = null;
			_003CgroupToFight_003E5__7 = null;
			_003C_003Et__builder.SetResult(result);
			async Task<(bool SpectatorLocked, bool JoinLocked, bool joinParty)> HandleLockSettings(bool lockSpec, bool lockJoin, bool lockParty)
			{
				bool spectatorLocked = true;
				bool joinLocked = true;
				bool joinParty = true;
				if (lockSpec)
				{
					spectatorLocked = await ((_003C_003Ec__DisplayClass19_0)(object)this)._003C_003E4__this.InstanceData.FighterManager.ForgeAndSendFightOptionToggleRequest(FightOption.SetSecret);
				}
				if (lockJoin && !((_003C_003Ec__DisplayClass19_0)(object)this)._003C_003E4__this.IsInTeam)
				{
					joinLocked = await ((_003C_003Ec__DisplayClass19_0)(object)this)._003C_003E4__this.InstanceData.FighterManager.ForgeAndSendFightOptionToggleRequest(FightOption.SetClosed);
				}
				if (lockParty)
				{
					joinLocked = await ((_003C_003Ec__DisplayClass19_0)(object)this)._003C_003E4__this.InstanceData.FighterManager.ForgeAndSendFightOptionToggleRequest(FightOption.SetToPartyOnly);
				}
				return (SpectatorLocked: spectatorLocked, JoinLocked: joinLocked, joinParty: joinParty);
			}
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
	private struct _003CHandleGather_003Ed__18 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public ScriptManager _003C_003E4__this;

		public LuaTable action;

		public Lua lua;

		private JitsuriInteractiveElement _003Celement_003E5__2;

		private TaskAwaiter<int> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			ScriptManager scriptManager = _003C_003E4__this;
			bool result;
			try
			{
				TaskAwaiter<int> awaiter;
				if (num == 0)
				{
					awaiter = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<int>);
					num = (_003C_003E1__state = -1);
					goto IL_0247;
				}
				_003C_003Ec__DisplayClass18_0 CS_0024_003C_003E8__locals25 = new _003C_003Ec__DisplayClass18_0();
				CS_0024_003C_003E8__locals25._003C_003E4__this = _003C_003E4__this;
				if (scriptManager.InstanceData.CharacterData.IsFighting)
				{
					result = true;
				}
				else if (_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "gather") == null || !(bool)_671BC22C.BF9F3D1F.FE8E0C9E[1090](action, "gather"))
				{
					result = false;
				}
				else if (!(_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "ELEMENTS_TO_GATHER") is LuaTable _2484EA))
				{
					result = false;
				}
				else
				{
					CS_0024_003C_003E8__locals25.elements = (from object e in _671BC22C.BF9F3D1F.FE8E0C9E[1291](_2484EA)
						select _671BC22C.BF9F3D1F.FE8E0C9E[1225](e)).ToArray();
					List<JitsuriInteractiveElement> interactiveElements = scriptManager.InstanceData.MapInformation.InteractiveElements;
					if (interactiveElements.Where(delegate(JitsuriInteractiveElement e)
					{
						_003C_003Ec__DisplayClass18_1 CS_0024_003C_003E8__locals26 = new _003C_003Ec__DisplayClass18_1();
						CS_0024_003C_003E8__locals26.e = e;
						if (Enumerable.Contains(CS_0024_003C_003E8__locals25.elements, _671BC22C.BF9F3D1F.FE8E0C9E[805](CS_0024_003C_003E8__locals26.e)) && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals26.e) != null && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals26.e).Count != 0)
						{
							JitsuriStatedElement? jitsuriStatedElement = CS_0024_003C_003E8__locals25._003C_003E4__this.InstanceData.MapInformation.StatedElements.Find((JitsuriStatedElement statedElement) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals26.e));
							if (jitsuriStatedElement != null && A98A1B8C._31A2CD33(jitsuriStatedElement) == 0)
							{
								return _671BC22C.BF9F3D1F.FE8E0C9E[853](CS_0024_003C_003E8__locals26.e);
							}
						}
						return false;
					}).ToList().Count == 0)
					{
						result = false;
					}
					else
					{
						List<JitsuriInteractiveElement> source = interactiveElements.Where(delegate(JitsuriInteractiveElement e)
						{
							_003C_003Ec__DisplayClass18_2 CS_0024_003C_003E8__locals27 = new _003C_003Ec__DisplayClass18_2();
							CS_0024_003C_003E8__locals27.e = e;
							if (!CS_0024_003C_003E8__locals25._003C_003E4__this.ElementIdErrorsOnGatherOnCurrentMap.Contains(_671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals27.e)) && Enumerable.Contains(CS_0024_003C_003E8__locals25.elements, _671BC22C.BF9F3D1F.FE8E0C9E[805](CS_0024_003C_003E8__locals27.e)) && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals27.e) != null && _671BC22C.BF9F3D1F.FE8E0C9E[1251](CS_0024_003C_003E8__locals27.e).Count != 0)
							{
								JitsuriStatedElement? jitsuriStatedElement = CS_0024_003C_003E8__locals25._003C_003E4__this.InstanceData.MapInformation.StatedElements.Find((JitsuriStatedElement statedElement) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals27.e));
								if (jitsuriStatedElement != null && A98A1B8C._31A2CD33(jitsuriStatedElement) == 0)
								{
									return _671BC22C.BF9F3D1F.FE8E0C9E[853](CS_0024_003C_003E8__locals27.e);
								}
							}
							return false;
						}).ToList();
						CS_0024_003C_003E8__locals25.characterMp = new _9F8EAF35(scriptManager.InstanceData.CharacterData.CharacterCellId);
						List<JitsuriInteractiveElement> list = source.OrderBy(delegate(JitsuriInteractiveElement e)
						{
							_003C_003Ec__DisplayClass18_3 CS_0024_003C_003E8__locals28 = new _003C_003Ec__DisplayClass18_3();
							CS_0024_003C_003E8__locals28.e = e;
							_9F8EAF35 fF9851AF = new _9F8EAF35(_671BC22C.BF9F3D1F.FE8E0C9E[1284](CS_0024_003C_003E8__locals25._003C_003E4__this.InstanceData.MapInformation.StatedElements.Find((JitsuriStatedElement statedElement) => _671BC22C.BF9F3D1F.FE8E0C9E[1307](statedElement) == _671BC22C.BF9F3D1F.FE8E0C9E[855](CS_0024_003C_003E8__locals28.e))));
							return CS_0024_003C_003E8__locals25.characterMp.FB2C1732(fF9851AF);
						}).ToList();
						if (list.Count != 0)
						{
							_003Celement_003E5__2 = list[0];
							awaiter = scriptManager.InstanceData.MoveAndGather(_671BC22C.BF9F3D1F.FE8E0C9E[855](_003Celement_003E5__2), _671BC22C.BF9F3D1F.FE8E0C9E[1251](_003Celement_003E5__2)[0].SkillInstanceUid).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_0247;
						}
						result = false;
					}
				}
				goto end_IL_000e;
				IL_0247:
				int result2 = awaiter.GetResult();
				string fB14C = D12304A7.FA3D1E1C(_671BC22C.BF9F3D1F.FE8E0C9E[805](_003Celement_003E5__2));
				string text = _671BC22C.BF9F3D1F.FE8E0C9E[235](fB14C);
				scriptManager.Log(new global::ED33D63D<string, global::_3D09A024<string>>("info.script.gatheringRessource", new global::_3D09A024<string>(text)), "gathering", D32842AE._543E5320._703EA0A0);
				switch (result2)
				{
				case -2:
					scriptManager.Log(new global::E5A6EF93<string>("error.gathering.unkwownError"), "gathering", D32842AE._543E5320._7C2AA78D);
					scriptManager.ElementIdErrorsOnGatherOnCurrentMap.Add(_671BC22C.BF9F3D1F.FE8E0C9E[855](_003Celement_003E5__2));
					break;
				case -1:
					scriptManager.Log(new global::E5A6EF93<string>("error.gathering.alreadyGathered"), "gathering", D32842AE._543E5320._7C2AA78D);
					scriptManager.ElementIdErrorsOnGatherOnCurrentMap.Add(_671BC22C.BF9F3D1F.FE8E0C9E[855](_003Celement_003E5__2));
					break;
				case -3:
					scriptManager.Log(new global::E5A6EF93<string>("info.gathering.fightJoined"), "gathering", D32842AE._543E5320._7C2AA78D);
					result = true;
					goto end_IL_000e;
				default:
				{
					scriptManager.Log(new global::ED33D63D<string, global::D5038E9C<int, string>>("success.script.gatherSuccess", new global::D5038E9C<int, string>(result2, text)), "gathering", D32842AE._543E5320._5D225697);
					string text2 = _671BC22C.BF9F3D1F.FE8E0C9E[235](fB14C);
					if (text2 == null)
					{
						DefaultInterpolatedStringHandler CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](0, 1);
						CEA3933A.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[805](_003Celement_003E5__2));
						text2 = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A);
					}
					global::_293D2A82<string, int, DateTime, long> _2B922E = new global::_293D2A82<string, int, DateTime, long>(text2, result2, _671BC22C.BF9F3D1F.FE8E0C9E[1548](), scriptManager.InstanceData.CharacterData.CharacterId);
					_499DDEB5._3C11A432("GatheringStats", _671BC22C.BF9F3D1F.FE8E0C9E[1563](_2B922E), scriptManager.InstanceData.ProcessId);
					break;
				}
				}
				result = true;
				end_IL_000e:;
			}
			catch (Exception exception)
			{
				_003C_003E1__state = -2;
				_003Celement_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003Celement_003E5__2 = null;
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
	private struct _003CRunScript_003Ed__32 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public ScriptManager _003C_003E4__this;

		private CancellationTokenSource _003C_globalCts_003E5__2;

		private TaskCompletionSource<bool> _003CGlobalCancelTcs_003E5__3;

		private List<InstanceData>.Enumerator _003C_003E7__wrap3;

		private InstanceData _003CinstanceData_003E5__5;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			ScriptManager scriptManager = _003C_003E4__this;
			try
			{
				if (num == 0)
				{
					goto IL_0067;
				}
				TaskAwaiter<bool> awaiter;
				if (num != 1)
				{
					scriptManager.isRunning = true;
					scriptManager.LastActionIndex = -1;
					_003C_globalCts_003E5__2 = _671BC22C.BF9F3D1F.FE8E0C9E[421]();
					_003CGlobalCancelTcs_003E5__3 = new TaskCompletionSource<bool>();
					if (scriptManager.IsInTeam)
					{
						_003C_003E7__wrap3 = scriptManager.TeamInstanceDataWithLeader.GetEnumerator();
						goto IL_0067;
					}
					if (scriptManager.InstanceData.GlobalCancelTcs == null)
					{
						goto IL_0209;
					}
					awaiter = scriptManager.InstanceData.GlobalCancelTcs.Task.GetAwaiter();
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
				awaiter.GetResult();
				goto IL_0209;
				IL_0067:
				try
				{
					if (num != 0)
					{
						goto IL_0151;
					}
					awaiter = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_00f4;
					IL_00f4:
					awaiter.GetResult();
					goto IL_00fc;
					IL_0151:
					if (_003C_003E7__wrap3.MoveNext())
					{
						_003CinstanceData_003E5__5 = _003C_003E7__wrap3.Current;
						if (_003CinstanceData_003E5__5.GlobalCancelTcs != null)
						{
							awaiter = _003CinstanceData_003E5__5.GlobalCancelTcs.Task.GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_00f4;
						}
						goto IL_00fc;
					}
					goto end_IL_0067;
					IL_00fc:
					if (_003CinstanceData_003E5__5._globalCts != null)
					{
						CancellationTokenSource? globalCts = _003CinstanceData_003E5__5._globalCts;
						if (globalCts != null)
						{
							_618921AB.CA3FD78A(globalCts);
						}
					}
					_003CinstanceData_003E5__5._globalCts = _003C_globalCts_003E5__2;
					_003CinstanceData_003E5__5.GlobalCancelTcs = _003CGlobalCancelTcs_003E5__3;
					_003CinstanceData_003E5__5 = null;
					goto IL_0151;
					end_IL_0067:;
				}
				finally
				{
					if (num < 0)
					{
						((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
					}
				}
				_003C_003E7__wrap3 = default(List<InstanceData>.Enumerator);
				goto end_IL_000e;
				IL_0209:
				if (scriptManager.InstanceData._globalCts != null)
				{
					CancellationTokenSource? globalCts2 = scriptManager.InstanceData._globalCts;
					if (globalCts2 != null)
					{
						_618921AB.CA3FD78A(globalCts2);
					}
				}
				scriptManager.InstanceData._globalCts = _003C_globalCts_003E5__2;
				scriptManager.InstanceData.GlobalCancelTcs = _003CGlobalCancelTcs_003E5__3;
				end_IL_000e:;
			}
			catch (Exception _341F081F)
			{
				_003C_003E1__state = -2;
				_003C_globalCts_003E5__2 = null;
				_003CGlobalCancelTcs_003E5__3 = null;
				_671BC22C.BF9F3D1F.FE8E0C9E[1494](ref _003C_003Et__builder, _341F081F);
				return;
			}
			_003C_003E1__state = -2;
			_003C_globalCts_003E5__2 = null;
			_003CGlobalCancelTcs_003E5__3 = null;
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
	private struct _003CScriptBank_003Ed__26 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public ScriptManager _003C_003E4__this;

		public Lua lua;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			ScriptManager scriptManager = _003C_003E4__this;
			bool result;
			try
			{
				if (num == 0)
				{
					goto IL_006f;
				}
				scriptManager.Log(new global::E5A6EF93<string>("warning.bank.goEmpty"), "gathering", D32842AE._543E5320._7C2AA78D);
				LuaFunction luaFunction = _671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "bank") as LuaFunction;
				if (luaFunction != null)
				{
					goto IL_006f;
				}
				scriptManager.Log(new global::E5A6EF93<string>("warning.bank.noFunction"), "gathering", D32842AE._543E5320._7C2AA78D);
				result = false;
				goto end_IL_000e;
				IL_006f:
				try
				{
					TaskAwaiter<bool> awaiter;
					if (num == 0)
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_014c;
					}
					object[] array = _671BC22C.BF9F3D1F.FE8E0C9E[1888](luaFunction, Array.Empty<object>());
					if (array != null && array.Length != 0)
					{
						if (array[0] is LuaTable _2484EA)
						{
							List<LuaTable> actions = _671BC22C.BF9F3D1F.FE8E0C9E[1291](_2484EA).OfType<LuaTable>().ToList();
							awaiter = scriptManager.HandleActions(lua, actions).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_014c;
						}
						scriptManager.Log(new global::E5A6EF93<string>("error.bank.unknownError"), "gathering", D32842AE._543E5320.B3B830AD);
						goto end_IL_006f;
					}
					scriptManager.Log(new global::E5A6EF93<string>("error.bank.noAction"), "gathering", D32842AE._543E5320.B3B830AD);
					result = false;
					goto end_IL_000e;
					IL_014c:
					if (awaiter.GetResult())
					{
						goto end_IL_006f;
					}
					scriptManager.Log(new global::E5A6EF93<string>("error.bank.unknownError"), "gathering", D32842AE._543E5320.B3B830AD);
					result = false;
					goto end_IL_000e;
					end_IL_006f:;
				}
				catch (Exception)
				{
					scriptManager.Log(new global::E5A6EF93<string>("error.bank.unknownError"), "gathering", D32842AE._543E5320.B3B830AD);
					result = false;
					goto end_IL_000e;
				}
				result = true;
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
	private struct _003CScriptMove_003Ed__27 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public Lua lua;

		public ScriptManager _003C_003E4__this;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			ScriptManager scriptManager = _003C_003E4__this;
			bool result;
			try
			{
				if (num == 0)
				{
					goto IL_0059;
				}
				LuaFunction luaFunction = _671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "move") as LuaFunction;
				if (luaFunction != null)
				{
					goto IL_0059;
				}
				scriptManager.Log(new global::E5A6EF93<string>("error.script.moveNotFound"), "gathering", D32842AE._543E5320.B3B830AD);
				result = false;
				goto end_IL_000e;
				IL_0059:
				try
				{
					TaskAwaiter<bool> awaiter;
					if (num == 0)
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_010e;
					}
					object[] array = _671BC22C.BF9F3D1F.FE8E0C9E[1888](luaFunction, Array.Empty<object>());
					if (array.Length != 0 && array[0] is LuaTable _2484EA)
					{
						awaiter = scriptManager.HandleActions(lua, _671BC22C.BF9F3D1F.FE8E0C9E[1291](_2484EA).OfType<LuaTable>().ToList()).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_010e;
					}
					scriptManager.Log(new global::E5A6EF93<string>("error.script.moveNoAction"), "gathering", D32842AE._543E5320.B3B830AD);
					result = false;
					goto end_IL_0059;
					IL_010e:
					result = awaiter.GetResult();
					end_IL_0059:;
				}
				catch (Exception fEBDE)
				{
					scriptManager.Log(new global::ED33D63D<string, global::_882F6A04<string>>("error.script.moveError", new global::_882F6A04<string>(_671BC22C.BF9F3D1F.FE8E0C9E[2117](fEBDE))), "gathering", D32842AE._543E5320.B3B830AD);
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

	private bool wasFighting;

	public bool isRunning;

	public bool craftingMode;

	private bool hasHandledLock;

	internal int LastActionIndex;

	private List<int> ElementIdErrorsOnGatherOnCurrentMap;

	private int _lastMapId;

	[CompilerGenerated]
	private InstanceData _003CInstanceData_003Ek__BackingField;

	internal InstanceData InstanceData
	{
		[CompilerGenerated]
		get
		{
			return _003CInstanceData_003Ek__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			sbyte b = -10;
			_003CInstanceData_003Ek__BackingField = value;
		}
	}

	public List<InstanceData> TeamInstanceDataWithLeader { get; private set; }

	public bool IsInTeam { get; private set; }

	internal ScriptManager(InstanceData instanceData)
	{
		ushort num = 34182;
		Unsafe.SkipInit(out short num3);
		Unsafe.SkipInit(out ushort num2);
		while (true)
		{
			switch ((uint)num % 6u)
			{
			default:
				do
				{
					LastActionIndex = ((short)num ^ -1449650132) - 1449631147;
					num3 = (short)((-1338399982 & -(num & -1665875189)) << 1);
				}
				while ((4 ^ num) > (int)(1439857516 - 3737035449u / (uint)(~num3)) * (num3 / -483737426 - 889272606));
				num = (ushort)((num % ~((uint)num3 / (uint)num) / 203126420 / (uint)(1571355044 + num)) ^ 0xC13);
				break;
			case 1u:
				num = (ushort)(num3 - -1876379854 + -1876363116);
				do
				{
					ElementIdErrorsOnGatherOnCurrentMap = new List<int>();
					num2 = (ushort)(num & 0x4158AF49);
				}
				while ((-1297293143 | num2) * -1014094973 >>> (int)num3 << (num3 >> 2122608021 / -num2) == num - 1586354714);
				num = (ushort)((0x351C91BA | num2) + ((((uint)(num2 & -978731076) < 3523266561u) ? 1 : 0) + -1414361470) - -523358967);
				break;
			case 2u:
				num = (ushort)(0xAE8B ^ (-23027 + num2));
				do
				{
					TeamInstanceDataWithLeader = new List<InstanceData>();
					num2 = (ushort)((int)(1561871930u % (uint)num) % -1188125416);
				}
				while (-1677365196 % num2 % (1274342874 - ~num) > (int)((uint)((int)num % (int)((uint)(~num2) / (uint)num)) / 3348914538u));
				num = (ushort)(0xDFD2 ^ ~(~(num >> 1)));
				break;
			case 3u:
				num = (ushort)(-(num - -637370971 * (int)((uint)num3 % (uint)num2)) - 1371036829);
				do
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[(((int)((3707872169u % (uint)num2) & (uint)(num2 % -157140559) & (uint)(-1744057550 ^ num)) < -1666423138 - (num + 1999653154 >> 19)) ? 1 : 0) + 2099](this);
					num2 = (ushort)(-1426837 + num << num + -517123565);
				}
				while ((uint)num2 >> (int)(0 - (uint)num / (uint)(1646122889 / ~num2)) != 0);
				num = (ushort)((num2 + num >> (824454960 * num2 + num >> 9)) - 591);
				break;
			case 4u:
				num = (ushort)((num2 & -1433232622) * (int)(4018443267u % (uint)num3) * (269265540 << (num & 0x57049F9C)) - -34182);
				do
				{
					InstanceData = instanceData;
				}
				while (~((num3 >> 24) / ~num2) >= (int)((uint)num % 2477354672u));
				num = (ushort)((num >>> ~((((uint)num3 < (uint)num2) ? 1 : 0) / (int)num3)) - -56609);
				break;
			case 5u:
				num = (ushort)(34182 + (num2 - 0 - (int)(4122838586u % (uint)num3 >> 31)) / -820554592);
				return;
			}
		}
	}

	private bool ExecuteForTeam(Func<InstanceData, Task<bool>> action)
	{
		_003C_003Ec__DisplayClass5_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass5_0();
		CS_0024_003C_003E8__locals3.action = action;
		if (!IsInTeam)
		{
			Task<bool> task = CS_0024_003C_003E8__locals3.action(InstanceData);
			_680DAE05._7422EE95(task);
			return task.Result;
		}
		IEnumerable<Task<bool>> source = TeamInstanceDataWithLeader.Select((InstanceData e) => CS_0024_003C_003E8__locals3.action(e));
		Task[] eD1889A = source.ToArray();
		_671BC22C.BF9F3D1F.FE8E0C9E[829](eD1889A);
		return source.All((Task<bool> r) => r.Result);
	}

	private bool ExecuteForTeam(Func<InstanceData, Task<GameMessage>> action)
	{
		_003C_003Ec__DisplayClass6_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass6_0();
		CS_0024_003C_003E8__locals3.action = action;
		try
		{
			if (!IsInTeam)
			{
				Task<GameMessage> task = CS_0024_003C_003E8__locals3.action(InstanceData);
				_680DAE05._7422EE95(task);
				return _9717D104._230D3B1D(task);
			}
			IEnumerable<Task<GameMessage>> source = TeamInstanceDataWithLeader.Select((InstanceData e) => CS_0024_003C_003E8__locals3.action(e));
			Task[] eD1889A = source.ToArray();
			_671BC22C.BF9F3D1F.FE8E0C9E[829](eD1889A);
			return source.All((Task<GameMessage> r) => _671BC22C.BF9F3D1F.FE8E0C9E[424](r));
		}
		catch (Exception)
		{
			return false;
		}
	}

	private async Task<bool> HandleAction(string path, string cell, string door, bool npcBank, bool crafting, int craftItemGid, int craftBankMapId, int craftWorkshopMapId, bool craftSpecialWorkshop, bool craftReturnBank, string lockedHouse, string lockedStorage, bool chestGuild, int chestGuildNumber, int thresholdKamas, int kamasToTake)
	{
		_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals63 = new _003C_003Ec__DisplayClass7_0();
		CS_0024_003C_003E8__locals63._003C_003E4__this = this;
		CS_0024_003C_003E8__locals63.path = path;
		CS_0024_003C_003E8__locals63.cell = cell;
		CS_0024_003C_003E8__locals63.crafting = crafting;
		CS_0024_003C_003E8__locals63.craftItemGid = craftItemGid;
		CS_0024_003C_003E8__locals63.craftBankMapId = craftBankMapId;
		CS_0024_003C_003E8__locals63.craftWorkshopMapId = craftWorkshopMapId;
		CS_0024_003C_003E8__locals63.craftSpecialWorkshop = craftSpecialWorkshop;
		CS_0024_003C_003E8__locals63.craftReturnBank = craftReturnBank;
		CS_0024_003C_003E8__locals63.thresholdKamas = thresholdKamas;
		CS_0024_003C_003E8__locals63.kamasToTake = kamasToTake;
		CS_0024_003C_003E8__locals63.lockedHouse = lockedHouse;
		await WaitForTeamToBeInSameMap();
		bool result = true;
		string currentPosition = GetPosition();
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[744](CS_0024_003C_003E8__locals63.path))
		{
			if (_671BC22C.BF9F3D1F.FE8E0C9E[1130](CS_0024_003C_003E8__locals63.path, ","))
			{
				_003C_003Ec__DisplayClass7_1 CS_0024_003C_003E8__locals58 = new _003C_003Ec__DisplayClass7_1();
				string[] array = _671BC22C.BF9F3D1F.FE8E0C9E[1565](CS_0024_003C_003E8__locals63.path, ',', StringSplitOptions.None);
				if (array.Length != 2 || !_671BC22C.BF9F3D1F.FE8E0C9E[422](array[0], ref CS_0024_003C_003E8__locals58.x) || !_671BC22C.BF9F3D1F.FE8E0C9E[422](array[1], ref CS_0024_003C_003E8__locals58.y))
				{
					Log(new global::ED33D63D<string, global::_6F1FA32D<string>>("error.script.badFormat", new global::_6F1FA32D<string>(CS_0024_003C_003E8__locals63.path)), "gathering", D32842AE._543E5320.B3B830AD);
					return false;
				}
				if (IsInTeam)
				{
					bool[] source = await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData member) => TravelToAsync(member, CS_0024_003C_003E8__locals58.x, CS_0024_003C_003E8__locals58.y)).ToArray());
					if (source.All((bool r) => r))
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Green);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[SUCCÈS] Tous les membres sont arrivés.");
					}
					else
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[ERREUR] Au moins un membre n'a pas pu se déplacer.");
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[759]();
					result = source.All((bool r) => r);
				}
				else
				{
					result = await TravelToAsync(InstanceData, CS_0024_003C_003E8__locals58.x, CS_0024_003C_003E8__locals58.y);
				}
			}
			else
			{
				_003C_003Ec__DisplayClass7_2 CS_0024_003C_003E8__locals59 = new _003C_003Ec__DisplayClass7_2();
				if (_671BC22C.BF9F3D1F.FE8E0C9E[422](CS_0024_003C_003E8__locals63.path, ref CS_0024_003C_003E8__locals59.mapId))
				{
					if (IsInTeam)
					{
						bool[] source2 = await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData member) => TravelToAsync(member, CS_0024_003C_003E8__locals59.mapId)).ToArray());
						if (source2.All((bool r) => r))
						{
							_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Green);
							_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[SUCCÈS] Tous les membres sont arrivés.");
						}
						else
						{
							_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
							_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[ERREUR] Au moins un membre n'a pas pu se déplacer.");
						}
						_671BC22C.BF9F3D1F.FE8E0C9E[759]();
						result = source2.All((bool r) => r);
					}
					else
					{
						result = await TravelToAsync(InstanceData, CS_0024_003C_003E8__locals59.mapId);
					}
				}
				else if (_671BC22C.BF9F3D1F.FE8E0C9E[2005](CS_0024_003C_003E8__locals63.path, "zaap(") && _671BC22C.BF9F3D1F.FE8E0C9E[120](CS_0024_003C_003E8__locals63.path, ")"))
				{
					_003C_003Ec__DisplayClass7_3 CS_0024_003C_003E8__locals60 = new _003C_003Ec__DisplayClass7_3();
					if (_671BC22C.BF9F3D1F.FE8E0C9E[422](_671BC22C.BF9F3D1F.FE8E0C9E[2035](CS_0024_003C_003E8__locals63.path, _671BC22C.BF9F3D1F.FE8E0C9E[70]("zaap("), _671BC22C.BF9F3D1F.FE8E0C9E[69](CS_0024_003C_003E8__locals63.path) - _671BC22C.BF9F3D1F.FE8E0C9E[70]("zaap(") - 1), ref CS_0024_003C_003E8__locals60.zaapId))
					{
						if (IsInTeam)
						{
							bool[] source3 = await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData member) => TravelToAsync(member, CS_0024_003C_003E8__locals60.zaapId)).ToArray());
							if (source3.All((bool r) => r))
							{
								_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Green);
								_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[SUCCÈS] Tous les membres sont arrivés.");
							}
							else
							{
								_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
								_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[ERREUR] Au moins un membre n'a pas pu se déplacer.");
							}
							_671BC22C.BF9F3D1F.FE8E0C9E[759]();
							result = source3.All((bool r) => r);
						}
					}
					else
					{
						result = await TravelToAsync(InstanceData, CS_0024_003C_003E8__locals60.zaapId);
					}
				}
				else
				{
					result = ((!IsInTeam) ? (await InstanceData.MoveToDirection(PathToDirection(_671BC22C.BF9F3D1F.FE8E0C9E[235](CS_0024_003C_003E8__locals63.path)))) : (await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData member) => member.MoveToDirection(CS_0024_003C_003E8__locals63._003C_003E4__this.PathToDirection(_671BC22C.BF9F3D1F.FE8E0C9E[235](CS_0024_003C_003E8__locals63.path)))))).All((bool r) => r));
					if (!result)
					{
						Log(new global::ED33D63D<string, global::_6211A713<string>>("error.script.move", new global::_6211A713<string>(currentPosition)), "gathering", D32842AE._543E5320.B3B830AD);
						return false;
					}
				}
			}
			return result;
		}
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[744](CS_0024_003C_003E8__locals63.cell))
		{
			result = ((!IsInTeam) ? (await InstanceData.MovePlayerAndChangeMap(_671BC22C.BF9F3D1F.FE8E0C9E[1176](CS_0024_003C_003E8__locals63.cell), -1)) : (await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData member) => member.MovePlayerAndChangeMap(_671BC22C.BF9F3D1F.FE8E0C9E[1176](CS_0024_003C_003E8__locals63.cell), -1)))).All((bool r) => r));
			if (!result)
			{
				Log(new global::ED33D63D<string, global::CD9D60A2<string>>("error.script.moveCell", new global::CD9D60A2<string>(CS_0024_003C_003E8__locals63.cell)), "gathering", D32842AE._543E5320.B3B830AD);
				return false;
			}
			Log(new global::ED33D63D<string, global::B895658D<string, string>>("warning.script.moveCell", new global::B895658D<string, string>(CS_0024_003C_003E8__locals63.cell, currentPosition)), "gathering", D32842AE._543E5320._7C2AA78D);
		}
		else if (chestGuild)
		{
			if (IsInTeam)
			{
				foreach (InstanceData item in TeamInstanceDataWithLeader)
				{
					bool flag = result;
					if (flag)
					{
						flag = await ChestGuildForInstanceData(item, chestGuildNumber);
					}
					result = flag;
				}
			}
			else
			{
				result = await ChestGuildForInstanceData(InstanceData, chestGuildNumber);
			}
			InstanceData.NotificationDiscord.DC998F04();
		}
		else if (npcBank && !craftingMode)
		{
			if (!IsInTeam)
			{
				await NpcBankForInstanceData(InstanceData, CS_0024_003C_003E8__locals63.crafting, CS_0024_003C_003E8__locals63.craftItemGid, CS_0024_003C_003E8__locals63.craftBankMapId, CS_0024_003C_003E8__locals63.craftWorkshopMapId, CS_0024_003C_003E8__locals63.craftSpecialWorkshop, CS_0024_003C_003E8__locals63.craftReturnBank, CS_0024_003C_003E8__locals63.thresholdKamas, CS_0024_003C_003E8__locals63.kamasToTake);
			}
			else
			{
				using List<InstanceData>.Enumerator enumerator = TeamInstanceDataWithLeader.GetEnumerator();
				while (enumerator.MoveNext())
				{
					_003C_003Ec__DisplayClass7_4 CS_0024_003C_003E8__locals73 = new _003C_003Ec__DisplayClass7_4();
					CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1 = CS_0024_003C_003E8__locals63;
					CS_0024_003C_003E8__locals73.instanceData = enumerator.Current;
					result = (await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData member) => CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1._003C_003E4__this.NpcBankForInstanceData(CS_0024_003C_003E8__locals73.instanceData, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.crafting, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.craftItemGid, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.craftBankMapId, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.craftWorkshopMapId, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.craftSpecialWorkshop, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.craftReturnBank, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.thresholdKamas, CS_0024_003C_003E8__locals73.CS_0024_003C_003E8__locals1.kamasToTake)))).All((bool r) => r);
				}
			}
			InstanceData.NotificationDiscord.DC998F04();
		}
		else if (_671BC22C.BF9F3D1F.FE8E0C9E[744](door))
		{
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[744](CS_0024_003C_003E8__locals63.lockedHouse))
			{
				if (!IsInTeam)
				{
					await LockedHouseForInstanceData(InstanceData, CS_0024_003C_003E8__locals63.lockedHouse);
				}
				else
				{
					using List<InstanceData>.Enumerator enumerator = TeamInstanceDataWithLeader.GetEnumerator();
					while (enumerator.MoveNext())
					{
						_003C_003Ec__DisplayClass7_5 CS_0024_003C_003E8__locals76 = new _003C_003Ec__DisplayClass7_5();
						CS_0024_003C_003E8__locals76.CS_0024_003C_003E8__locals2 = CS_0024_003C_003E8__locals63;
						CS_0024_003C_003E8__locals76.instanceData = enumerator.Current;
						result = (await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData member) => CS_0024_003C_003E8__locals76.CS_0024_003C_003E8__locals2._003C_003E4__this.LockedHouseForInstanceData(CS_0024_003C_003E8__locals76.instanceData, CS_0024_003C_003E8__locals76.CS_0024_003C_003E8__locals2.lockedHouse)))).All((bool r) => r);
					}
				}
			}
			else
			{
				if (_671BC22C.BF9F3D1F.FE8E0C9E[744](lockedStorage))
				{
					Log(new global::ED33D63D<string, global::_880A4502<string>>("error.script.undefinedAction", new global::_880A4502<string>(currentPosition)), "gathering", D32842AE._543E5320.B3B830AD);
					return true;
				}
				if (!IsInTeam)
				{
					await LockedStorageForInstanceData(InstanceData, lockedStorage);
				}
				else
				{
					foreach (InstanceData item2 in TeamInstanceDataWithLeader)
					{
						bool flag = result;
						if (flag)
						{
							flag = await LockedStorageForInstanceData(item2, lockedStorage);
						}
						result = flag;
					}
				}
				InstanceData.NotificationDiscord.DC998F04();
			}
		}
		string position = GetPosition();
		if (_671BC22C.BF9F3D1F.FE8E0C9E[1810](position, currentPosition))
		{
			Log(new global::ED33D63D<string, global::F7149E3F<string>>("success.script.newPosition", new global::F7149E3F<string>(position)), "gathering", D32842AE._543E5320._5D225697);
		}
		else
		{
			Log(new global::ED33D63D<string, global::B895658D<string, string>>("warning.script.noChange", new global::B895658D<string, string>(CS_0024_003C_003E8__locals63.cell, currentPosition)), "gathering", D32842AE._543E5320._7C2AA78D);
		}
		return result;
	}

	private async Task<bool> MoveAndOpenChest(int mapId, int cellId, string code)
	{
		if (!(await InstanceData.MovePlayerToWorldMapId(mapId)))
		{
			Log(new global::ED33D63D<string, global::EC94F596<int>>("house.moveToHouseFail", new global::EC94F596<int>(mapId)), "gathering", D32842AE._543E5320.B3B830AD);
			StopScript();
			return false;
		}
		Log(new global::E5A6EF93<string>("house.moveToHouseSuccess"), "gathering", D32842AE._543E5320._5D225697);
		if (!(await InstanceData.HouseManager._369E1935(cellId, E2BFF606: true)))
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[376]();
			Log(new global::E5A6EF93<string>("error.storage.interactChestFail"), "gathering", D32842AE._543E5320.B3B830AD);
			StopScript();
			return false;
		}
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[744](code) && _671BC22C.BF9F3D1F.FE8E0C9E[1810](code, "-1"))
		{
			Log(new global::E5A6EF93<string>("info.house.unlockAttempt"), "gathering", D32842AE._543E5320._703EA0A0);
			if (!(await InstanceData.HouseManager.F1104A3E(code)))
			{
				_671BC22C.BF9F3D1F.FE8E0C9E[376]();
				Log(new global::ED33D63D<string, global::_5A827491<string>>("error.storage.unlockFail", new global::_5A827491<string>(code)), "gathering", D32842AE._543E5320.B3B830AD);
				StopScript();
				return false;
			}
		}
		else
		{
			Log(new global::E5A6EF93<string>("info.storage.enterWithoutCode"), "gathering", D32842AE._543E5320._703EA0A0);
		}
		TaskAwaiter taskAwaiter = default(TaskAwaiter);
		while (InstanceData.TempStorage == null || InstanceData.TempStorage.Count == 0)
		{
			TaskAwaiter _6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](50));
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
			{
				await _6D28EB9F;
				_6D28EB9F = taskAwaiter;
				taskAwaiter = default(TaskAwaiter);
			}
			_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
		}
		return true;
	}

	private async Task<bool> LockedStorageForInstanceData(InstanceData instanceData, string lockedStorage)
	{
		string[] array = _671BC22C.BF9F3D1F.FE8E0C9E[1565](lockedStorage, '|', StringSplitOptions.None);
		int _599D = default(int);
		if (array.Length == 2 && _671BC22C.BF9F3D1F.FE8E0C9E[422](array[0], ref _599D))
		{
			string code = array[1];
			if (!(await instanceData.HouseManager._369E1935(_599D, E2BFF606: true)))
			{
				Log(new global::ED33D63D<string, _8515A539>("error.storage.interactChestFail", new _8515A539()), "gathering", D32842AE._543E5320.B3B830AD);
				return false;
			}
			bool flag;
			if (_671BC22C.BF9F3D1F.FE8E0C9E[720](code, "-1"))
			{
				Log(new global::E5A6EF93<string>("info.storage.enterWithoutCode"), "gathering", D32842AE._543E5320._703EA0A0);
				flag = true;
			}
			else
			{
				Log(new global::E5A6EF93<string>("info.house.unlockAttempt"), "gathering", D32842AE._543E5320._703EA0A0);
				flag = await instanceData.HouseManager.F1104A3E(code);
			}
			if (!flag)
			{
				Log(new global::ED33D63D<string, global::_5A827491<string>>("error.storage.unlockFail", new global::_5A827491<string>(code)), "gathering", D32842AE._543E5320.B3B830AD);
				return false;
			}
			Log(new global::E5A6EF93<string>("info.house.transfer"), "gathering", D32842AE._543E5320._703EA0A0);
			if (!(await instanceData.NpcManager._37BBC281()))
			{
				_671BC22C.BF9F3D1F.FE8E0C9E[376]();
				Log(new global::E5A6EF93<string>("error.storage.transferFail"), "gathering", D32842AE._543E5320.B3B830AD);
				return false;
			}
			Log(new global::E5A6EF93<string>("success.storage.transfer"), "gathering", D32842AE._543E5320._5D225697);
			Log(new global::E5A6EF93<string>("info.storage.closeChest"), "gathering", D32842AE._543E5320._703EA0A0);
			flag = await instanceData.NpcManager.CC17DB97();
			if (!flag)
			{
				Log(new global::E5A6EF93<string>("error.storage.leaveDialog"), "gathering", D32842AE._543E5320.B3B830AD);
				return false;
			}
			Log(new global::E5A6EF93<string>("success.storage.leaveDialog"), "gathering", D32842AE._543E5320._5D225697);
			return flag;
		}
		Log(new global::ED33D63D<string, global::_39108025<string>>("error.storage.badFormat", new global::_39108025<string>(lockedStorage)), "gathering", D32842AE._543E5320.B3B830AD);
		return false;
	}

	internal async Task<bool> GoToAndEnterHouse(int houseMapId, string nickName, string code)
	{
		MapIdCoordinates coordinates = _82210236.AB91771D(houseMapId, B31AE737: true);
		if (!(await InstanceData.GoToNearestZaap(coordinates)))
		{
			Log(new global::ED33D63D<string, global::EC94F596<int>>("house.goToHouseCoordinateFail", new global::EC94F596<int>(houseMapId)), "gathering", D32842AE._543E5320.B3B830AD);
			StopScript();
			return false;
		}
		Log(new global::E5A6EF93<string>("house.goToHouseCoordinateSuccess"), "gathering", D32842AE._543E5320._5D225697);
		if (!(await InstanceData.MovePlayerToWorldMapId(coordinates.MapId)))
		{
			Log(new global::ED33D63D<string, global::EC94F596<int>>("house.moveToHouseFail", new global::EC94F596<int>(coordinates.MapId)), "gathering", D32842AE._543E5320.B3B830AD);
			StopScript();
			return false;
		}
		Log(new global::E5A6EF93<string>("house.moveToHouseSuccess"), "gathering", D32842AE._543E5320._5D225697);
		(int? DoorElementId, int? InstanceId, int? CellId, int? SkillInstanceUid, JitsuriInteractiveElement? Element) houseInfo = InstanceData.MapInformation.GetHouseDoorElementIdAndInstanceIdByNickname(nickName);
		if (!houseInfo.DoorElementId.HasValue || !houseInfo.InstanceId.HasValue || !houseInfo.CellId.HasValue || !houseInfo.SkillInstanceUid.HasValue)
		{
			Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.notFound", new global::C795A725<string>(nickName)), "gathering", D32842AE._543E5320.B3B830AD);
			StopScript();
			return false;
		}
		Log(new global::E5A6EF93<string>("info.house.goToDoor"), "gathering", D32842AE._543E5320._703EA0A0);
		if (!(await InstanceData.MovePlayerAndChangeMap(houseInfo.CellId.Value, -1)))
		{
			Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.moveToDoorFail", new global::C795A725<string>(nickName)), "gathering", D32842AE._543E5320.B3B830AD);
			StopScript();
			return false;
		}
		if (!(await InstanceData.HouseManager.BD1B7212(houseInfo.DoorElementId.Value, houseInfo.InstanceId.Value, houseInfo.SkillInstanceUid.Value)))
		{
			Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.interactDoorFail", new global::C795A725<string>(nickName)), "gathering", D32842AE._543E5320.B3B830AD);
			StopScript();
			return false;
		}
		if (_671BC22C.BF9F3D1F.FE8E0C9E[744](code) || _671BC22C.BF9F3D1F.FE8E0C9E[720](code, "-1"))
		{
			Log(new global::ED33D63D<string, global::C795A725<string>>("info.house.enterWithoutCode", new global::C795A725<string>(nickName)), "gathering", D32842AE._543E5320._703EA0A0);
			if (!(await InstanceData.HouseManager._4D3DBD0C(houseInfo.DoorElementId.Value, houseInfo.InstanceId.Value, houseInfo.SkillInstanceUid.Value)))
			{
				Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.enterFail", new global::C795A725<string>(nickName)), "gathering", D32842AE._543E5320.B3B830AD);
				StopScript();
				return false;
			}
			Log(new global::ED33D63D<string, global::C795A725<string>>("success.house.entered", new global::C795A725<string>(nickName)), "gathering", D32842AE._543E5320._5D225697);
		}
		else
		{
			Log(new global::E5A6EF93<string>("info.house.unlockAttempt"), "gathering", D32842AE._543E5320._703EA0A0);
			if (!(await InstanceData.HouseManager._14A3599F(code)))
			{
				Log(new global::ED33D63D<string, global::_02A98019<string, string>>("error.house.unlockFail", new global::_02A98019<string, string>(nickName, code)), "gathering", D32842AE._543E5320.B3B830AD);
				StopScript();
				return false;
			}
			Log(new global::ED33D63D<string, global::C795A725<string>>("success.house.unlocked", new global::C795A725<string>(nickName)), "gathering", D32842AE._543E5320._5D225697);
		}
		return true;
	}

	private async Task<bool> LockedHouseForInstanceData(InstanceData instanceData, string lockedHouse)
	{
		string[] array = _671BC22C.BF9F3D1F.FE8E0C9E[1565](lockedHouse, '|', StringSplitOptions.None);
		if (array.Length == 2)
		{
			string nickname = array[0];
			string code = array[1];
			(int? DoorElementId, int? InstanceId, int? CellId, int? SkillInstanceUid, JitsuriInteractiveElement? Element) houseInfo = instanceData.MapInformation.GetHouseDoorElementIdAndInstanceIdByNickname(nickname);
			if (houseInfo.DoorElementId.HasValue && houseInfo.InstanceId.HasValue && houseInfo.CellId.HasValue && houseInfo.SkillInstanceUid.HasValue)
			{
				int value = houseInfo.CellId.Value;
				Log(new global::E5A6EF93<string>("info.house.goToDoor"), "gathering", D32842AE._543E5320._703EA0A0);
				if (!(await instanceData.MovePlayerAndChangeMap(value, -1)))
				{
					Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.moveToDoorFail", new global::C795A725<string>(nickname)), "gathering", D32842AE._543E5320.B3B830AD);
					return false;
				}
				if (!(await instanceData.HouseManager.BD1B7212(houseInfo.DoorElementId.Value, houseInfo.InstanceId.Value, houseInfo.SkillInstanceUid.Value)))
				{
					Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.interactDoorFail", new global::C795A725<string>(nickname)), "gathering", D32842AE._543E5320.B3B830AD);
					return false;
				}
				if (_671BC22C.BF9F3D1F.FE8E0C9E[720](code, "-1"))
				{
					Log(new global::ED33D63D<string, global::C795A725<string>>("info.house.enterWithoutCode", new global::C795A725<string>(nickname)), "gathering", D32842AE._543E5320._703EA0A0);
					if (!(await instanceData.HouseManager._4D3DBD0C(houseInfo.DoorElementId.Value, houseInfo.InstanceId.Value, houseInfo.SkillInstanceUid.Value)))
					{
						Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.enterFail", new global::C795A725<string>(nickname)), "gathering", D32842AE._543E5320.B3B830AD);
						return false;
					}
					Log(new global::ED33D63D<string, global::C795A725<string>>("success.house.entered", new global::C795A725<string>(nickname)), "gathering", D32842AE._543E5320._5D225697);
					return true;
				}
				Log(new global::E5A6EF93<string>("info.house.unlockAttempt"), "gathering", D32842AE._543E5320._703EA0A0);
				bool flag = await instanceData.HouseManager._14A3599F(code);
				if (!flag)
				{
					Log(new global::ED33D63D<string, global::_63B58FAD<string, string>>("error.house.unlockFail", new global::_63B58FAD<string, string>(nickname, code)), "gathering", D32842AE._543E5320.B3B830AD);
					return false;
				}
				Log(new global::ED33D63D<string, global::C795A725<string>>("success.house.unlocked", new global::C795A725<string>(nickname)), "gathering", D32842AE._543E5320._5D225697);
				return flag;
			}
			Log(new global::ED33D63D<string, global::C795A725<string>>("error.house.notFound", new global::C795A725<string>(nickname)), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Log(new global::ED33D63D<string, global::D1914E07<string>>("error.house.badFormat", new global::D1914E07<string>(lockedHouse)), "gathering", D32842AE._543E5320.B3B830AD);
		return false;
	}

	private async Task<bool> NpcBankForInstanceData(InstanceData instanceData, bool crafting, int craftItemGid, int craftBankMapId, int craftWorkshopMapId, bool craftSpecialWorkshop, bool craftReturnBank, int thresholdKamas, int kamasToTake)
	{
		if (!(await instanceData.NpcManager._1AAC1704(InstanceData.MapInformation.MapId)))
		{
			Log(new global::E5A6EF93<string>("error.bank.dialogOpenFail"), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Log(new global::E5A6EF93<string>("info.bank.dialogOpen"), "gathering", D32842AE._543E5320._703EA0A0);
		if (!(await instanceData.NpcManager._0399D809(0)))
		{
			Log(new global::E5A6EF93<string>("error.bank.optionSelectFail"), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Log(new global::E5A6EF93<string>("info.bank.dialogOpenSuccess"), "gathering", D32842AE._543E5320._703EA0A0);
		if (thresholdKamas != 0 && kamasToTake != 0 && instanceData.CharacterData.CharacterKamas <= thresholdKamas)
		{
			if (instanceData.StorageKamas >= kamasToTake)
			{
				Log(new global::_1F10139C<string, global::F130DF31<int>>("info.bank.withdraw.start", new global::F130DF31<int>(kamasToTake)), "gathering", D32842AE._543E5320._703EA0A0);
				if (!(await instanceData.MessageHandler.FC903D17(kamasToTake)))
				{
					Log(new global::ED33D63D<string, global::F130DF31<int>>("error.bank.withdrawFail", new global::F130DF31<int>(kamasToTake)), "gathering", D32842AE._543E5320.B3B830AD);
				}
				else
				{
					Log(new global::ED33D63D<string, global::F130DF31<int>>("success.bank.withdrawOk", new global::F130DF31<int>(kamasToTake)), "gathering", D32842AE._543E5320._5D225697);
				}
			}
			else
			{
				Log(new global::ED33D63D<string, global::_6512DEBD<int, long>>("warning.bank.notEnoughKamasInStorage", new global::_6512DEBD<int, long>(kamasToTake, instanceData.StorageKamas)), "gathering", D32842AE._543E5320._7C2AA78D);
			}
		}
		if (!(await instanceData.NpcManager._37BBC281()))
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[376]();
			Log(new global::E5A6EF93<string>("error.bank.transferFail"), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Log(new global::E5A6EF93<string>("success.bank.transfer"), "gathering", D32842AE._543E5320.B3B830AD);
		bool flag = await instanceData.NpcManager.CC17DB97();
		if (!flag)
		{
			Log(new global::E5A6EF93<string>("error.bank.dialogCloseFail"), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Log(new global::E5A6EF93<string>("success.bank.dropSuccess"), "gathering", D32842AE._543E5320._5D225697);
		if (crafting)
		{
			await instanceData.NpcManager.CC17DB97();
			Log(new global::E5A6EF93<string>("craft.info.start"), "gathering", D32842AE._543E5320.B3B830AD);
			flag = await instanceData.CraftManager._97103C08(craftItemGid, craftBankMapId, craftWorkshopMapId, craftSpecialWorkshop, craftReturnBank);
			if (!flag)
			{
				Log(new global::E5A6EF93<string>("craft.info.fail"), "gathering", D32842AE._543E5320.B3B830AD);
				return false;
			}
		}
		return flag;
	}

	private async Task<bool> ChestGuildForInstanceData(InstanceData instanceData, int chestGuildNumber)
	{
		if (!(await instanceData.NpcManager._3CA8B323(chestGuildNumber)))
		{
			Log(new global::E5A6EF93<string>("error.bank.dialogChestOpenFail"), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Log(new global::E5A6EF93<string>("info.bank.dialogChestOpen"), "gathering", D32842AE._543E5320._703EA0A0);
		List<JitsuriObjectItem> ressources = instanceData.Inventory.GetRessources();
		foreach (JitsuriObjectItem item in ressources)
		{
			int gid = _671BC22C.BF9F3D1F.FE8E0C9E[1819](_671BC22C.BF9F3D1F.FE8E0C9E[1827](item));
			JitsuriObjectItem objectByGid = instanceData.Inventory.GetObjectByGid(gid);
			if (objectByGid != null && _671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid) > 0 && !instanceData.Inventory.IsFavorite(_671BC22C.BF9F3D1F.FE8E0C9E[303](item)))
			{
				int _77B = _671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid);
				await instanceData.MessageHandler._08026097(_671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid), _77B, (int)_671BC22C.BF9F3D1F.FE8E0C9E[1827](objectByGid));
			}
		}
		bool flag = await instanceData.NpcManager.CC17DB97();
		if (!flag)
		{
			Log(new global::E5A6EF93<string>("error.bank.dialogCloseFail"), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Log(new global::E5A6EF93<string>("success.bank.dropSuccess"), "gathering", D32842AE._543E5320._5D225697);
		return flag;
	}

	[AsyncStateMachine(typeof(_003CHandleActions_003Ed__14))]
	private Task<bool> HandleActions(Lua lua, List<LuaTable> actions, bool isCustom = false)
	{
		sbyte b = 63;
		if ((uint)(b << (int)b) > (uint)(-(b & 0x18B1F5AB) << ((b == 1) ? 1 : 0) >>> (((byte)b % 1160196140) ^ ~(-1717431164 - b))))
		{
			b = (sbyte)((((b == b) ? 1u : 0u) << (-1422152685 << (int)(2650929071u / (uint)b))) - 2132645142 + 2132645147);
			goto IL_0059;
		}
		goto IL_00f1;
		IL_0156:
		_003CHandleActions_003Ed__14 stateMachine = default(_003CHandleActions_003Ed__14);
		byte b2 = default(byte);
		do
		{
			stateMachine._003C_003E1__state = (b - -b) ^ -127;
			b2 = (byte)(4294931200u >> (0 << (int)b));
		}
		while ((int)((uint)(1118283937 / b % -89006795 >>> (int)b) / (uint)b) > (int)((uint)(-13011166 % ~b2 / -2137184616) / ~((472051478 == b2) ? 1u : 0u)));
		b = (sbyte)((-1533435002 | b) - -1533434973);
		goto IL_0059;
		IL_0059:
		while (true)
		{
			switch ((uint)b % 6u)
			{
			case 1u:
				goto end_IL_0059;
			case 2u:
				goto IL_011a;
			case 3u:
				goto IL_014e;
			case 4u:
				b = (sbyte)((b << (int)b >> (int)b2) - -1073741887);
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				b = (sbyte)(-((-1516394354 & b) ^ 0x451D5F2F ^ ((b2 + 1102621188) | -2070288599)));
				b = (sbyte)((1024639541 - b << (int)b >>> 14) + -36875);
				continue;
			case 5u:
				b = (sbyte)((b2 | ((int)((uint)(b + 1938294545) / 395198233u) * (int)b)) + -224);
				return stateMachine._003C_003Et__builder.Task;
			}
			b = (sbyte)((b ^ 0x437CABDA) + -1132243869);
			stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
			if ((uint)(b & ((-2004182267 * b) ^ 0x34A66BC5)) / (uint)(-b) != (uint)(-710784581 + (int)((uint)(-283345023 >> (int)b) % (uint)(1320360327 % b)) >>> (b >>> 0)))
			{
				b = (sbyte)(-(-1052860609 << (int)b) - 2147483641);
				continue;
			}
			goto IL_0156;
			IL_011a:
			b = (sbyte)(((b + (b << (int)b) >>> 30) % -299528406) ^ 0x3E);
			stateMachine.lua = lua;
			if (b % b - b < (b ^ 0x1D3BC9A6))
			{
				continue;
			}
			goto IL_0156;
			continue;
			end_IL_0059:
			break;
		}
		b = (sbyte)((b ^ 0x4FD9475C) + -1339639580);
		goto IL_00f1;
		IL_014e:
		stateMachine.actions = actions;
		goto IL_0156;
		IL_00f1:
		stateMachine._003C_003E4__this = this;
		if (((470613030 * b) | 0) + -1971091959 != 0)
		{
			b = (sbyte)((b | 0) - 37);
			goto IL_0059;
		}
		goto IL_014e;
	}

	[AsyncStateMachine(typeof(_003CHandleGather_003Ed__18))]
	internal Task<bool> HandleGather(Lua lua, LuaTable action)
	{
		_003CHandleGather_003Ed__18 stateMachine = default(_003CHandleGather_003Ed__18);
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
		short num2 = default(short);
		while (true)
		{
			stateMachine._003C_003E4__this = this;
			uint num = 0u;
			while (true)
			{
				switch (num % 4)
				{
				case 1u:
					num = (((int)num < -1677457698) ? 1u : 0u) >> 14;
					stateMachine.action = action;
					do
					{
						stateMachine._003C_003E1__state = (((num / 3459532179u == 82047483) ? 1 : 0) - (int)(0xB293A006u ^ (1478213649 - num))) ^ -360161258;
						num2 = (sbyte)num;
					}
					while (757624491 + num == (((uint)num2 ^ (388324867 * ((uint)num2 / (uint)(~num2)))) & 0x81D7C8E));
					num = 61992970 + (0u / (uint)(num2 - 1125347761) + (ushort)(num2 >>> 22)) % 542121;
					continue;
				case 2u:
					num = (uint)((-948739108 >> (int)num2) + 948739108);
					stateMachine._003C_003Et__builder.Start(ref stateMachine);
					if ((num2 >> 13) + 1620304819 != 0)
					{
						num = (uint)(-(-(num2 % -1970566263)) ^ 0x76BE148F);
						continue;
					}
					break;
				case 3u:
					num = (((((uint)((int)num * (int)num2) > (uint)num2) ? 1u : 0u) == num) ? 1u : 0u);
					return stateMachine._003C_003Et__builder.Task;
				}
				stateMachine.lua = lua;
				if (35406 + num == ((num > 3635556927u) ? 1u : 0u))
				{
					break;
				}
				num = 3299577790u + ~(num ^ (uint)((int)num >> (int)num));
			}
		}
	}

	[AsyncStateMachine(typeof(_003CHandleFight_003Ed__19))]
	internal Task<bool> HandleFight(Lua lua, LuaTable action)
	{
		byte b = 98;
		_003CHandleFight_003Ed__19 stateMachine = default(_003CHandleFight_003Ed__19);
		while (true)
		{
			switch ((uint)b % 7u)
			{
			default:
				do
				{
					stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
				}
				while ((uint)(((int)(short)b * (int)((uint)(-b) % (uint)b)) ^ 0x12AA9199) > (uint)((-155516789 ^ b) - (sbyte)b));
				b = (byte)(0xCB10CA ^ (1688362137 % (int)(1931238818u / (uint)b)));
				continue;
			case 1u:
				b = (byte)((0xFB251 | b) + -1028591);
				stateMachine._003C_003E4__this = this;
				if ((uint)(b & 0x715FBEF0) > (uint)((int)((uint)(b * b) % (uint)b) % -491115749))
				{
					b = (byte)(-(1496586898 + b >>> (-1238216010 << (b & b))) - -273);
					continue;
				}
				break;
			case 2u:
				b = (byte)((428073118 * b % -785534915 >>> 10) % 166626 + -154816);
				stateMachine.lua = lua;
				b = (byte)((int)(984246809u % (uint)b) / -1371079065);
				if ((b ^ (-89309306 << -(b << 7))) != (int)((uint)(sbyte)b / (uint)(~b)) >> (int)(short)b)
				{
					b = (byte)(413833261 / (1092040092u % (uint)(~b)) / (421309065u % (uint)(~b)) - 4294967132u);
					continue;
				}
				break;
			case 3u:
				b = (byte)((uint)b / (uint)(-709759574 | (b - b)) - 0);
				goto IL_0143;
			case 4u:
				b = (byte)((uint)b % (uint)(0x503024AD | (b >>> (int)((uint)b / (uint)b))) - 39);
				stateMachine._003C_003E1__state = (int)((uint)(b + b) / ~((uint)b % 2306872106u) % 4281946771u - 1);
				b = (byte)((b - b) % 641773309 - b + ((791473982 << (int)b) - 1090882871) % (-98645997 >>> (int)b));
				b = (byte)((-519676263 * (((uint)(52713497 / (int)(~((b > b) ? 1u : 0u))) < (uint)(-b)) ? 1 : 0)) ^ -519676371);
				continue;
			case 5u:
				b = (byte)((0 * (-1979226459 | b) + b) ^ 0x7A);
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				b = (byte)((uint)(10935 % -b) / (196249246u % (uint)b));
				if ((((uint)b / (uint)(~(b & -1643799136))) ^ (uint)((((uint)b < (uint)b) ? 1 : 0) - 1)) >= 329u / (uint)(~b))
				{
					b = (byte)(-1166290774 + (b ^ 0x4584300B));
					continue;
				}
				goto IL_0143;
			case 6u:
				{
					b = (byte)(-25795 ^ (-4669055 / b));
					break;
				}
				IL_0143:
				stateMachine.action = action;
				b = b;
				b = (byte)(0x27 ^ ((uint)(b >>> (((int)b < (int)((uint)b / 1788836362u)) ? 1 : 0)) ^ ((((uint)b % (uint)(~b) < b) ? 1u : 0u) << ((3357833275u < (uint)b) ? 1 : 0))));
				continue;
			}
			break;
		}
		return stateMachine._003C_003Et__builder.Task;
	}

	private async Task<bool> WaitForTeamToBeInSameMap()
	{
		_003C_003Ec__DisplayClass20_0 CS_0024_003C_003E8__locals16 = new _003C_003Ec__DisplayClass20_0();
		CS_0024_003C_003E8__locals16._003C_003E4__this = this;
		if (!IsInTeam)
		{
			return true;
		}
		CS_0024_003C_003E8__locals16.leaderMapId = (int)InstanceData.MapInformation.MapId;
		TimeSpan timeout = _671BC22C.BF9F3D1F.FE8E0C9E[1010](120.0);
		TimeSpan pollInterval = _671BC22C.BF9F3D1F.FE8E0C9E[895](500.0);
		CS_0024_003C_003E8__locals16.sw = _671BC22C.BF9F3D1F.FE8E0C9E[104]();
		CS_0024_003C_003E8__locals16.i = 1;
		TaskAwaiter taskAwaiter = default(TaskAwaiter);
		while (_671BC22C.BF9F3D1F.FE8E0C9E[1909](_671BC22C.BF9F3D1F.FE8E0C9E[1619](CS_0024_003C_003E8__locals16.sw), timeout))
		{
			if (TeamInstanceDataWithLeader.All((InstanceData e) => e.MapInformation.MapId == CS_0024_003C_003E8__locals16.leaderMapId))
			{
				return true;
			}
			TeamInstanceDataWithLeader.Where((InstanceData e) => e.MapInformation.MapId != CS_0024_003C_003E8__locals16.leaderMapId).ToList().ForEach(delegate(InstanceData e)
			{
				ScriptManager scriptManager = CS_0024_003C_003E8__locals16._003C_003E4__this;
				string _970C = e.CharacterData.CharacterName;
				long mapId = e.MapInformation.MapId;
				TimeSpan CE12F = _671BC22C.BF9F3D1F.FE8E0C9E[1619](CS_0024_003C_003E8__locals16.sw);
				scriptManager.Log(new global::ED33D63D<string, global::_53B198A0<string, long, double, double, int>>("info.team.memberNotOnLeaderMap", new global::_53B198A0<string, long, double, double, int>(_970C, mapId, _671BC22C.BF9F3D1F.FE8E0C9E[903](ref CE12F), 20.0 - (double)(CS_0024_003C_003E8__locals16.i % 40) * 0.5, CS_0024_003C_003E8__locals16.leaderMapId)), "gathering", D32842AE._543E5320._703EA0A0);
			});
			TaskAwaiter _6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[2032](pollInterval));
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
			{
				await _6D28EB9F;
				_6D28EB9F = taskAwaiter;
				taskAwaiter = default(TaskAwaiter);
			}
			_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
			if (CS_0024_003C_003E8__locals16.i % 40 == 0)
			{
				ExecuteForTeam(delegate(InstanceData ctx)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[1566](ctx.GoToNearestZaap(_82210236.AB91771D(CS_0024_003C_003E8__locals16.leaderMapId, B31AE737: true)));
					return ctx.MovePlayerToWorldMapId(CS_0024_003C_003E8__locals16.leaderMapId);
				});
			}
			CS_0024_003C_003E8__locals16.i++;
		}
		_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Yellow);
		DefaultInterpolatedStringHandler D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](77, 1);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "[WARN] 120s écoulées, un ou plusieurs membres ne sont pas sur la même map (");
		D6A007B.AppendFormatted(CS_0024_003C_003E8__locals16.leaderMapId);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, ").");
		_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
		_671BC22C.BF9F3D1F.FE8E0C9E[759]();
		return false;
	}

	private unsafe void InitializeLuaFunctions(Lua lua)
	{
		_003C_003Ec__DisplayClass21_0 CS_0024_003C_003E8__locals159 = new _003C_003Ec__DisplayClass21_0();
		uint num = 1179644378u;
		num -= 3502551402u;
		sbyte b = default(sbyte);
		uint num2 = default(uint);
		while (true)
		{
			switch (num % 49)
			{
			default:
				num = ((num / (0xFB1C6939u | (0x3B91CB0F ^ num))) & ~(353058212 - num)) ^ 0x464FF1DA;
				goto IL_0100;
			case 1u:
				num = (uint)(((int)num - 1076157092 * (b & 0x6E0090A0)) / 1627630653 + 1179644378);
				CS_0024_003C_003E8__locals159.lua = lua;
				goto IL_0156;
			case 2u:
				num = (uint)(0x464FF1DA ^ (((b % 1143180475 * (0x7190637 ^ b)) & 0x333FFFB8) / 195270300));
				goto IL_0215;
			case 3u:
				num = (uint)((int)((uint)b / 1520832433u) % 1606327826 << (int)b / (int)num << 14);
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)(num * (num << (int)num << 4)) - -2087](CS_0024_003C_003E8__locals159.lua, "getCurrentPos", (Func<string>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.GetPosition()));
				_671BC22C.BF9F3D1F.FE8E0C9E[0x827 ^ (b & 0x2D3F1FB2)](CS_0024_003C_003E8__locals159.lua, "getMapId", (Func<int>)(() => (int)CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MapInformation.MapId));
				b = ((3488907134u > (0x318FC3A4 ^ (num / (uint)(~b))) / (uint)(-1985249663 | (1518748314 % (int)(~num)))) ? ((sbyte)1) : ((sbyte)0));
				if (b != 0)
				{
					num = (uint)(((int)(num + 1462900255) * -709635683 % ~(b >> (-2076545095 >> (int)b)) * -835562110) ^ 0x7C33279A);
					break;
				}
				goto IL_0100;
			case 4u:
				num = (uint)(((363221161 << (int)b) - ((b >>> (int)num) & b) >>> (int)(((num & (uint)b) << 7) & 0)) + -726442322);
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)((num >> 30 >> (int)(num | (uint)b)) / ~((646700307 == b) ? 1u : 0u) - (num << (int)((uint)(582651784 >> (int)b) % ~num))) - -2087](CS_0024_003C_003E8__locals159.lua, "getPodsMax", (Func<int>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CharacterData.MaxPods));
				num = (uint)(-1515962749 | b);
				num = (uint)(-33900803 ^ (-2 ^ b));
				break;
			case 5u:
				num = (uint)(-1482061946 + (~(b << (b >>> 24)) - ((int)num - (int)b)));
				goto IL_03f8;
			case 6u:
				num = (uint)((int)num % ((int)b + (int)((uint)(1603565622 - b) / 1587915404u)) - -626401391);
				_671BC22C.BF9F3D1F.FE8E0C9E[2037939193 + (-2037937106 >>> (int)(num * num))](CS_0024_003C_003E8__locals159.lua, "getJobLevel", new Func<int, int>(InstanceData.CharacterData.B523AC06));
				num = ~(1 & (num | num)) / ~(num - num);
				if (((uint)b / (uint)((int)num % 379193348) >> 999270570 / (-1014324558 * b)) - 1051717567 != 0)
				{
					num = 0 - (0 + (0 - num)) - 146848868;
					break;
				}
				goto IL_03f8;
			case 7u:
				num = (uint)(-1060473709 + ((int)num - -1207322577));
				_671BC22C.BF9F3D1F.FE8E0C9E[(0 ^ num) + 2086](CS_0024_003C_003E8__locals159.lua, "getCharacterLevel", (Func<int>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CharacterData.CharacterLevel));
				if (-1273085144 >>> (-29403 % (-644529738 >>> (int)b) >>> (int)num) != (int)(~(num >> (int)num % -1128759164)))
				{
					num = (uint)(-862297327 + (int)(1862550809 % num) / (b % 1486918533 + 587204610));
					break;
				}
				return;
			case 8u:
				num = 1u + (((uint)(-1539689291 << (int)b) < (~num & 0x903AA92Du) - num) ? 1u : 0u);
				_671BC22C.BF9F3D1F.FE8E0C9E[~b / (int)num - -2089](CS_0024_003C_003E8__locals159.lua, "getMaxQuantitesByGid", (Func<int, bool>)delegate(int gid)
				{
					Task<bool> maxItemByGid = CS_0024_003C_003E8__locals159._003C_003E4__this.GetMaxItemByGid(gid);
					_680DAE05._7422EE95(maxItemByGid);
					if (!maxItemByGid.Result)
					{
						CS_0024_003C_003E8__locals159._003C_003E4__this.StopScript(isStoppedManually: true);
					}
					return maxItemByGid.Result;
				});
				num = (uint)(1713157787 % (-1734110914 / ((0x43A2BEAB | b) + (int)(num % 3694310071u))));
				num = (uint)((int)num - -504573211 * (~b | (b * 724163208)) - 1419369803);
				break;
			case 9u:
				num = 0xFFFFFFFDu ^ (num ^ (0 - (num ^ (uint)b)));
				goto IL_0611;
			case 10u:
				num = (uint)(b + -446941200 - -446941199);
				_671BC22C.BF9F3D1F.FE8E0C9E[2087 + (int)num % (int)(~((uint)(-1995874145 % (int)(~num) << (int)b) / (uint)((int)(3532022925u % ~num) / (int)(~(715429281 * num)))))](CS_0024_003C_003E8__locals159.lua, "goAndOpenChest", (Func<int, int, string, bool>)delegate(int chestMapId, int chestCellId, string code)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.MoveAndOpenChest(chestMapId, chestCellId, code);
					_680DAE05._7422EE95(task);
					if (!task.Result)
					{
						CS_0024_003C_003E8__locals159._003C_003E4__this.StopScript(isStoppedManually: true);
					}
					return task.Result;
				});
				if ((uint)(-1176449561 / (33579040 % (b + -484617047))) >= (uint)((sbyte)((int)num ^ (int)b) ^ ((int)num * (b * b) - (512722442 << (int)b + (int)num))))
				{
					num = (uint)((byte)b / b + -436071021);
					break;
				}
				goto IL_0215;
			case 11u:
				num = (uint)(0x5DD9DFB5 ^ (1574559669 * b));
				_671BC22C.BF9F3D1F.FE8E0C9E[(((uint)(-1382187891 * (int)(num - 446505142)) > 3059788100u) ? 1 : 0) - -2087](CS_0024_003C_003E8__locals159.lua, "getInventoryItemCount", (Func<int, int>)((int gid) => (CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.Inventory.GetObjectByGid(gid) != null) ? _671BC22C.BF9F3D1F.FE8E0C9E[289](CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.Inventory.GetObjectByGid(gid)) : 0));
				_671BC22C.BF9F3D1F.FE8E0C9E[0x1861 ^ (0x1046 | num)](CS_0024_003C_003E8__locals159.lua, "teamUseInventoryItem", (Func<int, int, bool>)delegate(int gid, int quantity)
				{
					_003C_003Ec__DisplayClass21_2 CS_0024_003C_003E8__locals175 = new _003C_003Ec__DisplayClass21_2();
					CS_0024_003C_003E8__locals175.gid = gid;
					CS_0024_003C_003E8__locals175.quantity = quantity;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MessageHandler._368F20BF(CS_0024_003C_003E8__locals175.gid, CS_0024_003C_003E8__locals175.quantity));
				});
				b = (sbyte)((uint)(byte)(1795688757 % (int)(~num)) / (uint)b / (uint)(~(b - b)));
				if ((0xAC2E5FAFu ^ ((uint)b / (uint)(~(ushort)((int)b & (int)num)))) != 0)
				{
					num = (uint)(((0x2629E432 | -(455103167 * b)) >> 29) - -1211388056);
					break;
				}
				return;
			case 12u:
				num = 0x48345099 ^ num;
				goto IL_07d7;
			case 13u:
				num = num2 ^ 0x31921E29;
				goto IL_085f;
			case 14u:
				num = ((uint)b % 2034197426u) ^ 0xFF095ABAu ^ 0xFF095A84u;
				_671BC22C.BF9F3D1F.FE8E0C9E[((int)(30 + num) * -215488989 << 8) - -1393028647](CS_0024_003C_003E8__locals159.lua, "upgradeCharacterStatsBatchByName", (Func<string, int, int, int, int, int, int, bool>)delegate(string name, int agility, int strength, int vitality, int chance, int intelligence, int wisdom)
				{
					_003C_003Ec__DisplayClass21_3 CS_0024_003C_003E8__locals173 = new _003C_003Ec__DisplayClass21_3();
					CS_0024_003C_003E8__locals173.name = name;
					InstanceData instanceData = CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader?.FirstOrDefault((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[155](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals173.name, StringComparison.OrdinalIgnoreCase));
					if (instanceData == null)
					{
						return false;
					}
					Task<bool> task = instanceData.MessageHandler._251DD3AE(agility, strength, vitality, chance, intelligence, wisdom);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				_671BC22C.BF9F3D1F.FE8E0C9E[0x57189506 ^ (((640103468u > (uint)((int)(num / (uint)b) + -240608492)) ? 1u : 0u) | 0x57189D21u)](CS_0024_003C_003E8__locals159.lua, "upgradeCharacterStatsBatch", (Func<int, int, int, int, int, int, bool>)delegate(int agility, int strength, int vitality, int chance, int intelligence, int wisdom)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MessageHandler._251DD3AE(agility, strength, vitality, chance, intelligence, wisdom);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				num2 = (((((uint)b & num) > 2444922287u) ? 1u : 0u) & 0xFE369BB7u) >> 18;
				if ((int)(num2 % ~(num + num)) % -594200308 - b != 0)
				{
					num = ((uint)(-2062574786 / ~(b >> 19)) % (uint)(~(-145066379 / (((uint)b < 4246822421u) ? 1 : 0)))) ^ 0xFF6F23CFu;
					break;
				}
				goto IL_1257;
			case 15u:
				num = (ushort)num & ((((2057253147u > (uint)b) ? 1u : 0u) | (2267055632u % num)) / 3465845541u);
				_671BC22C.BF9F3D1F.FE8E0C9E[-7221484 + (int)(num + 7223571)](CS_0024_003C_003E8__locals159.lua, "useInventoryItem", (Func<int, int, bool>)delegate(int gid, int quantity)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MessageHandler._368F20BF(gid, quantity);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				num2 = (uint)((int)num >> (int)(0x7E89120D & num2));
				num = (uint)((byte)((int)num2 / 1862827707) - 2113671275);
				break;
			case 16u:
				num = (uint)(1580441217 + (-1580441233 | b));
				_671BC22C.BF9F3D1F.FE8E0C9E[(ushort)(-((int)num + -1396799218)) - 28363](CS_0024_003C_003E8__locals159.lua, "equipItem", (Func<int, int, bool>)delegate(int uid, int position)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MessageHandler._7C107FBB(uid, position);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				if ((int)(0 - num2) >> (int)num2 != (int)num2 + -1381588038)
				{
					num = 0xC853D875u ^ num ^ 0x34519C80;
					break;
				}
				goto IL_0c59;
			case 17u:
				num = (num - (num2 - (num2 << (25798191 >> (int)num2)))) ^ 0xFC0244F5u;
				goto IL_0a94;
			case 18u:
				num = (((int)((uint)(b | 0x4F2B022D) / (uint)(byte)(-2028760159 - b)) > 329440060) ? 1u : 0u);
				_671BC22C.BF9F3D1F.FE8E0C9E[-1543791352 + (b ^ 0x5C046B21)](CS_0024_003C_003E8__locals159.lua, "getInventoryItemByGid", (Func<int, object>)delegate(int gid)
				{
					RawInventory rawInventory = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData?.Inventory;
					if (rawInventory == null)
					{
						return (object)null;
					}
					JitsuriObjectItem objectByGid = rawInventory.GetObjectByGid(gid);
					if (objectByGid != null)
					{
						LuaTable obj = (LuaTable)_671BC22C.BF9F3D1F.FE8E0C9E[468](CS_0024_003C_003E8__locals159.lua, "return {}", "chunk")[0];
						F7BDF799.EB1D3905(obj, "uid", _671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid));
						F7BDF799.EB1D3905(obj, "gid", (int)_671BC22C.BF9F3D1F.FE8E0C9E[1827](objectByGid));
						F7BDF799.EB1D3905(obj, "quantity", _671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid));
						return obj;
					}
					return (object)null;
				});
				if ((byte)((int)num2 % (int)b >>> 1) << (int)num / 1334942257 > (int)(num & 0xFFFF8E8Eu))
				{
					num = (uint)((((int)num2 + (b >> (int)b) >> 22) | ((int)num * (b << 18))) ^ 0x4E3CEA91);
					break;
				}
				goto IL_0eb2;
			case 19u:
				num = (uint)(-335889952 * (int)num2 - -407492032);
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)(num | 0xA4C5AE79u) - -1530550702](CS_0024_003C_003E8__locals159.lua, "getEquippedItemAtPosition", (Func<int, object>)delegate(int position)
				{
					EquippedItem value = null;
					Dictionary<int, EquippedItem> dictionary = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CharacterData.EquippedItems;
					if (dictionary != null)
					{
						dictionary.TryGetValue(position, out value);
					}
					else if (CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CharacterData.EquippedItems is IList<EquippedItem> list && position >= 0 && position < list.Count)
					{
						value = list[position];
					}
					if (value != null)
					{
						LuaTable obj = (LuaTable)_671BC22C.BF9F3D1F.FE8E0C9E[468](CS_0024_003C_003E8__locals159.lua, "return {}", "chunk")[0];
						F7BDF799.EB1D3905(obj, "uid", (int)value.Uid);
						F7BDF799.EB1D3905(obj, "gid", (int)value.Gid);
						F7BDF799.EB1D3905(obj, "quantity", value.Quantity);
						return obj;
					}
					return (object)null;
				});
				_671BC22C.BF9F3D1F.FE8E0C9E[((454217121 % ((int)(4220840212u % num2) + -843966071)) | ((b >> 19) % b)) - 454215034](CS_0024_003C_003E8__locals159.lua, "craftItem", (Func<int, int, int, bool, bool, bool>)delegate(int itemGid, int mapIdBank, int mapIdWorkshop, bool specialCraft, bool returnBank)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CraftManager._97103C08(itemGid, mapIdBank, mapIdWorkshop, specialCraft, returnBank);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				num2 = (uint)((int)num >> 30 << (int)(~(0x6B6EB02 ^ num2)));
				if (((uint)(b << (int)(num2 << 10)) | ((uint)b / (((uint)b | num2) - num2 * 958114587))) != 0)
				{
					num = ((738868646 << (int)(~num) < (int)(1u ^ ((num + num > num2) ? 1u : 0u))) ? 1u : 0u) ^ 0xFF3BDC23u;
					break;
				}
				goto IL_105d;
			case 20u:
				num = (uint)(b + -62);
				goto IL_0c59;
			case 21u:
				num = 2110654401 + (num2 ^ 0x8231F83Fu);
				goto IL_0cd3;
			case 22u:
				num = (num2 | 0) >> 28 >> 16;
				_671BC22C.BF9F3D1F.FE8E0C9E[(((uint)((int)num2 % (int)(~(0 - (0 - num)))) > (0xC836B1A8u | (num2 % ~num2))) ? 1 : 0) + 2087](CS_0024_003C_003E8__locals159.lua, "getAllItems", _671BC22C.BF9F3D1F.FE8E0C9E[(int)num - -1129213436 + -1129211927](CS_0024_003C_003E8__locals159, (nint)__ldftn(_003C_003Ec__DisplayClass21_0._003CInitializeLuaFunctions_003Eb__25)));
				num2 += 3730302707u;
				num = (uint)(((-391518173 << (int)num << (int)num2) ^ ((int)num % (int)(~num))) - -1688674583);
				break;
			case 23u:
				num = (uint)(-63 + (sbyte)((int)num / (int)b + (b >>> 1604547234 / b)));
				goto IL_0e05;
			case 24u:
				num = (uint)(sbyte)((uint)(851043887 >> (int)b) + num2);
				goto IL_0eb2;
			case 25u:
				num = 0xA021831Bu ^ (3 + num);
				goto IL_0f1f;
			case 26u:
				num = (uint)((1058562 >>> ((int)num2 % ~b >> (int)(~num ^ (uint)(b >> (int)num)))) - 1058562);
				goto IL_0fcf;
			case 27u:
				num = (uint)((sbyte)(-1534410311 ^ (int)num) / (-308975346 / ~(-846822491 * b)) / (int)num2);
				goto IL_105d;
			case 28u:
				num = 583883123 + num2;
				goto IL_10ee;
			case 29u:
				num = 1157892646 + num2 + 3720957773u;
				goto IL_1154;
			case 30u:
				num = (uint)((b - 259596626) ^ 0x2D20FA2D);
				_671BC22C.BF9F3D1F.FE8E0C9E[576316324 + num](CS_0024_003C_003E8__locals159.lua, "exitDungeon", (Func<bool>)delegate
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[1566](CS_0024_003C_003E8__locals159._003C_003E4__this.WaitForTeamToBeInSameMap());
					_680DAE05._7422EE95(D1380F8D.D62C4B25([AsyncStateMachine(typeof(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_62_003Ed))] () =>
					{
						_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_62_003Ed stateMachine = default(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_62_003Ed);
						stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
						stateMachine._003C_003E1__state = -1;
						stateMachine._003C_003Et__builder.Start(ref stateMachine);
						return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
					}));
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MessageHandler.FA2C3C86());
				});
				b = (sbyte)((num2 + 1320029864) / (uint)(~((1637695252 - (-1314574545 >> (int)num)) & (-b / ~b))));
				if (~(157 % (int)num) != 0)
				{
					num = (uint)((b + -298664292 >>> 11) ^ -870779790);
					break;
				}
				return;
			case 31u:
				num = 3718653059u + (((int)(~(252152590 / num2)) < (((int)num2 + (int)b >> 9) ^ -1883624791)) ? 1u : 0u);
				goto IL_1257;
			case 32u:
				num = 2000012049 + num - 4063707304u;
				_671BC22C.BF9F3D1F.FE8E0C9E[(uint)(1585481246 >> ~b) / ((0 - 2467988516u / num2) ^ ~num2) + 2087](CS_0024_003C_003E8__locals159.lua, "replyNpc", (Func<int, bool>)delegate(int replyIndex)
				{
					_003C_003Ec__DisplayClass21_8 CS_0024_003C_003E8__locals168 = new _003C_003Ec__DisplayClass21_8();
					CS_0024_003C_003E8__locals168.replyIndex = replyIndex;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager._0399D809(CS_0024_003C_003E8__locals168.replyIndex));
				});
				b = (sbyte)(1017434890 << (int)(2810335514u / (num / (uint)((int)num2 / (int)num2))));
				num = ((1151466887 * b == b) ? 1u : 0u) + 2341954004u;
				break;
			case 33u:
				num = (uint)b ^ (((-535819359 == (int)num) ? 1u : 0u) % 1275799441u) ^ 0xDDA62489u;
				_671BC22C.BF9F3D1F.FE8E0C9E[2017 + (num2 & 0x4CD74DC6)](CS_0024_003C_003E8__locals159.lua, "replyNpcAndChangeMap", (Func<int, bool>)delegate(int replyIndex)
				{
					_003C_003Ec__DisplayClass21_9 CS_0024_003C_003E8__locals167 = new _003C_003Ec__DisplayClass21_9();
					CS_0024_003C_003E8__locals167.replyIndex = replyIndex;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam(delegate(InstanceData ctx)
					{
						_003C_003Ec__DisplayClass21_10 obj = new _003C_003Ec__DisplayClass21_10();
						obj.ctx = ctx;
						Func<GameMessage, bool> e586CB2B = (GameMessage msg) => _671BC22C.BF9F3D1F.FE8E0C9E[1647](msg) == GameMessage.ContentOneofCase.Event && _671BC22C.BF9F3D1F.FE8E0C9E[720](_671BC22C.BF9F3D1F.FE8E0C9E[1497](_671BC22C.BF9F3D1F.FE8E0C9E[1383](_671BC22C.BF9F3D1F.FE8E0C9E[160](msg))), obj.ctx.MessageHandler.F1A6D798[_79AC42A1.MapComplementaryInformationEvent]);
						Task<GameMessage> result = obj.ctx.MessageHandler._8E0F0412(e586CB2B, 2000);
						obj.ctx.NpcManager._0399D809(CS_0024_003C_003E8__locals167.replyIndex);
						return result;
					});
				});
				goto IL_1375;
			case 34u:
				num = (uint)((((int)num2 >> 7) | ((b >> 8) ^ -1771770534)) - -1195456297);
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)num / (int)(~num2) - 5189933](CS_0024_003C_003E8__locals159.lua, "goUseInteractive", (Func<int, bool>)delegate(int cellId)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MoveAndUseInteractive(cellId);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				b = (sbyte)(num2 / (uint)b % 3269004261u);
				num = 2299400103u + (((uint)b % 181403648u > 2645702421u) ? 1u : 0u);
				break;
			case 35u:
				num = (uint)(-576314237 ^ ((int)(0xBA12C15 ^ num) * ((int)(3179678724u / num) >> 3)));
				goto IL_144d;
			case 36u:
				num = 0xDDA624EDu ^ num2;
				_671BC22C.BF9F3D1F.FE8E0C9E[(((-2144129757 % (int)num) | (77903161 + b)) - (int)((~num2 | 0) % 18054)) ^ -404447920](CS_0024_003C_003E8__locals159.lua, "openSellerHdv", (Func<int, bool>)((int cellId) => CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.HdvManager._4000929B(ctx.MapInformation.MapId))));
				goto IL_14fb;
			case 37u:
				_671BC22C.BF9F3D1F.FE8E0C9E[-163509394 + (int)((uint)(-1107068119 % (181 >> (int)num)) % 4131455812u)](CS_0024_003C_003E8__locals159.lua, "sellAllItems", (Func<bool>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.HdvManager._10232BBF())));
				num2 = ~num >> 0 - (((uint)b > 0 - num) ? 1 : 0);
				num = (uint)(-95556559 ^ (b | -2867200));
				break;
			case 38u:
				num = (uint)((int)(0 - num2) % (int)(1863483957u / (uint)b >> (int)b) + (int)num / (int)(0 - num) + -576313760);
				goto IL_15b4;
			case 39u:
				num = (uint)(-307878781 + ((int)(num / num) - -1506765038 << (int)num2));
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)((((uint)((b & -1867207258) / 6) > num) ? 1u : 0u) | (uint)b) - -1961](CS_0024_003C_003E8__locals159.lua, "getDefenders", (Func<object>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.FighterManager.Defenders.Select(FighterToLua).ToList()));
				if (1252879277 % ((int)(((603989025 > (int)num) ? 1u : 0u) | 0xDB29692u) / ~(-887649102 >>> (int)b >> 20)) != 0)
				{
					num = (uint)(b - 1191569392 - 1803147612);
					break;
				}
				goto IL_1154;
			case 40u:
				num = (uint)(-576314725 ^ ((int)((num ^ 0xF489B83Eu) * ((uint)(-1652331123 ^ b) / num)) % (-874671535 >>> (int)b << 11)));
				goto IL_16fa;
			case 41u:
				num = (uint)((sbyte)((int)num2 & -b) - 576314113);
				_671BC22C.BF9F3D1F.FE8E0C9E[(1279037203 >>> (int)(((uint)b % num2) ^ 0x21B8F018 ^ (uint)(-460489815 - b))) - 154045](CS_0024_003C_003E8__locals159.lua, "getTeamMemberReachableCells", (Func<string, object>)delegate(string name)
				{
					_003C_003Ec__DisplayClass21_12 CS_0024_003C_003E8__locals166 = new _003C_003Ec__DisplayClass21_12();
					CS_0024_003C_003E8__locals166.name = name;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals166.name))?.FighterManager.FindReachableCells();
				});
				if (3158217227u / num == 0)
				{
					num = (uint)((int)(0x4E14AA9Fu & (((int)num2 > (int)num2) ? 1u : 0u)) % -28673864 - -96263727);
					break;
				}
				goto IL_14fb;
			case 42u:
				num = (uint)((34798637 / (int)(0 - num2)) ^ -576314113);
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)num - ((680307758 % -b) & -1165509585) + 576316236](CS_0024_003C_003E8__locals159.lua, "teamMemberMoveToCellId", (Func<string, int, bool>)delegate(string name, int cellId)
				{
					_003C_003Ec__DisplayClass21_13 CS_0024_003C_003E8__locals164 = new _003C_003Ec__DisplayClass21_13();
					CS_0024_003C_003E8__locals164.name = name;
					InstanceData instanceData = CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals164.name));
					if (instanceData == null)
					{
						return false;
					}
					int value = instanceData.FighterManager.GetSelf().Characteristics[CharacteristicKeyword.MovementPoints].Value;
					Task<bool> task = instanceData.FighterManager.Fighter.MoveTo(cellId, value);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				if ((453672858 - num2) / (num2 % 4255168669u) % 1 == 0)
				{
					num = (uint)(0x4586C82A ^ (-442260086 + b));
					break;
				}
				goto IL_18c1;
			case 43u:
				num = (num2 | 0x9BB27581u) + 564798722;
				_671BC22C.BF9F3D1F.FE8E0C9E[-1315979077 - b - -1315981290](CS_0024_003C_003E8__locals159.lua, "teamMemberCastSpellOnCellId", (Func<string, int, int, bool>)delegate(string name, int spellId, int cellId)
				{
					_003C_003Ec__DisplayClass21_14 CS_0024_003C_003E8__locals163 = new _003C_003Ec__DisplayClass21_14();
					CS_0024_003C_003E8__locals163.name = name;
					InstanceData instanceData = CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals163.name));
					if (instanceData == null)
					{
						return false;
					}
					Task<bool> task = instanceData.FighterManager.ForgeAndSendGameActionFightCastRequest(spellId, cellId);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				if (((num ^ (uint)(-2071124545 / (int)num2)) | (uint)((int)num % (int)num)) != 0)
				{
					num = (uint)(1739854363 + (1270221616 << (b | -294634063)));
					break;
				}
				goto IL_0f1f;
			case 44u:
				num = 3718653183u + ((num == (uint)(0 << ~(-1046624214 << (int)b))) ? 1u : 0u);
				goto IL_18c1;
			case 45u:
				num = 3142338947u + num2;
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)(num - 328102319) - -904418519](CS_0024_003C_003E8__locals159.lua, "getTeamMemberSpellPossibleCellsOnCellId", (Func<string, int, int, object>)delegate(string name, int spellId, int cellId)
				{
					_003C_003Ec__DisplayClass21_16 CS_0024_003C_003E8__locals162 = new _003C_003Ec__DisplayClass21_16();
					CS_0024_003C_003E8__locals162.name = name;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals162.name))?.FighterManager.GetSpellPossibleCellsOnCellId(spellId, cellId);
				});
				b = (sbyte)(~num2);
				if (((1921812775 > num / (0 - (num ^ 0xF015408Cu))) ? 1u : 0u) < (uint)((int)(num2 * num2) * (int)b + (int)(((-457762903 == (int)(0 - num)) ? 1u : 0u) >> (int)b)))
				{
					num = (((((0 == b) ? 1 : 0) > (0x6B837025 | b)) ? 1u : 0u) << (int)(~(~num2))) + 3392088324u;
					break;
				}
				goto IL_0fcf;
			case 46u:
				num = (uint)((b ^ ((int)(num - num2) + ((int)num2 >> -b))) - 1787071755);
				_671BC22C.BF9F3D1F.FE8E0C9E[-b + 1962](CS_0024_003C_003E8__locals159.lua, "getTeamMemberSpellZoneOnCellId", (Func<string, int, int, object>)delegate(string name, int spellId, int cellId)
				{
					_003C_003Ec__DisplayClass21_17 CS_0024_003C_003E8__locals161 = new _003C_003Ec__DisplayClass21_17();
					CS_0024_003C_003E8__locals161.name = name;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals161.name))?.FighterManager.GetSpellZoneOnCellId(spellId, cellId);
				});
				if (3200236479u % (num2 * num) - 85724554 != 0)
				{
					num = 120962602 + (num2 << (int)(~num2)) + 2092516387;
					break;
				}
				goto IL_0100;
			case 47u:
				num = ((num2 < (uint)(b ^ -20730)) ? 1u : 0u) % ((num2 - num) % (uint)((int)num + -600792563)) - 576314113;
				_671BC22C.BF9F3D1F.FE8E0C9E[0xA051CCAFu ^ ((uint)(b - ((byte)num2 ^ 0x5FAE3A87)) % ~(((-1574755564 >> (int)num < (int)((uint)b % num)) ? 1u : 0u) & num2))](CS_0024_003C_003E8__locals159.lua, "teamMemberFinishTurn", (Func<string, bool>)delegate(string name)
				{
					_003C_003Ec__DisplayClass21_18 CS_0024_003C_003E8__locals160 = new _003C_003Ec__DisplayClass21_18();
					CS_0024_003C_003E8__locals160.name = name;
					InstanceData instanceData = CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals160.name));
					if (instanceData == null)
					{
						return false;
					}
					Task<bool> task = instanceData.FighterManager.ForgeAndSendFightTurnReadyRequest();
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				num = (uint)((b + ~b) ^ 0x58B4670A);
				if (((uint)((int)(0x1F2E6592 | num2) * -170106431) & (0x13C3797u | ((b < -1268775359) ? 1u : 0u))) != 0)
				{
					num = 3740051384u + (num ^ 0x120702A1);
					break;
				}
				goto IL_1375;
			case 48u:
				{
					num = (uint)((int)(num2 % num) / (int)((ushort)num | (849772544 / num2)) - 1488251950);
					_671BC22C.BF9F3D1F.FE8E0C9E[(-(-184361981 - (int)num) >> 22) ^ -2322](CS_0024_003C_003E8__locals159.lua, "deleteItem", (Func<int, bool>)delegate(int gid)
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[968](gid);
						JitsuriObjectItem objectByGid = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.Inventory.GetObjectByGid(gid);
						if (objectByGid != null)
						{
							ScriptManager scriptManager = CS_0024_003C_003E8__locals159._003C_003E4__this;
							DefaultInterpolatedStringHandler ADB0868E = default(DefaultInterpolatedStringHandler);
							_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 60, 2);
							_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "[DEBUG] Objet trouvé dans l'inventaire : UID = ");
							ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid));
							_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ", quantité = ");
							ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid));
							scriptManager.Log(new global::E5A6EF93<string>(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E)), "gathering", D32842AE._543E5320._5D225697);
							if (_671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid) <= 0)
							{
								_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 58, 1);
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "[INFO] Objet GID ");
								ADB0868E.AppendFormatted(gid);
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " présent mais quantité nulle ou invalide.");
								_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E));
								return false;
							}
							int num3 = _671BC22C.BF9F3D1F.FE8E0C9E[289](objectByGid);
							Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MessageHandler.DB02459F(_671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid), num3);
							_671BC22C.BF9F3D1F.FE8E0C9E[1566](task);
							bool result = task.Result;
							_17348D39._53192AA0(result ? ConsoleColor.Green : ConsoleColor.Red);
							if (result)
							{
								ScriptManager scriptManager2 = CS_0024_003C_003E8__locals159._003C_003E4__this;
								_671BC22C.BF9F3D1F.FE8E0C9E[1164](ref ADB0868E, 51, 3);
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, "[SUCCÈS] Objet GID ");
								ADB0868E.AppendFormatted(gid);
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, " (UID: ");
								ADB0868E.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[303](objectByGid));
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ") supprimé avec succès x");
								ADB0868E.AppendFormatted(num3);
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref ADB0868E, ".");
								scriptManager2.Log(new global::E5A6EF93<string>(_671BC22C.BF9F3D1F.FE8E0C9E[934](ref ADB0868E)), "gathering", (!result) ? D32842AE._543E5320.B3B830AD : D32842AE._543E5320._5D225697);
								return task.Result;
							}
						}
						return false;
					});
					return;
				}
				IL_18c1:
				do
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[-219354133 ^ (b - 219356338)](CS_0024_003C_003E8__locals159.lua, "getTeamMemberSpellPossibleCells", (Func<string, int, object>)delegate(string name, int spellId)
					{
						_003C_003Ec__DisplayClass21_15 CS_0024_003C_003E8__locals165 = new _003C_003Ec__DisplayClass21_15();
						CS_0024_003C_003E8__locals165.name = name;
						return CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader.Find((InstanceData it) => _671BC22C.BF9F3D1F.FE8E0C9E[720](it.CharacterData.CharacterName, CS_0024_003C_003E8__locals165.name))?.FighterManager.GetSpellPossibleCells(spellId);
					});
				}
				while (num2 == (0x7F9F9895 ^ num2));
				num = (uint)((-1148295795 % (int)(num | (uint)(~b))) ^ 0x6320E7BE);
				break;
				IL_0100:
				CS_0024_003C_003E8__locals159._003C_003E4__this = this;
				b = (sbyte)(((int)num % (int)num << (int)num) / (int)num);
				if (-56952 * (ushort)b == 0)
				{
					num = 252625880 * num + 4219419275u;
					break;
				}
				goto IL_1154;
				IL_0c59:
				_671BC22C.BF9F3D1F.FE8E0C9E[(-1340690628 << (int)(0 - num * num - 94418346)) ^ -822081497](CS_0024_003C_003E8__locals159.lua, "putAllItems", _671BC22C.BF9F3D1F.FE8E0C9E[(0xF3AF0315u ^ (0 - num2)) + 206635728](CS_0024_003C_003E8__locals159, (nint)__ldftn(_003C_003Ec__DisplayClass21_0._003CInitializeLuaFunctions_003Eb__23)));
				if (b % 1 == 0)
				{
					num = 2357908393u + ~((num | (uint)b) / ~num);
					break;
				}
				goto IL_10ee;
				IL_1375:
				_671BC22C.BF9F3D1F.FE8E0C9E[(((num2 & 0x8E107424u) < 2937417634u) ? 1 : 0) - -2086](CS_0024_003C_003E8__locals159.lua, "leaveDialog", (Func<bool>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager.CC17DB97())));
				if (b + 0 != 0)
				{
					num = ~(((int)(num2 ^ 0xA0C9F15) > -1365546474) ? 1u : 0u) - 718873809;
					break;
				}
				goto IL_085f;
				IL_03f8:
				_671BC22C.BF9F3D1F.FE8E0C9E[0x5A5BC95A ^ (-((int)num / (int)b) << b / 1227847068)](CS_0024_003C_003E8__locals159.lua, "getPods", (Func<int>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CharacterData.Pods));
				num = (uint)(b * 626401392);
				if (1965853570 / (int)num >> 24 == 0)
				{
					num = (uint)(373726095 + ((int)(~(num / 1996837123) << 23) >> (int)b));
					break;
				}
				goto IL_14fb;
				IL_0fcf:
				_671BC22C.BF9F3D1F.FE8E0C9E[2017002253 / ((int)num - (28881943 + b >> 12)) - -288146](CS_0024_003C_003E8__locals159.lua, "changeMapByCellId", (Func<int, bool>)delegate(int cellId)
				{
					_003C_003Ec__DisplayClass21_4 CS_0024_003C_003E8__locals169 = new _003C_003Ec__DisplayClass21_4();
					CS_0024_003C_003E8__locals169.cellId = cellId;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MovePlayerAndChangeMap(CS_0024_003C_003E8__locals169.cellId, -1));
				});
				if (~(sbyte)num2 <= -1409286144 - (-695362011 ^ (1929664700 - (int)num / (int)(~num))))
				{
					num = (uint)(0x669863A3 ^ ((int)(num2 - num2) % (int)(num - ((1939824047 / num2) | num2))));
					break;
				}
				goto IL_15b4;
				IL_1154:
				_671BC22C.BF9F3D1F.FE8E0C9E[1977 + (short)((int)b ^ (int)num2)](CS_0024_003C_003E8__locals159.lua, "talkNpc", (Func<int, bool>)delegate(int npcActorId)
				{
					_003C_003Ec__DisplayClass21_6 CS_0024_003C_003E8__locals170 = new _003C_003Ec__DisplayClass21_6();
					CS_0024_003C_003E8__locals170.npcActorId = npcActorId;
					_671BC22C.BF9F3D1F.FE8E0C9E[1566](CS_0024_003C_003E8__locals159._003C_003E4__this.WaitForTeamToBeInSameMap());
					_680DAE05._7422EE95(D1380F8D.D62C4B25([AsyncStateMachine(typeof(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_60_003Ed))] () =>
					{
						_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_60_003Ed stateMachine = default(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_60_003Ed);
						stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
						stateMachine._003C_003E1__state = -1;
						stateMachine._003C_003Et__builder.Start(ref stateMachine);
						return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
					}));
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager.CC0805BE(ctx.MapInformation.MapId, CS_0024_003C_003E8__locals170.npcActorId, 3));
				});
				num = (uint)((-576314237 % (int)num) & (~((int)b / (int)num2) - (int)(218 / num)));
				if (b == 0)
				{
					num = (uint)(-2034620123 ^ ((1061072428 - (749063574 >> (int)b)) & -625409876));
					break;
				}
				goto IL_0a94;
				IL_15b4:
				_671BC22C.BF9F3D1F.FE8E0C9E[-1437303 + ((-1347099080 >>> (int)b) - (int)(~(((int)(num % 553881474) > -2105242853) ? 1u : 0u)))](CS_0024_003C_003E8__locals159.lua, "isFighting", (Func<bool>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CharacterData.IsFighting));
				_671BC22C.BF9F3D1F.FE8E0C9E[0xBB4C4121u ^ (num << (int)(short)((uint)b / (uint)b % 197358624))](CS_0024_003C_003E8__locals159.lua, "getAttackers", (Func<object>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.FighterManager.Attackers.Select(FighterToLua).ToList()));
				b = (sbyte)(-534698458 | (int)num2);
				num = ((3795341966u < (uint)b) ? 1u : 0u) + 3440177829u;
				break;
				IL_0f1f:
				_671BC22C.BF9F3D1F.FE8E0C9E[(0x2B3BB595 | ((int)num - (b ^ 0x6E6D67F))) - -79973010](CS_0024_003C_003E8__locals159.lua, "goToCellId", (Func<int, bool>)delegate(int cellId)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MovePlayerOnCellId(cellId);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				num2 = (uint)((int)((0 - num >> ((1812039571 < num2) ? 1 : 0)) | (uint)(-110 / (int)(~(1160707901 * num)))) % (int)(short)(((int)num >> 25) ^ (int)(num2 | 0xC027F8B)));
				if (0 % ~(781453065 >> (int)b) % ~(b >> (int)((num2 >> (int)num2) * num)) == 0)
				{
					num = (uint)(b - 897334073);
					break;
				}
				goto IL_0e05;
				IL_14fb:
				_671BC22C.BF9F3D1F.FE8E0C9E[0x82F ^ (0x30FB54CC & b)](CS_0024_003C_003E8__locals159.lua, "closeSellerHdv", (Func<bool>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.NpcManager.CC17DB97())));
				b = (sbyte)(-226399200 - -b);
				break;
				IL_0e05:
				_671BC22C.BF9F3D1F.FE8E0C9E[2087 + (uint)(-1356378587 | ((b - b) % ~(b / 1329288753))) / (uint)(-1391802190 | (379691498 - (int)num2 * -1599801728))](CS_0024_003C_003E8__locals159.lua, "getExistingItems", _671BC22C.BF9F3D1F.FE8E0C9E[((uint)((int)num2 / (int)(0x14FE2B66 ^ num2)) % (uint)(1 * b)) ^ 0x5E5](CS_0024_003C_003E8__locals159, (nint)__ldftn(_003C_003Ec__DisplayClass21_0._003CInitializeLuaFunctions_003Eb__26)));
				num2 = (uint)(-1248278247 * (b << -575933515 - b - (int)num));
				num = (uint)(0x8552B1A ^ (((int)num2 + -535828465) ^ (0x71A06590 | (b - -1682608199))));
				break;
				IL_0eb2:
				_671BC22C.BF9F3D1F.FE8E0C9E[(176748989 * num / ~((((num2 | (uint)b) & num2) > 916556179) ? 1u : 0u)) ^ 0x827](CS_0024_003C_003E8__locals159.lua, "DropItem", (Func<int, int, bool>)delegate(int gid, int quantity)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MessageHandler._56B395BF(gid, quantity);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				b = (sbyte)(num2 & 0x4A7B4671);
				if (num != (uint)(-208381132 + (int)num2))
				{
					num = 0xA7218318u ^ (num2 ^ (uint)b);
					break;
				}
				goto IL_105d;
				IL_0cd3:
				_671BC22C.BF9F3D1F.FE8E0C9E[-2509 ^ (-1115168840 / (~(-1993894747 / b) & -1942501610))](CS_0024_003C_003E8__locals159.lua, "putExistingItems", _671BC22C.BF9F3D1F.FE8E0C9E[~((int)num >> (b << 8)) - -1510](CS_0024_003C_003E8__locals159, (nint)__ldftn(_003C_003Ec__DisplayClass21_0._003CInitializeLuaFunctions_003Eb__24)));
				if ((((uint)(b << (int)num2) > 1067921937u) ? 1 : 0) >= (int)((uint)(499318623 >> (int)num2) / ((uint)(-b >> (int)num2) % 3322944283u)))
				{
					num = (uint)(((int)(num2 / ~((0u < (uint)(byte)num) ? 1u : 0u)) % (1251611205 << (int)num2)) ^ 0x29034809);
					break;
				}
				goto IL_0c59;
				IL_105d:
				_671BC22C.BF9F3D1F.FE8E0C9E[(((int)(num2 & 0x6703EF21) >> b - 78177679 >> (int)(~num2) == (int)((((uint)(-1751369209 - (int)num) < (uint)(b - b)) ? 1u : 0u) | 0xFC254321u)) ? 1 : 0) + 2087](CS_0024_003C_003E8__locals159.lua, "teamGoToCellId", (Func<int, bool>)delegate(int cellId)
				{
					_003C_003Ec__DisplayClass21_5 CS_0024_003C_003E8__locals171 = new _003C_003Ec__DisplayClass21_5();
					CS_0024_003C_003E8__locals171.cellId = cellId;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MovePlayerOnCellId(CS_0024_003C_003E8__locals171.cellId));
				});
				num = 583883123 + num2;
				if ((uint)(-132396 * b) < 1287762793 * num)
				{
					num = (uint)(1160752122 + (338183203 / (int)num + -1786698321));
					break;
				}
				goto IL_144d;
				IL_144d:
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)(0 - num / num2) - -33808023](CS_0024_003C_003E8__locals159.lua, "teamGoUseInteractive", (Func<int, bool>)delegate(int cellId)
				{
					_003C_003Ec__DisplayClass21_11 CS_0024_003C_003E8__locals172 = new _003C_003Ec__DisplayClass21_11();
					CS_0024_003C_003E8__locals172.cellId = cellId;
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam((InstanceData ctx) => ctx.MoveAndUseInteractive(CS_0024_003C_003E8__locals172.cellId));
				});
				if ((uint)(b / -7) >= 0 - num2 * (0x220E7A36 ^ num2) * num)
				{
					num = (uint)(((b >>> 19) * (-567613122 << (int)(0x4E030E1E | num))) ^ 0x50B29521);
					break;
				}
				goto IL_0156;
				IL_0215:
				_671BC22C.BF9F3D1F.FE8E0C9E[((4254681012u < (((uint)b % num) & (uint)b)) ? 1 : 0) - -1 - -2086](CS_0024_003C_003E8__locals159.lua, "stopScript", _671BC22C.BF9F3D1F.FE8E0C9E[((-69 > (int)(num >> 2)) ? 1u : 0u) ^ 0x5E5u](CS_0024_003C_003E8__locals159, (nint)__ldftn(_003C_003Ec__DisplayClass21_0._003CInitializeLuaFunctions_003Eb__2)));
				num = (uint)b;
				num = 654555196 * num * (num << -1507630293 + b) - 258291825;
				break;
				IL_0156:
				_671BC22C.BF9F3D1F.FE8E0C9E[0xD46CE3 ^ (13919428 >> b / 85811121)](CS_0024_003C_003E8__locals159.lua, "printMessage", (Action<string, string>)delegate(string message, string color)
				{
					string a11A83A = _671BC22C.BF9F3D1F.FE8E0C9E[235](color);
					D32842AE._543E5320 _543E = ((!_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "yellow")) ? (_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "green") ? D32842AE._543E5320._5D225697 : (_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "red") ? D32842AE._543E5320.B3B830AD : ((!_671BC22C.BF9F3D1F.FE8E0C9E[720](a11A83A, "blue")) ? D32842AE._543E5320._703EA0A0 : D32842AE._543E5320._703EA0A0))) : D32842AE._543E5320._7C2AA78D);
					D32842AE._543E5320 dB93A = _543E;
					D32842AE._0700BABF("gathering", message, CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.ProcessId, dB93A);
				});
				_638791A3.FD379696(CS_0024_003C_003E8__locals159.lua, "delay", (Action<int>)delegate(int milliseconds)
				{
					_003C_003Ec__DisplayClass21_1 obj = new _003C_003Ec__DisplayClass21_1();
					obj.milliseconds = milliseconds;
					_680DAE05._7422EE95(D1380F8D.D62C4B25([AsyncStateMachine(typeof(_003C_003Ec__DisplayClass21_1._003C_003CInitializeLuaFunctions_003Eb__55_003Ed))] () =>
					{
						_003C_003Ec__DisplayClass21_1._003C_003CInitializeLuaFunctions_003Eb__55_003Ed stateMachine = default(_003C_003Ec__DisplayClass21_1._003C_003CInitializeLuaFunctions_003Eb__55_003Ed);
						stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
						stateMachine._003C_003E4__this = obj;
						stateMachine._003C_003E1__state = -1;
						stateMachine._003C_003Et__builder.Start(ref stateMachine);
						return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
					}));
				});
				if (-(-1248820547 / ~(b * (b & b))) != 0)
				{
					num = 0x207FB726 ^ ((uint)((int)num + (b >> (int)(num % 2268455306u))) % 4188823842u);
					break;
				}
				goto IL_16fa;
				IL_1257:
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)num2 - -1977](CS_0024_003C_003E8__locals159.lua, "talkNpcId", (Func<int, bool>)delegate(int npcId)
				{
					_003C_003Ec__DisplayClass21_7 CS_0024_003C_003E8__locals176 = new _003C_003Ec__DisplayClass21_7();
					CS_0024_003C_003E8__locals176.npcId = npcId;
					_671BC22C.BF9F3D1F.FE8E0C9E[1566](CS_0024_003C_003E8__locals159._003C_003E4__this.WaitForTeamToBeInSameMap());
					_680DAE05._7422EE95(D1380F8D.D62C4B25([AsyncStateMachine(typeof(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_64_003Ed))] () =>
					{
						_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_64_003Ed stateMachine = default(_003C_003Ec._003C_003CInitializeLuaFunctions_003Eb__21_64_003Ed);
						stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[1862]();
						stateMachine._003C_003E1__state = -1;
						stateMachine._003C_003Et__builder.Start(ref stateMachine);
						return _671BC22C.BF9F3D1F.FE8E0C9E[1982](ref stateMachine._003C_003Et__builder);
					}));
					return CS_0024_003C_003E8__locals159._003C_003E4__this.ExecuteForTeam(delegate(InstanceData ctx)
					{
						JitsuriActorPositionInformation jitsuriActorPositionInformation = ctx.MapInformation?.Actors?.FirstOrDefault((JitsuriActorPositionInformation a) => a != null && _671BC22C.BF9F3D1F.FE8E0C9E[148](a)?.RolePlayActor?.NpcActor?.NpcId == CS_0024_003C_003E8__locals176.npcId);
						return (jitsuriActorPositionInformation == null) ? Task.FromResult(result: false) : ctx.NpcManager.CC0805BE(ctx.MapInformation.MapId, (int)_671BC22C.BF9F3D1F.FE8E0C9E[444](jitsuriActorPositionInformation), 3);
					});
				});
				if (387325472 >>> b - ((b / 169738505) & (-1649681100 >> (int)num2)) != b)
				{
					num = 0 - num - 3383900515u;
					break;
				}
				goto IL_0215;
				IL_10ee:
				_671BC22C.BF9F3D1F.FE8E0C9E[(int)num2 - -1977](CS_0024_003C_003E8__locals159.lua, "goToMapId", (Func<int, bool>)delegate(int mapId)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[1566](CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.GoToNearestZaap(_82210236.AB91771D(mapId, B31AE737: true)));
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MovePlayerToWorldMapId(mapId);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				b = (sbyte)((0 - num2) / (0 - (num >> 27)) << (int)b);
				if (42 + (0 - num) != 0)
				{
					num = (uint)(2671127 * b + (int)num + 1833999550);
					break;
				}
				goto IL_0a94;
				IL_085f:
				_671BC22C.BF9F3D1F.FE8E0C9E[((int)(371041330 - num) >> (int)num) - 371039243](CS_0024_003C_003E8__locals159.lua, "getTeam", (Func<object>)delegate
				{
					List<object> list = new List<object>();
					foreach (InstanceData item in CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader)
					{
						list.Add(new Dictionary<string, object>
						{
							["breedId"] = item.CharacterData.CharacterBreedId,
							["name"] = item.CharacterData.CharacterName,
							["level"] = item.CharacterData.CharacterLevel,
							["id"] = item.CharacterData.CharacterId
						});
					}
					return list.ToArray();
				});
				b = (sbyte)(21566 << (((b > 13845633) ? 1 : 0) >> (int)num));
				if ((int)(576 - num2) <= (b & -1868852597))
				{
					num = 0xBD28E727u ^ ((39829087 == num2) ? 1u : 0u);
					break;
				}
				goto IL_07d7;
				IL_0a94:
				_671BC22C.BF9F3D1F.FE8E0C9E[0x827 ^ (b / (512478474 << (int)(num & num)))](CS_0024_003C_003E8__locals159.lua, "unequipItem", (Func<int, bool>)delegate(int uid)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.MessageHandler._7C107FBB(uid, 63);
					_680DAE05._7422EE95(task);
					return task.Result;
				});
				num2 = (uint)(0xFC47A4F & b);
				if ((int)(num & 0x749A7C38) < -1853562317 / (int)(~(0x8F8FB7B8u & num2) << 7))
				{
					num = (1674535010 - num2) ^ 0x42520DDE;
					break;
				}
				goto IL_0611;
				IL_07d7:
				do
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[(((uint)((-465329086 >> (int)(num * 1619147033)) + 1077001788) < 2419988159u) ? 1 : 0) - -2086](CS_0024_003C_003E8__locals159.lua, "getClass", (Func<int>)(() => CS_0024_003C_003E8__locals159._003C_003E4__this.InstanceData.CharacterData.CharacterBreedId));
					num2 = (uint)(2098813067 >>> ((int)num >> 0 << 3)) ^ (num + ((num << 14) + 1284196002));
				}
				while ((uint)(((int)num2 / ~b) & -539497280) <= num2);
				num = (uint)((ushort)num2 ^ -1623894050);
				break;
				IL_0611:
				_671BC22C.BF9F3D1F.FE8E0C9E[2086 + (uint)b % 67702278u](CS_0024_003C_003E8__locals159.lua, "goToAndEnterHouse", (Func<int, string, string, bool>)delegate(int doorMapId, string nickname, string code)
				{
					Task<bool> task = CS_0024_003C_003E8__locals159._003C_003E4__this.GoToAndEnterHouse(doorMapId, nickname, code);
					_680DAE05._7422EE95(task);
					if (!task.Result)
					{
						CS_0024_003C_003E8__locals159._003C_003E4__this.StopScript(isStoppedManually: true);
					}
					return task.Result;
				});
				if (((4294967294u < 0 - num) ? 1u : 0u) >= (((int)(0 - num) > 0) ? 1u : 0u))
				{
					num = (uint)(0x2D288A05 ^ (((1391856478u < (uint)(-1033721366 << (int)b)) ? 1 : 0) % (int)(~(num >> (int)b << (int)b))));
					break;
				}
				goto IL_0cd3;
				IL_16fa:
				_671BC22C.BF9F3D1F.FE8E0C9E[0x22DBA ^ ((uint)((int)num2 >> (int)(ushort)num) / (uint)((b & 0x4A8EF20) << 4))](CS_0024_003C_003E8__locals159.lua, "getCharacterTurn", (Func<object>)delegate
				{
					InstanceData instanceData = CS_0024_003C_003E8__locals159._003C_003E4__this.TeamInstanceDataWithLeader?.FirstOrDefault((InstanceData x) => x != null && x.FighterManager?.IsMyTurn == true);
					return (instanceData == null) ? null : new Dictionary<string, object>
					{
						["breedId"] = instanceData.CharacterData.CharacterBreedId,
						["name"] = instanceData.CharacterData.CharacterName,
						["level"] = instanceData.CharacterData.CharacterLevel,
						["id"] = instanceData.CharacterData.CharacterId
					};
				});
				num |= (uint)b;
				num = ~(~((num >> (int)b > 4206200339u) ? 1u : 0u)) ^ 0xF7BA439Bu;
				break;
			}
		}
	}

	private static Dictionary<string, object> FighterToLua(FighterManager.ActorFighter f)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			["gid"] = f.Gid,
			["actorId"] = f.ActorId,
			["cellId"] = f.CellId,
			["isAlive"] = f.IsAlive,
			["isSummon"] = f.IsSummon,
			["team"] = f.Team,
			["teamId"] = f.TeamId,
			["permanentDamage"] = f.PermanentDamage,
			["lifePct"] = f.GetLifePointPercentage()
		};
		Dictionary<string, int> value = new Dictionary<string, int>
		{
			["hitPoints"] = f.Characteristics[CharacteristicKeyword.HitPoints].Value,
			["vitality"] = f.Characteristics[CharacteristicKeyword.Vitality].Value,
			["hitPointLoss"] = f.Characteristics[CharacteristicKeyword.HitPointLoss].Value
		};
		dictionary["characteristics"] = value;
		if (f is FighterManager.Defender defender)
		{
			dictionary["monsterGrade"] = defender.MonsterGrade;
			dictionary["monsterLevel"] = defender.MonsterLevel;
		}
		return dictionary;
	}

	private bool IsInventoryFull(Lua lua)
	{
		ushort num = 40526;
		if (num >> (int)num >> (int)num == ((((463931708 > num / num) ? 1 : 0) < -1073741824) ? 1 : 0) >> (729700247 >>> (int)num))
		{
			num = (ushort)(-811015602 ^ (-811050228 ^ num));
			goto IL_0044;
		}
		goto IL_00ef;
		IL_00ef:
		_003C_003Ec__DisplayClass23_0 _003C_003Ec__DisplayClass23_1 = default(_003C_003Ec__DisplayClass23_0);
		byte b = default(byte);
		_003C_003Ec__DisplayClass23_1.maxPodsPercentage = (int)(((uint)(-(b | 0x451F68B9)) % (uint)((int)((uint)b / (uint)num) + -1808221026)) ^ 0x26A7DAF3);
		uint num2 = ((264090638 % num > 1421013545 / num * (1025425967 >>> (1244768003 << (int)num))) ? 1u : 0u);
		goto IL_0044;
		IL_0044:
		object obj = default(object);
		ushort num3 = default(ushort);
		bool result = default(bool);
		ushort num4 = default(ushort);
		while (true)
		{
			switch ((uint)num % 6u)
			{
			default:
				num = (ushort)((((ushort)(num / -1164860115) & -449314627) >>> (int)num) + 40526);
				goto IL_007f;
			case 1u:
				break;
			case 2u:
				obj = _671BC22C.BF9F3D1F.FE8E0C9E[(-928045285 | b) / (int)(~(num * num2)) / ~(b % -995984729) - -23651](lua, "MAX_PODS");
				num3 = (ushort)(num * -2004315643);
				if ((num ^ 0x2A) != 0)
				{
					num = (ushort)((int)((uint)num3 % 1285556763u) - ((2143161377 / (int)num2) & 0x252350A5) - -622978374);
					continue;
				}
				goto IL_02b7;
			case 3u:
				num = (ushort)(-1643098213 / (0x66BDB41E & (-1206264793 / num3)) - -40526);
				if (obj == null)
				{
					goto IL_01c3;
				}
				_003C_003Ec__DisplayClass23_1.maxPodsPercentage = _671BC22C.BF9F3D1F.FE8E0C9E[(0 >>> (int)b) - -1225](obj);
				goto IL_0207;
			case 4u:
				num = (ushort)(5 / num3 + 40526);
				result = IsCharacterFull(_003C_003Ec__DisplayClass23_1.maxPodsPercentage, InstanceData);
				num3 = (ushort)((0x7E920EBD | ((b << 18) & (1932845317 / num4))) % ~((int)((uint)num / 360760369u) * (num3 / 1360810142)));
				if ((int)(num2 | ((uint)b / (uint)(~b) >> 28)) >> (int)(~(~num2)) == 0)
				{
					num = (ushort)((((uint)(477717427 << (int)num4) > (uint)num3) ? 1u : 0u) - 4294951638u);
					continue;
				}
				goto IL_007f;
			case 5u:
				{
					num = (ushort)((short)(((num2 < b) ? 1u : 0u) << 3) - -40526);
					goto IL_02b7;
				}
				IL_01c3:
				Log(new global::E5A6EF93<string>("warning.script.noMaxpods"), "gathering", (D32842AE._543E5320)(ushort)((num3 & b) << 12));
				goto IL_0207;
				IL_007f:
				_003C_003Ec__DisplayClass23_1 = new _003C_003Ec__DisplayClass23_0();
				_003C_003Ec__DisplayClass23_1._003C_003E4__this = this;
				b = (byte)((((43410 > num) ? 1u : 0u) & (uint)num) << 24);
				if (2898148910u >> ~(924874431 - b) != 0)
				{
					num = (ushort)((-(num % ~b) >> (int)num >> 1) - -35131);
					continue;
				}
				goto IL_01c3;
				IL_02b7:
				if (IsInTeam)
				{
					return TeamInstanceDataWithLeader.Any(_003C_003Ec__DisplayClass23_1._003CIsInventoryFull_003Eb__0);
				}
				return result;
				IL_0207:
				num4 = (ushort)(num + -845883254);
				num = (ushort)((0xF91 ^ ((int)num2 % -18458093)) - -43528);
				continue;
			}
			break;
		}
		num = (ushort)(3623919182u + (num - 1739546134u % (uint)(-877605577 << (int)b) << (int)num));
		goto IL_00ef;
	}

	private async Task<bool> GetMaxItemByGid(int gid)
	{
		_003C_003Ec__DisplayClass24_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass24_0();
		CS_0024_003C_003E8__locals2.gid = gid;
		if (InstanceData?.TempStorage == null || InstanceData.TempStorage.Count == 0)
		{
			return false;
		}
		Dictionary<int, JitsuriObjectItem> tempStorage = InstanceData.TempStorage;
		bool _770C9CB = false;
		JitsuriObjectItem maxItem;
		try
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[1348](tempStorage, ref _770C9CB);
			maxItem = (from i in InstanceData.TempStorage.Values
				where _671BC22C.BF9F3D1F.FE8E0C9E[1827](i) == CS_0024_003C_003E8__locals2.gid
				orderby _671BC22C.BF9F3D1F.FE8E0C9E[289](i) descending
				select i).FirstOrDefault();
		}
		finally
		{
			if (_770C9CB)
			{
				_671BC22C.BF9F3D1F.FE8E0C9E[576](tempStorage);
			}
		}
		if (maxItem == null)
		{
			return false;
		}
		long _4A0C = _671BC22C.BF9F3D1F.FE8E0C9E[1827](maxItem);
		int num = CC1A5C3D.E4A99F3F(_671BC22C.BF9F3D1F.FE8E0C9E[1454](ref _4A0C));
		int num2 = InstanceData.CharacterData.MaxPods - InstanceData.CharacterData.Pods;
		int _262BB78A = ((num > 0) ? (num2 / num) : _671BC22C.BF9F3D1F.FE8E0C9E[289](maxItem));
		int quantityToTake = _671BC22C.BF9F3D1F.FE8E0C9E[325](_671BC22C.BF9F3D1F.FE8E0C9E[289](maxItem), _262BB78A);
		if (quantityToTake <= 0)
		{
			return false;
		}
		int cD = -quantityToTake;
		bool flag = await InstanceData.NpcManager._94BEE8AE(_671BC22C.BF9F3D1F.FE8E0C9E[303](maxItem), cD);
		if (flag)
		{
			tempStorage = InstanceData.TempStorage;
			_770C9CB = false;
			try
			{
				_671BC22C.BF9F3D1F.FE8E0C9E[1348](tempStorage, ref _770C9CB);
				if (quantityToTake < _671BC22C.BF9F3D1F.FE8E0C9E[289](maxItem))
				{
					D297950D.B7B40E0C(maxItem, _7395F51B._19143A27(maxItem) - quantityToTake);
					InstanceData.TempStorage[_671BC22C.BF9F3D1F.FE8E0C9E[303](maxItem)] = maxItem;
				}
				else
				{
					InstanceData.TempStorage.Remove(_671BC22C.BF9F3D1F.FE8E0C9E[303](maxItem));
				}
			}
			finally
			{
				if (_770C9CB)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[576](tempStorage);
				}
			}
		}
		return flag;
	}

	private bool IsCharacterFull(int maxPodsValue, InstanceData instanceData)
	{
		if (instanceData.CharacterData.Pods == 0)
		{
			return false;
		}
		return (double)((float)instanceData.CharacterData.Pods / (float)instanceData.CharacterData.MaxPods) * 100.0 >= (double)maxPodsValue;
	}

	[AsyncStateMachine(typeof(_003CScriptBank_003Ed__26))]
	private Task<bool> ScriptBank(Lua lua)
	{
		_003CScriptBank_003Ed__26 stateMachine = default(_003CScriptBank_003Ed__26);
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
		stateMachine._003C_003E4__this = this;
		short num = -4419;
		num = (short)((-517942858 / (int)(~((39984 < num) ? 1u : 0u))) ^ 0x1EDF16C6);
		while (true)
		{
			switch ((uint)num % 4u)
			{
			default:
				num = (short)(-4420 ^ ((num << (((uint)(-1044900463 >> (int)num) < (uint)num) ? 1 : 0)) / num));
				stateMachine.lua = lua;
				num = (short)((int)(0 - (uint)num / (uint)num) / (int)num * (int)((uint)(num >> 14) % (uint)num));
				num = (short)((49 / ~((uint)num / (uint)(-num ^ -382288240))) ^ 0x2491);
				break;
			case 1u:
				num += -9361;
				stateMachine._003C_003E1__state = (((uint)(0 / ~num - num) > (uint)num) ? 1 : 0) - 1;
				num = (short)(((num ^ num) - num) | num);
				num = (short)((num << (num >>> 22) >> (num ^ -1859833160)) - -16806);
				break;
			case 2u:
				num = (short)(num >>> -(num ^ 0));
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				num = (short)((num << (int)num) | 0x11A65D00);
				num = (short)(-7529 + num);
				break;
			case 3u:
				num = (short)(1696287346 - num + -1696247259);
				return stateMachine._003C_003Et__builder.Task;
			}
		}
	}

	[AsyncStateMachine(typeof(_003CScriptMove_003Ed__27))]
	private Task<bool> ScriptMove(Lua lua)
	{
		uint num = 258925078u;
		num = (0x46 ^ num) + 506703424;
		_003CScriptMove_003Ed__27 stateMachine = default(_003CScriptMove_003Ed__27);
		ushort num2 = default(ushort);
		sbyte b = default(sbyte);
		byte b2 = default(byte);
		while (true)
		{
			switch (num % 5)
			{
			default:
				num = ((0u & ((-180914274 > (int)(1446536340 - num)) ? 1u : 0u)) | (uint)(((1115711030 < 3314601091u / num) ? 1 : 0) - (int)num)) - 3270413658u;
				stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<bool>.Create();
				num2 = (ushort)(((uint)((int)(num - num) + ((int)num >> 31)) | (num + 28568)) - ~num);
				num = (uint)(-618659311 ^ (-1760180949 << (num2 / (byte)num >> 25)));
				break;
			case 1u:
				num = (((int)(((-2024148785 < 563962684 >>> (int)num2) ? 1u : 0u) / (num >> (int)num2)) > 1865965875) ? 1u : 0u) + 258925078u;
				stateMachine._003C_003E4__this = this;
				b = (((int)num > (int)num) ? ((sbyte)1) : ((sbyte)0));
				num = (num & (uint)((int)((num & 0x9A3721B0u) >> (int)num) + (int)b)) - 1048149714;
				break;
			case 2u:
				num = (uint)(((int)((uint)((int)num2 / (int)num - (int)(num & 0x389F32B1)) % num) >> (int)num2) - -226444833);
				goto IL_0101;
			case 3u:
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				num = num;
				if (num2 - (num2 ^ b2) != 0)
				{
					num = (uint)(0x1A1ED0B & (b2 << 8)) % (uint)(~((int)num / -1268881695)) - 2726189668u;
					break;
				}
				goto IL_0101;
			case 4u:
				{
					num = (0x5003 & ((uint)b % 1362572855u)) - 4036042218u;
					return stateMachine._003C_003Et__builder.Task;
				}
				IL_0101:
				stateMachine.lua = lua;
				stateMachine._003C_003E1__state = (int)(num * ((uint)b / (num & 0x5934528)) - 1);
				b2 = (byte)((-1228102879 / ~((num2 >> 8) / 1628980135)) ^ 0x3921E0BD);
				break;
			}
		}
	}

	internal async Task<bool> ExecuteScriptWithTeam(string scriptString, List<InstanceData> instancesIds)
	{
		_ = 5;
		try
		{
			if (isRunning)
			{
				return false;
			}
			isRunning = true;
			global::E7BC18AE<bool, string> _2B922E = new global::E7BC18AE<bool, string>(isRunning, "gathering");
			_499DDEB5._3C11A432("ServiceState", _671BC22C.BF9F3D1F.FE8E0C9E[1563](_2B922E), InstanceData.ProcessId);
			if (InstanceData.PartyManager._8B821C87)
			{
				await InstanceData.PartyManager._663AC8B1();
			}
			TaskAwaiter _6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
			{
				await _6D28EB9F;
				TaskAwaiter taskAwaiter = default(TaskAwaiter);
				_6D28EB9F = taskAwaiter;
			}
			_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
			foreach (InstanceData instanceData in instancesIds)
			{
				if (!_671BC22C.BF9F3D1F.FE8E0C9E[720](instanceData.CharacterData.CharacterName, InstanceData.CharacterData.CharacterName))
				{
					await InstanceData.PartyManager.E30A31A6(instanceData.CharacterData.CharacterName);
					instanceData.ScriptManager.isRunning = true;
				}
			}
			TeamInstanceDataWithLeader = instancesIds;
			TeamInstanceDataWithLeader.Add(InstanceData);
			IsInTeam = true;
			foreach (InstanceData instanceData in instancesIds)
			{
				await instanceData.PartyManager.F122D311();
				await instanceData.PartyManager._2135FC00();
			}
			return await ExecuteScript(scriptString, forceEvenIfIsRunning: true);
		}
		catch
		{
			StopScript();
			return false;
		}
	}

	internal async Task<bool> ExecuteScript(string scriptString, bool forceEvenIfIsRunning = false)
	{
		bool result = true;
		if (!(await _598ED02C.A897F72F("can_useGathering")))
		{
			return false;
		}
		if (_671BC22C.BF9F3D1F.FE8E0C9E[744](scriptString))
		{
			Log(new global::E5A6EF93<string>("warning.script.noScript"), "gathering", D32842AE._543E5320._7C2AA78D);
			return false;
		}
		if (isRunning && !forceEvenIfIsRunning)
		{
			Log(new global::E5A6EF93<string>("error.script.alreadyRunning"), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		Lua lua = _671BC22C.BF9F3D1F.FE8E0C9E[839](_55A5C801: true);
		try
		{
			RunScript();
			int _631FBD2F = (int)InstanceData.MapInformation.MapId;
			string position = GetPosition();
			Log(new global::ED33D63D<string, global::_6494D781<string, int>>("info.script.startingPosition", new global::_6494D781<string, int>(position, _631FBD2F)), "gathering", D32842AE._543E5320._703EA0A0);
			InitializeLuaFunctions(lua);
			_671BC22C.BF9F3D1F.FE8E0C9E[468](lua, scriptString, "chunk");
			_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "bank");
			_671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "move");
			LuaFunction fightFunc = _671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "fight") as LuaFunction;
			bool wasFighting = false;
			bool wasDead = false;
			TaskAwaiter taskAwaiter = default(TaskAwaiter);
			while (result && isRunning)
			{
				global::E7BC18AE<bool, string> _2B922E = new global::E7BC18AE<bool, string>(isRunning, "gathering");
				_499DDEB5._3C11A432("ServiceState", _671BC22C.BF9F3D1F.FE8E0C9E[1563](_2B922E), InstanceData.ProcessId);
				TaskAwaiter _6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](10));
				if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
				{
					await _6D28EB9F;
					_6D28EB9F = taskAwaiter;
					taskAwaiter = default(TaskAwaiter);
				}
				_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
				if (InstanceData.CharacterData.IsFighting)
				{
					if (!wasFighting)
					{
						Log(new global::E5A6EF93<string>("info.script.inFight"), "gathering", D32842AE._543E5320._703EA0A0);
					}
					if (fightFunc != null)
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[1888](fightFunc, Array.Empty<object>());
					}
					else
					{
						_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000));
						if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
						{
							await _6D28EB9F;
							_6D28EB9F = taskAwaiter;
							taskAwaiter = default(TaskAwaiter);
						}
						_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					}
					wasFighting = true;
					continue;
				}
				if (wasFighting)
				{
					Log(new global::E5A6EF93<string>("info.script.fightEnded"), "gathering", D32842AE._543E5320._703EA0A0);
					_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](500));
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
					{
						await _6D28EB9F;
						_6D28EB9F = taskAwaiter;
						taskAwaiter = default(TaskAwaiter);
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					wasFighting = false;
				}
				if (InstanceData.CharacterData.CharacterLifeStatus != LifeStatus.AliveAndKicking)
				{
					if (!wasDead)
					{
						Log(new global::E5A6EF93<string>("info.script.isDead"), "gathering", D32842AE._543E5320.B3B830AD);
					}
					_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](1000));
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
					{
						await _6D28EB9F;
						_6D28EB9F = taskAwaiter;
						taskAwaiter = default(TaskAwaiter);
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					continue;
				}
				if (wasDead)
				{
					Log(new global::E5A6EF93<string>("info.script.resurrected"), "gathering", D32842AE._543E5320.B3B830AD);
					_6D28EB9F = _671BC22C.BF9F3D1F.FE8E0C9E[1015](_671BC22C.BF9F3D1F.FE8E0C9E[1440](3000));
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[1783](ref _6D28EB9F))
					{
						await _6D28EB9F;
						_6D28EB9F = taskAwaiter;
						taskAwaiter = default(TaskAwaiter);
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[1560](ref _6D28EB9F);
					wasDead = false;
				}
				if (IsInventoryFull(lua) && !craftingMode)
				{
					_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Yellow);
					DefaultInterpolatedStringHandler D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](33, 2);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "[INFO] Inventaire plein (Pods: ");
					D6A007B.AppendFormatted(InstanceData.CharacterData.Pods);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "/");
					D6A007B.AppendFormatted(InstanceData.CharacterData.MaxPods);
					_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, ")");
					_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
					object obj = _671BC22C.BF9F3D1F.FE8E0C9E[752](lua, "AUTO_DELETE");
					if (obj == null)
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[WARN] Le champ 'AUTO_DELETE' est introuvable dans le script Lua.");
					}
					else if (obj is LuaTable _2484EA)
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Yellow);
						_671BC22C.BF9F3D1F.FE8E0C9E[1676]("[INFO] AUTO_DELETE trouvé, traitement des objets à supprimer...");
						{
							IEnumerator enumerator = _671BC22C.BF9F3D1F.FE8E0C9E[98](_671BC22C.BF9F3D1F.FE8E0C9E[1291](_2484EA));
							try
							{
								while (_671BC22C.BF9F3D1F.FE8E0C9E[1381](enumerator))
								{
									object _83107EBE = _671BC22C.BF9F3D1F.FE8E0C9E[107](enumerator);
									_003C_003Ec__DisplayClass29_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass29_0();
									CS_0024_003C_003E8__locals5.gid = _671BC22C.BF9F3D1F.FE8E0C9E[1225](_83107EBE);
									_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Yellow);
									D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](55, 1);
									_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "\n[INFO] Début traitement de GID ");
									D6A007B.AppendFormatted(CS_0024_003C_003E8__locals5.gid);
									_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, " pour tous les membres…");
									_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
									_671BC22C.BF9F3D1F.FE8E0C9E[759]();
									if (IsInTeam)
									{
										if ((await Task.WhenAll(TeamInstanceDataWithLeader.Select((InstanceData memberData) => DeleteOneItemAsync(memberData, CS_0024_003C_003E8__locals5.gid)).ToArray())).Any((bool r) => !r))
										{
											_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
											D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](51, 1);
											_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "[WARN] Certaines suppressions ont échoué pour GID ");
											D6A007B.AppendFormatted(CS_0024_003C_003E8__locals5.gid);
											_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, ".");
											_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
											_671BC22C.BF9F3D1F.FE8E0C9E[759]();
										}
									}
									else
									{
										await DeleteOneItemAsync(InstanceData, CS_0024_003C_003E8__locals5.gid);
									}
								}
							}
							finally
							{
								IDisposable disposable = enumerator as IDisposable;
								if (disposable != null)
								{
									disposable.Dispose();
								}
							}
						}
					}
					else
					{
						_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
						D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](55, 1);
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "[ERREUR] 'AUTO_DELETE' n'est pas une LuaTable (type = ");
						D6A007B.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[869](obj));
						_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, ")");
						_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref D6A007B));
					}
					_671BC22C.BF9F3D1F.FE8E0C9E[759]();
					if (IsInventoryFull(lua) && !craftingMode)
					{
						result = await ScriptBank(lua) || InstanceData.CharacterData.IsFighting || InstanceData.CharacterData.CharacterLifeStatus != LifeStatus.AliveAndKicking;
					}
				}
				else
				{
					result = await ScriptMove(lua) || InstanceData.CharacterData.IsFighting || InstanceData.CharacterData.CharacterLifeStatus != LifeStatus.AliveAndKicking;
				}
			}
			Log(new global::E5A6EF93<string>("error.script.stopped"), "gathering", D32842AE._543E5320.B3B830AD);
		}
		catch (Exception ex)
		{
			Log(new global::E5A6EF93<string>(ex.ToString()), "gathering", D32842AE._543E5320.B3B830AD);
			return false;
		}
		finally
		{
			StopScript();
			global::E7BC18AE<bool, string> _2B922E2 = new global::E7BC18AE<bool, string>(isRunning, "gathering");
			_499DDEB5._3C11A432("ServiceState", _671BC22C.BF9F3D1F.FE8E0C9E[1563](_2B922E2), InstanceData.ProcessId);
			Log(new global::E5A6EF93<string>("error.script.stopped"), "gathering", D32842AE._543E5320.B3B830AD);
			InstanceData.NotificationDiscord.B78A5700();
		}
		return result;
	}

	internal string GetPosition()
	{
		sbyte b = 0;
		DefaultInterpolatedStringHandler CEA3933A = default(DefaultInterpolatedStringHandler);
		sbyte b2 = default(sbyte);
		ushort num = default(ushort);
		while (true)
		{
			switch ((uint)b % 5u)
			{
			default:
				_671BC22C.BF9F3D1F.FE8E0C9E[~(288572344 << (b >> (((uint)b < 1303172875u) ? 1 : 0))) + 288573509](ref CEA3933A, 1 ^ ((b >>> (int)b) / ~((((uint)(-1330097001 * b) > (uint)(b * 237205770)) ? 1 : 0) % -368076501)), (int)(((uint)(-b) % (uint)(~b)) ^ 2));
				b2 = ((b / ((0x1426221B | b) - 1814914057) < (int)(1779905073u % (uint)(~b))) ? ((sbyte)1) : ((sbyte)0));
				b = (sbyte)(((b2 & 0x2B3F12AD) + b >> b - ~b2) * b - -46);
				continue;
			case 1u:
				b = (sbyte)((0x1C675 | b) ^ 0x1C67F);
				goto IL_00b9;
			case 2u:
				b = (sbyte)((0x66AFD123 | (134954426 << (int)num)) % 77160536 - 5174651);
				_671BC22C.BF9F3D1F.FE8E0C9E[(957510550 >> (int)num) + -957509698](ref CEA3933A, ",");
				b = (sbyte)(-1173605341 % ~b / 683082687);
				if ((num ^ (b >>> (int)(3929871153u / (uint)(~num)))) + (b - -1147821429) != 0)
				{
					b = (sbyte)((1 - b) ^ 0x1D);
					continue;
				}
				break;
			case 3u:
				b = (sbyte)(-2026965229 + -2026965229 / ~((((uint)(b2 ^ 0x66244B23) > (uint)num) ? 1 : 0) / (int)((uint)b % 1998521366u)));
				CEA3933A.AppendFormatted(InstanceData.MapInformation.WorldMapY);
				if ((uint)((((-267931384 >> (int)b2) / b2) ^ b2) >> (int)((uint)num / (uint)(~num) >> 4) % -467495257) > (uint)(-1072592767 * ~b2))
				{
					b = (sbyte)(0x3BA4341F ^ ((int)(((uint)b2 % (uint)(~(num >>> (int)b2))) ^ 0x3BA4342F) - (int)b));
					continue;
				}
				goto IL_00b9;
			case 4u:
				{
					b = (sbyte)(((85368714 >> b % ~num) + (int)((uint)(-b2) % (uint)b2) * -2128035915) ^ 0x5169F8A);
					break;
				}
				IL_00b9:
				CEA3933A.AppendFormatted(InstanceData.MapInformation.WorldMapX);
				num = ((-192382906 == (int)(152002712u / (uint)b2)) ? ((ushort)1) : ((ushort)0));
				b = (sbyte)(42 + (num ^ (0 - (uint)num / 1024023973u - (byte)num)));
				continue;
			}
			break;
		}
		return _671BC22C.BF9F3D1F.FE8E0C9E[b + 934](ref CEA3933A);
	}

	internal _1E885D2B PathToDirection(string path)
	{
		uint num = 0u;
		if (((int)(~(1128091042 - num)) >> (int)num) * 1051952901 != 0)
		{
			goto IL_001a;
		}
		goto IL_0081;
		IL_001a:
		while (true)
		{
			switch (num % 3)
			{
			default:
				if (!_671BC22C.BF9F3D1F.FE8E0C9E[((((uint)(2008486153 % (int)(~num)) & (num % ~num)) | 0x24B7BF2C) * (num << (int)num - (int)num % (int)(~num))) ^ 0x2D0](path, "top"))
				{
					goto IL_006a;
				}
				num = (uint)(~(~((int)num % -1206573402)));
				return (_1E885D2B)(((int)(num ^ 0xD7975C1Eu) / ~((sbyte)num >> 23 >>> 19)) ^ 0x2868A3E2);
			case 1u:
				break;
			case 2u:
				num = (uint)(((ushort)num << (int)(~(0x9D & num))) ^ 0x4D973);
				if (!_671BC22C.BF9F3D1F.FE8E0C9E[720 + (495797553 % (int)(num & 0x1BBB7C86) >>> (int)(num | num)) % (int)num](path, "left"))
				{
					if (_671BC22C.BF9F3D1F.FE8E0C9E[((uint)((int)(num / 2584344885u << 2) >> 0) | num) - 27331](path, "right"))
					{
						num = ~((num > (uint)(-1189266171 * (int)num)) ? 1u : 0u) | num;
						return (_1E885D2B)((int)(0x10 & ~num) - -3);
					}
					num = ((num < (uint)(((num < 4147227562u) ? 1 : 0) / (int)num)) ? 1u : 0u);
					return (_1E885D2B)((-670173384 >>> ((int)num >> 1)) - -670173388);
				}
				num <<= (int)num;
				return (_1E885D2B)(2 + (uint)(-1708494201 >> (int)num) / ~(num ^ num));
			}
			break;
			IL_006a:
			num = (uint)(-28051 / (int)(~(1063728487 * (num >> 30))));
		}
		goto IL_0081;
		IL_0081:
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[(0x739F9E39 & num & (uint)(-215579590 / (sbyte)num)) - 2352](path, "bottom"))
		{
			num = num;
			num -= 97834743;
			goto IL_001a;
		}
		return (_1E885D2B)(num + 412252933 - 412280983);
	}

	[AsyncStateMachine(typeof(_003CRunScript_003Ed__32))]
	internal Task RunScript()
	{
		short num = -13138;
		if ((uint)(num | 0x4A95A420) >= (uint)((~num << 0) * 1536547119))
		{
			num = (short)((int)num + (((uint)num > 1209296315u) ? 1 : 0) - -17121);
			goto IL_002e;
		}
		goto IL_0085;
		IL_0085:
		_003CRunScript_003Ed__32 stateMachine = default(_003CRunScript_003Ed__32);
		do
		{
			stateMachine._003C_003E4__this = this;
		}
		while ((-234881024 | ~(-1599663605 - num)) << (0 - ((num < 679539596) ? 1 : 0)) / num == 0);
		num = (short)((num & -202) - -41443);
		goto IL_002e;
		IL_002e:
		short num2 = default(short);
		byte b = default(byte);
		while (true)
		{
			switch ((uint)num % 4u)
			{
			case 1u:
				num = (short)(-1476408146 ^ (-12941136 << (num >> (int)num)));
				goto IL_00da;
			case 2u:
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
				num2 = (short)(~((0x14884C33 | b) >>> (b & -482977876)) & 0x1D2E3F0B);
				if ((((uint)(1402992955 * num2) / 3667338499u) | (((uint)num2 > (uint)(sbyte)num) ? 1u : 0u)) - 0 <= (uint)((((((uint)num2 > 3232727970u) ? 1 : 0) - (int)num) & (sbyte)(-710377310 - num)) | -625871737))
				{
					num = (short)(-(-1022233040 ^ num2) + -1022228533);
					continue;
				}
				goto IL_00da;
			case 3u:
				{
					num = (short)(((-1346764402 / (int)(~((uint)num / 2545761727u))) & (((1645498122 / num) & num) << (0x22BF | num2))) - 13138);
					goto IL_01b7;
				}
				IL_01b7:
				return _671BC22C.BF9F3D1F.FE8E0C9E[-63502 + (ushort)(num >> (int)num2)](ref stateMachine._003C_003Et__builder);
				IL_00da:
				stateMachine._003C_003E1__state = 0xF9277F7 ^ (-261248936 & num);
				b = (byte)(0 - ((uint)(num >>> 25) / 272513042u + 1354369054));
				if (num >> ((num < num) ? 1 : 0) != 0)
				{
					continue;
				}
				goto IL_01b7;
			}
			break;
		}
		num = (short)(((918424740 % num % (int)(0x2232231B | (438781089u % (uint)num))) & num) + -14290);
		stateMachine._003C_003Et__builder = _671BC22C.BF9F3D1F.FE8E0C9E[(-382348772 ^ num) - 382343020]();
		goto IL_0085;
	}

	private static async Task<bool> DeleteOneItemAsync(InstanceData instanceData, int gid)
	{
		_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Yellow);
		DefaultInterpolatedStringHandler CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](26, 2);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[INFO] [");
		_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, instanceData.CharacterData.CharacterName);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Traitement GID ");
		CEA3933A.AppendFormatted(gid);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "…");
		_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
		JitsuriObjectItem item = instanceData.Inventory.GetObjectByGid(gid);
		if (item == null)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Gray);
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](26, 2);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[INFO] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, instanceData.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Aucun objet GID ");
			CEA3933A.AppendFormatted(gid);
			_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
			return true;
		}
		if (_671BC22C.BF9F3D1F.FE8E0C9E[289](item) <= 0)
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Gray);
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](40, 3);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[INFO] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, instanceData.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Quantité invalide (");
			CEA3933A.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[289](item));
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ") pour GID ");
			CEA3933A.AppendFormatted(gid);
			_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
			return true;
		}
		_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Cyan);
		CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](38, 4);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[ACTION] [");
		_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, instanceData.CharacterData.CharacterName);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Suppression GID ");
		CEA3933A.AppendFormatted(gid);
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, " (UID ");
		CEA3933A.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[303](item));
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ") ×");
		CEA3933A.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[289](item));
		_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "…");
		_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
		bool num = await instanceData.MessageHandler.DB02459F(_671BC22C.BF9F3D1F.FE8E0C9E[303](item), _671BC22C.BF9F3D1F.FE8E0C9E[289](item));
		_17348D39._53192AA0(num ? ConsoleColor.Green : ConsoleColor.Red);
		string bD85770F;
		if (!num)
		{
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](34, 2);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[ERREUR] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, instanceData.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Échec suppression GID ");
			CEA3933A.AppendFormatted(gid);
			bD85770F = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A);
		}
		else
		{
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](27, 3);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[SUCCÈS] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, instanceData.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] GID ");
			CEA3933A.AppendFormatted(gid);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, " supprimé ×");
			CEA3933A.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[289](item));
			bD85770F = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A);
		}
		_1E3B359C._6E3CC3BA(bD85770F);
		_671BC22C.BF9F3D1F.FE8E0C9E[759]();
		return num;
	}

	internal void StopScript(bool isStoppedManually = false)
	{
		ushort num = 26010;
		num = (ushort)((((num % num < num) ? 1 : 0) % (num & -97304316) << 1) - -42414);
		int num2 = default(int);
		global::E7BC18AE<bool, string> _2B922E = default(global::E7BC18AE<bool, string>);
		while (true)
		{
			CancellationTokenSource? globalCts;
			switch ((uint)num % 11u)
			{
			default:
				num = (ushort)(~(((((-1123435087 > num) ? 1u : 0u) < 3625300915u) ? 1 : 0) % (int)num) + 26012);
				goto IL_006d;
			case 1u:
				num = (ushort)(0x36C1744D ^ (0x36C111D7 | num2));
				TeamInstanceDataWithLeader.ForEach(delegate(InstanceData e)
				{
					e.ScriptManager.isRunning = false;
				});
				num2 = (((uint)(((1916064047 == num2) ? 1 : 0) % ~(num2 >>> num2)) > (uint)((num | 0x59186338) & 0x16A76791)) ? 1 : 0) & (-1537018514 % num);
				if (num2 << num2 % -22664018 != (-num2 >>> (int)num) - ~(num2 + num * num))
				{
					num = (ushort)(1248835565 + (-1248819700 ^ num));
					break;
				}
				goto IL_0073;
			case 2u:
				num = (ushort)(0x6598 ^ ((~(num2 & 0x2B2A09A) + (num >> 1)) / (num >> 2)));
				IsInTeam = (byte)(-79777 + (int)(2905485187u % ~(((0x78231D92 | num2) < (num2 ^ -1506894163)) ? 1u : 0u)) / (-1633211497 | -num)) != 0;
				num = (ushort)((-(-num) % (int)(~(((num2 | -1842708605) > -594211113) ? 1u : 0u))) & num2);
				if ((-904761937 & num) == 0)
				{
					num = (ushort)(-251110393 + (1329046710 >> (int)num) * -1532741497);
					break;
				}
				return;
			case 3u:
				num = (ushort)(num - -130117730 - 130119779);
				TeamInstanceDataWithLeader = new List<InstanceData>();
				num2 = (int)((uint)((num & num) * -2035791720) % 398117153u);
				if ((-1 | num) != 0)
				{
					num = (ushort)(-127500 / ~num2 - 99545);
					break;
				}
				goto IL_035b;
			case 4u:
				num = (ushort)(-130117115 + ((2081819825 >>> num2 >> 4) | num));
				isRunning = (byte)(1428637588 % ~(((num >>> num2) | (ushort)num2) % ~num)) != 0;
				num = (ushort)((uint)(num - -1182116348 + -623713289 >> (num2 >> 24)) % (uint)(~num2));
				num = (ushort)(0x7829 ^ num2);
				break;
			case 5u:
				num = (ushort)((uint)((898030723 % num >>> 18) - num) / (uint)(~(num2 & (num2 | num2))) + 36339);
				LastActionIndex = (int)(361843762u % (uint)(~num2) / 2534115903u - 1);
				num = (ushort)((byte)num * (byte)(num + -1053298242 >>> (num ^ 0x412D2890)));
				num = (ushort)(-14172 ^ (-11646 + num2));
				break;
			case 6u:
				num = (ushort)(num * -1500595148 - 944524854);
				_2B922E = new global::E7BC18AE<bool, string>(isRunning, "gathering");
				num2 = ~(-838413257 | num);
				if (num + 1865477166 - 6 != 0)
				{
					num = (ushort)((-207912132 - num) ^ -207957373);
					break;
				}
				return;
			case 7u:
				num = (ushort)((((uint)((short)num2 / (-734567017 << (int)num)) > (uint)(-1975048745 - num2)) ? 1 : 0) * -1569108399 - -59778);
				goto IL_035b;
			case 8u:
			{
				num = (ushort)(~((uint)(-1097705588 >> (int)num) / 67134593u) ^ 0xFFFF1642u);
				TaskCompletionSource<bool> globalCancelTcs = InstanceData.GlobalCancelTcs;
				if (globalCancelTcs == null)
				{
					if ((uint)(num + -173974499) >= (uint)(-100198240 >>> ~num2))
					{
						num = (ushort)(0x13683CD7 ^ (1177166245 % ((num & num) - 851593540) - (((int)((uint)(0x69A3808C ^ num) % 3258157981u) > 182) ? 1 : 0)));
						break;
					}
					goto IL_035b;
				}
				globalCancelTcs.TrySetResult((byte)(1706376108 - num2 + -867973475) != 0);
				if (~(num - (uint)num / (uint)num >> 29) != 0)
				{
					num = (ushort)(1174745008 + (num2 | -2003066983));
					break;
				}
				goto IL_0392;
			}
			case 9u:
				num = (ushort)((((uint)(-num2) / (uint)num2) | (2010440664u % (uint)(~num) - num)) ^ 0x77D30F25);
				return;
			case 10u:
				{
					num = (ushort)((-1330619851 ^ num2) - -2125959429);
					return;
				}
				IL_006d:
				if (isStoppedManually)
				{
					goto IL_0073;
				}
				goto IL_0097;
				IL_0097:
				num2 = (int)((((uint)(-759899723 % num) < 1854384770u) ? 1u : 0u) / (uint)num) >> ((num == num) ? 1 : 0);
				if ((uint)(num >>> num2) > (uint)num2)
				{
					num = (ushort)(((0x55226BBF ^ num2) - num2) ^ 0x552291B5);
					break;
				}
				goto IL_006d;
				IL_0073:
				Log(new global::E5A6EF93<string>("script arrete manuellement"), "gathering", (D32842AE._543E5320)(-(num - -992146836) ^ -992172846));
				goto IL_0097;
				IL_035b:
				_499DDEB5._3C11A432("ServiceState", _671BC22C.BF9F3D1F.FE8E0C9E[0x48B4BC ^ (1219667774 >>> num2)](_2B922E), InstanceData.ProcessId);
				goto IL_0392;
				IL_0392:
				globalCts = InstanceData._globalCts;
				if (globalCts != null)
				{
					FE36D684.D110FE2A(globalCts);
				}
				if (num - 651723752 != num2)
				{
					num = (ushort)(774890421 + num2 * 1410316729);
					break;
				}
				goto IL_0097;
			}
		}
	}

	private static async Task<bool> TravelToAsync(InstanceData ctx, int x, int y)
	{
		MapIdCoordinates coords = _82210236.E7ADD8BD(x, y);
		DefaultInterpolatedStringHandler CEA3933A;
		if (!(await ctx.GoToNearestZaap(coords)))
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](42, 3);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[ERREUR] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, ctx.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Échec GoToNearestZaap vers (");
			CEA3933A.AppendFormatted(x);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ",");
			CEA3933A.AppendFormatted(y);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ")");
			_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
			_671BC22C.BF9F3D1F.FE8E0C9E[759]();
			return false;
		}
		bool num = await ctx.MovePlayerToWorldMapId(coords.MapId);
		_17348D39._53192AA0(num ? ConsoleColor.Green : ConsoleColor.Red);
		string bD85770F;
		if (!num)
		{
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](42, 2);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[ERREUR] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, ctx.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Échec MovePlayerToWorldMapId(");
			CEA3933A.AppendFormatted(coords.MapId);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ")");
			bD85770F = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A);
		}
		else
		{
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](37, 4);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[SUCCÈS] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, ctx.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Arrivé en (");
			CEA3933A.AppendFormatted(x);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ",");
			CEA3933A.AppendFormatted(y);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ") sur la map ");
			CEA3933A.AppendFormatted(coords.MapId);
			bD85770F = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A);
		}
		_1E3B359C._6E3CC3BA(bD85770F);
		_671BC22C.BF9F3D1F.FE8E0C9E[759]();
		return num;
	}

	private static async Task<bool> TravelToAsync(InstanceData ctx, int mapId)
	{
		MapIdCoordinates coords = _82210236.AB91771D(mapId, B31AE737: true);
		DefaultInterpolatedStringHandler CEA3933A;
		if (!(await ctx.GoToNearestZaap(coords)))
		{
			_671BC22C.BF9F3D1F.FE8E0C9E[197](ConsoleColor.Red);
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](54, 4);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[ERREUR] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, ctx.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Échec GoToNearestZaap vers (");
			CEA3933A.AppendFormatted(coords.WorldX);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ",");
			CEA3933A.AppendFormatted(coords.WorldY);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ") sur la map ");
			CEA3933A.AppendFormatted(coords.MapId);
			_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A));
			_671BC22C.BF9F3D1F.FE8E0C9E[759]();
			return false;
		}
		bool num = await ctx.MovePlayerToWorldMapId(coords.MapId);
		_17348D39._53192AA0(num ? ConsoleColor.Green : ConsoleColor.Red);
		string bD85770F;
		if (!num)
		{
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](42, 2);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[ERREUR] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, ctx.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Échec MovePlayerToWorldMapId(");
			CEA3933A.AppendFormatted(coords.MapId);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ")");
			bD85770F = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A);
		}
		else
		{
			CEA3933A = _671BC22C.BF9F3D1F.FE8E0C9E[1165](37, 4);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "[SUCCÈS] [");
			_671BC22C.BF9F3D1F.FE8E0C9E[311](ref CEA3933A, ctx.CharacterData.CharacterName);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, "] Arrivé en (");
			CEA3933A.AppendFormatted(coords.WorldX);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ",");
			CEA3933A.AppendFormatted(coords.WorldY);
			_671BC22C.BF9F3D1F.FE8E0C9E[852](ref CEA3933A, ") sur la map ");
			CEA3933A.AppendFormatted(coords.MapId);
			bD85770F = _671BC22C.BF9F3D1F.FE8E0C9E[934](ref CEA3933A);
		}
		_1E3B359C._6E3CC3BA(bD85770F);
		_671BC22C.BF9F3D1F.FE8E0C9E[759]();
		return num;
	}

	private void Log(object message, string category, D32842AE._543E5320 level)
	{
		uint num = 673478862u;
		string text2 = default(string);
		ushort num2 = default(ushort);
		string aEB611B = default(string);
		while (true)
		{
			string text;
			switch (num % 3)
			{
			default:
				text2 = message as string;
				goto IL_0021;
			case 1u:
				num = (uint)(-2118050210 + num2 + -1503438224);
				text = text2;
				goto IL_0095;
			case 2u:
				{
					num = (uint)((int)(17883660 % num) + ((int)num + (((int)num - -2079108848) ^ -1945675518)) + -1346835594);
					D32842AE._0700BABF(category, aEB611B, InstanceData.ProcessId, level);
					return;
				}
				IL_0021:
				while (text2 != null)
				{
					num2 = (ushort)(1697386036 * ((int)num % ~(-586413567 % ((int)num / 496413734))));
					if (628334401 / (int)(num ^ num2) != 0)
					{
						continue;
					}
					goto IL_006d;
				}
				text = _671BC22C.BF9F3D1F.FE8E0C9E[(num & 0xB99DBD87u) - 671363691](message);
				goto IL_0095;
				IL_006d:
				num = (uint)(((byte)num >> (int)(3641412135u % num)) + -776939839);
				break;
				IL_0095:
				aEB611B = text;
				if ((num ^ 0xA348B453u) > ((num >> 10) | (uint)((int)((uint)((int)num % -668976710) / num) % 621350062)))
				{
					num = (uint)((int)(((394739976 == num) ? 1u : 0u) | num) / (int)(~(514503993 / (num ^ 0xD08237A4u))) - -1742279549);
					break;
				}
				goto IL_0021;
			}
		}
	}
}
