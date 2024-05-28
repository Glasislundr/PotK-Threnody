// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.Session
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Device;

#nullable disable
namespace Gsc.Auth
{
  public static class Session
  {
    public static ISession DefaultSession { get; private set; }

    public static void Init(string envName, IAccountManager accountManager)
    {
      Session.DefaultSession = (ISession) new Gsc.Auth.GAuth.DMMGamesStore.Session(envName, accountManager);
    }
  }
}
