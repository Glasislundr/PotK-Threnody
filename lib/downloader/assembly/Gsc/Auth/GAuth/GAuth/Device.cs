// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.Device
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth
{
  public class Device : IDevice
  {
    public readonly string IDFA;
    public readonly string ID;

    public static Device Instance { get; private set; }

    public string Platform => "none";

    public bool initialized => true;

    public bool hasError => false;

    public Device()
    {
      Device.Instance = this;
      this.IDFA = App.GetIdfa();
      this.ID = App.GetClientId();
    }
  }
}
