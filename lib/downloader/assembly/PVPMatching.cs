// Decompiled with JetBrains decompiler
// Type: PVPMatching
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Net;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PVPMatching : MonoBehaviour
{
  [SerializeField]
  private int timeoutMilliseconds = 100000;
  [SerializeField]
  private string matchingHost;
  [SerializeField]
  private int matchingPort;
  private PVPManager _pvpManager;
  private MatchingPeer peer;

  public bool IsCancel { private get; set; }

  private PVPManager pvpManager
  {
    get
    {
      if (Object.op_Equality((Object) this._pvpManager, (Object) null))
        this._pvpManager = Singleton<PVPManager>.GetInstance();
      return this._pvpManager;
    }
  }

  public void setMatchingServer(string host, int port)
  {
    this.matchingHost = host;
    this.matchingPort = port;
  }

  public void getMatchingServer(out string host, out int port)
  {
    host = this.matchingHost;
    port = this.matchingPort;
  }

  public void closePeer()
  {
    if (this.peer == null)
      return;
    this.peer.Close();
    this.peer = (MatchingPeer) null;
  }

  public void cleanupDestroy()
  {
    this.closePeer();
    Object.Destroy((Object) this);
  }

  public Future<MatchingPeer.MatchedConfirmation> matchingPVP(
    int deck_type_id,
    int deck_number,
    string roomkey = "",
    MatchingDebugInfo debugInfo = null)
  {
    return new Future<MatchingPeer.MatchedConfirmation>((Func<Promise<MatchingPeer.MatchedConfirmation>, IEnumerator>) (promise => this._matching(promise, deck_type_id, deck_number, roomkey, debugInfo)));
  }

  public Future<MatchingPeer.Matched> matchingReady(bool enable)
  {
    return new Future<MatchingPeer.Matched>((Func<Promise<MatchingPeer.Matched>, IEnumerator>) (promise => this._ready(promise, enable)));
  }

  private IEnumerator _matching(
    Promise<MatchingPeer.MatchedConfirmation> promise,
    int deck_type_id,
    int deck_number,
    string roomkey,
    MatchingDebugInfo debugInfo)
  {
    this.IsCancel = false;
    this.closePeer();
    this.peer = new MatchingPeer();
    Future<None> connectF = this.peer.Connect(this.matchingHost, this.matchingPort);
    IEnumerator e = connectF.Wait();
    while (e.MoveNext())
    {
      if (this.IsCancel)
        yield break;
      else
        yield return e.Current;
    }
    e = (IEnumerator) null;
    if (connectF.Exception != null)
    {
      promise.Exception = connectF.Exception;
    }
    else
    {
      string platform = "windows";
      Future<MatchingPeer.SendMessage> startMatchingPvpF = this.peer.StartMatchingPvp(SMManager.Get<Player>().id, deck_type_id, deck_number, platform, Revision.ApplicationVersion, Revision.DLCVersion, WebQueue.AuthToken, roomkey, this.pvpManager.matchingType, false, (MatchingDebugInfo) null);
      e = startMatchingPvpF.Wait();
      while (e.MoveNext())
      {
        if (this.IsCancel)
          yield break;
        else
          yield return e.Current;
      }
      e = (IEnumerator) null;
      if (startMatchingPvpF.Exception != null)
      {
        promise.Exception = startMatchingPvpF.Exception;
      }
      else
      {
        Future<MatchingPeer.ReceivedMessage> authF = this.peer.ReceiveOps(this.timeoutMilliseconds, MatchingServerOperation.Auth);
        e = authF.Wait();
        while (e.MoveNext())
        {
          if (this.IsCancel)
            yield break;
          else
            yield return e.Current;
        }
        e = (IEnumerator) null;
        if (authF.Result.Error != null)
          promise.Exception = authF.Result.Error;
        else if (!authF.Result.Auth.success)
        {
          promise.Exception = new Exception("auth check is false");
        }
        else
        {
          while (true)
          {
            authF = this.peer.ReceiveOps(this.timeoutMilliseconds, MatchingServerOperation.MatchedConfirmation, MatchingServerOperation.Heartbeat, MatchingServerOperation.Message);
            e = authF.Wait();
            while (e.MoveNext())
            {
              if (this.IsCancel)
                yield break;
              else
                yield return e.Current;
            }
            e = (IEnumerator) null;
            if (authF.Result.Error == null)
            {
              if (authF.Result.MatchedConfirmation == null)
              {
                if (authF.Result.Message == null || !authF.Result.Message.title.Equals("Start-NPC"))
                {
                  if (authF.Result.Heartbeat != null && authF.Result.Heartbeat.requireResponse)
                  {
                    e = this.peer.HeartbeatResponse().Wait();
                    while (e.MoveNext())
                    {
                      if (this.IsCancel)
                        yield break;
                      else
                        yield return e.Current;
                    }
                    e = (IEnumerator) null;
                  }
                  authF = (Future<MatchingPeer.ReceivedMessage>) null;
                }
                else
                  goto label_33;
              }
              else
                goto label_31;
            }
            else
              break;
          }
          promise.Exception = authF.Result.Error;
          yield break;
label_31:
          MatchingPeer.MatchedConfirmation matchedConfirmation = authF.Result.MatchedConfirmation;
          goto label_42;
label_33:
          matchedConfirmation = new MatchingPeer.MatchedConfirmation();
          matchedConfirmation.player_id = authF.Result.Message.title;
label_42:
          promise.Result = matchedConfirmation;
          connectF = (Future<None>) null;
          startMatchingPvpF = (Future<MatchingPeer.SendMessage>) null;
        }
      }
    }
  }

  private IEnumerator _ready(Promise<MatchingPeer.Matched> promise, bool enable)
  {
    bool flag;
    try
    {
      Future<MatchingPeer.SendMessage> ready = this.peer.Ready(enable);
      IEnumerator e = ready.Wait();
      while (e.MoveNext())
      {
        if (this.IsCancel)
        {
          flag = false;
          goto label_27;
        }
        else
          yield return e.Current;
      }
      e = (IEnumerator) null;
      if (ready.Exception != null)
      {
        promise.Exception = ready.Exception;
        flag = false;
      }
      else
      {
        Future<MatchingPeer.ReceivedMessage> matchedF;
        while (true)
        {
          matchedF = this.peer.ReceiveOps(this.timeoutMilliseconds, MatchingServerOperation.Matched, MatchingServerOperation.Heartbeat);
          e = matchedF.Wait();
          while (e.MoveNext())
          {
            if (this.IsCancel)
            {
              flag = false;
              goto label_27;
            }
            else
              yield return e.Current;
          }
          e = (IEnumerator) null;
          if (matchedF.Result.Error == null)
          {
            if (matchedF.Result.Matched == null)
            {
              if (matchedF.Result.Heartbeat != null && matchedF.Result.Heartbeat.requireResponse)
              {
                e = this.peer.HeartbeatResponse().Wait();
                while (e.MoveNext())
                {
                  if (this.IsCancel)
                  {
                    flag = false;
                    goto label_27;
                  }
                  else
                    yield return e.Current;
                }
                e = (IEnumerator) null;
              }
              if (!this.IsCancel)
                matchedF = (Future<MatchingPeer.ReceivedMessage>) null;
              else
                goto label_25;
            }
            else
              goto label_15;
          }
          else
            break;
        }
        promise.Exception = matchedF.Result.Error;
        flag = false;
        goto label_27;
label_15:
        promise.Result = matchedF.Result.Matched;
        ready = (Future<MatchingPeer.SendMessage>) null;
        yield break;
label_25:
        flag = false;
      }
label_27:;
    }
    finally
    {
      this.closePeer();
    }
    return flag;
  }
}
