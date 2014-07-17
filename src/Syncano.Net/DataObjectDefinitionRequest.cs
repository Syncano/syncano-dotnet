using System.Collections.Generic;

namespace Syncano.Net
{
    public class DataObjectDefinitionRequest
    {
        public DataObjectDefinitionRequest()
        {
            Folder = "Default";
            State = DataObjectState.Pending;
        }

        public string ProjectId { get; set; }

        public string CollectionId { get; set; }

        public string CollectionKey { get; set; }

        public string DataKey { get; set; }

        public string UserName { get; set; }

        public string SourceUrl { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public string ImageBase64 { get; set; }

        public string ImageUrl { get; set; }

        public long? DataOne { get; set; }

        public long? DataTwo { get; set; }

        public long? DataThree { get; set; }

        public string Folder { get; set; }

        public DataObjectState State { get; set; }

        public string ParentId { get; set; }

        public Dictionary<string, string> Additional { get; set; } 

    }
}
