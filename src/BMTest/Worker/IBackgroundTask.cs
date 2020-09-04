using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomService.Worker
{
	public interface IBackgroundTask
	{
		public Task DoWork(CancellationToken token);
	}
}
