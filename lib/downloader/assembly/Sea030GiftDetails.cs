// Decompiled with JetBrains decompiler
// Type: Sea030GiftDetails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030GiftDetails : MonoBehaviour
{
  [SerializeField]
  public GameObject scrollContainer;
  [SerializeField]
  public GameObject attention;
  private Sea030GiftListMenu giftListMenu;

  public IEnumerator ShowWindow(GearGear item, bool isGray)
  {
    Sea030GiftDetails menu = this;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    ((Component) menu).gameObject.SetActive(true);
    Sea030GiftListMenu.isCallGiftRecipeWindowOpen = true;
    if (Sea030GiftListMenu.isCreateCallItem)
      menu.StartCoroutine(menu.giftListMenu.ReInit());
    IEnumerator e = menu.scrollContainer.GetComponent<Sea030GiftDetailsScrollContainer>().Init(item, menu, isGray);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void SetMenu(Sea030GiftListMenu menu) => this.giftListMenu = menu;

  private void Update()
  {
  }

  public void Hide()
  {
    Sea030GiftListMenu.initPopupGear = (GearGear) null;
    NGxScrollMasonry component = this.scrollContainer.GetComponent<NGxScrollMasonry>();
    ((Component) component.Scroll).transform.Clear();
    component.Reset();
    ((Component) component.Scroll).gameObject.SetActive(false);
    this.giftListMenu.Hide();
  }
}
