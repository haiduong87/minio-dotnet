﻿/*
 * Minimal Object Storage Library, (C) 2015 Minio, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minio.Client;
using Minio.Client.xml;

namespace Minio.ClientTests
{
    /// <summary>
    /// Summary description for IntegrationTest
    /// </summary>
    [TestClass]
    public class IntegrationTest
    {
        public IntegrationTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        private static readonly string bucket = "goroutine-dotnet";
        private static ObjectStorageClient client = ObjectStorageClient.GetClient("https://s3-us-west-2.amazonaws.com", "", "");
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [ExpectedException(typeof(RequestException))]
        public void MakeBucket()
        {
            client.MakeBucket(bucket);
        }

        [TestMethod]
        public void ListBuckets()
        {
            var buckets = client.ListBuckets();
            foreach (Bucket bucket in buckets.Buckets)
            {
                Console.Out.WriteLine(bucket.Name + " " + bucket.CreationDate);
            }
        }

        [TestMethod]
        public void BucketExists()
        {
            bool exists = client.BucketExists(bucket);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void BucketExistsNonExistantBucket()
        {
            bool exists = client.BucketExists(bucket + "-missing");
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void GetAndSetBucketAcl()
        {
            client.SetBucketAcl(bucket, Acl.PublicRead);
            Acl acl = client.GetBucketAcl(bucket);
            Assert.AreEqual(Acl.PublicRead, acl);

            client.SetBucketAcl(bucket, Acl.PublicReadWrite);
            acl = client.GetBucketAcl(bucket);
            Assert.AreEqual(Acl.PublicReadWrite, acl);

            client.SetBucketAcl(bucket, Acl.AuthenticatedRead);
            acl = client.GetBucketAcl(bucket);
            Assert.AreEqual(Acl.AuthenticatedRead, acl);

            client.SetBucketAcl(bucket, Acl.Private);
            acl = client.GetBucketAcl(bucket);
            Assert.AreEqual(Acl.Private, acl);
        }
    }
}
