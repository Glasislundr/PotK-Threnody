// Decompiled with JetBrains decompiler
// Type: GearIconHistoryWithNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class GearIconHistoryWithNumber : WithNumber
{
  public override void pressButton()
  {
    if (!this.withNumberInfo.icon.withNumberInfo.buttonOn)
      return;
    base.pressButton();
    string sceneName = this.withNumberInfo.icon.withNumberInfo.unitData.Gear.kind.Enum == GearKindEnum.smith || this.withNumberInfo.icon.withNumberInfo.unitData.Gear.kind.Enum == GearKindEnum.drilling || this.withNumberInfo.icon.withNumberInfo.unitData.Gear.kind.Enum == GearKindEnum.special_drilling || this.withNumberInfo.icon.withNumberInfo.unitData.Gear.kind.Enum == GearKindEnum.sea_present ? (this.withNumberInfo.icon.withNumberInfo.unitData.Gear.compose_kind.kind.Enum == GearKindEnum.smith || this.withNumberInfo.icon.withNumberInfo.unitData.Gear.compose_kind.kind.Enum == GearKindEnum.drilling || this.withNumberInfo.icon.withNumberInfo.unitData.Gear.compose_kind.kind.Enum == GearKindEnum.special_drilling || this.withNumberInfo.icon.withNumberInfo.unitData.Gear.compose_kind.kind.Enum == GearKindEnum.sea_present ? "guide011_4_2c" : "guide011_4_2b") : "guide011_4_2";
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, true, (object) this.withNumberInfo.icon.withNumberInfo.unitData.Gear, (object) true, (object) 0);
  }
}
