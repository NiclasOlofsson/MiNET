using System;
using System.Numerics;
using MiNET.Entities.Passive;

namespace MiNET.Entities.Behaviors
{
	public class JumpAttackBehavior : IBehavior
	{
		private readonly Wolf _wolf;
		private readonly double _leapHeight;

		public JumpAttackBehavior(Wolf wolf, double leapHeight)
		{
			_wolf = wolf;
			_leapHeight = leapHeight;
		}

		public bool ShouldStart()
		{
			if (_wolf.Target == null) return false;
			if (_wolf.Target.HealthManager.IsDead) return false;

			var distance = _wolf.DistanceTo(_wolf.Target);

			return distance >= 4 && distance <= 16 && _wolf.IsOnGround && new Random().Next(5) == 0;
		}

		public bool CanContinue()
		{
			return !_wolf.IsOnGround;
		}

		public void OnTick()
		{
			var direction = (Vector3) _wolf.Target.KnownPosition - _wolf.KnownPosition;
			var distance = _wolf.DistanceTo(_wolf.Target);

			var velocity = _wolf.Velocity;
			var x = direction.X/distance*0.5D*0.8 + velocity.X*0.2;
			var z = direction.Z/distance*0.5D*0.8 + velocity.Z*0.2;
			var y = _leapHeight;

			_wolf.Velocity += new Vector3((float) x, (float) y, (float) z);
		}

		public void OnEnd()
		{
		}
	}
}