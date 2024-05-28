// Decompiled with JetBrains decompiler
// Type: Raid032BattleBossInfo
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
public class Raid032BattleBossInfo : MonoBehaviour
{
  [SerializeField]
  private Color32 DISABLE_REWARD_COLOR = new Color32((byte) 75, (byte) 75, (byte) 75, byte.MaxValue);
  [SerializeField]
  private Color32 DISABLE_REWARD_ICON_COLOR = new Color32((byte) 128, (byte) 128, (byte) 128, byte.MaxValue);
  [Space(10f)]
  [SerializeField]
  private UILabel mNameLabel;
  [SerializeField]
  private UILabel mLvLabel;
  [SerializeField]
  private Transform mBossAnimImageAnchor;
  [SerializeField]
  private UILabel mNowHpLabel;
  [SerializeField]
  private UILabel mMaxHpLabel;
  [SerializeField]
  private NGTweenGaugeScale mHpGauge;
  [SerializeField]
  private Transform mRewardsAnchor;
  [SerializeField]
  private CreateIconObject mKillRewardAnchor;
  [SerializeField]
  private CreateIconObject[] mDamageRewardAnchors;
  private GameObject mDetailPopupPrefab;
  private Raid032BattleBossAnime mBossAnimImage;
  private List<GuildRaid.RaidReward> mKillRewards;
  private List<SpreadColorSprite> mRewardIconCache = new List<SpreadColorSprite>();
  private List<UIWidget> mTweenWwidgetCache;
  private List<TweenColor> mTweenCache;
  private bool isStartedEndless;

