// Decompiled with JetBrains decompiler
// Type: Shop00742SeasonTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00742SeasonTicket : MonoBehaviour
{
  [SerializeField]
  protected UILabel TxtFlavor;
  [SerializeField]
  protected UILabel TxtName;
  [SerializeField]
  protected UI2DSprite SlcTarget;
  private float scale = 0.7f;

  public IEnumerator Init(int entity_id)
  {
    SeasonTicketSeasonTicket ticketSeasonTicket = MasterData.SeasonTicketSeasonTicket[entity_id];
    this.TxtFlavor.SetText(ticketSeasonTicket.description);
    this.TxtName.SetText(ticketSeasonTicket.name);
    Future<Sprite> spriteF = ticketSeasonTicket.LoadLargeF();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = spriteF.Result;
    UI2DSprite slcTarget1 = this.SlcTarget;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) slcTarget1).width = num1;
    UI2DSprite slcTarget2 = this.SlcTarget;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) slcTarget2).height = num2;
    ((Component) this.SlcTarget).transform.localScale = new Vector3(this.scale, this.scale);
    spriteF = (Future<Sprite>) null;
  }
}
