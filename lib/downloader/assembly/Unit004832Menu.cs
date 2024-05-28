// Decompiled with JetBrains decompiler
// Type: Unit004832Menu
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
public class Unit004832Menu : BackButtonMenuBase
{
  [SerializeField]
  private Vector3 TextYPos1;
  [SerializeField]
  private Vector3 TextYPos2;
  [SerializeField]
  protected UILabel TxtDescription1;
  [SerializeField]
  protected UILabel TxtDescription2;
  private List<PlayerUnit> selectedBasePlayerUnits;
  private List<List<UnitIconInfo>> selectedMaterialUnitIconInfos;
  private PlayerUnit[] selectUnits;
  private PlayerUnit baseUnit;
  [NonSerialized]
  public Action callbackNo;
  private Action callbackYes;
  private Func<List<PlayerUnit>, WebAPI.Response.UnitBuildup, Dictionary<string, object>> resultFuncBuildup;

  public Unit00468Scene.Mode mode { get; set; }

  public void Init(bool isAlert, bool isMemoryAlert, Action actionYes)
  {
    this.callbackYes = actionYes;
    this.InitCommon(isAlert, isMemoryAlert);
  }

  public void Init(
    Func<List<PlayerUnit>, WebAPI.Response.UnitBuildup, Dictionary<string, object>> func,
    PlayerUnit basePlayerUnit,
    PlayerUnit[] materialUnit,
    bool isAlert,
    bool isMemoryAlert)
  {
    this.resultFuncBuildup = func;
    this.baseUnit = basePlayerUnit;
    this.selectUnits = materialUnit;
    this.InitCommon(isAlert, isMemoryAlert);
  }

  public void Init(
    List<PlayerUnit> selectedBasePlayerUnits,
    List<List<UnitIconInfo>> selectedMaterialUnitIconInfos,
    bool isAlert,
    bool isMemoryAlert)
  {
    this.selectedBasePlayerUnits = selectedBasePlayerUnits;
    this.selectedMaterialUnitIconInfos = selectedMaterialUnitIconInfos;
    this.Init((Func<List<PlayerUnit>, WebAPI.Response.UnitBuildup, Dictionary<string, object>>) null, (PlayerUnit) null, (PlayerUnit[]) null, isAlert, isMemoryAlert);
  }

  private void InitCommon(bool isAlert, bool isMemoryAlert)
  {
    if (isAlert & isMemoryAlert)
    {
      ((Component) this.TxtDescription1).transform.localPosition = this.TextYPos1;
      ((Component) this.TxtDescription1).gameObject.SetActive(true);
      ((Component) this.TxtDescription2).transform.localPosition = this.TextYPos2;
      ((Component) this.TxtDescription2).gameObject.SetActive(true);
    }
    else if (isAlert)
    {
      ((Component) this.TxtDescription1).transform.localPosition = this.TextYPos1;
      ((Component) this.TxtDescription1).gameObject.SetActive(true);
      ((Component) this.TxtDescription2).gameObject.SetActive(false);
    }
    else if (isMemoryAlert)
    {
      ((Component) this.TxtDescription2).transform.localPosition = this.TextYPos1;
      ((Component) this.TxtDescription2).gameObject.SetActive(true);
      ((Component) this.TxtDescription1).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.TxtDescription1).transform.localPosition = this.TextYPos1;
      ((Component) this.TxtDescription1).gameObject.SetActive(true);
      this.TxtDescription1.text = "選択した姫を統合します。\n本当に実行してよろしいですか？";
      ((Component) this.TxtDescription2).gameObject.SetActive(false);
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    if (this.callbackNo != null)
    {
      this.callbackNo();
      this.callbackNo = (Action) null;
    }
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    if (this.callbackYes != null)
      this.callbackYes();
    else if (this.mode == Unit00468Scene.Mode.UnitLumpTouta)
      this.StartCoroutine(this.lumpcCombine());
    else
      Debug.LogError((object) ("Unit004832Menu: 想定されたmodeでありません. mode is " + (object) this.mode));
  }

