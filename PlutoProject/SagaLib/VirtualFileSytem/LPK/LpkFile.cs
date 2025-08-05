using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using SagaLib.VirtualFileSytem.LPK.LZMA;

namespace SagaLib.VirtualFileSytem.LPK
{
    /* LPK 文件格式 1.0
     * struct
     * {
     *      uint magicNumber = 0x004B504C;
     *      uint hashOffset;
     *      byte[] data;
     *      uint hashSize;
     *      Dictionary<string, int> hashTable;
     *      LpkFileInfo[fileCount] fileMetadata;
     * }
     */

    public class LpkFile
    {
        private static readonly byte[] key = Encoding.ASCII.GetBytes("1234567890123456");
        private readonly Rijndael aes = Rijndael.Create();
        private readonly Stream fileStream;
        private readonly Dictionary<string, int> hashTable;
        private int hashSize, hashOffset;

        public LpkFile(Stream stream)
        {
            fileStream = stream;
            aes.Mode = CipherMode.ECB;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.None;

            if (stream.Length != 0)
            {
                var sr = new BinaryReader(stream);
                var bf = new BinaryFormatter();
                var magic = sr.ReadInt32();
                //检查幻数
                if (magic != 0x004B504C) throw new Exception("This is not a LPK Archive");
                try
                {
                    //哈希表偏移
                    hashOffset = sr.ReadInt32();
                    stream.Position = hashOffset;
                    //哈希表尺寸
                    hashSize = sr.ReadInt32();

                    //读取哈希表
                    hashTable = (Dictionary<string, int>)bf.Deserialize(
                        new MemoryStream(Decrypt(sr.ReadBytes(hashSize))));
                }
                catch (Exception ex)
                {
                    throw new Exception("This Archive is corrupted and cannot be opened", ex);
                }

                if (hashTable == null) throw new Exception("This Archive is corrupted and cannot be opened");
            }
            //新建哈希表
            else
            {
                hashTable = new Dictionary<string, int>();
                var bw = new BinaryWriter(stream);
                bw.Write(0x004B504C);
                hashSize = 0;
                hashOffset = 8;
                bw.Write(hashOffset);
                bw.Write(hashSize);
            }

            GetFileNames = GetInfos();
        }

        /// <summary>
        ///     取得所有文件的元信息
        /// </summary>
        /// <returns>元信息</returns>
        public List<LpkFileInfo> GetFileNames { get; private set; } = new List<LpkFileInfo>();

        /// <summary>
        ///     文件总数
        /// </summary>
        public int FileCount => hashTable.Count;

        /// <summary>
        ///     总大小
        /// </summary>
        public long TotalSize
        {
            get
            {
                long size = 0;
                foreach (var i in GetFileNames) size += i.UncompressedSize;
                return size;
            }
        }

        /// <summary>
        ///     总压缩后大小
        /// </summary>
        public long TotalCompressedSize
        {
            get
            {
                long size = 0;
                foreach (var i in GetFileNames) size += i.FileSize;
                return size;
            }
        }

