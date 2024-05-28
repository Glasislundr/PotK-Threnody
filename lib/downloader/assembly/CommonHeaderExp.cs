// Decompiled with JetBrains decompiler
// Type: CommonHeaderExp
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
public class CommonHeaderExp : CommonHeaderBase
{
  private GameObject popupPlayerStatus;
  [SerializeField]
  private Transform popupState;
  private UIWidget popupStateWidget;
  private TweenAlpha popupAnimation;
  private UILabel nextExp;
  private UILabel nowLv;
  private UILabel UnitCount;
  private UILabel ItemCount;
  private UILabel guildName;
  private UILabel roleName;
  private UILabel userName;
  private bool createPrefab;
  private CommonHeaderLevelExp _popup_level_exp;
  private bool isButtonEnable = true;

  private void Awake()
  {
    this.nextExp = (UILabel) null;
    this.nowLv = (UILabel) null;
    this.createPrefab = false;
    this.popupStateWidget = ((Component) this.popupState).GetComponent<UIWidget>();
    this.popupAnimation = ((Component) this.popupState).GetComponent<TweenAlpha>();
    this.StartCoroutine(this.CreatePrefab());
  }

  public void SetIsButtonEnable(bool v) => this.isButtonEnable = v;

  private void Start() => this.Init();

  public void OnPress(bool isDown)
  {
    if (!this.isButtonEnable)
      return;
    if (isDown)
    {
      this.SetTextNextExpToNowLv();
      this.SetTextUserName();
      this.AnimSet(1f);
    }
    else
      this.AnimSet(0.0f);
  }

