// Decompiled with JetBrains decompiler
// Type: Celeste.SpawnFacingTrigger
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

#nullable disable
namespace Celeste;

[Tracked(false)]
public class SpawnFacingTrigger : Entity
{
  public Facings Facing;

  public SpawnFacingTrigger(EntityData data, Vector2 offset)
    : base(data.Position + offset)
  {
    this.Collider = (Collider) new Hitbox((float) data.Width, (float) data.Height);
    this.Facing = data.Enum<Facings>("facing");
    this.Visible = this.Active = false;
  }
}
