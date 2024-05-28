// Decompiled with JetBrains decompiler
// Type: Guide01122Menu
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
public class Guide01122Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGHorizontalScrollParts informationScrollView;
  [SerializeField]
  private SpriteSelectDirectButton[] groupSprites;
  [SerializeField]
  private GameObject slc_GroupBase;
  private TweenHeight tween_GroupBaseOpen;
  private TweenHeight tween_GroupBaseClose;
  [SerializeField]
  private GameObject dir_GroupSprite;
  private TweenPosition tween_GroupSpriteDirOpen;
  private TweenPosition tween_GroupSpriteDirClose;
  [SerializeField]
  private UIButton IbtnGroupTabIdle;
  [SerializeField]
  private GameObject dir_GroupPressed;
  private bool isGroupOpen = true;
  private bool isGroupTween;
  private int dispGroupCount;
  private float groupSpriteHeight = 46.5f;
  private int groupBaseHeightInit = 36;
  private GameObject refUnitObject;
  private const int TWEEN_GROUPID_START = 100;
  private const int TWEEN_GROUPID_END = 101;
  [SerializeField]
  private GameObject slcSelectEvolution;
  [SerializeField]
  private UIButton ibtnEvolution;
  [SerializeField]
  private UI2DSprite rarityBtnSprite;
  private GameObject floatingRarityDialogObject;
  [SerializeField]
  private GameObject slcAwakening;
  [SerializeField]
  private UISprite slcCountry;
  public UILabel txt_CharacterName;
  public UILabel txt_JobName;
  public UILabel txt_number;
  public UILabel txt_DefeatNum;
  public GameObject DirCharacter;
  public UI2DSprite rarityStarsIcon;
  public GearKindIcon GearKindIcon;
  public GuideUnitDetailScrollViewSkill detailSkill;
  public GuideUnitDetailScrollViewJob detailJob;
  private UnitUnit unit_;
  private UnitUnit[] commonUnitList;
  private PlayerUnitHistory[] historyList;
  private int job_id;
  private MasterDataTable.UnitJob jobData;
  private PlayerUnitSkills koyuDuel;
  private PlayerUnitSkills koyuMulti;
  private int m_windowHeight;
  private int m_windowWidth;
  private RenderTextureRecoveryUtil util;

  public virtual void IbtnZoom()
  {
    if (this.IsPushAndSet())
      return;
    Unit0043Scene.changeScene(true, this.unit_, this.job_id, true, this.koyuDuel, this.koyuMulti, true);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSoundManager>.GetInstance().stopVoice();
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  private void OnDestroy() => Singleton<NGSoundManager>.GetInstance().stopVoice();

  public virtual void updateMenu(UnitUnit unit, int job_id)
  {
    this.StartCoroutine(this.onStartSceneAsync(unit, false, job_id));
  }

  public IEnumerator onStartSceneAsync(UnitUnit unit, bool voiceFlag, int select_job_id = 0)
  {
    this.unit_ = unit;
    this.koyuDuel = this.koyuMulti = (PlayerUnitSkills) null;
    this.commonUnitList = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.history_group_number == unit.history_group_number && x.character.category == UnitCategory.player)).OrderBy<UnitUnit, int>((Func<UnitUnit, int>) (x => x.ID)).ToArray<UnitUnit>();
    this.historyList = SMManager.Get<PlayerUnitHistory[]>();
    PlayerUnitHistory history = ((IEnumerable<PlayerUnitHistory>) this.historyList).FirstOrDefault<PlayerUnitHistory>((Func<PlayerUnitHistory, bool>) (x => x.unit_id == unit.ID));
    if (history == null)
    {
      Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
    }
    else
    {
      this.job_id = 0;
      this.jobData = (MasterDataTable.UnitJob) null;
      List<int> jobIdList = unit.getClassChangeJobIdList();
      if (jobIdList.Count > 4)
        jobIdList = jobIdList.GetRange(0, 4);
      if (select_job_id != 0)
      {
        if (select_job_id != unit.job_UnitJob)
          this.job_id = select_job_id;
      }
      else if (jobIdList.Any<int>((Func<int, bool>) (x => x != 0)))
      {
        jobIdList.Reverse();
        this.job_id = jobIdList.FirstOrDefault<int>((Func<int, bool>) (x => ((IEnumerable<int?>) history.job_ids).Contains<int?>(new int?(x))));
      }
      if (this.job_id == 0)
        this.job_id = unit.job_UnitJob;
      if (!MasterData.UnitJob.TryGetValue(this.job_id, out this.jobData))
      {
        Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
      }
      else
      {
        this.SetUnitName(unit);
        this.SetJobName(unit);
        PlayerUnit playerUnit = PlayerUnit.create_by_unitunit(unit);
        playerUnit.job_id = this.jobData.ID;
        this.SetUnitRarity(playerUnit);
        this.SetUnitGearType(unit, this.job_id);
        this.SetNumber(unit);
        this.SetDefeat(unit, this.historyList);
        IEnumerator e = this.SetUnitImg(unit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = this.detailSkill.init(unit, history);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.koyuDuel = this.detailSkill.koyuDuel;
        this.koyuMulti = this.detailSkill.koyuMulti;
        e = this.detailJob.init(unit, this.jobData);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Implicit((Object) this.slc_GroupBase))
        {
          foreach (TweenHeight componentsInChild in this.slc_GroupBase.GetComponentsInChildren<TweenHeight>())
          {
            if (((UITweener) componentsInChild).tweenGroup == 100)
              this.tween_GroupBaseOpen = componentsInChild;
            if (((UITweener) componentsInChild).tweenGroup == 101)
              this.tween_GroupBaseClose = componentsInChild;
          }
          foreach (TweenPosition componentsInChild in this.dir_GroupSprite.GetComponentsInChildren<TweenPosition>())
          {
            if (((UITweener) componentsInChild).tweenGroup == 100)
              this.tween_GroupSpriteDirOpen = componentsInChild;
            if (((UITweener) componentsInChild).tweenGroup == 101)
              this.tween_GroupSpriteDirClose = componentsInChild;
          }
          this.DisplayGroupLogo(unit);
        }
        RarityIcon.SetRarity(playerUnit, this.rarityBtnSprite, false, true, true);
        if (this.commonUnitList.Length < 2 && jobIdList.Count < 2)
        {
          this.slcSelectEvolution.SetActive(false);
          ((Behaviour) this.ibtnEvolution).enabled = false;
        }
        if (voiceFlag && Object.op_Implicit((Object) Singleton<NGSoundManager>.GetInstanceOrNull()))
        {
          Singleton<NGSoundManager>.GetInstance().stopVoice();
          Singleton<NGSoundManager>.GetInstance().playVoiceByID(unit.unitVoicePattern, 42);
        }
      }
    }
  }

  public void onEndScene()
  {
  }

  public IEnumerator onEndSceneAsync()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
    {
      instance.stopVoice();
      yield break;
    }
  }

  private void DisplayGroupLogo(UnitUnit playerUnit)
  {
    if (this.groupSprites == null)
      return;
    int index = 0;
    ((IEnumerable<SpriteSelectDirectButton>) this.groupSprites).ForEach<SpriteSelectDirectButton>((Action<SpriteSelectDirectButton>) (x => ((Component) x).gameObject.SetActive(false)));
    UnitGroup groupInfo = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == playerUnit.ID));
    if (groupInfo != null)
    {
      UnitGroupLargeCategory groupLargeCategory = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).FirstOrDefault<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x => x.ID == groupInfo.group_large_category_id_UnitGroupLargeCategory));
      UnitGroupSmallCategory groupSmallCategory = ((IEnumerable<UnitGroupSmallCategory>) MasterData.UnitGroupSmallCategoryList).FirstOrDefault<UnitGroupSmallCategory>((Func<UnitGroupSmallCategory, bool>) (x => x.ID == groupInfo.group_small_category_id_UnitGroupSmallCategory));
      UnitGroupClothingCategory clothingCategory1 = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).FirstOrDefault<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x => x.ID == groupInfo.group_clothing_category_id_UnitGroupClothingCategory));
      UnitGroupClothingCategory clothingCategory2 = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).FirstOrDefault<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x => x.ID == groupInfo.group_clothing_category_id_2_UnitGroupClothingCategory));
      UnitGroupGenerationCategory generationCategory = ((IEnumerable<UnitGroupGenerationCategory>) MasterData.UnitGroupGenerationCategoryList).FirstOrDefault<UnitGroupGenerationCategory>((Func<UnitGroupGenerationCategory, bool>) (x => x.ID == groupInfo.group_generation_category_id_UnitGroupGenerationCategory));
      if (groupLargeCategory == null && groupSmallCategory == null && clothingCategory1 == null && clothingCategory2 == null && generationCategory == null)
        return;
      if (groupLargeCategory != null && groupLargeCategory.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(groupLargeCategory.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (groupSmallCategory != null && groupSmallCategory.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(groupSmallCategory.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (clothingCategory1 != null && clothingCategory1.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(clothingCategory1.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (clothingCategory2 != null && clothingCategory2.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(clothingCategory2.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (generationCategory != null && generationCategory.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(generationCategory.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
    }
    if (!Object.op_Inequality((Object) this.slc_GroupBase, (Object) null))
      return;
    this.dispGroupCount = index;
    this.tween_GroupBaseOpen.to = (int) this.getGroupHeightTarget();
    this.tween_GroupBaseClose.from = (int) this.getGroupHeightTarget();
    this.tween_GroupSpriteDirOpen.to.y = this.getGroupPositionTarget();
    this.tween_GroupSpriteDirOpen.from.y = this.getGroupPositionInit();
    this.tween_GroupSpriteDirClose.to.y = this.getGroupPositionInit();
    this.tween_GroupSpriteDirClose.from.y = this.getGroupPositionTarget();
    this.setGroupPos();
    if (this.dispGroupCount != 0)
      return;
    ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
    this.dir_GroupPressed.SetActive(false);
  }

  public void IbtnGroupOpen()
  {
    if (this.isGroupTween || this.isGroupOpen)
      return;
    ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
    this.dir_GroupPressed.SetActive(true);
    NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), 100);
    this.isGroupOpen = true;
    this.isGroupTween = true;
  }

  public void onFinishedGroupOpen() => this.isGroupTween = false;

  public void IbtnGroupClose()
  {
    if (this.isGroupTween || !this.isGroupOpen)
      return;
    NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), 101);
    this.isGroupOpen = false;
    this.isGroupTween = true;
  }

  public void onFinishedGroupClose()
  {
    if (!this.isGroupTween)
      return;
    if (this.dispGroupCount > 0)
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(true);
    else
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
    this.dir_GroupPressed.SetActive(false);
    this.isGroupTween = false;
  }

  private void setGroupPos()
  {
    if (this.isGroupOpen)
    {
      ((UIWidget) this.slc_GroupBase.GetComponent<UISprite>()).height = (int) this.getGroupHeightTarget();
      this.dir_GroupSprite.transform.localPosition = new Vector3(this.dir_GroupSprite.transform.localPosition.x, this.getGroupPositionTarget(), this.dir_GroupSprite.transform.localPosition.z);
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
      this.dir_GroupPressed.SetActive(true);
    }
    else
    {
      ((UIWidget) this.slc_GroupBase.GetComponent<UISprite>()).height = this.groupBaseHeightInit;
      this.dir_GroupSprite.transform.localPosition = new Vector3(this.dir_GroupSprite.transform.localPosition.x, this.getGroupPositionInit(), this.dir_GroupSprite.transform.localPosition.z);
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(true);
      this.dir_GroupPressed.SetActive(false);
    }
    this.isGroupTween = false;
  }

  private float getGroupHeightTarget()
  {
    return (float) ((double) this.groupBaseHeightInit + (double) this.groupSpriteHeight * (double) this.dispGroupCount + 8.0);
  }

  private float getGroupPositionTarget() => 0.0f;

  private float getGroupPositionInit()
  {
    return (float) (-(double) this.groupSpriteHeight * (double) this.dispGroupCount - 8.0);
  }

  public void onSelectRarityBtn()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openRarityDetailDialog());
  }

  public IEnumerator openRarityDetailDialog()
  {
    Guide01122Menu menu = this;
    Future<GameObject> loader = new ResourceObject("Prefabs/guide01122/SelectRarity").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = loader.Result.Clone();
    prefab.GetComponent<GuideRaritySelectDialog>().Init(menu, menu.unit_, menu.commonUnitList, menu.historyList, menu.job_id);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    menu.StartCoroutine(menu.IsPushOff());
  }

  public void SetUnitName(UnitUnit unit)
  {
    if (string.IsNullOrEmpty(unit.formal_name))
      this.txt_CharacterName.SetText(unit.name);
    else
      this.txt_CharacterName.SetText(unit.formal_name);
  }

  public void SetJobName(UnitUnit unit) => this.txt_JobName.SetTextLocalize(this.jobData.name);

  public void SetNumber(UnitUnit unit)
  {
    this.txt_number.SetTextLocalize("NO." + (unit.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public void SetDefeat(UnitUnit unit, PlayerUnitHistory[] historyList)
  {
    int defeat = 0;
    ((IEnumerable<PlayerUnitHistory>) historyList).ForEach<PlayerUnitHistory>((Action<PlayerUnitHistory>) (obj =>
    {
      if (obj.unit_id != unit.ID)
        return;
      defeat += obj.defeat;
    }));
    if (defeat > 99999)
      defeat = 99999;
    this.txt_DefeatNum.SetTextLocalize(defeat);
  }

  public void SetUnitRarity(PlayerUnit playerUnit)
  {
    RarityIcon.SetRarity(playerUnit, this.rarityStarsIcon, true, true);
    this.slcAwakening.SetActive(false);
    if (!playerUnit.unit.awake_unit_flag)
      return;
    this.slcAwakening.SetActive(true);
  }

  public void SetUnitGearType(UnitUnit unit, int job_id)
  {
    if (!Object.op_Implicit((Object) this.GearKindIcon))
      return;
    this.GearKindIcon.Init(unit.kind, unit.GetElement());
  }

  public IEnumerator SetUnitImg(UnitUnit unit)
  {
    if (Object.op_Inequality((Object) this.refUnitObject, (Object) null))
    {
      Object.Destroy((Object) this.refUnitObject);
      this.refUnitObject = (GameObject) null;
    }
    Future<GameObject> loader1 = new ResourceObject("Prefabs/guide01122/dyn_Character").Load<GameObject>();
    IEnumerator e = loader1.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.refUnitObject = loader1.Result.Clone(this.DirCharacter.transform);
    UI2DSprite dynCharacter = this.refUnitObject.GetComponent<UI2DSprite>();
    loader1 = (Future<GameObject>) null;
    Future<Sprite> loader2 = unit.job_UnitJob == this.job_id ? unit.LoadFullSprite() : unit.LoadJobFullSprite(this.job_id);
    e = loader2.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    dynCharacter.sprite2D = loader2.Result;
    loader2 = (Future<Sprite>) null;
  }

  protected override void Update()
  {
    if (this.m_windowHeight == 0 || this.m_windowWidth == 0)
    {
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    else if (this.m_windowHeight != Screen.height || this.m_windowWidth != Screen.width)
    {
      this.StartCoroutine(this.onStartSceneAsync(this.unit_, false));
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    base.Update();
    if (!Object.op_Inequality((Object) this.util, (Object) null))
      return;
    this.util.FixRenderTexture();
  }
}
