// Decompiled with JetBrains decompiler
// Type: RepairObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class RepairObject
{
  public Animator animator;
  private string skipAnimation = "";
  public GameObject itemSum;
  public AnimationItemIcon itemSumType;
  public GameObject lostSum;
  public AnimationItemIcon lostSumType;
  public GameObject getSum1;
  public AnimationItemIcon getSum1Type;
  public GameObject getSum2;
  public AnimationItemIcon getSum2Type;
  public GameObject normalSum;
  public AnimationItemIcon normalSumType;

  public GameCore.ItemInfo itemInfo { get; set; }

  public WebAPI.Response.ItemGearRepairRepair_results result { get; set; }

  public WebAPI.Response.ItemGearRepairListRepair_results result_powered { get; set; }

  public void SkipEffect() => this.animator.Play(this.skipAnimation, 0, 0.0f);

  public void ObjectOff() => this.itemSum.SetActive(false);

  public void Normal(GameObject obj, GameObject normal)
  {
    this.itemSum.SetActive(true);
    this.normalSum = normal.Clone(this.itemSum.transform);
    this.animator = this.normalSum.GetComponent<Animator>();
    this.skipAnimation = "normal_sum_11";
    this.itemSumType = this.SetCloneIcon(this.itemSumType, this.itemSum.transform, obj);
    this.normalSumType = this.SetCloneIcon(this.normalSumType, this.normalSum.transform, obj);
    this.itemSumType.SetBroken(true);
    this.normalSumType.SetBroken(true);
  }

  public void Success(GameObject obj, GameObject success)
  {
    this.itemSum.SetActive(true);
    this.getSum1 = success.Clone(this.itemSum.transform);
    this.animator = this.getSum1.GetComponent<Animator>();
    this.skipAnimation = "get_sum_11";
    this.itemSumType = this.SetCloneIcon(this.itemSumType, this.itemSum.transform, obj);
    this.getSum1Type = this.SetCloneIcon(this.getSum1Type, this.getSum1.transform, obj);
    this.itemSumType.SetBroken(true);
    this.getSum1Type.SetBroken(false);
  }

  public void Lost(GameObject obj, GameObject lost)
  {
    this.itemSum.SetActive(true);
    this.lostSum = lost.Clone(this.itemSum.transform);
    this.animator = this.lostSum.GetComponent<Animator>();
    this.skipAnimation = "lost_sum_11";
    this.itemSumType = this.SetCloneIcon(this.itemSumType, this.itemSum.transform, obj);
    this.lostSumType = this.SetCloneIcon(this.lostSumType, this.lostSum.transform, obj);
    this.itemSumType.SetBroken(true);
    this.lostSumType.SetBroken(true);
  }

  public void SetTexture(Texture texture)
  {
    if (Object.op_Inequality((Object) this.itemSum, (Object) null) && this.itemSum.activeSelf)
      ((Renderer) this.itemSum.GetComponent<MeshRenderer>()).material.mainTexture = texture;
    if (Object.op_Inequality((Object) this.lostSum, (Object) null) && this.lostSum.activeSelf)
      ((Renderer) this.lostSum.GetComponent<MeshRenderer>()).material.mainTexture = texture;
    if (Object.op_Inequality((Object) this.getSum1, (Object) null) && this.getSum1.activeSelf)
      ((Renderer) this.getSum1.GetComponent<MeshRenderer>()).material.mainTexture = texture;
    if (Object.op_Inequality((Object) this.getSum2, (Object) null) && this.getSum2.activeSelf)
      ((Renderer) this.getSum2.GetComponent<MeshRenderer>()).material.mainTexture = texture;
    if (!Object.op_Inequality((Object) this.normalSum, (Object) null) || !this.normalSum.activeSelf)
      return;
    ((Renderer) this.normalSum.GetComponent<MeshRenderer>()).material.mainTexture = texture;
  }

  private AnimationItemIcon SetCloneIcon(AnimationItemIcon icon, Transform trans, GameObject obj)
  {
    if (Object.op_Equality((Object) icon, (Object) null))
      icon = obj.Clone(((Component) trans).transform).GetComponent<AnimationItemIcon>();
    ((Component) icon).transform.localPosition = new Vector3(0.0f, 0.0f, -0.05f);
    ((Object) ((Component) icon).gameObject).name = "AnimationItemIcon";
    icon.Set(this.itemInfo);
    return icon;
  }
}
