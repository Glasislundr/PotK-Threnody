// Decompiled with JetBrains decompiler
// Type: Gsc.Network.ReturnCallback`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Network
{
  public delegate TResult ReturnCallback<TRequest, TResponse, TResult>(
    TRequest request,
    TResponse response);
}
