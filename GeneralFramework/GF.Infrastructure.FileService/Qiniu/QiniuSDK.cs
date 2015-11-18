using System;
using Qiniu.RS;
using Qiniu.RPC;
using System.Collections.Generic;
using Qiniu.IO;
using Qiniu.IO.Resumable;

namespace GF.Infrastructure.FileService
{
    public class QiniuSDK
    {
        /// <summary>
        /// 查看单个文件属性信息
        /// </summary>
        /// <param name="bucket">七牛云存储空间名</param>
        /// <param name="key">文件key</param>
        public static void Stat(string bucket, string key)
        {
            RSClient client = new RSClient();
            Entry entry = client.Stat(new EntryPath(bucket, key));
            if (entry.OK)
            {
                Console.WriteLine("Hash: " + entry.Hash);
                Console.WriteLine("Fsize: " + entry.Fsize);
                Console.WriteLine("PutTime: " + entry.PutTime);
                Console.WriteLine("MimeType: " + entry.MimeType);
                Console.WriteLine("Customer: " + entry.Customer);
            }
            else
            {
                Console.WriteLine("Failed to Stat");
            }
        }
        /// <summary>
        /// 复制单个文件
        /// </summary>
        /// <param name="bucketSrc">需要复制的文件所在的空间名</param>
        /// <param name="keySrc">需要复制的文件key</param>
        /// <param name="bucketDest">目标文件所在的空间名</param>
        /// <param name="keyDest">标文件key</param>
        public static void Copy(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            RSClient client = new RSClient();
            CallRet ret = client.Copy(new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest));
            if (ret.OK)
            {
                Console.WriteLine("Copy OK");
            }
            else
            {
                Console.WriteLine("Failed to Copy");
            }
        }
        /// <summary>
        /// 移动单个文件
        /// </summary>
        /// <param name="bucketSrc">需要移动的文件所在的空间名</param>
        /// <param name="keySrc">需要移动的文件</param>
        /// <param name="bucketDest">目标文件所在的空间名</param>
        /// <param name="keyDest">目标文件key</param>
        public static void Move(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            Console.WriteLine("\n===> Move {0}:{1} To {2}:{3}",
            bucketSrc, keySrc, bucketDest, keyDest);
            RSClient client = new RSClient();
            new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest);
            CallRet ret = client.Move(new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest));
            if (ret.OK)
            {
                Console.WriteLine("Move OK");
            }
            else
            {
                Console.WriteLine("Failed to Move");
            }
        }
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="bucket">文件所在的空间名</param>
        /// <param name="key">文件key</param>
        public static void Delete(string bucket, string key)
        {
            Console.WriteLine("\n===> Delete {0}:{1}", bucket, key);
            RSClient client = new RSClient();
            CallRet ret = client.Delete(new EntryPath(bucket, key));
            if (ret.OK)
            {
                Console.WriteLine("Delete OK");
            }
            else
            {
                Console.WriteLine("Failed to delete");
            }
        }

        //批量操作
        public static void BatchStat(string bucket, string[] keys)
        {
            RSClient client = new RSClient();
            List<EntryPath> EntryPaths = new List<EntryPath>();
            foreach (string key in keys)
            {
                Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
                EntryPaths.Add(new EntryPath(bucket, key));
            }
            client.BatchStat(EntryPaths.ToArray());
        }
        public static void BatchCopy(string bucket, string[] keys)
        {
            List<EntryPathPair> pairs = new List<EntryPathPair>();
            foreach (string key in keys)
            {
                EntryPathPair entry = new EntryPathPair(bucket, key, Guid.NewGuid().ToString());
                pairs.Add(entry);
            }
            RSClient client = new RSClient();
            client.BatchCopy(pairs.ToArray());
        }
        public static void BatchMove(string bucket, string[] keys)
        {
            List<EntryPathPair> pairs = new List<EntryPathPair>();
            foreach (string key in keys)
            {
                EntryPathPair entry = new EntryPathPair(bucket, key, Guid.NewGuid().ToString());
                pairs.Add(entry);
            }
            RSClient client = new RSClient();
            client.BatchMove(pairs.ToArray());
        }
        public static void BatchDelete(string bucket, string[] keys)
        {
            RSClient client = new RSClient();
            List<EntryPath> EntryPaths = new List<EntryPath>();
            foreach (string key in keys)
            {
                Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
                EntryPaths.Add(new EntryPath(bucket, key));
            }
            client.BatchDelete(EntryPaths.ToArray());
        }

        /// <summary>
        /// 上传本地文件
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="fname"></param>
        /// <param name="putFinished">上传完成事件</param>
        public static void PutFile(string bucket, string key, string fname, EventHandler<PutRet> putFinished = null)
        {
            var policy = new PutPolicy(bucket, 3600);
            string upToken = policy.Token();
            PutExtra extra = new PutExtra();
            IOClient client = new IOClient();
            if (putFinished != null)
                client.PutFinished += putFinished;
            client.PutFile(upToken, key, fname, extra);
        }

        /// <summary>
        /// 断点续上传本地文件
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="fname"></param>
        public static void ResumablePutFile(string bucket, string key, string fname)
        {
            Console.WriteLine("\n===> ResumablePutFile {0}:{1} fname:{2}", bucket, key, fname);
            PutPolicy policy = new PutPolicy(bucket, 3600);
            string upToken = policy.Token();
            Settings setting = new Settings();
            ResumablePutExtra extra = new ResumablePutExtra();
            ResumablePut client = new ResumablePut(setting, extra);
            client.PutFile(upToken, fname, Guid.NewGuid().ToString());
        }
    }
}
