// Decompiled with JetBrains decompiler
// Type: Startup00014Renzoku
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
public class Startup00014Renzoku : Startup00014Menu
{
  protected readonly string faceName = "happy";
  protected int loginTotalDay = 5;
  [SerializeField]
  private Vector3 loginAnimPos = Vector3.zero;
  [SerializeField]
  protected UILabel TxtNavi;
  [SerializeField]
  protected UITweener fade;
  [SerializeField]
  protected GameObject get;
  [SerializeField]
  protected GameObject next;
  [SerializeField]
  protected Transform charaContainer;
  [SerializeField]
  protected GameObject GetNextMaskTop;
  [SerializeField]
  protected GameObject GetNextMaskLeft;
  [SerializeField]
  protected GameObject mainItemBaseObj;
  [SerializeField]
  private UITexture back;
  [SerializeField]
  private UITexture title;
  private Texture frame;
  protected GameObject loginAnime;
  protected GameObject finishStamp;
  protected GameObject todayStamp;
  protected GameObject stampFrame;
  protected Sprite mainSprite;
  protected Texture2D maskTexture;
  protected int unitID = 100611;
  protected UnitUnit unitData;
  protected int charaID;
  protected bool story;
  private const float scale = 1.5f;
  protected const int depth = 100;
  protected Startup00014BonusIcon itemList = new Startup00014BonusIcon();
  private GameObject getItem;
  protected Startup00014BonusIcon newItemList = new Startup00014BonusIcon();
  private GameObject nextItem;
  private List<GameObject> stampList = new List<GameObject>();
  private List<Startup00014StampFrame> stampFrameList = new List<Startup00014StampFrame>();
  private int pageCount;
  protected GameObject naviChara;
  protected int maxRewardValue;
  protected int drawIconValue = 10;
  protected PlayerLoginBonus loginBonus;
  protected int stampValue;
  protected List<LoginbonusReward> loginBonusRewardList;
  protected LoginbonusReward loginBonusReward;
  protected bool rewardReset;
  protected bool rewardLast;

  public void onNextButton()
  {
    Singleton<NGSoundManager>.GetInstance().stopSE();
    if (this.pageCount == 0)
      this.NowStamp();
    else if (this.pageCount == 1)
    {
      this.PlayVoiceAndFaceChange();
      this.RewardChange();
    }
    else if (this.pageCount == 2)
      this.SceneEnd();
    ++this.pageCount;
  }

  private void PlayVoiceAndFaceChange()
  {
    this.changeFace(this.faceName);
    Singleton<NGSoundManager>.GetInstance().stopVoice();
    if (this.charaID == 0)
      Singleton<NGSoundManager>.GetInstance().playVoiceByStringID("VO_9999", "durin_0005");
    else if (this.loginBonusReward.que_name2 != null)
    {
      string[] strArray = this.loginBonusReward.que_name2.Split(',');
      if (strArray.Length > 1)
        Singleton<NGSoundManager>.GetInstance().playVoiceByStringID(strArray[1], strArray[0]);
      else if (this.story)
        Singleton<NGSoundManager>.GetInstance().playVoiceByStringID("VO_" + (object) this.charaID, this.loginBonusReward.que_name2);
      else if (this.unitData.unitVoicePattern != null)
        Singleton<NGSoundManager>.GetInstance().playVoiceByStringID(this.unitData.unitVoicePattern, this.loginBonusReward.que_name2);
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
  }

  private void RewardReset()
  {
    this.itemList.ListEnable(false);
    this.newItemList.ListEnable(true);
    foreach (Object stamp in this.stampList)
      Object.Destroy(stamp);
  }

  private void SceneEnd()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.fade.onFinished = new List<EventDelegate>();
    this.fade.PlayReverse();
    this.fade.AddOnFinished(new EventDelegate.Callback(this.AnimationEnd));
    ((Behaviour) ((Component) this.fade).gameObject.GetComponent<UIButton>()).enabled = false;
  }

