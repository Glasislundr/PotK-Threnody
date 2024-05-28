// Decompiled with JetBrains decompiler
// Type: ServerTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;

#nullable disable
public static class ServerTime
{
  private static readonly TimeSpan nextSyncSpan = new TimeSpan(0, 5, 0);
  private static bool isInit = false;
  private static DateTime nextSyncLocalTime;
  private static DateTime lastSyncServerTime;
  private static DateTime lastSyncLocalTime;

  public static void SetSyncServerTime(DateTime serverTime)
  {
    ServerTime.isInit = true;
    ServerTime.lastSyncServerTime = serverTime;
    ServerTime.lastSyncLocalTime = DateTime.Now;
    ServerTime.nextSyncLocalTime = DateTime.UtcNow.Add(ServerTime.nextSyncSpan);
  }

  public static IEnumerator WaitSync()
  {
    if (ServerTime.needUpdate())
    {
      Future<WebAPI.Response.HomeNow> result = WebAPI.HomeNow();
      IEnumerator e = result.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (result.Result != null)
      {
        ServerTime.lastSyncServerTime = result.Result.now;
        ServerTime.lastSyncLocalTime = DateTime.Now;
        ServerTime.nextSyncLocalTime = DateTime.UtcNow.Add(ServerTime.nextSyncSpan);
        ServerTime.isInit = true;
      }
    }
  }

  public static DateTime NowAppTime()
  {
    if (ServerTime.isInit)
      return ServerTime.lastSyncServerTime;
    Debug.LogError((object) "wait for API response. so return localtime");
    return DateTime.UtcNow.AddHours(Consts.GetInstance().APP_TIME_ZONE);
  }

  public static DateTime LastSyncLocalTime()
  {
    if (ServerTime.isInit)
      return ServerTime.lastSyncLocalTime;
    Debug.LogError((object) "wait for API response. so return localtime");
    return DateTime.UtcNow.AddHours(Consts.GetInstance().APP_TIME_ZONE);
  }

  private static bool needUpdate()
  {
    return !ServerTime.isInit || DateTime.UtcNow > ServerTime.nextSyncLocalTime;
  }

  public static DateTime NowAppTimeAddDelta()
  {
    TimeSpan timeSpan = ServerTime.nextSyncSpan - (ServerTime.nextSyncLocalTime - DateTime.UtcNow);
    return ServerTime.lastSyncServerTime.Add(timeSpan);
  }
}
