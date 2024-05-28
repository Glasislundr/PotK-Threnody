// Decompiled with JetBrains decompiler
// Type: Unit004OverKillersStealPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004OverKillersStealPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtDirection1;
  [SerializeField]
  private UILabel txtDirection2;
  [SerializeField]
  private Transform lnkCharacter;
  private PlayerUnit playerUnit;
  private Action yes;
  private Action no;
  private const float UnitIconScale = 0.8f;

  public void Init(PlayerUnit playerUnit, Action yes = null, Action no = null)
  {
    this.playerUnit = playerUnit;
    this.yes = yes;
    this.no = no;
    this.StartCoroutine(this.SetSprite());
  }

  private IEnumerator SetSprite()
  {
    Unit004OverKillersStealPopup killersStealPopup = this;
    killersStealPopup.txtDirection1.SetText(Consts.GetInstance().UNIT_004_OVER_KILLERS_STEAL_POPUP_DIRECTIONS1);
    killersStealPopup.txtDirection2.SetText("[ffff00]" + killersStealPopup.playerUnit.unit.name + "[-]\n" + Consts.GetInstance().UNIT_004_OVER_KILLERS_STEAL_POPUP_DIRECTIONS2);
    ((UIRect) ((Component) killersStealPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone(killersStealPopup.lnkCharacter);
    UnitIcon uniticon = prefab.GetComponent<UnitIcon>();
    uniticon.setBottom(killersStealPopup.playerUnit);
    uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    e = uniticon.SetUnit(killersStealPopup.playerUnit, killersStealPopup.playerUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    uniticon.BottomModeValue = UnitIconBase.GetBottomModeLevel(killersStealPopup.playerUnit.unit, killersStealPopup.playerUnit);
    uniticon.setLevelText(killersStealPopup.playerUnit);
    prefab.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
    ((UIRect) ((Component) killersStealPopup).GetComponent<UIWidget>()).alpha = 1f;
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Action no = this.no;
    if (no != null)
      no();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void ibtnOK()
  {
    if (this.IsPushAndSet())
      return;
    Action yes = this.yes;
    if (yes != null)
      yes();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
