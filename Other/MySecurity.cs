using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MyUtil.Other
{
    public class MySecurity
    {
        /// <summary>
        /// 初始化安全类
        /// </summary>
        public MySecurity()
        {
            ///默认密钥
            key = "0123456789";
        }
        private string key; //默认密钥

        private byte[] sKey;
        private byte[] sIV;

        #region 加密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        static public string SEncryptString(string inputStr, string keyStr)
        {
            MySecurity ws = new MySecurity();
            return ws.EncryptString(inputStr, keyStr);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        public string EncryptString(string inputStr, string keyStr)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                if (keyStr == "")
                    keyStr = key;
                byte[] inputByteArray = Encoding.Default.GetBytes(inputStr);
                byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
                SHA1 ha = new SHA1Managed();
                byte[] hb = ha.ComputeHash(keyByteArray);
                sKey = new byte[8];
                sIV = new byte[8];
                for (int i = 0; i < 8; i++)
                    sKey[i] = hb[i];
                for (int i = 8; i < 16; i++)
                    sIV[i - 8] = hb[i];
                des.Key = sKey;
                des.IV = sIV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                cs.Close();
                ms.Close();
                return ret.ToString();
            }
            catch
            {
                return "";
            }
           
        }
        #endregion

        #region 解密字符串
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        static public string SDecryptString(string inputStr, string keyStr)
        {
            MySecurity ws = new MySecurity();
            return ws.DecryptString(inputStr, keyStr);
        }
        /// <summary>
        ///  解密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        static public string SDecryptString(string inputStr)
        {
            MySecurity ws = new MySecurity();
            return ws.DecryptString(inputStr, "");
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        public string DecryptString(string inputStr, string keyStr)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                if (keyStr == "")
                    keyStr = key;
                byte[] inputByteArray = new byte[inputStr.Length / 2];
                for (int x = 0; x < inputStr.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(inputStr.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
                SHA1 ha = new SHA1Managed();
                byte[] hb = ha.ComputeHash(keyByteArray);
                sKey = new byte[8];
                sIV = new byte[8];
                for (int i = 0; i < 8; i++)
                    sKey[i] = hb[i];
                for (int i = 8; i < 16; i++)
                    sIV[i - 8] = hb[i];
                des.Key = sKey;
                des.IV = sIV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }catch
            {
                return "";
            }
            
        }
        #endregion

        #region 加密字符串 密钥为系统默认 0123456789
        /// <summary>
        /// 加密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>输出加密后字符串</returns>
        static public string SEncryptString(string inputStr)
        {
            MySecurity ws = new MySecurity();
            return ws.EncryptString(inputStr, "");
        }
        #endregion

        #region 加密文件
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">加密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>  
        public bool EncryptFile(string filePath, string savePath, string keyStr)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                if (keyStr == "")
                    keyStr = key;
                FileStream fs = File.OpenRead(filePath);
                byte[] inputByteArray = new byte[fs.Length];
                fs.Read(inputByteArray, 0, (int)fs.Length);
                fs.Close();
                byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
                SHA1 ha = new SHA1Managed();
                byte[] hb = ha.ComputeHash(keyByteArray);
                sKey = new byte[8];
                sIV = new byte[8];
                for (int i = 0; i < 8; i++)
                    sKey[i] = hb[i];
                for (int i = 8; i < 16; i++)
                    sIV[i - 8] = hb[i];
                des.Key = sKey;
                des.IV = sIV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                fs = File.OpenWrite(savePath);
                foreach (byte b in ms.ToArray())
                {
                    fs.WriteByte(b);
                }
                fs.Close();
                cs.Close();
                ms.Close();
                return true;
            }catch
            {
                return false;
            }
            
        }
        #endregion

        #region 解密文件
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">解密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>    
        public bool DecryptFile(string filePath, string savePath, string keyStr)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                if (keyStr == "")
                    keyStr = key;
                FileStream fs = File.OpenRead(filePath);
                byte[] inputByteArray = new byte[fs.Length];
                fs.Read(inputByteArray, 0, (int)fs.Length);
                fs.Close();
                byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
                SHA1 ha = new SHA1Managed();
                byte[] hb = ha.ComputeHash(keyByteArray);
                sKey = new byte[8];
                sIV = new byte[8];
                for (int i = 0; i < 8; i++)
                    sKey[i] = hb[i];
                for (int i = 8; i < 16; i++)
                    sIV[i - 8] = hb[i];
                des.Key = sKey;
                des.IV = sIV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                fs = File.OpenWrite(savePath);
                foreach (byte b in ms.ToArray())
                {
                    fs.WriteByte(b);
                }
                fs.Close();
                cs.Close();
                ms.Close();
                return true;
            }catch
            {
                return false;
            }
            
            
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// 128位MD5算法加密字符串
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        public static string MD5(string text)
        {

            //如果字符串为空，则返回
            if (Tools.IsNullOrEmpty<string>(text))
            {
                return "";
            }

            byte[] buffer = Encoding.Default.GetBytes(text); //将字符串解析成字节数组，随便按照哪种解析格式都行

            //返回MD5值的字符串表示
            return MD5(buffer);
        }

        /// <summary>
        /// 128位MD5算法加密Byte数组
        /// </summary>
        /// <param name="data">要加密的Byte数组</param>    
        public static string MD5(byte[] data)
        {
            //如果Byte数组为空，则返回
            if (Tools.IsNullOrEmpty<byte[]>(data))
            {
                return "";
            }

            try
            {
                //创建MD5密码服务提供程序
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                byte[] result = md5.ComputeHash(data);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return Convert.ToBase64String(result);
            }
            catch
            {

                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "";
            }
        }

        #endregion

        #region Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (Tools.IsNullOrEmpty<string>(text))
            {
                return "";
            }

            try
            {
                char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
                                            'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
                                            'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
                                            '8','9','+','/','='};
                byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                StringBuilder outmessage;
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use = 0;
                if ((use = messageLen % 3) > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                outmessage = new System.Text.StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (Tools.IsNullOrEmpty<string>(text))
            {
                return "";
            }

            //将空格替换为加号
            text = text.Replace(" ", "+");

            try
            {
                if ((text.Length % 4) != 0)
                {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }
                string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.Default.GetString(outbyte);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RSA加密解密

        #region RSA 的密钥产生 
        /// <summary>
        /// RSA 的密钥产生 产生私钥 和公钥 
        /// </summary>
        /// <param name="xmlKeys">私钥</param>
        /// <param name="xmlPublicKey">公钥</param>
        public static void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }
        #endregion

        #region RSA的加密函数 
        //############################################################################## 
        //RSA 方式加密 
        //说明KEY必须是XML的行式,返回的是字符串 
        //在有一点需要说明！！该加密方式有 长度 限制的！！ 
        //############################################################################## 

        //RSA的加密函数  string
        public static string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(m_strEncryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }catch
            {
                return "";
            }
            
        }

        //RSA的加密函数 byte[]
        public static string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            try
            {
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                CypherTextBArray = rsa.Encrypt(EncryptString, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch
            {
                return "";
            }
            

        }
        #endregion

        #region RSA的解密函数 
        //RSA的解密函数  string
        public static string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;
            }catch
            {
                return "";
            }
            

        }

        //RSA的解密函数  byte
        public static string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            try
            {
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                DypherTextBArray = rsa.Decrypt(DecryptString, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;
            }catch
            {
                return "";
            }
           

        }
        #endregion

        #endregion

        #region DES加密
        /// <summary>
        /// C# DES解密方法
        /// </summary>
        /// <param name="encryptedValue">待解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecrypt(string encryptedValue, string key, string iv)
        {
            try
            {
                using (DESCryptoServiceProvider sa =
                new DESCryptoServiceProvider
                { Key = Encoding.UTF8.GetBytes(key), IV = Encoding.UTF8.GetBytes(iv) })
                {
                    using (ICryptoTransform ct = sa.CreateDecryptor())
                    {
                        byte[] byt = Convert.FromBase64String(encryptedValue);

                        using (var ms = new MemoryStream())
                        {
                            using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                            {
                                cs.Write(byt, 0, byt.Length);
                                cs.FlushFinalBlock();
                            }
                            return Encoding.UTF8.GetString(ms.ToArray());
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            
        }
        #endregion

        #region DES解密
        /// <summary>
        /// C# DES加密方法
        /// </summary>
        /// <param name="encryptedValue">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncrypt(string originalValue, string key, string iv)
        {
            try
            {
                using (DESCryptoServiceProvider sa
                = new DESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(key), IV = Encoding.UTF8.GetBytes(iv) })
                {
                    using (ICryptoTransform ct = sa.CreateEncryptor())
                    {
                        byte[] by = Encoding.UTF8.GetBytes(originalValue);
                        using (var ms = new MemoryStream())
                        {
                            using (var cs = new CryptoStream(ms, ct,
                                                             CryptoStreamMode.Write))
                            {
                                cs.Write(by, 0, by.Length);
                                cs.FlushFinalBlock();
                            }
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            
        }
        #endregion

        #region AES加密/解密(无向量)

        public static string AESEncrypt(string str)
        {
            return AESEncrypt(str, "");
        }

        public static string AESDecrypt(string str)
        {
            return AESDecrypt(str, "");
        }

        public static string AESEncrypt(string str, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return null;
                Byte[] toEncryptArray = Encoding.Default.GetBytes(str);

                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = Encoding.Default.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                string a = BitConverter.ToString(resultArray).Replace("-", "");
                return a;
            }catch
            {
                return "";
            }
           
            //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AESDecrypt(string str, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return null;
                Byte[] toEncryptArray = Convert.FromBase64String(str);

                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Encoding.UTF8.GetString(resultArray);
            }catch
            {
                return "";
            }
            
        }

        #endregion
    }
}
