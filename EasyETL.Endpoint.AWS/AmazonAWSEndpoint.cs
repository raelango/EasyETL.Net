using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyETL.Endpoint
{
    [DisplayName("Amazon S3 Bucket")]
    [EasyProperty("CanListen","False")]
    [EasyProperty("HasFiles", "True")]
    [EasyProperty("CanStream", "True")]
    [EasyProperty("CanRead", "True")]
    [EasyProperty("CanWrite", "True")]
    [EasyProperty("CanList", "True")]

    [EasyField("BucketName", "Name of the Amazon S3 Bucket")]
    [EasyField("AccessKey","Access Key to access the S3 Bucket")]
    [EasyField("SecretKey", "Secret Key to access the S3 Bucket","","","",true)]
    [EasyField("Overwrite", "Set to True if you would like to overwrite existing files when copying to this S3 Bucket","True","","True;False")]
    public class AmazonAWSEndpoint : AbstractFileEasyEndpoint
    {
        string BucketName = String.Empty;
        string AccessKey = String.Empty;
        string SecretKey = String.Empty;

        AmazonS3Client AWSClient = null;

        public AmazonAWSEndpoint()
        {

        }

        public AmazonAWSEndpoint(string accessKey, string secretKey, string bucketName, bool overwrite = false)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            BucketName = bucketName;
            Overwrite = overwrite;
        }

        #region public overriden methods

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "bucketname":
                    BucketName = fieldValue; break;
                case "accesskey":
                    AccessKey = fieldValue; break;
                case "secretkey":
                    SecretKey = fieldValue; break;
            }
        }

        public override bool IsFieldSettingsComplete()
        {
            return base.IsFieldSettingsComplete() && !String.IsNullOrWhiteSpace(BucketName) && !String.IsNullOrWhiteSpace(AccessKey) && !String.IsNullOrWhiteSpace(SecretKey);
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            GetS3Client();
            return (AWSClient !=null);
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string,string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("BucketName", BucketName);
            resultDict.Add("AccessKey", AccessKey);
            resultDict.Add("SecretKey", SecretKey);
            return resultDict;
        }


        public override string[] GetList(string filter = "*.*")
        {
            GetS3Client();
            List<string> fileList = new List<string>();
            if (AWSClient != null) {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = BucketName
                };
                while (request != null) {
                    ListObjectsResponse loResponse = AWSClient.ListObjects(request);
                    fileList.AddRange(loResponse.S3Objects.Select(s => s.Key).ToList());
                    if (loResponse.IsTruncated)
                        request.Marker = loResponse.NextMarker;
                    else
                        request = null;
                }
            }
            return fileList.ToArray();
        }

        public override Stream GetStream(string fileName)
        {
            if (!FileExists(fileName)) return null;
            if (AWSClient != null)
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = fileName
                };
                using (GetObjectResponse response = AWSClient.GetObject(request))
                {
                    MemoryStream ms = new MemoryStream();
                    response.ResponseStream.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return ms;
                }
            }
            return null;
        }

        public override byte[] Read(string fileName)
        {
            Stream fileStream = GetStream(fileName);
            if (fileStream != null)
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return Encoding.ASCII.GetBytes(reader.ReadToEnd());
                }
            }
            return null;
        }

        public override bool Write(string fileName, byte[] data)
        {
            try
            {
                if ((!FileExists(fileName)) || Overwrite)
                {
                    using (TransferUtility transferUtility = new TransferUtility(AWSClient))
                    {
                        using (MemoryStream ms = new MemoryStream(data)) {
                            transferUtility.Upload(ms, BucketName, AccessKey);
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override bool FileExists(string fileName)
        {
            try
            {
                GetS3Client();
                AWSClient.GetObjectMetadata(BucketName, fileName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override bool Delete(string fileName)
        {
            try
            {
                if (FileExists(fileName))
                {
                    DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = BucketName,
                        Key = AccessKey
                    };
                    AWSClient.DeleteObject(deleteRequest);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        public AmazonS3Client GetS3Client()
        {
            if ((AWSClient == null) && IsFieldSettingsComplete())
            {
                AWSClient = new AmazonS3Client(AccessKey, SecretKey,Amazon.RegionEndpoint.USEast1);
            }
            return AWSClient;
        }
    }
}
