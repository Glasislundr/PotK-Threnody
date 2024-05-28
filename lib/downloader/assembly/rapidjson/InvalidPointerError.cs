// Decompiled with JetBrains decompiler
// Type: rapidjson.InvalidPointerError
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace rapidjson
{
  public class InvalidPointerError : Exception
  {
    public InvalidPointerError(string pointer)
      : base(pointer)
    {
    }
  }
}
