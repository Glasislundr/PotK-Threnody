// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.BasicApi.Nearby.EndpointDetails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GooglePlayGames.OurUtils;

#nullable disable
namespace GooglePlayGames.BasicApi.Nearby
{
  public struct EndpointDetails
  {
    private readonly string mEndpointId;
    private readonly string mName;
    private readonly string mServiceId;

    public EndpointDetails(string endpointId, string name, string serviceId)
    {
      this.mEndpointId = Misc.CheckNotNull<string>(endpointId);
      this.mName = Misc.CheckNotNull<string>(name);
      this.mServiceId = Misc.CheckNotNull<string>(serviceId);
    }

    public string EndpointId => this.mEndpointId;

    public string Name => this.mName;

    public string ServiceId => this.mServiceId;
  }
}