  public IEnumerator InitAsync(
    GuildRaid masterData,
    WebAPI.Response.GuildraidBattleDetail webResponse)
  {
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.mDetailPopupPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/popup/popup_000_7_4_2__anim_popup01").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mDetailPopupPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    yield return (object) MasterData.LoadBattleStageEnemy(MasterData.BattleStage[masterData.stage_id]);
    BattleStageEnemy boss = masterData.getBoss();
    UnitUnit unit = boss.unit;
    this.isStartedEndless = webResponse.is_started_endless;
    this.mNameLabel.SetTextLocalize(unit.name);
    this.mLvLabel.SetTextLocalize(boss.level);
    if (Object.op_Equality((Object) this.mBossAnimImage, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/raid032_battle/dir_RaidBoss_anim").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mBossAnimImage = prefabF.Result.CloneAndGetComponent<Raid032BattleBossAnime>(this.mBossAnimImageAnchor);
      prefabF = (Future<GameObject>) null;
    }
    yield return (object) this.mBossAnimImage.InitializeAsync(unit, masterData.image_offset_y);
    int hp = boss.hp;
    GuildRaidEndless guildRaidEndless = ((IEnumerable<GuildRaidEndless>) MasterData.GuildRaidEndlessList).FirstOrDefault<GuildRaidEndless>((Func<GuildRaidEndless, bool>) (x => x.ID == masterData.ID));
    int num1 = 5;
    int periodID = ((IEnumerable<GuildRaid>) MasterData.GuildRaidList).FirstOrDefault<GuildRaid>((Func<GuildRaid, bool>) (x => x.ID == masterData.ID)).period_id;
    KeyValuePair<int, GuildRaid> keyValuePair = MasterData.GuildRaid.Where<KeyValuePair<int, GuildRaid>>((Func<KeyValuePair<int, GuildRaid>, bool>) (x => x.Value.period_id == periodID)).OrderByDescending<KeyValuePair<int, GuildRaid>, int>((Func<KeyValuePair<int, GuildRaid>, int>) (x => x.Value.lap)).FirstOrDefault<KeyValuePair<int, GuildRaid>>();
    if (keyValuePair.Value != null)
      num1 = keyValuePair.Value.lap;
    int loopCount = webResponse.loop_count;
    if (loopCount > num1 && guildRaidEndless != null)
    {
      int num2 = loopCount - num1;
      hp += num2 * guildRaidEndless.hp;
    }
    this.setHp(hp, hp - webResponse.boss_total_damage);
    this.initRewardTweens();
    if (!webResponse.is_started_endless)
      yield return (object) this.setKillRewards(masterData.getKillRewardsList());
    else
      yield return (object) this.setKillRewards(masterData.getRaidEndlessKillRewardsList());
    yield return (object) this.setDamageRewards(masterData.getDamageRewardsList(), masterData.getDamageRewardRatiosList(), webResponse.boss_damage_ratio);
  }

  public void Reload(bool rewardEnable)
  {
    if (rewardEnable)
    {
      if (this.mTweenCache != null)
      {
        foreach (UITweener uiTweener in this.mTweenCache)
          uiTweener.PlayReverse();
      }
    }
    else if (this.mTweenCache != null)
    {
      foreach (UITweener uiTweener in this.mTweenCache)
        uiTweener.PlayForward();
    }
    this.mBossAnimImage.StartAnime();
  }

  private void setHp(int max, int now)
  {
    now = Mathf.Max(0, now);
    this.mMaxHpLabel.SetTextLocalize("/" + max.ToString());
    this.mNowHpLabel.SetTextLocalize(now.ToString());
    this.mHpGauge.setValue(now, max, false);
  }

  private IEnumerator setKillRewards(List<GuildRaid.RaidReward> rewards)
  {
    this.mKillRewards = rewards;
    if (this.mKillRewards.Count > 1)
    {
      yield return (object) this.mKillRewardAnchor.CreateThumbnail(MasterDataTable.CommonRewardType.emblem, 0, visibleBottom: false, isButton: false);
    }
    else
    {
      GuildRaid.RaidReward raidReward = rewards.First<GuildRaid.RaidReward>();
      yield return (object) this.mKillRewardAnchor.CreateThumbnail(raidReward.Type, raidReward.Id, visibleBottom: false, isButton: false);
    }
    GameObject icon = this.mKillRewardAnchor.GetIcon();
    if (Object.op_Equality((Object) icon.GetComponent<SpreadColorSprite>(), (Object) null))
      icon.AddComponent<SpreadColorSprite>();
    this.mRewardIconCache.Add(icon.GetComponent<SpreadColorSprite>());
  }

  private IEnumerator setDamageRewards(
    List<GuildRaid.RaidReward> rewards,
    List<int> damageRatios,
    int damageRatio)
  {
    Raid032BattleBossInfo raid032BattleBossInfo1 = this;
    for (int i = 0; i < raid032BattleBossInfo1.mDamageRewardAnchors.Length; ++i)
    {
      Raid032BattleBossInfo raid032BattleBossInfo = raid032BattleBossInfo1;
      if (raid032BattleBossInfo1.isStartedEndless)
      {
        ((Component) ((Component) raid032BattleBossInfo1.mDamageRewardAnchors[i]).transform.parent).gameObject.SetActive(false);
      }
      else
      {
        ((Component) ((Component) raid032BattleBossInfo1.mDamageRewardAnchors[i]).transform.parent).gameObject.SetActive(true);
        MasterDataTable.CommonRewardType type = rewards[i].Type;
        int rewardId = rewards[i].Id;
        int quantity = rewards[i].Quantity;
        bool isGotten = damageRatios[i] <= damageRatio;
        yield return (object) raid032BattleBossInfo1.mDamageRewardAnchors[i].CreateThumbnail(type, rewardId);
        GameObject icon1 = raid032BattleBossInfo1.mDamageRewardAnchors[i].GetIcon();
        if (Object.op_Inequality((Object) icon1.GetComponent<UnitIcon>(), (Object) null))
        {
          icon1.GetComponent<UnitIcon>().onClick = (Action<UnitIconBase>) (icon => raid032BattleBossInfo.onPushRewardIcon(type, rewardId));
          icon1.GetComponent<UnitIcon>().Gray = isGotten;
          icon1.AddComponent<SpreadColorSprite>();
        }
        else if (Object.op_Inequality((Object) icon1.GetComponent<ItemIcon>(), (Object) null))
        {
          icon1.GetComponent<ItemIcon>().onClick = (Action<ItemIcon>) (icon => raid032BattleBossInfo.onPushRewardIcon(type, rewardId));
          icon1.GetComponent<ItemIcon>().Gray = isGotten;
          icon1.AddComponent<SpreadColorSprite>();
        }
        else if (Object.op_Inequality((Object) icon1.GetComponent<UniqueIcons>(), (Object) null))
        {
          icon1.GetComponent<UniqueIcons>().onClick = (Action<UniqueIcons>) (icon => raid032BattleBossInfo.onPushRewardIcon(type, rewardId));
          icon1.GetComponent<UniqueIcons>().Gray = isGotten;
        }
        raid032BattleBossInfo1.mRewardIconCache.Add(icon1.GetComponent<SpreadColorSprite>());
        ((Component) ((Component) raid032BattleBossInfo1.mDamageRewardAnchors[i]).transform.parent.Find("txt_Item_Num")).GetComponent<UILabel>().SetTextLocalize("x" + (object) quantity);
        ((Component) ((Component) raid032BattleBossInfo1.mDamageRewardAnchors[i]).transform.parent.Find("slc_Get")).gameObject.SetActive(isGotten);
      }
    }
  }

  public void onFightingFeeButton()
  {
    if (this.mKillRewards.Count > 1)
    {
      this.StartCoroutine(RaidBattleFightingFeePopup.show(this.mKillRewards));
    }
    else
    {
      GuildRaid.RaidReward raidReward = this.mKillRewards.First<GuildRaid.RaidReward>();
      this.onPushRewardIcon(raidReward.Type, raidReward.Id);
    }
  }

  private void onPushRewardIcon(MasterDataTable.CommonRewardType rewardType, int rewardId)
  {
    switch (rewardType)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.supply:
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.quest_key:
      case MasterDataTable.CommonRewardType.season_ticket:
      case MasterDataTable.CommonRewardType.material_unit:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.awake_skill:
      case MasterDataTable.CommonRewardType.gear_body:
        this.StartCoroutine(this.openRewardDetail(rewardType, rewardId));
        break;
    }
  }

