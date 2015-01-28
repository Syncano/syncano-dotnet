# Syncano.Net
## Overview
Syncano.Net is a .Net library that provides communication with Syncano ([www.syncano.com](http://www.syncano.com/)) via http or tcp.

##Getting Started

To get started you have to obtain Syncano Instance name and Api Key. This credentials will allow you to connect to Syncano. Basically you have two options to connect:

- over Http using Syncano.Net (Portable Class Library) - [Install-Package Syncano.Net.Http](https://www.nuget.org/packages/Syncano.Net.Http/)
- over Tcp using SyncanoSyncServer.Net - [Install-Package Syncano.Net.Tcp](https://www.nuget.org/packages/Syncano.Net.Tcp/)


##Syncano Rest Api

To connect to Syncano Rest Api you can use class Syncano.



    //Create Syncano class
    var syncano = new SyncanoClient(instanceName, apiKey);
   
    //Create new project
    Project project = await syncano.Projects.New("MyNewProject", "This is my first project in Syncano.");
    
    //Create collection in project
    Collection collection = await syncano.Collections.New(project.Id, "MyNewCollection");
    
    //Create new folder in collection
    Folder folder = await syncano.Folders.New(project.Id, "MyNewFolder", collection.Id);
    
    //Create new Data Object in folder
    DataObjectDefinitionRequest request = new DataObjectDefinitionRequest();
    request.ProjectId = project.Id;
    request.CollectionId = collection.Id;
    request.Folder = folder.Name;
    request.Title = "DataObject Title";
    request.Text = "Sample data object text content.";
    
    DataObject dataObject = await syncano.DataObjects.New(request);
    
    //Delete Data Object
    DataObjectSimpleQueryRequest deleteRequest = new DataObjectSimpleQueryRequest();
    deleteRequest.ProjectId = project.Id;
    deleteRequest.CollectionId = collection.Id;
    deleteRequest.Folder = folder.Name;
    deleteRequest.DataId = dataObject.Id;
    
    await syncano.DataObjects.Delete(deleteRequest);
    
    //Delete folder
    await syncano.Folders.Delete(project.Id, folder.Name, collection.Id);
    
    //Delete collection
    await syncano.Collections.Delete(project.Id, collection.Id);
    
    //Delete project
    await syncano.Projects.Delete(project.Id);
    
##Syncano Sync Server


SyncServer class allows to connect to Syncano Instance over Tcp. SyncServer can perform all operations of Syncano class, for example create and delete projects, collections, folders and data objects. Hovever main adavantage over http is that it supports real time notificaitons. 

    //Create and start SyncServer
    SyncServer syncServer = new SyncServer(instanceName, apiKey);
    await syncServer.Start();
    
    //Subscribe to Default project
    await syncServer.RealTimeSync.SubscribeProject(projectId, Context.Connection);
    
    //Subscribe to recive notifications about new Data Objects
    syncServer.NewDataObservable.Subscribe(ndn =>
    {
    //Access NewDataNotification object
    });
    
    //Unsubscribe
    await syncServer.RealTimeSync.UnsubscribeProject(projectId);
    
