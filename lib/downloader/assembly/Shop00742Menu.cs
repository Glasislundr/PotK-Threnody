// Decompiled with JetBrains decompiler
// Type: Shop00742Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00742Menu : MonoBehaviour
{
  public IEnumerator Init(MasterDataTable.CommonRewardType type, PlayerShopArticle article)
  {
    ShopContent shopContent = ((IEnumerable<ShopContent>) MasterData.ShopContentList).Where<ShopContent>((Func<ShopContent, bool>) (x => x.article.ID == article.article.ID)).FirstOrDefault<ShopContent>();
    if (shopContent != null)
    {
      IEnumerator e = this.Init(type, shopContent.entity_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator Init(ShopContent content)
  {
    IEnumerator e = this.Init(content.entity_type, content.entity_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(EarthShopContent content)
  {
    IEnumerator e = this.Init(content.entity_type, content.entity_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(MasterDataTable.CommonRewardType type, int id)
  {
    IEnumerator e = ((Component) this).GetComponent<ItemDetailPopupBase>().SetInfo(type, id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(MasterDataTable.CommonRewardType type, int id, Action act)
  {
    ItemDetailPopupBase component = ((Component) this).GetComponent<ItemDetailPopupBase>();
    component.SetAction(act);
    IEnumerator e = component.SetInfo(type, id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static bool IsEnableShowPopup(MasterDataTable.CommonRewardType type)
  {
    return type == MasterDataTable.CommonRewardType.unit || type == MasterDataTable.CommonRewardType.material_unit || type == MasterDataTable.CommonRewardType.gear || type == MasterDataTable.CommonRewardType.material_gear || type == MasterDataTable.CommonRewardType.gear_body || type == MasterDataTable.CommonRewardType.quest_key || type == MasterDataTable.CommonRewardType.season_ticket;
  }
}
