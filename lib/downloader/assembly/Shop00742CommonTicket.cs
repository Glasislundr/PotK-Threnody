// Decompiled with JetBrains decompiler
// Type: Shop00742CommonTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00742CommonTicket : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtFlavor;
  [SerializeField]
  private UI2DSprite SlcTarget;
  [SerializeField]
  private UI2DSprite background;

  public IEnumerator Init(MasterDataTable.CommonRewardType type, int entity_id)
  {
    switch (type)
    {
      case MasterDataTable.CommonRewardType.gacha_ticket:
        yield return (object) this.doGachaTicket(entity_id);
        break;
      case MasterDataTable.CommonRewardType.unit_ticket:
        yield return (object) this.doUnitTicket(entity_id);
        break;
      case MasterDataTable.CommonRewardType.stamp:
        yield return (object) this.doStamp(entity_id);
        break;
      case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
        yield return (object) this.doTicket(entity_id);
        break;
      case MasterDataTable.CommonRewardType.recovery_item:
        yield return (object) this.doRecoveryItem(entity_id);
        break;
      case MasterDataTable.CommonRewardType.common_ticket:
        yield return (object) this.doCommonTicket(entity_id);
        break;
    }
  }

  private IEnumerator doGachaTicket(int entity_id)
  {
    GachaTicket gachaTicket = ((IEnumerable<GachaTicket>) MasterData.GachaTicketList).First<GachaTicket>((Func<GachaTicket, bool>) (x => x.ID == entity_id));
    this.TxtFlavor.SetText(gachaTicket.name);
    Future<Sprite> r = gachaTicket.LoadSpriteOrDefault();
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = r.Result;
  }

  private IEnumerator doUnitTicket(int entity_id)
  {
    SelectTicket selectTicket;
    if (MasterData.SelectTicket.TryGetValue(entity_id, out selectTicket))
    {
      this.TxtFlavor.SetText(selectTicket.name);
      Future<Sprite> r = selectTicket.createLoaderThumb();
      yield return (object) r.Wait();
      this.SlcTarget.sprite2D = r.Result;
    }
  }

  private IEnumerator doTicket(int entity_id)
  {
    UnitTypeTicket unitTypeTicket = ((IEnumerable<UnitTypeTicket>) MasterData.UnitTypeTicketList).First<UnitTypeTicket>((Func<UnitTypeTicket, bool>) (x => x.ID == entity_id));
    this.TxtFlavor.SetText(unitTypeTicket.name);
    Future<Sprite> r = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("ReincarnationTypeTicket/{0}/ticket", (object) unitTypeTicket.icon_id));
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = r.Result;
  }

  private IEnumerator doStamp(int entity_id)
  {
    ((Behaviour) this.background).enabled = false;
    ((Behaviour) this.SlcTarget).enabled = false;
    this.TxtFlavor.SetText(((IEnumerable<GuildStamp>) MasterData.GuildStampList).First<GuildStamp>((Func<GuildStamp, bool>) (x => x.groupID_GuildStampGroup == entity_id)).groupID.name);
    Future<GameObject> r = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("GUI/chat_stamp_group{0}/chat_stamp_group{1}_prefab", (object) entity_id, (object) entity_id));
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UIAtlas component = r.Result.GetComponent<UIAtlas>();
    UISprite uiSprite = ((Component) this.SlcTarget).gameObject.AddComponent<UISprite>();
    uiSprite.atlas = component;
    uiSprite.spriteName = component.spriteList[0].name;
    ((UIWidget) uiSprite).depth = ((UIWidget) this.SlcTarget).depth;
  }

  private IEnumerator doRecoveryItem(int entity_id)
  {
    this.TxtFlavor.SetText(((IEnumerable<RecoveryItemAPHeal>) MasterData.RecoveryItemAPHealList).First<RecoveryItemAPHeal>((Func<RecoveryItemAPHeal, bool>) (x => x.ID == entity_id)).name);
    Future<Sprite> r = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/recovery_item/{0}/2D/item_thum", (object) entity_id));
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = r.Result;
  }

  private IEnumerator doCommonTicket(int entity_id)
  {
    CommonTicket commonTicket = ((IEnumerable<CommonTicket>) MasterData.CommonTicketList).First<CommonTicket>((Func<CommonTicket, bool>) (x => x.ID == entity_id));
    if (commonTicket != null)
    {
      this.TxtFlavor.SetText(commonTicket.name);
      Future<Sprite> r = commonTicket.LoadIconMSpriteF();
      IEnumerator e = r.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SlcTarget.sprite2D = r.Result;
    }
  }
}
