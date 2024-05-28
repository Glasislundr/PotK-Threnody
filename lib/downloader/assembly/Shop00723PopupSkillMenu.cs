// Decompiled with JetBrains decompiler
// Type: Shop00723PopupSkillMenu
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
public class Shop00723PopupSkillMenu : BackButtonMenuBase
{
  [Header("Scroll Objects")]
  [SerializeField]
  private GameObject mainView_;
  [SerializeField]
  private ScrollViewSpecifyBounds mainScroll_;
  [SerializeField]
  private UICenterOnChild uiCentering_;
  [SerializeField]
  private Shop00723PopupSkillContainer container_;
  [SerializeField]
  private UIDragScrollView hdragScrollView_;
  [SerializeField]
  private GameObject arrowLeft_;
  [SerializeField]
  private GameObject arrowRight_;
  [Header("Buttons")]
  [SerializeField]
  private GameObject dirHimeType;
  [SerializeField]
  private SpreadColorButton btnKing;
  [SerializeField]
  private SpreadColorButton btnLife;
  [SerializeField]
  private SpreadColorButton btnAttack;
  [SerializeField]
  private SpreadColorButton btnMagic;
  [SerializeField]
  private SpreadColorButton btnDefense;
  [SerializeField]
  private SpreadColorButton btnAgility;
  [SerializeField]
  private UIGrid buttonsGrid;
  private List<Shop00723PopupSkillContainer> allContainer = new List<Shop00723PopupSkillContainer>();
  private SelectTicketChoices ticketChoice;
  private Shop00723Menu menu_;
  private SelectTicketSelectSample sample_;
  private bool isStarted_;
  private bool isDelayInitialize_ = true;
  private Shop00723PopupSkillMenu.UnitTree unitTree_;
  private List<Shop00723PopupSkillMenu.UnitTree> listUnit_;
  private GameObject[] hscrollItems_;
  private int current_;
  private GameObject currentItem_;
  private GameObject pageTrigger_;
  private const int SINGLE_SKILLCHANGE = 1;
  private const int NUM_INITIALIZED = 1;
  private const int NUM_NO_HSCROLL = 1;
  private float _lastX;

  private Shop00723PopupSkillMenu.UnitTree makeUnitTree(int id)
  {
    List<int> listId = new List<int>();
    return this.makeUnitTree(0, id, (Shop00723PopupSkillMenu.UnitTree) null, listId);
  }

  private Shop00723PopupSkillMenu.UnitTree makeUnitTree(
    int nest,
    int id,
    Shop00723PopupSkillMenu.UnitTree parent,
    List<int> listId)
  {
    UnitUnit unit;
    if (!MasterData.UnitUnit.TryGetValue(id, out unit))
      return (Shop00723PopupSkillMenu.UnitTree) null;
    Shop00723PopupSkillMenu.UnitTree ret = new Shop00723PopupSkillMenu.UnitTree(nest, unit, parent);
    if (listId.Contains(id))
      return ret;
    listId.Add(id);
    UnitEvolutionPattern[] evolutionPattern = unit.EvolutionPattern;
    if (((IEnumerable<UnitEvolutionPattern>) evolutionPattern).Any<UnitEvolutionPattern>())
    {
      ++nest;
      ret.setChilds(((IEnumerable<UnitEvolutionPattern>) evolutionPattern).Where<UnitEvolutionPattern>((Func<UnitEvolutionPattern, bool>) (u => this.isNormalUnit(u.target_unit_UnitUnit))).Select<UnitEvolutionPattern, Shop00723PopupSkillMenu.UnitTree>((Func<UnitEvolutionPattern, Shop00723PopupSkillMenu.UnitTree>) (eu => this.makeUnitTree(nest, eu.target_unit_UnitUnit, ret, listId))).Where<Shop00723PopupSkillMenu.UnitTree>((Func<Shop00723PopupSkillMenu.UnitTree, bool>) (tr => tr != null)).ToArray<Shop00723PopupSkillMenu.UnitTree>());
    }
    return ret;
  }

  private bool isNormalUnit(int id)
  {
    UnitUnit unitUnit;
    return MasterData.UnitUnit.TryGetValue(id, out unitUnit) && unitUnit.IsNormalUnit;
  }

