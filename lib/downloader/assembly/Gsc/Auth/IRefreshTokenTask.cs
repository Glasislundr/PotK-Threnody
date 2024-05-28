// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.IRefreshTokenTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Tasks;

#nullable disable
namespace Gsc.Auth
{
  public interface IRefreshTokenTask : ITask
  {
    WebTaskResult Result { get; }
  }
}
