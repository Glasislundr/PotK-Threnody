// Decompiled with JetBrains decompiler
// Type: unit004812Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class unit004812Scene : NGSceneBase
{
  public List<PlayerUnit> unitList;
  public List<PlayerUnit> resultList;
  public bool is_success_;
  private GameObject princessSynthesisPrefab;
  private string nowBgmName;
  [SerializeField]
  private unit004812Menu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_12", stack);
  }

  public static void changeScene(
    bool stack,
    List<PlayerUnit> num_list,
    List<PlayerUnit> result_list,
    List<int> other_list,
    Dictionary<string, object> showPopupData)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_12", (stack ? 1 : 0) != 0, (object) num_list, (object) result_list, (object) other_list, (object) showPopupData);
  }

  public static void changeScene(
    bool stack,
    List<PlayerUnit> num_list,
    List<PlayerUnit> result_list,
    List<int> other_list,
    Dictionary<string, object> showPopupData,
    Unit00468Scene.Mode mode)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_12", (stack ? 1 : 0) != 0, (object) num_list, (object) result_list, (object) other_list, (object) showPopupData, (object) mode);
  }

  public IEnumerator onStartSceneAsync()
  {
    PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
    if (playerUnitArray[0] == (PlayerUnit) null)
      Debug.LogError((object) "have unit data none");
    this.unitList = new List<PlayerUnit>();
    this.unitList.Clear();
    int num = Random.Range(1, 6);
    for (int index = 0; index < num; ++index)
    {
      if (playerUnitArray.Length > index)
        this.unitList.Add(playerUnitArray[index]);
      else
        this.unitList.Add(playerUnitArray[0]);
    }
    if (num == 3)
      this.is_success_ = true;
    List<PlayerUnit> result_list = new List<PlayerUnit>();
    result_list.Add(playerUnitArray[0]);
    result_list.Add(playerUnitArray[0]);
    List<int> other_list = new List<int>();
    other_list.Add(0);
    other_list.Add(0);
    Dictionary<string, object> showPopupData = new Dictionary<string, object>();
    IEnumerator e = this.onStartSceneAsync(this.unitList, result_list, other_list, showPopupData, Unit00468Scene.Mode.Unit0048);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    List<PlayerUnit> num_list,
    List<PlayerUnit> result_list,
    List<int> other_list,
    Dictionary<string, object> showPopupData)
  {
    this.unitList = num_list;
    this.resultList = result_list;
    IEnumerator e;
    if (Object.op_Equality((Object) this.princessSynthesisPrefab, (Object) null))
    {
      Future<GameObject> princessSynthesisPrefabf = Res.Prefabs.synthetic.Synthesis.Load<GameObject>();
      e = princessSynthesisPrefabf.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.princessSynthesisPrefab = princessSynthesisPrefabf.Result;
      princessSynthesisPrefabf = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.menu.effect, (Object) null))
      this.menu.effect = Object.Instantiate<GameObject>(this.princessSynthesisPrefab).GetComponent<EffectControllerPrincessSynthesis>();
    this.menu.showPopupData = showPopupData;
    e = this.menu.SetEffectData(num_list, result_list, other_list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    List<PlayerUnit> num_list,
    List<PlayerUnit> result_list,
    List<int> other_list,
    Dictionary<string, object> showPopupData,
    Unit00468Scene.Mode mode)
  {
    this.menu.mode = mode;
    IEnumerator e = this.onStartSceneAsync(num_list, result_list, other_list, showPopupData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    List<List<PlayerUnit>> selectedMaterialPlayerUnits,
    List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits,
    List<Unit004832Menu.OhterInfo> otherInfos,
    List<Dictionary<string, object>> showPopupDatas)
  {
    if (selectedMaterialPlayerUnits.Count != resultPlayerUnits.Count || resultPlayerUnits.Count != otherInfos.Count || otherInfos.Count != showPopupDatas.Count)
      Debug.LogError((object) "LumpTouta ListError");
    this.menu.mode = Unit00468Scene.Mode.UnitLumpTouta;
    this.menu.selectedMaterialPlayerUnits = selectedMaterialPlayerUnits;
    this.menu.resultPlayerUnits = resultPlayerUnits;
    this.menu.otherInfos = otherInfos;
    this.menu.showPopupDatas = showPopupDatas;
    IEnumerator e = this.onStartSceneAsync(selectedMaterialPlayerUnits[0], new List<PlayerUnit>()
    {
      resultPlayerUnits[0].beforePlayerUnit,
      resultPlayerUnits[0].afterPlayerUnit
    }, new List<int>()
    {
      Convert.ToInt32(otherInfos[0].is_success),
      otherInfos[0].increment_medal,
      otherInfos[0].gain_trust_result.is_equip_awake_skill_release ? 1 : 0,
      otherInfos[0].gain_trust_result.has_new_player_awake_skill ? 1 : 0
    }, showPopupDatas[0]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.nowBgmName = Singleton<NGSoundManager>.GetInstance().GetBgmName();
    Singleton<NGSoundManager>.GetInstance().StopBgm();
  }

  public void onStartScene(
    List<PlayerUnit> num_list,
    List<PlayerUnit> result_list,
    List<int> other_list,
    Dictionary<string, object> showPopupData)
  {
    this.onStartScene();
  }

  public void onStartScene(
    List<PlayerUnit> num_list,
    List<PlayerUnit> result_list,
    List<int> other_list,
    Dictionary<string, object> showPopupData,
    Unit00468Scene.Mode mode)
  {
    this.onStartScene();
  }

  public void onStartScene(
    List<List<PlayerUnit>> selectedMaterialPlayerUnits,
    List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits,
    List<Unit004832Menu.OhterInfo> otherInfos,
    List<Dictionary<string, object>> showPopupDatas)
  {
    this.onStartScene();
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    Singleton<NGSoundManager>.GetInstance().PlayBgm(this.nowBgmName);
  }

  public override IEnumerator onEndSceneAsync()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ((Component) this.menu.effect).gameObject.SetActive(false);
  }
}
