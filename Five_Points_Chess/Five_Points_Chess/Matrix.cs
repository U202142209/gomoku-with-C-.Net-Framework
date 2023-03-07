//作者；@我不是大佬
// wx；safesea
// qq;2869210303
// using System;
using System.Collections.Generic;
using System.Linq;

namespace Five_Points_Chess
{
    class Matrix
    {
        ///棋盘的点阵类
        ///使用_表示空的点位，o表示己方点位，x表示对方点位
        private char[,] elems = new char[15, 15];
        //记录o的分值；字典类型
        private Dictionary<string, int> oDictory = new Dictionary<string, int>();
        //记录x的分值；字典类型
        private Dictionary<string, int> xDictory = new Dictionary<string, int>();
        //棋盘的 72 条序列串；字符串数组
        private string[] sssss = new string[72];
        public int oscore = 0; //整个棋盘中，o的分数，电脑，默认为0
        public int xscore = 0; //整个棋盘中，x的分数，用户，默认为0

        /// <summary>
        /// //初始化 构造方法，将点阵的每个点标记为；_，表示空点，同时；给出x和o的评分表
        /// </summary>
        public Matrix()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    this.elems[i, j] = '_';
                }
            }

            oDictory.Add("ooooo", 999999);          // 获胜
            oDictory.Add("_oooo_", 300000);         // 必胜
            oDictory.Add("_oooox", 61000);          // 对方必须防守
            oDictory.Add("xoooo_", 61000);          // 对方必须防守
            oDictory.Add("_ooo_o_", 60000);         // 对方必须防守
            oDictory.Add("_o_ooo_", 60000);         // 对方必须防守
            oDictory.Add("_oo_oo_", 60000);         // 对方必须防守
            oDictory.Add("xooo_o", 60000);          // 对方必须防守
            oDictory.Add("xo_ooo", 60000);          // 对方必须防守
            oDictory.Add("xoo_oo", 60000);          // 对方必须防守
            oDictory.Add("ooo_ox", 60000);          // 对方必须防守
            oDictory.Add("o_ooox", 60000);          // 对方必须防守
            oDictory.Add("oo_oox", 60000);          // 对方必须防守
            oDictory.Add("__ooo_", 10200);          // 对方需要防守
            oDictory.Add("_ooo__", 10200);          // 对方需要防守
            oDictory.Add("_oo_o_", 10000);          // 对方需要防守
            oDictory.Add("_o_oo_", 10000);          // 对方需要防守
            oDictory.Add("_o_o_o_", 300);
            oDictory.Add("__oo_", 150);
            oDictory.Add("_oo__", 150);
            oDictory.Add("xoo_o_", 100);
            oDictory.Add("xo_oo_", 100);
            oDictory.Add("_oo_ox", 100);
            oDictory.Add("_o_oox", 100);
            oDictory.Add("xo_o_o_", 100);
            oDictory.Add("_o_o_x", 100);
            oDictory.Add("__ooox", 50);
            oDictory.Add("xooo__", 50);
            oDictory.Add("_o_o_", 30);
            oDictory.Add("_o__o_", 20);
            oDictory.Add("__o__", 10);


            xDictory.Add("xxxxx", 999999);          // 获胜
            xDictory.Add("_xxxx_", 300000);         // 必胜
            xDictory.Add("_xxxxo", 61000);          // 对方必须防守
            xDictory.Add("oxxxx_", 61000);          // 对方必须防守
            xDictory.Add("_xxx_x_", 60000);         // 对方必须防守
            xDictory.Add("_x_xxx_", 60000);         // 对方必须防守
            xDictory.Add("_xx_xx_", 60000);         // 对方必须防守
            xDictory.Add("oxxx_x", 60000);          // 对方必须防守
            xDictory.Add("ox_xxx", 60000);          // 对方必须防守
            xDictory.Add("oxx_xx", 60000);          // 对方必须防守
            xDictory.Add("xxx_xo", 60000);          // 对方必须防守
            xDictory.Add("x_xxxo", 60000);          // 对方必须防守
            xDictory.Add("xx_xxo", 60000);          // 对方必须防守
            xDictory.Add("__xxx_", 10200);          // 对方需要防守
            xDictory.Add("_xxx__", 10200);          // 对方需要防守
            xDictory.Add("_xx_x_", 10000);          // 对方需要防守
            xDictory.Add("_x_xx_", 10000);          // 对方需要防守
            xDictory.Add("_x_x_x_", 300);
            xDictory.Add("__xx_", 150);
            xDictory.Add("_xx__", 150);
            xDictory.Add("oxx_x_", 100);
            xDictory.Add("ox_xx_", 100);
            xDictory.Add("_xx_xo", 100);
            xDictory.Add("_x_xxo", 100);
            xDictory.Add("ox_x_x_", 100);
            xDictory.Add("_x_x_o", 100);
            xDictory.Add("__xxxo", 50);
            xDictory.Add("oxxx__", 50);
            xDictory.Add("_x_x_", 30);
            xDictory.Add("_x__x_", 20);
            xDictory.Add("__x__", 10);
        }

        //拷贝点阵
        /// <summary>
        /// 尝试修改指定坐标的值得到新的矩阵
        /// </summary>
        /// <param name="m"> 需要被复制的点阵</param>
        /// <param name="xzuobiao">需要修改的 x 坐标，[1,15]</param>
        /// <param name="yzuobiao">需要修改的 y 坐标，[1,15]</param>
        /// <param name="value"> 需要的值</param>
        public void copyMatrix(Matrix m, int xzuobiao, int yzuobiao, char value)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    this.elems[i, j] = m.elems[i, j];
                }
            }
            this.elems[xzuobiao - 1, yzuobiao - 1] = value;
        }


        // 此方法的作用是实现通过类似于a[1,2]=3的方式访问和赋值
        public char this[int i, int j]
        {
            get { return this.elems[i - 1, j - 1]; }
            set { this.elems[i - 1, j - 1] = value; }
        }

        /// <summary>
        /// 获取72条序列串，按照；||  --  \\  //  的顺序
        /// 对于\\ ;首先获取了右上角的\（11），再获取了左下角的 \ （10）
        /// 对于//；首先获取了左上角的\（11），再获取了右下角的 \ （10）
        /// 在72条序列串中；
        /// [0:14]; ||   。从左到右的排列顺序
        /// [15:29];--   。按照从上到下的排列顺序
        /// [30:40];右上角的\   。从左到右的排列顺序
        /// [41:50];左下角的\   。从上到下的排列顺序
        /// [51:61];左上角的/   。从左到右的排列顺序
        /// [62:71];右下角的/   。从下到上的排列顺序
        /// 所以，在计算机选择落点的过程中，西药将所有的点都遍历一次，同时计算分数，为了比秒重复计算
        /// 可以采用如下的算法；对于没一点[i.j];(1<=i,j<=15)，需要获取棋盘的72条序列串中，属于[i,j]的四条序列串
        /// 计算分数时，只需要计算原来的72条序列串中属于[i,j]的四条的分数s1，在计算穿过[i,j]的新的四条序列串的分数s2
        /// 则该点的相对于o的分数就等于；oscore - s1 + s2 ;
        /// </summary>
        public void getAllSssss()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    this.sssss[i] += this.elems[i, j];  //竖线 |
                    this.sssss[i + 15] += this.elems[j, i]; //横线 ——
                }
            }
            // 斜线 \ 方向
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 15 - i; j++)
                {
                    this.sssss[30 + i] += this.elems[i + j, j];
                }
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = i; j < 15; j++)
                {
                    this.sssss[30 + 11 + i - 1] += this.elems[j - i, j];
                }
            }

            // 斜线 / 方向
            for (int i = 4; i < 15; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    this.sssss[30 + 21 + i - 4] += this.elems[i - j, j];
                }
            }
            for (int i = 4; i < 14; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    this.sssss[30 + 21 + 11 + i - 4] += this.elems[14 - j, 14 - i + j];
                }
            }
        }


        /// <summary>
        /// 获取72条序列串中属于[i,j] （1<= i,j <= 15）的四条序列串的索引；按照；||  --  \\  //  的顺序
        /// </summary>
        /// <param name="i">x的坐标；[1,15]</param>
        /// <param name="j">y的坐标；[1,15]</param>
        /// <returns>序列串索引 ；按照；||  --  \\  //  的顺序</returns>
        private int[] get_point_four_sss(int i, int j)
        {
            int[] result = new int[4];
            result[0] = i - 1;
            result[1] = j - 1 + 15;
            if (i >= j)
            {
                //左上角的\\
                result[2] = i - j + 30;
            }
            else
            {
                //右下角的\\
                result[2] = j - i + 40;
            }
            if (i + j <= 16)
            {
                //左上角的 //
                result[3] = i + j + 45;
            }
            else
            {
                //右下角的
                result[3] = 88 - i - j;
            }
            return result;
        }

        /// <summary>
        /// 获取一个点对用的四条字符串，按照；||  --  \\  //  的顺序
        /// </summary>
        /// <param name="location">点的坐标，[1,15]的范围 </param>
        /// <returns> 字符串数组 </returns>
        public string[] getFourStrings(int[] location)
        {
            string[] sigezifuch = new string[4];
            int lx = location[0];
            int ly = location[1];
            for (int i = 1; i <= 15; i++)
            {
                sigezifuch[0] += this[lx, i];   //  ||
                sigezifuch[1] += this[i, ly];   //  --
                
            }

            if (lx >= ly)       // \\ 
            {
                for (int i = 1; i <= 15 - lx + ly; i++)
                {
                    sigezifuch[2] += this[i + lx - ly, i];  //形状；\
                }
            }
            else
            {
                for (int i = 1 - lx + ly; i <= 15; i++)
                {
                    sigezifuch[2] += this[i + lx - ly, i];  //形状；\

                }
            }
            if (lx + ly >= 16)  // //
            {
                for (int i = lx + ly - 15; i <= 15; i++)
                {
                    sigezifuch[3] += this[lx + ly - i, i];  //形状；/
                }
            }
            else
            {
                for (int i = 1; i <= lx + ly - 1; i++)
                {
                    sigezifuch[3] += this[i, lx + ly - i];  //形状；/
                }
            }
            return sigezifuch;
        }


        /// </summary> 获取每隔序列串的分数
        /// <param name="a">需要寻找的目标字符串，可选择的范围；o 或者 x</param>
        /// <param name="searchmyself">是否查找自己的 o </param>
        /// <returns>返回这条序列串关于o或者x的分数，int类型</returns>
        public int getEverySssScore(string a, bool searchmyself)
        {
            if (searchmyself)
            {
                if (a.Contains('o'))
                {
                    int mysum = 0;
                    foreach (KeyValuePair<string, int> kvp in oDictory)
                    {
                        //遍历字典，逐个查找，返回累加值
                        mysum += SubstringCount(a, kvp.Key) * kvp.Value;
                    }
                    return mysum;
                }
                // 如果没有自己的旗子，直接返回最小值0
                return 0;
            }
            else
            {
                if (a.Contains('x'))
                {
                    int yoursum = 0;
                    foreach (KeyValuePair<string, int> kvp in xDictory)
                    {
                        //遍历字典，逐个查找，返回累加值
                        yoursum += SubstringCount(a, kvp.Key) * kvp.Value;
                    }
                    return yoursum;
                }
                // 如果没有自己的旗子，直接返回最小值0
                return 0;
            }

        }

        /// <summary>
        /// 在电脑或者用户每次落子之后，更新当前局面下用户和电脑的分数，同时更新72条序列串
        /// </summary>
        /// <param name="i">用户或者电脑的骡子点，取值为；[1,15]</param>
        /// <param name="j">用户或者电脑的骡子点，取值为；[1,15]</param>
        public void updata(int i,int j)
        {
            int[] tempint = this.get_point_four_sss(i,j);
            string[] tempstrings = this.getFourStrings(new int[2] { i, j });
            for (int m = 0; m < 4; m++)
            {
                ///由于每次落子时候值改变了四条序列串，只需要计算这四条序列串的分数变化量
                oscore -= this.getEverySssScore(sssss[tempint[m]], true);
                xscore -= this.getEverySssScore(sssss[tempint[m]], false);
                this.sssss[tempint[m]] = tempstrings[m];
                oscore += this.getEverySssScore(sssss[tempint[m]], true);
                xscore += this.getEverySssScore(sssss[tempint[m]], false);
            }
        }

        /// 计算字符串中子串出现的次数
        /// <param name="str">字符串 内容</param>
        /// <param name="substring">子串 关键字</param>
        /// <returns>出现的次数</returns>
        private int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }
            return 0;
        }

        /// <summary>
        /// 获取当前两方棋盘的分数，感觉这个函数的效率太低了，到时候的删除，不值得使用
        /// </summary>
        /// <returns> [自己的分数，对手的分数]  </returns>
        private int[] getTotalScore()
        {
            // 重新计算棋盘的布局
            getAllSssss();
            int mytotalscore = 0;
            int yourscore = 0;
            foreach (string a in this.sssss)
            {
                mytotalscore += getEverySssScore(a, true);
                yourscore += getEverySssScore(a, false);
            }
            int[] res = new int[] { mytotalscore, yourscore };
            return res;
        }

        /// <summary>感觉这个函数的效率太低了，到时候的删除，不值得使用
        /// 在大于零的范围内， 获取二维int[,]数组中的最大值，最小值及其坐标
        /// </summary>返回的坐标的取值范围是[0,14];
        /// <param name="gender">二维int数组 </param>
        /// <returns>[最大值,最大值x坐标,最大值y坐标,记录最大值出现的次数] </returns>
        public static int[] GetMaxElement(int[,] gender)
        {
            int max = gender[0, 0];//定义max容器装最大值
            int maxx = 0;
            int maxy = 0;
            int count = 0;
            for (int i = 0; i < gender.GetLength(0); ++i)//gender有几行
            {
                for (int b = 0; b < gender.GetLength(1); ++b)//gender有几列
                {
                    if (gender[i, b] > 0)
                    {
                        if (gender[i, b] > max)
                        {
                            max = gender[i, b];//比大的大则用它
                            maxx = i;
                            maxy = b;
                            count = 1;
                        }
                        if (gender[i,b] == max)
                        {
                            count++;
                        }
                    }
                }
            }

            int[] result = new int[4] {
                max,
                maxx,
                maxy,
                count,
            };
            return result;
        }
    }
}
