// Decompiled with JetBrains decompiler
// Type: Guild011PledgeMenu
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
public class Guild011PledgeMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject firstObj;
  [SerializeField]
  private UILabel firstUnitName;
  [SerializeField]
  private GameObject firsrUnit;
  [SerializeField]
  private UIGrid scrollGrid;
  [SerializeField]
  private GameObject nonListObj;
  private GameObject unitPrefab;
  private GameObject pledgeNumPrefab;
  private PlayerCallDivorceHistory[] divorceList;

  public IEnumerator onStartSceneAsync()
  {
    Guild011PledgeMenu guild011PledgeMenu = this;
    Modified<Player> modified = SMManager.Observe<Player>();
    guild011PledgeMenu.divorceList = (PlayerCallDivorceHistory[]) null;
    guild011PledgeMenu.divorceList = modified.Value.call_divorce_histories;
    if (guild011PledgeMenu.divorceList == null || guild011PledgeMenu.divorceList.Length == 0)
    {
      guild011PledgeMenu.firstObj.SetActive(false);
      guild011PledgeMenu.nonListObj.SetActive(true);
    }
    else
    {
      yield return (object) guild011PledgeMenu.StartCoroutine(guild011PledgeMenu.PrefabLoad());
      IEnumerator e = guild011PledgeMenu.CreateUnitIcon(guild011PledgeMenu.unitPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
    }
  }

  private IEnumerator PrefabLoad()
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitPrefab = prefabF.Result;
    prefabF = new ResourceObject("Prefabs/guide011_PledgeHime_List/dir_PledgeHime_Num").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.pledgeNumPrefab = prefabF.Result;
  }

  private IEnumerator CreateUnitIcon(GameObject prefab)
  {
    Guild011PledgeMenu guild011PledgeMenu1 = this;
    UnitIcon firstUnitIcon = Object.Instantiate<GameObject>(prefab, guild011PledgeMenu1.firsrUnit.transform).GetComponent<UnitIcon>();
    List<UnitUnit> iconData = ((IEnumerable<PlayerCallDivorceHistory>) guild011PledgeMenu1.divorceList).Select<PlayerCallDivorceHistory, UnitUnit>((Func<PlayerCallDivorceHistory, UnitUnit>) (x => Array.Find<UnitUnit>(MasterData.UnitUnitList, (Predicate<UnitUnit>) (y => y.IsNormalUnit && x.same_character_id == y.same_character_id)))).ToList<UnitUnit>();
    // ISSUE: reference to a compiler-generated method
    UnitUnit firstUnit = iconData.Find(new Predicate<UnitUnit>(guild011PledgeMenu1.\u003CCreateUnitIcon\u003Eb__10_1));
    yield return (object) firstUnitIcon.SetUnit(firstUnit, CommonElement.none, false);
    guild011PledgeMenu1.firstUnitName.SetTextLocalize(firstUnit.name);
    firstUnitIcon.BottomBaseObject = false;
    Guild011PledgeMenu guild011PledgeMenu = guild011PledgeMenu1;
    for (int i = 1; i < guild011PledgeMenu1.divorceList.Length; ++i)
    {
      GameObject UnitIconObj = Object.Instantiate<GameObject>(prefab, ((Component) guild011PledgeMenu1.scrollGrid).gameObject.transform);
      UnitIcon UnitIcon = UnitIconObj.GetComponent<UnitIcon>();
      yield return (object) UnitIcon.SetUnit(iconData.Find((Predicate<UnitUnit>) (x => x.same_character_id == guild011PledgeMenu.divorceList[i].same_character_id)), CommonElement.none, false);
      UnitIcon.BottomBaseObject = false;
      Object.Instantiate<GameObject>(guild011PledgeMenu1.pledgeNumPrefab, UnitIconObj.transform).GetComponent<PledgeHimeNum>().SetPledgeNum(guild011PledgeMenu1.divorceList[i].divorce_number);
      UnitIconObj = (GameObject) null;
      UnitIcon = (UnitIcon) null;
    }
    guild011PledgeMenu1.scrollGrid.Reposition();
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) null;
  }

  public void IbtnBack() => this.backScene();

  public override void onBackButton() => this.IbtnBack();
}
