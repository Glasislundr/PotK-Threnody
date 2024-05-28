// Decompiled with JetBrains decompiler
// Type: Unit004ExtraskillEquipListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004ExtraskillEquipListScene : NGSceneBase
{
  [SerializeField]
  private Unit004ExtraskillEquipListMenu menu;

  public static void changeScene(bool bStack, EditAwakeSkillParam param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_extraskill_equip_list", (bStack ? 1 : 0) != 0, (object) param);
  }

  public static void changeScene(bool stack, PlayerUnit unit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_extraskill_equip_list", (stack ? 1 : 0) != 0, (object) unit);
  }

  public IEnumerator onStartSceneAsync(EditAwakeSkillParam param)
  {
    this.menu.EditParam = param;
    this.menu.TargetUnit = param.baseUnit;
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(EditAwakeSkillParam param)
  {
  }

  public virtual IEnumerator onStartSceneAsync(PlayerUnit unit)
  {
    this.menu.TargetUnit = unit;
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(PlayerUnit unit)
  {
  }
}
