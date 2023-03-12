using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Security.Cryptography;
using System.Text;

namespace AzureBlobStorage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string GetMD5HashFromStream(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] blockHash = md5.ComputeHash(data);
            return Convert.ToBase64String(blockHash, 0, 16);
        }
        private void Upload(CloudBlobContainer cloudBlobContainer, string fileName)
        {
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference("Ebook");

            int blockSize = 1024 * 1024; //1 MB

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // reading the fule length
                long fileSize = fileStream.Length;

                //block count is the number of blocks + 1 for the last one
                int blockCount = (int)((float)fileSize / (float)blockSize) + 1;

                //List of block ids; the blocks will be committed in the order of this list 
                List<string> blockIDs = new List<string>();

                //starting block number - 1
                int blockNumber = 0;

                try
                {
                    int bytesRead = 0; //number of bytes read so far
                    long bytesLeft = fileSize; //number of bytes left to read and upload

                    //do until all of the bytes are uploaded
                    while (bytesLeft > 0)
                    {
                        blockNumber++;
                        int bytesToRead;
                        if (bytesLeft >= blockSize)
                        {
                            //more than one block left, so put up another whole block
                            bytesToRead = blockSize;
                        }
                        else
                        {
                            //less than one block left, read the rest of it
                            bytesToRead = (int)bytesLeft;
                        }

                        //create a blockID from the block number, add it to the block ID list
                        //the block ID is a base64 string
                        string blockId =Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("BlockId{0}",
                        blockNumber.ToString("0000000"))));
                        blockIDs.Add(blockId);
                        //set up new buffer with the right size, and read that many bytes into it 
                        byte[] bytes = new byte[bytesToRead];
                        fileStream.Read(bytes, 0, bytesToRead);

                        //calculate the MD5 hash of the byte array
                        string blockHash = GetMD5HashFromStream(bytes);

                        //upload the block, provide the hash so Azure can verify it
                        blob.PutBlockAsync(blockId, new MemoryStream(bytes), blockHash);

                        //increment/decrement counters
                        bytesRead += bytesToRead;
                        bytesLeft -= bytesToRead;
                    }

                    //commit the blocks
                    blob.PutBlockListAsync(blockIDs);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Exception thrown = {0}", ex);
                }
            }
        }
        private async void btnBlockBlob_Click(object sender, EventArgs e)
        {
            // Step 1 :- connect to the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=shivblob;AccountKey=nuGq71akmiGgxAwUpzuq4L+WocsioPcHPt1kGbVWekFf/9h+8ODxGAqnY387f7glNoftXd8oMgEHS6Z23wXUSg==;EndpointSuffix=core.windows.net");
            // Step 2 :- get reference o the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions = new BlobRequestOptions()
            {

                SingleBlobUploadThresholdInBytes = 1024 * 1024, //1MB, the minimum the minimum we can change it to max 4mb each blob block
                ParallelOperationThreadCount = 1
            };
            // Step 3 :- from the blob client you will get refernce to the container
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");
            // Step 4 "- use the container and get access to block blob
            //Upload(container, @"C:\Users\SHIV\Desktop\Ebook.pdf");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("Ebook");
            //blockBlob.DownloadToFile(@"c:\my.pdf", FileMode.CreateNew);

            //check all the blocks in a block, we can do that using a for loop
            //foreach (var blockListItem in blockBlob.DownloadBlockList())
            //{
            //    Console.WriteLine("Block ID: " + blockListItem.Name);
            //    Console.WriteLine("Block size: " + blockListItem.Length);

            //}
            // blockBlob.StreamWriteSizeInBytes = 1024 * 1024;
            // Step 5 :- Uploading a PDF 13 mb
            // blockBlob.UploadFromFile(@"C:\Users\SHIV\Desktop\Ebook.pdf");
            //the below code is for downloading from the blob
            blockBlob.StreamMinimumReadSizeInBytes = 1024 * 1024;
            using (var blobStream = await blockBlob.OpenReadAsync())
            {
                using (var fs = new FileStream(@"c:\mynew.pdf", FileMode.Create))
                {
                    await blobStream.CopyToAsync(fs);
                }
            }
        }

        private void btnAppendblob_Click(object sender, EventArgs e)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=shivblob;AccountKey=nuGq71akmiGgxAwUpzuq4L+WocsioPcHPt1kGbVWekFf/9h+8ODxGAqnY387f7glNoftXd8oMgEHS6Z23wXUSg==;EndpointSuffix=core.windows.net");
            // Step 2 :- get reference o the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Step 3 :- from the blob client you will get refernce to the container
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");
            // Step 4 :- Append blob
            CloudAppendBlob appendBlob = container.GetAppendBlobReference("myblobapp");
            //appendBlob.DownloadToFile()
            if (!appendBlob.ExistsAsync().Result)
            {
                appendBlob.CreateOrReplaceAsync();
            }
            appendBlob.AppendFromFileAsync(@"d:\Hello1.txt");
            appendBlob.AppendFromFileAsync(@"d:\Hello2.txt");
        }

        private void btnPageBlob_Click(object sender, EventArgs e)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=shivblob;AccountKey=nuGq71akmiGgxAwUpzuq4L+WocsioPcHPt1kGbVWekFf/9h+8ODxGAqnY387f7glNoftXd8oMgEHS6Z23wXUSg==;EndpointSuffix=core.windows.net");
            // Step 2 :- get reference o the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Step 3 :- from the blob client you will get refernce to the container
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            CloudPageBlob pb = container.GetPageBlobReference("pageblob123");
            if (!pb.ExistsAsync().Result)
            {
                pb.CreateAsync(3 * 512);
            }
            Stream blobStream = pb.OpenReadAsync().Result;
            byte[] data = new byte[512];
            blobStream.Seek(512, SeekOrigin.Begin); // start reading fro 512
            blobStream.Read(data, 0, 512);
            File.WriteAllBytes(@"d:\new2.txt", data);
            //IEnumerable<PageRange> ranges = pb.GetPageRanges();

            //foreach (PageRange range in ranges)
            //{
            //    Console.WriteLine(range.StartOffset + " " + range.EndOffset);
            //}
            //byte[] bytearry = new byte[512];
            //MemoryStream m = new MemoryStream();
            //FileStream fs = new FileStream(@"d:\Hello1.txt", FileMode.Open);
            //fs.Read(bytearry, 0, 512);
            //m = new MemoryStream(bytearry);
            //pb.WritePages(m, 0);

            //fs = new FileStream(@"d:\Hello2.txt", FileMode.Open);
            //fs.Read(bytearry, 0, 512);
            //m = new MemoryStream(bytearry);
            //pb.WritePages(m, 512);

        }
    }
}