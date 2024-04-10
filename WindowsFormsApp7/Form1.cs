using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        int[] freqS = { 1225, 941, 819, 726, 710, 706, 685, 636, 457, 391, 383, 377, 334, 289, 258, 226, 171, 159, 158, 147, 109, 41, 21, 14, 9, 8 };
        int[] frq = { 819, 147, 383, 391, 1225, 226, 171, 457, 710, 14, 41, 377, 334, 706, 726, 289, 9, 685, 636, 941, 258, 109, 159, 21, 158, 8 };
        int[] orifrq =  {819, 147, 383, 391, 1225, 226, 171, 457, 710, 14, 41, 377, 334, 706, 726, 289, 9, 685, 636, 941, 258, 109, 159, 21, 158, 8 };
        int[] curfrq = new int[26];//e=1225,a=819
        Boolean cr1is2 = false;

        public Form1()
        {
            InitializeComponent();
        }

        private double onedigitL(double inp)
        {
            return Math.Floor(inp * 10) / 10;
        }
        private double onedigitH(double inp)
        {
            return Math.Ceiling(inp * 10) / 10;
        }

        private int[] sftrArr(int[] inp,int off)
        {
            int[] retarr = new int[inp.Length];
            for (int i = off; i < inp.Length; i++)
            {
                retarr[i - off] = inp[i];
            }
            for (int i = 0; i < off; i++)
            {
                retarr[i - off + retarr.Length] = inp[i];
            }
            return retarr;
        }

        private Boolean isEng(char inp)
        {
            return (inp >= 'a' && inp <= 'z' || inp >= 'A' && inp <= 'Z');
        }

        private void drawStd()
        {
            Series std = new Series();
            std.ChartType = SeriesChartType.Line;
            for (int i = 1; i <= 26; i++)
            {
                std.Points.AddXY(i, frq[i - 1]/100);
            }
            chart2.Series.Add(std);
            return;
        }

        private int[] getFreqS(int itvl)
        {
            int[] arr=new int[26];
            int tot = 0;
            byte cont = 0;
            foreach (var item in richTextBox1.Text)
            {
                if (!isEng(item))
                    continue;
                if (cont == 0)
                {
                    tot++;
                    arr[Char.ToLower(item) - 'a']++;
                }
                cont++;
                if (cont == itvl)
                    cont = 0;
            }
            for (int i = 0; i < 26; i++)
            {
                arr[i] *= 10000;
                arr[i] /= tot;
            }
            Array.Sort(arr); Array.Reverse(arr);
            return arr;
        }

        private int[] getFrq(int itvl,int off)
        {
            int[] arr = new int[26];
            int tot = 0;
            for (int i = off; i < richTextBox1.Text.Length; i+=itvl)
            {
                char item = richTextBox1.Text[i];
                if (!isEng(item))
                    continue;
                    tot++;
                    arr[Char.ToLower(item) - 'a']++;
            }
            for (int i = 0; i < 26; i++)
            {
                arr[i] *= 10000;
                arr[i] /= tot;
            }
            return arr;
        }

        private int intarrDot(int[] a,int[] b)
        {
            double []c=new double[26];
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > 6)
                    c[i] = Math.Sqrt(a[i] - 6) * 6 + 0.1 * a[i] - 14;
                else if (a[i] <= 0)
                    c[i] = a[i];
            }
            int fin=0;
            int grlen = a.Length > b.Length ? a.Length : b.Length;
            for (int i = 0; i < grlen; i++)
            {
                fin += (int)(c[i] * b[i]);
            }
            return fin;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Array.Sort(frq);
            String t="";
            for (int i = 0; i < 26; i++)
            {
                t += frq[i].ToString() +' '+ freqS[25-i].ToString()+'\n';
            }
            MessageBox.Show(t);
            */
            if(richTextBox1.Text.Length<40)
            {
                MessageBox.Show("Short Text");
                //return;
            }
            cr1is2 = false;
            if (numericUpDown1.Value > numericUpDown2.Value)
            {
                decimal tmp = 0;
                tmp =numericUpDown1.Value;
                numericUpDown1.Value = numericUpDown2.Value;
                numericUpDown2.Value = tmp;
            }

            chart1.Series.Clear();

            int[] f = new int[(int)(1+numericUpDown2.Value-numericUpDown1.Value)];

            foreach (var item in textBox4.Text)
            {
                if (Char.IsLetter(item))
                {
                        frq[(int)item - (int)'a'] = 0;
                }
            }
            freqS = (int[])frq.Clone();
            Array.Sort(freqS);
            Array.Reverse(freqS);

            for (int i =(int) numericUpDown1.Value; i <= numericUpDown2.Value; i++)
            {
                f[i-(int)numericUpDown1.Value]=intarrDot(freqS, getFreqS(i));
            }
            Series srs = new Series();
            double min = 10, max = 0;
            srs.ChartType = SeriesChartType.Line;
            for (int i = 0; i < f.Length; i++)
            {
                if (f[i] / 10000000d > max)
                    max = f[i] / 10000000d;
                if (f[i] / 10000000d < min)
                    min = f[i] / 10000000d;

                srs.Points.AddXY(i+numericUpDown1.Value,f[i]/10000000d);
            }
            int lklhod = 0,mstloc = 0;
            for (int i = 1; i < f.Length-1; i++)
            {
                if(2 * f[i] - f[i - 1] - f[i + 1] > lklhod)
                {
                    lklhod = 2 * f[i] - f[i - 1] - f[i + 1];
                    lklhod *= 1+2/(i+1);//penalty
                    mstloc = i;
                }
            }
            if(2*f[0]-2*f[1]>lklhod)
            {
                lklhod = 2 * f[0] - 2 * f[1];
                mstloc = 0;
            }
            if(2*f[f.Length-1]-2*f[f.Length-2]>lklhod)
            {
                lklhod = 2 * f[f.Length - 1] - 2 * f[f.Length - 2];
                mstloc = f.Length - 1;
            }
            numericUpDown3.Value = mstloc+numericUpDown1.Value;
            chart1.ChartAreas[0].AxisX.Minimum =(int) numericUpDown1.Value;
            chart1.ChartAreas[0].AxisX.Maximum = (int)numericUpDown2.Value;
            if (!(onedigitL((3 * min - max) / 2) < 0))
            {
                chart1.ChartAreas[0].AxisY.Minimum = onedigitL((3 * min - max) / 2);
                chart1.ChartAreas[0].AxisY.Maximum = onedigitH((-min + 3 * max) / 2);
            }
            else
            {
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = max;
            }
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            srs.Color = Color.Blue;
            chart1.Series.Add(srs);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chart2.ChartAreas[0].AxisY.Minimum = 1;
            chart2.ChartAreas[0].AxisY.Maximum = 26;
            drawStd();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label1.Text = hScrollBar1.Value + "-" + (char)(hScrollBar1.Value + 'A');
            numericUpDown5.Value = hScrollBar1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (numericUpDown4.Value != numericUpDown4.Maximum)
            {
                textBox1.Text += label1.Text.Substring(label1.Text.Length - 1);
                numericUpDown4.Value++;
                cr1is2 = false;
                button2_Click(0, new EventArgs());
            }
            else
            {
                textBox1.Text += label1.Text.Substring(label1.Text.Length - 1);
                MessageBox.Show("Already reached the end of the key!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length < 40)
            {
                //MessageBox.Show("Short Text");
                //return;
            }


            foreach (var item in textBox4.Text)
            {
                if (Char.IsLetter(item))
                {
                    if (checkBox1.Checked)
                        frq[(int)item - (int)'a'] = -100000;
                    else
                        frq[(int)item - (int)'a'] = 0;
                }
            }
            freqS = (int[])frq.Clone();
            Array.Sort(freqS);
            Array.Reverse(freqS);


            chart2.Series.Clear();
            drawStd();
            curfrq=getFrq((int)numericUpDown3.Value,(int)numericUpDown4.Value-1);
            Series ser = new Series();
            ser.ChartType = SeriesChartType.Line;
            for (int i =(int) numericUpDown5.Value; i < 26; i++)
            {
                ser.Points.AddXY(i- (int)numericUpDown5.Value+1, curfrq[i]/100);
            }
            for (int i = 0; i < (int)numericUpDown5.Value; i++)
            {
                ser.Points.AddXY(i - (int)numericUpDown5.Value +26+ 1, curfrq[i] / 100);
            }
            chart2.Series.Add(ser);

            if(!cr1is2)
            {
                chart1.Series.Clear();
                Series ser1 = new Series();
                ser1.ChartType = SeriesChartType.Line;
                double max = 0,tmp=0,min=1000;
                int loc = 0;
                for (int i = 0; i < 26; i++)
                {
                    tmp = intarrDot(frq, sftrArr(curfrq, i)) / 100000;
                    ser1.Points.AddXY(i+1,tmp);
                    if (tmp > max)
                    {
                        max = tmp;
                        loc= i;
                    }
                    else if(tmp<min)
                    {
                        min = tmp;
                    }
                }
                hScrollBar1.Value = loc;
                hScrollBar1_Scroll(0,new ScrollEventArgs(ScrollEventType.ThumbPosition,loc));

                chart1.ChartAreas[0].AxisX.Minimum = 1;
                chart1.ChartAreas[0].AxisX.Maximum = 26;
                if (!(onedigitL((3 * min - max) / 2) < 0))
                {
                    chart1.ChartAreas[0].AxisY.Minimum = onedigitL((3 * min - max) / 2);
                    chart1.ChartAreas[0].AxisY.Maximum = onedigitH((-min + 3 * max) / 2);
                }
                else
                {
                    chart1.ChartAreas[0].AxisY.Minimum = 0;
                    chart1.ChartAreas[0].AxisY.Maximum = max;
                }
                    chart1.Series.Add(ser1);
            }
            cr1is2 = true;


            string deced;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            hScrollBar1.Value =(int) numericUpDown5.Value;
            label1.Text = hScrollBar1.Value + "-" + (char)(hScrollBar1.Value + 'A');
            button2_Click(0,new EventArgs());
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown4.Maximum = numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            cr1is2 = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder tmp=new StringBuilder();
            Char tm;
            foreach (var item in richTextBox1.Text)
            {
                tm = Char.ToLower(item);
                if (tm <= 'z' && tm >= 'a')
                    tmp.Append(tm);
            }
            richTextBox1.Text = tmp.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            frq = (int[])orifrq.Clone();
            freqS = (int[])orifrq.Clone();
            Array.Sort(freqS);
            Array.Reverse(freqS);
        }
    }
}
