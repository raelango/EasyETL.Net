using Amazon.S3;
using Amazon.S3.Model;
using EasyETL.Endpoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Transfer;

namespace EasyETL.Endpoint
{
    public class AmazonAWSEndpoint : AbstractFileEasyEndpoint
    {
        string BucketName = String.Empty;
        string AccessKey = String.Empty;
        string SecretKey = String.Empty;

        public override bool CanListen
        {
            get
            {
                return false;
            }
        }

        AmazonS3Client AWSClient = null;

        public AmazonAWSEndpoint(string accessKey, string secretKey, string bucketName, bool overwrite = false)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            BucketName = bucketName;
            Overwrite = overwrite;
        }

        #region public overriden methods
        public override string[] GetList(string filter = "*.*")
        {
            GetS3Client();
            List<string> fileList = new List<string>();
            if (AWSClient != null) {
                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = BucketName;
                while (request != null) {
                    ListObjectsResponse loResponse = AWSClient.ListObjects(request);
                    fileList.AddRange(loResponse.S3Objects.Select(s => s.BucketName).ToList());
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
                GetObjectRequest request = new GetObjectRequest();
                request.BucketName = BucketName;
                using (GetObjectResponse response = AWSClient.GetObject(request))
                {
                    return response.ResponseStream;
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
            return false;
        }

        public override bool Delete(string fileName)
        {
            try
            {
                if (FileExists(fileName))
                {
                    DeleteObjectRequest deleteRequest = new DeleteObjectRequest();
                    deleteRequest.BucketName = BucketName;
                    deleteRequest.Key = AccessKey;
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
            if (AWSClient == null)
            {
                AWSClient = new AmazonS3Client(AccessKey, SecretKey);
            }
            return AWSClient;
        }
    }
}
