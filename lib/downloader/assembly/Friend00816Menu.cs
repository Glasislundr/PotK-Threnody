// Decompiled with JetBrains decompiler
// Type: Friend00816Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Friend00816Menu : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtDescription01;
  [SerializeField]
  protected UILabel TxtDescription0324_;
  [SerializeField]
  protected UILabel TxtLastplay;
  [SerializeField]
  protected UILabel TxtPlayername;
  [SerializeField]
  protected UILabel TxtPopuptitle;
  [SerializeField]
  protected UILabel TxtREADME;
  [SerializeField]
  private GameObject LinkUnit;
  [SerializeField]
  protected UILabel TxtLv;
  [SerializeField]
  protected UI2DSprite Emblem;
  private GameObject unitIconGo;
  private UnitIcon unitIcon;

  public IEnumerator SetTargetUserData()
  {
    this.TxtPlayername.SetText("テストプレイヤー");
    this.TxtLv.SetTextLocalize(77);
    this.TxtLastplay.SetText("最終プレイ：[FFFF00]取得予定[-]");
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIconGo = Object.Instantiate<GameObject>(prefabF.Result);
    this.unitIconGo.transform.parent = this.LinkUnit.transform;
    this.unitIconGo.transform.localPosition = Vector3.zero;
    this.unitIconGo.transform.localScale = Vector3.one;
    this.LinkUnit.SetActive(true);
    this.unitIcon = this.unitIconGo.GetComponent<UnitIcon>();
    e = this.unitIcon.SetUnit(MasterData.UnitUnit[100111], CommonElement.none, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(0);
    e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Emblem.sprite2D = sprF.Result;
  }

  public IEnumerator SetTargetUserData(WebAPI.Response.PlayerSearch result)
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime dateTime = ServerTime.NowAppTime();
    this.TxtDescription01.SetTextLocalize("ＩＤ：" + result.target_player.short_id);
    this.TxtPlayername.SetTextLocalize(result.target_player.name);
    this.TxtLv.SetTextLocalize(result.target_player.level);
    DateTime playerLastSignedInAt = result.target_player_helper.target_player_last_signed_in_at;
    TimeSpan self = dateTime - playerLastSignedInAt;
    this.TxtLastplay.SetText(Consts.Format(Consts.GetInstance().LAST_PLAY, (IDictionary) new Hashtable()
    {
      {
        (object) "time",
        (object) self.DisplayStringForFriendsGuildMember()
      }
    }));
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIconGo = Object.Instantiate<GameObject>(prefabF.Result);
    this.unitIconGo.transform.parent = this.LinkUnit.transform;
    this.unitIconGo.transform.localPosition = Vector3.zero;
    this.unitIconGo.transform.localScale = Vector3.one;
    this.LinkUnit.SetActive(true);
    this.unitIcon = this.unitIconGo.GetComponent<UnitIcon>();
    e = this.unitIcon.setSimpleUnit(result.target_leader_unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon.setLevelText(result.target_leader_unit);
    this.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(result.target_player_helper.current_emblem_id);
    e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Emblem.sprite2D = sprF.Result;
  }

  public virtual void IbtnPopupOk() => Debug.Log((object) "click default event IbtnPopupOk");
}
