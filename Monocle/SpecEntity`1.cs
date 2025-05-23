// Decompiled with JetBrains decompiler
// Type: Monocle.SpecEntity`1
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;

namespace Monocle
{

    public class SpecEntity<T> : Entity where T : Scene
    {
      public T SpecScene { get; private set; }

      public SpecEntity(Vector2 position)
        : base(position)
      {
      }

      public SpecEntity()
      {
      }

      public override void Added(Scene scene)
      {
        base.Added(scene);
        if (!(this.Scene is T))
          return;
        this.SpecScene = this.Scene as T;
      }

      public override void Removed(Scene scene)
      {
        this.SpecScene = default (T);
        base.Removed(scene);
      }
    }
}
