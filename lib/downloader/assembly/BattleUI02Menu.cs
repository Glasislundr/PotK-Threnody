// Decompiled with JetBrains decompiler
// Type: BattleUI02Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BattleUI02Menu : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtAttack22;
  [SerializeField]
  protected UILabel TxtAvoid22;
  [SerializeField]
  protected UILabel TxtCharaName18;
  [SerializeField]
  protected UILabel TxtDefence22;
  [SerializeField]
  protected UILabel TxtHit22;
  [SerializeField]
  protected UILabel TxtMdefence22;
  [SerializeField]
  protected UILabel TxtMOffence22;
  [SerializeField]
  protected UILabel TxtSlay22;
  [SerializeField]
  protected UILabel TxtTitle30;

  protected virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  protected virtual void IbtnBack() => Debug.Log((object) "click default event IbtnBack");

  protected virtual void IbtnBaseOff() => Debug.Log((object) "click default event IbtnBaseOff");

  protected virtual void IbtnBaseOn() => Debug.Log((object) "click default event IbtnBaseOn");

  protected virtual void IbtnBattleOff() => Debug.Log((object) "click default event IbtnBattleOff");

  protected virtual void IbtnBattleOn() => Debug.Log((object) "click default event IbtnBattleOn");

  protected virtual void IbtnDetailOff() => Debug.Log((object) "click default event IbtnDetailOff");

  protected virtual void IbtnDetailOn() => Debug.Log((object) "click default event IbtnDetailOn");

  protected virtual void IbtnOthersOff() => Debug.Log((object) "click default event IbtnOthersOff");

  protected virtual void IbtnOthersOn() => Debug.Log((object) "click default event IbtnOthersOn");

  protected virtual void IbtnSortEnemy() => Debug.Log((object) "click default event IbtnSortEnemy");

  protected virtual void IbtnSortNeutral()
  {
    Debug.Log((object) "click default event IbtnSortNeutral");
  }

  protected virtual void IbtnSortSelf() => Debug.Log((object) "click default event IbtnSortSelf");

  protected virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");
}
