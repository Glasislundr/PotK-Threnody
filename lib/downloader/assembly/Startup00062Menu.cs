// Decompiled with JetBrains decompiler
// Type: Startup00062Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class Startup00062Menu : MonoBehaviour
{
  [SerializeField]
  protected UILabel TxtDiscription;

  public IEnumerator Start()
  {
    this.TxtDiscription.SetText(Consts.GetInstance().MAINTENANCE_WAIT_DISCRIPTION);
    IEnumerator e = this.Request();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void Init(WebAPI.Response.OfficialinfoMaintenance res)
  {
    this.TxtDiscription.SetText(Consts.Format(Consts.GetInstance().MAINTENANCE_WAIT_DISCRIPTION2, (IDictionary) new Hashtable()
    {
      {
        (object) "title",
        (object) res.message_schedule
      },
      {
        (object) "message",
        (object) res.message_body
      }
    }));
    if (res.is_maintenance)
    {
      this.StartCoroutine(this.Request(false));
    }
    else
    {
      CommonRoot instance1 = Singleton<CommonRoot>.GetInstance();
      if (Object.op_Inequality((Object) instance1, (Object) null))
        Object.Destroy((Object) ((Component) instance1).gameObject);
      NGSceneManager instance2 = Singleton<NGSceneManager>.GetInstance();
      if (Object.op_Inequality((Object) instance2, (Object) null))
        Object.Destroy((Object) ((Component) instance2).gameObject);
      SceneManager.LoadScene("startup000_6");
    }
  }

  public IEnumerator Request(bool second = true)
  {
    if (!second)
      yield return (object) new WaitForSeconds(60f);
    Future<WebAPI.Response.OfficialinfoMaintenance> fRes = WebAPI.OfficialinfoMaintenance();
    IEnumerator e = fRes.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Init(fRes.Result);
  }
}
