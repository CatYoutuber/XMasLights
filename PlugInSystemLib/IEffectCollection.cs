namespace XMasLights.PlugInSystem
{
	public interface IEffectCollection
	{
		ILightsEffect[] GetEffects();
		string GetPlugInName();
		string GetPlugInDescription();
		bool HasWebsite();
		string GetPlugInWebsite();
	}
}
