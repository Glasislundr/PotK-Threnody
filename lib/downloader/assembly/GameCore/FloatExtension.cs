// Decompiled with JetBrains decompiler
// Type: GameCore.FloatExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GameCore
{
  public static class FloatExtension
  {
    private const int SAFE_DECIMAL = 100;

    public static int GetInteger(this float number) => (int) (number.CarryPercent() / 100L);

    public static int GetDecimalAsPercent(this float number)
    {
      return (int) (number.CarryPercent() % 100L);
    }

    public static long CarryPercent(this float number)
    {
      long num;
      if ((long) ((double) (num = (long) ((double) number * 100.0)) * 10.0) % 10L > 0L)
        ++num;
      return num;
    }
  }
}
