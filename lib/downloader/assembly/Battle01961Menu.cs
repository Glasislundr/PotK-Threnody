// Decompiled with JetBrains decompiler
// Type: Battle01961Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01961Menu : BattleBackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtCharaname;
  [SerializeField]
  protected UILabel TxtCharaname2;
  [SerializeField]
  protected UILabel TxtConsume;
  [SerializeField]
  protected UILabel TxtCritical;
  [SerializeField]
  protected UILabel TxtHp;
  [SerializeField]
  protected UILabel TxtHp2;
  [SerializeField]
  protected UILabel TxtHpmax;
  [SerializeField]
  protected UILabel TxtHpmax2;
  [SerializeField]
  protected UILabel TxtREADME;
  [SerializeField]
  protected UILabel TxtRecovery;
  [SerializeField]
  protected UILabel TxtSkillname;
  [SerializeField]
  protected UILabel TxtWeaponName;
  [SerializeField]
  private BattleHealCharacterInfoHealer healer;
  [SerializeField]
  private BattleHealCharacterInfoInjured injured;
  private Battle019_6_1_RecoveryButton rButton;

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBack()
  {
    if (Object.op_Equality((Object) this.rButton, (Object) null))
    {
      Battle019_6_1_RecoveryButton[] componentsInChildren = ((Component) this).GetComponentsInChildren<Battle019_6_1_RecoveryButton>(true);
      if (componentsInChildren.Length != 0)
        this.rButton = componentsInChildren[0];
    }
    if (this.rButton.isComplited)
      return;
    this.rButton.isComplited = true;
    this.backScene();
  }

  public IEnumerator Init(BL.UnitPosition attack, BL.UnitPosition defense)
  {
    Battle01961Menu battle01961Menu = this;
    AttackStatus[] attackStatus = BattleFuncs.getAttackStatusArray(attack, defense, true, true);
    AttackStatus[] attackStatusArray = BattleFuncs.getAttackStatusArray(defense, attack, false, true);
    IEnumerator e = battle01961Menu.injured.Init(defense, attackStatusArray);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = battle01961Menu.healer.Init(attack, attackStatus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) battle01961Menu).GetComponent<UIPanel>().SetDirty();
  }
}
