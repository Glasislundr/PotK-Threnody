// Decompiled with JetBrains decompiler
// Type: EventTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using com.adjust.sdk;
using Gsc.Purchase;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class EventTracker : MonoBehaviour
{
  [SerializeField]
  private string adjust_AppToken;
  [SerializeField]
  private long adjust_SecretKey_iOS;
  [SerializeField]
  private long adjust_SecretKey_Android;
  [SerializeField]
  private long[] adjust_SecretInfo_iOS;
  [SerializeField]
  private long[] adjust_SecretInfo_Android;
  [SerializeField]
  private string Growthbeat_ApplicationID;
  [SerializeField]
  private string Growthbeat_CredentialID;
  [SerializeField]
  private string Growthbeat_SenderID;
  private const string SESSION_CALLBACK_DEVICE_ID = "device_id";
  private const string EVENT_TOKEN_PAYMENT = "sxcah5";
  private const string EVENT_TOKEN_TUTORIAL_FINISH = "1miwvd";
  private const string EVENT_TOKEN_NEW_PLAYER_ID = "cx2bgj";
  private static bool isEnabledDeviceId;

  private IEnumerator Start()
  {
    EventTracker eventTracker = this;
    GrowthPush.Environment environment = GrowthPush.Environment.Production;
    GrowthPush.GetInstance().Initialize(eventTracker.Growthbeat_ApplicationID, eventTracker.Growthbeat_CredentialID, environment, false);
    GrowthPush.GetInstance().RequestDeviceToken(eventTracker.Growthbeat_SenderID);
    eventTracker.StartCoroutine("SetDeviceToken");
    GrowthPush.GetInstance().ClearBadge();
    Player player;
    while (true)
    {
      player = SMManager.Get<Player>();
      if (player == null)
        yield return (object) null;
      else
        break;
    }
    AdjustConfig adjustConfig = new AdjustConfig(eventTracker.adjust_AppToken, AdjustEnvironment.Production);
    adjustConfig.setLogLevel(AdjustLogLevel.Suppress);
    if (EventTracker.IsEnabledPlayerId(player))
      adjustConfig.setExternalDeviceId(player.id);
    Adjust.start(adjustConfig);
    GrowthPush.GetInstance().TrackEvent("Launch");
    GrowthPush.GetInstance().SetTag("Development", "OFF");
    if (Persist.pushnotification.Data.enablePush)
      GrowthPush.GetInstance().SetTag("PushNotification", "ON");
    else
      GrowthPush.GetInstance().SetTag("PushNotification", "OFF");
    GrowthPush.GetInstance().SetTag("PLAYER_LEVEL", player.level.ToString());
    GrowthPush.GetInstance().SetTag("PLAYER_CONTINUATUIN", player.continuation_date.ToString());
    GrowthPush.GetInstance().SetTag("HAVE_COINS", (player.paid_coin + player.free_coin).ToString());
    GrowthPush.GetInstance().SetTag("HAVE_FREE_COINS", player.free_coin.ToString());
    GrowthPush.GetInstance().SetTag("HAVE_PAID_COINS", player.paid_coin.ToString());
  }

  private IEnumerator SetDeviceToken()
  {
    yield break;
  }

  private static bool EnableDeviceId()
  {
    if (EventTracker.isEnabledDeviceId)
      return true;
    Player player = SMManager.Get<Player>();
    if (EventTracker.IsEnabledPlayerId(player))
    {
      Adjust.addSessionCallbackParameter("device_id", player.id);
      EventTracker.isEnabledDeviceId = true;
    }
    return EventTracker.isEnabledDeviceId;
  }

  private static bool IsEnabledPlayerId(Player player)
  {
    return player != null && !string.IsNullOrEmpty(player.id) && player.id != "1";
  }

  public static void SendPayment(ProductInfo product)
  {
    EventTracker.EnableDeviceId();
    AdjustEvent adjustEvent = new AdjustEvent("sxcah5");
    adjustEvent.setRevenue((double) product.Price, product.CurrencyCode);
    Adjust.trackEvent(adjustEvent);
    GrowthPush.GetInstance().TrackEvent("BUY_COIN");
    Player player = SMManager.Get<Player>();
    GrowthPush.GetInstance().SetTag("HAVE_COINS", (player.paid_coin + player.free_coin).ToString());
    GrowthPush.GetInstance().SetTag("HAVE_FREE_COINS", player.free_coin.ToString());
    GrowthPush.GetInstance().SetTag("HAVE_PAID_COINS", player.paid_coin.ToString());
  }

  public static void SendTutorialFinish()
  {
    EventTracker.EnableDeviceId();
    Adjust.trackEvent(new AdjustEvent("1miwvd"));
  }

  public static void SendNewPlayerId()
  {
    EventTracker.EnableDeviceId();
    Adjust.trackEvent(new AdjustEvent("cx2bgj"));
  }
}
