// Decompiled with JetBrains decompiler
// Type: Shop0071LeaderStand
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
public class Shop0071LeaderStand : MonoBehaviour
{
  public void SetLeaderCharacter()
  {
    UnitUnit unitUnit = ShopTopUnit.GetShopTopUnit();
    int job_id;
    if (unitUnit == null)
    {
      PlayerUnit displayPlayerUnit = this.GetDisplayPlayerUnit();
      unitUnit = displayPlayerUnit.unit;
      job_id = displayPlayerUnit.job_id;
    }
    else
      job_id = unitUnit.job_UnitJob;
    this.StartCoroutine(this.SetLeaderCharacter(unitUnit.ID, job_id));
  }

  private IEnumerator SetLeaderCharacter(int id, int job_id)
  {
    Shop0071LeaderStand shop0071LeaderStand = this;
    UnitUnit unitdata = MasterData.UnitUnit[id];
    Future<Sprite> CharacterF = unitdata.LoadSpriteLarge(job_id, 1f);
    IEnumerator e = CharacterF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = CharacterF.Result;
    ((Component) shop0071LeaderStand).GetComponent<NGxMaskSprite>().sprite2D = result;
    yield return (object) shop0071LeaderStand.DownloadVoice(unitdata.unitVoicePattern.file_name);
    Singleton<NGSoundManager>.GetInstance().stopVoice();
    Singleton<NGSoundManager>.GetInstance().playVoiceByID(unitdata.unitVoicePattern, 62);
  }

  private IEnumerator DownloadVoice(string vo_number)
  {
    List<string> paths = new List<string>();
    string platform = Singleton<NGSoundManager>.GetInstance().platform;
    paths.Add(platform + "/" + vo_number + "_acb");
    paths.Add(platform + "/" + vo_number + "_awb");
    yield return (object) OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) paths, false);
  }

  private PlayerUnit GetDisplayPlayerUnit()
  {
    int mypage_unit_id = MypageUnitUtil.getUnitId();
    if (mypage_unit_id == 0)
      return this.GetDeckLeaderPlayerUnit();
    PlayerUnit displayPlayerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == mypage_unit_id));
    if (!(displayPlayerUnit == (PlayerUnit) null))
      return displayPlayerUnit;
    MypageUnitUtil.setDefaultUnitNotFound();
    return this.GetDeckLeaderPlayerUnit();
  }

  private PlayerUnit GetDeckLeaderPlayerUnit()
  {
    PlayerDeck[] playerDeckArray = SMManager.Get<PlayerDeck[]>();
    PlayerUnit leaderPlayerUnit = ((IEnumerable<PlayerUnit>) playerDeckArray[Persist.deckOrganized.Data.number].player_units).FirstOrDefault<PlayerUnit>();
    if (leaderPlayerUnit == (PlayerUnit) null)
      leaderPlayerUnit = ((IEnumerable<PlayerUnit>) playerDeckArray[0].player_units).First<PlayerUnit>();
    return leaderPlayerUnit;
  }
}
