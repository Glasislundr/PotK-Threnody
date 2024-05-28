// Decompiled with JetBrains decompiler
// Type: HpNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HpNumber : BattleMonoBehaviour
{
  [SerializeField]
  protected GameObject base_root;
  [SerializeField]
  protected GameObject[] base_origin = new GameObject[10];
  [SerializeField]
  protected GameObject[] base_keta_roots;
  protected GameObject[][] base_nums;
  [SerializeField]
  protected GameObject damage_root;
  [SerializeField]
  protected GameObject[] damage_origin = new GameObject[10];
  [SerializeField]
  protected GameObject[] damage_keta_roots;
  protected GameObject[][] damage_nums;
  [SerializeField]
  protected GameObject heal_root;
  [SerializeField]
  protected GameObject[] heal_origin = new GameObject[10];
  [SerializeField]
  protected GameObject[] heal_keta_roots;
  protected GameObject[][] heal_nums;
  [SerializeField]
  protected Animator animator;
  private int startHp;
  private int targetHp;
  private float interval;

  private void Awake()
  {
    this.initNumberObjects(out this.base_nums, this.base_origin, this.base_keta_roots);
    this.initNumberObjects(out this.damage_nums, this.damage_origin, this.damage_keta_roots);
    this.initNumberObjects(out this.heal_nums, this.heal_origin, this.heal_keta_roots);
  }

  private void initNumberObjects(
    out GameObject[][] numbers,
    GameObject[] origins,
    GameObject[] ketaRoots)
  {
    numbers = new GameObject[ketaRoots.Length][];
    numbers[0] = origins;
    for (int index1 = 1; index1 < ketaRoots.Length; ++index1)
    {
      numbers[index1] = new GameObject[10];
      for (int index2 = 0; index2 < 10; ++index2)
        numbers[index1][index2] = origins[index2].Clone(ketaRoots[index1].transform);
    }
  }

  public void setUnit(BL.Unit unit)
  {
    ((Component) this).gameObject.SetActive(false);
    this.base_root.SetActive(false);
    this.damage_root.SetActive(false);
    this.heal_root.SetActive(false);
  }

  public void StartAnime(int prev_hp, int new_hp)
  {
    ((Component) this).gameObject.SetActive(true);
    if (prev_hp != new_hp)
    {
      int num = prev_hp - new_hp;
      this.startHp = prev_hp;
      this.targetHp = new_hp;
      if (num > 0)
      {
        this.base_root.SetActive(true);
        this.damage_root.SetActive(true);
        this.SetBaseNumber(prev_hp);
        this.SetDamageNumber(Mathf.Abs(num));
        this.animator.Play("Damage", -1, 0.0f);
      }
      else
      {
        if (num >= 0)
          return;
        this.base_root.SetActive(true);
        this.heal_root.SetActive(true);
        this.SetBaseNumber(prev_hp);
        this.SetHealNumber(Mathf.Abs(num));
        this.animator.Play("Heal", -1, 0.0f);
      }
    }
    else
    {
      this.base_root.SetActive(true);
      this.SetBaseNumber(prev_hp);
      this.animator.Play("Stay", -1, 0.0f);
    }
  }

  public void EndAnime()
  {
    this.animator.StopPlayback();
    ((Component) this).gameObject.SetActive(false);
  }

  private void SetBaseNumber(int _number)
  {
    this.SetNumber(_number, this.base_keta_roots, this.base_nums);
  }

  private void SetDamageNumber(int _number)
  {
    this.SetNumber(_number, this.damage_keta_roots, this.damage_nums);
  }

  private void SetHealNumber(int _number)
  {
    this.SetNumber(_number, this.heal_keta_roots, this.heal_nums);
  }

  private void SetNumber(int _number, GameObject[] keta_roots, GameObject[][] numbers)
  {
    int num1 = 9;
    for (int index = 1; index < keta_roots.Length; ++index)
      num1 = num1 * 10 + 9;
    int num2 = Mathf.Clamp(_number, 0, num1);
    int num3 = 1;
    for (int index1 = 0; index1 < keta_roots.Length; ++index1)
    {
      ((IEnumerable<GameObject>) numbers[index1]).ForEach<GameObject>((Action<GameObject>) (obj => obj.SetActive(false)));
      if (num2 >= num3 || index1 == 0)
      {
        int index2 = num2 / num3 % 10;
        numbers[index1][index2].SetActive(true);
      }
      num3 *= 10;
    }
  }

  public void StartCounter(float time)
  {
    int num = this.targetHp - this.startHp;
    this.interval = time / (float) Mathf.Abs(num);
    if (num > 0)
    {
      this.StartCoroutine(this.CountUp());
    }
    else
    {
      if (num >= 0)
        return;
      this.StartCoroutine(this.CountDown());
    }
  }

  private IEnumerator CountUp()
  {
    int addHp = 0;
    float total_time = 0.0f;
    while (this.startHp + addHp <= this.targetHp)
    {
      addHp = Mathf.CeilToInt(total_time / this.interval);
      this.SetBaseNumber(Mathf.Min(this.startHp + addHp, this.targetHp));
      yield return (object) null;
      total_time += Time.deltaTime;
    }
  }

  private IEnumerator CountDown()
  {
    int addHp = 0;
    float total_time = 0.0f;
    while (this.startHp - addHp >= this.targetHp)
    {
      addHp = Mathf.CeilToInt(total_time / this.interval);
      this.SetBaseNumber(Mathf.Max(this.startHp - addHp, this.targetHp));
      yield return (object) null;
      total_time += Time.deltaTime;
    }
  }
}
