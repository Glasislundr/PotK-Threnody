// Decompiled with JetBrains decompiler
// Type: Raid032ShopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032ShopMenu : ShopArticleListMenu
{
  [SerializeField]
  private UILabel lblMedalNum;
  [SerializeField]
  private UI2DSprite spriteMedal;

  public override IEnumerator Init(Future<GameObject> cellPrefab)
  {
    this.lblMedalNum.SetTextLocalize(SMManager.Get<Player>().raid_medal);
    Future<Sprite> ft = new ResourceObject("Icons/RaidJuel_Icon").Load<Sprite>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.spriteMedal.sprite2D = ft.Result;
    ShopCommon.PayTypeRaidJuel = ft.Result;
    e = base.Init(cellPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override IEnumerator LoadDetailPopup()
  {
    Raid032ShopMenu raid032ShopMenu = this;
    // ISSUE: reference to a compiler-generated method
    yield return (object) raid032ShopMenu.\u003C\u003En__1();
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) raid032ShopMenu.mapDetailPopup, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/popup/popup_031_map_detail__anim_popup01").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      raid032ShopMenu.mapDetailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) raid032ShopMenu.facilityDetailPopup, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/popup/popup_031_facility_detail__anim_popup01").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      raid032ShopMenu.facilityDetailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
