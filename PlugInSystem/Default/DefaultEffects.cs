using System;
using System.Diagnostics;
using System.Drawing;

namespace XMasLights.PlugInSystem.Default
{
	class DefaultEffects : IEffectCollection
	{
		public ILightsEffect[] GetEffects()
		{
			return new ILightsEffect[] { new PairEffect(), new RunningEffect(), new RainbowEffect() };
		}

        public string GetPlugInName() => "Default Effects";
		public string GetPlugInDescription() => "Builtin Effects collection by CatYoutuber";
		public bool HasWebsite() => false;
		public string GetPlugInWebsite() => "";
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
		public static Color GetRgb(double r, double g, double b)
		{
			return Color.FromArgb(255, (byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
		}
	}
	public class PairEffect : ILightsEffect
	{
		Color[] DefaultColors = { Color.Red, Color.Yellow, Color.Lime, Color.Blue };
		Color disabled = Color.FromArgb(64, 64, 64);
		public void FillColors(Color[] arr, int nLights)
		{
			for (int i = 0; i < nLights; i++)
			{
				/*arr[i] = DefaultColors[i % 4];
				if ((Environment.TickCount / 750) % 2 == 0)
					arr[i] = i % 2 == 0 ? disabled : arr[i];
				else
					arr[i] = i % 2 == 0 ? arr[i] : disabled;*/

				double brightness = (Math.Sin((Environment.TickCount % 2000 / 2000d) * Math.PI * 2) + 1) / 2;
				double value = i % 2 == 0 ? brightness : 1 - brightness;
				Color color = DefaultColors[i % 4];

				arr[i] = Color.FromArgb(255, (byte)(color.R * value), (byte)(color.G * value), (byte)(color.B * value));
			}
		}

        public string GetDescription() => "Colors smoothly enables and disables by pairs";

        public string GetName() => "Pairs";

        public int GetRequiredFrameRate() => 10;
    }
	public class RunningEffect : ILightsEffect
	{
		public void FillColors(Color[] arr, int nLights)
		{
			Color[] tmp = new Color[4];
			for (int i = 0; i < tmp.Length; i++)
				tmp[i] = Color.FromArgb(64, 64, 64);
			tmp[(Environment.TickCount / 250) % 4] = Color.White;
			for (int i = 0; i < nLights; i++)
				arr[i] = tmp[i % 4];
		}

        public string GetDescription() => "Running light effect";

        public string GetName() => "Running";

        public int GetRequiredFrameRate() => 10;
    }
	public class RainbowEffect : ILightsEffect
	{
		void MHsvToRgb(double h, double S, double V, out int r, out int g, out int b)
		{
			double H = h;
			while (H < 0) { H += 360; };
			while (H >= 360) { H -= 360; };
			double R, G, B;
			if (V <= 0)
			{ R = G = B = 0; }
			else if (S <= 0)
			{
				R = G = B = V;
			}
			else
			{
				double hf = H / 60.0;
				int i = (int)Math.Floor(hf);
				double f = hf - i;
				double pv = V * (1 - S);
				double qv = V * (1 - S * f);
				double tv = V * (1 - S * (1 - f));
				switch (i)
				{

					// Red is the dominant color

					case 0:
						R = V;
						G = tv;
						B = pv;
						break;

					// Green is the dominant color

					case 1:
						R = qv;
						G = V;
						B = pv;
						break;
					case 2:
						R = pv;
						G = V;
						B = tv;
						break;

					// Blue is the dominant color

					case 3:
						R = pv;
						G = qv;
						B = V;
						break;
					case 4:
						R = tv;
						G = pv;
						B = V;
						break;

					// Red is the dominant color

					case 5:
						R = V;
						G = pv;
						B = qv;
						break;

					// Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

					case 6:
						R = V;
						G = tv;
						B = pv;
						break;
					case -1:
						R = V;
						G = pv;
						B = qv;
						break;

					// The color is not defined, we should throw an error.

					default:
						//LFATAL("i Value error in Pixel conversion, Value is %d", i);
						R = G = B = V; // Just pretend its black/white
						break;
				}
			}
			r = Clamp((int)(R * 255.0));
			g = Clamp((int)(G * 255.0));
			b = Clamp((int)(B * 255.0));
		}

		/// <summary>
		/// Clamp a value to 0-255
		/// </summary>
		int Clamp(int i)
		{
			if (i < 0) return 0;
			if (i > 255) return 255;
			return i;
		}

		Color CMHsvToRgb(double H, double S, double V)
		{
			int r, g, b;
			MHsvToRgb(H, S, V, out r, out g, out b);
			return Color.FromArgb(r, g, b);
		}
		static long map(long x, long in_min, long in_max, long out_min, long out_max)
		{
			return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}
		static byte offset = 1;
		public void FillColors(Color[] arr, int nLights)
		{
			Color[] tmp = new Color[256];
			for (int c = 0; c < 255; c++)
				tmp[c] = CMHsvToRgb(map((byte)(c - offset),0,255,0,360), 1d, 1d);

			for (int l = 0; l < nLights; l++)
				arr[l] = tmp[l * (255 / nLights)];
			offset++;
		}

        public string GetDescription() => "Rainbow effect desc1~";

        public string GetName() => "Rainbow";

        public int GetRequiredFrameRate() => 20;
    }
}
