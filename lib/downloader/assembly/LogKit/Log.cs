// Decompiled with JetBrains decompiler
// Type: LogKit.Log
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace LogKit
{
  public struct Log
  {
    public Guid ID;
    public string Tag;
    public DateTime Date;
    public LogLevel LogLevel;
    public UserInfo UserInfo;
  }
}
