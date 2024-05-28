// Decompiled with JetBrains decompiler
// Type: GrowthPush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class GrowthPush
{
  private static GrowthPush instance = new GrowthPush();

  private GrowthPush()
  {
  }

  public static GrowthPush GetInstance() => GrowthPush.instance;

  public void Initialize(
    string applicationId,
    string credentialId,
    GrowthPush.Environment environment)
  {
    this.Initialize(applicationId, credentialId, environment, true);
  }

  public void Initialize(
    string applicationId,
    string credentialId,
    GrowthPush.Environment environment,
    bool adInfoEnable)
  {
    this.Initialize(applicationId, credentialId, environment, adInfoEnable, (string) null);
  }

  public void Initialize(
    string applicationId,
    string credentialId,
    GrowthPush.Environment environment,
    bool adInfoEnable,
    string channelId)
  {
  }

  public void RequestDeviceToken(string senderId) => this.RequestDeviceToken();

  public void RequestDeviceToken()
  {
  }

  public string GetDeviceToken() => (string) null;

  public void SetDeviceToken(string deviceToken)
  {
  }

  public void ClearBadge()
  {
  }

  public void SetTag(string name) => this.SetTag(name, "");

  public void SetTag(string name, string value)
  {
  }

  public void TrackEvent(string name) => this.TrackEvent(name, "");

  public void TrackEvent(string name, string value)
  {
  }

  public void TrackEvent(string name, string value, string gameObject, string methodName)
  {
  }

  public void RenderMessage(string uuid)
  {
  }

  public void SetChannelId(string channelId)
  {
  }

  public void DeleteDefaultNotificationChannel()
  {
  }

  public void SetBaseUrl(string baseUrl)
  {
  }

  public enum Environment
  {
    Unknown,
    Development,
    Production,
  }
}
