// Decompiled with JetBrains decompiler
// Type: BattleUI02Tab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI02Tab : BattleMonoBehaviour
{
  public GameObject pmBaseTab;
  public GameObject pmBattleTab;
  public GameObject pmDetailTab;
  public GameObject pmEtcTab;
  public GameObject mBase;
  public GameObject mBaseA;
  public GameObject mBtl;
  public GameObject mBtlA;
  public GameObject mDet;
  public GameObject mDetA;
  public GameObject mEtc;
  public GameObject mEtcA;
  private GameObject[] buttonList;
  private GameObject[] paramList;
  private BattleUI02Tab.Tabname currentTab;

  protected override IEnumerator Start_Original()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleUI02Tab battleUi02Tab = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleUi02Tab.buttonList = new GameObject[8]
    {
      battleUi02Tab.mBase,
      battleUi02Tab.mBaseA,
      battleUi02Tab.mBtl,
      battleUi02Tab.mBtlA,
      battleUi02Tab.mDet,
      battleUi02Tab.mDetA,
      battleUi02Tab.mEtc,
      battleUi02Tab.mEtcA
    };
    battleUi02Tab.paramList = new GameObject[4]
    {
      battleUi02Tab.pmBaseTab,
      battleUi02Tab.pmBattleTab,
      battleUi02Tab.pmDetailTab,
      battleUi02Tab.pmEtcTab
    };
    return false;
  }

  protected override IEnumerator Start_Battle()
  {
    this.currentTab = BattleUI02Tab.Tabname.BaseTab;
    yield break;
  }

  protected override void LateUpdate_Battle()
  {
  }

  public void onBaseButton() => this.SetActiceButton(BattleUI02Tab.Tabname.BaseTab);

  public void onBattleButton() => this.SetActiceButton(BattleUI02Tab.Tabname.BattleTab);

  public void onDetailButton() => this.SetActiceButton(BattleUI02Tab.Tabname.DetailTab);

  public void onEtcButton() => this.SetActiceButton(BattleUI02Tab.Tabname.EtcTab);

  private void SetActiceButton(BattleUI02Tab.Tabname tabName)
  {
    if (this.currentTab == tabName)
      return;
    this.currentTab = tabName;
    for (int index = 0; index < this.buttonList.Length; index += 2)
    {
      bool flag = (BattleUI02Tab.Tabname) (index / 2) == tabName;
      this.buttonList[index].SetActive(!flag);
      this.buttonList[index + 1].SetActive(flag);
    }
    for (int index = 0; index < this.paramList.Length; ++index)
      this.paramList[index].SetActive((BattleUI02Tab.Tabname) index == tabName);
  }

  private enum Tabname
  {
    BaseTab,
    BattleTab,
    DetailTab,
    EtcTab,
  }
}