        internal byte[] HashBuffer
        {
            get
            {
                var bf = new BinaryFormatter();
                var ms = new MemoryStream();
                bf.Serialize(ms, hashTable);
                ms.Flush();
                var rest = 1024 - ms.Length % 1024;
                ms.SetLength(ms.Length + rest);
                ms.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     取得文件元信息
        /// </summary>
        /// <param name="name">文件名</param>
        /// <returns>元信息</returns>
        public LpkFileInfo GetInfo(string name)
        {
            fileStream.Position = hashTable[name];
            var info = new LpkFileInfo(fileStream);
            info.Name = name;
            return info;
        }

        private List<LpkFileInfo> GetInfos()
        {
            if (hashTable != null)
            {
                var list = new List<LpkFileInfo>();
                foreach (var i in hashTable.Keys)
                {
                    fileStream.Position = hashTable[i];
                    var info = new LpkFileInfo(fileStream);
                    info.Name = i;
                    list.Add(info);
                }

                return list;
            }

            return new List<LpkFileInfo>();
        }

        /// <summary>
        ///     检查某文件是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool Exists(string fileName)
        {
            return hashTable.ContainsKey(fileName);
        }

        public void Close()
        {
            fileStream.Flush();
            fileStream.Close();
        }

        /// <summary>
        ///     向压缩包添加文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="inStream">要添加的文件的流</param>
        public void AddFile(string fileName, Stream inStream)
        {
            AddFile(fileName, inStream, null);
        }

        /// <summary>
        ///     向压缩包添加文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="inStream">要添加的文件的流</param>
        /// <param name="progress">压缩进度回调对象</param>
        public void AddFile(string fileName, Stream inStream, ICodeProgress progress)
        {
            lock (hashTable)
            {
                //元信息备份缓存
                var metaBackup = new byte[LpkFileInfo.Size * hashTable.Count];
                uint crc;
                //备份老哈希表偏移
                var oldHashOffset = hashOffset;

                var bwOri = new BinaryWriter(fileStream);
                uint size, uncompressedSize;

                if (hashTable.ContainsKey(fileName))
                    throw new ArgumentException("A file with this name(" + fileName + ") already exists!");

                crc = LzmaHelper.CRC32(inStream);
                inStream.Position = 0;
                //取得要添加的文件的原大小
                uncompressedSize = (uint)inStream.Length;

                //备份压缩包原元信息
                fileStream.Position = hashOffset + hashSize + 4;
                fileStream.Read(metaBackup, 0, LpkFileInfo.Size * hashTable.Count);

                //取得新哈希表大小
                hashTable.Add(fileName, 0);
                var newHashSize = HashBuffer.Length;

                //压缩并写入新加文件数据
                fileStream.Position = hashOffset;
                LzmaHelper.Compress(inStream, fileStream, progress);
                //取得新加文件压缩后大小
                size = (uint)(fileStream.Position - hashOffset);
                //设置哈希表新偏移
                hashOffset = (int)fileStream.Position;

                //将备份的元信息写回文件，便于之后修正
                fileStream.Position += newHashSize + 4;
                fileStream.Write(metaBackup, 0, metaBackup.Length);
                //将哈希表新偏移写回文件
                fileStream.Position = 4;
                bwOri.Write(hashOffset);

                //开始修正元数据
                fileStream.Position = hashOffset + 4 + newHashSize;
                var files = new string[hashTable.Count];
                hashTable.Keys.CopyTo(files, 0);

                foreach (var i in files)
                {
                    LpkFileInfo info;

                    if (i != fileName)
                    {
                        //修正文件元数据偏移
                        hashTable[i] += hashOffset - oldHashOffset + (newHashSize - hashSize);
                        fileStream.Position = hashTable[i];
                        info = new LpkFileInfo(fileStream);
                        info.HeaderOffset = (uint)hashTable[i];
                    }
                    else
                    {
                        //计算新文件元数据偏移
                        hashTable[i] = 4 + hashOffset + newHashSize + (hashTable.Count - 1) * LpkFileInfo.Size;
                        info = new LpkFileInfo();
                        //计算元数据信息
                        info.HeaderOffset = (uint)hashTable[i];
                        info.DataOffset = (uint)oldHashOffset;
                        info.UncompressedSize = uncompressedSize;
                        info.CRC = crc;
                        info.FileSize = size;
                    }

                    //将修正后的元数据写回文件
                    info.WriteToStream(fileStream);
                }

                //将新哈希表写回文件
                fileStream.Position = hashOffset;
                bwOri.Write(newHashSize);
                bwOri.Write(Encrypt(HashBuffer));
                hashSize = newHashSize;

                GetFileNames = GetInfos();
            }
        }

        /// <summary>
        ///     从压缩包解压某文件到内存
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public MemoryStream OpenFile(string fileName)
        {
            return OpenFile(fileName, null);
        }

        /// <summary>
        ///     从压缩包解压某文件到内存
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="progress">解压进度回调对象</param>
        /// <returns></returns>
        public MemoryStream OpenFile(string fileName, ICodeProgress progress)
        {
            lock (hashTable)
            {
                var ms = new MemoryStream();
                fileStream.Position = hashTable[fileName];
                var fileInfo = new LpkFileInfo(fileStream);
                fileStream.Position = fileInfo.DataOffset;
                try
                {
                    LzmaHelper.Decompress(fileStream, ms, fileInfo.FileSize, fileInfo.UncompressedSize, progress);
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                    throw new Exception(string.Format("File:{1} CRC({0:X}) error, file open failed!", fileInfo.CRC,
                        fileName));
                }

                ms.Position = 0;
                var crc = LzmaHelper.CRC32(ms);
                if (fileInfo.CRC != crc)
                    throw new Exception(string.Format("CRC(Original:{0:X} Current:{1:X}) error, file open failed!",
                        fileInfo.CRC, crc));
                ms.Position = 0;
                return ms;
            }
        }

        internal byte[] Encrypt(byte[] buff)
        {
            var output = new byte[buff.Length];
            var left = buff.Length % 1024;
            var crypt = aes.CreateEncryptor(key, new byte[16]);
            var buf = new byte[1024];
            for (var i = 0; i < buff.Length / 1024; i++)
            {
                Array.Copy(buff, i * 1024, buf, 0, 1024);
                crypt.TransformBlock(buf, 0, 1024, buf, 0);
                Array.Copy(buf, 0, output, i * 1024, 1024);
            }

            Array.Copy(buff, buff.Length - left, output, buff.Length - left, left);
            return output;
        }

        internal byte[] Decrypt(byte[] buff)
        {
            var output = new byte[buff.Length];
            var left = buff.Length % 1024;
            var crypt = aes.CreateDecryptor(key, new byte[16]);
            var buf = new byte[1024];
            for (var i = 0; i < buff.Length / 1024; i++)
            {
                Array.Copy(buff, i * 1024, buf, 0, 1024);
                crypt.TransformBlock(buf, 0, 1024, buf, 0);
                Array.Copy(buf, 0, output, i * 1024, 1024);
            }

            Array.Copy(buff, buff.Length - left, output, buff.Length - left, left);
            return output;
        }
    }
}