// Decompiled with JetBrains decompiler
// Type: PopupEditCustomDeck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CustomDeck/Popup/EditCustomDeck")]
public class PopupEditCustomDeck : BackButtonPopupBase
{
  [SerializeField]
  private PopupEditCustomDeckUnit editView_;
  [SerializeField]
  private NGHorizontalScrollParts scroll_;
  [SerializeField]
  private UIGrid grid_;
  [SerializeField]
  private GameObject topArrow_;
  private EditCustomDeckMenu menu_;
  private PlayerCustomDeckUnit_parameter_list[] units_;
  private PopupEditCustomDeckUnit[] pages_;
  private int currentIndex_;
  private Action onClose_;
  private bool isDisabledOpenSe_;
  private int? requestChange_;
  private const string SE_PAGE = "SE_1005";

  public static GameObject show(
    GameObject prefab,
    EditCustomDeckMenu menu,
    PlayerCustomDeckUnit_parameter_list[] targets,
    int noCenter,
    Action eventClose,
    bool bDisabledOpenSe)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true);
    gameObject.GetComponent<PopupEditCustomDeck>().initialize(menu, targets, noCenter, eventClose, bDisabledOpenSe);
    return gameObject;
  }

  private void initialize(
    EditCustomDeckMenu menu,
    PlayerCustomDeckUnit_parameter_list[] targets,
    int noCenter,
    Action eventClose,
    bool bDisabledOpenSe)
  {
    ((Component) this).GetComponent<UIRect>().alpha = 0.0f;
    this.setTopObject(((Component) this).gameObject);
    this.menu_ = menu;
    this.units_ = targets;
    this.currentIndex_ = 0;
    for (int index = 0; index < targets.Length; ++index)
    {
      if (targets[index].index == noCenter)
      {
        this.currentIndex_ = index;
        break;
      }
    }
    this.onClose_ = eventClose;
    this.isDisabledOpenSe_ = bDisabledOpenSe;
  }

  private IEnumerator Start()
  {
    PopupEditCustomDeck popupEditCustomDeck = this;
    popupEditCustomDeck.pages_ = new PopupEditCustomDeckUnit[popupEditCustomDeck.units_.Length];
    popupEditCustomDeck.pages_[0] = popupEditCustomDeck.editView_;
    ((Behaviour) popupEditCustomDeck.scroll_).enabled = false;
    if (popupEditCustomDeck.units_.Length > 1)
    {
      GameObject gameObject = ((Component) popupEditCustomDeck.editView_).gameObject;
      for (int index = 1; index < popupEditCustomDeck.pages_.Length; ++index)
        popupEditCustomDeck.pages_[index] = popupEditCustomDeck.scroll_.instantiateParts(gameObject, false).GetComponent<PopupEditCustomDeckUnit>();
    }
    else
      popupEditCustomDeck.topArrow_.SetActive(false);
    for (int n = 0; n < popupEditCustomDeck.pages_.Length; ++n)
    {
      IEnumerator e = popupEditCustomDeck.pages_[n].doInitialize(popupEditCustomDeck.menu_, popupEditCustomDeck.units_[n]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    yield return (object) null;
    ((Component) popupEditCustomDeck).GetComponent<AnchorCustomAdjustment>().resetAnchors((Transform[]) null);
    yield return (object) null;
    foreach (Object componentsInChild in ((Component) popupEditCustomDeck).GetComponentsInChildren<UIDragScrollView>())
      Object.Destroy(componentsInChild);
    int num = Mathf.CeilToInt(popupEditCustomDeck.scroll_.scrollView.GetComponent<UIPanel>().width);
    popupEditCustomDeck.grid_.cellWidth = (float) ((num + 1) / 2 * 2);
    // ISSUE: method pointer
    popupEditCustomDeck.grid_.onReposition = new UIGrid.OnReposition((object) popupEditCustomDeck, __methodptr(\u003CStart\u003Eb__12_0));
    popupEditCustomDeck.grid_.Reposition();
    yield return (object) null;
    popupEditCustomDeck.changeCenter(popupEditCustomDeck.currentIndex_, false);
    ((Behaviour) popupEditCustomDeck.scroll_).enabled = true;
    popupEditCustomDeck.requestChange_ = new int?();
    yield return (object) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupEditCustomDeck).gameObject, popupEditCustomDeck.isDisabledOpenSe_);
  }

  protected override void Update()
  {
    base.Update();
    if (!this.requestChange_.HasValue)
      return;
    this.currentIndex_ = this.requestChange_.Value;
    this.changeCenter(this.currentIndex_, true);
    this.requestChange_ = new int?();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.onClose_();
  }

  private void changeCenter(int index, bool bNotification)
  {
    GameObject[] array = ((IEnumerable<PopupEditCustomDeckUnit>) this.pages_).Select<PopupEditCustomDeckUnit, GameObject>((Func<PopupEditCustomDeckUnit, GameObject>) (x => ((Component) x).gameObject)).ToArray<GameObject>();
    this.scroll_.setItemPositionQuick(index);
    int index1 = index;
    ((IEnumerable<GameObject>) array).ToggleOnce(index1);
    if (!bNotification)
      return;
    this.menu_.changeEditTarget(this.units_[index]);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1005");
  }

  public void toLeft()
  {
    if (this.currentIndex_ <= 0)
      return;
    this.requestChange_ = new int?(this.currentIndex_ - 1);
  }

  public void toRight()
  {
    if (this.currentIndex_ + 1 >= this.pages_.Length)
      return;
    this.requestChange_ = new int?(this.currentIndex_ + 1);
  }
}
