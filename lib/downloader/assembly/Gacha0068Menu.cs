// Decompiled with JetBrains decompiler
// Type: Gacha0068Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Gacha0068Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtAttack;
  [SerializeField]
  protected UILabel TxtCritical;
  [SerializeField]
  protected UILabel TxtDefense;
  [SerializeField]
  protected UILabel TxtDexterity;
  [SerializeField]
  protected UILabel TxtEvasion;
  [SerializeField]
  protected UILabel TxtExp;
  [SerializeField]
  protected UILabel TxtExpmax;
  [SerializeField]
  protected UILabel TxtFighting;
  [SerializeField]
  protected UILabel TxtHp;
  [SerializeField]
  protected UILabel TxtHpmax;
  [SerializeField]
  protected UILabel TxtJobname;
  [SerializeField]
  protected UILabel TxtMatk;
  [SerializeField]
  protected UILabel TxtMdef;
  [SerializeField]
  protected UILabel TxtCost;
  [SerializeField]
  protected UILabel TxtMovement;
  [SerializeField]
  protected UILabel TxtPrincesstype;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtUnity;
  [SerializeField]
  private UI2DSprite Character;
  [SerializeField]
  private Transform TopStarPos;
  [SerializeField]
  private Transform WeaponPos;
  [SerializeField]
  private Transform HimePos;
  [SerializeField]
  private GameObject NewIcon;
  [SerializeField]
  private GameObject Model;
  [SerializeField]
  private GameObject ModelPrefab;
  [SerializeField]
  private NGTweenGaugeScale EXPGauge;
  [SerializeField]
  private LimitBreakIcon LimitBreak;
  [SerializeField]
  private UIButton btnBack;
  private GameObject MyPrefab;
  private GearKindIcon WeaponIcon;
  [SerializeField]
  protected UI2DSprite rarityStarsIcon;
  [SerializeField]
  private GameObject slcAwakening;
  [SerializeField]
  private UIGrid uiGrid;
  [SerializeField]
  private UISprite slcCountry;
  [SerializeField]
  private UI2DSprite slcInclusion;
  private UnitTypeIcon HimeIcon;
  private PlayerUnit PlayerUnitData;
  private UI3DModel UIModel;
  private bool _enableBackScene;
  private int m_windowHeight;
  private int m_windowWidth;

  public bool EnableBackScene
  {
    get => this._enableBackScene;
    set => this._enableBackScene = value;
  }

  public void IbtnGo() => ((Component) this).gameObject.SetActive(false);

  public virtual void IbtnBack()
  {
    if (!this.EnableBackScene)
      return;
    GachaResultData.ResultData data = GachaResultData.GetInstance().GetData();
    if (data != null && data.additionalItems != null && data.additionalItems.Length != 0)
    {
      if (Object.op_Inequality((Object) this.UIModel, (Object) null))
        ((Component) this.UIModel.ModelCamera).gameObject.SetActive(false);
      Gacha00613Scene.ChangeScene(false, data.is_retry);
    }
    else
    {
      if (((Component) this.btnBack).GetComponent<Collider>().enabled && ((Component) this.btnBack).gameObject.activeSelf)
      {
        Singleton<PopupManager>.GetInstance().onDismiss();
        this.backScene();
      }
      ((Component) this.btnBack).GetComponent<Collider>().enabled = false;
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public UIButton BackSceneButton => this.btnBack;

  public IEnumerator Set(PlayerUnit playerUnit, bool newFlag)
  {
    Gacha0068Menu gacha0068Menu = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    gacha0068Menu.PlayerUnitData = playerUnit;
    ((Component) gacha0068Menu).gameObject.SetActive(true);
    gacha0068Menu.SetStatus();
    gacha0068Menu.SetNewIcon(newFlag);
    gacha0068Menu.LimitBreak.Init(gacha0068Menu.PlayerUnitData.breakthrough_count, gacha0068Menu.PlayerUnitData.unit.breakthrough_limit);
    IEnumerator e = gacha0068Menu.SetCharacterImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = gacha0068Menu.SetWeaponIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = gacha0068Menu.SetRarityStar();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) gacha0068Menu.slcCountry, (Object) null))
    {
      ((Component) gacha0068Menu.slcCountry).gameObject.SetActive(false);
      if (playerUnit.unit.country_attribute.HasValue)
      {
        ((Component) gacha0068Menu.slcCountry).gameObject.SetActive(true);
        playerUnit.unit.SetCuntrySpriteName(ref gacha0068Menu.slcCountry);
      }
    }
    if (Object.op_Inequality((Object) gacha0068Menu.slcInclusion, (Object) null))
    {
      ((Component) gacha0068Menu.slcInclusion).gameObject.SetActive(false);
      if (playerUnit.unit.inclusion_ip.HasValue)
      {
        ((Component) gacha0068Menu.slcInclusion).gameObject.SetActive(true);
        e = playerUnit.unit.SetInclusionIP(gacha0068Menu.slcInclusion);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    gacha0068Menu.uiGrid.Reposition();
    e = gacha0068Menu.SetCharacterModel();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    ((Component) gacha0068Menu.btnBack).GetComponent<Collider>().enabled = true;
  }

  private IEnumerator LateFitMask()
  {
    while (Object.op_Equality((Object) this.MyPrefab, (Object) null))
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.01f);
    this.MyPrefab.GetComponent<NGxMaskSpriteWithScale>().FitMask();
  }

  private void SetStatus()
  {
    Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnitData);
    string name1 = this.PlayerUnitData.unit.name;
    string name2 = this.PlayerUnitData.unit.job.name;
    string name3 = this.PlayerUnitData.unit_type.name;
    int maxLevel = this.PlayerUnitData.max_level;
    int level = this.PlayerUnitData.level;
    int hp1 = nonBattleParameter.Hp;
    int hp2 = nonBattleParameter.Hp;
    int cost = this.PlayerUnitData.cost;
    int move = this.PlayerUnitData.move;
    int unityInt = this.PlayerUnitData.unityInt;
    int physicalAttack = nonBattleParameter.PhysicalAttack;
    int physicalDefense = nonBattleParameter.PhysicalDefense;
    int magicAttack = nonBattleParameter.MagicAttack;
    int magicDefense = nonBattleParameter.MagicDefense;
    int hit = nonBattleParameter.Hit;
    int critical = nonBattleParameter.Critical;
    int evasion = nonBattleParameter.Evasion;
    int combat = nonBattleParameter.Combat;
    this.SetText(this.TxtTitle, name1);
    this.SetText(this.TxtJobname, name2);
    this.SetText(this.TxtPrincesstype, name3);
    this.SetText(this.TxtExpmax, " /" + maxLevel.ToString());
    this.SetText(this.TxtExp, level.ToString());
    this.SetText(this.TxtHpmax, " /" + hp1.ToString());
    this.SetText(this.TxtHp, hp2.ToString());
    this.SetText(this.TxtCost, cost.ToString());
    this.SetText(this.TxtMovement, move.ToString());
    this.SetText(this.TxtUnity, unityInt > 0 ? "[ffff00]" + unityInt.ToString() : unityInt.ToString());
    this.SetText(this.TxtAttack, physicalAttack.ToString());
    this.SetText(this.TxtDefense, physicalDefense.ToString());
    this.SetText(this.TxtMatk, magicAttack.ToString());
    this.SetText(this.TxtMdef, magicDefense.ToString());
    this.SetText(this.TxtDexterity, hit.ToString());
    this.SetText(this.TxtCritical, critical.ToString());
    this.SetText(this.TxtEvasion, evasion.ToString());
    this.SetText(this.TxtFighting, combat.ToString());
    int max = this.PlayerUnitData.exp_next + this.PlayerUnitData.exp - 1;
    int n = this.PlayerUnitData.exp;
    if (n > max)
      n = max;
    this.EXPGauge.setValue(n, max, false);
  }

  private void SetNewIcon(bool flag) => this.NewIcon.SetActive(flag);

  private void SetText(UILabel lavel, string text) => lavel.SetTextLocalize(text.ToConverter());

  private IEnumerator SetRarityStar()
  {
    this.slcAwakening.SetActive(false);
    if (this.PlayerUnitData.unit.awake_unit_flag)
      this.slcAwakening.SetActive(true);
    RarityIcon.SetRarity(this.PlayerUnitData.unit, this.rarityStarsIcon, true);
    yield break;
  }

  private IEnumerator SetWeaponIcon()
  {
    if (Object.op_Equality((Object) this.WeaponIcon, (Object) null))
    {
      Future<GameObject> SetGearPrefab = Res.Icons.GearKindIcon.Load<GameObject>();
      IEnumerator e = SetGearPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.WeaponIcon = SetGearPrefab.Result.Clone(this.WeaponPos).GetComponent<GearKindIcon>();
      ((UIWidget) this.WeaponIcon.iconSprite).depth = 43;
      SetGearPrefab = (Future<GameObject>) null;
    }
    this.WeaponIcon.Init(this.PlayerUnitData.unit.kind, this.PlayerUnitData.GetElement());
  }

  private IEnumerator SetCharacterImage()
  {
    if (Object.op_Inequality((Object) this.MyPrefab, (Object) null))
      Object.Destroy((Object) this.MyPrefab);
    int depth = ((UIWidget) ((Component) this.Character).GetComponent<UI2DSprite>()).depth;
    Future<Sprite> spritef = this.PlayerUnitData.unit.LoadFullSprite();
    IEnumerator e = spritef.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite sprite2 = spritef.Result;
    Future<GameObject> prefabf = this.PlayerUnitData.unit.LoadMypage();
    e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.MyPrefab = prefabf.Result.Clone(((Component) this.Character).transform);
    UI2DSprite component = this.MyPrefab.GetComponent<UI2DSprite>();
    component.sprite2D = sprite2;
    ((UIWidget) component).depth = depth;
    Future<Sprite> maskf = Res.Units.Shared.unit_full.Load<Sprite>();
    e = maskf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = maskf.Result;
    this.MyPrefab.GetComponent<NGxMaskSpriteWithScale>().maskTexture = result.texture;
  }

  private IEnumerator SetCharacterModel()
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.UIModel, (Object) null))
    {
      Future<GameObject> fModel = Res.Prefabs.gacha006_8.slc_3DModel.Load<GameObject>();
      e = fModel.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ModelPrefab = fModel.Result;
      this.UIModel = this.ModelPrefab.Clone(this.Model.transform).GetComponent<UI3DModel>();
      this.UIModel.isNotLotate = true;
      fModel = (Future<GameObject>) null;
    }
    else
      ((Component) this.UIModel.ModelCamera).gameObject.SetActive(true);
    this.UIModel.SetScale = 220f;
    e = this.UIModel.Unit(this.PlayerUnitData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
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
      this.StartCoroutine(this.Set(this.PlayerUnitData, true));
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    base.Update();
  }
}
