// Decompiled with JetBrains decompiler
// Type: Unit00420ResetPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit00420ResetPopup : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite linkCharacter;
  [SerializeField]
  private UILabel txtNumUnitPossession;
  [SerializeField]
  private UILabel txtDescriptionNoUnit;
  [SerializeField]
  private UILabel txtDescriptionNoReinforce;
  [SerializeField]
  private GameObject ibtnPopupClose;
  [SerializeField]
  private GameObject ibtnPopupNormal;
  private const int RESET_UNIT_ID = 701204;
  private Unit004ReinforcePage menu;
  private PlayerUnit baseUnit;
  private PlayerMaterialUnit resetUnit;

  public IEnumerator Init(Unit004ReinforcePage bMenu, PlayerUnit bUnit, PlayerMaterialUnit rUnit)
  {
    this.menu = bMenu;
    this.baseUnit = bUnit;
    this.resetUnit = rUnit;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitIconGo = Object.Instantiate<GameObject>(prefabF.Result);
    UnitIcon unitIcon = unitIconGo.GetComponent<UnitIcon>();
    e = unitIcon.SetUnit(MasterData.UnitUnit[701204], MasterData.UnitUnit[701204].GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.linkCharacter).gameObject.SetActive(false);
    unitIconGo.gameObject.SetParent(((Component) this.linkCharacter).gameObject, 0.85f);
    ((Component) this.linkCharacter).gameObject.SetActive(true);
    unitIconGo.SetActive(true);
    unitIcon.onClick = (Action<UnitIconBase>) (_ => { });
    prefabF = (Future<GameObject>) null;
    unitIconGo = (GameObject) null;
    unitIcon = (UnitIcon) null;
    int quantity = this.resetUnit == null ? 0 : this.resetUnit.quantity;
    this.txtNumUnitPossession.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_00420_RESET_POPUP_NUM_UNIT_POSSESSION, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) quantity
      }
    }));
    ((Component) this.txtDescriptionNoUnit).gameObject.SetActive(false);
    ((Component) this.txtDescriptionNoReinforce).gameObject.SetActive(false);
    this.ibtnPopupClose.SetActive(false);
    this.ibtnPopupNormal.SetActive(false);
    if (bUnit.buildup_count == 0)
    {
      ((Component) this.txtDescriptionNoReinforce).gameObject.SetActive(true);
      this.ibtnPopupClose.SetActive(true);
    }
    else if (quantity == 0)
    {
      ((Component) this.txtDescriptionNoUnit).gameObject.SetActive(true);
      this.ibtnPopupClose.SetActive(true);
    }
    else
      this.ibtnPopupNormal.SetActive(true);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.reset());
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public IEnumerator reset()
  {
    Unit00420ResetPopup unit00420ResetPopup = this;
    if (unit00420ResetPopup.resetUnit != null && !Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      intList1.Add(unit00420ResetPopup.resetUnit.id);
      intList2.Add(1);
      Future<WebAPI.Response.UnitBuildup> paramF = WebAPI.UnitBuildup(unit00420ResetPopup.baseUnit.id, intList1.ToArray(), intList2.ToArray(), (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (paramF.Result != null)
      {
        unit00420ResetPopup.StartCoroutine(unit00420ResetPopup.UpdateMenu());
        paramF = (Future<WebAPI.Response.UnitBuildup>) null;
      }
    }
  }

  private IEnumerator UpdateMenu()
  {
    Unit00420ResetPopup unit00420ResetPopup = this;
    // ISSUE: reference to a compiler-generated method
    PlayerUnit baseUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), new Predicate<PlayerUnit>(unit00420ResetPopup.\u003CUpdateMenu\u003Eb__15_0));
    if (baseUnit == (PlayerUnit) null)
      baseUnit = unit00420ResetPopup.baseUnit;
    IEnumerator e = unit00420ResetPopup.menu.doReset(baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }
}
