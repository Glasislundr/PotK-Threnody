// Decompiled with JetBrains decompiler
// Type: MasterDataTable.LoginbonusLoginbonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class LoginbonusLoginbonus
  {
    public int ID;
    public string name;
    public bool is_loop;
    public int draw_type_LoginbonusDrawType;
    public bool require_continue_login;
    public DateTime? start_at;
    public DateTime? end_at;
    public int draw_reward_num;

    public static LoginbonusLoginbonus Parse(MasterDataReader reader)
    {
      return new LoginbonusLoginbonus()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        is_loop = reader.ReadBool(),
        draw_type_LoginbonusDrawType = reader.ReadInt(),
        require_continue_login = reader.ReadBool(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        draw_reward_num = reader.ReadInt()
      };
    }

    public LoginbonusDrawType draw_type => (LoginbonusDrawType) this.draw_type_LoginbonusDrawType;
  }
}
