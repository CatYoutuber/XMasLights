using CoreAudioApi;
using System;
using System.Drawing;
using System.Diagnostics;
using XMasLights.PlugInSystem;

namespace ColorMusicEffect
{
	public class ColorMusicEffects : IEffectCollection
	{
		public static MMDevice mmdevice;
		public static byte requiredFramerate = 20;
		public ColorMusicEffects()
		{
			mmdevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
			/*string str = "";
			for (int i = 0; i < mmdevice.AudioMeterInformation.PeakValues.Count; i++)
				str += "ID: " + i + " Value: " + mmdevice.AudioMeterInformation.PeakValues[i] + "\n";
			Debug.WriteLine(str);*/
		}
		public ILightsEffect[] GetEffects()
		{
			return new ILightsEffect[] { new Normal(), new Colored(), new SyncronousColored(), new _2Channels() };
		}

		public string GetPlugInName() => "ColorMusic Effects";
		public string GetPlugInDescription() => "ColorMusic Effects by CatYoutuber";

		public static Color HsvToRgb(double h, double s, double v)
			{
				int hi = (int)Math.Floor(h / 60.0) % 6;
				double f = (h / 60.0) - Math.Floor(h / 60.0);

				double p = v * (1.0 - s);
				double q = v * (1.0 - (f * s));
				double t = v * (1.0 - ((1.0 - f) * s));

				Color ret;

				switch (hi)
				{
					case 0:
						ret = GetRgb(v, t, p);
						break;
					case 1:
						ret = GetRgb(q, v, p);
						break;
					case 2:
						ret = GetRgb(p, v, t);
						break;
					case 3:
						ret = GetRgb(p, q, v);
						break;
					case 4:
						ret = GetRgb(t, p, v);
						break;
					case 5:
						ret = GetRgb(v, p, q);
						break;
					default:
						ret = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
						break;
				}
				return ret;
			}
		public static Color GetRgb(double r, double g, double b) => Color.FromArgb(255, (byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
		public class Normal : ILightsEffect
		{
			public void FillColors(Color[] arr, int nLights)
			{
				int enabledLights = (int)(mmdevice.AudioMeterInformation.MasterPeakValue * nLights);
				for (int i = 0; i < nLights; i++)
					arr[i] = Color.FromArgb(64, 64, 64);
				for (int i = 0; i < enabledLights; i++)
					arr[i] = Color.White;
			}

			public string GetDescription()
			{
				return "Enables lights from left to right by loudness";
			}

			public string GetName()
			{
				return "Normal";
			}

			public int GetRequiredFrameRate()
			{
				return requiredFramerate;
			}
		}
		public class Colored : ILightsEffect
		{
			
			public void FillColors(Color[] arr, int nLights)
			{
				int enabledLights = (int)(mmdevice.AudioMeterInformation.MasterPeakValue * nLights);
				for (int i = 0; i < nLights; i++)
					arr[i] = Color.FromArgb(64, 64, 64);
				for (int i = 0; i < enabledLights; i++)
					arr[i] = HsvToRgb(255 - map(i, 0, nLights, 0, 255), 1.0, 1.0);
			}
			static long map(long x, long in_min, long in_max, long out_min, long out_max)
			{
				return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
			}
			public string GetDescription()
			{
				return "Enables lights from left to right by loudness";
			}

			public string GetName()
			{
				return "Colored";
			}

			public int GetRequiredFrameRate()
			{
				return requiredFramerate;
			}
		}
		public class SyncronousColored : ILightsEffect
		{
			
			public void FillColors(Color[] arr, int nLights)
			{
				for (int i = 0; i < nLights; i++)
					arr[i] = HsvToRgb(255.0 - (mmdevice.AudioMeterInformation.MasterPeakValue * 255), 1.0, 1.0);
			}
			public string GetDescription()
			{
				return "Changes all lights color by loudness";
			}

			public string GetName()
			{
				return "Syncronous Colored";
			}

			public int GetRequiredFrameRate()
			{
				return requiredFramerate;
			}
		}
		public class _2Channels : ILightsEffect
		{

			public void FillColors(Color[] arr, int nLights)
			{
				int enabledLights = (int)(mmdevice.AudioMeterInformation.MasterPeakValue * nLights);
				for (int i = 0; i < nLights; i++)
					arr[i] = Color.FromArgb(64, 64, 64);

				for (int i = nLights / 2 + 2; i > (nLights - enabledLights) / 2; i--)
					arr[i] = HsvToRgb(255 - map(i, 0, nLights, 0, 255), 1.0, 1.0);

				for (int i = 0; i < (enabledLights / 2) - 1; i++)
					arr[nLights / 2 + i] = HsvToRgb(255 - map(i, 0, nLights / 2 + 1, 0, 255), 1.0, 1.0);
			}
			static long map(long x, long in_min, long in_max, long out_min, long out_max)
			{
				return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
			}
			public string GetDescription()
			{
				return "Enables lights from center to edges by loudness";
			}

			public string GetName()
			{
				return "2 Channels";
			}

			public int GetRequiredFrameRate()
			{
				return requiredFramerate;
			}
		}
	}
}
