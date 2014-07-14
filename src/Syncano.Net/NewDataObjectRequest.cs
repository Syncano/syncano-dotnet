﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class NewDataObjectRequest
    {
        public NewDataObjectRequest()
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

        public int? DataOne { get; set; }

        public int? DataTwo { get; set; }

        public int? DataThree { get; set; }

        public string Folder { get; set; }

        public DataObjectState State { get; set; }

        public string ParentId { get; set; }

    }
}