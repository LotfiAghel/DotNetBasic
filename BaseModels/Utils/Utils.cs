using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SGSStandalone.Core
{
    public static class TimeUtils
    {
        public static DateTime milliSecToUtc(this long ts)
        {
            return (new System.DateTime(1970, 1, 1)).AddMilliseconds(ts);
            //return (long)(dt - new System.DateTime(1970, 1, 1)).TotalMilliseconds;
        }
        public static long totalMilliSeconds(this System.DateTime dt)
        {
            return (long)(dt - new System.DateTime(1970, 1, 1)).TotalMilliseconds;
        }
        public static long totalStep(this System.DateTime dt)
        {
            return (long)(dt - new System.DateTime(1970, 1, 1)).TotalMilliseconds/100;
        }
        public static long totalSeconds(this System.DateTime dt)
        {
            return (long)(dt - new System.DateTime(1970, 1, 1)).TotalSeconds;
        }
        public static long totalDays(this System.DateTime dt)
        {
            return (long)(dt - new System.DateTime(1970, 1, 1)).TotalDays;
        }
        public static DateTime FirstDate=new System.DateTime(1970, 1, 1);
        public static long MothIndex0(this System.DateTime dt)
        {
            return (long)(dt.Year * 12 + (dt.Month - 1)) ;
        }
        public static long MothIndex(this System.DateTime dt)
        {
            return (long)(dt.MothIndex0() - FirstDate.MothIndex0());
        }
        public static DateTime IndexToMonth(this long dt)
        {
            return ((int)dt).IndexToMonth();
        }
        public static DateTime IndexToMonth(this int dt)
        {
            return new System.DateTime(1970 + dt / 12, dt % 12 + 1, 1);
        }
        public static long totalWeek(this System.DateTime dt)
        {
            return (long)(dt - new System.DateTime(1970, 1, 1)).TotalDays / 7;
        }
        public static long total30DayMonth(this System.DateTime dt)
        {
            return (long)(dt - new System.DateTime(1970, 1, 1)).TotalDays / 30;
        }
        public static DateTime endOfCurrentWeek(this System.DateTime dt)
        {
            long t = (dt.totalWeek() + 1) * 7;
            var st = new System.DateTime(1970, 1, 1);
            return st.AddDays(t);
        }
        public static DateTime endOfCurrentMonth(this System.DateTime dt)
        {
            long t = (dt.total30DayMonth() + 1) * 30;
            var st = new System.DateTime(1970, 1, 1);
            return st.AddDays(t);
        }
        public static System.DateTime endOfDay(this System.DateTime dt)
        {
            long t = (dt.totalDays() + 1);
            var st = new System.DateTime(1970, 1, 1);
            return st.AddDays(t);
        }
        public static System.DateTime endOfLastComplteDay(this System.DateTime dt)
        {
            long t = dt.totalDays()-1 ;
            var st = new System.DateTime(1970, 1, 1);
            return st.AddDays(t).AddHours(23).AddMinutes(59).AddSeconds(59).AddMicroseconds(999);
        }
        public static System.DateTime endOfShamsiDay(this System.DateTime dt)
        {
            long t = (dt.AddHours(3.5).totalDays() + 1);
            var st = new System.DateTime(1970, 1, 1);
            return st.AddDays(t).AddHours(-3.5);
        }
        public static System.DateTime startOfDay(this System.DateTime dt)
        {
            long t = (dt.totalDays());
            var st = new System.DateTime(1970, 1, 1);
            return st.AddDays(t);
        }

        public static System.DateTime endOfDay(long day)
        {
            var st = new System.DateTime(1970, 1, 1);
            return st.AddDays(day);
        }

        internal static Task WaitUntil(Func<bool> p, int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }

    public static class Utils
	{
		public static async Task SetTimeout(int ms, Action action, CancellationToken cancellationToken)
		{
			var t = Task.Delay(ms, cancellationToken);
			await t;
			if (!t.IsCanceled)
				action();
		}

		public static async Task WaitUntil(Func<bool> func, int minDelay = 1, int maxDelay = 1000, int step = 1)
		{
			int delay = minDelay;
			while (!func())
			{
				await Task.Delay(delay);
				if (delay < maxDelay)
					delay += step;
			}
		}

        public static async Task<bool> WaitUntilAsync(Func<Task<bool>> func, int delay, int timeout)
        {
            var expiration = DateTime.UtcNow.AddMilliseconds(timeout);
            while (!await func())
            {
                await Task.Delay(delay);
                if (DateTime.UtcNow > expiration)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Waits the until the condition is true
        /// </summary>
        /// <returns>IsSuccessful (i.e. not timed out)</returns>
        /// <param name="delay">Delay in ms</param>
        /// <param name="timeout">Timeout in ms</param>
        public static async Task<bool> WaitUntil(Func<bool> func, int delay, int timeout)
		{
			var expiration = DateTime.UtcNow.AddMilliseconds(timeout);
			while (!func())
			{
				await Task.Delay(delay);
				if (DateTime.UtcNow > expiration)
					return false;
			}
			return true;
		}

		/// <param name="delayFactor">Time in ms that is added to the delay after each failure</param>
		/// <param name="func">(tryIndex, lastTry, cancel)=>predicate</param>
		public static async Task<bool> TryMultipleTimes(Func<int, bool, CancellationTokenSource, Task<bool>> func, int times = 10, int delayFactor = 1000)
		{
			CancellationTokenSource cancellation = new CancellationTokenSource();
			for (int i = 0; i < times; i++)
			{
				try
				{
					var res = await func(i, i == times - 1, cancellation);
					if (res)
						return true;
					if (cancellation.IsCancellationRequested)
						return false;
				}
				catch (Exception ex)
				{
					Logging.Warn("Utils.TryMultipleTimes", ex + "");
				}
				await Task.Delay(delayFactor * i);
			}
			return false;
		}

		public static async Task<string> Bash(string cmd)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				var escapedArgs = cmd.Replace("\"", "\\\"");

				var process = new Process()
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = "/bin/bash",
						Arguments = $"-c \"{escapedArgs}\"",
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true,
					}
				};
				process.Start();
				string result = await process.StandardOutput.ReadToEndAsync();
				await Task.Run(() => process.WaitForExit());
				return result;
			}
			return null;
		}

        public static void BashShell(string cmd)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var escapedArgs = cmd.Replace("\"", "\\\"");

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
            }
        }

        public static long ToEpochSeconds(this DateTime dt)
		{
			return (long)(dt - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		public static DateTime ParseEpochSeconds(long ts)
		{
			return new DateTime(1970, 1, 1).AddSeconds(ts);
		}

		public static void Populate<T>(this JToken value, T target) where T : class
        {
			Populate(value, target, JsonSerializer.CreateDefault());
        }

		public static void Populate<T>(this JToken value, T target, JsonSerializer serializer) where T : class
		{
			serializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
			using (var sr = value.CreateReader())
			{
				serializer.Populate(sr, target);
			}
		}
	}
}
