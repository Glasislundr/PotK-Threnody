// Decompiled with JetBrains decompiler
// Type: PopupXLevelUp
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
[AddComponentMenu("Popup/Unit/PopupXLevelUp")]
public class PopupXLevelUp : BackButtonPopupBase
{
  [SerializeField]
  private Transform lnkIcon_;
  [SerializeField]
  private UIScrollView scrollView_;
  [SerializeField]
  private UIGrid grid_;
  [SerializeField]
  private UILabel txtLevel_;
  [SerializeField]
  private UILabel txtResultLevel_;
  [SerializeField]
  private UILabel txtMaxLevel_;
  [SerializeField]
  private UILabel txtNextExp_;
  [SerializeField]
  private UILabel txtExp_;
  [SerializeField]
  private UIButton btnClear_;
  [SerializeField]
  private UIButton btnExecute_;
  [SerializeField]
  private NGTweenGaugeScale expGauge_;
  private PlayerUnit target_;
  private Action<PlayerUnit> result_;
  private Action onClose_;
  private int baseLevel_;
  private int baseLevelFromExp_;
  private int baseLevelToExp_;
  private int baseExp_;
  private int maxLevel_;
  private int maxExp_;
  private Dictionary<PlayerMaterialUnit, int> dicQuantity_;
  private Dictionary<PlayerMaterialUnit, PopupXLevelUpExpEditor> dicEditor_;
  private Dictionary<PlayerMaterialUnit, MaterialXLevelExp> dicExp_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit004_2/popup_XLvUp_MaterialSelect").Load<GameObject>();
  }

  public static void show(
    GameObject prefab,
    PlayerUnit playerUnit,
    Action<PlayerUnit> result,
    Action eventClose)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true);
    PopupXLevelUp component = gameObject.GetComponent<PopupXLevelUp>();
    component.setTopObject(gameObject);
    component.target_ = playerUnit;
    component.result_ = result;
    component.onClose_ = eventClose;
  }

  public bool isBoostExp { get; private set; }

  public float scaleExp { get; private set; } = 1f;

  private IEnumerator Start()
  {
    PopupXLevelUp popup = this;
    ((Component) popup).gameObject.GetComponent<UIRect>().alpha = 0.0f;
    float? xexperience = Singleton<NGGameDataManager>.GetInstance().BoostInfo.XExperience;
    popup.isBoostExp = xexperience.HasValue && (double) xexperience.Value > 1.0;
    popup.scaleExp = popup.isBoostExp ? xexperience.Value : 1f;
    Future<GameObject> ldIcon1 = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    yield return (object) ldIcon1.Wait();
    UnitIcon icon = ldIcon1.Result.Clone(popup.lnkIcon_).GetComponent<UnitIcon>();
    icon.RarityCenter();
    yield return (object) icon.SetPlayerUnit(popup.target_, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
    icon.SetIconBoxCollider(false);
    ldIcon1 = (Future<GameObject>) null;
    icon = (UnitIcon) null;
    popup.dicQuantity_ = new Dictionary<PlayerMaterialUnit, int>();
    popup.dicEditor_ = new Dictionary<PlayerMaterialUnit, PopupXLevelUpExpEditor>();
    popup.dicExp_ = new Dictionary<PlayerMaterialUnit, MaterialXLevelExp>();
    PopupXLevelUp popupXlevelUp = popup;
    PlayerUnitXJobStatus xJobStatus = popup.target_.x_job_status;
    int totalExp = xJobStatus != null ? xJobStatus.total_exp : 0;
    popupXlevelUp.baseExp_ = totalExp;
    UnitXLevel data = UnitXLevel.expToData(popup.baseExp_);
    popup.baseLevel_ = data.level;
    popup.baseLevelFromExp_ = data.from_exp;
    popup.baseLevelToExp_ = data.to_exp;
    popup.maxLevel_ = popup.target_.max_x_level;
    // ISSUE: reference to a compiler-generated method
    UnitXLevel unitXlevel = Array.Find<UnitXLevel>(MasterData.UnitXLevelList, new Predicate<UnitXLevel>(popup.\u003CStart\u003Eb__33_0));
    if (unitXlevel == null)
    {
      unitXlevel = ((IEnumerable<UnitXLevel>) MasterData.UnitXLevelList).Last<UnitXLevel>();
      popup.maxLevel_ = unitXlevel.level;
      Debug.LogError((object) string.Format("XLevel 用経験値テーブル上限がLevel={0} です!!", (object) popup.maxLevel_));
    }
    popup.maxExp_ = unitXlevel.from_exp;
    popup.txtLevel_.SetTextLocalize(popup.baseLevel_);
    popup.txtMaxLevel_.SetTextLocalize("/" + (object) popup.maxLevel_);
    ldIcon1 = PopupXLevelUpExpEditor.createLoader();
    yield return (object) ldIcon1.Wait();
    Future<GameObject> ldIcon2 = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    yield return (object) ldIcon2.Wait();
    UnitUnit[] masters = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.is_exp_material && MasterData.MaterialXLevelExp.ContainsKey(x.ID))).OrderBy<UnitUnit, int>((Func<UnitUnit, int>) (y => MasterData.MaterialXLevelExp[y.ID].point)).ThenBy<UnitUnit, int>((Func<UnitUnit, int>) (z => z.ID)).ToArray<UnitUnit>();
    PlayerMaterialUnit[] playerMaterials = SMManager.Get<PlayerMaterialUnit[]>();
    for (int n = 0; n < masters.Length; ++n)
    {
      PlayerMaterialUnit key = Array.Find<PlayerMaterialUnit>(playerMaterials, (Predicate<PlayerMaterialUnit>) (x => x._unit == masters[n].ID && x.quantity > 0));
      if (key != null)
      {
        PopupXLevelUpExpEditor component = ldIcon1.Result.Clone(((Component) popup.grid_).transform).GetComponent<PopupXLevelUpExpEditor>();
        popup.dicEditor_[key] = component;
        popup.dicExp_[key] = MasterData.MaterialXLevelExp[key._unit];
        component.resetMaxVolume(Mathf.Min(key.quantity, component.maxVolume));
        yield return (object) component.initialize(ldIcon2.Result, popup, key);
      }
    }
    // ISSUE: method pointer
    popup.grid_.onReposition = new UIGrid.OnReposition((object) popup, __methodptr(\u003CStart\u003Eb__33_4));
    popup.grid_.Reposition();
    ldIcon1 = (Future<GameObject>) null;
    ldIcon2 = (Future<GameObject>) null;
    playerMaterials = (PlayerMaterialUnit[]) null;
    popup.updateInfo(true);
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popup).gameObject);
  }

  public int onChangeValue(PlayerMaterialUnit item, int value)
  {
    int num1 = value;
    int exp = this.dicQuantity_.Sum<KeyValuePair<PlayerMaterialUnit, int>>((Func<KeyValuePair<PlayerMaterialUnit, int>, int>) (x => x.Key == item ? 0 : x.Value * this.dicExp_[x.Key].point));
    int point = this.dicExp_[item].point;
    int num2 = point * num1;
    if (this.maxExp_ < this.baseExp_ + this.boostExp(exp + num2))
    {
      if (this.maxExp_ <= this.baseExp_ + this.boostExp(exp))
      {
        num1 = 0;
      }
      else
      {
        Decimal scaleExp = (Decimal) this.scaleExp;
        Decimal num3 = (Decimal) (this.maxExp_ - this.baseExp_) - (Decimal) exp * scaleExp;
        num1 = !(num3 <= 0M) ? (int) Decimal.Ceiling(num3 / ((Decimal) point * scaleExp)) : 0;
      }
    }
    this.dicQuantity_[item] = num1;
    this.updateInfo();
    return num1;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.onClose_();
  }

  public void onClickedClear()
  {
    if (this.IsPush)
      return;
    this.dicQuantity_.Clear();
    foreach (KeyValuePair<PlayerMaterialUnit, PopupXLevelUpExpEditor> keyValuePair in this.dicEditor_)
      keyValuePair.Value.setVolume(0, true);
    this.updateInfo(true);
  }

  public void onClickedExecute()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doWaitLoad(PopupXLevelUpConfirm.createLoader(), (Action<GameObject>) (prefab =>
    {
      Tuple<PlayerMaterialUnit, int>[] array = this.dicQuantity_.Where<KeyValuePair<PlayerMaterialUnit, int>>((Func<KeyValuePair<PlayerMaterialUnit, int>, bool>) (x => x.Value > 0)).Select<KeyValuePair<PlayerMaterialUnit, int>, Tuple<PlayerMaterialUnit, int>>((Func<KeyValuePair<PlayerMaterialUnit, int>, Tuple<PlayerMaterialUnit, int>>) (y => Tuple.Create<PlayerMaterialUnit, int>(y.Key, y.Value))).ToArray<Tuple<PlayerMaterialUnit, int>>();
      PopupXLevelUpConfirm.show(prefab, array, new Action<bool>(this.onEndConfirm));
    })));
  }

  private IEnumerator doWaitLoad(Future<GameObject> ldPrefab, Action<GameObject> onLoadedPrefab)
  {
    yield return (object) ldPrefab.Wait();
    onLoadedPrefab(ldPrefab.Result);
  }

  private void onEndConfirm(bool bOk)
  {
    if (bOk)
      this.StartCoroutine(this.doLevelUp());
    else
      this.IsPush = false;
  }

  private IEnumerator doLevelUp()
  {
    PopupXLevelUp popupXlevelUp = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    KeyValuePair<PlayerMaterialUnit, int>[] array = popupXlevelUp.dicQuantity_.Where<KeyValuePair<PlayerMaterialUnit, int>>((Func<KeyValuePair<PlayerMaterialUnit, int>, bool>) (a => a.Value > 0)).ToArray<KeyValuePair<PlayerMaterialUnit, int>>();
    int[] material_player_material_unit_ids = new int[array.Length];
    int[] material_player_material_unit_nums = new int[array.Length];
    for (int index = 0; index < array.Length; ++index)
    {
      material_player_material_unit_ids[index] = array[index].Key.id;
      material_player_material_unit_nums[index] = array[index].Value;
    }
    Future<WebAPI.Response.UnitXLevelUp> webApi = WebAPI.UnitXLevelUp(popupXlevelUp.target_.id, material_player_material_unit_ids, material_player_material_unit_nums, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<PopupManager>.GetInstance().closeAll();
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeScene();
    }));
    yield return (object) webApi.Wait();
    if (webApi.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      // ISSUE: reference to a compiler-generated method
      popupXlevelUp.result_(Array.Find<PlayerUnit>(webApi.Result.player_units, new Predicate<PlayerUnit>(popupXlevelUp.\u003CdoLevelUp\u003Eb__40_2)));
    }
  }

  private void updateInfo(bool bImmediate = false)
  {
    int num1 = this.boostExp(this.dicQuantity_.Sum<KeyValuePair<PlayerMaterialUnit, int>>((Func<KeyValuePair<PlayerMaterialUnit, int>, int>) (d => this.dicExp_[d.Key].point * d.Value)));
    int num2 = num1 + this.baseExp_;
    this.txtExp_.SetTextLocalize(num2);
    ((UIButtonColor) this.btnClear_).isEnabled = num1 > 0;
    ((UIButtonColor) this.btnExecute_).isEnabled = num1 > 0;
    int exp = Mathf.Min(num2, this.maxExp_);
    int level = UnitXLevel.expToLevel(exp);
    this.txtResultLevel_.SetTextLocalize(level);
    int nextLevel = level < this.maxLevel_ ? level + 1 : this.maxLevel_;
    this.txtNextExp_.SetTextLocalize(Mathf.Max(Array.Find<UnitXLevel>(MasterData.UnitXLevelList, (Predicate<UnitXLevel>) (x => x.level == nextLevel)).from_exp - exp, 0));
    nextLevel = this.baseLevel_ < this.maxLevel_ ? this.baseLevel_ + 1 : this.baseLevel_;
    int max = this.baseLevelToExp_ - this.baseLevelFromExp_;
    int n = max > 0 ? Mathf.Min(max, exp - this.baseLevelFromExp_) : 0;
    if (bImmediate)
      this.expGauge_.setValue(n, max, false);
    else
      this.expGauge_.setValue(n, max, duration: 0.5f);
  }

  public int boostExp(int exp) => (int) Decimal.Floor((Decimal) exp * (Decimal) this.scaleExp);

  public int multiply(MaterialXLevelExp exp, int quantity) => this.boostExp(exp.point * quantity);
}
