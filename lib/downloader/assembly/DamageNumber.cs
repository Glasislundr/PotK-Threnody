// Decompiled with JetBrains decompiler
// Type: DamageNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DamageNumber : MonoBehaviour
{
  private const int KETA_MAX = 8;
  [SerializeField]
  private GameObject[] num_0;
  [SerializeField]
  private GameObject[] num_00;
  [SerializeField]
  private GameObject[] num_000;
  [SerializeField]
  private GameObject[] num_0000;
  [SerializeField]
  private GameObject[] num_00000;
  [SerializeField]
  private GameObject[] num_000000;
  [SerializeField]
  private GameObject[] num_0000000;
  [SerializeField]
  private GameObject[] num_00000000;
  private GameObject[][] numbers = new GameObject[8][];

  private void Awake()
  {
    this.numbers[0] = this.num_0;
    this.numbers[1] = this.num_00;
    this.numbers[2] = this.num_000;
    this.numbers[3] = this.num_0000;
    this.numbers[4] = this.num_00000;
    this.numbers[5] = this.num_000000;
    this.numbers[6] = this.num_0000000;
    this.numbers[7] = this.num_00000000;
  }

  public void setDamage(int damage)
  {
    this.DisableNumbers();
    if (damage == 0)
      return;
    int num1 = 9;
    for (int index = 1; index < 8; ++index)
      num1 = num1 * 10 + 9;
    damage = Mathf.Clamp(damage, 0, num1);
    int num2 = 1;
    for (int index1 = 0; index1 < 8; ++index1)
    {
      if (damage >= num2 || index1 == 0)
      {
        int index2 = damage / num2 % 10;
        this.numbers[index1][index2].SetActive(true);
      }
      num2 *= 10;
    }
    ((Component) this).gameObject.SetActive(true);
    this.StartCoroutine(this.disActiveSelf(2f));
  }

  private IEnumerator disActiveSelf(float duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DamageNumber damageNumber = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Component) damageNumber).gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void DisableNumbers()
  {
    foreach (GameObject[] number in this.numbers)
    {
      foreach (GameObject gameObject in number)
        gameObject.SetActive(false);
    }
  }
}
