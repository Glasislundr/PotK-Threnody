// Decompiled with JetBrains decompiler
// Type: MypageUnitUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public static class MypageUnitUtil
{
  public static int getUnitId() => Persist.mypageUnitId.Data._unit_id;

  public static void setUnit(int unit_id)
  {
    Persist.mypageUnitId.Data._unit_id = unit_id;
    Persist.mypageUnitId.Flush();
  }

  public static void setDefault() => MypageUnitUtil.setUnit(0);

  public static void setDefaultUnitNotFound() => MypageUnitUtil.setDefault();

  public static void sellUnit(int unit_id)
  {
    if (unit_id != MypageUnitUtil.getUnitId())
      return;
    MypageUnitUtil.setDefault();
  }
}
