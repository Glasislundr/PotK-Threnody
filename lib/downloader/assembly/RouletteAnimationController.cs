// Decompiled with JetBrains decompiler
// Type: RouletteAnimationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RouletteAnimationController : MonoBehaviour
{
  [SerializeField]
  private RouletteMenu rouletteMenu;
  private Animator animator;
  private UILabel serifLabel;
  private List<string> characterSefits = new List<string>();
  private int currentSerifIndex;
  public float enableAllButtonsDelay = 1f;

  public void Init(Animator animator, UILabel serifLabel, List<string> characterSefits)
  {
    this.animator = animator;
    this.serifLabel = serifLabel;
    this.characterSefits = characterSefits;
  }

  public void CanRoulettePlay()
  {
    this.currentSerifIndex = 0;
    this.serifLabel.SetTextLocalize(this.characterSefits[this.currentSerifIndex]);
    this.animator.SetTrigger("FadeIn");
  }

  public void CanNotRoulettePlay() => this.animator.SetTrigger("Close");

  public void OnTapOttimo()
  {
    ++this.currentSerifIndex;
    if (this.currentSerifIndex < this.characterSefits.Count)
    {
      this.serifLabel.SetTextLocalize(this.characterSefits[this.currentSerifIndex]);
    }
    else
    {
      if (this.currentSerifIndex != this.characterSefits.Count)
        return;
      this.animator.SetTrigger("OttimoOut");
      this.rouletteMenu.SetButtonsAvailbility(true, this.enableAllButtonsDelay);
    }
  }

  public void OnTapCurtain() => this.animator.SetTrigger("Skip");
}
