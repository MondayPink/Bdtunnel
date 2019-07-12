/* BoutDuTunnel Copyright (c) 2007-2016 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

using System;
using System.Collections;
using System.Collections.Generic;
using Bdt.Shared.Logs;

namespace Bdt.Server.Service
{
	public abstract class TimeoutObject
	{
		private int TimeoutDelay { get; set; }
		public DateTime LastAccess { get; set; }

		protected TimeoutObject(int timeoutdelay)
		{
			TimeoutDelay = timeoutdelay;
		}

		protected abstract void Timeout(ILogger logger);

		protected virtual bool CheckTimeout(ILogger logger)
		{
			if (TimeoutDelay > 0)
				return DateTime.Now.Subtract(LastAccess).TotalHours > TimeoutDelay;
			return false;
		}

		public static void CheckTimeout<T>(ILogger logger, Dictionary<int, T> collection) where T : TimeoutObject
		{
			foreach (int key in new ArrayList(collection.Keys))
			{
				var item = collection[key];
				if (!item.CheckTimeout(logger))
					continue;

				item.Timeout(logger);
				collection.Remove(key);
			}
		}
	}
}
