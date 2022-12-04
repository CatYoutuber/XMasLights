using System;
using XMasLights;
using XMasLights.PlugInSystem;

namespace TestEffects
{
	class TestEffects : IEffectCollection
	{
		public ILightsEffect[] GetEffects()
		{
			return new ILightsEffect[] { new Effect1() };
		}

        public string GetPlugInDescription()
        {
			return "Тест Еффектс Плагин дескрипшын";
        }

        public string GetPlugInName()
		{
			return "Тест Еффектс Плагин бай КетЮтубер";
		}
		
	}
}
