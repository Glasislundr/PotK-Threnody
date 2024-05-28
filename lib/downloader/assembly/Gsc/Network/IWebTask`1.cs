// Decompiled with JetBrains decompiler
// Type: Gsc.Network.IWebTask`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Tasks;
using System.Collections;

#nullable disable
namespace Gsc.Network
{
  public interface IWebTask<TResponse> : IWebTask, IWebTaskBase, ITask, IEnumerator where TResponse : Gsc.Network.Response<TResponse>
  {
    TResponse Response { get; }

    IErrorResponse error { get; }
  }
}
