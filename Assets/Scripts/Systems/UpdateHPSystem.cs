using ECS;

namespace Game
{
	public class UpdateHPUISystem : ComponentSystem, IInitialize
	{
		public void Initialize()
		{
			_world.ForEach((BaseHP baseHP, HPRenderer renderer) => {
				renderer.SetBaseHP(baseHP.value);
			});

			_world.All<HP, HPRenderer>().OnReplaced.Bind(OnHPChanged);
		}

		private void OnHPChanged(int entity)
		{
			_world.Get(entity, out HP hp, out HPRenderer hpRenderer);
			hpRenderer.SetHP(hp.value);
		}
	}
}