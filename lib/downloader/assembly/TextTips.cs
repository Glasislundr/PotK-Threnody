// Decompiled with JetBrains decompiler
// Type: TextTips
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class TextTips : CommonTips
{
  private const uint CREATE_WORD_PARTS_COUNT = 6;
  [SerializeField]
  private UI2DSprite slcBgSprite;
  [SerializeField]
  private Transform textTipsPosition;
  [SerializeField]
  private Transform backgroundTipsPosition;
  [SerializeField]
  private NGxBlinkExNext blink;
  [SerializeField]
  private GameObject slcLineDeco;
  [SerializeField]
  private GameObject slcLineDeco2;
  [SerializeField]
  private GameObject blackS;
  private List<int> makeIdList;
  private GameObject textTipsPrefab;
  private GameObject backgroundTipsPrefab;
  private GameObject backgroundTips;
  private Sprite backgroundTipsSprite;
  private List<GameObject> blinkGroup = new List<GameObject>();

  protected override void Awake()
  {
    this.blink.StopAllCoroutines();
    ((Behaviour) this.blink).enabled = false;
    base.Awake();
    this.makeIdList = this.GetValidDataIdListAtRandom();
    List<int> makeIdList = this.makeIdList;
  }

  protected override IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TextTips textTips = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) textTips.StartCoroutine(textTips.Init());
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void Update()
  {
    if (Singleton<PopupManager>.GetInstance().ModalWindowIsOpen || !Input.GetMouseButtonDown(0) || !((Behaviour) this.blink).isActiveAndEnabled)
      return;
    this.blink.BlinkToNextElement();
  }

  private IEnumerator Init()
  {
    IEnumerator e = this.LoadTipsPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadTipsPrefabs()
  {
    TextTips textTips = this;
    textTips.blackS.SetActive(true);
    Future<GameObject> textTipsPrefabf = Res.Prefabs.TipsLoading.dir_txt_tips.Load<GameObject>();
    IEnumerator e = textTipsPrefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    textTips.textTipsPrefab = textTipsPrefabf.Result;
    Future<GameObject> backgroundTipsPrefabf = Res.Prefabs.TipsLoading.slc_SlideShow_bg.Load<GameObject>();
    e = backgroundTipsPrefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    textTips.backgroundTipsPrefab = backgroundTipsPrefabf.Result;
    TipsTextTips tipsTextTips = (TipsTextTips) null;
    if (MasterData.TipsTextTips.TryGetValue(textTips.makeIdList[0], out tipsTextTips))
    {
      Future<Sprite> bgSpriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>("TipsLoading/" + tipsTextTips.image_name);
      e = bgSpriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      textTips.backgroundTips = Object.Instantiate<GameObject>(textTips.backgroundTipsPrefab, textTips.backgroundTipsPosition);
      UI2DSprite component = textTips.backgroundTips.GetComponent<UI2DSprite>();
      component.sprite2D = bgSpriteF.Result;
      ((UIRect) component).alpha = 0.0f;
      textTips.backgroundTips.gameObject.SetActive(true);
      textTips.StartCoroutine(textTips.AlphaBlend(component));
      bgSpriteF = (Future<Sprite>) null;
    }
    textTips.slcLineDeco.SetActive(true);
    textTips.slcLineDeco2.SetActive(true);
    TipsTextTips tips = (TipsTextTips) null;
    for (int index = 0; index < textTips.makeIdList.Count; ++index)
    {
      if (MasterData.TipsTextTips.TryGetValue(textTips.makeIdList[index], out tips))
      {
        GameObject gameObject = Object.Instantiate<GameObject>(textTips.textTipsPrefab, textTips.textTipsPosition);
        gameObject.GetComponent<TextTipsPrefab>().Init(tips);
        textTips.blinkGroup.Add(gameObject);
      }
    }
    if (textTips.blinkGroup.Count > 1)
    {
      textTips.blink.StopAllCoroutines();
      textTips.blink.SetChildren(textTips.blinkGroup.ToArray());
      ((Behaviour) textTips.blink).enabled = true;
    }
  }

  private IEnumerator AlphaBlend(UI2DSprite sprite)
  {
    ((UIRect) sprite).alpha = 0.0f;
    do
    {
      yield return (object) null;
      UI2DSprite ui2Dsprite = sprite;
      ((UIRect) ui2Dsprite).alpha = ((UIRect) ui2Dsprite).alpha + 1f * Time.deltaTime;
    }
    while ((double) ((UIRect) sprite).alpha < 1.0);
    ((UIRect) sprite).alpha = 1f;
  }

  private List<int> GetValidDataIdListAtRandom()
  {
    List<TipsTextTips> list1 = ((IEnumerable<TipsTextTips>) MasterData.TipsTextTipsList).Where<TipsTextTips>((Func<TipsTextTips, bool>) (tips => tips.enable && tips.image_name != "")).ToList<TipsTextTips>();
    TipsTextTips mainTips = list1[Random.Range(0, list1.Count)];
    List<int> list2 = ((IEnumerable<TipsTextTips>) MasterData.TipsTextTipsList).Where<TipsTextTips>((Func<TipsTextTips, bool>) (tips => tips.enable && tips.image_name == mainTips.image_name)).Select<TipsTextTips, int>((Func<TipsTextTips, int>) (tips => tips.ID)).ToList<int>();
    if (list2 == null)
      return (List<int>) null;
    int capacity = Mathf.Min(6, list2.Count);
    if (capacity < 1)
      return (List<int>) null;
    List<int> dataIdListAtRandom = new List<int>(capacity);
    for (int index1 = 0; index1 < capacity; ++index1)
    {
      int index2 = Random.Range(0, list2.Count);
      dataIdListAtRandom.Add(list2[index2]);
      list2.RemoveAt(index2);
    }
    return dataIdListAtRandom;
  }
}
