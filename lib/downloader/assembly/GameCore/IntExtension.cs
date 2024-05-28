// Decompiled with JetBrains decompiler
// Type: GameCore.IntExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GameCore
{
  public static class IntExtension
  {
    public static string ToLocalizeNumberText(this int number) => number.ToString().ToConverter();

    public static bool IsInvalid(this int? number) => !number.HasValue || number.Value == 0;

    public static bool IsInvalid(this int number) => number == 0;

    public static bool IsValid(this int? number) => number.HasValue && number.Value != 0;

    public static bool IsValid(this int number) => number != 0;
  }
}
