using ECS;

namespace Game
{
	public class UpdateHPUISystem : ComponentSystem, IInitialize
	{
		public void Initialize()
		{
			_world.All<BaseHP, HPRenderer>().OnAdded.Bind(OnHPInit);
			_world.All<HP, HPRenderer>().OnReplaced.Bind(OnHPChanged);
		}

		private void OnHPInit(int entity)
		{
			_world.Get(entity, out BaseHP baseHp, out HPRenderer hpRenderer);
			hpRenderer.SetBaseHP(baseHp.value);
		}

		private void OnHPChanged(int entity)
		{
			_world.Get(entity, out HP hp, out HPRenderer hpRenderer);
			hpRenderer.SetHP(hp.value);
		}
	}
}