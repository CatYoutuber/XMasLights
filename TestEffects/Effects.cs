using System;
using System.Drawing;
using XMasLights;
using XMasLights.PlugInSystem;

namespace TestEffects
{
	class Effect1 : ILightsEffect
	{
		public void FillColors(Color[] arr, int nLights)
		{
			Color[] cls = { Color.Red, Color.Lime, Color.Blue, Color.Aqua, Color.Yellow, Color.Magenta };
			for(int i = 0; i < nLights; i++)
				arr[i] = cls[(Environment.TickCount / 500) % 6];
		}

		public string GetDescription()
		{
			return "Дескрипшин";
		}

		public string GetName()
		{
			return "Нейм";
		}

		public int GetRequiredFrameRate()
		{
			return 10;
		}
	}
}