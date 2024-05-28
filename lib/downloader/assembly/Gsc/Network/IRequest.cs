// Decompiled with JetBrains decompiler
// Type: Gsc.Network.IRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Network
{
  public interface IRequest
  {
    bool isDone { get; }

    WebTaskResult GetResult();

    string GetRequestID();

    string GetHost();

    string GetUrl();

    string GetPath();

    string GetMethod();

    byte[] GetPayload();

    IWebTask Cast();

    IWebTask Send();

    void Retry();
  }
}