  public void OnStonePlus()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || !this.isButtonEnable)
      return;
    this.StartCoroutine(PopupUtility.BuyKiseki());
  }

  public void OnAPPlus()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || !this.isButtonEnable || Singleton<NGGameDataManager>.GetInstance().IsEarth)
      return;
    this.StartCoroutine(PopupUtility.RecoveryAP());
  }

  private IEnumerator CreatePrefab()
  {
    CommonHeaderExp commonHeaderExp = this;
    commonHeaderExp._popup_level_exp = (CommonHeaderLevelExp) null;
    if (Object.op_Inequality((Object) commonHeaderExp.popupPlayerStatus, (Object) null))
      Object.Destroy((Object) commonHeaderExp.popupPlayerStatus);
    commonHeaderExp.popupPlayerStatus = (GameObject) null;
    Future<GameObject> featureObj = Res.Prefabs.mypage.popup_PlayerStatus.Load<GameObject>();
    IEnumerator e = featureObj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    commonHeaderExp.popupPlayerStatus = featureObj.Result.Clone(commonHeaderExp.popupState);
    commonHeaderExp._popup_level_exp = commonHeaderExp.popupPlayerStatus.GetComponent<CommonHeaderLevelExp>();
    commonHeaderExp.ap = commonHeaderExp.popupPlayerStatus.GetComponentInChildren<CommonHeaderAP>();
    commonHeaderExp.bp = commonHeaderExp.popupPlayerStatus.GetComponentInChildren<CommonHeaderBP>();
    Transform transform1 = commonHeaderExp.popupPlayerStatus.transform.Find("slc_player_status_base");
    if (Object.op_Equality((Object) transform1, (Object) null))
    {
      Debug.LogWarning((object) "名前が変わってる");
    }
    else
    {
      Transform transform2 = transform1.Find("txt_ToNextLV");
      Transform transform3 = transform1.Find("txt_Lv");
      if (Object.op_Equality((Object) transform2, (Object) null) || Object.op_Equality((Object) transform3, (Object) null))
      {
        Debug.LogWarning((object) "名前が変わってる");
      }
      else
      {
        commonHeaderExp.nextExp = ((Component) transform2).GetComponent<UILabel>();
        commonHeaderExp.nowLv = ((Component) transform3).GetComponent<UILabel>();
        Transform transform4 = transform1.Find("txt_hime");
        Transform transform5 = transform1.Find("txt_item");
        if (Object.op_Equality((Object) transform4, (Object) null) || Object.op_Equality((Object) transform5, (Object) null))
        {
          Debug.LogWarning((object) "名前が変わってる");
        }
        else
        {
          commonHeaderExp.UnitCount = ((Component) transform4).GetComponent<UILabel>();
          commonHeaderExp.ItemCount = ((Component) transform5).GetComponent<UILabel>();
          Transform transform6 = transform1.Find("txt_guild_name");
          Transform transform7 = transform1.Find("txt_guild_post");
          if (Object.op_Equality((Object) transform6, (Object) null) || Object.op_Equality((Object) transform7, (Object) null))
          {
            Debug.LogWarning((object) "名前が変わってる");
          }
          else
          {
            commonHeaderExp.guildName = ((Component) transform6).GetComponent<UILabel>();
            commonHeaderExp.roleName = ((Component) transform7).GetComponent<UILabel>();
            Transform transform8 = transform1.Find("txt_User_Name");
            if (Object.op_Inequality((Object) transform8, (Object) null))
              commonHeaderExp.userName = ((Component) transform8).GetComponent<UILabel>();
            commonHeaderExp.createPrefab = true;
          }
        }
      }
    }
  }

  private void SetItemCount(int now_itemCount = 0, int max_ItemCount = 0)
  {
    this.ItemCount.SetTextLocalize(Consts.Format(Consts.GetInstance().HEADER_POPUP_ITEM_COUNT, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) now_itemCount
      },
      {
        (object) "max",
        (object) max_ItemCount
      }
    }));
  }

  private void SetUnitCount(int now_UnitCount = 0, int max_UnitCount = 0)
  {
    this.UnitCount.SetTextLocalize(Consts.Format(Consts.GetInstance().HEADER_POPUP_UNIT_COUNT, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) now_UnitCount
      },
      {
        (object) "max",
        (object) max_UnitCount
      }
    }));
  }

  private void SetGuildName(string name)
  {
    if (!(this.guildName.text != name))
      return;
    this.guildName.SetTextLocalize(name);
  }

  private void SetRoleName(string name)
  {
    if (!(this.roleName.text != name))
      return;
    this.roleName.SetTextLocalize(name);
  }

  private void AnimSet(float to)
  {
    if (Object.op_Equality((Object) this.popupAnimation, (Object) null) || Object.op_Equality((Object) this.popupStateWidget, (Object) null))
    {
      Debug.LogWarning((object) "初期化できてない");
    }
    else
    {
      this.popupAnimation.to = to;
      ((Behaviour) this.popupAnimation).enabled = false;
      this.popupAnimation.from = ((UIRect) this.popupStateWidget).alpha;
      ((UITweener) this.popupAnimation).duration = Mathf.Abs(this.popupAnimation.to - this.popupAnimation.from) / 4f;
      ((UITweener) this.popupAnimation).ResetToBeginning();
      ((Behaviour) this.popupAnimation).enabled = true;
    }
  }

  private void SetTextNextExpToNowLv()
  {
    if (Object.op_Equality((Object) this.nextExp, (Object) null) || Object.op_Equality((Object) this.nowLv, (Object) null) || Player.Current == null)
    {
      Debug.LogWarning((object) "初期化できてない");
    }
    else
    {
      this.nextExp.SetTextLocalize(Player.Current.exp_next.ToString());
      this.nowLv.SetTextLocalize(Player.Current.level.ToString());
      if (!Object.op_Inequality((Object) this._popup_level_exp, (Object) null))
        return;
      this._popup_level_exp.updateData();
    }
  }

  private void SetTextUserName()
  {
    if (Object.op_Equality((Object) this.userName, (Object) null))
      return;
    this.userName.SetTextLocalize(Player.Current.name);
  }

  protected override void Update()
  {
    if (Object.op_Equality((Object) this.ap, (Object) null) || Object.op_Equality((Object) this.bp, (Object) null) || !this.createPrefab || this.player.Value == null)
      return;
    base.Update();
    this.UpdateApRecoveryTime();
    this.UpdateBpReocveryTime();
    if (this.UpdateUnits() && this.units.Value != null)
      this.SetUnitCount(this.units.Value.Length, this.player.Value.max_units);
    if (this.UpdateItems() && this.items.Value != null)
      this.SetItemCount(((IEnumerable<PlayerItem>) this.items.Value).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou())).ToArray<PlayerItem>().Length, this.player.Value.max_items);
    if (this.UpdateApGauge() && this.units.Value != null && this.items.Value != null)
    {
      this.bp.setValue(this.player.Value.bp);
      this.SetUnitCount(this.units.Value.Length, this.player.Value.max_units);
      this.SetItemCount(((IEnumerable<PlayerItem>) this.items.Value).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou())).ToArray<PlayerItem>().Length, this.player.Value.max_items);
    }
    if (PlayerAffiliation.Current == null)
      return;
    if (PlayerAffiliation.Current.status == GuildMembershipStatus.membership)
    {
      this.SetGuildName(PlayerAffiliation.Current.guild.guild_name);
      this.SetRoleName(PlayerAffiliation.Current.role_name.name);
    }
    else
    {
      this.SetGuildName(Consts.GetInstance().GUILD_HEADER_GUILD_NAME_NONE);
      this.SetRoleName(Consts.GetInstance().COMMON_NOVALUE);
    }
  }
}