  private void RewardChange()
  {
    this.TxtNavi.SetTextLocalize(this.loginBonusReward.next_reward_message);
    if (this.rewardLast)
      return;
    this.get.SetActive(false);
    this.next.SetActive(true);
    this.Play(this.next.transform);
    this.nextItem.SetActive(true);
    this.getItem.SetActive(false);
    if (!this.rewardReset)
      return;
    this.RewardReset();
  }

  protected void DestroyTrash()
  {
    foreach (Component stampFrame in this.stampFrameList)
      Object.Destroy((Object) stampFrame.gameObject);
    foreach (Object stamp in this.stampList)
      Object.Destroy(stamp);
    Object.Destroy((Object) this.getItem);
    Object.Destroy((Object) this.nextItem);
    Object.Destroy((Object) this.naviChara);
    this.itemList.Clear();
    this.newItemList.Clear();
    this.stampList.Clear();
    this.stampFrameList.Clear();
  }

  protected virtual void Init()
  {
    this.DestroyTrash();
    this.rewardReset = false;
    this.rewardLast = false;
    this.loginTotalDay = this.loginBonus.login_days;
    this.loginBonusRewardList = ((IEnumerable<LoginbonusReward>) MasterData.LoginbonusRewardList).Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.loginbonus == this.loginBonus.loginbonus)).OrderBy<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToList<LoginbonusReward>();
    this.loginBonusReward = this.loginBonusRewardList.Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.number == this.loginTotalDay)).First<LoginbonusReward>();
    this.maxRewardValue = this.loginBonusRewardList.Count;
    this.unitID = this.loginBonusReward.character_id;
    this.story = this.unitID < 100000;
    if (this.story)
    {
      this.charaID = this.unitID;
    }
    else
    {
      this.unitData = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.ID == this.unitID)).First<UnitUnit>();
      this.charaID = this.unitData.character.ID;
    }
    this.drawIconValue = this.loginBonus.loginbonus.draw_reward_num;
    this.listIcons = this.positionList[this.drawIconValue - 1].positionList;
    this.stampValue = this.loginTotalDay % this.drawIconValue;
    if (this.stampValue == 0)
      this.stampValue = this.drawIconValue;
    if (this.stampValue != 0)
      return;
    this.stampValue = 1;
  }

  protected virtual IEnumerator LoadResource()
  {
    Future<GameObject> animeF = Res.Animations.longin_bonus.LoginAnimationRoot.Load<GameObject>();
    IEnumerator e = animeF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.loginAnime = animeF.Result;
    Future<GameObject> finishF = Res.Prefabs.startup000_14.slc_Stamp_Finished.Load<GameObject>();
    e = finishF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.finishStamp = finishF.Result;
    Future<GameObject> todayF = Res.Prefabs.startup000_14.slc_Stamp_Today.Load<GameObject>();
    e = todayF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.todayStamp = todayF.Result;
    Future<GameObject> stampFrameF = Res.Prefabs.startup000_14.stampFrame.Load<GameObject>();
    e = stampFrameF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.stampFrame = stampFrameF.Result;
    Future<GameObject> naviF = (Future<GameObject>) null;
    Future<Sprite> spriteF = (Future<Sprite>) null;
    if (this.story)
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMobUnit(this.charaID), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      naviF = MobUnits.LoadStory(this.charaID);
      spriteF = MobUnits.LoadSpriteLarge(this.charaID);
    }
    else
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) new List<UnitUnit>()
      {
        this.unitData
      }, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      naviF = this.unitData.LoadStory();
      spriteF = this.unitData.LoadSpriteLarge();
    }
    e = naviF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.naviChara = naviF.Result.Clone(((Component) this.charaContainer).transform);
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mainSprite = spriteF.Result;
    Future<Sprite> futureC = Res.GUI._009_3_sozai.mask_Chara_C.Load<Sprite>();
    e = futureC.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.maskTexture = futureC.Result.texture;
    Future<Texture> backF = Singleton<ResourceManager>.GetInstance().Load<Texture>(this.LoadString(this.loginBonus.loginbonus.draw_type_LoginbonusDrawType, "Login_Base"));
    e = backF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((UIWidget) this.back).mainTexture = backF.Result;
    ((UIWidget) this.back).SetDimensions(((UIWidget) this.back).mainTexture.width, ((UIWidget) this.back).mainTexture.height);
    Future<Texture> titleF = Singleton<ResourceManager>.GetInstance().Load<Texture>(this.LoadString(this.loginBonus.loginbonus.draw_type_LoginbonusDrawType, "Login_Title"));
    e = titleF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((UIWidget) this.title).mainTexture = titleF.Result;
    ((UIWidget) this.title).SetDimensions(((UIWidget) this.title).mainTexture.width, ((UIWidget) this.title).mainTexture.height);
    Future<Texture> frameF = Singleton<ResourceManager>.GetInstance().Load<Texture>(this.LoadString(this.loginBonus.loginbonus.draw_type_LoginbonusDrawType, "thum_base"));
    e = frameF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.frame = frameF.Result;
  }

  protected string LoadString(int number, string name)
  {
    return string.Format("Prefabs/startup000_14/image/{0}/{1}", (object) number, (object) name);
  }

  protected void InitNaviChara()
  {
    NGxMaskSpriteWithScale component = this.naviChara.GetComponent<NGxMaskSpriteWithScale>();
    if (this.charaID == 0)
    {
      this.naviChara.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
    }
    else
    {
      component.SetMaskEnable(false);
      component.MainUI2DSprite.sprite2D = this.mainSprite;
      this.naviChara.transform.localScale = new Vector3(this.loginBonusReward.character_scale, this.loginBonusReward.character_scale, 1f);
      this.naviChara.transform.localPosition = new Vector3(this.loginBonusReward.character_x, this.loginBonusReward.character_y, 0.0f);
    }
    component.maskTexture = this.maskTexture;
    component.FitMask();
  }

  public override IEnumerator InitSceneAsync(PlayerLoginBonus lB)
  {
    Startup00014Renzoku startup00014Renzoku = this;
    startup00014Renzoku.GetNextMaskTop.SetActive(false);
    startup00014Renzoku.GetNextMaskLeft.SetActive(false);
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    startup00014Renzoku.next.SetActive(false);
    startup00014Renzoku.loginBonus = lB;
    startup00014Renzoku.Init();
    IEnumerator e = startup00014Renzoku.LoadResource();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Renzoku.InitNaviChara();
    startup00014Renzoku.naviChara.GetComponent<UIWidget>().depth = 100;
    // ISSUE: reference to a compiler-generated method
    int receiveCount = startup00014Renzoku.loginBonusRewardList.Where<LoginbonusReward>(new Func<LoginbonusReward, bool>(startup00014Renzoku.\u003CInitSceneAsync\u003Eb__52_0)).ToList<LoginbonusReward>().Count;
    int num1 = startup00014Renzoku.maxRewardValue / startup00014Renzoku.drawIconValue;
    int num2 = receiveCount / startup00014Renzoku.drawIconValue;
    int startIndex = num2 * startup00014Renzoku.drawIconValue;
    List<int> number = startup00014Renzoku.loginBonusRewardList.Select<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToList<int>();
    if (receiveCount % startup00014Renzoku.drawIconValue == 0 && receiveCount != 0)
    {
      int index = startIndex;
      startup00014Renzoku.rewardReset = true;
      startIndex -= startup00014Renzoku.drawIconValue;
      if (num2 % num1 == 0 && num2 != 0)
      {
        index = 0;
        if (!startup00014Renzoku.loginBonus.loginbonus.is_loop)
          startup00014Renzoku.rewardLast = true;
      }
      if (!startup00014Renzoku.rewardLast)
      {
        e = startup00014Renzoku.newItemList.Initialize(startup00014Renzoku.listIcons, startup00014Renzoku.loginBonusRewardList, number[index]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        startup00014Renzoku.newItemList.ListEnable(false);
      }
    }
    e = startup00014Renzoku.itemList.Initialize(startup00014Renzoku.listIcons, startup00014Renzoku.loginBonusRewardList, number[startIndex]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Renzoku.SetRewardObject(receiveCount);
    startup00014Renzoku.TxtNavi.SetTextLocalize(startup00014Renzoku.loginBonusReward.reward_message);
    startup00014Renzoku.SetStampFrame();
    startup00014Renzoku.OldStamp();
    yield return (object) new WaitForSeconds(0.1f);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    ((Component) startup00014Renzoku).gameObject.SetActive(true);
    startup00014Renzoku.GetNextMaskTop.SetActive(true);
    startup00014Renzoku.GetNextMaskLeft.SetActive(true);
    startup00014Renzoku.fade.onFinished = new List<EventDelegate>();
    startup00014Renzoku.fade.AddOnFinished(new EventDelegate.Callback(startup00014Renzoku.AnimationStart));
    startup00014Renzoku.fade.PlayForward();
  }

  public void AnimationStart()
  {
    GameObject gameObject = Object.Instantiate<GameObject>(this.loginAnime);
    gameObject.transform.parent = ((Component) this).transform;
    gameObject.transform.position = this.mainItemBaseObj.transform.position;
    this.get.SetActive(true);
    this.Play(this.get.transform);
    if (this.charaID == 0)
      Singleton<NGSoundManager>.GetInstance().playVoiceByStringID("VO_9999", "durin_navi_0062");
    else if (this.loginBonusReward.que_name1 != null)
    {
      string[] strArray = this.loginBonusReward.que_name1.Split(',');
      if (strArray.Length > 1)
        Singleton<NGSoundManager>.GetInstance().playVoiceByStringID(strArray[1], strArray[0]);
      else if (this.story)
        Singleton<NGSoundManager>.GetInstance().playVoiceByStringID("VO_" + (object) this.charaID, this.loginBonusReward.que_name1);
      else if (this.unitData.unitVoicePattern != null)
        Singleton<NGSoundManager>.GetInstance().playVoiceByStringID(this.unitData.unitVoicePattern, this.loginBonusReward.que_name1);
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1032");
  }

  public void AnimationEnd()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Object.Destroy((Object) ((Component) this).gameObject);
  }

  public void SetRewardObject(int receiveCount)
  {
    int index1 = (receiveCount - 1) % this.drawIconValue;
    int index2 = (index1 + 1) % this.drawIconValue;
    this.getItem = this.itemList.IconList[index1].Clone(this.mainItemBaseObj.transform);
    this.nextItem = !this.rewardReset || this.newItemList.IconList.Count <= index2 ? this.itemList.IconList[index2].Clone(this.mainItemBaseObj.transform) : this.newItemList.IconList[index2].Clone(this.mainItemBaseObj.transform);
    this.nextItem.SetActive(false);
  }

  private void changeFace(string name)
  {
    NGxFaceSprite component = this.naviChara.GetComponent<NGxFaceSprite>();
    component.UnitID = this.unitID;
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    this.StartCoroutine(component.ChangeFace(name));
  }

  private void onStampSE()
  {
    Singleton<NGSoundManager>.GetInstance().stopSE();
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1033");
  }

  private void SetStampFrame()
  {
    for (int index = 0; index < this.drawIconValue; ++index)
    {
      Startup00014StampFrame component = this.stampFrame.Clone(((Component) this.listIcons[index]).transform).GetComponent<Startup00014StampFrame>();
      component.ChangeFrame(this.frame);
      component.ArrowPosition(this.drawIconValue - 1);
      this.stampFrameList.Add(component);
    }
    this.stampFrameList[this.drawIconValue - 1].ArrowEnable(false);
  }

  protected void OldStamp()
  {
    for (int index = 0; index < this.stampValue - 1; ++index)
    {
      GameObject gameObject = this.finishStamp.Clone(((Component) this.listIcons[index]).transform);
      gameObject.transform.localPosition = new Vector3(0.0f, 5f, 0.0f);
      this.stampList.Add(gameObject);
    }
  }

  private void NowStamp()
  {
    this.onStampSE();
    GameObject gameObject = this.todayStamp.Clone(((Component) this.listIcons[this.stampValue - 1]).transform);
    gameObject.transform.localPosition = new Vector3(0.0f, 5f, 0.0f);
    this.stampList.Add(gameObject);
  }
}
