// Decompiled with JetBrains decompiler
// Type: ServerSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc;

#nullable disable
public static class ServerSelector
{
  public static string ApiUrl => SDK.Configuration.Env.ServerUrl;

  public static string DlcUrl => SDK.Configuration.GetEnv<Gsc.App.Environment>().DlcPath;
}