  private List<Shop00723PopupSkillMenu.UnitTree> unitTreeToList(Shop00723PopupSkillMenu.UnitTree ut)
  {
    List<Shop00723PopupSkillMenu.UnitTree> unitTreeList = new List<Shop00723PopupSkillMenu.UnitTree>();
    this.unitTreeToList(unitTreeList, ut);
    return unitTreeList.OrderBy<Shop00723PopupSkillMenu.UnitTree, int>((Func<Shop00723PopupSkillMenu.UnitTree, int>) (u => u.nest_)).ToList<Shop00723PopupSkillMenu.UnitTree>();
  }

  private void unitTreeToList(
    List<Shop00723PopupSkillMenu.UnitTree> unitList,
    Shop00723PopupSkillMenu.UnitTree ut)
  {
    unitList.Add(ut);
    if (ut.childs_ == null || ut.childs_.Length == 0)
      return;
    foreach (Shop00723PopupSkillMenu.UnitTree child in ut.childs_)
      this.unitTreeToList(unitList, child);
  }

  private void setSubName(List<Shop00723PopupSkillMenu.UnitTree> units)
  {
    int nest = 0;
    while (true)
    {
      List<Shop00723PopupSkillMenu.UnitTree> list = units.Where<Shop00723PopupSkillMenu.UnitTree>((Func<Shop00723PopupSkillMenu.UnitTree, bool>) (u => u.nest_ == nest)).ToList<Shop00723PopupSkillMenu.UnitTree>();
      if (list.Count != 0)
      {
        if (list.Count > 1)
        {
          char ch = 'a';
          foreach (Shop00723PopupSkillMenu.UnitTree unitTree in list)
            unitTree.setSubName(string.Format("{0}", (object) ch++));
        }
        ++nest;
      }
      else
        break;
    }
  }

  public void initialize(
    Shop00723Menu menu,
    SelectTicketSelectSample unitSample,
    SelectTicketChoices choice)
  {
    ((Component) this).GetComponent<UIRect>().alpha = 0.0f;
    this.isStarted_ = false;
    this.isDelayInitialize_ = true;
    this.menu_ = menu;
    this.sample_ = unitSample;
    this.unitTree_ = this.makeUnitTree(this.sample_.reward_id);
    this.listUnit_ = this.unitTreeToList(this.unitTree_);
    this.setSubName(this.listUnit_);
    this.current_ = 0;
    this.currentItem_ = (GameObject) null;
    ((Behaviour) this.mainScroll_).enabled = false;
    this.ticketChoice = choice;
    this.setActiveArrow();
  }

