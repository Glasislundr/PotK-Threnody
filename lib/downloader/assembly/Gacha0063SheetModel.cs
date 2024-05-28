// Decompiled with JetBrains decompiler
// Type: Gacha0063SheetModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
public class Gacha0063SheetModel
{
  private GachaModule module;

  public bool IsSheetGachaOpen => this.module.type == 6;

  public Gacha0063SheetModel(GachaModule module) => this.module = module;
}
