// Decompiled with JetBrains decompiler
// Type: Raid032BattleBossAnime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032BattleBossAnime : MonoBehaviour
{
  [SerializeField]
  private SpriteRenderer mImage;
  [SerializeField]
  private Animator mImageAnimator;
  [SerializeField]
  private Transform mImageOffsetAnchor;

  public IEnumerator InitializeAsync(UnitUnit unit, float offsetY)
  {
    Future<Sprite> future = unit.LoadSpriteLarge();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mImage.sprite = future.Result;
    Vector3 localPosition = this.mImageOffsetAnchor.localPosition;
    this.mImageOffsetAnchor.localPosition = new Vector3(localPosition.x, localPosition.y + offsetY, localPosition.z);
    this.StartAnime();
  }

  public void StartAnime() => this.mImageAnimator.SetTrigger("survive");
}
