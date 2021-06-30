using System;
using System.Collections.Generic;
using System.Text;

namespace MyUtil.Calulation
{
  
    public class Vector
    {
        int row, line;
        List<List<float>> digital;

        public Vector(int row, int line, float[] digital)
        {
            this.row = row;
            this.line = line;
            if (row * line == digital.Length || row * line < digital.Length)
            {
                this.digital = GetDigital(digital);
            }
            else if (row * line > digital.Length)
            {
                float[] newdigital = new float[row * line];
                for (int i = 0; i < row * line; i++)
                {
                    if (i < digital.Length)
                    {
                        newdigital[i] = digital[i];
                    }
                    else
                    {
                        newdigital[i] = 0;
                    }

                }
                this.digital = GetDigital(newdigital);
            }


        }

        public Vector(int row, int line, List<List<float>> digital)
        {
            this.row = row;
            this.line = line;
            //验证 row 和line 与 digital 的匹配
            if (digital.Count * digital[0].Count != row * line)
            {
                List<float> digitals = new List<float>();

                for (int i = 0; i < digital.Count; i++)
                {
                    for (int j = 0; j < digital[0].Count; j++)
                    {
                        digitals.Add(digital[i][j]);
                    }
                }

                float[] digitalss = digitals.ToArray();

                if (row * line < digitalss.Length)
                {
                    this.digital = GetDigital(digitalss);
                }
                else if (row * line > digitalss.Length)
                {
                    float[] newdigital = new float[row * line];
                    for (int i = 0; i < row * line; i++)
                    {
                        if (i < digitalss.Length)
                        {
                            newdigital[i] = digitalss[i];
                        }
                        else
                        {
                            newdigital[i] = 0;
                        }

                    }
                    this.digital = GetDigital(newdigital);
                }



            }
            else
            {

                this.digital = digital;
            }



        }

        public Vector(int row, int line) : this(row, line, new float[] { 0 })
        {
        }

        public Vector(string str)
        {
            string[] array = str.Split(';');
            int row = array.Length;
            int line = 0;
            List<float> digital = GetNumber(array, row, ref line);
            this.row = row;
            this.line = line;
            this.digital = GetDigital(digital.ToArray());
        }

        List<float> GetNumber(string[] array, int row, ref int line)
        {
            //12 1   1
            //10  1    1
            //5 5 5

            //得到字符串数据；处理掉多余的空格或者逗号
            string number = "";
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    if (IsNumber(array[i][j]))
                    {
                        number += ' ';
                    }
                    else
                    {
                        number += array[i][j];
                    }
                }
                if (i != array.Length - 1)
                {
                    number += ' ';
                }

            }

            //row * line = leng;
            //将字符串数据加入到列表中
            string[] numbers = number.Split(' ');


