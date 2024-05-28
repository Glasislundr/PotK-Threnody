// Decompiled with JetBrains decompiler
// Type: CallSkillCutin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class CallSkillCutin : MonoBehaviour
{
  [SerializeField]
  private MeshRenderer cutinObject;

  public static IEnumerator Show(int same_character_id)
  {
    Future<GameObject> prefabF = new ResourceObject("Animations/battle_cutin/battle_cutin_call").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject obj = prefabF.Result.Clone();
    e = obj.GetComponent<CallSkillCutin>().Initialize(same_character_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1076");
    yield return (object) new WaitForSeconds(2.5f);
    Object.Destroy((Object) obj);
  }

  public IEnumerator Initialize(int same_character_id)
  {
    if (!Object.op_Equality((Object) this.cutinObject, (Object) null))
    {
      UnitUnit unitUnit1 = (UnitUnit) null;
      foreach (UnitUnit unitUnit2 in MasterData.UnitUnitList)
      {
        if (unitUnit2.same_character_id == same_character_id && (unitUnit1 == null || unitUnit1.ID > unitUnit2.ID))
          unitUnit1 = unitUnit2;
      }
      string eventImageName = unitUnit1.getEventImageName();
      if (eventImageName != null)
      {
        Future<Sprite> fs = Singleton<ResourceManager>.GetInstance().Load<Sprite>(eventImageName);
        IEnumerator e = fs.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Texture texture = (Texture) fs.Result.texture;
        if (Object.op_Inequality((Object) texture, (Object) null))
          ((Renderer) this.cutinObject).materials[0].SetTexture("_MainTex", texture);
      }
    }
  }
}
