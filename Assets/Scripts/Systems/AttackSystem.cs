using ECS;
using UnityEngine;

namespace Game
{
	public class AttackSystem : ComponentSystem, IInitialize
	{
		private Constants _constants;

		public void Initialize()
		{
			_world.All<Attack>().OnAdded.Bind(OnAttack);
		}

		private void OnAttack(int attackEntity)
		{
			_world.Get(attackEntity, out Attack attack);
			_world.Get(attack.fromEntity, out Number fromNumber);
			_world.Get(attack.toEntity, out Number toNumber, out HP toHP);

			var dmgIndex = (3 + fromNumber.value - toNumber.value) % 3;
			var dmg = _constants.GetDmg(dmgIndex);

			toHP.value = Mathf.Max(toHP.value - dmg, 0);
			_world.Replace(attack.toEntity, toHP);
		}
	}
}