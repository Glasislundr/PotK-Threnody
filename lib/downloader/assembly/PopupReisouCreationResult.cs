// Decompiled with JetBrains decompiler
// Type: PopupReisouCreationResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupReisouCreationResult : MonoBehaviour
{
  [SerializeField]
  protected GameObject dirIcon;
  [SerializeField]
  protected UILabel txtDescription;

  public IEnumerator Init(PlayerItem item, int create_count)
  {
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon itemIcon = prefabF.Result.Clone(this.dirIcon.transform).GetComponent<ItemIcon>();
    e = itemIcon.InitByPlayerItem(item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableLongPressEventReisou(item);
    this.txtDescription.SetText(Consts.GetInstance().POPUP_REISOU_CREATION_RESULT_DESCRIPTION.F((object) item.name, (object) create_count));
  }

  public void onBtnYes() => Singleton<PopupManager>.GetInstance().closeAll();
}
