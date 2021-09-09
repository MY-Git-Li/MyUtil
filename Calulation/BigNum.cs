using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil.Calulation
{
    public class BigNum
    {
        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>返回结果</returns>
        public static string Add(char[] num1, char[] num2)
        {
            //对两个加数进行初始化，使长度相同
            if (num1.Length > num2.Length)
                num2 = InitString(num1, num2);
            if (num1.Length < num2.Length)
                num1 = InitString(num1, num2);
            string result = "";
            string finalRes = "";
            //每一位相加都保留进位
            int jinwei = 0;
            for (int i = num1.Length - 1; i >= 0; i--)
            {
                //将字符转换为数字
                int res = num1[i] + num2[i] - 2 * '0' + jinwei;
                jinwei = 0;
                if (res >= 10)
                {
                    jinwei = 1;
                    res -= 10;
                }
                result += res;
            }
            if (jinwei > 0)
                result += jinwei;
            //将结果反转
            for (int i = result.Length - 1; i >= 0; i--)
            {
                finalRes += result[i];
            }
            //移除不必要的零
            finalRes = RemoveZero(finalRes);
            return finalRes;
        }

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>返回结果</returns>
        public static string Sub(char[] num1, char[] num2)
        {
            string result = "";
            string finalRes = "";
            //保留两个数相减的借位，用于下一位计算
            int jiewei = 0;
            //判断两个数的大小，决定是否加“-”号
            int bigger = num1BiggerThanNum2(num1, num2);
            if (bigger < 0)
            {
                char[] c = num1;
                num1 = num2;
                num2 = c;
                finalRes += '-';
            }
            if (bigger == 0)
                return "0";
            //将两个数初始化，使位数相同
            if (num1.Length > num2.Length)
                num2 = InitString(num1, num2);
            if (num1.Length < num2.Length)
                num1 = InitString(num1, num2);
            for (int i = num1.Length - 1; i >= 0; i--)
            {
                //两数相减，不需要将字符转换为数字
                int res = num1[i] - num2[i] - jiewei;
                jiewei = 0;
                if (res < 0)
                {
                    res += 10;
                    jiewei = 1;
                }
                result += res;
            }
            //将结果反转
            for (int i = result.Length - 1; i >= 0; i--)
            {
                finalRes += result[i];
            }
            finalRes = RemoveZero(finalRes);
            return finalRes;
        }


        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>返回结果</returns>
        public static string Mut(char[] num1, char[] num2)
        {
            string finalRes = "0";
            //判断大小，将较大的数放到上方，较小的数用来乘较大的数
            int bigger = num1BiggerThanNum2(num1, num2);
            if (bigger < 0)
            {
                char[] c = num1;
                num1 = num2;
                num2 = c;
            }
            //两个数相乘的进位，用于计算
            int jinwei = 0;
            for (int i = 0; i < num2.Length; i++)
            {
                string result = "";
                string str = "";
                for (int j = num1.Length - 1; j >= 0; j--)
                {
                    int res = (num1[j] - '0') * (num2[num2.Length - i - 1] - '0') + jinwei;
                    jinwei = 0;
                    if (res >= 10)
                    {
                        int temp = res;
                        res = res % 10;
                        jinwei = temp / 10;
                    }
                    str += res;
                }
                if (jinwei > 0)
                    str += jinwei;
                //将每一位相乘的结果反转
                for (int k = str.Length - 1; k >= 0; k--)
                {
                    result += str[k];
                }
                //每相乘一位，都需要在末尾加零
                for (int k = 0; k < i; k++)
                {
                    result += "0";
                }
                //将每一位相乘的结果与之前的结果相加
                finalRes = Add(finalRes.ToCharArray(), result.ToCharArray());
            }


            return finalRes;
        }

        public static string Pow(string x,char y)
        {
            char[] num1 = x.ToCharArray();
            char[] num2 = new char[] { y };

            for (char i = '0'; i < y; i++)
            {
               string num12 = Mut(num1, num2);
               num1 = num12.ToCharArray();
            }


            char[] retnum = new char[] { '1' };
            return Mut(num1, retnum);
        }
        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>返回的字符数组中，第一个元素代表商，第二个元素代表余数</returns>
        public static string[] Des(char[] num1, char[] num2)
        {
            string[] finalRes = new string[2];
            //判断两个数的大小，如果除数大于被除数，则直接返回结果
            int bigger = num1BiggerThanNum2(num1, num2);
            if (bigger < 0)
            {
                finalRes[0] = "0";
                for (int i = 0; i < num1.Length; i++)
                {
                    finalRes[1] += num1[i];
                }
            }
            else if (bigger == 0)
            {
                finalRes[0] = "1";
                finalRes[1] = "0";
            }
            else//除数小于被除数
            {
                //被除数，从除数的位数开始逐个记录
                string beichushu = "";
                string shang = "";
                for (int i = 0; i < num2.Length; i++)
                {
                    beichushu += num1[i];
                }
                int flag = num2.Length - 1;
                for (int i = num2.Length - 1; i < num1.Length - 1; i++)
                {
                    //第一轮除法计算完成，添加被除数下一位
                    if (num1BiggerThanNum2(beichushu.ToCharArray(), num2) < 0)
                    {

                        if (i + 1 == num1.Length)
                            break;
                        beichushu += num1[i + 1];
                    }
                    //判断当前被除数是否大于除数，如果小于则添加被除数下一位
                    if (num1BiggerThanNum2(beichushu.ToCharArray(), num2) < 0)
                    {

                        if (i + 1 == num1.Length)
                            break;
                        beichushu += num1[i + 1];
                        shang += "0";
                        continue;
                    }
                    //判断当前除数与被除数大小，如果两数相等，则被除数减为0，商为1
                    if (num1BiggerThanNum2(beichushu.ToCharArray(), num2) == 0)
                    {
                        beichushu = "0";
                        shang += "1";
                    }
                    //当除数小于被除数，则进行减法计算，看能够被多少个除数除
                    if (num1BiggerThanNum2(beichushu.ToCharArray(), num2) > 0)
                    {
                        string currentShang = "0";
                        for (int j = 1; num1BiggerThanNum2(beichushu.ToCharArray(), num2) >= 0; j++)
                        {
                            beichushu = Sub(beichushu.ToCharArray(), num2);
                            currentShang = j.ToString();
                        }
                        shang += currentShang;
                    }
                }
                //将结果中多余的零去除
                finalRes[0] = RemoveZero(shang);

                finalRes[1] = RemoveZero(beichushu);
                //如果商为0，则返回0
                if (finalRes[1] == "")
                    finalRes[1] = "0";
            }
            return finalRes;
        }


        /// <summary>
        /// 将两个字符串进行初始化，以达到相同的长度，在较短的字符串前边补零
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>返回初始化后的字符串</returns>
        public static char[] InitString(char[] a, char[] b)
        {
            int aLength = a.Length;
            int bLength = b.Length;
            char[] c = new char[Math.Max(aLength, bLength)];
            int start = Math.Abs(aLength - bLength);
            for (int i = 0; i < Math.Max(aLength, bLength); i++)
            {
                if (i < start)
                    c[i] = '0';
                else
                {
                    if (aLength > bLength)
                        c[i] = b[i - start];
                    if (aLength < bLength)
                        c[i] = a[i - start];
                }
            }
            return c;
        }

        /// <summary>
        /// 判断两个数的大小
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>num1大于num2，则返回1；num1等于num2，则返回0；num1小于num2，则返回-1；</returns>
        public static int num1BiggerThanNum2(char[] num1, char[] num2)
        {
            if (num1.Length > num2.Length)
            {
                return 1;
            }
            if (num1.Length < num2.Length)
            {
                return -1;
            }
            if (num1.Length == num2.Length)
            {
                for (int i = 0; i < num2.Length; i++)
                {
                    if (num1[i] > num2[i])
                        return 1;
                    else if (num1[i] < num2[i])
                        return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 移除字符串前边不必要的零
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回正确字符串</returns>
        public static string RemoveZero(string str)
        {
            string result = "";
            bool start = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '0')
                {
                    start = true;
                }
                if (start)
                    result += str[i];
            }
            return result;
        }
    }
}

