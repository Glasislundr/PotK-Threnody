// Decompiled with JetBrains decompiler
// Type: Guide01142bMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guide01142bMenu : BackButtonMenuBase
{
  public NGxScroll scroll;
  private int currentIndex;
  private int lastIndex;
  [SerializeField]
  private UICenterOnChild centerOnChild;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private GameObject rightArrow;
  [SerializeField]
  private GameObject leftArrow;
  private GameObject[] detailObject;
  private Dictionary<GameObject, Guide0112BuguDetailB> detailPrefabDict;
  private bool isArrowBtn = true;
  private readonly int DISPLAY_OBJECT_MAX = 4;

  public IEnumerator onStartSceneAsync(GearGear gear, bool isDispNumber, int index)
  {
    Guide01142bMenu guide01142bMenu = this;
    guide01142bMenu.leftArrow.SetActive(false);
    guide01142bMenu.rightArrow.SetActive(false);
    ((Behaviour) guide01142bMenu.scroll.scrollView).enabled = false;
    IEnumerator e = guide01142bMenu.GearsInit(new GearGear[1]
    {
      gear
    }, (isDispNumber ? 1 : 0) != 0, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GearGear[] gears, bool isDispNumber, int index = 0)
  {
    IEnumerator e = this.GearsInit(gears, isDispNumber, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator GearsInit(GearGear[] gear, bool isDispNumber, int index)
  {
    Guide01142bMenu m = this;
    Future<GameObject> prefabF = Res.Prefabs.guide011_4_2.guid_bugu_detail_b.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    m.detailObject = new GameObject[gear.Length];
    m.detailPrefabDict = new Dictionary<GameObject, Guide0112BuguDetailB>();
    for (int index1 = 0; index1 < gear.Length; ++index1)
    {
      m.detailObject[index1] = Object.Instantiate<GameObject>(result);
      m.scroll.Add(m.detailObject[index1]);
      Guide0112BuguDetailB component = m.detailObject[index1].GetComponent<Guide0112BuguDetailB>();
      m.detailPrefabDict.Add(m.detailObject[index1], component);
    }
    m.scroll.ResolvePosition();
    for (int i = 0; i < m.detailObject.Length; ++i)
    {
      Guide0112BuguDetailB d = m.detailObject[i].GetComponent<Guide0112BuguDetailB>();
      d.Init(m, gear[i], isDispNumber);
      e = d.InitDetailedScreen(gear[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      d.index = i;
      d.SetContainerPosition();
      d = (Guide0112BuguDetailB) null;
    }
    m.currentIndex = index;
    ((Component) m.scroll.scrollView).transform.localPosition = new Vector3(-m.scroll.grid.cellWidth * (float) m.currentIndex, 0.0f, 0.0f);
    m.SetMenuInformation(m.currentIndex);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  protected override void Update()
  {
    base.Update();
    int num1 = this.currentIndex;
    if ((double) ((Component) this.scroll.scrollView).transform.localPosition.x < 0.0)
    {
      int num2 = (int) Mathf.Abs((((Component) this.scroll.scrollView).transform.localPosition.x - this.scroll.grid.cellWidth / 2f) / this.scroll.grid.cellWidth);
      num1 = num2 <= this.detailObject.Length ? num2 : this.detailObject.Length - 1;
    }
    if (this.currentIndex == num1)
      return;
    this.currentIndex = num1;
    bool flag = true;
    if (this.currentIndex < 0)
    {
      this.currentIndex = 0;
      flag = false;
    }
    if (this.currentIndex >= this.detailObject.Length)
    {
      this.currentIndex = this.detailObject.Length - 1;
      flag = false;
    }
    this.SetMenuInformation(this.currentIndex);
    if (!flag)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1005");
  }

  private void SetMenuInformation(int idx)
  {
    if (idx < 0 || idx > this.detailObject.Length - 1)
      return;
    this.detailObject[idx].GetComponent<Guide0112BuguDetailB>().SetGearInformation();
    this.rightArrow.SetActive(true);
    this.leftArrow.SetActive(true);
    if (idx == 0)
      this.leftArrow.SetActive(false);
    if (idx != this.detailObject.Length - 1)
      return;
    this.rightArrow.SetActive(false);
  }

  public void SetTitleText(string gearName)
  {
    ((Component) this.txtTitle).gameObject.SetActive(true);
    this.txtTitle.SetText(gearName);
  }

  private void CenterOnChild(int num)
  {
    foreach (GameObject key in this.detailObject)
    {
      if (this.detailPrefabDict[key].index == num)
      {
        this.currentIndex = num;
        // ISSUE: method pointer
        this.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(\u003CCenterOnChild\u003Eb__17_0));
        this.centerOnChild.CenterOn(key.transform);
        break;
      }
    }
  }

  public void IbtnLeftArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    int num = this.currentIndex - 1;
    if (num < 0)
      return;
    this.CenterOnChild(num);
  }

  public void IbtnRightArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    int num = this.currentIndex + 1;
    if (num > this.detailObject.Length - 1)
      return;
    this.CenterOnChild(num);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
