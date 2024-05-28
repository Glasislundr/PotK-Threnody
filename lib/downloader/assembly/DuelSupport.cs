// Decompiled with JetBrains decompiler
// Type: DuelSupport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class DuelSupport : MonoBehaviour
{
  public GameObject hitStr;
  public GameObject[] aryGoHit_01;
  public GameObject[] aryGoHit_10;
  public GameObject dodgeStr;
  public GameObject[] aryGoDodge_01;
  public GameObject[] aryGoDodge_10;
  public GameObject critStr;
  public GameObject[] aryGoCritical_01;
  public GameObject[] aryGoCritical_10;
  public GameObject cridodgeStr;
  public GameObject[] aryGoCriDodge_01;
  public GameObject[] aryGoCriDodge_10;
  public float itemHeight = 10f;
  private int dispCount;

  private void Start() => this.DisableAllNumbers();

  private void DisableAllNumbers()
  {
    foreach (GameObject gameObject in this.aryGoHit_10)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.aryGoDodge_10)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.aryGoCritical_10)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.aryGoCriDodge_10)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.aryGoHit_01)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.aryGoDodge_01)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.aryGoCritical_01)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.aryGoCriDodge_01)
      gameObject.SetActive(false);
    this.hitStr.SetActive(false);
    this.dodgeStr.SetActive(false);
    this.critStr.SetActive(false);
    this.cridodgeStr.SetActive(false);
  }

  public void setNumbers(int hit, int dodge, int critical, int dodgecritical)
  {
    hit = NC.Clamp(0, 99, hit);
    dodge = NC.Clamp(0, 99, dodge);
    critical = NC.Clamp(0, 99, critical);
    dodgecritical = NC.Clamp(0, 99, dodgecritical);
    if (hit == 0 && dodge == 0 && critical == 0 && dodgecritical == 0)
      return;
    this.itemHeight *= ((Component) this).gameObject.transform.localScale.x;
    this.setNumber(this.aryGoHit_10, this.aryGoHit_01, hit, this.hitStr);
    this.setNumber(this.aryGoDodge_10, this.aryGoDodge_01, dodge, this.dodgeStr);
    this.setNumber(this.aryGoCritical_10, this.aryGoCritical_01, critical, this.critStr);
    this.setNumber(this.aryGoCriDodge_10, this.aryGoCriDodge_01, dodgecritical, this.cridodgeStr);
  }

  private void setNumber(
    GameObject[] target_10,
    GameObject[] target_01,
    int number,
    GameObject strGO)
  {
    if (number == 0)
      return;
    int[] array = this.numToArray(number);
    if (array[1] != 0)
      target_10[array[1]].SetActive(true);
    target_01[array[0]].SetActive(true);
    strGO.SetActive(true);
    Vector3 position = ((Component) strGO.transform.parent).gameObject.transform.position;
    position.y -= this.itemHeight * (float) this.dispCount;
    ((Component) strGO.transform.parent).gameObject.transform.position = position;
    ++this.dispCount;
  }

  private int[] numToArray(int num)
  {
    int num1 = num % 100;
    int[] array = new int[2]{ 0, num1 / 10 };
    array[0] = num1 % 10;
    return array;
  }

  private IEnumerator disableThis(float duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DuelSupport duelSupport = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Component) duelSupport).gameObject.SetActive(false);
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
}
