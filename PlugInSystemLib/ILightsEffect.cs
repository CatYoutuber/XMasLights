using System.Drawing;

namespace XMasLights.PlugInSystem
{
	public interface ILightsEffect
	{
		void FillColors(Color[] arr, int nLights);
		string GetName();
		string GetDescription();
		int GetRequiredFrameRate();
	}
}
