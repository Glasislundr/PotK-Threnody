// Decompiled with JetBrains decompiler
// Type: GameCore.CharExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GameCore
{
  public static class CharExtension
  {
    public static bool IsInvalidInput(this char c)
    {
      if (char.IsControl(c))
        return true;
      return c >= '\uE000' && c <= '\uF8FF';
    }
  }
}
