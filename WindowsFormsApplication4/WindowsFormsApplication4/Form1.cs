using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        Stack<string> s1 = new Stack<string>();
        Stack<string> s2 = new Stack<string>();
        Stack<string> s3 = new Stack<string>();

        Stack<string> i1 = new Stack<string>();
        Stack<string> i2 = new Stack<string>();
        Stack<string> i3 = new Stack<string>();

        int[,] sudoku = new int[9, 9];
        int[, ,] ihtimaller = new int[9, 9 , 9];
        int[, ,] sudoku2 = new int[3, 3, 9];

        int[,] sudoku_1 = new int[9, 9];
        int[, ,] ihtimaller_1 = new int[9, 9, 9];

        int[,] sudoku_2 = new int[9, 9];
        int[, ,] ihtimaller_2 = new int[9, 9, 9];

        int[,] sudoku_x = new int[9, 9];

        int[,] dizi = new int[3, 3];
        int change = 0;
        int hangiThread = 0;

        
        public void button01()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j <3; j++)
                {
                    dizi[i,j]=0;
                }
            }
            Stream myStream = null;
            openFileDialog1.Title = "Open Text File";
            openFileDialog1.Filter = "TXT Files|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string[] filelines = File.ReadAllLines(filename);
                for (int i = 0; i < filelines.Length; i++)
                {
                    for (int k = 0; k < filelines[i].Length; k++)
                    {
                        if (filelines[i][k] == '*')
                        {
                            sudoku[i, k] = 0;
                            sudoku_1[i, k] = 0;
                            sudoku_2[i, k] = 0;
                        }
                        else
                        {
                            sudoku[i, k] = Convert.ToInt32(filelines[i][k].ToString());
                            sudoku_1[i, k] = Convert.ToInt32(filelines[i][k].ToString());
                            sudoku_2[i, k] = Convert.ToInt32(filelines[i][k].ToString());
                        }
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sudoku2[(int)i / 3, (int)j / 3, dizi[(int)i / 3, (int)j / 3]] = sudoku[i, j];
                    dizi[(int)i / 3, (int)j / 3]++;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        ihtimaller[i, j, a] = 1;
                        ihtimaller_1[i, j, a] = 1;
                        ihtimaller_2[i, j, a] = 1;
                    }
                }
            }
            
            int x = 0, b = 0;
            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is TextBox && x < 9)
                {
                    if (sudoku[x, b] == 0)
                    {
                        kontrol.Text = null;
                    }
                    else
                    {
                        kontrol.Text = sudoku[x, b].ToString();
                    }
                    b++;
                    if (b % 9 == 0)
                    {
                        x++;
                        b = 0;
                    }
                }
            }
            
        }
        
        public void button02()
        {
            int s;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        s = sudoku[i, a];
                        if (s != 0)
                        {
                            ihtimaller[i, j, s - 1] = 0;
                            ihtimaller_1[i, j, s - 1] = 0;
                            ihtimaller_2[i, j, s - 1] = 0;
                        }
                            
                    }
                    for (int a = 0; a < 9; a++)
                    {
                        s = sudoku[a, j];
                        if (s != 0)
                        {
                            ihtimaller[i, j, s - 1] = 0;
                            ihtimaller_1[i, j, s - 1] = 0;
                            ihtimaller_2[i, j, s - 1] = 0;
                        }
                            
                    }

                    for (int a = 0; a < 9; a++)
                    {
                        s = sudoku2[(int)i / 3, (int)j / 3, a];
                        if (s != 0)
                        {
                            ihtimaller[i, j, s - 1] = 0;
                            ihtimaller_1[i, j, s - 1] = 0;
                            ihtimaller_2[i, j, s - 1] = 0;
                        }
                            
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] != 0)
                    {
                        for (int a = 0; a < 9; a++)
                        {
                            ihtimaller[i, j, a] = 0;
                            ihtimaller_1[i, j, a] = 0;
                            ihtimaller_2[i, j, a] = 0;
                        }

                    }

                }
            }
        }

        public void button03(int startX, int startY)
        {
            int sayac = 0, hold = 0, sayac2 = 0, sayac3 = 0;
            int[] varmi = new int[9];
            int[] varmiX = new int[9];
            int[] varmiY = new int[9];

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        if (ihtimaller[i, j, a] != 0)
                        {
                            hold = a + 1; //sudoku[] matrisine yazmak için
                            sayac++; //ihtimal bulundu

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller[i, k, a] == 1)
                                {
                                    sayac2++;
                                }
                            }
                            if (sayac2 == 1)
                            {
                                if (sudoku[i, j] == 0)
                                {
                                    sudoku[i, j] = hold;
                                    s1.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller[i, j, k] != 0)
                                    {
                                        ihtimaller[i, j, k] = 0;
                                        i1.Push(i + "," + j + "," + (k+1));
                                    }
                                    if (ihtimaller[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller[i, k, hold - 1] = 0;
                                        i1.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller[k, j, hold - 1] = 0;
                                        i1.Push(k + "," + j + "," + hold);
                                    }
                                }
                            }
                            sayac2 = 0;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller[k, j, a] == 1)
                                {
                                    sayac3++;
                                }
                            }
                            if (sayac3 == 1)
                            {
                                //     MessageBox.Show("C: "+hold);
                                if (sudoku[i, j] == 0)
                                {
                                    sudoku[i, j] = hold;
                                    s1.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller[i, j, k] != 0)
                                    {
                                        ihtimaller[i, j, k] = 0;
                                        i1.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller[i, k, hold - 1] = 0;
                                        i1.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller[k, j, hold - 1] = 0;
                                        i1.Push(k + "," + j + "," + hold);
                                    }
                                    

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            sayac3 = 0;


                        }
                    }
                    if (sayac == 1)
                    {
                        //  MessageBox.Show("A: "+hold+" "+i+" "+j);
                        if (sudoku[i, j] == 0)
                        {
                            sudoku[i, j] = hold;
                            s1.Push(i + "," + j + "," + hold);
                            change++;
                        }
                        for (int k = 0; k < 9; k++)
                        {

                            if (ihtimaller[i, j, k] != 0)
                            {
                                ihtimaller[i, j, k] = 0;
                                i1.Push(i + "," + j + "," + (k + 1));
                            }
                            if (ihtimaller[i, k, hold - 1] != 0)
                            {
                                ihtimaller[i, k, hold - 1] = 0;
                                i1.Push(i + "," + k + "," + hold);
                            }
                            if (ihtimaller[k, j, hold - 1] != 0)
                            {
                                ihtimaller[k, j, hold - 1] = 0;
                                i1.Push(k + "," + j + "," + hold);
                            }

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sayac = 0;
                }
            }


            for (int k = 0; k < 9; k++)
            {
                for (int i = startX; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i >= 0 && i <= 2 && j >= 0 && i <= 2)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[0]++;
                                varmiX[0] = i;
                                varmiY[0] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[1]++;
                                varmiX[1] = i;
                                varmiY[1] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[2]++;
                                varmiX[2] = i;
                                varmiY[2] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[3]++;
                                varmiX[3] = i;
                                varmiY[3] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[4]++;
                                varmiX[4] = i;
                                varmiY[4] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[5]++;
                                varmiX[5] = i;
                                varmiY[5] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[6]++;
                                varmiX[6] = i;
                                varmiY[6] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[7]++;
                                varmiX[7] = i;
                                varmiY[7] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[8]++;
                                varmiX[8] = i;
                                varmiY[8] = j;
                            }
                        }
                    }
                }

                for (int a = 0; a < 9; a++)
                {
                    if (varmi[a] == 1)
                    {
                        //  MessageBox.Show("B: "+hold);
                        if (sudoku[varmiX[a], varmiY[a]] == 0)
                        {
                            sudoku[varmiX[a], varmiY[a]] = k + 1;
                            s1.Push(varmiX[a] + "," + varmiY[a] + "," + (k + 1));
                            change++;
                        }
                        
                        for (int b = 0; b < 9; b++)
                        {
                            if (ihtimaller[varmiX[a], varmiY[a], b] != 0)
                            {
                                ihtimaller[varmiX[a], varmiY[a], b] = 0;
                                i1.Push(varmiX[a] + "," + varmiY[a] + "," + (b + 1));
                            }
                            if (ihtimaller[varmiX[a], b, k] != 0)
                            {
                                ihtimaller[varmiX[a], b, k] = 0;
                                i1.Push(varmiX[a] + "," + b + "," + (k + 1));
                            }
                            if (ihtimaller[b, varmiY[a], k] != 0)
                            {
                                ihtimaller[b, varmiY[a], k] = 0;
                                i1.Push(b + "," + varmiY[a] + "," + (k + 1));
                            }
                            
                        }
                    }
                }
                for (int b = 0; b < 9; b++)
                {
                    varmi[b] = 0;
                }

            }
            /**
            int x = 0, y = 0;
            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is TextBox)
                {
                    if (sudoku[x, y] == 0)
                    {
                        kontrol.Text = null;
                    }
                    else
                    {
                        kontrol.Text = sudoku[x, y].ToString();
                    }
                    y++;
                    if (y % 9 == 0)
                    {
                        x++;
                        y = 0;
                    }
                }
            }
            */
            sayac = 0; hold = 0; sayac2 = 0; sayac3 = 0;
            for (int i = 0; i < 9; i++)
            {
                varmi[i] = 0;
                varmiX[i] = 0;
                varmiY[i] = 0;
            }


            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        if (ihtimaller[i, j, a] != 0)
                        {
                            hold = a + 1; //sudoku[] matrisine yazmak için
                            sayac++;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller[i, k, a] == 1)
                                {
                                    sayac2++;
                                }
                            }
                            if (sayac2 == 1)
                            {
                                //    MessageBox.Show("D: "+hold);
                                if (sudoku[i, j] == 0)
                                {
                                    sudoku[i, j] = hold;
                                    s1.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller[i, j, k] != 0)
                                    {
                                        ihtimaller[i, j, k] = 0;
                                        i1.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller[i, k, hold - 1] = 0;
                                        i1.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller[k, j, hold - 1] = 0;
                                        i1.Push(k + "," + j + "," + hold);
                                    }
                                }
                            }
                            sayac2 = 0;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller[k, j, a] == 1)
                                {
                                    sayac3++;
                                }
                            }
                            if (sayac3 == 1)
                            {
                                //     MessageBox.Show("C: "+hold);
                                if (sudoku[i, j] == 0)
                                {
                                    sudoku[i, j] = hold;
                                    s1.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller[i, j, k] != 0)
                                    {
                                        ihtimaller[i, j, k] = 0;
                                        i1.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller[i, k, hold - 1] = 0;
                                        i1.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller[k, j, hold - 1] = 0;
                                        i1.Push(k + "," + j + "," + hold);
                                    }

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller[f, g, hold - 1] = 0;
                                                    i1.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            sayac3 = 0;
                        }
                    }
                    if (sayac == 1)
                    {
                        //  MessageBox.Show("A: "+hold+" "+i+" "+j);
                        if (sudoku[i, j] == 0)
                        {
                            sudoku[i, j] = hold;
                            s1.Push(i + "," + j + "," + hold);
                            change++;
                        }
                        for (int k = 0; k < 9; k++)
                        {

                            if (ihtimaller[i, j, k] != 0)
                            {
                                ihtimaller[i, j, k] = 0;
                                i1.Push(i + "," + j + "," + (k + 1));
                            }
                            if (ihtimaller[i, k, hold - 1] != 0)
                            {
                                ihtimaller[i, k, hold - 1] = 0;
                                i1.Push(i + "," + k + "," + hold);
                            }
                            if (ihtimaller[k, j, hold - 1] != 0)
                            {
                                ihtimaller[k, j, hold - 1] = 0;
                                i1.Push(k + "," + j + "," + hold);
                            }

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        ihtimaller[f, g, hold - 1] = 0;
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller[f, g, hold - 1] = 0;
                                            i1.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sayac = 0;
                }
            }


            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < startX; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i >= 0 && i <= 2 && j >= 0 && i <= 2)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[0]++;
                                varmiX[0] = i;
                                varmiY[0] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[1]++;
                                varmiX[1] = i;
                                varmiY[1] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[2]++;
                                varmiX[2] = i;
                                varmiY[2] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[3]++;
                                varmiX[3] = i;
                                varmiY[3] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[4]++;
                                varmiX[4] = i;
                                varmiY[4] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[5]++;
                                varmiX[5] = i;
                                varmiY[5] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[6]++;
                                varmiX[6] = i;
                                varmiY[6] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[7]++;
                                varmiX[7] = i;
                                varmiY[7] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller[i, j, k] == 1)
                            {
                                varmi[8]++;
                                varmiX[8] = i;
                                varmiY[8] = j;
                            }
                        }
                    }
                }

                for (int a = 0; a < 9; a++)
                {
                    if (varmi[a] == 1)
                    {
                        //  MessageBox.Show("B: "+hold);
                        if (sudoku[varmiX[a], varmiY[a]] == 0)
                        {
                            sudoku[varmiX[a], varmiY[a]] = k + 1;
                            s1.Push(varmiX[a] + "," + varmiY[a] + "," + (k + 1));
                            change++;
                        }
                        for (int b = 0; b < 9; b++)
                        {
                            if (ihtimaller[varmiX[a], varmiY[a], b] != 0)
                            {
                                ihtimaller[varmiX[a], varmiY[a], b] = 0;
                                i1.Push(varmiX[a] + "," + varmiY[a] + "," + (b + 1));
                            }
                            if (ihtimaller[varmiX[a], b, k] != 0)
                            {
                                ihtimaller[varmiX[a], b, k] = 0;
                                i1.Push(varmiX[a] + "," + b + "," + (k + 1));
                            }
                            if (ihtimaller[b, varmiY[a], k] != 0)
                            {
                                ihtimaller[b, varmiY[a], k] = 0;
                                i1.Push(b + "," + varmiY[a] + "," + (k + 1));
                            }
                        }
                    }
                }
                for (int b = 0; b < 9; b++)
                {
                    varmi[b] = 0;
                }

            }
            /*
            x = 0;  y = 0;
            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is TextBox)
                {
                    if (sudoku[x, y] == 0)
                    {
                        kontrol.Text = null;
                    }
                    else
                    {
                        kontrol.Text = sudoku[x, y].ToString();
                    }
                    y++;
                    if (y % 9 == 0)
                    {
                        x++;
                        y = 0;
                    }
                }
            }
            */
        }

        public void button04(int startX, int startY)
        {
            int sayac1 = 0, sayac2 = 0, sayac3 = 0;
            int tut1 = 0, tut2 = 0;

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller[i, j, a] == 1)
                                {
                                    //Yatay 2liler

                                    for (int x = 0; x < 9; x++)
                                    {
                                        if (ihtimaller[i, x, a] == 1 && ihtimaller[i, x, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = x;
                                        }
                                        if (ihtimaller[i, x, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller[i, x, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int y = 0; y < 9; y++)
                                        {
                                            if (y != a && y != k)
                                            {
                                                if (ihtimaller[i, tut1, y] != 0)
                                                {
                                                    ihtimaller[i, tut1, y] = 0;
                                                    i1.Push(i + "," + tut1 + "," + (y+1));
                                                }
                                                if (ihtimaller[i, j, y] != 0)
                                                {
                                                    ihtimaller[i, j, y] = 0;
                                                    i1.Push(i + "," + j + "," + (y + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;


                                    //3lüdeki 2liler

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }                                                    
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }                                                    
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    { //Burası
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }                                                   
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;




                                }
                            }
                        }
                    }
                }
            }

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller[i, j, a] == 1)
                                {
                                    //Dikey 2liler
                                    for (int m = 0; m < 9; m++)
                                    {
                                        if (ihtimaller[m, j, a] == 1 && ihtimaller[m, j, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = m;

                                        }
                                        if (ihtimaller[m, j, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller[m, j, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int n = 0; n < 9; n++)
                                        {
                                            if (n != a && n != k)
                                            {
                                                if (ihtimaller[tut1, j, n] != 0)
                                                {
                                                    ihtimaller[tut1, j, n] = 0;
                                                    i1.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                                if (ihtimaller[i, j, n] != 0)
                                                {
                                                    ihtimaller[i, j, n] = 0;
                                                    i1.Push(i + "," + j + "," + (n + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;
                                }
                            }
                        }
                    }
                }
            }

            // AYRIM // **************************************************************************************************************

            sayac1 = 0; sayac2 = 0; sayac3 = 0;
            tut1 = 0; tut2 = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller[i, j, a] == 1)
                                {
                                    //Yatay 2liler

                                    for (int x = 0; x < 9; x++)
                                    {
                                        if (ihtimaller[i, x, a] == 1 && ihtimaller[i, x, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = x;
                                        }
                                        if (ihtimaller[i, x, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller[i, x, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int y = 0; y < 9; y++)
                                        {
                                            if (y != a && y != k)
                                            {
                                                if (ihtimaller[i, tut1, y] != 0)
                                                {
                                                    ihtimaller[i, tut1, y] = 0;
                                                    i1.Push(i + "," + tut1 + "," + (y + 1));
                                                }
                                                if (ihtimaller[i, j, y] != 0)
                                                {
                                                    ihtimaller[i, j, y] = 0;
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;


                                    //3lüdeki 2liler

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    { //Burası
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller[f, g, a] == 1 && ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller[tut1, tut2, n] = 0;
                                                        i1.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller[i, j, n] != 0)
                                                    {
                                                        ihtimaller[i, j, n] = 0;
                                                        i1.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;




                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller[i, j, a] == 1)
                                {
                                    //Dikey 2liler
                                    for (int m = 0; m < 9; m++)
                                    {
                                        if (ihtimaller[m, j, a] == 1 && ihtimaller[m, j, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = m;

                                        }
                                        if (ihtimaller[m, j, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller[m, j, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int n = 0; n < 9; n++)
                                        {
                                            if (n != a && n != k)
                                            {
                                                if (ihtimaller[tut1, j, n] != 0)
                                                {
                                                    ihtimaller[tut1, j, n] = 0;
                                                    i1.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                                if (ihtimaller[i, j, n] != 0)
                                                {
                                                    ihtimaller[i, j, n] = 0;
                                                    i1.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;
                                }
                            }
                        }
                    }
                }
            }

            // AYRIM // **************************************************************************************************************
        } //2Liler       

        public void button05(int startX, int startY)
        {
            int sayac = 0, sayac2 = 0, sayac3 = 0, sira = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = startY; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2) //sayac 2 yada 3 ise
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                    
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = startY; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                //            MessageBox.Show("Girdi11 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                             //               MessageBox.Show("Girdi13 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                  //          MessageBox.Show("Girdi15 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                 //           MessageBox.Show("Girdi17 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }
            // AYRIM // €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = startX+1; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi1 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //     MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi11 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi13 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi15 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi17 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX+1; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //        MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //             MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        // MessageBox.Show("Deneme");
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //                 MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi11 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //               MessageBox.Show("Girdi13 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi15 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //           MessageBox.Show("Girdi17 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM // €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

            // AYRIM2 // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi1 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //     MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi11 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi13 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi15 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi17 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //        MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //             MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        // MessageBox.Show("Deneme");
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //                 MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi11 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //               MessageBox.Show("Girdi13 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi15 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //           MessageBox.Show("Girdi17 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM2 // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

            // AYRIM3 // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = 0; j < startY; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi1 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //     MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi11 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi13 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi15 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi17 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[f, a, k] != 0)
                                                {
                                                    ihtimaller[f, a, k] = 0;
                                                    i1.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = 0; j < startY; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //        MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //             MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        // MessageBox.Show("Deneme");
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //                 MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                ihtimaller[a, f, k] = 0;
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi11 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //               MessageBox.Show("Girdi13 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi15 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //           MessageBox.Show("Girdi17 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller[a, f, k] != 0)
                                                {
                                                    ihtimaller[a, f, k] = 0;
                                                    i1.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM3 // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||


        }


        public void button03_1(int startX, int startY)
        {
            int sayac = 0, hold = 0, sayac2 = 0, sayac3 = 0;
            int[] varmi = new int[9];
            int[] varmiX = new int[9];
            int[] varmiY = new int[9];

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        if (ihtimaller_1[i, j, a] != 0)
                        {
                            hold = a + 1; //sudoku_1[] matrisine yazmak için
                            sayac++;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_1[i, k, a] == 1)
                                {
                                    sayac2++;
                                }
                            }
                            if (sayac2 == 1)
                            {
                                //    MessageBox.Show("D: "+hold);
                                if (sudoku_1[i, j] == 0)
                                {
                                    sudoku_1[i, j] = hold;
                                    s2.Push(i + "," + j + "," + hold);
                                    change++;
                                }

                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_1[i, j, k] != 0)
                                    {
                                        ihtimaller_1[i, j, k] = 0;
                                        i2.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_1[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_1[i, k, hold - 1] = 0;
                                        i2.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_1[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_1[k, j, hold - 1] = 0;
                                        i2.Push(k + "," + j + "," + hold);
                                    }
                                }
                            }
                            sayac2 = 0;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_1[k, j, a] == 1)
                                {
                                    sayac3++;
                                }
                            }
                            if (sayac3 == 1)
                            {
                                //     MessageBox.Show("C: "+hold);
                                if (sudoku_1[i, j] == 0)
                                {
                                    sudoku_1[i, j] = hold;
                                    s2.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_1[i, j, k] != 0)
                                    {
                                        ihtimaller_1[i, j, k] = 0;
                                        i2.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_1[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_1[i, k, hold - 1] = 0;
                                        i2.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_1[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_1[k, j, hold - 1] = 0;
                                        i2.Push(k + "," + j + "," + hold);
                                    }


                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            sayac3 = 0;


                        }
                    }
                    if (sayac == 1)
                    {
                        //  MessageBox.Show("A: "+hold+" "+i+" "+j);
                        if (sudoku_1[i, j] == 0)
                        {
                            sudoku_1[i, j] = hold;
                            s2.Push(i + "," + j + "," + hold);
                            change++;
                        }
                        for (int k = 0; k < 9; k++)
                        {

                            if (ihtimaller_1[i, j, k] != 0)
                            {
                                ihtimaller_1[i, j, k] = 0;
                                i2.Push(i + "," + j + "," + (k + 1));
                            }
                            if (ihtimaller_1[i, k, hold - 1] != 0)
                            {
                                ihtimaller_1[i, k, hold - 1] = 0;
                                i2.Push(i + "," + k + "," + hold);
                            }
                            if (ihtimaller_1[k, j, hold - 1] != 0)
                            {
                                ihtimaller_1[k, j, hold - 1] = 0;
                                i2.Push(k + "," + j + "," + hold);
                            }

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sayac = 0;
                }
            }


            for (int k = 0; k < 9; k++)
            {
                for (int i = startX; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i >= 0 && i <= 2 && j >= 0 && i <= 2)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[0]++;
                                varmiX[0] = i;
                                varmiY[0] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[1]++;
                                varmiX[1] = i;
                                varmiY[1] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[2]++;
                                varmiX[2] = i;
                                varmiY[2] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[3]++;
                                varmiX[3] = i;
                                varmiY[3] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[4]++;
                                varmiX[4] = i;
                                varmiY[4] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[5]++;
                                varmiX[5] = i;
                                varmiY[5] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[6]++;
                                varmiX[6] = i;
                                varmiY[6] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[7]++;
                                varmiX[7] = i;
                                varmiY[7] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[8]++;
                                varmiX[8] = i;
                                varmiY[8] = j;
                            }
                        }
                    }
                }

                for (int a = 0; a < 9; a++)
                {
                    if (varmi[a] == 1)
                    {
                        //  MessageBox.Show("B: "+hold);
                        if (sudoku_1[varmiX[a], varmiY[a]] == 0)
                        {
                            sudoku_1[varmiX[a], varmiY[a]] = k + 1;
                            s2.Push(varmiX[a] + "," + varmiY[a] + "," + (k + 1));
                            change++;
                        }

                        for (int b = 0; b < 9; b++)
                        {
                            if (ihtimaller_1[varmiX[a], varmiY[a], b] != 0)
                            {
                                ihtimaller_1[varmiX[a], varmiY[a], b] = 0;
                                i2.Push(varmiX[a] + "," + varmiY[a] + "," + (b + 1));
                            }
                            if (ihtimaller_1[varmiX[a], b, k] != 0)
                            {
                                ihtimaller_1[varmiX[a], b, k] = 0;
                                i2.Push(varmiX[a] + "," + b + "," + (k + 1));
                            }
                            if (ihtimaller_1[b, varmiY[a], k] != 0)
                            {
                                ihtimaller_1[b, varmiY[a], k] = 0;
                                i2.Push(b + "," + varmiY[a] + "," + (k + 1));
                            }

                        }
                    }
                }
                for (int b = 0; b < 9; b++)
                {
                    varmi[b] = 0;
                }

            }
            /**
            int x = 0, y = 0;
            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is TextBox)
                {
                    if (sudoku_1[x, y] == 0)
                    {
                        kontrol.Text = null;
                    }
                    else
                    {
                        kontrol.Text = sudoku_1[x, y].ToString();
                    }
                    y++;
                    if (y % 9 == 0)
                    {
                        x++;
                        y = 0;
                    }
                }
            }
            */
            sayac = 0; hold = 0; sayac2 = 0; sayac3 = 0;
            for (int i = 0; i < 9; i++)
            {
                varmi[i] = 0;
                varmiX[i] = 0;
                varmiY[i] = 0;
            }


            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        if (ihtimaller_1[i, j, a] != 0)
                        {
                            hold = a + 1; //sudoku_1[] matrisine yazmak için
                            sayac++;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_1[i, k, a] == 1)
                                {
                                    sayac2++;
                                }
                            }
                            if (sayac2 == 1)
                            {
                                //    MessageBox.Show("D: "+hold);
                                if (sudoku_1[i, j] == 0)
                                {
                                    sudoku_1[i, j] = hold;
                                    s2.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_1[i, j, k] != 0)
                                    {
                                        ihtimaller_1[i, j, k] = 0;
                                        i2.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_1[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_1[i, k, hold - 1] = 0;
                                        i2.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_1[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_1[k, j, hold - 1] = 0;
                                        i2.Push(k + "," + j + "," + hold);
                                    }
                                }
                            }
                            sayac2 = 0;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_1[k, j, a] == 1)
                                {
                                    sayac3++;
                                }
                            }
                            if (sayac3 == 1)
                            {
                                //     MessageBox.Show("C: "+hold);
                                if (sudoku_1[i, j] == 0)
                                {
                                    sudoku_1[i, j] = hold;
                                    s2.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_1[i, j, k] != 0)
                                    {
                                        ihtimaller_1[i, j, k] = 0;
                                        i2.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_1[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_1[i, k, hold - 1] = 0;
                                        i2.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_1[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_1[k, j, hold - 1] = 0;
                                        i2.Push(k + "," + j + "," + hold);
                                    }

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_1[f, g, hold - 1] = 0;
                                                    i2.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            sayac3 = 0;
                        }
                    }
                    if (sayac == 1)
                    {
                        //  MessageBox.Show("A: "+hold+" "+i+" "+j);
                        if (sudoku_1[i, j] == 0)
                        {
                            sudoku_1[i, j] = hold;
                            s2.Push(i + "," + j + "," + hold);
                            change++;
                        }
                        for (int k = 0; k < 9; k++)
                        {

                            if (ihtimaller_1[i, j, k] != 0)
                            {
                                ihtimaller_1[i, j, k] = 0;
                                i2.Push(i + "," + j + "," + (k + 1));
                            }
                            if (ihtimaller_1[i, k, hold - 1] != 0)
                            {
                                ihtimaller_1[i, k, hold - 1] = 0;
                                i2.Push(i + "," + k + "," + hold);
                            }
                            if (ihtimaller_1[k, j, hold - 1] != 0)
                            {
                                ihtimaller_1[k, j, hold - 1] = 0;
                                i2.Push(k + "," + j + "," + hold);
                            }

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        ihtimaller_1[f, g, hold - 1] = 0;
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_1[f, g, hold - 1] = 0;
                                            i2.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sayac = 0;
                }
            }


            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < startX; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i >= 0 && i <= 2 && j >= 0 && i <= 2)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[0]++;
                                varmiX[0] = i;
                                varmiY[0] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[1]++;
                                varmiX[1] = i;
                                varmiY[1] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[2]++;
                                varmiX[2] = i;
                                varmiY[2] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[3]++;
                                varmiX[3] = i;
                                varmiY[3] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[4]++;
                                varmiX[4] = i;
                                varmiY[4] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[5]++;
                                varmiX[5] = i;
                                varmiY[5] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[6]++;
                                varmiX[6] = i;
                                varmiY[6] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[7]++;
                                varmiX[7] = i;
                                varmiY[7] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_1[i, j, k] == 1)
                            {
                                varmi[8]++;
                                varmiX[8] = i;
                                varmiY[8] = j;
                            }
                        }
                    }
                }

                for (int a = 0; a < 9; a++)
                {
                    if (varmi[a] == 1)
                    {
                        //  MessageBox.Show("B: "+hold);
                        if (sudoku_1[varmiX[a], varmiY[a]] == 0)
                        {
                            sudoku_1[varmiX[a], varmiY[a]] = k + 1;
                            s2.Push(varmiX[a] + "," + varmiY[a] + "," + (k + 1));
                            change++;
                        }
                        for (int b = 0; b < 9; b++)
                        {
                            if (ihtimaller_1[varmiX[a], varmiY[a], b] != 0)
                            {
                                ihtimaller_1[varmiX[a], varmiY[a], b] = 0;
                                i2.Push(varmiX[a] + "," + varmiY[a] + "," + (b + 1));
                            }
                            if (ihtimaller_1[varmiX[a], b, k] != 0)
                            {
                                ihtimaller_1[varmiX[a], b, k] = 0;
                                i2.Push(varmiX[a] + "," + b + "," + (k + 1));
                            }
                            if (ihtimaller_1[b, varmiY[a], k] != 0)
                            {
                                ihtimaller_1[b, varmiY[a], k] = 0;
                                i2.Push(b + "," + varmiY[a] + "," + (k + 1));
                            }
                        }
                    }
                }
                for (int b = 0; b < 9; b++)
                {
                    varmi[b] = 0;
                }

            }
            /*
            x = 0;  y = 0;
            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is TextBox)
                {
                    if (sudoku_1[x, y] == 0)
                    {
                        kontrol.Text = null;
                    }
                    else
                    {
                        kontrol.Text = sudoku_1[x, y].ToString();
                    }
                    y++;
                    if (y % 9 == 0)
                    {
                        x++;
                        y = 0;
                    }
                }
            }
            */
        }

        public void button04_1(int startX, int startY)
        {
            int sayac1 = 0, sayac2 = 0, sayac3 = 0;
            int tut1 = 0, tut2 = 0;

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_1[i, j, a] == 1)
                                {
                                    //Yatay 2liler

                                    for (int x = 0; x < 9; x++)
                                    {
                                        if (ihtimaller_1[i, x, a] == 1 && ihtimaller_1[i, x, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = x;
                                        }
                                        if (ihtimaller_1[i, x, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_1[i, x, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int y = 0; y < 9; y++)
                                        {
                                            if (y != a && y != k)
                                            {
                                                if (ihtimaller_1[i, tut1, y] != 0)
                                                {
                                                    ihtimaller_1[i, tut1, y] = 0;
                                                    i2.Push(i + "," + tut1 + "," + (y + 1));
                                                }
                                                if (ihtimaller_1[i, j, y] != 0)
                                                {
                                                    ihtimaller_1[i, j, y] = 0;
                                                    i2.Push(i + "," + j + "," + (y + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;


                                    //3lüdeki 2liler

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    { //Burası
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;




                                }
                            }
                        }
                    }
                }
            }

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_1[i, j, a] == 1)
                                {
                                    //Dikey 2liler
                                    for (int m = 0; m < 9; m++)
                                    {
                                        if (ihtimaller_1[m, j, a] == 1 && ihtimaller_1[m, j, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = m;

                                        }
                                        if (ihtimaller_1[m, j, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_1[m, j, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int n = 0; n < 9; n++)
                                        {
                                            if (n != a && n != k)
                                            {
                                                if (ihtimaller_1[tut1, j, n] != 0)
                                                {
                                                    ihtimaller_1[tut1, j, n] = 0;
                                                    i2.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                                if (ihtimaller_1[i, j, n] != 0)
                                                {
                                                    ihtimaller_1[i, j, n] = 0;
                                                    i2.Push(i + "," + j + "," + (n + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;
                                }
                            }
                        }
                    }
                }
            }

            // AYRIM // **************************************************************************************************************

            sayac1 = 0; sayac2 = 0; sayac3 = 0;
            tut1 = 0; tut2 = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_1[i, j, a] == 1)
                                {
                                    //Yatay 2liler

                                    for (int x = 0; x < 9; x++)
                                    {
                                        if (ihtimaller_1[i, x, a] == 1 && ihtimaller_1[i, x, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = x;
                                        }
                                        if (ihtimaller_1[i, x, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_1[i, x, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int y = 0; y < 9; y++)
                                        {
                                            if (y != a && y != k)
                                            {
                                                if (ihtimaller_1[i, tut1, y] != 0)
                                                {
                                                    ihtimaller_1[i, tut1, y] = 0;
                                                    i2.Push(i + "," + tut1 + "," + (y + 1));
                                                }
                                                if (ihtimaller_1[i, j, y] != 0)
                                                {
                                                    ihtimaller_1[i, j, y] = 0;
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;


                                    //3lüdeki 2liler

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    { //Burası
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_1[f, g, a] == 1 && ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_1[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_1[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_1[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_1[tut1, tut2, n] = 0;
                                                        i2.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_1[i, j, n] != 0)
                                                    {
                                                        ihtimaller_1[i, j, n] = 0;
                                                        i2.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;




                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_1[i, j, a] == 1)
                                {
                                    //Dikey 2liler
                                    for (int m = 0; m < 9; m++)
                                    {
                                        if (ihtimaller_1[m, j, a] == 1 && ihtimaller_1[m, j, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = m;

                                        }
                                        if (ihtimaller_1[m, j, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_1[m, j, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int n = 0; n < 9; n++)
                                        {
                                            if (n != a && n != k)
                                            {
                                                if (ihtimaller_1[tut1, j, n] != 0)
                                                {
                                                    ihtimaller_1[tut1, j, n] = 0;
                                                    i2.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                                if (ihtimaller_1[i, j, n] != 0)
                                                {
                                                    ihtimaller_1[i, j, n] = 0;
                                                    i2.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;
                                }
                            }
                        }
                    }
                }
            }

            // AYRIM // **************************************************************************************************************
        } //2Liler       

        public void button05_1(int startX, int startY)
        {
            int sayac = 0, sayac2 = 0, sayac3 = 0, sira = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = startY; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi2 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));

                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //     MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = startY; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //        MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //             MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        // MessageBox.Show("Deneme");
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //                 MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //               MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //           MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }
            // AYRIM // €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = startX + 1; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi2 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //     MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX + 1; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //        MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //             MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        // MessageBox.Show("Deneme");
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //                 MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //               MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //           MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM // €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

            // AYRIM2 // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi2 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //     MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //        MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //             MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        // MessageBox.Show("Deneme");
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //                 MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //               MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //           MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM2 // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

            // AYRIM3 // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = 0; j < startY; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi2 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //   MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //     MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //      MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //       MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[f, a, k] != 0)
                                                {
                                                    ihtimaller_1[f, a, k] = 0;
                                                    i2.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = 0; j < startY; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_1[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //        MessageBox.Show("Girdi3 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi5 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //             MessageBox.Show("Girdi7 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        // MessageBox.Show("Deneme");
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //                 MessageBox.Show("Girdi9 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                ihtimaller_1[a, f, k] = 0;
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //            MessageBox.Show("Girdi21 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //               MessageBox.Show("Girdi23 " + f + " " + k);
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //          MessageBox.Show("Girdi25 " + f + " " + k);
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_1[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_1[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            //           MessageBox.Show("Girdi27 " + f + " " + k);
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_1[a, f, k] != 0)
                                                {
                                                    ihtimaller_1[a, f, k] = 0;
                                                    i2.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM3 // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||


        }


        public void button03_2(int startX, int startY)
        {
            int sayac = 0, hold = 0, sayac2 = 0, sayac3 = 0;
            int[] varmi = new int[9];
            int[] varmiX = new int[9];
            int[] varmiY = new int[9];

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        if (ihtimaller_2[i, j, a] != 0)
                        {
                            hold = a + 1; //sudoku_2[] matrisine yazmak için
                            sayac++;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_2[i, k, a] == 1)
                                {
                                    sayac2++;
                                }
                            }
                            if (sayac2 == 1)
                            {
                                //    MessageBox.Show("D: "+hold);
                                if (sudoku_2[i, j] == 0)
                                {
                                    sudoku_2[i, j] = hold;
                                    s3.Push(i + "," + j + "," + hold);
                                    change++;
                                }

                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_2[i, j, k] != 0)
                                    {
                                        ihtimaller_2[i, j, k] = 0;
                                        i3.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_2[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_2[i, k, hold - 1] = 0;
                                        i3.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_2[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_2[k, j, hold - 1] = 0;
                                        i3.Push(k + "," + j + "," + hold);
                                    }
                                }
                            }
                            sayac2 = 0;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_2[k, j, a] == 1)
                                {
                                    sayac3++;
                                }
                            }
                            if (sayac3 == 1)
                            {
                                //     MessageBox.Show("C: "+hold);
                                if (sudoku_2[i, j] == 0)
                                {
                                    sudoku_2[i, j] = hold;
                                    s3.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_2[i, j, k] != 0)
                                    {
                                        ihtimaller_2[i, j, k] = 0;
                                        i3.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_2[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_2[i, k, hold - 1] = 0;
                                        i3.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_2[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_2[k, j, hold - 1] = 0;
                                        i3.Push(k + "," + j + "," + hold);
                                    }


                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            sayac3 = 0;


                        }
                    }
                    if (sayac == 1)
                    {
                        //  MessageBox.Show("A: "+hold+" "+i+" "+j);
                        if (sudoku_2[i, j] == 0)
                        {
                            sudoku_2[i, j] = hold;
                            s3.Push(i + "," + j + "," + hold);
                            change++;
                        }
                        for (int k = 0; k < 9; k++)
                        {

                            if (ihtimaller_2[i, j, k] != 0)
                            {
                                ihtimaller_2[i, j, k] = 0;
                                i3.Push(i + "," + j + "," + (k + 1));
                            }
                            if (ihtimaller_2[i, k, hold - 1] != 0)
                            {
                                ihtimaller_2[i, k, hold - 1] = 0;
                                i3.Push(i + "," + k + "," + hold);
                            }
                            if (ihtimaller_2[k, j, hold - 1] != 0)
                            {
                                ihtimaller_2[k, j, hold - 1] = 0;
                                i3.Push(k + "," + j + "," + hold);
                            }

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sayac = 0;
                }
            }


            for (int k = 0; k < 9; k++)
            {
                for (int i = startX; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i >= 0 && i <= 2 && j >= 0 && i <= 2)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[0]++;
                                varmiX[0] = i;
                                varmiY[0] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[1]++;
                                varmiX[1] = i;
                                varmiY[1] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[2]++;
                                varmiX[2] = i;
                                varmiY[2] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[3]++;
                                varmiX[3] = i;
                                varmiY[3] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[4]++;
                                varmiX[4] = i;
                                varmiY[4] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[5]++;
                                varmiX[5] = i;
                                varmiY[5] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[6]++;
                                varmiX[6] = i;
                                varmiY[6] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[7]++;
                                varmiX[7] = i;
                                varmiY[7] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[8]++;
                                varmiX[8] = i;
                                varmiY[8] = j;
                            }
                        }
                    }
                }

                for (int a = 0; a < 9; a++)
                {
                    if (varmi[a] == 1)
                    {
                        //  MessageBox.Show("B: "+hold);
                        if (sudoku_2[varmiX[a], varmiY[a]] == 0)
                        {
                            sudoku_2[varmiX[a], varmiY[a]] = k + 1;
                            s3.Push(varmiX[a] + "," + varmiY[a] + "," + (k + 1));
                            change++;
                        }

                        for (int b = 0; b < 9; b++)
                        {
                            if (ihtimaller_2[varmiX[a], varmiY[a], b] != 0)
                            {
                                ihtimaller_2[varmiX[a], varmiY[a], b] = 0;
                                i3.Push(varmiX[a] + "," + varmiY[a] + "," + (b + 1));
                            }
                            if (ihtimaller_2[varmiX[a], b, k] != 0)
                            {
                                ihtimaller_2[varmiX[a], b, k] = 0;
                                i3.Push(varmiX[a] + "," + b + "," + (k + 1));
                            }
                            if (ihtimaller_2[b, varmiY[a], k] != 0)
                            {
                                ihtimaller_2[b, varmiY[a], k] = 0;
                                i3.Push(b + "," + varmiY[a] + "," + (k + 1));
                            }

                        }
                    }
                }
                for (int b = 0; b < 9; b++)
                {
                    varmi[b] = 0;
                }

            }
            /**
            int x = 0, y = 0;
            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is TextBox)
                {
                    if (sudoku_2[x, y] == 0)
                    {
                        kontrol.Text = null;
                    }
                    else
                    {
                        kontrol.Text = sudoku_2[x, y].ToString();
                    }
                    y++;
                    if (y % 9 == 0)
                    {
                        x++;
                        y = 0;
                    }
                }
            }
            */
            sayac = 0; hold = 0; sayac2 = 0; sayac3 = 0;
            for (int i = 0; i < 9; i++)
            {
                varmi[i] = 0;
                varmiX[i] = 0;
                varmiY[i] = 0;
            }


            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int a = 0; a < 9; a++)
                    {
                        if (ihtimaller_2[i, j, a] != 0)
                        {
                            hold = a + 1; //sudoku_2[] matrisine yazmak için
                            sayac++;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_2[i, k, a] == 1)
                                {
                                    sayac2++;
                                }
                            }
                            if (sayac2 == 1)
                            {
                                //    MessageBox.Show("D: "+hold);
                                if (sudoku_2[i, j] == 0)
                                {
                                    sudoku_2[i, j] = hold;
                                    s3.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_2[i, j, k] != 0)
                                    {
                                        ihtimaller_2[i, j, k] = 0;
                                        i3.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_2[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_2[i, k, hold - 1] = 0;
                                        i3.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_2[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_2[k, j, hold - 1] = 0;
                                        i3.Push(k + "," + j + "," + hold);
                                    }
                                }
                            }
                            sayac2 = 0;

                            for (int k = 0; k < 9; k++)
                            {
                                if (ihtimaller_2[k, j, a] == 1)
                                {
                                    sayac3++;
                                }
                            }
                            if (sayac3 == 1)
                            {
                                //     MessageBox.Show("C: "+hold);
                                if (sudoku_2[i, j] == 0)
                                {
                                    sudoku_2[i, j] = hold;
                                    s3.Push(i + "," + j + "," + hold);
                                    change++;
                                }
                                for (int k = 0; k < 9; k++)
                                {
                                    if (ihtimaller_2[i, j, k] != 0)
                                    {
                                        ihtimaller_2[i, j, k] = 0;
                                        i3.Push(i + "," + j + "," + (k + 1));
                                    }
                                    if (ihtimaller_2[i, k, hold - 1] != 0)
                                    {
                                        ihtimaller_2[i, k, hold - 1] = 0;
                                        i3.Push(i + "," + k + "," + hold);
                                    }
                                    if (ihtimaller_2[k, j, hold - 1] != 0)
                                    {
                                        ihtimaller_2[k, j, hold - 1] = 0;
                                        i3.Push(k + "," + j + "," + hold);
                                    }

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {

                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, hold - 1] != 0)
                                                {
                                                    ihtimaller_2[f, g, hold - 1] = 0;
                                                    i3.Push(f + "," + g + "," + hold);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            sayac3 = 0;
                        }
                    }
                    if (sayac == 1)
                    {
                        //  MessageBox.Show("A: "+hold+" "+i+" "+j);
                        if (sudoku_2[i, j] == 0)
                        {
                            sudoku_2[i, j] = hold;
                            s3.Push(i + "," + j + "," + hold);
                            change++;
                        }
                        for (int k = 0; k < 9; k++)
                        {

                            if (ihtimaller_2[i, j, k] != 0)
                            {
                                ihtimaller_2[i, j, k] = 0;
                                i3.Push(i + "," + j + "," + (k + 1));
                            }
                            if (ihtimaller_2[i, k, hold - 1] != 0)
                            {
                                ihtimaller_2[i, k, hold - 1] = 0;
                                i3.Push(i + "," + k + "," + hold);
                            }
                            if (ihtimaller_2[k, j, hold - 1] != 0)
                            {
                                ihtimaller_2[k, j, hold - 1] = 0;
                                i3.Push(k + "," + j + "," + hold);
                            }

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        ihtimaller_2[f, g, hold - 1] = 0;
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, hold - 1] != 0)
                                        {
                                            ihtimaller_2[f, g, hold - 1] = 0;
                                            i3.Push(f + "," + g + "," + hold);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sayac = 0;
                }
            }


            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < startX; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i >= 0 && i <= 2 && j >= 0 && i <= 2)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[0]++;
                                varmiX[0] = i;
                                varmiY[0] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[1]++;
                                varmiX[1] = i;
                                varmiY[1] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[2]++;
                                varmiX[2] = i;
                                varmiY[2] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[3]++;
                                varmiX[3] = i;
                                varmiY[3] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[4]++;
                                varmiX[4] = i;
                                varmiY[4] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[5]++;
                                varmiX[5] = i;
                                varmiY[5] = j;
                            }
                        }
                        else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[6]++;
                                varmiX[6] = i;
                                varmiY[6] = j;
                            }
                        }
                        else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[7]++;
                                varmiX[7] = i;
                                varmiY[7] = j;
                            }
                        }
                        else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                        {
                            if (ihtimaller_2[i, j, k] == 1)
                            {
                                varmi[8]++;
                                varmiX[8] = i;
                                varmiY[8] = j;
                            }
                        }
                    }
                }

                for (int a = 0; a < 9; a++)
                {
                    if (varmi[a] == 1)
                    {
                        //  MessageBox.Show("B: "+hold);
                        if (sudoku_2[varmiX[a], varmiY[a]] == 0)
                        {
                            sudoku_2[varmiX[a], varmiY[a]] = k + 1;
                            s3.Push(varmiX[a] + "," + varmiY[a] + "," + (k + 1));
                            change++;
                        }
                        for (int b = 0; b < 9; b++)
                        {
                            if (ihtimaller_2[varmiX[a], varmiY[a], b] != 0)
                            {
                                ihtimaller_2[varmiX[a], varmiY[a], b] = 0;
                                i3.Push(varmiX[a] + "," + varmiY[a] + "," + (b + 1));
                            }
                            if (ihtimaller_2[varmiX[a], b, k] != 0)
                            {
                                ihtimaller_2[varmiX[a], b, k] = 0;
                                i3.Push(varmiX[a] + "," + b + "," + (k + 1));
                            }
                            if (ihtimaller_2[b, varmiY[a], k] != 0)
                            {
                                ihtimaller_2[b, varmiY[a], k] = 0;
                                i3.Push(b + "," + varmiY[a] + "," + (k + 1));
                            }
                        }
                    }
                }
                for (int b = 0; b < 9; b++)
                {
                    varmi[b] = 0;
                }

            }
            /*
            x = 0;  y = 0;
            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is TextBox)
                {
                    if (sudoku_2[x, y] == 0)
                    {
                        kontrol.Text = null;
                    }
                    else
                    {
                        kontrol.Text = sudoku_2[x, y].ToString();
                    }
                    y++;
                    if (y % 9 == 0)
                    {
                        x++;
                        y = 0;
                    }
                }
            }
            */
        }

        public void button04_2(int startX, int startY)
        {
            int sayac1 = 0, sayac2 = 0, sayac3 = 0;
            int tut1 = 0, tut2 = 0;

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_2[i, j, a] == 1)
                                {
                                    //Yatay 2liler

                                    for (int x = 0; x < 9; x++)
                                    {
                                        if (ihtimaller_2[i, x, a] == 1 && ihtimaller_2[i, x, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = x;
                                        }
                                        if (ihtimaller_2[i, x, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_2[i, x, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int y = 0; y < 9; y++)
                                        {
                                            if (y != a && y != k)
                                            {
                                                if (ihtimaller_2[i, tut1, y] != 0)
                                                {
                                                    ihtimaller_2[i, tut1, y] = 0;
                                                    i3.Push(i + "," + tut1 + "," + (y + 1));
                                                }
                                                if (ihtimaller_2[i, j, y] != 0)
                                                {
                                                    ihtimaller_2[i, j, y] = 0;
                                                    i3.Push(i + "," + j + "," + (y + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;


                                    //3lüdeki 2liler

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    { //Burası
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;




                                }
                            }
                        }
                    }
                }
            }

            for (int i = startX; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_2[i, j, a] == 1)
                                {
                                    //Dikey 2liler
                                    for (int m = 0; m < 9; m++)
                                    {
                                        if (ihtimaller_2[m, j, a] == 1 && ihtimaller_2[m, j, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = m;

                                        }
                                        if (ihtimaller_2[m, j, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_2[m, j, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int n = 0; n < 9; n++)
                                        {
                                            if (n != a && n != k)
                                            {
                                                if (ihtimaller_2[tut1, j, n] != 0)
                                                {
                                                    ihtimaller_2[tut1, j, n] = 0;
                                                    i3.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                                if (ihtimaller_2[i, j, n] != 0)
                                                {
                                                    ihtimaller_2[i, j, n] = 0;
                                                    i3.Push(i + "," + j + "," + (n + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;
                                }
                            }
                        }
                    }
                }
            }

            // AYRIM // **************************************************************************************************************

            sayac1 = 0; sayac2 = 0; sayac3 = 0;
            tut1 = 0; tut2 = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_2[i, j, a] == 1)
                                {
                                    //Yatay 2liler

                                    for (int x = 0; x < 9; x++)
                                    {
                                        if (ihtimaller_2[i, x, a] == 1 && ihtimaller_2[i, x, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = x;
                                        }
                                        if (ihtimaller_2[i, x, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_2[i, x, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int y = 0; y < 9; y++)
                                        {
                                            if (y != a && y != k)
                                            {
                                                if (ihtimaller_2[i, tut1, y] != 0)
                                                {
                                                    ihtimaller_2[i, tut1, y] = 0;
                                                    i3.Push(i + "," + tut1 + "," + (y + 1));
                                                }
                                                if (ihtimaller_2[i, j, y] != 0)
                                                {
                                                    ihtimaller_2[i, j, y] = 0;
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;


                                    //3lüdeki 2liler

                                    if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 0; g < 3; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                                    { //Burası
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 3; g < 6; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 0; f < 3; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 3; f < 6; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                                    {
                                        for (int f = 6; f < 9; f++)
                                        {
                                            for (int g = 6; g < 9; g++)
                                            {
                                                if (ihtimaller_2[f, g, a] == 1 && ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac1++;
                                                    tut1 = f;
                                                    tut2 = g;
                                                }
                                                if (ihtimaller_2[f, g, a] == 1)
                                                {
                                                    sayac2++;
                                                }
                                                if (ihtimaller_2[f, g, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                        {
                                            for (int n = 0; n < 9; n++)
                                            {
                                                if (n != a && n != k)
                                                {
                                                    if (ihtimaller_2[tut1, tut2, n] != 0)
                                                    {
                                                        ihtimaller_2[tut1, tut2, n] = 0;
                                                        i3.Push(tut1 + "," + tut2 + "," + (n + 1));
                                                    }
                                                    if (ihtimaller_2[i, j, n] != 0)
                                                    {
                                                        ihtimaller_2[i, j, n] = 0;
                                                        i3.Push(i + "," + j + "," + (n + 1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;




                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1 && k != 9)
                        {
                            for (int a = k + 1; a < 9; a++)
                            {
                                if (ihtimaller_2[i, j, a] == 1)
                                {
                                    //Dikey 2liler
                                    for (int m = 0; m < 9; m++)
                                    {
                                        if (ihtimaller_2[m, j, a] == 1 && ihtimaller_2[m, j, k] == 1)
                                        {
                                            sayac1++;
                                            tut1 = m;

                                        }
                                        if (ihtimaller_2[m, j, a] == 1)
                                        {
                                            sayac2++;
                                        }
                                        if (ihtimaller_2[m, j, k] == 1)
                                        {
                                            sayac3++;
                                        }
                                    }
                                    if (sayac1 == 2 && sayac2 == 2 && sayac3 == 2)
                                    {
                                        for (int n = 0; n < 9; n++)
                                        {
                                            if (n != a && n != k)
                                            {
                                                if (ihtimaller_2[tut1, j, n] != 0)
                                                {
                                                    ihtimaller_2[tut1, j, n] = 0;
                                                    i3.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                                if (ihtimaller_2[i, j, n] != 0)
                                                {
                                                    ihtimaller_2[i, j, n] = 0;
                                                    i3.Push(tut1 + "," + j + "," + (n + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac1 = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                    tut1 = 0;
                                }
                            }
                        }
                    }
                }
            }

            // AYRIM // **************************************************************************************************************
        } //2Liler       

        public void button05_2(int startX, int startY)
        {
            int sayac = 0, sayac2 = 0, sayac3 = 0, sira = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = startY; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = startY; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }
            // AYRIM // €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = startX + 1; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX + 1; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM // €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

            // AYRIM2 // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = 0; i < startX; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            // AYRIM2 // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

            // AYRIM3 // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

            sayac = 0; sayac2 = 0; sayac3 = 0; sira = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = 0; j < startY; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {//Yatay
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[f, g, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[f, a, k] != 0)
                                                {
                                                    ihtimaller_2[f, a, k] = 0;
                                                    i3.Push(f + "," + a + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }
                        }
                    }
                }
            }

            sayac = 0;
            sayac2 = 0;
            sayac3 = 0;

            for (int i = startX; i <= startX; i++)
            {
                for (int j = 0; j < startY; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ihtimaller_2[i, j, k] == 1)
                        {

                            if (i >= 0 && i <= 2 && j >= 0 && j <= 2)
                            {

                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {

                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;

                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 0 && j <= 2)
                            {
                                for (int f = 0; f < 3; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 0; b < 3; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }
                            }

                            else if (i >= 0 && i <= 2 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 6; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                ihtimaller_2[a, f, k] = 0;
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 3 && j <= 5)
                            {
                                for (int f = 3; f < 6; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 3; b < 6; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 0 && i <= 2 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 0; g < 3; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 0; a < 3; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 3; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }

                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 3 && i <= 5 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 3; g < 6; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 3; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 3; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                            for (int a = 6; a < 9; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }

                            else if (i >= 6 && i <= 8 && j >= 6 && j <= 8)
                            {
                                for (int f = 6; f < 9; f++)
                                {
                                    for (int g = 6; g < 9; g++)
                                    {
                                        if (ihtimaller_2[g, f, k] == 1)
                                        {
                                            sayac++;
                                        }
                                    }
                                    if (sayac >= 2)
                                    {
                                        for (int a = 6; a < 9; a++)
                                        {
                                            for (int b = 6; b < 9; b++)
                                            {
                                                if (ihtimaller_2[a, b, k] == 1)
                                                {
                                                    sayac3++;
                                                }
                                            }
                                        }
                                        if (sayac == sayac3)
                                        {
                                            for (int a = 0; a < 6; a++)
                                            {
                                                if (ihtimaller_2[a, f, k] != 0)
                                                {
                                                    ihtimaller_2[a, f, k] = 0;
                                                    i3.Push(a + "," + f + "," + (k + 1));
                                                }
                                            }
                                        }
                                    }
                                    sayac = 0;
                                    sayac2 = 0;
                                    sayac3 = 0;
                                }

                            }


                        }
                    }
                }
            }

            // AYRIM3 // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||


        }

        int processingCount = 0;
        public void ThreadCall(int startX, int startY)
        {
            processingCount = 0;
            while (bittiMi2(sudoku) == 0)
            {
                change = 0;
                button03(startX, startY);
                while (change == 0)
                {
                    change = 1;
                    button04(startX, startY);
                    button05(startX, startY);
                    processingCount++;
                    if (bittiMi2(sudoku) == 1 || change == 1)
                        break;
                    
                }
                if (change > 0)
                    processingCount = 0;
                if (bittiMi2(sudoku) == 1 || processingCount >= 5)
                    break;
            }           
        }
        
        public void ThreadCall_1(int startX, int startY)
        {
            processingCount = 0;
            while (bittiMi2(sudoku_1) == 0)
            {
                change = 0;
                button03_1(startX, startY);
                while (change == 0)
                {
                    change = 1;
                    button04_1(startX, startY);
                    button05_1(startX, startY);
                    processingCount++;
                    if (bittiMi2(sudoku_1) == 1 || change == 1)
                        break;
                    
                }
                if (change > 0)
                    processingCount = 0;
                if (bittiMi2(sudoku_1) == 1 || processingCount >= 5)
                    break;
            }

            
        }
        public void ThreadCall_2(int startX, int startY)
        {
            processingCount = 0;
            while (bittiMi2(sudoku_2) == 0)
            {
                change = 0;
                button03_2(startX, startY);
                while (change == 0)
                {
                    change = 1;
                    button04_2(startX, startY);
                    button05_2(startX, startY);
                    processingCount++;
                    if (bittiMi2(sudoku_2) == 1 || change == 1)
                        break;
                    
                }
                if (change > 0)
                    processingCount = 0;
                if (bittiMi2(sudoku_2) == 1 || processingCount >= 5)
                    break;
            }

            
        }

        public int bittiMi2(int[,] sudokux)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudokux[i, j] == 0)
                    {
                        return 0;
                    }
                }
            }
            return 1;
        }
        List<int> uretilenler = new List<int>();
        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            button01();
            button02();
            for (int i = 0; i < 9; i++)
            {
                for (int k = 0; k < 9; k++)
                {
                    sudoku_x[i, k] = 0;
                }
            }
            Random rnd = new Random();
            int rnd1 = rnd.Next(0, 9);
            uretilenler.Add(rnd1);
            int rnd2 = rnd.Next(0, 9);
            while (uretilenler.Contains(rnd2))
            {
                rnd2 = rnd.Next(0, 9);
            }
            uretilenler.Add(rnd2);
            int rnd3 = rnd.Next(0, 9);
            while (uretilenler.Contains(rnd3))
            {
                rnd3 = rnd.Next(0, 9);
            }
            uretilenler.Add(rnd3);
         
            Thread th1 = new Thread(() => ThreadCall(rnd.Next(0, 9), rnd.Next(0, 9)));
            Thread th2 = new Thread(() => ThreadCall_1(rnd.Next(0, 9), rnd.Next(0, 9)));
            Thread th3 = new Thread(() => ThreadCall_2(rnd.Next(0, 9), rnd.Next(0, 9)));

            th1.Start();
            th2.Start();
            th3.Start();

            uretilenler.Clear();
            
            if(bittiMi2(sudoku) == 1)
            {
                th2.Abort();
                th3.Abort();
            }
            else if (bittiMi2(sudoku_1) == 1)
            {
                th1.Abort();
                th3.Abort();
            }
            else if (bittiMi2(sudoku_2) == 1)
            {
                th1.Abort();
                th2.Abort();
            }

            Thread.Sleep(200);

            if (!th1.IsAlive)
            {
                MessageBox.Show("1.thread bitti ve " + (s1.Count + i1.Count) + " adımda sudoku çözüldü.");
                th2.Abort();
                th3.Abort();

                hangiThread = 1;
                button2.Visible = true;
                ekranaYaz();
            }

            
            else if (!th2.IsAlive)
            {
                MessageBox.Show("2.thread bitti ve " + (s2.Count + i2.Count) + " adımda sudoku çözüldü.");
                th1.Abort();
                th3.Abort();

                hangiThread = 2;
                button2.Visible = true;
                ekranaYaz();
            }

             
            else if (!th3.IsAlive)
            {
                MessageBox.Show("3.thread bitti ve " + (s3.Count + i3.Count) + " adımda sudoku çözüldü.");
                th1.Abort();
                th2.Abort();
                hangiThread = 3;
                button2.Visible = true;
                ekranaYaz();
            }
            dosyayaYaz("s1", s1);
            dosyayaYaz("s2", s2);
            dosyayaYaz("s3", s3);
            dosyayaYaz("i1", i1);
            dosyayaYaz("i2", i2);
            dosyayaYaz("i3", i3);
        }

        public void dosyayaYaz(string dosyaAdi, Stack<string> degerler) {

            string dosyaYol = Application.StartupPath + @"\adimlar\" + dosyaAdi + ".txt";

            if (File.Exists(dosyaYol))
            {
                File.Delete(dosyaYol);
            }
            FileStream fs = new FileStream(dosyaYol, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);

            foreach (var item in degerler)
            {
                sw.WriteLine(item);
                sw.Flush();
            }
            sw.Close();
            fs.Close();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
        int deger2 = 0;
        private void ekranaYaz()
        {
            if (hangiThread == 1)
            {
                    foreach (var item in s1)
                    {
                        
                        var splitString = item.Split(',');

                        deger2 = 0;
                        foreach (Control kontrol in this.Controls)
                        {
                            if (kontrol is TextBox)
                            {
                               if (Convert.ToInt32(splitString[0]) * 9 + Convert.ToInt32(splitString[1]) == deger2)
                               {
                                   kontrol.Text = splitString[2];
                                   var t = Task.Delay(300);
                                   t.Wait();
                                   break;
                               }
                               deger2++;                                                        
                            }
                        }
                    }
            }
            else if (hangiThread == 2)
            {
                foreach (var item in s2)
                {
                    var splitString = item.Split(',');

                    deger2 = 0;
                    foreach (Control kontrol in this.Controls)
                    {
                        if (kontrol is TextBox)
                        {
                            if (Convert.ToInt32(splitString[0]) * 9 + Convert.ToInt32(splitString[1]) == deger2)
                            {
                                kontrol.Text = splitString[2];
                                var t = Task.Delay(300);
                                t.Wait();
                                break;
                            }
                            deger2++;
                        }
                    }
                }
            }
            else if (hangiThread == 3)
            {
                foreach (var item in s3)
                {
                    var splitString = item.Split(',');

                    deger2 = 0;
                    foreach (Control kontrol in this.Controls)
                    {
                        if (kontrol is TextBox)
                        {
                            if (Convert.ToInt32(splitString[0]) * 9 + Convert.ToInt32(splitString[1]) == deger2)
                            {
                                kontrol.Text = splitString[2];
                                var t = Task.Delay(300);
                                t.Wait();
                                break;
                            }
                            deger2++;
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (hangiThread == 1) {
                Form2 frm2 = new Form2(s2);
                Form3 frm3 = new Form3(s3);

                frm2.Show();
                frm3.Show();
            }
            else if (hangiThread == 2)
            {
                Form2 frm2 = new Form2(s1);
                Form3 frm3 = new Form3(s3);

                frm2.Show();
                frm3.Show();
            }
            else if (hangiThread == 3)
            {
                Form2 frm2 = new Form2(s1);
                Form3 frm3 = new Form3(s2);

                frm2.Show();
                frm3.Show();
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
