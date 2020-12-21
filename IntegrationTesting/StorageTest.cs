using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.CoSFE.DatabaseUtils;
using IIG.FileWorker;
using System.Text;
using System.Linq;

namespace IntegrationTesting
{
    [TestClass]
    public class StorageTest
    {
        private const string Server = @"DESKTOP-5K2F4JH";
        private const string Database = @"IIG.CoSWE.StorageDB";
        private const bool IsTrusted = false;
        private const string Login = @"sa";
        private const string Password = @"123456";
        private const int ConnectionTime = 75;

        StorageDatabaseUtils storageDatabaseUtils = new StorageDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);

        private const string testStr1 = "To be, or not to be, that is the question:\r\nWhether 'tis nobler in the mind to suffer\r\nThe slings and arrows of outrageous fortune,\r\nOr to take arms against a sea of troubles\r\nAnd by opposing end them.";
        private const string testStr2 = "作品名：小林秀雄小論\r\n作品名読み：こばやしひでおしょうろん\r\n著者名：中原 中也";
        private const string testStr3 = "🐂🌹🚒";

        [TestMethod]
        public void AddFileToDBTest()
        {
            Assert.IsTrue(storageDatabaseUtils.AddFile(
                "William Shakespeare.txt",
                Encoding.ASCII.GetBytes(BaseFileWorker.ReadAll("C:\\FileWorkerTest\\ReadTest\\William Shakespeare.txt"))
                ));
        }

        [TestMethod]
        public void AddASCIIFileToDBTest()
        {
            Assert.IsTrue(storageDatabaseUtils.AddFile(
                "中原 中也.txt",
                Encoding.ASCII.GetBytes(BaseFileWorker.ReadAll("C:\\FileWorkerTest\\ReadTest\\中原 中也.txt"))
        ));
        }

        [TestMethod]
        public void AddNotUsualByteCodingFileToDBTest()
        {
            Assert.IsTrue(storageDatabaseUtils.AddFile(
                "🐂🌹🚒.txt",
                Encoding.ASCII.GetBytes(BaseFileWorker.ReadAll("C:\\FileWorkerTest\\ReadTest\\🐂🌹🚒.txt"))
        ));
        }

        [TestMethod]
        public void AddEmptyValuesToDBTest()
        {
            Assert.IsFalse(storageDatabaseUtils.AddFile("", new byte[0]));
        }

        [TestMethod]
        public void deleteFileFromDBTest()
        {
           Assert.IsTrue(storageDatabaseUtils.DeleteFile(11));

           Assert.IsTrue(storageDatabaseUtils.DeleteFile(1000));
           Assert.IsTrue(storageDatabaseUtils.DeleteFile(0));
           Assert.IsTrue(storageDatabaseUtils.DeleteFile(-1000));
        }

        [TestMethod]
        public void getFileWithNoReferenceFromDBTest()
        {
            string filename_out;
            byte[] content_out;

            Assert.IsFalse(storageDatabaseUtils.GetFile(1, out filename_out, out content_out));
            Assert.IsNull(filename_out);
            Assert.IsNull(content_out);

            Assert.IsFalse(storageDatabaseUtils.GetFile(1000, out filename_out, out content_out));
            Assert.IsNull(filename_out);
            Assert.IsNull(content_out);

            Assert.IsFalse(storageDatabaseUtils.GetFile(0, out filename_out, out content_out));
            Assert.IsNull(filename_out);
            Assert.IsNull(content_out);

            Assert.IsFalse(storageDatabaseUtils.GetFile(-1000, out filename_out, out content_out));
            Assert.IsNull(filename_out);
            Assert.IsNull(content_out);
        }

        [TestMethod]
        public void getFileFromDBTest()
        {
            string filename_out;
            byte[] content_out;

            Assert.IsTrue(storageDatabaseUtils.GetFile(5, out filename_out, out content_out));
            Assert.AreEqual("William Shakespeare.txt", filename_out);
            Assert.AreEqual(testStr1, Encoding.ASCII.GetString(content_out, 0, content_out.Length));
            Assert.AreEqual(
                BaseFileWorker.ReadAll("C:\\FileWorkerTest\\ReadTest\\William Shakespeare.txt"), 
                Encoding.ASCII.GetString(content_out, 0, content_out.Length)
                );
        }

        [TestMethod]
        public void getASCIIFileFromDBTest()
        {
            string filename_out;
            byte[] content_out;

            Assert.IsTrue(storageDatabaseUtils.GetFile(6, out filename_out, out content_out));
            Assert.AreEqual("中原 中也.txt", filename_out);
            Assert.IsTrue(Encoding.ASCII.GetBytes(testStr2).SequenceEqual(content_out));
            Assert.IsTrue(
                Encoding.ASCII.GetBytes(BaseFileWorker.ReadAll("C:\\FileWorkerTest\\ReadTest\\中原 中也.txt")).SequenceEqual(content_out)
                );
        }

        [TestMethod]
        public void getNotUsualByteCodingFileFromDBTest()
        {
            string filename_out;
            byte[] content_out;

            Assert.IsTrue(storageDatabaseUtils.GetFile(7, out filename_out, out content_out));
            Assert.AreEqual("🐂🌹🚒.txt", filename_out);
            Assert.IsTrue(Encoding.ASCII.GetBytes(testStr3).SequenceEqual(content_out));
            Assert.IsTrue(
                Encoding.ASCII.GetBytes(BaseFileWorker.ReadAll("C:\\FileWorkerTest\\ReadTest\\🐂🌹🚒.txt")).SequenceEqual(content_out)
                );
        }

        [TestMethod]
        public void getFilesFromDBTest()
        {
            Assert.IsNotNull(storageDatabaseUtils.GetFiles());
        }

        [TestMethod]
        public void getFilesByNameFromDBTest()
        {
            Assert.IsNotNull(storageDatabaseUtils.GetFiles("William Shakespeare.txt"));
            Assert.IsNotNull(storageDatabaseUtils.GetFiles("中原 中也.txt"));
            Assert.IsNotNull(storageDatabaseUtils.GetFiles("🐂🌹🚒.txt"));
        }
    }
}
