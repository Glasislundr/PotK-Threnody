// Decompiled with JetBrains decompiler
// Type: SeaTalkDestruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class SeaTalkDestruction : BackButtonPopupWindow
{
  private PlayerCallLetter playerCallLetter;
  private TalkUnitInfo talkUnitInfo;
  private Action callBack;
  [SerializeField]
  private Transform unitIconParent;
  [SerializeField]
  private Transform destructionItemIconParent;
  [SerializeField]
  private UILabel destructionItemQuantity;
  [SerializeField]
  private SpreadColorButton callDestructionButton;

  public IEnumerator Init(
    PlayerCallLetter playerCallLetter,
    TalkUnitInfo talkUnitInfo,
    Action callBack)
  {
    this.playerCallLetter = playerCallLetter;
    this.talkUnitInfo = talkUnitInfo;
    this.callBack = callBack;
    yield return (object) this.SetPartnerUnit();
    yield return (object) this.SetDestructionIcon();
    int num = 0;
    PlayerMaterialGear playerMaterialGear = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear_id == 14999998));
    if (playerMaterialGear != (PlayerMaterialGear) null)
    {
      num = Mathf.Min(playerMaterialGear.quantity, 9999);
      this.destructionItemQuantity.text = string.Format("所持数 : {0}", (object) num);
    }
    if (num <= 0)
    {
      ((UIWidget) this.destructionItemQuantity).color = Color.red;
      this.destructionItemQuantity.text = string.Format("所持数 : {0}", (object) num);
      ((UIButtonColor) this.callDestructionButton).isEnabled = false;
    }
    else
      this.destructionItemQuantity.text = string.Format("所持数 : {0}", (object) num);
  }

  private IEnumerator SetPartnerUnit()
  {
    Future<GameObject> f = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon unitIcon = f.Result.Clone(this.unitIconParent).GetComponent<UnitIcon>();
    e = unitIcon.SetUnit(this.talkUnitInfo.unit, this.talkUnitInfo.unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) unitIcon.Button).gameObject.SetActive(false);
    unitIcon.BottomBaseObject = false;
    unitIcon.BackgroundModeValue = UnitIcon.BackgroundMode.Call;
  }

  private IEnumerator SetDestructionIcon()
  {
    Future<GameObject> f = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon component = f.Result.Clone(this.destructionItemIconParent).GetComponent<ItemIcon>();
    GearGear gear = (GearGear) null;
    MasterData.GearGear.TryGetValue(14999998, out gear);
    e = component.InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void OnDestructionButton() => this.StartCoroutine(this.onDestructionButton());

  private IEnumerator onDestructionButton()
  {
    Future<GameObject> f = new ResourceObject("Prefabs/sea030_talk/popup_030_sea_PledgeCancell_Confirm__anim_fade").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(f.Result, maskAlpha: 0.7f, fadeOutFlag: true);
    gameObject.SetActive(false);
    gameObject.GetComponent<SeaTalkDestructionConfirmation>().Init(this.playerCallLetter, this.callBack);
    gameObject.SetActive(true);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().onDismiss();
}
