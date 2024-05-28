// Decompiled with JetBrains decompiler
// Type: Story0096EpisodeBlockParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story0096EpisodeBlockParts : MonoBehaviour
{
  [SerializeField]
  private GameObject[] DirEpisodenum;
  [SerializeField]
  private UIButton IbtnEpisodeBlock;

  public IEnumerator setData(int id)
  {
    foreach (GameObject gameObject in this.DirEpisodenum)
      gameObject.SetActive(false);
    Future<GameObject> Prefab0097F = Res.Prefabs.popup.popup_002_15_2__anim_popup01.Load<GameObject>();
    IEnumerator e = Prefab0097F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject Prefab0097 = Prefab0097F.Result;
    this.DirEpisodenum[id].SetActive(true);
    EventDelegate.Set(this.IbtnEpisodeBlock.onClick, (EventDelegate.Callback) (() => Singleton<PopupManager>.GetInstance().openAlert(Prefab0097)));
  }
}
