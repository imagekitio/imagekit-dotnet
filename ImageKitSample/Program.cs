﻿using Imagekit.Models;
using Imagekit.Sdk;
using System;
using System.Collections.Generic;
using Imagekit;
using static Imagekit.Models.CustomMetaDataFieldSchemaObject;
using Newtonsoft.Json.Linq;

namespace ImagekitSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Create Instance of ImageKit
            ImagekitClient imagekit = new ImagekitClient("TestPublicKey", "TestPrivateKey", "https://api.imagekit.io/");


            #region URL Generation

            /// Generating URLs
            string imageURL = imagekit.Url(new Transformation().Width(400).Height(300))
                .Path("/default-image.jpg")
                .UrlEndpoint("https://ik.imagekit.io/your_imagekit_id/endpoint")
                .TransformationPosition("query")
                .Generate();
            Console.WriteLine("Url for first image transformed with height: 300, width: 400 - {0}", imageURL);


            ///// Generating Signed URL
            //var imgURL1 = "https://ik.imagekit.io/demo/default-image.jpg";
            //string[] queryParams = { "b=123", "a=test" };
            //try
            //{
            //    var signedUrl = imagekit.Url(new Transformation().Width(400).Height(300))
            //    .Src(imgURL1)
            //    .QueryParameters(queryParams)
            //    .ExpireSeconds(600)
            //    .Signed()
            //    .Generate();
            //    Console.WriteLine("Signed Url for first image transformed with height: 300, width: 400: - {0}", signedUrl);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            #endregion

            #region  Purge

            imagekit.PurgeCache("https://ik.imagekit.io/dnggmzz0v/default-image.jpg");

            imagekit.PurgeStatus("62e5778f31305bff3223b791");

            #endregion

            #region  Upload BY URI| Bytes | Base64


            // Upload By URI
            FileCreateRequest request = new FileCreateRequest
            {
                Url = new Uri(@"C:\test.jpg"),
                FileName = "test.jpg"
            };
            ResponseMetaData resp1 = imagekit.Upload(request);


            //Upload by bytes

            string base64 = "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==";

            byte[] bytes = Convert.FromBase64String(base64);
            FileCreateRequest ob = new FileCreateRequest
            {
                Bytes = bytes,
                FileName = Guid.NewGuid().ToString()
            };

            List<string> tags = new List<string>
            {
                "Software",
                "Developer",
                "Engineer"
            };
            ob.Tags = tags;
            ob.Folder = "demo1";
            string customCoordinates = "10,10,20,20";
            ob.CustomCoordinates = customCoordinates;
            List<string> responseFields = new List<string>
            {
                "thumbnail",
                "tags",
                "customCoordinates"
            };

            ob.ResponseFields = responseFields;
            JObject optionsInnerObject = new JObject
            {
                { "add_shadow", true }
            };
            JObject innerObject1 = new JObject
            {
                { "name", "remove-bg" },
                { "options", optionsInnerObject }
            };
            JObject innerObject2 = new JObject
            {
                { "name", "google-auto-tagging" },
                { "minConfidence", 10 },
                { "maxTags", 5 }
            };
            JArray jsonArray = new JArray
            {
                innerObject1,
                innerObject2
            };

            ob.Extensions = jsonArray;
            ob.WebhookUrl = "https://webhook.site/c78d617f-33bc-40d9-9e61-608999721e2e";
            ob.UseUniqueFileName = false;
            ob.IsPrivateFileValue = false;
            ob.OverwriteFile = false;
            ob.OverwriteAiTags = false;
            ob.OverwriteTags = false;
            ob.OverwriteCustomMetadata = true;
            JObject jsonObjectCustomMetadata = new JObject
            {
                { "test1", 10 }
            };
            ob.CustomMetadata = jsonObjectCustomMetadata;

            ResponseMetaData resp2 = imagekit.Upload(ob);

            //Upload by Base64
            FileCreateRequest ob2 = new FileCreateRequest
            {
                Base64 = base64,
                FileName = Guid.NewGuid().ToString()
            };
            ResponseMetaData resp = imagekit.Upload(ob2);




            #endregion

            #region  Metadata
            imagekit.GetFileMetadata("fileId");


            imagekit.GetRemoteFileMetadata("https://ik.imagekit.io/demo/medium_cafe_B1iTdD0C.jpg");



            // CustomMetaDataFields
            imagekit.GetCustomMetaDataFields(true);

            //CreateCustomMetaDataFields

            CustomMetaDataFieldCreateRequest requestModel = new CustomMetaDataFieldCreateRequest
            {
                Name = "Tst3",
                Label = "Test3"
            };
            CustomMetaDataFieldSchemaObject schema = new CustomMetaDataFieldSchemaObject
            {
                Type = CustomMetaDataTypeEnum.Number,
                MinValue = 1000,
                MaxValue = 3000,
                MinLength = 500,
                MaxLength = 600,
                IsValueRequired = false
            };

            requestModel.Schema = schema;
            imagekit.CreateCustomMetaDataFields(requestModel);



            // UpdateCustomMetaDataFields

            CustomMetaDataFieldUpdateRequest requestUpdateModel = new CustomMetaDataFieldUpdateRequest
            {
                Id = "Tst3"
            };
            CustomMetaDataFieldSchemaObject updateschema = new CustomMetaDataFieldSchemaObject
            {
                Type = CustomMetaDataTypeEnum.Number,
                MinValue = 1000,
                MaxValue = 3000,
                MinLength = 500,
                MaxLength = 600,
                IsValueRequired = false
            };

            requestUpdateModel.Schema = schema;
            imagekit.UpdateCustomMetaDataFields(requestUpdateModel);


            #endregion

            #region FileVersionRequest
            DeleteFileVersionRequest delRequest = new DeleteFileVersionRequest
            {
                FileId = "fileId",
                VersionId = "versionId"
            };
            imagekit.DeleteFileVersion(delRequest);



            // RestoreFileVersion

            imagekit.RestoreFileVersion("abc", "1");


            // GetFileVersions

            imagekit.GetFileVersions("fileId");



            // GetFileVersionDetails

            imagekit.GetFileVersionDetails("fileId", "versionId");

            #endregion

            #region Manage File
            //List and search files
            GetFileListRequest model = new GetFileListRequest
            {
                Type = "file",
                Limit = 10,
                Skip = 0
            };
            var res = imagekit.GetFileListRequest(model);

            imagekit.GetFileDetail("fileId");

            imagekit.DeleteFile("fileId");


            //Bulk Delete
            List<string> ob3 = new List<string>();
            ob3.Add("fileId-1");
            ob3.Add("fileId-2");
            imagekit.BulkDeleteFiles(ob3);

            //Copy File
            CopyFileRequest cpyRequest = new CopyFileRequest
            {
                SourceFilePath = "Tst3",
                DestinationPath = "Tst3",
                IncludeFileVersions = true
            };
            imagekit.CopyFile(cpyRequest);



            //MoveFile

            MoveFileRequest moveFile = new MoveFileRequest
            {
                SourceFilePath = "Tst3",
                DestinationPath = "Tst3"
            };
            imagekit.MoveFile(moveFile);


            //RenameFile

            RenameFileRequest renameFileRequest = new RenameFileRequest
            {
                FilePath = "Tst3",
                NewFileName = "Tst4",
                PurgeCache = false
            };
            imagekit.RenameFile(renameFileRequest);


            #endregion

            #region ManageFolder

            CreateFolderRequest createFolderRequest = new CreateFolderRequest
            {
                FolderName = "abc",
                ParentFolderPath = "source/folder/path"
            };
            imagekit.CreateFolder(createFolderRequest);



            //DeleteFolderRequest

            DeleteFolderRequest deleteFolderRequest = new DeleteFolderRequest
            {
                FolderPath = "source/folder/path/new_folder"
            };
            imagekit.DeleteFolder(deleteFolderRequest);



            // CopyFolder
            CopyFolderRequest cpyFolderRequest = new CopyFolderRequest
            {
                SourceFolderPath = "Tst3",
                DestinationPath = "Tst3",
                IncludeFileVersions = true
            };

            imagekit.CopyFolder(cpyFolderRequest);

            // MoveFolder
            MoveFolderRequest moveFolderRequest = new MoveFolderRequest
            {
                SourceFolderPath = "Tst3",
                DestinationPath = "Tst3"

            };

            imagekit.MoveFolder(moveFolderRequest);

            #endregion

            #region GetBulkJobStatus

            imagekit.GetBulkJobStatus("jobId");

            #endregion

            #region  Tags
            TagsRequest tagsRequest = new TagsRequest
            {
                Tags = new List<string>
    {
        "tag1",
        "tag2"
    },
                FileIds = new List<string>
    {
        "field1"
    }
            };
            imagekit.AddTags(tagsRequest);


            TagsRequest removeTagsRequest = new TagsRequest
            {
                Tags = new List<string>
    {
        "tag1",
        "tag2"
    },
                FileIds = new List<string>
    {
        "field1"
    }
            };
            imagekit.RemoveTags(removeTagsRequest);


            AiTagsRequest removeAITagsRequest = new AiTagsRequest
            {
                AiTags = new List<string>
    {
        "tag1",
        "tag2"
    },
                FileIds = new List<string>
    {
        "field1"
    }
            };
            imagekit.RemoveAiTags(removeAITagsRequest);

            #endregion

        }

    }
}
