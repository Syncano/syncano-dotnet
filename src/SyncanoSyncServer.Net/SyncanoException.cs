using System;
using System.Runtime.Serialization;

namespace SyncanoSyncServer.Net
{
    [Serializable]
    public class SyncanoException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SyncanoException()
        {
        }

        public SyncanoException(string message) : base(message)
        {
        }

        public SyncanoException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SyncanoException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}