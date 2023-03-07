//作者；@我不是大佬
// wx；safesea
// qq;2869210303
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Five_Points_Chess
{
    public partial class Form1 : Form
    {
        // 棋盘中，x表示用户的落子，o表示电脑的落子，_表示空点位

        private Graphics my_graphics;   //画布
        private int qipansize = 15;     //默认棋盘15X15
        private int gezisize = 40;      //每隔格子的大小
        private Brush whitebush = new SolidBrush(Color.White);//填充的颜色，白色
        private Brush blackbush = new SolidBrush(Color.Black);//黑色
        private bool start = false; //是否开始了游戏
        private bool userturn = false;//是否是用户落子

        private string[] trysigezifuch;     //
        private string[] userfourzifuch;
        private Matrix tryqipan = new Matrix();     //电脑的尝试
        private Matrix usertry = new Matrix();       //用户的尝试
        private int[,] o_o_scores = new int[15, 15];//在假设该点被电脑占领的情况下，电脑对应的分数
        private int[,] o_x_scores = new int[15, 15];//在假设该点被电脑占领的情况下，用户对应的分数
        private int[,] x_o_scores = new int[15, 15];//在假设该点被用户占领的情况下，电脑对应的分数
        private int[,] x_x_scores = new int[15, 15];//在假设该点被用户占领的情况下，用户对应的分数
        private int[] temppoint = new int[2];// 用户提示当前计算机的骡子点
        private Brush redbrush = new SolidBrush(Color.Red);
        private Matrix qipancontent = new Matrix();     //记录棋盘上的旗子分布
        //是否为第一回合，是则调用经典开局
        private int firsthuihe = 1;
        public Form1()
        {
            //设置窗体的启动位置，剧中显示
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        //绘制坐标轴
        private void draw_xy()
        {
            Pen my_pen = new Pen(Color.Black, 1);
            for (int j = 1; j <= qipansize; j++)
            {
                my_graphics.DrawLine(my_pen, new Point(gezisize, gezisize * j), new Point(gezisize * qipansize, gezisize * j));
                my_graphics.DrawLine(my_pen, new Point(gezisize * j, gezisize), new Point(gezisize * j, gezisize * qipansize));
            }
            //绘制五个定位点
            int radus = 12;
            my_graphics.FillEllipse(blackbush, 8 * gezisize - 6, 8 * gezisize - 6, radus, radus);
            my_graphics.FillEllipse(blackbush, 4 * gezisize - 6, 4 * gezisize - 6, radus, radus);
            my_graphics.FillEllipse(blackbush, 12 * gezisize - 6, 4 * gezisize - 6, radus, radus);
            my_graphics.FillEllipse(blackbush, 8 * gezisize - 6, 8 * gezisize - 6, radus, radus);
            my_graphics.FillEllipse(blackbush, 4 * gezisize - 6, 12 * gezisize - 6, radus, radus);
            my_graphics.FillEllipse(blackbush, 12 * gezisize - 6, 12 * gezisize - 6, radus, radus);
            //my_graphics.FillRectangle(new SolidBrush(Color.FromArgb(55,228, 185, 135)), 0, 0, 650,this.Height);                        
            // 制作图例
            radus = gezisize - 5;
            my_graphics.FillEllipse(blackbush, 18 * gezisize - gezisize / 2, 12 * gezisize - gezisize / 2, radus, radus);
            my_graphics.FillEllipse(whitebush, 18 * gezisize - gezisize / 2, 13 * gezisize - gezisize / 2, radus, radus);
            my_graphics.FillEllipse(redbrush, 18 * gezisize - gezisize / 2, 14 * gezisize - gezisize / 2, radus, radus);
            //选择字体、字号、风格
            Font font = new Font("Adobe Gothic Std", 15f, FontStyle.Bold);
            //在位置（150，200）处绘制文字
            my_graphics.DrawString("计算机落子", font, blackbush, 750, 465);
            my_graphics.DrawString("玩家落子", font, blackbush, 750, 510);
            my_graphics.DrawString("计算机上一回合落子", font, blackbush, 750, 555);
        }

        // 加载窗体
        private void Form1_Load(object sender, EventArgs e)
        {
            my_graphics = this.CreateGraphics();
            this.Width = 1000;
            this.Height = 800;

            //初始化棋盘
            qipancontent.getAllSssss();

        }
        // 窗体的打印事件
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            draw_xy();
        }

        //将鼠标的坐标转化为对应的点阵坐标
        private int[] transformzuobiao(Point mlnpoint)
        {
            int x = mlnpoint.X / gezisize;
            int y = mlnpoint.Y / gezisize;
            int newX = mlnpoint.X % gezisize;   //余数
            int newY = mlnpoint.Y % gezisize;   //余数

            if (newX > gezisize / 2)
            {
                x++;
            }
            if (newY > gezisize / 2)
            {
                y++;
            }
            return new int[] { x, y };
        }
        // 绘制棋子的函数
        private void draw_qizzi(Brush b, int[] p)
        {
            int radus = gezisize - 5;
            my_graphics.FillEllipse(b, p[0] * gezisize - gezisize / 2, p[1] * gezisize - gezisize / 2, radus, radus);
        }

        // 鼠标点击事件，用户落子
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (start)
            {
                if (userturn)
                {
                    Point mlnpoint = e.Location;
                    int[] pzuobiao = transformzuobiao(mlnpoint);       //对应矩阵的位置

                    // 测试内容
                    string str = "鼠标点击位置；" + e.Location.ToString();

                    // 判断是否达到了落子的条件
                    if (pzuobiao[0] <= 0 || pzuobiao[1] <= 0 || pzuobiao[0] > qipansize || pzuobiao[1] > qipansize)
                    {
                        str += "    鼠标位置超出了棋盘的范围，无法落子，请重试";
                        this.Text = str;
                        return;     // 终止函数，什么事也不做
                    }

                    if (qipancontent[pzuobiao[0], pzuobiao[1]] != '_')
                    {
                        this.Text = "请在空的点位落子";
                        return;
                    }

                    str += "     当前落子位置；(" + pzuobiao[0].ToString() + "\t,\t" + pzuobiao[1].ToString() + ")";
                    this.Text = str;
                    //落子
                    draw_qizzi(whitebush, pzuobiao);
                    qipancontent[pzuobiao[0], pzuobiao[1]] = 'x';
                    //更新棋盘；用户和电脑的分数，棋盘的72条序列串
                    qipancontent.updata(pzuobiao[0], pzuobiao[1]);

                    //判断是否结束了游戏
                    string[] sigefuzich = qipancontent.getFourStrings(pzuobiao);
                    if (checkWin(sigefuzich, 'x'))
                    {
                        MessageBox.Show("恭喜，你赢了", "温馨提示");
                        start = false;
                        return;
                    }

                    userturn = false;

                    //判断是否为第一回合，否则调用经典开局
                    if (firsthuihe == 1)
                    {
                        try
                        {
                            computers_turn(pzuobiao[0] + 1, pzuobiao[1] + 1);
                            temppoint = new int[] { pzuobiao[0] + 1, pzuobiao[1] + 1 };
                            firsthuihe++;
                            return;
                        }
                        catch { }

                    }

                    //电脑落子
                    Computer_operat();
                }
                else
                {
                    this.Text = "现在是电脑的落子时间，请稍后...";
                    return;
                }
            }
            else
            {
                this.Text = "点击按钮开始游戏...";
                return;
            }
        }
        private string[] firstcare = new string[] {
            "_xxxxo,"
        };

        /// 判断某一方是否赢了
        /// <param name="x">检查的字符</param>
        /// <returns>赢了返回true，否则返回false </returns>
        private bool checkWin(string[] sigezifuch, char x)
        {
            string xxxxx;
            if (x == 'x')
            {
                xxxxx = "xxxxx";
            }
            else if (x == 'o')
            {
                xxxxx = "ooooo";
            }
            else
            {
                return false;       //部分和条件，无法查找
            }
            //开始判断
            foreach (string n in sigezifuch)
            {
                if (n.Contains(xxxxx))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个字符串数组中每隔字符串是否包含指定的字符串，如果有一个包含则返回true
        /// </summary>
        /// <param name="content">字符串数组</param>
        /// <param name="value">需要查找的字符串</param>
        private bool isInAtrings(string[] content, string value)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i].Contains(value))
                {
                    return true;
                }
            }
            return false;
        }


        /// 电脑落子
        private void Computer_operat()
        {
            ///简单算法的实现步骤
            ///(0)，建立评分函数，对当前电脑和用户的棋局打分
            ///(1)，判断如果这个点被计算机占领，是否形成五子连珠的必胜局面
            ///(2)，判断如果该点被计算机占领，是否阻断了用户的五子连珠(对方已经四子连珠)，此时必须防守
            ///(3)，判断如果计算机占领该店后，是否可以使得对手必须做出防守的动作
            ///(4)，如果以上的基础情况不满足，计算计算机占领每个点后电脑和用户对应的分数，储存在二维数组中
            ///(5)，假设这个点被用户占领，计算用户对应每个点的分数，储存在二维数组中
            ///(6)，获取用户分数数组的最大值，如果最大值超过需要防守的阈值，则提前占领该店
            ///(7)，选取自己分数最大的点，同时用户分数最小的点，作为落子点.


            //测试内容
            int r1 = 1;             // 储存最佳点位的x坐标，取值范围：[1,15]
            int r2 = 15;            // 储存最佳点位的y坐标，取值范围：[1,15]

            //判断自己是否可以五子连珠,获取这个点被电脑占领的情况下用户和电脑的棋盘分数；
            for (int i = 1; i <= 15; i++)
            {
                for (int j = 1; j <= 15; j++)
                {
                    if (qipancontent[i, j] == '_')
                    {
                        // 尝试将这个点变成自己的点后观察变化，是否出现了 必胜 （五子连珠）的局面
                        tryqipan.copyMatrix(qipancontent, i, j, 'o');
                        int[] position = new int[] { i, j };
                        trysigezifuch = tryqipan.getFourStrings(position);
                        if (isInAtrings(trysigezifuch, "ooooo"))
                        {
                            //必胜
                            r1 = i;
                            r2 = j;
                            goto HaveFoundBestZuobiao;
                        }
                        // 假设这个点被用户占领
                        usertry.copyMatrix(qipancontent, i, j, 'x');
                        userfourzifuch = usertry.getFourStrings(position);
                        //获取这个点被电脑占领的情况下用户和电脑的棋盘分数；
                        string[] tempstrings = qipancontent.getFourStrings(position);
                        int temp_o_o_score = 0;
                        int temp_o_x_score = 0;
                        int temp_x_x_score = 0;
                        int tempint = 0;
                        for (int m = 0; m < 4; m++)
                        {
                            ///由于每次落子时候值改变了四条序列串，只需要计算这四条序列串的分数变化量
                            temp_o_o_score -= qipancontent.getEverySssScore(tempstrings[m], true);
                            //临时变量
                            tempint = qipancontent.getEverySssScore(tempstrings[m], false);
                            temp_o_x_score -= tempint;
                            temp_x_x_score -= tempint;
                            temp_o_o_score += qipancontent.getEverySssScore(trysigezifuch[m], true);
                            temp_o_x_score += qipancontent.getEverySssScore(trysigezifuch[m], false); ;
                            temp_x_x_score += qipancontent.getEverySssScore(userfourzifuch[m], false);
                        }
                        o_o_scores[i - 1, j - 1] = qipancontent.oscore + temp_o_o_score;
                        o_x_scores[i - 1, j - 1] = qipancontent.xscore + temp_o_x_score;
                        x_x_scores[i - 1, j - 1] = qipancontent.xscore + temp_x_x_score;
                    }
                    else
                    {
                        o_o_scores[i - 1, j - 1] = -1;
                        o_x_scores[i - 1, j - 1] = -1;
                        x_x_scores[i - 1, j - 1] = -1;
                        x_o_scores[i - 1, j - 1] = -1;
                    }
                }
            }

            //接下来就是分析最佳点位
            //获取最大值
            int[] xtemp = Matrix.GetMaxElement(x_x_scores);
            int[] temp = Matrix.GetMaxElement(o_o_scores);

            if (xtemp[0] >= 650000)
            {
                //判断是否要阻断对手的五子连珠
                r1 = xtemp[1] + 1;
                r2 = xtemp[2] + 1;
                goto HaveFoundBestZuobiao;
            }
            else
            {
                //自己是否可以让对手必须防守
                if (temp[0] >= 60000)
                {
                    r1 = temp[1] + 1;
                    r2 = temp[2] + 1;
                    goto HaveFoundBestZuobiao;
                }
                else
                {
                    //对手的分数超过阈值，进行防守
                    if (xtemp[0] >= 70000)
                    {

                        r1 = xtemp[1] + 1;
                        r2 = xtemp[2] + 1;
                        goto HaveFoundBestZuobiao;
                    }
                    else
                    {
                        //如果有很多个最大值，选取对自己最好，对别人最差的点
                        if (temp[3] > 1)
                        {
                            int min = 999999;
                            for (int i = 0; i < 15; ++i)
                            {
                                for (int b = 0; b < 15; ++b)
                                {
                                    if (o_o_scores[i, b] == temp[0])
                                    {
                                        if ((min > o_x_scores[i, b]))
                                        {
                                            min = o_x_scores[i, b];
                                            r1 = i + 1;
                                            r2 = b + 1;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            r1 = temp[1] + 1;
                            r2 = temp[2] + 1;
                        }
                    }
                }
            }

        //已经找到了最优位置 
        HaveFoundBestZuobiao:
            //将上一回合计算机落子的位置颜色更正
            draw_qizzi(blackbush, temppoint);
            temppoint = new int[] { r1, r2 };
            computers_turn(r1, r2);
        }

        /// <summary>
        /// 判断这个点的周围 是否有用户的旗子x存在
        /// </summary>
        /// <param name="i">取值范围；[1,15]</param>
        /// <param name="">取值范围；[1,15]</param>
        /// <returns></returns>
        private bool isXAround(int i, int j)
        {
            if (
                (qipancontent[i - 1, j - 1] == 'x') ||
                (qipancontent[i - 1, j + 1] == 'x') ||
                (qipancontent[i + 1, j - 1] == 'x') ||
                (qipancontent[i + 1, j - 1] == 'x')
               )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 回去最佳点位后，电脑落子
        /// </summary>
        /// <param name="r1">r1;[1,15]</param>
        /// <param name="r2">r2;[1,15]</par0am>
        private void computers_turn(int r1, int r2)
        {
            //以上需要经过算法得出最佳的点位；[r1,r2];
            int[] computerlocation = new int[] { r1, r2 };

            //更新棋盘；用户和电脑的分数，棋盘的72条序列串
            qipancontent[r1, r2] = 'o';
            qipancontent.updata(r1, r2);
            //绘制旗子
            draw_qizzi(redbrush, computerlocation);
            userturn = true;
            //判断电脑是否赢了
            string[] sigezifuch = qipancontent.getFourStrings(computerlocation);
            if (checkWin(sigezifuch, 'o'))
            {
                //输出游戏结束
                MessageBox.Show("你也太菜了吧，连电脑都玩不赢！", "温馨提示");
                return;
            }
            this.Text = "电脑此回合的落子位置；( " + r1.ToString() + " , " + r2.ToString() + " )";
            //输出当前电脑和用户的分数
            string msg = "电脑;" + qipancontent.oscore.ToString() + "\n用户；" + qipancontent.xscore.ToString() + "\n差值；" + (qipancontent.oscore - qipancontent.xscore).ToString();
            richTextBox1.Text = msg;
        }


        //开始游戏
        private void button1_Click(object sender, EventArgs e)
        {
            start = true;
            userturn = true;
            button1.Enabled = false;
        }
        //重新开始游戏
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //退出程序
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        /// 将字符串数组输出为字符串
        private string toString(string[] a)
        {
            string text = "";
            foreach (string s in a)
            {
                text += s + "\n";
            }
            return text;
        }
        //将以为int数组输出为字符串
        private string toString(int[] a)
        {
            string text = "";
            foreach (int s in a)
            {
                text += s.ToString() + ',';
            }
            return text;
        }
        //将二维int数组输出为字符串
        private string toString(int[,] a)
        {
            string text = "";
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                {
                    text += a[i, j].ToString() + ',';
                }
                text += "\n";
            }
            return text;
        }
    }
}
