// Decompiled with JetBrains decompiler
// Type: Guild028ShopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild028ShopMenu : ShopArticleListMenu
{
  [SerializeField]
  private UILabel lblMedalNum;
  [SerializeField]
  private UI2DSprite spriteMedal;
  [SerializeField]
  private GameObject slc_Listbase_None;

  public override IEnumerator Init(Future<GameObject> cellPrefab)
  {
    Guild028ShopMenu guild028ShopMenu = this;
    guild028ShopMenu.lblMedalNum.SetTextLocalize(PlayerAffiliation.Current.guild_medal);
    Future<Sprite> ft = new ResourceObject("Icons/GuildMedal_Icon").Load<Sprite>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guild028ShopMenu.spriteMedal.sprite2D = ft.Result;
    ShopCommon.PayTypeGuildMedal = ft.Result;
    // ISSUE: reference to a compiler-generated method
    e = guild028ShopMenu.\u003C\u003En__0(cellPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guild028ShopMenu.slc_Listbase_None.SetActive(guild028ShopMenu.ScrollList == null || guild028ShopMenu.ScrollList.Count <= 0);
  }

  protected override IEnumerator LoadDetailPopup()
  {
    Guild028ShopMenu guild028ShopMenu = this;
    // ISSUE: reference to a compiler-generated method
    yield return (object) guild028ShopMenu.\u003C\u003En__1();
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) guild028ShopMenu.mapDetailPopup, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/popup/popup_031_map_detail__anim_popup01").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild028ShopMenu.mapDetailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) guild028ShopMenu.facilityDetailPopup, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/popup/popup_031_facility_detail__anim_popup01").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild028ShopMenu.facilityDetailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  protected override void UpdatePurchasedHolding(long nextholding)
  {
    this.lblMedalNum.SetTextLocalize(PlayerAffiliation.Current.guild_medal);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.backScene();
  }
}
