// Decompiled with JetBrains decompiler
// Type: Versus02611ClassList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02611ClassList : MonoBehaviour
{
  [SerializeField]
  private GameObject unlockClass;
  [SerializeField]
  private GameObject currentClass;
  [SerializeField]
  private GameObject lockClass;
  [SerializeField]
  private UILabel txtName;
  private int id;
  private int best_class;

  public IEnumerator Init(int id, string name, int current_id, bool isLock, int best_class)
  {
    this.id = id;
    this.best_class = best_class;
    bool flag = current_id == id;
    this.txtName.SetText(name);
    this.currentClass.SetActive(flag);
    this.unlockClass.SetActive(!isLock && !flag);
    this.lockClass.SetActive(isLock && !flag);
    yield break;
  }

  public void IbtnChangeScene() => Versus02612Scene.ChangeScene(true, this.id, this.best_class);
}
