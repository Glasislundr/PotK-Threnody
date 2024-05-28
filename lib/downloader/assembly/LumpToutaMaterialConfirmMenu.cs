// Decompiled with JetBrains decompiler
// Type: LumpToutaMaterialConfirmMenu
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
[AddComponentMenu("Scenes/Unit/LumpTouta/MaterialConfirmMenu")]
public class LumpToutaMaterialConfirmMenu : UnitSelectMenuBase
{
  private const int COLUM_COUNT = 5;
  private const int MAX_SELECT = 30;
  [SerializeField]
  private GameObject baseWidthLine;
  [SerializeField]
  private UILabel useZeny;
  [SerializeField]
  private UILabel selectedCount;
  [SerializeField]
  private SpreadColorButton DecisionButton;
  private List<PlayerUnit> selectedBasePlayerUnits = new List<PlayerUnit>();
  private List<List<UnitIconInfo>> selectedMaterialUnitIconInfos = new List<List<UnitIconInfo>>();
  private Dictionary<UnitIconInfo, int> unityValueUpMaxCounts = new Dictionary<UnitIconInfo, int>();
  private Dictionary<int, GameObject> cacheWidthLines = new Dictionary<int, GameObject>();
  private Dictionary<PlayerUnit, LumpToutaMaterialConfirmMenu.ShareWork> dicShareWorks;
  private GameObject toutaIconPrefab;
  private const int UNITY_UNIT_CONVERSION = 100;