  private IEnumerator openRewardDetail(MasterDataTable.CommonRewardType rewardType, int rewardId)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.mDetailPopupPrefab);
    popup.SetActive(false);
    yield return (object) popup.GetComponent<Shop00742Menu>().Init(rewardType, rewardId);
    popup.SetActive(true);
  }

  public void SetRewardsEnable(bool enable)
  {
    if (enable)
      this.enableRewards();
    else
      this.disableRewards();
  }

  private void initRewardTweens()
  {
    this.mTweenWwidgetCache = new List<UIWidget>();
    foreach (UIWidget componentsInChild in ((Component) this.mRewardsAnchor).GetComponentsInChildren<UIWidget>(true))
      this.mTweenWwidgetCache.Add(componentsInChild);
  }

  private void disableRewards()
  {
    this.mTweenCache = new List<TweenColor>();
    foreach (UIWidget uiWidget in this.mTweenWwidgetCache)
    {
      Color color = uiWidget.color;
      TweenColor tweenColor = TweenColor.Begin(((UIRect) uiWidget).cachedGameObject, 0.0f, Color32.op_Implicit(this.DISABLE_REWARD_COLOR));
      if (Object.op_Inequality((Object) tweenColor, (Object) null))
      {
        tweenColor.from = color;
        this.mTweenCache.Add(tweenColor);
      }
    }
    foreach (UIWidget uiWidget in this.mRewardIconCache)
      uiWidget.color = Color32.op_Implicit(this.DISABLE_REWARD_ICON_COLOR);
  }

  private void enableRewards()
  {
    if (this.mTweenCache != null)
    {
      foreach (UITweener uiTweener in this.mTweenCache)
        uiTweener.PlayReverse();
    }
    foreach (SpreadColorSprite spreadColorSprite in this.mRewardIconCache)
    {
      if (this.isGrayIcon(((Component) spreadColorSprite).gameObject))
        ((UIWidget) spreadColorSprite).color = Color32.op_Implicit(this.DISABLE_REWARD_ICON_COLOR);
      else
        ((UIWidget) spreadColorSprite).color = Color.white;
    }
  }

  private bool isGrayIcon(GameObject icon)
  {
    bool flag = false;
    if (Object.op_Inequality((Object) icon.GetComponent<UnitIcon>(), (Object) null))
      flag = icon.GetComponent<UnitIcon>().Gray;
    else if (Object.op_Inequality((Object) icon.GetComponent<ItemIcon>(), (Object) null))
      flag = icon.GetComponent<ItemIcon>().Gray;
    else if (Object.op_Inequality((Object) icon.GetComponent<UniqueIcons>(), (Object) null))
      flag = icon.GetComponent<UniqueIcons>().Gray;
    return flag;
  }
}