            List<float> digital = new List<float>();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] != " " && numbers[i] != "")
                {
                    digital.Add((float)Convert.ToDouble(numbers[i]));
                }
            }
            //得到行
            line = (int)(digital.Count * 1.0f / row);
            return digital;
        }

        List<List<float>> GetDigital(float[] digital)
        {
            List<List<float>> lists = new List<List<float>>();

            for (int i = 0; i < this.row; i++)
            {
                List<float> list_line = new List<float>();
                for (int j = 0; j < this.line; j++)
                {
                    list_line.Add(digital[i * line + j]);
                }
                lists.Add(list_line);
            }

            return lists;
        }

        /// <summary>
        /// 显示矩阵
        /// </summary>
        public string Show()
        {
            string str = "";
            for (int i = 0; i < row; i++)
            {
                //ToDo
                // --    --
                //|45 45 45|
                //|        |
                //|        |
                // --    --
                for (int j = 0; j < line; j++)
                {
                    str += digital[i][j];
                    str += "\t";
                }
                str += "\n";
            }

            return str;
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="b">矩阵加量</param>
        /// <returns>和矩阵</returns>
        public Vector Add(Vector b)
        {
            Vector c = new Vector(b.row, b.line, new float[] { 0 });
            if (IsSame(this, b))
            {
                for (int i = 0; i < b.row; i++)
                {
                    for (int j = 0; j < b.line; j++)
                    {
                        c.digital[i][j] = this.digital[i][j] + b.digital[i][j];
                    }
                }
                return c;
            }
            ///不能加后返回结果0：
            return new Vector(1, 1, new float[] { 0 });
        }


        /// <summary>
        /// 矩阵减法
        /// </summary>
        /// <param name="b">矩阵减量</param>
        /// <returns>差矩阵</returns>
        public Vector Sub(Vector b)
        {
            Vector c = new Vector(b.row, b.line, new float[] { 0 });
            if (IsSame(this, b))
            {
                for (int i = 0; i < b.row; i++)
                {
                    for (int j = 0; j < b.line; j++)
                    {
                        c.digital[i][j] = this.digital[i][j] - b.digital[i][j];
                    }
                }
                return c;
            }
            ///不能减后返回结果  ：
            return new Vector(1, 1, new float[] { 0 });
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="num">实数</param>
        /// <returns>数乘矩阵</returns>
        public Vector NumMulVec(float num)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < line; j++)
                {
                    digital[i][j] = this.digital[i][j] * num;
                }
            }
            return this;
        }

        /// <summary>
        /// 矩阵积
        /// </summary>
        /// <param name="b">矩阵因子</param>
        /// <returns>积矩阵</returns>
        public Vector MulVec(Vector b)
        {
            Vector c = new Vector(this.row, b.line);
            if (IsMul(this, b))
            {

                for (int i = 0; i < this.row; i++)
                {
                    for (int j = 0; j < b.line; j++)
                    {
                        float sum = 0;
                        for (int s = 0; s < b.row; s++)
                        {
                            float a = this.digital[i][s];
                            float bb = b.digital[s][j];
                            sum += a * bb;
                        }
                        c.digital[i][j] = sum;
                    }

                }
                return c;
            }
            ///不能积后返回结果：
            c = new Vector(1, 1, new float[] { 0 });
            return c;
        }

        /// <summary>
        /// 得到代数余子式
        /// </summary>
        /// <param name="i">行</param>
        /// <param name="j">列</param>
        /// <returns></returns>
        Vector GetCofactor(int m, int n)
        {
            //判断是否能获得代数余子式
            if (row == line)
            {
                //符号判断
                float sym;
                if ((m + n) % 2 != 0)
                {
                    sym = -1;
                }
                else
                {
                    sym = 1;
                }
                //得到余氏矩阵
                List<float> num = new List<float>();
                for (int i = 0; i < row; i++)
                {
                    if (i != m)
                    {
                        for (int j = 0; j < line; j++)
                        {
                            if (j != n)
                            {
                                num.Add(this.digital[i][j]);
                            }
                        }
                    }

                }
                Vector cofactor = new Vector(row - 1, line - 1, num.ToArray());

                //判断是否为2阶矩阵，是返回结果
                if (cofactor.row == 2)
                {

                    return new Vector((sym * (cofactor.digital[0][0] * cofactor.digital[1][1] - cofactor.digital[0][1] * cofactor.digital[1][0])).ToString());
                }
                else if (cofactor.row > 2)//不是二阶矩阵，则求高阶余氏矩阵的行列式
                {
                    return sym * cofactor.GetDetmi();
                }
                else
                {
                    try
                    {
                        return new Vector((sym * cofactor.digital[0][0]).ToString());
                    }
                    catch
                    {
                        return new Vector("0");
                    }

                }

            }

            ///不是方阵，不能得出代数余子式，0返回
            return new Vector("0");
        }

        /// <summary>
        /// 得到行列式
        /// </summary>
        /// <returns></returns>
        public Vector GetDetmi()
        {


            if (row == line && row == 1)
            {
                return new Vector(digital[0][0].ToString());
            }

            Vector sum = new Vector("0");
            for (int i = 0; i < line; i++)
            {

                sum = sum + (digital[0][i] * GetCofactor(0, i));
            }
            return sum;
        }

        /// <summary>
        /// 得到伴随矩阵
        /// </summary>
        /// <returns></returns>
        public Vector GetStartVec()
        {
            if (row == line && row == 1)
            {
                return new Vector("1");
            }


            List<float> aij = new List<float>();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < line; j++)
                {
                    aij.Add(this.GetCofactor(i, j).digital[0][0]);
                }
            }

            return new Vector(row, line, aij.ToArray()).GetTransVec();
        }

        /// <summary>
        /// 得到转置矩阵
        /// </summary>
        /// <returns></returns>
        public Vector GetTransVec()
        {
            List<float> digital = new List<float>();
            for (int i = 0; i < line; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    digital.Add(this.digital[j][i]);
                }
            }
            return new Vector(line, row, digital.ToArray());
        }

        /// <summary>
        /// 得到逆矩阵
        /// </summary>
        /// <returns></returns>
        public Vector GetInvVec()
        {
            return GetDetmi() != new Vector("0") ? this.GetStartVec().NumMulVec(1 / this.GetDetmi().digital[0][0]) : new Vector(1, 1, new float[] { 0 });
        }

        /// <summary>
        /// 能否相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        bool IsSame(Vector a, Vector b)
        {
            return a.row == b.row && a.line == b.line;
        }

        /// <summary>
        /// 能否相乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        bool IsMul(Vector a, Vector b)
        {
            return a.line == b.row;
        }

        /// <summary>
        /// 是否合法数字
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        bool IsNumber(char a)
        {
            return (a < '0' || a > '9') && a != '-' && a != '.';
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        public void GetAllVec()
        {
            Console.WriteLine("原矩阵=");
            Show();

            Console.WriteLine("行列式=" + GetDetmi());
            Console.WriteLine();

            Console.WriteLine("转置矩阵=");
            GetTransVec().Show();

            Console.WriteLine("伴随矩阵=");
            GetStartVec().Show();

            Console.WriteLine("逆矩阵=");
            GetInvVec().Show();

            Console.WriteLine("A^2=");
            MulVec(this).Show();

            Console.WriteLine("A^3=");
            MulVec(this).MulVec(this).Show();
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return a.Add(b);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return a.Sub(b);
        }

        public static Vector operator *(Vector a, Vector b)
        {
            return a.MulVec(b);
        }

        public static Vector operator *(Vector a, float b)
        {
            return a.NumMulVec(b);
        }

        public static Vector operator *(float b, Vector a)
        {
            return a.NumMulVec(b);
        }

        public static bool operator ==(Vector b, Vector a)
        {
            if (b.line == a.line)
            {
                if (b.row == a.row)
                {
                    for (int i = 0; i < b.digital.Count; i++)
                    {
                        if (a.digital[i] != b.digital[i])
                        {
                            return false;
                        }

                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(Vector b, Vector a)
        {
            return a == b ? false : true;
        }

        public override bool Equals(object obj)
        {
            return this == (Vector)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 采用内置数据类型运算，导致精度不够使得分子分母数据差距很大时转换不理想。但能满足日常使用.....后续优化---使用字符串进行运算BigInteger类
    /// </summary>
    public class NumToFractioner
    {
        bool symbol;
        int dec;//整数部分
        string Num;  //小数部分
        bool isRecurringDecimal;
        int startRecurringDigit; //开始循环的位数
        int digit;  //循环的位数
        string Res;  //结果
        double numOrig; //原来的数

        double bainumerator;  //分母
        double denominator;//分子
        int accuracyFrequency = 3;//精度频率，循环小数出现的次数

        /// <summary>
        /// 转换后的分母
        /// </summary>
        public double Bainumerator { get => bainumerator; }

        /// <summary>
        /// 转换后分子
        /// </summary>
        public double Denominator { get => denominator; }

        /// <summary>
        /// 精度频率，循环小数出现的次数--用于判断循环节的位置。默认为3
        /// </summary>
        public int AccuracyFrequency { set => accuracyFrequency = value; }

        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="num">转换的小数</param>
        /// <returns>分数</returns>
        public string Run(double num)
        {
            numOrig = num;
            if (Init())
            {
                Cheack();
                Change();
            }

            return Res;

        }
        void Change()
        {
            if (isRecurringDecimal)
            {
                ChangeRecurringDecimal();
            }
            else
            {
                ChangeNotRecurringDecimal();
            }

        }



        void ChangeNotRecurringDecimal()
        {
            digit = Num.Length - 2;

            double def = Math.Pow(10, digit);//分母

            double mem = Convert.ToDouble(Num.Substring(2, digit)); //分子



            mem += dec * def;
            double factor = GetLargestCommonDivisor(def, mem);

            def = def / factor;
            mem = mem / factor;




            if (symbol)
            {
                bainumerator = -def;
                denominator = mem;
                Res = "-" + GetNumString(mem) + "/" + GetNumString(def);
            }
            else
            {
                bainumerator = def;
                denominator = mem;
                Res = GetNumString(mem) + "/" + GetNumString(def);
            }
        }

        void ChangeRecurringDecimal()
        {
            double def = Math.Pow(10, digit + startRecurringDigit) - Math.Pow(10, startRecurringDigit);//分母
            double mem;
            if (startRecurringDigit != 0)
            {
                mem = Convert.ToDouble(Num.Substring(2, digit + startRecurringDigit)) - Convert.ToDouble(Num.Substring(2, startRecurringDigit)); //分子

            }
            else
            {
                mem = Convert.ToDouble(Num.Substring(2, digit + startRecurringDigit)); //分子

            }

            mem += dec * def;

            double factor = GetLargestCommonDivisor(def, mem);

            def = def / factor;
            mem = mem / factor;


            if (symbol)
            {
                bainumerator = -def;
                denominator = mem;
                Res = "-" + GetNumString(mem) + "/" + GetNumString(def);
            }
            else
            {
                bainumerator = def;
                denominator = mem;
                Res = GetNumString(mem) + "/" + GetNumString(def);
            }


        }



        bool Init()
        {
            if (numOrig > 0)
            {
                symbol = false;
            }
            else
            {
                symbol = true;
                numOrig = -numOrig;
            }

            if (numOrig == 0)
            {
                Res = "0";
                denominator = 0;
                bainumerator = 0;
                return false;
            }
            else
            {
                dec = (int)numOrig;

                if (numOrig - dec == 0)
                {
                    Res = GetNumString(dec);
                    denominator = dec;
                    bainumerator = 1;
                    return false;
                }
                else
                {

                    Num = GetNumString(numOrig - dec);
                }

                return true;
            }


        }


        string GetNumString(double num)
        {
            string Ress = "";

            if (num == 0)
            {
                return "0.0";
            }

            if (num < 1f)
            {
                Ress = "0.";
                while (num < 0.001)
                {
                    Ress += "0";
                    num *= 10;
                }
                Ress += num.ToString().Substring(2);
            }
            else
            {

                int dige = 0;
                while (num >= 1f)
                {
                    num = num / 10;
                    dige++;
                }

                Ress = "0.";
                while (num < 0.001)
                {
                    Ress += "0";
                    num *= 10;
                }
                Ress += num.ToString().Substring(2);
                Ress = Ress.Substring(2);

                while (Ress.Length < dige)
                {
                    Ress += "0";
                }

            }

            return Ress;
        }


        void Cheack()
        {

            bool yes = false;

            int maybeDigit = Num.Length - 2;
            bool go = false;
            int k;
            //从第几位开始循环
            for (k = 0; k < Num.Length - 2; k++)
            {
                //i 可能循环的位数
                for (int i = 1; i < Num.Length - 2; i++)
                {
                    yes = true;
                    //j 精确验证次数
                    for (int j = 0; j < accuracyFrequency; j++)
                    {


                        if (Num.Substring(k + 2, 1) == "0")
                        {
                            yes = false;
                            break;
                        }


                        int x = FUN(i, j, k);

                        if (i > Num.Length - 2 - k)
                        {
                            yes = false;
                            break;
                        }

                        int y = i;

                        if (x > Num.Length - 1)
                        {
                            if (k == Num.Length - 3)
                            {
                                yes = true;
                                break;
                            }
                            yes = false;
                            break;

                        }

                        if (x + y > Num.Length)
                        {
                            yes = false;
                            break;
                        }


                        if (Num.Substring(k + 2, i) != Num.Substring(x, y))
                        {
                            yes = false;
                            break;
                        }
                        else
                        {
                            yes = true;

                        }

                    }

                    maybeDigit = i;
                    if (yes)
                    {
                        go = true;
                        break;
                    }

                }
                if (go)
                {
                    break;
                }

            }

            if (yes)
            {
                startRecurringDigit = k;
            }


            if (yes && maybeDigit != Num.Length - 2 && startRecurringDigit != Num.Length - 3)
            {
                digit = maybeDigit;
                isRecurringDecimal = true;

            }
            else
            {
                digit = 0;
                isRecurringDecimal = false;
            }


        }

        static int FUN(int i, int j, int k)
        {
            if (j == 0)
            {
                return 2 + k;
            }
            else
            {
                return FUN(i, j - 1, k) + i;
            }
        }

        /// <summary>
        /// 得到使用转换的小数
        /// </summary>
        /// <returns></returns>
        public string Trans()
        {
            //double s = denominator / bainumerator;
            //double dec = (double)(int)s;
            //if (s - dec !=0)
            //{
            //    return GetNumString(dec) + "." + GetNumString(s - dec).Substring(2);
            //}else
            //{
            //    return GetNumString(dec);
            //}

            return numOrig.ToString();

        }









        //获取最大公约数
        static double GetLargestCommonDivisor(double n1, double n2)
        {
            double max = n1 > n2 ? n1 : n2;
            double min = n1 < n2 ? n1 : n2;
            double remainder;
            while (min != 0)
            {
                remainder = max % min;
                max = min;
                min = remainder;
            }
            return max;
        }

        //获取最小公约数
        static double GetLeastCommonMutiple(double n1, double n2)
        {
            return n1 * n2 / GetLargestCommonDivisor(n1, n2);
        }
    }
   
  
}