  public IEnumerator StartAsync(
    List<PlayerUnit> selectedBasePlayerUnits,
    Dictionary<PlayerUnit, List<PlayerUnit>> allSamePlayerUnits)
  {
    LumpToutaMaterialConfirmMenu materialConfirmMenu1 = this;
    materialConfirmMenu1.isMaterial = true;
    IEnumerator e = materialConfirmMenu1.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) materialConfirmMenu1.toutaIconPrefab, (Object) null))
    {
      Future<GameObject> ld = new ResourceObject("Prefabs/unit004_LumpTouta/LumpToutaIcon").Load<GameObject>();
      yield return (object) ld.Wait();
      materialConfirmMenu1.toutaIconPrefab = ld.Result;
      ld = (Future<GameObject>) null;
    }
    materialConfirmMenu1.unitPrefab = materialConfirmMenu1.toutaIconPrefab;
    Tuple<PlayerMaterialUnit, int[]>[] valueUpMaterials = LumpToutaBaseUnitSelectMenu.getUnityValueUpMaterials();
    materialConfirmMenu1.dicShareWorks = selectedBasePlayerUnits.ToDictionary<PlayerUnit, PlayerUnit, LumpToutaMaterialConfirmMenu.ShareWork>((Func<PlayerUnit, PlayerUnit>) (k => k), (Func<PlayerUnit, LumpToutaMaterialConfirmMenu.ShareWork>) (v => new LumpToutaMaterialConfirmMenu.ShareWork(v)));
    List<PlayerMaterialUnit> playerMaterialUnitList = new List<PlayerMaterialUnit>();
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    PlayerUnit.GetUnityValueMax();
    foreach (PlayerUnit selectedBasePlayerUnit1 in selectedBasePlayerUnits)
    {
      LumpToutaMaterialConfirmMenu materialConfirmMenu = materialConfirmMenu1;
      PlayerUnit selectedBasePlayerUnit = selectedBasePlayerUnit1;
      LumpToutaMaterialConfirmMenu.ShareWork dicShareWork = materialConfirmMenu1.dicShareWorks[selectedBasePlayerUnit];
      materialConfirmMenu1.selectedBasePlayerUnits.Add(selectedBasePlayerUnit);
      UnitUnit baseUnitUnit = selectedBasePlayerUnit.unit;
      double unityTotal = (double) selectedBasePlayerUnit.unityTotal;
      List<Tuple<PlayerMaterialUnit, UnitUnit, float>> materials = new List<Tuple<PlayerMaterialUnit, UnitUnit, float>>();
      foreach (Tuple<PlayerMaterialUnit, int[]> tuple in ((IEnumerable<Tuple<PlayerMaterialUnit, int[]>>) valueUpMaterials).Where<Tuple<PlayerMaterialUnit, int[]>>((Func<Tuple<PlayerMaterialUnit, int[]>, bool>) (o => ((IEnumerable<int>) o.Item2).Contains<int>(baseUnitUnit.ID))))
      {
        UnitUnit unit = tuple.Item1.unit;
        UnityValueUpPattern unityValueUpPattern = Array.Find<UnityValueUpPattern>(unit.UnityValueUpPatterns, (Predicate<UnityValueUpPattern>) (o =>
        {
          int? unitUnitUnit = o.unit_UnitUnit;
          int id = baseUnitUnit.ID;
          return unitUnitUnit.GetValueOrDefault() == id & unitUnitUnit.HasValue;
        }));
        materials.Add(Tuple.Create<PlayerMaterialUnit, UnitUnit, float>(tuple.Item1, unit, unityValueUpPattern.up_value));
      }
      List<Tuple<PlayerMaterialUnit, UnitUnit, float>> list = materialConfirmMenu1.sortUnityValueUpMaterials((IEnumerable<Tuple<PlayerMaterialUnit, UnitUnit, float>>) materials).Take<Tuple<PlayerMaterialUnit, UnitUnit, float>>(30).ToList<Tuple<PlayerMaterialUnit, UnitUnit, float>>();
      dicShareWork.materials = list.Select<Tuple<PlayerMaterialUnit, UnitUnit, float>, LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>((Func<Tuple<PlayerMaterialUnit, UnitUnit, float>, LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>) (x => new LumpToutaMaterialConfirmMenu.UnityValueUpMaterial(x.Item1, x.Item3))).ToArray<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>();
      PlayerUnit[] array = allSamePlayerUnits[selectedBasePlayerUnit].Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => selectedBasePlayerUnit.id != x.id && !materialConfirmMenu.IsExclusionUnitForLumpToutaMaterial(x))).OrderBy<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.rarity.index)).ThenBy<PlayerUnit, float>((Func<PlayerUnit, float>) (x => x.unityTotal)).ThenBy<PlayerUnit, float>((Func<PlayerUnit, float>) (x => x.buildup_unity_value_f - (float) x.unity_value)).Take<PlayerUnit>(30).ToArray<PlayerUnit>();
      dicShareWork.materialUnits = array;
      materialConfirmMenu1.dealUnityValueUp(dicShareWork);
      playerMaterialUnitList.AddRange(((IEnumerable<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>) dicShareWork.materials).Select<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial, PlayerMaterialUnit>((Func<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial, PlayerMaterialUnit>) (x => x.material)));
      playerUnitList.AddRange((IEnumerable<PlayerUnit>) dicShareWork.materialUnits);
    }
    materialConfirmMenu1.SetIconType(UnitMenuBase.IconType.Normal);
    materialConfirmMenu1.IconHeight = UnitIcon.HeightWithHpGauge;
    // ISSUE: reference to a compiler-generated method
    materialConfirmMenu1.InitializeInfoEx((IEnumerable<PlayerUnit>) playerUnitList.ToArray(), (IEnumerable<PlayerMaterialUnit>) playerMaterialUnitList.ToArray(), (Persist<Persist.UnitSortAndFilterInfo>) null, false, false, false, true, true, false, new Action(materialConfirmMenu1.\u003CStartAsync\u003Eb__14_2));
    e = materialConfirmMenu1.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    materialConfirmMenu1.InitializeEnd();
    materialConfirmMenu1.UpdateBottomInfo();
  }

  private IEnumerable<Tuple<PlayerMaterialUnit, UnitUnit, float>> sortUnityValueUpMaterials(
    IEnumerable<Tuple<PlayerMaterialUnit, UnitUnit, float>> materials)
  {
    return (IEnumerable<Tuple<PlayerMaterialUnit, UnitUnit, float>>) materials.OrderBy<Tuple<PlayerMaterialUnit, UnitUnit, float>, bool>((Func<Tuple<PlayerMaterialUnit, UnitUnit, float>, bool>) (x => x.Item2.expire_date == null)).ThenBy<Tuple<PlayerMaterialUnit, UnitUnit, float>, float>((Func<Tuple<PlayerMaterialUnit, UnitUnit, float>, float>) (y => y.Item3)).ThenBy<Tuple<PlayerMaterialUnit, UnitUnit, float>, int>((Func<Tuple<PlayerMaterialUnit, UnitUnit, float>, int>) (z => z.Item2.ID));
  }

  private void dealUnityValueUp(LumpToutaMaterialConfirmMenu.ShareWork swork)
  {
    int num1 = Mathf.CeilToInt(swork.baseUnit.unityTotal * 100f);
    int remainValue = PlayerUnit.GetUnityValueMax() * 100 - num1;
    long num2 = ((IEnumerable<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>) swork.materials).Sum<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>((Func<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial, long>) (x => (long) Mathf.CeilToInt(x.unityValue * 100f) * (long) x.material.quantity));
    int num3 = ((IEnumerable<PlayerUnit>) swork.materialUnits).Take<PlayerUnit>(1).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => Mathf.CeilToInt(Mathf.Min(x.unityTotal + (float) PlayerUnit.GetUnityValue(), (float) PlayerUnit.GetUnityValueMax()) * 100f)));
    List<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial> unityValueUpMaterialList = new List<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>(swork.materials.Length);
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>(swork.materialUnits.Length);
    int lastValue = 0;
    LumpToutaMaterialConfirmMenu.DealMode dealMode = LumpToutaMaterialConfirmMenu.DealMode.MtoU;
    if (remainValue <= 0)
      dealMode = LumpToutaMaterialConfirmMenu.DealMode.Finish;
    else if ((long) remainValue > num2 && (long) remainValue <= num2 + (long) num3)
      dealMode = LumpToutaMaterialConfirmMenu.DealMode.UtoM;
    do
    {
      if ((uint) dealMode > 1U)
      {
        if ((uint) (dealMode - 2) <= 1U)
        {
          if (remainValue > 0 && swork.materialUnits.Length != 0)
          {
            int num4 = Mathf.Min(30 - unityValueUpMaterialList.Count, swork.materialUnits.Length);
            for (int index = 0; remainValue > 0 && index < num4; ++index)
            {
              PlayerUnit materialUnit = swork.materialUnits[index];
              lastValue = Mathf.CeilToInt((float) (((double) materialUnit.unityTotal + (double) PlayerUnit.GetUnityValue()) * 100.0));
              remainValue -= lastValue;
              playerUnitList.Add(materialUnit);
              if (dealMode == LumpToutaMaterialConfirmMenu.DealMode.UtoM)
                break;
            }
          }
          dealMode = dealMode != LumpToutaMaterialConfirmMenu.DealMode.U_Fin ? LumpToutaMaterialConfirmMenu.DealMode.M_Fin : LumpToutaMaterialConfirmMenu.DealMode.Finish;
        }
      }
      else
      {
        for (int index = 0; remainValue > 0 && index < swork.materials.Length; ++index)
        {
          LumpToutaMaterialConfirmMenu.UnityValueUpMaterial material = swork.materials[index];
          lastValue = Mathf.CeilToInt(material.unityValue * 100f);
          if (remainValue < lastValue)
          {
            material.quantity = 1;
          }
          else
          {
            int num5 = Mathf.Min(material.material.quantity, remainValue / lastValue);
            material.quantity = num5;
          }
          remainValue -= lastValue * material.quantity;
          unityValueUpMaterialList.Add(material);
        }
        dealMode = dealMode != LumpToutaMaterialConfirmMenu.DealMode.M_Fin ? LumpToutaMaterialConfirmMenu.DealMode.U_Fin : LumpToutaMaterialConfirmMenu.DealMode.Finish;
      }
    }
    while (dealMode < LumpToutaMaterialConfirmMenu.DealMode.Finish);
    swork.materials = unityValueUpMaterialList.ToArray();
    swork.materialUnits = playerUnitList.ToArray();
  }

  private void ExInitializeInfo()
  {
    foreach (PlayerUnit selectedBasePlayerUnit in this.selectedBasePlayerUnits)
    {
      LumpToutaMaterialConfirmMenu.ShareWork dicShareWork = this.dicShareWorks[selectedBasePlayerUnit];
      List<UnitIconInfo> unitIconInfoList = new List<UnitIconInfo>();
      foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      {
        UnitIconInfo info = allUnitInfo;
        UnitUnit u = info.unit;
        if (u.IsNormalUnit)
        {
          if (Array.Find<PlayerUnit>(dicShareWork.materialUnits, (Predicate<PlayerUnit>) (x => x.id == info.playerUnit.id)) != (PlayerUnit) null)
            unitIconInfoList.Add(info);
        }
        else if (Array.Find<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>(dicShareWork.materials, (Predicate<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>) (x => x.material._unit == u.ID)) != null)
          unitIconInfoList.Add(info);
      }
      this.selectedMaterialUnitIconInfos.Add(unitIconInfoList);
    }
    List<UnitIconInfo> unitIconInfoList1 = new List<UnitIconInfo>();
    foreach (List<UnitIconInfo> materialUnitIconInfo in this.selectedMaterialUnitIconInfos)
      unitIconInfoList1.AddRange((IEnumerable<UnitIconInfo>) materialUnitIconInfo.OrderBy<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (x => x.unit.IsNormalUnit)));
    this.allUnitInfos = unitIconInfoList1;
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      allUnitInfo.IsNoCounterAndGray = true;
      allUnitInfo.select = 0;
      if (allUnitInfo.unit.is_unity_value_up)
      {
        LumpToutaMaterialConfirmMenu.UnityValueUpMaterial unitValueUpMaterial = this.findUnitValueUpMaterial(allUnitInfo.unit);
        allUnitInfo.count = allUnitInfo.SelectedCount = unitValueUpMaterial.quantity;
        this.unityValueUpMaxCounts.Add(allUnitInfo, unitValueUpMaterial.quantity);
      }
    }
  }

  private LumpToutaMaterialConfirmMenu.UnityValueUpMaterial findUnitValueUpMaterial(UnitUnit u)
  {
    LumpToutaMaterialConfirmMenu.UnityValueUpMaterial unitValueUpMaterial = (LumpToutaMaterialConfirmMenu.UnityValueUpMaterial) null;
    foreach (LumpToutaMaterialConfirmMenu.ShareWork shareWork in this.dicShareWorks.Values)
    {
      unitValueUpMaterial = Array.Find<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>(shareWork.materials, (Predicate<LumpToutaMaterialConfirmMenu.UnityValueUpMaterial>) (x => x.material._unit == u.ID));
      if (unitValueUpMaterial != null)
        break;
    }
    return unitValueUpMaterial;
  }

  private GameObject createWidthLine(int _index)
  {
    GameObject widthLine = Object.Instantiate<GameObject>(this.baseWidthLine);
    widthLine.transform.parent = ((Component) this.scroll.scrollView).transform;
    widthLine.transform.localPosition = new Vector3(0.0f, (float) -(_index / 5 * this.IconHeight + this.IconHeight / 2), 0.0f);
    widthLine.transform.localScale = Vector3.one;
    return widthLine;
  }

  private void ShowWidthLine(int index)
  {
    if (this.cacheWidthLines.Count < 3)
    {
      for (int count = this.cacheWidthLines.Count; count < 3; ++count)
      {
        if (!this.cacheWidthLines.ContainsKey(count * 5))
        {
          GameObject widthLine = this.createWidthLine(count * 5);
          this.cacheWidthLines.Add(count * 5, widthLine);
        }
      }
    }
    if (!this.cacheWidthLines.ContainsKey(index))
    {
      GameObject widthLine = this.createWidthLine(index);
      this.cacheWidthLines.Add(index, widthLine);
    }
    foreach (KeyValuePair<int, GameObject> cacheWidthLine in this.cacheWidthLines)
    {
      if (cacheWidthLine.Key <= index - this.IconMaxValue || cacheWidthLine.Key >= index + this.IconMaxValue)
        cacheWidthLine.Value.SetActive(false);
      else
        cacheWidthLine.Value.SetActive(true);
    }
  }

  protected override void Select(UnitIconBase selectUnitIcon)
  {
    if (selectUnitIcon.unit.IsNormalUnit)
      this.UnitIconSelect(selectUnitIcon);
    else
      this.StartCoroutine(this.UnityUpIconSelect(selectUnitIcon as UnitIcon));
  }

  private void UnitIconSelect(UnitIconBase selectUnitIcon)
  {
    if (selectUnitIcon.Selected)
    {
      this.UnSelect(selectUnitIcon);
      PlayerUnit playerUnit = (PlayerUnit) null;
      UnitIconInfo unitIconInfo = (UnitIconInfo) null;
      foreach (PlayerUnit selectedBasePlayerUnit in this.selectedBasePlayerUnits)
      {
        if (selectedBasePlayerUnit.unit.same_character_id == selectUnitIcon.unit.same_character_id)
        {
          playerUnit = selectedBasePlayerUnit;
          unitIconInfo = this.allUnitInfos.First<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit == selectUnitIcon.PlayerUnit));
          break;
        }
      }
      this.selectedMaterialUnitIconInfos[this.selectedBasePlayerUnits.IndexOf(playerUnit)].Remove(unitIconInfo);
    }
    else
    {
      this.OnSelect(selectUnitIcon);
      PlayerUnit playerUnit = (PlayerUnit) null;
      UnitIconInfo unitIconInfo = (UnitIconInfo) null;
      foreach (PlayerUnit selectedBasePlayerUnit in this.selectedBasePlayerUnits)
      {
        if (selectedBasePlayerUnit.unit.same_character_id == selectUnitIcon.unit.same_character_id)
        {
          playerUnit = selectedBasePlayerUnit;
          unitIconInfo = this.allUnitInfos.First<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit == selectUnitIcon.PlayerUnit));
          break;
        }
      }
      this.selectedMaterialUnitIconInfos[this.selectedBasePlayerUnits.IndexOf(playerUnit)].Add(unitIconInfo);
    }
    this.UpdateBottomInfo();
  }

  private IEnumerator UnityUpIconSelect(UnitIcon selectUnitIcon)
  {
    LumpToutaMaterialConfirmMenu materialConfirmMenu = this;
    Future<GameObject> f = new ResourceObject("Prefabs/unit/popup_Select_KillersInnocent").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    LumpToutaCombinePopup component = Singleton<PopupManager>.GetInstance().open(f.Result).GetComponent<LumpToutaCombinePopup>();
    UnitIconInfo unitIconInfo = materialConfirmMenu.allUnitInfos.First<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit == selectUnitIcon.PlayerUnit));
    UnitIcon baseUnitIcon = selectUnitIcon;
    UnitIconInfo unitIconInfo1 = unitIconInfo;
    int unityValueUpMaxCount = materialConfirmMenu.unityValueUpMaxCounts[unitIconInfo];
    Action<int> onOk = (Action<int>) (selectedCount =>
    {
      selectUnitIcon.SetCounter(selectedCount);
      unitIconInfo.count = selectedCount;
      UnityValueUpPattern[] unityValueUpPatterns = unitIconInfo.unit.UnityValueUpPatterns;
      PlayerUnit playerUnit = (PlayerUnit) null;
      foreach (PlayerUnit selectedBasePlayerUnit1 in this.selectedBasePlayerUnits)
      {
        PlayerUnit selectedBasePlayerUnit = selectedBasePlayerUnit1;
        if (((IEnumerable<UnityValueUpPattern>) unityValueUpPatterns).Any<UnityValueUpPattern>((Func<UnityValueUpPattern, bool>) (o =>
        {
          int? unitUnitUnit = o.unit_UnitUnit;
          int unit = selectedBasePlayerUnit._unit;
          return unitUnitUnit.GetValueOrDefault() == unit & unitUnitUnit.HasValue;
        })))
        {
          playerUnit = selectedBasePlayerUnit;
          break;
        }
      }
      if (selectedCount <= 0)
      {
        this.UnSelect((UnitIconBase) selectUnitIcon);
        this.selectedMaterialUnitIconInfos[this.selectedBasePlayerUnits.IndexOf(playerUnit)].Remove(unitIconInfo);
      }
      else
      {
        this.OnSelect((UnitIconBase) selectUnitIcon);
        int index = this.selectedBasePlayerUnits.IndexOf(playerUnit);
        if (!this.selectedMaterialUnitIconInfos[index].Contains(unitIconInfo))
          this.selectedMaterialUnitIconInfos[index].Add(unitIconInfo);
      }
      this.UpdateBottomInfo();
    });
    component.Init(baseUnitIcon, unitIconInfo1, unityValueUpMaxCount, onOk);
  }

  public override void OnSelect(UnitIconBase unitIcon)
  {
    unitIcon.SelectByCheckIcon(false);
    this.UnitInfoUpdates(unitIcon);
  }

  public override void UpdateSelectIcon()
  {
    foreach (UnitIconInfo displayUnitInfo in this.displayUnitInfos)
    {
      if (Object.op_Inequality((Object) displayUnitInfo.icon, (Object) null) && displayUnitInfo.playerUnit.favorite)
        displayUnitInfo.icon.Gray = true;
    }
    foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
    {
      UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(selectedUnitIcon.playerUnit);
      if (unitInfoDisplay != null && Object.op_Inequality((Object) unitInfoDisplay.icon, (Object) null))
        unitInfoDisplay.icon.SelectByCheckIcon(false);
    }
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    LumpToutaIcon allUnitIcon = (LumpToutaIcon) this.allUnitIcons[unit_index];
    UnitIconInfo info = this.displayUnitInfos[info_index];
    if (allUnitIcon.unit.IsNormalUnit)
    {
      allUnitIcon.setUnityTotal(Mathf.Min(allUnitIcon.PlayerUnit.unity_value + PlayerUnit.GetUnityValue(), PlayerUnit.GetUnityValueMax()), allUnitIcon.PlayerUnit.buildup_unity_value_f);
    }
    else
    {
      LumpToutaMaterialConfirmMenu.UnityValueUpMaterial unitValueUpMaterial = this.findUnitValueUpMaterial(allUnitIcon.unit);
      allUnitIcon.setUnityTotal(0, unitValueUpMaterial.unityValue);
    }
    info.gray = false;
    if (info.select >= 0)
      info.icon.SelectByCheckIcon(false);
    if (info_index % 5 == 0)
      this.ShowWidthLine(info_index);
    allUnitIcon.onClick = (Action<UnitIconBase>) (ui => this.Select(ui));
    allUnitIcon.onLongPress = (Action<UnitIconBase>) (x => Unit0042Scene.changeSceneEvolutionUnit(true, info.playerUnit, this.getUnits(), true, IsToutaPlusNoEnable: true));
  }

  public override bool SelectedUnitIsMax() => false;

  private void UpdateBottomInfo()
  {
    long num1 = 0;
    foreach (PlayerUnit selectedBasePlayerUnit in this.selectedBasePlayerUnits)
    {
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      foreach (UnitIconInfo unitIconInfo in this.selectedMaterialUnitIconInfos[this.selectedBasePlayerUnits.IndexOf(selectedBasePlayerUnit)])
      {
        for (int index = 0; index < unitIconInfo.count; ++index)
          playerUnitList.Add(unitIconInfo.playerUnit);
      }
      num1 += CalcUnitCompose.priceCompose(selectedBasePlayerUnit, playerUnitList.ToArray());
    }
    this.useZeny.SetTextLocalize(num1.ToString());
    int num2 = 0;
    foreach (List<UnitIconInfo> materialUnitIconInfo in this.selectedMaterialUnitIconInfos)
      num2 += materialUnitIconInfo.Count<UnitIconInfo>();
    this.selectedCount.text = num2.ToString();
    if (num2 <= 0)
      ((UIButtonColor) this.DecisionButton).isEnabled = false;
    else
      ((UIButtonColor) this.DecisionButton).isEnabled = true;
  }

  public void OnCombineButton() => this.StartCoroutine(this.Combine());

  private IEnumerator Combine()
  {
    LumpToutaMaterialConfirmMenu materialConfirmMenu = this;
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false);
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_004_8_3_2__anim_popup02.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Unit004832Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Unit004832Menu>();
    materialConfirmMenu.IsPush = true;
    // ISSUE: reference to a compiler-generated method
    component.callbackNo = new Action(materialConfirmMenu.\u003CCombine\u003Eb__32_0);
    component.mode = Unit00468Scene.Mode.UnitLumpTouta;
    bool isAlert = false;
    foreach (List<UnitIconInfo> materialUnitIconInfo in materialConfirmMenu.selectedMaterialUnitIconInfos)
    {
      foreach (UnitIconInfo unitIconInfo in materialUnitIconInfo)
      {
        if (unitIconInfo.playerUnit.unit.rarity.index >= 2)
        {
          isAlert = true;
          break;
        }
      }
      if (isAlert)
        break;
    }
    bool isMemoryAlert = false;
    if (PlayerTransmigrateMemoryPlayerUnitIds.Current != null)
    {
      int?[] memoryPlayerUnitIds = PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids;
      foreach (List<UnitIconInfo> materialUnitIconInfo in materialConfirmMenu.selectedMaterialUnitIconInfos)
      {
        foreach (UnitIconInfo unitIconInfo in materialUnitIconInfo)
        {
          if (((IEnumerable<int?>) memoryPlayerUnitIds).Contains<int?>(new int?(unitIconInfo.playerUnit.id)))
          {
            isMemoryAlert = true;
            break;
          }
        }
        if (isMemoryAlert)
          break;
      }
    }
    component.Init(materialConfirmMenu.selectedBasePlayerUnits, materialConfirmMenu.selectedMaterialUnitIconInfos, isAlert, isMemoryAlert);
  }

  private class UnityValueUpMaterial
  {
    public int quantity;

    public PlayerMaterialUnit material { get; private set; }

    public float unityValue { get; private set; }

    public UnityValueUpMaterial(PlayerMaterialUnit m, float v)
    {
      this.material = m;
      this.unityValue = v;
      this.quantity = 0;
    }
  }

  private class ShareWork
  {
    public PlayerUnit[] materialUnits;
    public LumpToutaMaterialConfirmMenu.UnityValueUpMaterial[] materials;
    public float unityValue;

    public PlayerUnit baseUnit { get; private set; }

    public ShareWork(PlayerUnit u)
    {
      this.baseUnit = u;
      this.materialUnits = (PlayerUnit[]) null;
      this.materials = (LumpToutaMaterialConfirmMenu.UnityValueUpMaterial[]) null;
      this.unityValue = 0.0f;
    }
  }

  private enum DealMode
  {
    MtoU,
    M_Fin,
    UtoM,
    U_Fin,
    Finish,
  }
}
