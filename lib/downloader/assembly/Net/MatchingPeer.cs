// Decompiled with JetBrains decompiler
// Type: Net.MatchingPeer
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
  public class MatchingPeer : Peer
  {
    private IEnumerator ReceiveUntilF(
      int timeoutMilliseconds,
      Func<MatchingPeer.ReceivedMessage, bool> pred,
      Promise<MatchingPeer.ReceivedMessage> promise)
    {
      Future<MatchingPeer.ReceivedMessage> resultF;
      while (true)
      {
        resultF = this.ReceiveType(timeoutMilliseconds);
        IEnumerator e = resultF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!pred(resultF.Result))
          resultF = (Future<MatchingPeer.ReceivedMessage>) null;
        else
          break;
      }
      promise.Result = resultF.Result;
    }

    public Future<MatchingPeer.ReceivedMessage> ReceiveUntil(
      int timeoutMilliseconds,
      Func<MatchingPeer.ReceivedMessage, bool> pred)
    {
      return new Future<MatchingPeer.ReceivedMessage>((Func<Promise<MatchingPeer.ReceivedMessage>, IEnumerator>) (promise => this.ReceiveUntilF(timeoutMilliseconds, pred, promise)));
    }

    public Future<MatchingPeer.ReceivedMessage> ReceiveOps(params MatchingServerOperation[] ops)
    {
      return this.ReceiveUntil(-1, (Func<MatchingPeer.ReceivedMessage, bool>) (r => ((IEnumerable<MatchingServerOperation>) ops).Any<MatchingServerOperation>((Func<MatchingServerOperation, bool>) (c => r.Error != null || c == (MatchingServerOperation) r.Header.Type))));
    }

    public Future<MatchingPeer.ReceivedMessage> ReceiveOps(
      int timeoutMilliseconds,
      params MatchingServerOperation[] ops)
    {
      return this.ReceiveUntil(timeoutMilliseconds, (Func<MatchingPeer.ReceivedMessage, bool>) (r => ((IEnumerable<MatchingServerOperation>) ops).Any<MatchingServerOperation>((Func<MatchingServerOperation, bool>) (c => r.Error != null || c == (MatchingServerOperation) r.Header.Type))));
    }

    private IEnumerator ReceiveTypeE(
      int timeoutMilliseconds,
      Promise<MatchingPeer.ReceivedMessage> promise)
    {
      MatchingPeer matchingPeer = this;
      Future<Tuple<Peer.CommonHeader, byte[]>> receiveF = matchingPeer.Receive(timeoutMilliseconds);
      IEnumerator e = receiveF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (receiveF.Exception != null)
      {
        promise.Result = new MatchingPeer.ReceivedMessage()
        {
          Error = receiveF.Exception
        };
      }
      else
      {
        Tuple<Peer.CommonHeader, byte[]> result = receiveF.Result;
        int type = result.Item1.Type;
        AssocList<string, object> assocList = (AssocList<string, object>) matchingPeer.Serializer.DeserializeFromMemory(result.Item2);
        MatchingPeer.ReceivedMessage receivedMessage = new MatchingPeer.ReceivedMessage();
        receivedMessage.Header = result.Item1;
        switch (type)
        {
          case 0:
            receivedMessage.Stat = new MatchingPeer.Stat()
            {
              stat = (MatchingServerStat) assocList["stat"]
            };
            break;
          case 1:
            receivedMessage.Auth = new MatchingPeer.Auth()
            {
              success = (bool) assocList["success"]
            };
            break;
          case 2:
            receivedMessage.Message = new MatchingPeer.Message()
            {
              title = (string) assocList["title"],
              message = (string) assocList["message"]
            };
            break;
          case 3:
            receivedMessage.Heartbeat = new MatchingPeer.Heartbeat()
            {
              requireResponse = (bool) assocList["requireResponse"]
            };
            break;
          case 4:
            receivedMessage.MatchedConfirmation = new MatchingPeer.MatchedConfirmation()
            {
              player_id = (string) assocList["player_id"]
            };
            break;
          case 5:
            receivedMessage.Matched = new MatchingPeer.Matched()
            {
              host = (string) assocList["host"],
              port = (int) assocList["port"],
              battleToken = (string) assocList["battleToken"]
            };
            break;
          default:
            receivedMessage.Error = new Exception("invalid type");
            break;
        }
        promise.Result = receivedMessage;
      }
    }

    public Future<MatchingPeer.ReceivedMessage> ReceiveType(int timeoutMilliseconds = -1)
    {
      return new Future<MatchingPeer.ReceivedMessage>((Func<Promise<MatchingPeer.ReceivedMessage>, IEnumerator>) (promise => this.ReceiveTypeE(timeoutMilliseconds, promise)));
    }

    private IEnumerator SendNothrowE(
      Func<Future<int>> send,
      Promise<MatchingPeer.SendMessage> promise)
    {
      Future<int> f = send();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (f.Exception != null)
        promise.Result = new MatchingPeer.SendMessage()
        {
          Error = f.Exception
        };
      else
        promise.Result = new MatchingPeer.SendMessage()
        {
          SendId = f.Result
        };
    }

    public Future<MatchingPeer.SendMessage> SendNothrow(Func<Future<int>> send)
    {
      return new Future<MatchingPeer.SendMessage>((Func<Promise<MatchingPeer.SendMessage>, IEnumerator>) (promise => this.SendNothrowE(send, promise)));
    }

    public Future<MatchingPeer.SendMessage> StatRequest()
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[0], new object[0], 0);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(0, args, 5000)));
    }

    public Future<MatchingPeer.SendMessage> StartMatchingPvp(
      string playerId,
      int deckType,
      int deckId,
      string platform,
      string applicationVersion,
      string dlcVersion,
      string accessToken,
      string roomKey,
      PvpMatchingTypeEnum pvpMatchingTypeEnum,
      bool ignoreVersion,
      MatchingDebugInfo matchingDebugInfo)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[11]
      {
        nameof (accessToken),
        nameof (applicationVersion),
        nameof (deckId),
        nameof (deckType),
        nameof (dlcVersion),
        nameof (ignoreVersion),
        nameof (matchingDebugInfo),
        nameof (platform),
        nameof (playerId),
        nameof (pvpMatchingTypeEnum),
        nameof (roomKey)
      }, new object[11]
      {
        (object) accessToken,
        (object) applicationVersion,
        (object) deckId,
        (object) deckType,
        (object) dlcVersion,
        (object) ignoreVersion,
        (object) matchingDebugInfo,
        (object) platform,
        (object) playerId,
        (object) pvpMatchingTypeEnum,
        (object) roomKey
      }, 11);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(1, args, 5000)));
    }

    public Future<MatchingPeer.SendMessage> Ready(bool enable)
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[1]
      {
        nameof (enable)
      }, new object[1]{ (object) enable }, 1);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(2, args, 5000)));
    }

    public Future<MatchingPeer.SendMessage> HeartbeatResponse()
    {
      AssocList<string, object> args = new AssocList<string, object>(new string[0], new object[0], 0);
      return this.SendNothrow((Func<Future<int>>) (() => this.Send(3, args, 5000)));
    }

    [Serializable]
    public class Stat
    {
      public MatchingServerStat stat;

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
    public class Auth
    {
      public bool success;

      public override string ToString()
      {
        int num = this.success ? 1 : 0;
        return "Auth(" + string.Format("success={0}, ", (object) this.success.ToString()) + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["success"] = (object) this.success
        };
      }
    }

    [Serializable]
    public class Message
    {
      public string title;
      public string message;

      public override string ToString()
      {
        return "Message(" + string.Format("title={0}, ", this.title != null ? (object) this.title.ToString() : (object) "[null]") + string.Format("message={0}, ", this.message != null ? (object) this.message.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["title"] = (object) this.title,
          ["message"] = (object) this.message
        };
      }
    }

    [Serializable]
    public class Heartbeat
    {
      public bool requireResponse;

      public override string ToString()
      {
        int num = this.requireResponse ? 1 : 0;
        return "Heartbeat(" + string.Format("requireResponse={0}, ", (object) this.requireResponse.ToString()) + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["requireResponse"] = (object) this.requireResponse
        };
      }
    }

    [Serializable]
    public class MatchedConfirmation
    {
      public string player_id;

      public override string ToString()
      {
        return "MatchedConfirmation(" + string.Format("player_id={0}, ", this.player_id != null ? (object) this.player_id.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["player_id"] = (object) this.player_id
        };
      }
    }

    [Serializable]
    public class Matched
    {
      public string host;
      public int port;
      public string battleToken;

      public override string ToString()
      {
        string str1 = "Matched(" + string.Format("host={0}, ", this.host != null ? (object) this.host.ToString() : (object) "[null]");
        int port = this.port;
        string str2 = string.Format("port={0}, ", (object) this.port.ToString());
        return str1 + str2 + string.Format("battleToken={0}, ", this.battleToken != null ? (object) this.battleToken.ToString() : (object) "[null]") + ")";
      }

      public Dictionary<string, object> ToJson()
      {
        return new Dictionary<string, object>()
        {
          ["host"] = (object) this.host,
          ["port"] = (object) this.port,
          ["battleToken"] = (object) this.battleToken
        };
      }
    }

    [Serializable]
    public class ReceivedMessage
    {
      public Peer.CommonHeader Header;
      public Exception Error;
      public MatchingPeer.Stat Stat;
      public MatchingPeer.Auth Auth;
      public MatchingPeer.Message Message;
      public MatchingPeer.Heartbeat Heartbeat;
      public MatchingPeer.MatchedConfirmation MatchedConfirmation;
      public MatchingPeer.Matched Matched;

      public override string ToString()
      {
        string str = string.Format("ReceivedMessage(Header={0}, ", (object) this.Header);
        if (this.Stat != null)
          str += this.Stat.ToString();
        if (this.Auth != null)
          str += this.Auth.ToString();
        if (this.Message != null)
          str += this.Message.ToString();
        if (this.Heartbeat != null)
          str += this.Heartbeat.ToString();
        if (this.MatchedConfirmation != null)
          str += this.MatchedConfirmation.ToString();
        if (this.Matched != null)
          str += this.Matched.ToString();
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
