// Decompiled with JetBrains decompiler
// Type: BattleFiledSkillIconEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleFiledSkillIconEffect : MonoBehaviour
{
  [SerializeField]
  private Renderer renderer;
  [SerializeField]
  private Animator animator;

  public void SetSkillIcon(BattleskillSkill skill) => this.StartCoroutine(this.set(skill));

  private IEnumerator set(BattleskillSkill skill)
  {
    Material cpyMaterial = new Material(this.renderer.material);
    Future<Sprite> f = skill.LoadBattleSkillIcon();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    cpyMaterial.mainTexture = (Texture) f.Result.texture;
    if (Object.op_Inequality((Object) this.renderer.material, (Object) null))
      Object.Destroy((Object) this.renderer.material);
    this.renderer.material = cpyMaterial;
    this.animator.SetTrigger("play");
  }
}
