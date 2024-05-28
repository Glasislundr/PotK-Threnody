// Decompiled with JetBrains decompiler
// Type: AnalyticsSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnalyticsSample : MonoBehaviour
{
  private bool logEnabled = true;
  public string spotCode = "";
  public bool useReceiver;
  public bool testMode;
  private bool pushNotificationToggleState;

  private void Start()
  {
    this.pushNotificationToggleState = MetapsAnalyticsScript.IsPushNotificationEnabled();
  }

  private void OnEnable()
  {
  }

  private void OnDisable()
  {
  }

  private void Update()
  {
  }

  private void OnGUI()
  {
    GUIStyle guiStyle = new GUIStyle();
    guiStyle.alignment = (TextAnchor) 4;
    guiStyle.normal.textColor = Color.white;
    guiStyle.wordWrap = true;
    float num1 = 60f;
    float num2 = 300f;
    float num3 = 40f;
    float num4 = 50f;
    double num5 = (double) (Screen.width / 2) - (double) num2 / 2.0;
    GUI.Label(new Rect((float) num5, num4, num2, num3), "Analytics Unity Sample App", guiStyle);
    float num6 = num4 + num1;
    if (GUI.Button(new Rect((float) num5, num6, num2, num3), "Purchase Diamonds for 0.99 USD"))
      MetapsAnalyticsScript.TrackPurchase("Diamonds", 0.99, "USD");
    float num7 = num6 + num1;
    if (GUI.Button(new Rect((float) num5, num7, num2, num3), "Track Send 5 invites"))
      MetapsAnalyticsScript.TrackEvent("Invite", "Send", 5);
    float num8 = num7 + num1;
    if (GUI.Button(new Rect((float) num5, num8, num2, num3), "Track Tutorial Clear"))
      MetapsAnalyticsScript.TrackEvent("Tutorial", "Clear");
    float num9 = num8 + num1;
    if (GUI.Button(new Rect((float) num5, num9, num2, num3), "Track Spend 10 tickets in shop"))
      MetapsAnalyticsScript.TrackSpend("Shop", "tickets", 10);
    float num10 = num9 + num1;
    if (GUI.Button(new Rect((float) num5, num10, num2, num3), "Set User Profile age"))
      MetapsAnalyticsScript.SetUserProfile("BIRTHDAY", "19801101");
    float num11 = num10 + num1;
    if (GUI.Button(new Rect((float) num5, num11, num2, num3), "Remove Attribute"))
      MetapsAnalyticsScript.SetAttribute("AGE", (string) null);
    float num12 = num11 + num1;
    if (GUI.Button(new Rect((float) num5, num12, num2, num3), "Track Action"))
      MetapsAnalyticsScript.TrackAction("ConversionPoint");
    float num13 = num12 + num1;
    if (GUI.Button(new Rect((float) num5, num13, num2, num3), "Enable log"))
    {
      this.logEnabled = !this.logEnabled;
      MetapsAnalyticsScript.SetLogEnabled(this.logEnabled);
    }
    bool flag = GUI.Toggle(new Rect((float) num5, num13 + num1, num2, num3), this.pushNotificationToggleState, "Push Notification enabled");
    if (flag == this.pushNotificationToggleState)
      return;
    this.pushNotificationToggleState = flag;
    MetapsAnalyticsScript.SetPushNotificationEnabled(flag);
  }
}
