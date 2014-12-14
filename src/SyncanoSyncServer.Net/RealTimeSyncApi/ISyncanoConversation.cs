using System;

namespace SyncanoSyncServer.Net.RealTimeSyncApi
{
    public interface ISyncanoConversation
    {
        DateTime Created { get; }
        DateTime? Finished { get; }
        DateTime? Sent { get; }
        bool HasCompleted { get; }
        bool WasTimeouted { get; }

        ApiCommandRequest Request { get; }
        TimeSpan Duration { get; }
        void SetResponse(string message);
        void SetError(Exception exception);
        void VerifyTimeout();
        void SetSent();
    }
}