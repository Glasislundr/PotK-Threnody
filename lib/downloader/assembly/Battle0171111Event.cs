// Decompiled with JetBrains decompiler
// Type: Battle0171111Event
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Battle0171111Event : BattleMonoBehaviour
{
  public GameObject DialogConteiner;
  public Transform skillTargetParent1;
  public Transform skillTargetParent2;
  public Transform weaponKindParent;
  public GearKind weaponKind;
  public BattleskillGenre? skillTargetID1;
  public BattleskillGenre? skillTargetID2;
  public string skillName;
  public string skillDesc;
  private string skillCategoryInfo;
  [SerializeField]
  private UILabel label_SkillName;
  [SerializeField]
  private UILabel label_SkillDesc;
  [SerializeField]
  private UILabel label_RemainTurn;
  private GameObject skillTgtIcon1;
  private GameObject skillTgtIcon2;
  private CommonElementIcon elementIcon;
  private BattleSkillIcon skillIcon;
  [SerializeField]
  private GameObject slc_Skillproperty_blank;
  [SerializeField]
  private GameObject dir_Lv;
  [SerializeField]
  private GameObject slc_SkillLv;
  [SerializeField]
  private UISprite slc_SkillLvMax;
  [SerializeField]
  private UILabel label_SkillLv;
  [SerializeField]
  private UILabel label_SkillLvMax;
  [SerializeField]
  private UILabel label_CategoryDescription;
  [SerializeField]
  private UISprite bgSprite;
  [SerializeField]
  private GameObject slc_SkillLvMaxSea;
  public bool isPopupManagedInAwake;
  private bool isShop;
  private ItemDetailPopupBase itemDetailPopupBase;
  private PlayerUnit targetUnit;
  [SerializeField]
  private UIButton btnChange;
  private bool isChangeButtonHover;
  private BattleskillSkill displaySkill;
  private bool isShow;
  private bool is_popup_managed;

  public bool isPopupManaged
  {
    get => this.is_popup_managed;
    set
    {
      Collider component1 = ((Component) ((Component) this).transform.parent).gameObject.GetComponent<Collider>();
      if (Object.op_Inequality((Object) component1, (Object) null))
        component1.enabled = value;
      UIButton component2 = ((Component) ((Component) this).transform.parent).gameObject.GetComponent<UIButton>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        ((Behaviour) component2).enabled = value;
      this.is_popup_managed = value;
    }
  }

  private void Awake() => this.isPopupManaged = this.isPopupManagedInAwake;

  private void OnEnable()
  {
  }

  public void Show()
  {
    if (this.DialogConteiner.activeInHierarchy && this.isShow)
      return;
    this.isShow = true;
    this.DialogConteiner.SetActive(true);
    this.StopCoroutine("setIcons");
    this.StartCoroutine("setIcons");
    ((IEnumerable<UITweener>) ((Component) this).gameObject.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (c =>
    {
      ((Behaviour) c).enabled = true;
      c.onFinished.Clear();
      c.PlayForward();
    }));
  }

  public IEnumerator ShowAsync()
  {
    Battle0171111Event battle0171111Event = this;
    if (!battle0171111Event.DialogConteiner.activeInHierarchy || !battle0171111Event.isShow)
    {
      battle0171111Event.isShow = true;
      battle0171111Event.DialogConteiner.SetActive(true);
      battle0171111Event.StopCoroutine("setIcons");
      IEnumerator e = battle0171111Event.setIcons();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((IEnumerable<UITweener>) ((Component) battle0171111Event).gameObject.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (c =>
      {
        ((Behaviour) c).enabled = true;
        c.onFinished.Clear();
        c.PlayForward();
      }));
    }
  }

  public void Hide()
  {
    if (!this.DialogConteiner.activeInHierarchy && !this.isShow)
      return;
    this.isShow = false;
    UITweener[] tweens = ((Component) this).gameObject.GetComponentsInChildren<UITweener>();
    if (tweens.Length == 0)
      return;
    int finishCount = 0;
    EventDelegate.Callback onFinish = (EventDelegate.Callback) (() =>
    {
      if (!Object.op_Inequality((Object) this.DialogConteiner, (Object) null) || ++finishCount < tweens.Length)
        return;
      this.DialogConteiner.SetActive(false);
    });
    ((IEnumerable<UITweener>) tweens).ForEach<UITweener>((Action<UITweener>) (c =>
    {
      c.onFinished.Clear();
      c.AddOnFinished(onFinish);
      c.PlayReverse();
    }));
  }

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    BoxCollider componentInChildren2 = ((Component) trans).GetComponentInChildren<BoxCollider>();
    if (!Object.op_Inequality((Object) componentInChildren1, (Object) null))
      return icon;
    ((UIWidget) componentInChildren1).SetDimensions((int) componentInChildren2.size.x, (int) componentInChildren2.size.y);
    UI2DSprite ui2Dsprite = componentInChildren1;
    ((UIWidget) ui2Dsprite).depth = ((UIWidget) ui2Dsprite).depth + 150;
    return icon;
  }

  private void Update()
  {
    if ((this.is_popup_managed || !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2) && (double) Input.GetAxis("Mouse ScrollWheel") == 0.0) && (!Object.op_Inequality((Object) this.battleManager, (Object) null) || !this.battleManager.isOvo || this.battleManager.timeLimit != 0) && (!Object.op_Inequality((Object) this.battleManager, (Object) null) || !this.battleManager.isOvo || this.env.core.phaseState.state != BL.Phase.finalize && this.env.core.phaseState.state != BL.Phase.pvp_start_init) || this.targetUnit != (PlayerUnit) null && Object.op_Inequality((Object) this.btnChange, (Object) null) && this.isChangeButtonHover)
      return;
    this.close();
  }

  public void close()
  {
    this.Hide();
    if (!this.isShop)
      return;
    this.itemDetailPopupBase.IbtnNo();
  }

  public void onButtonClicked()
  {
  }

  public void setTargetUnit(PlayerUnit playerUnit) => this.targetUnit = playerUnit;

  public void onChangeHoverOver() => this.isChangeButtonHover = true;

  public void onChangeHoverOut() => this.isChangeButtonHover = false;

  public void onChangeClicked()
  {
    if (this.targetUnit == (PlayerUnit) null)
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Unit004ExtraskillEquipListScene.changeScene(true, this.targetUnit);
    this.close();
  }

  private IEnumerator setIcons()
  {
    this.label_SkillName.SetText(this.skillName);
    this.label_SkillDesc.SetText(this.skillDesc);
    if (Object.op_Inequality((Object) this.label_CategoryDescription, (Object) null))
      this.label_CategoryDescription.SetTextLocalize(this.skillCategoryInfo);
    if (Object.op_Inequality((Object) this.bgSprite, (Object) null) && Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      if (string.IsNullOrEmpty(this.skillCategoryInfo))
      {
        this.bgSprite.spriteName = "slc_popup_base_b.png__GUI__unit_detail_sea__unit_detail_sea_prefab";
        ((UIWidget) this.bgSprite).height = 173;
      }
      else
      {
        this.bgSprite.spriteName = "slc_popup_base_c.png__GUI__unit_detail_sea__unit_detail_sea_prefab";
        ((UIWidget) this.bgSprite).height = 238;
      }
    }
    ((Component) this.weaponKindParent).gameObject.SetActive(true);
    Future<GameObject> commonElementPrefabF;
    IEnumerator e;
    if (this.displaySkill.skill_type == BattleskillSkillType.magic)
    {
      if (Object.op_Equality((Object) this.elementIcon, (Object) null))
      {
        commonElementPrefabF = Res.Icons.CommonElementIcon.Load<GameObject>();
        e = commonElementPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.elementIcon = this.createIcon(commonElementPrefabF.Result, this.weaponKindParent).GetComponent<CommonElementIcon>();
        commonElementPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Inequality((Object) this.elementIcon, (Object) null))
        ((Component) this.elementIcon).gameObject.SetActive(true);
      if (Object.op_Inequality((Object) this.skillIcon, (Object) null))
        ((Component) this.skillIcon).gameObject.SetActive(false);
      this.elementIcon.Init(this.displaySkill.element);
    }
    else if (this.displaySkill.skill_type == BattleskillSkillType.leader)
    {
      if (Object.op_Inequality((Object) this.elementIcon, (Object) null))
        ((Component) this.elementIcon).gameObject.SetActive(false);
      if (Object.op_Inequality((Object) this.skillIcon, (Object) null))
        ((Component) this.skillIcon).gameObject.SetActive(false);
    }
    else
    {
      if (Object.op_Equality((Object) this.skillIcon, (Object) null))
      {
        commonElementPrefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
        e = commonElementPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.skillIcon = this.createIcon(commonElementPrefabF.Result, this.weaponKindParent).GetComponent<BattleSkillIcon>();
        commonElementPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Inequality((Object) this.elementIcon, (Object) null))
        ((Component) this.elementIcon).gameObject.SetActive(false);
      if (Object.op_Inequality((Object) this.skillIcon, (Object) null))
      {
        ((Component) this.skillIcon).gameObject.SetActive(true);
        e = this.skillIcon.Init(this.displaySkill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    if (Object.op_Equality((Object) this.skillTgtIcon1, (Object) null) && Object.op_Equality((Object) this.skillTgtIcon2, (Object) null))
    {
      commonElementPrefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
      e = commonElementPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = commonElementPrefabF.Result;
      this.skillTgtIcon1 = this.createIcon(result, this.skillTargetParent1);
      this.skillTgtIcon2 = this.createIcon(result, this.skillTargetParent2);
      commonElementPrefabF = (Future<GameObject>) null;
    }
    this.skillTgtIcon1.GetComponent<SkillGenreIcon>().Init(this.skillTargetID1);
    this.skillTgtIcon2.GetComponent<SkillGenreIcon>().Init(this.skillTargetID2);
  }

  public void setClosePopup(ItemDetailPopupBase itm)
  {
    this.isShop = true;
    this.itemDetailPopupBase = itm;
  }

  public void setData(BattleskillSkill skill)
  {
    this.displaySkill = skill;
    this.skillDesc = skill.description;
    this.skillName = skill.name;
    this.skillTargetID1 = skill.genre1;
    this.skillTargetID2 = skill.genre2;
    this.weaponKind = MasterData.GearKind[3];
    this.setSkillLv(0, skill.upper_level);
    this.disableRemainTurn();
  }

  public void setTextStatusFrame(string text) => this.skillCategoryInfo = text ?? "";

  public void setData(BattleskillSkill skill, string addDescription)
  {
    this.displaySkill = skill;
    this.skillDesc = skill.description;
    this.skillName = skill.name;
    string str = Consts.Format(Consts.GetInstance().popup_004_ExtraSkill_affection_condition_category, (IDictionary) new Hashtable()
    {
      {
        (object) "condition",
        (object) AwakeSkillCategory.GetEquipableText(skill.awake_skill_category_id)
      }
    });
    this.skillCategoryInfo = !string.IsNullOrEmpty(addDescription) ? str + addDescription : "";
    this.skillTargetID1 = skill.genre1;
    this.skillTargetID2 = skill.genre2;
    this.weaponKind = MasterData.GearKind[3];
    this.setSkillLv(0, skill.upper_level);
    this.disableRemainTurn();
  }

  public void setSkillLv(int lv, int lv_upper = 0)
  {
    string str = lv.ToString();
    string text = lv_upper.ToString();
    if (lv < 1)
      str = "-";
    if (lv_upper < 1)
    {
      str = "-";
      text = "-";
    }
    if (lv < 1 && lv_upper > 0)
    {
      if (Object.op_Inequality((Object) this.slc_SkillLv, (Object) null))
        this.slc_SkillLv.gameObject.SetActive(false);
      if (Object.op_Inequality((Object) this.slc_SkillLvMax, (Object) null))
        ((Component) this.slc_SkillLvMax).gameObject.SetActive(true);
      if (Object.op_Inequality((Object) this.slc_SkillLvMaxSea, (Object) null))
        this.slc_SkillLvMaxSea.gameObject.SetActive(true);
      ((Component) this.label_SkillLv).gameObject.SetActive(false);
      if (!Object.op_Inequality((Object) this.label_SkillLvMax, (Object) null))
        return;
      ((Component) this.label_SkillLvMax).gameObject.SetActive(true);
      this.label_SkillLvMax.SetText(text);
    }
    else
    {
      if (Object.op_Inequality((Object) this.slc_SkillLv, (Object) null))
        this.slc_SkillLv.gameObject.SetActive(true);
      if (Object.op_Inequality((Object) this.slc_SkillLvMax, (Object) null))
        ((Component) this.slc_SkillLvMax).gameObject.SetActive(false);
      if (Object.op_Inequality((Object) this.slc_SkillLvMaxSea, (Object) null))
        this.slc_SkillLvMaxSea.gameObject.SetActive(false);
      ((Component) this.label_SkillLv).gameObject.SetActive(true);
      if (Object.op_Inequality((Object) this.label_SkillLvMax, (Object) null))
        ((Component) this.label_SkillLvMax).gameObject.SetActive(false);
      this.label_SkillLv.SetText(str + "/" + text);
    }
  }

  public void enableSkillLv(bool enable) => this.dir_Lv.SetActive(enable);

  public void setSkillProperty(bool flg)
  {
    if (Object.op_Inequality((Object) this.slc_Skillproperty_blank, (Object) null))
      this.slc_Skillproperty_blank.SetActive(flg);
    ((Component) this.skillTargetParent1).gameObject.SetActive(flg);
    ((Component) this.skillTargetParent2).gameObject.SetActive(flg);
  }

  public void setRemainTurn(int turn)
  {
    if (!Object.op_Inequality((Object) this.label_RemainTurn, (Object) null))
      return;
    ((Component) this.label_RemainTurn).gameObject.SetActive(true);
    this.label_RemainTurn.SetTextLocalize(Consts.Format(Consts.GetInstance().AILMENT_REMAINTURN, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (turn),
        (object) turn
      }
    }));
  }

  public void disableRemainTurn()
  {
    if (!Object.op_Inequality((Object) this.label_RemainTurn, (Object) null))
      return;
    ((Component) this.label_RemainTurn).gameObject.SetActive(false);
  }

  public void cleanContainerTopComponents()
  {
    System.Type[] deltypes = new System.Type[5]
    {
      typeof (TweenAlpha),
      typeof (UIButton),
      typeof (BoxCollider),
      typeof (UI2DSprite),
      typeof (UIWidget)
    };
    foreach (Object @object in ((IEnumerable<Component>) this.DialogConteiner.GetComponents<Component>()).Where<Component>((Func<Component, bool>) (c =>
    {
      System.Type ct = c.GetType();
      return ((IEnumerable<System.Type>) deltypes).Any<System.Type>((Func<System.Type, bool>) (t => t.Equals(ct)));
    })))
      Object.Destroy(@object);
  }
}