  private IEnumerator lumpcCombine()
  {
    Unit004832Menu unit004832Menu = this;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<CommonRoot>.GetInstance().isLoading)
    {
      unit004832Menu.IsPush = false;
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      List<PlayerUnit> requestSelectedBasePlayerUnits = new List<PlayerUnit>();
      List<(int, List<UnitIconInfo>)> valueTupleList = new List<(int, List<UnitIconInfo>)>();
      List<List<UnitIconInfo>> unitIconInfoListList = new List<List<UnitIconInfo>>();
      int num = 0;
      for (int index = 0; index < unit004832Menu.selectedBasePlayerUnits.Count; ++index)
      {
        PlayerUnit selectedBasePlayerUnit = unit004832Menu.selectedBasePlayerUnits[index];
        List<UnitIconInfo> materialUnitIconInfo = unit004832Menu.selectedMaterialUnitIconInfos[index];
        if (materialUnitIconInfo.Count > 0)
        {
          List<UnitIconInfo> unitIconInfoList1 = new List<UnitIconInfo>();
          List<UnitIconInfo> unitIconInfoList2 = new List<UnitIconInfo>();
          foreach (UnitIconInfo unitIconInfo in materialUnitIconInfo)
          {
            UnitIconInfo info = unitIconInfo;
            if (((IEnumerable<UnityValueUpPattern>) MasterData.UnityValueUpPatternList).Any<UnityValueUpPattern>((Func<UnityValueUpPattern, bool>) (o => o.material_unit_UnitUnit == info.unit.ID)))
              unitIconInfoList1.Add(info);
            else
              unitIconInfoList2.Add(info);
          }
          requestSelectedBasePlayerUnits.Add(selectedBasePlayerUnit);
          if (unitIconInfoList1.Count > 0)
            valueTupleList.Add((num, unitIconInfoList1));
          unitIconInfoListList.Add(unitIconInfoList2);
          ++num;
        }
      }
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      if (valueTupleList.Count > 0)
      {
        foreach ((int, List<UnitIconInfo>) valueTuple in valueTupleList)
        {
          foreach (UnitIconInfo unitIconInfo in valueTuple.Item2)
          {
            intList3.Add(valueTuple.Item1);
            intList1.Add(unitIconInfo.playerUnit.id);
            intList2.Add(unitIconInfo.count);
          }
        }
      }
      int[] array1 = intList1.ToArray();
      int[] array2 = intList2.ToArray();
      int[] array3 = intList3.ToArray();
      int[][] numArray = new int[30][]
      {
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0],
        new int[0]
      };
      for (int index = 0; index < unitIconInfoListList.Count; ++index)
      {
        List<UnitIconInfo> source = unitIconInfoListList[index];
        if (source.Any<UnitIconInfo>())
          numArray[index] = source.Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>();
      }
      Future<WebAPI.Response.UnitLumpCompose> paramF = WebAPI.UnitLumpCompose(requestSelectedBasePlayerUnits.Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>(), array1, array2, array3, numArray[0], numArray[1], numArray[2], numArray[3], numArray[4], numArray[5], numArray[6], numArray[7], numArray[8], numArray[9], numArray[10], numArray[11], numArray[12], numArray[13], numArray[14], numArray[15], numArray[16], numArray[17], numArray[18], numArray[19], numArray[20], numArray[21], numArray[22], numArray[23], numArray[24], numArray[25], numArray[26], numArray[27], numArray[28], numArray[29]);
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      WebAPI.Response.UnitLumpCompose result = paramF.Result;
      if (result == null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        unit004832Menu.IsPush = false;
      }
      else
      {
        Singleton<NGGameDataManager>.GetInstance().clearPreviewInheritance();
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) result.corps_player_unit_ids);
        PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
        List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnitList = new List<Unit004832Menu.ResultPlayerUnit>();
        foreach (PlayerUnit beforePlayerUnit in requestSelectedBasePlayerUnits)
        {
          foreach (PlayerUnit afterPlayerUnit in playerUnitArray)
          {
            if (afterPlayerUnit.id == beforePlayerUnit.id)
            {
              resultPlayerUnitList.Add(new Unit004832Menu.ResultPlayerUnit(beforePlayerUnit, afterPlayerUnit));
              break;
            }
          }
        }
        Singleton<NGSceneManager>.GetInstance().clearStack("unit004_LumpTouta");
        Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_LumpTouta_Confirmation");
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_LumpTouta_Result", false, (object) resultPlayerUnitList, (object) result.increment_medal, (object) result.gain_trust_results, (object) result.unlock_quests);
        unit004832Menu.IsPush = false;
      }
    }
  }

  public class ResultPlayerUnit
  {
    public PlayerUnit beforePlayerUnit;
    public PlayerUnit afterPlayerUnit;

    public ResultPlayerUnit(PlayerUnit beforePlayerUnit, PlayerUnit afterPlayerUnit)
    {
      this.beforePlayerUnit = beforePlayerUnit;
      this.afterPlayerUnit = afterPlayerUnit;
    }
  }

  public class OhterInfo
  {
    public bool is_success;
    public int increment_medal;
    public GainTrustResult gain_trust_result;

    public OhterInfo(bool is_success, int increment_medal, GainTrustResult gain_trust_result)
    {
      this.is_success = is_success;
      this.increment_medal = increment_medal;
      this.gain_trust_result = gain_trust_result;
    }
  }
}
