// Decompiled with JetBrains decompiler
// Type: Battle0202502Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle0202502Menu : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtFriendName;
  [SerializeField]
  protected UILabel TxtLastTime;
  [SerializeField]
  protected UILabel TxtPopuptitle26;
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 1f;
  [SerializeField]
  private GameObject linkChar;
  [SerializeField]
  public UIButton OK;
  [SerializeField]
  protected UI2DSprite Emblem;
  private DateTime lastTime;
  [SerializeField]
  protected GameObject slc_Master;
  [SerializeField]
  protected GameObject slc_Friend;
  [SerializeField]
  protected GameObject slc_Guild;

  public IEnumerator Init(PlayerHelper helper, int incr_friend_point)
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = prefabF.Result.Clone(this.linkChar.transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    e = unitScript.SetUnit(helper.leader_unit, helper.leader_unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.BottomModeValue = UnitIconBase.GetBottomMode(helper.leader_unit.unit, (PlayerUnit) null);
    unitScript.setLevelText(helper.leader_unit.total_level.ToLocalizeNumberText());
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    this.lastTime = helper.target_player_last_signed_in_at;
    this.SetText(helper.target_player_name, incr_friend_point);
    e = this.SetEmblem(helper.current_emblem_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.slc_Friend.SetActive(false);
    this.slc_Guild.SetActive(false);
    this.slc_Master.SetActive(false);
    if (helper.is_friend)
      this.slc_Friend.SetActive(true);
    else if (helper.is_guild_member)
      this.slc_Guild.SetActive(true);
    else
      this.slc_Master.SetActive(true);
  }

  public void SetText(string name, int point)
  {
    this.TxtFriendName.SetTextLocalize(name);
    TimeSpan self = ServerTime.NowAppTime() - this.lastTime;
    if (self.Days < 3)
      this.TxtLastTime.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00282_LAST_LOGIN, (IDictionary) new Hashtable()
      {
        {
          (object) "time",
          (object) self.DisplayString()
        }
      }));
    else
      this.TxtLastTime.SetTextLocalize(Consts.GetInstance().QUEST_00282_FRIEND_MANAGER);
  }

  public virtual void IbtnPopupOk() => Singleton<PopupManager>.GetInstance().dismiss();

  private IEnumerator SetEmblem(int emblemId)
  {
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(emblemId);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Emblem.sprite2D = sprF.Result;
  }
}
