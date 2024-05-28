// Decompiled with JetBrains decompiler
// Type: ErrorFlagExtention
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
internal static class ErrorFlagExtention
{
  public static bool IsOn(this Unit004TrainingPage.ErrorFlag self, Unit004TrainingPage.ErrorFlag v)
  {
    return (self & v) != 0;
  }

  public static bool IsOff(this Unit004TrainingPage.ErrorFlag self, Unit004TrainingPage.ErrorFlag v)
  {
    return (self & v) == Unit004TrainingPage.ErrorFlag.Clear;
  }

  public static bool Any(this Unit004TrainingPage.ErrorFlag self, Unit004TrainingPage.ErrorFlag v = Unit004TrainingPage.ErrorFlag.Any)
  {
    return self.IsOn(v);
  }

  public static Unit004TrainingPage.ErrorFlag On(
    ref this Unit004TrainingPage.ErrorFlag self,
    Unit004TrainingPage.ErrorFlag v)
  {
    self |= v;
    return self;
  }

  public static Unit004TrainingPage.ErrorFlag Off(
    ref this Unit004TrainingPage.ErrorFlag self,
    Unit004TrainingPage.ErrorFlag v)
  {
    self &= ~v;
    return self;
  }

  public static Unit004TrainingPage.ErrorFlag Reset(
    ref this Unit004TrainingPage.ErrorFlag self,
    Unit004TrainingPage.ErrorFlag v = Unit004TrainingPage.ErrorFlag.Clear)
  {
    self = v;
    return self;
  }
}
