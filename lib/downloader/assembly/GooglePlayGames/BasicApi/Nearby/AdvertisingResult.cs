// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.BasicApi.Nearby.AdvertisingResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GooglePlayGames.OurUtils;

#nullable disable
namespace GooglePlayGames.BasicApi.Nearby
{
  public struct AdvertisingResult
  {
    private readonly ResponseStatus mStatus;
    private readonly string mLocalEndpointName;

    public AdvertisingResult(ResponseStatus status, string localEndpointName)
    {
      this.mStatus = status;
      this.mLocalEndpointName = Misc.CheckNotNull<string>(localEndpointName);
    }

    public bool Succeeded => this.mStatus == ResponseStatus.Success;

    public ResponseStatus Status => this.mStatus;

    public string LocalEndpointName => this.mLocalEndpointName;
  }
}
