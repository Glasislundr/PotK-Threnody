// Decompiled with JetBrains decompiler
// Type: UnitIconHistoryWithNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class UnitIconHistoryWithNumber : WithNumber
{
  public override void pressButton()
  {
    if (!this.withNumberInfo.icon.withNumberInfo.buttonOn)
      return;
    base.pressButton();
    string sceneName = "";
    if (this.withNumberInfo.icon.withNumberInfo.IsMaterial)
      sceneName = "guide011_2_2b";
    else if (this.withNumberInfo.icon.withNumberInfo.unitData != null)
    {
      if (this.withNumberInfo.icon.withNumberInfo.unitData.Unit.character.category == UnitCategory.player)
        sceneName = "guide011_2_2";
      else if (this.withNumberInfo.icon.withNumberInfo.unitData.Unit.character.category == UnitCategory.enemy)
        sceneName = "guide011_3_2";
    }
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, true, (object) this.withNumberInfo.icon.withNumberInfo.unitData.Unit);
  }
}
