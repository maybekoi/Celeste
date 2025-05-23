// Decompiled with JetBrains decompiler
// Type: Celeste.FlyFeather
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;

namespace Celeste
{

    [Tracked(false)]
    public class FlyFeather : Entity
    {
      public static ParticleType P_Collect;
      public static ParticleType P_Boost;
      public static ParticleType P_Flying;
      public static ParticleType P_Respawn;
      private const float RespawnTime = 3f;
      private Sprite sprite;
      private Monocle.Image outline;
      private Wiggler wiggler;
      private BloomPoint bloom;
      private VertexLight light;
      private Level level;
      private SineWave sine;
      private bool shielded;
      private bool singleUse;
      private Wiggler shieldRadiusWiggle;
      private Wiggler moveWiggle;
      private Vector2 moveWiggleDir;
      private float respawnTimer;

      public FlyFeather(Vector2 position, bool shielded, bool singleUse)
        : base(position)
      {
        this.shielded = shielded;
        this.singleUse = singleUse;
        this.Collider = (Collider) new Hitbox(20f, 20f, -10f, -10f);
        this.Add((Component) new PlayerCollider(new Action<Player>(this.OnPlayer)));
        this.Add((Component) (this.sprite = GFX.SpriteBank.Create("flyFeather")));
        this.Add((Component) (this.wiggler = Wiggler.Create(1f, 4f, (Action<float>) (v => this.sprite.Scale = Vector2.One * (float) (1.0 + (double) v * 0.20000000298023224)))));
        this.Add((Component) (this.bloom = new BloomPoint(0.5f, 20f)));
        this.Add((Component) (this.light = new VertexLight(Color.White, 1f, 16 /*0x10*/, 48 /*0x30*/)));
        this.Add((Component) (this.sine = new SineWave(0.6f).Randomize()));
        this.Add((Component) (this.outline = new Monocle.Image(GFX.Game["objects/flyFeather/outline"])));
        this.outline.CenterOrigin();
        this.outline.Visible = false;
        this.shieldRadiusWiggle = Wiggler.Create(0.5f, 4f);
        this.Add((Component) this.shieldRadiusWiggle);
        this.moveWiggle = Wiggler.Create(0.8f, 2f);
        this.moveWiggle.StartZero = true;
        this.Add((Component) this.moveWiggle);
        this.UpdateY();
      }

      public FlyFeather(EntityData data, Vector2 offset)
        : this(data.Position + offset, data.Bool(nameof (shielded)), data.Bool(nameof (singleUse)))
      {
      }

      public override void Added(Scene scene)
      {
        base.Added(scene);
        this.level = this.SceneAs<Level>();
      }

      public override void Update()
      {
        base.Update();
        if ((double) this.respawnTimer > 0.0)
        {
          this.respawnTimer -= Engine.DeltaTime;
          if ((double) this.respawnTimer <= 0.0)
            this.Respawn();
        }
        this.UpdateY();
        this.light.Alpha = Calc.Approach(this.light.Alpha, this.sprite.Visible ? 1f : 0.0f, 4f * Engine.DeltaTime);
        this.bloom.Alpha = this.light.Alpha * 0.8f;
      }

      public override void Render()
      {
        base.Render();
        if (!this.shielded || !this.sprite.Visible)
          return;
        Draw.Circle(this.Position + this.sprite.Position, (float) (10.0 - (double) this.shieldRadiusWiggle.Value * 2.0), Color.White, 3);
      }

      private void Respawn()
      {
        if (this.Collidable)
          return;
        this.outline.Visible = false;
        this.Collidable = true;
        this.sprite.Visible = true;
        this.wiggler.Start();
        Audio.Play("event:/game/06_reflection/feather_reappear", this.Position);
        this.level.ParticlesFG.Emit(FlyFeather.P_Respawn, 16 /*0x10*/, this.Position, Vector2.One * 2f);
      }

      private void UpdateY()
      {
        this.sprite.X = 0.0f;
        this.sprite.Y = this.bloom.Y = this.sine.Value * 2f;
        Sprite sprite = this.sprite;
        sprite.Position = sprite.Position + this.moveWiggleDir * this.moveWiggle.Value * -8f;
      }

      private void OnPlayer(Player player)
      {
        Vector2 speed = player.Speed;
        if (this.shielded && !player.DashAttacking)
        {
          player.PointBounce(this.Center);
          this.moveWiggle.Start();
          this.shieldRadiusWiggle.Start();
          this.moveWiggleDir = (this.Center - player.Center).SafeNormalize(Vector2.UnitY);
          Audio.Play("event:/game/06_reflection/feather_bubble_bounce", this.Position);
          Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
        }
        else
        {
          bool flag = player.StateMachine.State == 19;
          if (!player.StartStarFly())
            return;
          if (!flag)
            Audio.Play(this.shielded ? "event:/game/06_reflection/feather_bubble_get" : "event:/game/06_reflection/feather_get", this.Position);
          else
            Audio.Play(this.shielded ? "event:/game/06_reflection/feather_bubble_renew" : "event:/game/06_reflection/feather_renew", this.Position);
          this.Collidable = false;
          this.Add((Component) new Coroutine(this.CollectRoutine(player, speed)));
          if (this.singleUse)
            return;
          this.outline.Visible = true;
          this.respawnTimer = 3f;
        }
      }

      private IEnumerator CollectRoutine(Player player, Vector2 playerSpeed)
      {
        FlyFeather flyFeather = this;
        flyFeather.level.Shake();
        flyFeather.sprite.Visible = false;
        yield return (object) 0.05f;
        float direction = !(playerSpeed != Vector2.Zero) ? (flyFeather.Position - player.Center).Angle() : playerSpeed.Angle();
        flyFeather.level.ParticlesFG.Emit(FlyFeather.P_Collect, 10, flyFeather.Position, Vector2.One * 6f);
        SlashFx.Burst(flyFeather.Position, direction);
      }
    }
}
