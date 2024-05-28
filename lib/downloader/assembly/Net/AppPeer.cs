// Decompiled with JetBrains decompiler
// Type: Net.AppPeer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using GameCore.Serialization;
using GameCore.Stat;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace Net
{
  public class AppPeer : Peer
  {
    private IEnumerator ReceiveUntilF(
      int timeoutMilliseconds,
      Func<AppPeer.ReceivedMessage, bool> pred,
      Promise<AppPeer.ReceivedMessage> promise)
    {
      Future<AppPeer.ReceivedMessage> resultF;
      while (true)
      {
        resultF = this.ReceiveType(timeoutMilliseconds);
        IEnumerator e = resultF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!pred(resultF.Result))
          resultF = (Future<AppPeer.ReceivedMessage>) null;
        else
          break;
      }
      promise.Result = resultF.Result;
    }

    public Future<AppPeer.ReceivedMessage> ReceiveUntil(
      int timeoutMilliseconds,
      Func<AppPeer.ReceivedMessage, bool> pred)
    {
      return new Future<AppPeer.ReceivedMessage>((Func<Promise<AppPeer.ReceivedMessage>, IEnumerator>) (promise => this.ReceiveUntilF(timeoutMilliseconds, pred, promise)));
    }

    public Future<AppPeer.ReceivedMessage> ReceiveOps(params AppServerOperation[] ops)
    {
      return this.ReceiveUntil(-1, (Func<AppPeer.ReceivedMessage, bool>) (r => ((IEnumerable<AppServerOperation>) ops).Any<AppServerOperation>((Func<AppServerOperation, bool>) (c => r.Error != null || c == (AppServerOperation) r.Header.Type))));
    }

    public Future<AppPeer.ReceivedMessage> ReceiveOps(
      int timeoutMilliseconds,
      params AppServerOperation[] ops)
    {
      return this.ReceiveUntil(timeoutMilliseconds, (Func<AppPeer.ReceivedMessage, bool>) (r => ((IEnumerable<AppServerOperation>) ops).Any<AppServerOperation>((Func<AppServerOperation, bool>) (c => r.Error != null || c == (AppServerOperation) r.Header.Type))));
    }

    private IEnumerator ReceiveTypeE(
      int timeoutMilliseconds,
      Promise<AppPeer.ReceivedMessage> promise)
    {
      AppPeer appPeer = this;
      Future<Tuple<Peer.CommonHeader, byte[]>> receiveF = appPeer.Receive(timeoutMilliseconds);
      IEnumerator e = receiveF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (receiveF.Exception != null)
      {
        promise.Result = new AppPeer.ReceivedMessage()
        {
          Error = receiveF.Exception
        };
      }
      else
      {
        Tuple<Peer.CommonHeader, byte[]> result = receiveF.Result;
        int type = result.Item1.Type;
        AssocList<string, object> assocList = (AssocList<string, object>) appPeer.Serializer.DeserializeFromMemory(result.Item2);
        AppPeer.ReceivedMessage receivedMessage = new AppPeer.ReceivedMessage();
        receivedMessage.Header = result.Item1;
        switch (type)
        {
          case 0:
            receivedMessage.Stat = new AppPeer.Stat()
            {
              stat = (GameServerStat) assocList["stat"]
            };
            break;
          case 1:
            receivedMessage.Ready = new AppPeer.Ready()
            {
              buffer = (byte[]) assocList["buffer"],
              order = (int) assocList["order"]
            };
            break;
          case 2:
            receivedMessage.LocateUnits = new AppPeer.LocateUnits()
            {
              locationTimeoutMilliseconds = (int) assocList["locationTimeoutMilliseconds"]
            };
            break;
          case 3:
            receivedMessage.GameInitialize = new AppPeer.GameInitialize()
            {
              unitPositions = (Tuple<int, int, int>[]) assocList["unitPositions"]
            };
            break;
          case 4:
            receivedMessage.TurnInitialize = new AppPeer.TurnInitialize()
            {
              remainTurn = (int) assocList["remainTurn"],
              respawnUnitPositions = (Tuple<int, int, int>[]) assocList["respawnUnitPositions"],
              points = (int[]) assocList["points"],
              deadUnitRespawnCounts = (Tuple<int, int>[]) assocList["deadUnitRespawnCounts"],
              callSkillPoints = (Decimal[]) assocList["callSkillPoints"],
              random = (XorShift) assocList["random"]
            };
            break;
          case 5:
            receivedMessage.MoveUnitRequest = new AppPeer.MoveUnitRequest()
            {
              order = (int) assocList["order"],
              moveTimeoutMilliseconds = (int) assocList["moveTimeoutMilliseconds"]
            };
            break;
          case 6:
            receivedMessage.ActionUnit = new AppPeer.ActionUnit()
            {
              aiUnit = (BL.AIUnitNetwork) assocList["aiUnit"],
              points = (int[]) assocList["points"],
              deadUnitRespawnCounts = (Tuple<int, int>[]) assocList["deadUnitRespawnCounts"]
            };
            break;
          case 7:
            receivedMessage.WipedOut = new AppPeer.WipedOut()
            {
              isWipedOuts = (bool[]) assocList["isWipedOuts"],
              points = (int[]) assocList["points"]
            };
            break;
          case 8:
            receivedMessage.FinishBattle = new AppPeer.FinishBattle()
            {
              victoryEffects = (PvpVictoryEffectEnum[]) assocList["victoryEffects"]
            };
            break;
          case 9:
            receivedMessage.Recovery = new AppPeer.Recovery()
            {
              recovery = (RecoveryType) assocList["recovery"]
            };
            break;
          case 10:
            receivedMessage.NoRoom = new AppPeer.NoRoom();
            break;
          case 11:
            receivedMessage.ActionCallSkill = new AppPeer.ActionCallSkill()
            {
              battleCallSkillResult = (BL.BattleCallSkillResultNetwork) assocList["battleCallSkillResult"],
              points = (int[]) assocList["points"],
              deadUnitRespawnCounts = (Tuple<int, int>[]) assocList["deadUnitRespawnCounts"],
              callSkillPoints = (Decimal[]) assocList["callSkillPoints"]
            };
            break;
          default:
            receivedMessage.Error = new Exception("invalid type");
            break;
        }
        promise.Result = receivedMessage;
      }
    }

    public Future<AppPeer.ReceivedMessage> ReceiveType(int timeoutMilliseconds = -1)
    {
      return new Future<AppPeer.ReceivedMessage>((Func<Promise<AppPeer.ReceivedMessage>, IEnumerator>) (promise => this.ReceiveTypeE(timeoutMilliseconds, promise)));
    }

    private IEnumerator SendNothrowE(Func<Future<int>> send, Promise<AppPeer.SendMessage> promise)
    {
      Future<int> f = send();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (f.Exception != null)
        promise.Result = new AppPeer.SendMessage()
        {
          Error = f.Exception
        };
      else
        promise.Result = new AppPeer.SendMessage()
        {
          SendId = f.Result
        };
    }

    public Future<AppPeer.SendMessage> SendNothrow(Func<Future<int>> send)
    {
      return new Future<AppPeer.SendMessage>((Func<Promise<AppPeer.SendMessage>, IEnumerator>) (promise => this.SendNothrowE(send, promise)));
    }

    public Future<AppPeer.SendMessage> StatRequest()
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[0], new object[0], 0);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(0, args, 5000)));
    }

    public Future<AppPeer.SendMessage> JoinRoom(string battleToken)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[1]
      {
        nameof (battleToken)
      }, new object[1]{ (object) battleToken }, 1);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(1, args, 5000)));
    }

    public Future<AppPeer.SendMessage> ReadyCompleted()
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[0], new object[0], 0);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(2, args, 5000)));
    }

    public Future<AppPeer.SendMessage> LocateUnitsCompleted(Tuple<int, int, int>[] unitPositions)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[1]
      {
        nameof (unitPositions)
      }, new object[1]{ (object) unitPositions }, 1);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(3, args, 5000)));
    }

    public Future<AppPeer.SendMessage> TurnInitializeCompleted()
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[0], new object[0], 0);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(4, args, 5000)));
    }

    public Future<AppPeer.SendMessage> MoveUnitTimeout(int? currentUnitPositionId)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[1]
      {
        nameof (currentUnitPositionId)
      }, new object[1]{ (object) currentUnitPositionId }, 1);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(5, args, 5000)));
    }

    public Future<AppPeer.SendMessage> MoveUnit(int unitPositionId, int row, int column)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[3]
      {
        nameof (column),
        nameof (row),
        nameof (unitPositionId)
      }, new object[3]
      {
        (object) column,
        (object) row,
        (object) unitPositionId
      }, 3);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(6, args, 5000)));
    }

    public Future<AppPeer.SendMessage> MoveUnitWithAttack(
      int unitPositionId,
      int row,
      int column,
      int targetUnitPositionId,
      bool isHeal,
      int attackStatusIndex)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[6]
      {
        nameof (attackStatusIndex),
        nameof (column),
        nameof (isHeal),
        nameof (row),
        nameof (targetUnitPositionId),
        nameof (unitPositionId)
      }, new object[6]
      {
        (object) attackStatusIndex,
        (object) column,
        (object) isHeal,
        (object) row,
        (object) targetUnitPositionId,
        (object) unitPositionId
      }, 6);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(7, args, 5000)));
    }

    public Future<AppPeer.SendMessage> MoveUnitWithSkill(
      int unitPositionId,
      int row,
      int column,
      int[] targetUnitPositionIds,
      int skillId,
      int[] targetPanelRows,
      int[] targetPanelColumns)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[7]
      {
        nameof (column),
        nameof (row),
        nameof (skillId),
        nameof (targetPanelColumns),
        nameof (targetPanelRows),
        nameof (targetUnitPositionIds),
        nameof (unitPositionId)
      }, new object[7]
      {
        (object) column,
        (object) row,
        (object) skillId,
        (object) targetPanelColumns,
        (object) targetPanelRows,
        (object) targetUnitPositionIds,
        (object) unitPositionId
      }, 7);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(8, args, 5000)));
    }

    public Future<AppPeer.SendMessage> ActionUnitCompleted()
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[0], new object[0], 0);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(9, args, 5000)));
    }

    public Future<AppPeer.SendMessage> WipedOutCompleted()
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[0], new object[0], 0);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(10, args, 5000)));
    }

    public Future<AppPeer.SendMessage> RecoveryRequest(string battleToken)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[1]
      {
        nameof (battleToken)
      }, new object[1]{ (object) battleToken }, 1);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(11, args, 5000)));
    }

    public Future<AppPeer.SendMessage> AutoOnRequest(bool canCallSkillAuto)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[1]
      {
        nameof (canCallSkillAuto)
      }, new object[1]{ (object) canCallSkillAuto }, 1);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(12, args, 5000)));
    }

    public Future<AppPeer.SendMessage> UseCallSkill(int[] targetUnitPositionIds, int skillId)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[2]
      {
        nameof (skillId),
        nameof (targetUnitPositionIds)
      }, new object[2]
      {
        (object) skillId,
        (object) targetUnitPositionIds
      }, 2);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(13, args, 5000)));
    }

    [Serializable]
    public class Stat
    {
      public GameServerStat stat;

      public override string ToString()
      {
        return "Stat(" + string.Format("stat={0}, ", this.stat != null ? (object) this.stat.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["stat"] = (object) this.stat
        };
      }
    }

    [Serializable]
    public class Ready
    {
      public byte[] buffer;
      public int order;

      public override string ToString()
      {
        string str1 = "Ready(" + string.Format("buffer={0}, ", this.buffer != null ? (object) this.buffer.ToString() : (object) "[null]");
        int order = this.order;
        string str2 = string.Format("order={0}, ", (object) this.order.ToString());
        return str1 + str2 + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["buffer"] = (object) this.buffer,
          ["order"] = (object) this.order
        };
      }
    }

    [Serializable]
    public class LocateUnits
    {
      public int locationTimeoutMilliseconds;

      public override string ToString()
      {
        int timeoutMilliseconds = this.locationTimeoutMilliseconds;
        return "LocateUnits(" + string.Format("locationTimeoutMilliseconds={0}, ", (object) this.locationTimeoutMilliseconds.ToString()) + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["locationTimeoutMilliseconds"] = (object) this.locationTimeoutMilliseconds
        };
      }
    }

    [Serializable]
    public class GameInitialize
    {
      public Tuple<int, int, int>[] unitPositions;

      public override string ToString()
      {
        return "GameInitialize(" + string.Format("unitPositions={0}, ", this.unitPositions != null ? (object) this.unitPositions.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["unitPositions"] = (object) this.unitPositions
        };
      }
    }

    [Serializable]
    public class TurnInitialize
    {
      public int remainTurn;
      public Tuple<int, int, int>[] respawnUnitPositions;
      public int[] points;
      public Tuple<int, int>[] deadUnitRespawnCounts;
      public Decimal[] callSkillPoints;
      public XorShift random;

      public override string ToString()
      {
        int remainTurn = this.remainTurn;
        return "TurnInitialize(" + string.Format("remainTurn={0}, ", (object) this.remainTurn.ToString()) + string.Format("respawnUnitPositions={0}, ", this.respawnUnitPositions != null ? (object) this.respawnUnitPositions.ToString() : (object) "[null]") + string.Format("points={0}, ", this.points != null ? (object) this.points.ToString() : (object) "[null]") + string.Format("deadUnitRespawnCounts={0}, ", this.deadUnitRespawnCounts != null ? (object) this.deadUnitRespawnCounts.ToString() : (object) "[null]") + string.Format("callSkillPoints={0}, ", this.callSkillPoints != null ? (object) this.callSkillPoints.ToString() : (object) "[null]") + string.Format("random={0}, ", this.random != null ? (object) this.random.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["remainTurn"] = (object) this.remainTurn,
          ["respawnUnitPositions"] = (object) this.respawnUnitPositions,
          ["points"] = (object) this.points,
          ["deadUnitRespawnCounts"] = (object) this.deadUnitRespawnCounts,
          ["callSkillPoints"] = (object) this.callSkillPoints
        };
      }
    }

    [Serializable]
    public class MoveUnitRequest
    {
      public int order;
      public int moveTimeoutMilliseconds;

      public override string ToString()
      {
        int order = this.order;
        string str1 = "MoveUnitRequest(" + string.Format("order={0}, ", (object) this.order.ToString());
        int timeoutMilliseconds = this.moveTimeoutMilliseconds;
        string str2 = string.Format("moveTimeoutMilliseconds={0}, ", (object) this.moveTimeoutMilliseconds.ToString());
        return str1 + str2 + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["order"] = (object) this.order,
          ["moveTimeoutMilliseconds"] = (object) this.moveTimeoutMilliseconds
        };
      }
    }

    [Serializable]
    public class ActionUnit
    {
      public BL.AIUnitNetwork aiUnit;
      public int[] points;
      public Tuple<int, int>[] deadUnitRespawnCounts;

      public override string ToString()
      {
        return "ActionUnit(" + string.Format("aiUnit={0}, ", this.aiUnit != null ? (object) this.aiUnit.ToString() : (object) "[null]") + string.Format("points={0}, ", this.points != null ? (object) this.points.ToString() : (object) "[null]") + string.Format("deadUnitRespawnCounts={0}, ", this.deadUnitRespawnCounts != null ? (object) this.deadUnitRespawnCounts.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["aiUnit"] = (object) this.aiUnit,
          ["points"] = (object) this.points,
          ["deadUnitRespawnCounts"] = (object) this.deadUnitRespawnCounts
        };
      }
    }

    [Serializable]
    public class WipedOut
    {
      public bool[] isWipedOuts;
      public int[] points;

      public override string ToString()
      {
        return "WipedOut(" + string.Format("isWipedOuts={0}, ", this.isWipedOuts != null ? (object) this.isWipedOuts.ToString() : (object) "[null]") + string.Format("points={0}, ", this.points != null ? (object) this.points.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["isWipedOuts"] = (object) this.isWipedOuts,
          ["points"] = (object) this.points
        };
      }
    }

    [Serializable]
    public class FinishBattle
    {
      public PvpVictoryEffectEnum[] victoryEffects;

      public override string ToString()
      {
        return "FinishBattle(" + string.Format("victoryEffects={0}, ", this.victoryEffects != null ? (object) this.victoryEffects.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["victoryEffects"] = (object) this.victoryEffects
        };
      }
    }

    [Serializable]
    public class Recovery
    {
      public RecoveryType recovery;

      public override string ToString()
      {
        return "Recovery(" + string.Format("recovery={0}, ", this.recovery != null ? (object) this.recovery.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["recovery"] = (object) this.recovery
        };
      }
    }

    [Serializable]
    public class NoRoom
    {
      public override string ToString() => "NoRoom(" + ")";

      public Dictionary<string, object> ToJson() => new Dictionary<string, object>();
    }

    [Serializable]
    public class ActionCallSkill
    {
      public BL.BattleCallSkillResultNetwork battleCallSkillResult;
      public int[] points;
      public Tuple<int, int>[] deadUnitRespawnCounts;
      public Decimal[] callSkillPoints;

      public override string ToString()
      {
        return "ActionCallSkill(" + string.Format("battleCallSkillResult={0}, ", this.battleCallSkillResult != null ? (object) this.battleCallSkillResult.ToString() : (object) "[null]") + string.Format("points={0}, ", this.points != null ? (object) this.points.ToString() : (object) "[null]") + string.Format("deadUnitRespawnCounts={0}, ", this.deadUnitRespawnCounts != null ? (object) this.deadUnitRespawnCounts.ToString() : (object) "[null]") + string.Format("callSkillPoints={0}, ", this.callSkillPoints != null ? (object) this.callSkillPoints.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["battleCallSkillResult"] = (object) this.battleCallSkillResult,
          ["points"] = (object) this.points,
          ["deadUnitRespawnCounts"] = (object) this.deadUnitRespawnCounts,
          ["callSkillPoints"] = (object) this.callSkillPoints
        };
      }
    }

    [Serializable]
    public class ReceivedMessage
    {
      public Peer.CommonHeader Header;
      public Exception Error;
      public AppPeer.Stat Stat;
      public AppPeer.Ready Ready;
      public AppPeer.LocateUnits LocateUnits;
      public AppPeer.GameInitialize GameInitialize;
      public AppPeer.TurnInitialize TurnInitialize;
      public AppPeer.MoveUnitRequest MoveUnitRequest;
      public AppPeer.ActionUnit ActionUnit;
      public AppPeer.WipedOut WipedOut;
      public AppPeer.FinishBattle FinishBattle;
      public AppPeer.Recovery Recovery;
      public AppPeer.NoRoom NoRoom;
      public AppPeer.ActionCallSkill ActionCallSkill;

      public override string ToString()
      {
        string str = string.Format("ReceivedMessage(Header={0}, ", (object) this.Header);
        if (this.Stat != null)
          str += this.Stat.ToString();
        if (this.Ready != null)
          str += this.Ready.ToString();
        if (this.LocateUnits != null)
          str += this.LocateUnits.ToString();
        if (this.GameInitialize != null)
          str += this.GameInitialize.ToString();
        if (this.TurnInitialize != null)
          str += this.TurnInitialize.ToString();
        if (this.MoveUnitRequest != null)
          str += this.MoveUnitRequest.ToString();
        if (this.ActionUnit != null)
          str += this.ActionUnit.ToString();
        if (this.WipedOut != null)
          str += this.WipedOut.ToString();
        if (this.FinishBattle != null)
          str += this.FinishBattle.ToString();
        if (this.Recovery != null)
          str += this.Recovery.ToString();
        if (this.NoRoom != null)
          str += this.NoRoom.ToString();
        if (this.ActionCallSkill != null)
          str += this.ActionCallSkill.ToString();
        return str + ")";
      }

      public void ThrowIfError()
      {
        if (this.Error != null)
          throw this.Error;
      }
    }

    [Serializable]
    public class SendMessage
    {
      public int SendId;
      public Exception Error;

      public override string ToString()
      {
        return string.Format("SendMessage(SendId={0}, Error={1})", (object) this.SendId, (object) this.Error);
      }

      public void ThrowIfError()
      {
        if (this.Error != null)
          throw this.Error;
      }
    }
  }
}
