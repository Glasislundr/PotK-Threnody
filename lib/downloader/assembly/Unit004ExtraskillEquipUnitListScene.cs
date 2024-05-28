// Decompiled with JetBrains decompiler
// Type: Unit004ExtraskillEquipUnitListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004ExtraskillEquipUnitListScene : NGSceneBase
{
  [SerializeField]
  private Unit004ExtraskillEquipUnitListMenu menu;

  public static void changeScene(bool stack, PlayerUnit targetUnit, PlayerAwakeSkill targetSkill)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_extraskill_equip_unit_list", (stack ? 1 : 0) != 0, (object) targetUnit, (object) targetSkill);
  }

  public virtual IEnumerator onStartSceneAsync(PlayerUnit targetUnit, PlayerAwakeSkill targetSkill)
  {
    IEnumerator e = this.menu.Init(targetUnit, targetSkill);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(PlayerUnit targetUnit, PlayerAwakeSkill targetSkill)
  {
  }

  public override void onEndScene()
  {
  }
}
