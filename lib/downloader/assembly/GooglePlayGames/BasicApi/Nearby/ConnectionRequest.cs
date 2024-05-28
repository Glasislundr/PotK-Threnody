// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.BasicApi.Nearby.ConnectionRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GooglePlayGames.OurUtils;

#nullable disable
namespace GooglePlayGames.BasicApi.Nearby
{
  public struct ConnectionRequest
  {
    private readonly EndpointDetails mRemoteEndpoint;
    private readonly byte[] mPayload;

    public ConnectionRequest(
      string remoteEndpointId,
      string remoteEndpointName,
      string serviceId,
      byte[] payload)
    {
      Logger.d("Constructing ConnectionRequest");
      this.mRemoteEndpoint = new EndpointDetails(remoteEndpointId, remoteEndpointName, serviceId);
      this.mPayload = Misc.CheckNotNull<byte[]>(payload);
    }

    public EndpointDetails RemoteEndpoint => this.mRemoteEndpoint;

    public byte[] Payload => this.mPayload;
  }
}
