using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lyralei
{
    //public sealed class SingleThreadSynchronizationContext : SynchronizationContext
    //{
    //    private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> m_queue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
    //    private readonly Thread m_thread = Thread.CurrentThread;

    //    public override void Post(SendOrPostCallback d, object state)
    //    {
    //        if (d == null) throw new ArgumentNullException("d");
    //        m_queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
    //    }

    //    public override void Send(SendOrPostCallback d, object state)
    //    {
    //        throw new NotSupportedException("Synchronously sending is not supported.");
    //    }

    //    public void RunOnCurrentThread()
    //    {
    //        foreach (var workItem in m_queue.GetConsumingEnumerable())
    //            workItem.Key(workItem.Value);
    //    }

    //    public void Complete() { m_queue.CompleteAdding(); }
    //}
}