  private IEnumerator Start()
  {
    Shop00723PopupSkillMenu shop00723PopupSkillMenu = this;
    UIRect component1 = shop00723PopupSkillMenu.mainView_.GetComponent<UIRect>();
    AnchorTargetAdjustment component2 = shop00723PopupSkillMenu.mainView_.GetComponent<AnchorTargetAdjustment>();
    component1.SetAnchor(((Component) component2).transform.GetParentInFind(component2.targetParentName));
    component1.Update();
    ((Component) shop00723PopupSkillMenu.container_).transform.localPosition = Vector3.zero;
    UIRect component3 = ((Component) shop00723PopupSkillMenu.container_).GetComponent<UIRect>();
    component3.ResetAnchors();
    component3.Update();
    component3.SetAnchor((Transform) null);
    UIRect component4 = shop00723PopupSkillMenu.container_.dirTop_.GetComponent<UIRect>();
    component4.ResetAnchors();
    component4.Update();
    component4.SetAnchor((Transform) null);
    List<GameObject> gameObjectList = new List<GameObject>()
    {
      ((Component) shop00723PopupSkillMenu.container_).gameObject
    };
    if (shop00723PopupSkillMenu.listUnit_.Count > 1)
    {
      float num = (float) shop00723PopupSkillMenu.mainView_.GetComponent<UIWidget>().width + shop00723PopupSkillMenu.uiCentering_.nextPageThreshold;
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(0.0f, ((Component) shop00723PopupSkillMenu.container_).transform.localPosition.y, 0.0f);
      for (int index = 1; index < shop00723PopupSkillMenu.listUnit_.Count; ++index)
      {
        GameObject gameObject = ((Component) shop00723PopupSkillMenu.container_).gameObject.Clone(((Component) shop00723PopupSkillMenu.mainScroll_).transform);
        vector3.x = num * (float) index;
        gameObject.transform.localPosition = vector3;
        shop00723PopupSkillMenu.mainScroll_.AddBound(gameObject.GetComponent<UIWidget>());
        gameObjectList.Add(gameObject);
      }
    }
    shop00723PopupSkillMenu.hscrollItems_ = gameObjectList.ToArray();
    IEnumerator e = shop00723PopupSkillMenu.coInitUnit(((Component) shop00723PopupSkillMenu.container_).gameObject, shop00723PopupSkillMenu.unitTree_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    shop00723PopupSkillMenu.uiCentering_.CenterOn(((Component) shop00723PopupSkillMenu.container_).transform);
    shop00723PopupSkillMenu.pageTrigger_ = ((Component) shop00723PopupSkillMenu.container_).gameObject;
    e = shop00723PopupSkillMenu.coInitializeDelay();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) shop00723PopupSkillMenu).gameObject);
    shop00723PopupSkillMenu.isStarted_ = true;
  }

  private void OnDestroy()
  {
    if (!this.isDelayInitialize_)
      return;
    this.StopCoroutine("coInitializeDelay");
  }

  private IEnumerator coInitializeDelay()
  {
    this.isDelayInitialize_ = true;
    yield return (object) new WaitForEndOfFrame();
    for (int index = 1; index < this.hscrollItems_.Length; ++index)
    {
      IEnumerator e = this.coInitUnit(this.hscrollItems_[index], this.listUnit_[index]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    ((Behaviour) this.mainScroll_).enabled = this.hscrollItems_.Length > 1;
    this.isDelayInitialize_ = false;
  }

  private IEnumerator coInitUnit(GameObject goList, Shop00723PopupSkillMenu.UnitTree utree)
  {
    Shop00723PopupSkillContainer container = goList.GetComponent<Shop00723PopupSkillContainer>();
    this.allContainer.Add(container);
    container.unitID_ = utree.unit_.ID;
    UILabel txtRarity = container.txtRarity_;
    string format = Consts.GetInstance().SHOP_00723_TITLE_RARITY;
    int num = utree.unit_.rarity.index + 1;
    string str = num.ToString() + (string.IsNullOrEmpty(utree.subName_) ? "" : utree.subName_);
    string text = string.Format(format, (object) str);
    txtRarity.SetTextLocalize(text);
    int idxScroll = 0;
    UnitBattleSkillOrigin[] battleSkillOriginArray = utree.unit_.MakeLeaderSkillOrigins();
    GameObject go;
    if (battleSkillOriginArray.Length != 0)
    {
      go = this.menu_.prefabLeader.Clone();
      go.SetActive(false);
      container.scroll_.Add(go);
      num = idxScroll++;
      go.GetComponent<Shop00723PopupSkillLeader>().initialize(battleSkillOriginArray);
      if (battleSkillOriginArray.Length > 1)
      {
        List<UnitBattleSkillOrigin> list = ((IEnumerable<UnitBattleSkillOrigin>) battleSkillOriginArray).ToList<UnitBattleSkillOrigin>();
        while (true)
        {
          list.RemoveAt(0);
          if (list.Any<UnitBattleSkillOrigin>())
          {
            go = this.menu_.prefabLeader.Clone();
            go.SetActive(false);
            container.scroll_.Add(go);
            num = idxScroll++;
            go.GetComponent<Shop00723PopupSkillLeader>().initialize(list.ToArray());
          }
          else
            break;
        }
      }
    }
    foreach (UnitBattleSkillOrigin[] source in (IEnumerable<UnitBattleSkillOrigin[]>) ((IEnumerable<UnitBattleSkillOrigin[]>) utree.unit_.MakeSkillOrigins()).OrderBy<UnitBattleSkillOrigin[], int>((Func<UnitBattleSkillOrigin[], int>) (sa =>
    {
      UnitBattleSkillOrigin battleSkillOrigin = ((IEnumerable<UnitBattleSkillOrigin>) sa).First<UnitBattleSkillOrigin>();
      if (battleSkillOrigin.IsOriginAwake)
        return 2;
      return battleSkillOrigin.skill_.skill_type != BattleskillSkillType.magic ? 0 : 1;
    })))
    {
      if (((IEnumerable<UnitBattleSkillOrigin>) source).Any<UnitBattleSkillOrigin>((Func<UnitBattleSkillOrigin, bool>) (sd => sd.skill_ != null)))
      {
        List<UnitBattleSkillOrigin> slst = ((IEnumerable<UnitBattleSkillOrigin>) source).ToList<UnitBattleSkillOrigin>();
        do
        {
          go = this.menu_.prefabSkill.Clone();
          go.SetActive(false);
          Shop00723PopupSkillSkill ss = go.GetComponent<Shop00723PopupSkillSkill>();
          IEnumerator e = ss.coInitialize(this.menu_, slst.ToArray());
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          UnitSkill unitSkill = ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).FirstOrDefault<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit.ID == container.unitID_ && x.skill_BattleskillSkill == ss.skillID));
          if (unitSkill != null && unitSkill.unit_type != 0)
            container.scroll_.Insert(idxScroll, go);
          else
            container.scroll_.Add(go);
          slst.RemoveAt(0);
        }
        while (slst.Count > 0);
        slst = (List<UnitBattleSkillOrigin>) null;
      }
    }
    foreach (GameObject gameObject in container.scroll_.Arr)
      gameObject.SetActive(true);
    container.scroll_.ResolvePosition();
    if (this.ticketChoice == null)
      this.ViewTabs(false, ((Component) container).gameObject);
    else
      this.EnableButtons();
  }

  protected override void Update()
  {
    this.CheckScrollHorizontalMoving();
    base.Update();
    if (this.isDelayInitialize_)
      return;
    GameObject centeredObject = this.uiCentering_.centeredObject;
    if (Object.op_Inequality((Object) this.pageTrigger_, (Object) centeredObject))
    {
      this.playPageSE();
      this.pageTrigger_ = centeredObject;
    }
    if (Object.op_Equality((Object) this.currentItem_, (Object) centeredObject))
      return;
    SpringPanel component = ((Component) this.mainScroll_).GetComponent<SpringPanel>();
    if (Object.op_Inequality((Object) component, (Object) null) && ((Behaviour) component).enabled)
      return;
    this.currentItem_ = centeredObject;
    for (int index = 0; index < this.hscrollItems_.Length; ++index)
    {
      if (Object.op_Equality((Object) this.hscrollItems_[index], (Object) centeredObject))
      {
        this.current_ = index;
        break;
      }
    }
    this.updateArrow(this.current_);
  }

  private void playPageSE()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.playSE("SE_1005");
  }

  public override void onBackButton() => this.onClickClose();

  public void onClickClose()
  {
    if (!this.isStarted_ || this.IsPushAndSet())
      return;
    this.menu_.onClosedSkill();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onClickedLeft()
  {
    if (this.hscrollItems_ == null)
      return;
    int index = this.current_ - 1;
    if (index < 0)
      return;
    this.uiCentering_.CenterOn(this.hscrollItems_[index].transform);
    this.setActiveArrow();
  }

  public void onClickedRight()
  {
    if (this.hscrollItems_ == null)
      return;
    int index = this.current_ + 1;
    if (index >= this.hscrollItems_.Length)
      return;
    this.uiCentering_.CenterOn(this.hscrollItems_[index].transform);
    this.setActiveArrow();
  }

  private void updateArrow(int index)
  {
    this.setActiveArrow(index - 1 >= 0, index + 1 < this.listUnit_.Count);
  }

  private void setActiveArrow(bool activeLeft = false, bool activeRight = false)
  {
    this.setActiveArrow(this.arrowLeft_, activeLeft);
    this.setActiveArrow(this.arrowRight_, activeRight);
  }

  private void setActiveArrow(GameObject go, bool isactive)
  {
    ((UIButtonColor) go.GetComponent<UIButton>()).isEnabled = isactive;
  }

  public void onKingButton()
  {
    ((Behaviour) this.mainScroll_).enabled = false;
    this.SetVisibleSkill(Shop00723PopupSkillMenu.buttonPushed.king);
    this.SetIconColor(Shop00723PopupSkillMenu.buttonPushed.king);
    ((Behaviour) this.mainScroll_).enabled = true;
  }

  public void onLifeButton()
  {
    ((Behaviour) this.mainScroll_).enabled = false;
    this.SetVisibleSkill(Shop00723PopupSkillMenu.buttonPushed.life);
    this.SetIconColor(Shop00723PopupSkillMenu.buttonPushed.life);
    ((Behaviour) this.mainScroll_).enabled = true;
  }

  public void onAttackButton()
  {
    ((Behaviour) this.mainScroll_).enabled = false;
    this.SetVisibleSkill(Shop00723PopupSkillMenu.buttonPushed.attack);
    this.SetIconColor(Shop00723PopupSkillMenu.buttonPushed.attack);
    ((Behaviour) this.mainScroll_).enabled = true;
  }

  public void onMagicButton()
  {
    ((Behaviour) this.mainScroll_).enabled = false;
    this.SetVisibleSkill(Shop00723PopupSkillMenu.buttonPushed.magic);
    this.SetIconColor(Shop00723PopupSkillMenu.buttonPushed.magic);
    ((Behaviour) this.mainScroll_).enabled = true;
  }

  public void onDefenseButton()
  {
    ((Behaviour) this.mainScroll_).enabled = false;
    this.SetVisibleSkill(Shop00723PopupSkillMenu.buttonPushed.defense);
    this.SetIconColor(Shop00723PopupSkillMenu.buttonPushed.defense);
    ((Behaviour) this.mainScroll_).enabled = true;
  }

  public void onAgilityButton()
  {
    ((Behaviour) this.mainScroll_).enabled = false;
    this.SetVisibleSkill(Shop00723PopupSkillMenu.buttonPushed.agility);
    this.SetIconColor(Shop00723PopupSkillMenu.buttonPushed.agility);
    ((Behaviour) this.mainScroll_).enabled = true;
  }

  private void EnableButtons()
  {
    ((Component) this.btnKing).gameObject.SetActive(false);
    ((Component) this.btnLife).gameObject.SetActive(false);
    ((Component) this.btnAttack).gameObject.SetActive(false);
    ((Component) this.btnMagic).gameObject.SetActive(false);
    ((Component) this.btnDefense).gameObject.SetActive(false);
    ((Component) this.btnAgility).gameObject.SetActive(false);
    int[] unitTypes = this.ticketChoice.unit_types;
    if (((IEnumerable<int>) unitTypes).Contains<int>(1))
      ((Component) this.btnKing).gameObject.SetActive(true);
    if (((IEnumerable<int>) unitTypes).Contains<int>(2))
      ((Component) this.btnLife).gameObject.SetActive(true);
    if (((IEnumerable<int>) unitTypes).Contains<int>(3))
      ((Component) this.btnAttack).gameObject.SetActive(true);
    if (((IEnumerable<int>) unitTypes).Contains<int>(4))
      ((Component) this.btnMagic).gameObject.SetActive(true);
    if (((IEnumerable<int>) unitTypes).Contains<int>(5))
      ((Component) this.btnDefense).gameObject.SetActive(true);
    if (((IEnumerable<int>) unitTypes).Contains<int>(6))
      ((Component) this.btnAgility).gameObject.SetActive(true);
    this.buttonsGrid.Reposition();
    switch (unitTypes[0])
    {
      case 1:
        this.onKingButton();
        break;
      case 2:
        this.onLifeButton();
        break;
      case 3:
        this.onAttackButton();
        break;
      case 4:
        this.onMagicButton();
        break;
      case 5:
        this.onDefenseButton();
        break;
      case 6:
        this.onAgilityButton();
        break;
    }
  }

  private void SetIconColor(Shop00723PopupSkillMenu.buttonPushed btn)
  {
    this.btnKing.SetColor(((UIButtonColor) this.btnKing).disabledColor);
    this.btnLife.SetColor(((UIButtonColor) this.btnLife).disabledColor);
    this.btnAttack.SetColor(((UIButtonColor) this.btnAttack).disabledColor);
    this.btnMagic.SetColor(((UIButtonColor) this.btnMagic).disabledColor);
    this.btnDefense.SetColor(((UIButtonColor) this.btnDefense).disabledColor);
    this.btnAgility.SetColor(((UIButtonColor) this.btnAgility).disabledColor);
    switch (btn)
    {
      case Shop00723PopupSkillMenu.buttonPushed.king:
        this.btnKing.SetColor(Color.white);
        break;
      case Shop00723PopupSkillMenu.buttonPushed.life:
        this.btnLife.SetColor(Color.white);
        break;
      case Shop00723PopupSkillMenu.buttonPushed.attack:
        this.btnAttack.SetColor(Color.white);
        break;
      case Shop00723PopupSkillMenu.buttonPushed.magic:
        this.btnMagic.SetColor(Color.white);
        break;
      case Shop00723PopupSkillMenu.buttonPushed.defense:
        this.btnDefense.SetColor(Color.white);
        break;
      case Shop00723PopupSkillMenu.buttonPushed.agility:
        this.btnAgility.SetColor(Color.white);
        break;
    }
  }

  private void SetVisibleSkill(Shop00723PopupSkillMenu.buttonPushed btn)
  {
    foreach (Shop00723PopupSkillContainer popupSkillContainer in this.allContainer)
    {
      Shop00723PopupSkillContainer c = popupSkillContainer;
      bool showIt = false;
      foreach (GameObject gameObject in c.scroll_.Arr)
      {
        gameObject.SetActive(false);
        if (Object.op_Inequality((Object) gameObject.GetComponent<Shop00723PopupSkillLeader>(), (Object) null))
        {
          gameObject.SetActive(true);
        }
        else
        {
          Shop00723PopupSkillSkill popupSkill = gameObject.GetComponent<Shop00723PopupSkillSkill>();
          if (Object.op_Inequality((Object) popupSkill, (Object) null))
          {
            UnitSkill unitSkill = ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).FirstOrDefault<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit.ID == c.unitID_ && x.skill_BattleskillSkill == popupSkill.skillID));
            if (unitSkill != null)
            {
              if (unitSkill.unit_type == 0)
                gameObject.SetActive(true);
              if (btn == (Shop00723PopupSkillMenu.buttonPushed) unitSkill.unit_type)
              {
                showIt = true;
                gameObject.SetActive(true);
              }
            }
            else
              gameObject.SetActive(true);
          }
        }
      }
      this.ViewTabs(showIt, ((Component) c).gameObject);
      c.scroll_.Resize();
      c.scroll_.ResolvePosition();
    }
  }

  private void ViewTabs(bool showIt, GameObject container)
  {
    this.dirHimeType.SetActive(showIt);
    UIPanel component1 = ((Component) container.transform.Find("ScrollView")).GetComponent<UIPanel>();
    UIPanel component2 = ((Component) container.transform.Find("dir_VScrollBar")).GetComponent<UIPanel>();
    if (showIt)
    {
      ((UIRect) component1).SetAnchor(container, 0, 0, 0, -62);
      ((UIRect) component2).SetAnchor(container, 0, -8, 0, -66);
    }
    else
    {
      ((UIRect) component1).SetAnchor(container, 0, 0, 0, 15);
      ((UIRect) component2).SetAnchor(container, 0, -8, 0, 9);
    }
  }

  private void CheckScrollHorizontalMoving()
  {
    if ((double) ((Component) this.mainScroll_).transform.position.x != (double) this._lastX)
    {
      this.StateAllButtons(false);
      this._lastX = ((Component) this.mainScroll_).transform.position.x;
    }
    else
      this.StateAllButtons(true);
  }

  private void StateAllButtons(bool active)
  {
    ((Collider) ((Component) this.btnKing).GetComponent<BoxCollider>()).enabled = active;
    ((Collider) ((Component) this.btnLife).GetComponent<BoxCollider>()).enabled = active;
    ((Collider) ((Component) this.btnAttack).GetComponent<BoxCollider>()).enabled = active;
    ((Collider) ((Component) this.btnMagic).GetComponent<BoxCollider>()).enabled = active;
    ((Collider) ((Component) this.btnDefense).GetComponent<BoxCollider>()).enabled = active;
    ((Collider) ((Component) this.btnAgility).GetComponent<BoxCollider>()).enabled = active;
  }

  private enum buttonPushed
  {
    king = 1,
    life = 2,
    attack = 3,
    magic = 4,
    defense = 5,
    agility = 6,
  }

  private class UnitTree
  {
    public int nest_ { get; private set; }

    public string subName_ { get; private set; }

    public UnitUnit unit_ { get; private set; }

    public Shop00723PopupSkillMenu.UnitTree parent_ { get; private set; }

    public Shop00723PopupSkillMenu.UnitTree[] childs_ { get; private set; }

    public UnitTree(int nest, UnitUnit unit, Shop00723PopupSkillMenu.UnitTree parent)
    {
      this.nest_ = nest;
      this.unit_ = unit;
      this.parent_ = parent;
    }

    public void setChilds(Shop00723PopupSkillMenu.UnitTree[] childs) => this.childs_ = childs;

    public void setSubName(string sub) => this.subName_ = sub;
  }
}
