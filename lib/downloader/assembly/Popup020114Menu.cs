// Decompiled with JetBrains decompiler
// Type: Popup020114Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;

#nullable disable
public class Popup020114Menu : Popup020113Menu
{
  public override IEnumerator Init(QuestHarmonyS quest)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Popup020114Menu popup020114Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    popup020114Menu.UnitName[0].SetTextLocalize(quest.unit.name);
    popup020114Menu.UnitName[1].SetTextLocalize(quest.target_unit.name);
    popup020114Menu.UnitName[2].SetTextLocalize(quest.target_unit2.name);
    popup020114Menu.StartCoroutine(popup020114Menu.SetCharaIcon(quest.unit.character, quest.target_unit.character, quest.target_unit2.character));
    return false;
  }

  public IEnumerator SetCharaIcon(UnitCharacter unit_1, UnitCharacter unit_2, UnitCharacter unit_3)
  {
    Popup020114Menu popup020114Menu = this;
    IEnumerator e = popup020114Menu.LoadUnitPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = popup020114Menu.SetUnitPrefab(popup020114Menu.UnitIconObject[0], unit_1.GetDefaultUnitUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = popup020114Menu.SetUnitPrefab(popup020114Menu.UnitIconObject[1], unit_2.GetDefaultUnitUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = popup020114Menu.SetUnitPrefab(popup020114Menu.UnitIconObject[2], unit_3.GetDefaultUnitUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
