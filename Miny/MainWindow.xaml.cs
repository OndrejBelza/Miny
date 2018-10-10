using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Miny
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<cor> Cordinates = new List<cor>();
        List<Button> BtnList = new List<Button>();
        class cor
        {
            public int row { get; set; }
            public int col { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            GenerateBombsCordinates();
            GenereateGridButtons();
            RegenerateGrid();








        }
        Random rnd = new Random();

        private void RegenerateGrid()
        {
            test.ColumnDefinitions.Clear();
            test.RowDefinitions.Clear();
            int DH = int.Parse(Height_.Text);
            int DW = int.Parse(Width_.Text);

            while (DH != 0)
            {
                RowDefinition gridRow1 = new RowDefinition();
                test.RowDefinitions.Add(gridRow1);
                DH--;
            }
            while (DW != 0)
            {
                ColumnDefinition gridRow1 = new ColumnDefinition();
                test.ColumnDefinitions.Add(gridRow1);
                DW--;
            }

            foreach (PlayGroundButton btn in BtnList)
            {
                PlayGroundButton newbtn = btn;
                Grid.SetColumn(newbtn, btn.Col_);
                Grid.SetRow(newbtn, btn.Row_);
                test.Children.Add(newbtn);
            }
           
           
        }
        private void PlayGroundButtonClick(object sender, EventArgs e)
        {
            var BombCount = 0;
            PlayGroundButton button = sender as PlayGroundButton;
            int btn_r = button.Row_;
            int btn_c = button.Col_;
            bool RowMatch = false;
            bool ColMatch = false;
            bool IsBomb = false;
            foreach(cor test in Cordinates)
            {
                RowMatch = false;
                ColMatch = false;
                if (btn_r == test.row)
                {
                    RowMatch = true;
                }
                if (btn_c == test.col)
                {
                    ColMatch = true;
                }
                if(RowMatch && ColMatch)
                {
                    IsBomb = true;
                }
            }
            if (IsBomb)
            {
                MessageBox.Show("Bomb?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else
            {
                
                button.IsEnabled = false;
                foreach(PlayGroundButton btn in BtnList)
                {
                    if(btn_c == btn.Col_)
                    {
                        if (btn_r == btn.Row_ + 1)
                        {
                            if (btn.IsBomb)
                            {
                                BombCount++;
                            }
                            else
                            {
                                btn.IsEnabled = false;
                            }
                        }else if(btn_r == btn.Row_ - 1)
                        {
                            if (btn.IsBomb)
                            {
                                BombCount++;
                            }
                            else
                            {
                                btn.IsEnabled = false;
                            }
                        }
                    }
                    
                    

                    
                }
            }
            button.Content = BombCount.ToString();
           
        }
        private void GenerateBombsCordinates()
        {
            int DH = int.Parse(Height_.Text);
            int DW = int.Parse(Width_.Text);
            int BombNum = int.Parse(BombNum_.Text);
            bool Fine;


            while (BombNum != 0)
            {
                bool done = false;
                while (done == false)
                {

                    int Row = rnd.Next(0, DH);
                    int Col = rnd.Next(0, DW);
                    cor curcor = new cor();
                    curcor.col = Col;
                    curcor.row = Row;

                    if (Cordinates.Count() == 0)
                    {
                        Cordinates.Add(curcor);
                        BombNum--;
                        done = true;
                    }
                    else
                    {
                        Fine = true;
                        foreach (cor test in Cordinates)
                        {
                            if (test == curcor)
                            {
                                Fine = false;

                            }

                        }
                        if (Fine)
                        {
                            Cordinates.Add(curcor);
                            BombNum--;
                            done = true;
                        }
                    }








                }


            }
        }

        private void GenereateGridButtons()
        {
           

            

            

            int DH = int.Parse(Height_.Text);
            int DW = int.Parse(Width_.Text);
            

            int count = 1;
            for (int i = 0; i < DH; i++)
            {
                for (int j = 0; j < DW; j++)
                {

                    PlayGroundButton MyControl1 = new PlayGroundButton();
                    MyControl1.Content = "Btn" + count.ToString();
                    MyControl1.Name = "Btn" + count.ToString();
                    MyControl1.IsBomb = false;
                    MyControl1.Row_ = i;
                    MyControl1.Col_ = j;
                    MyControl1.Click += new RoutedEventHandler(PlayGroundButtonClick);

                    


                    foreach (cor bombcor in Cordinates)
                    {
                        if(bombcor.row == i)
                        {
                            if(bombcor.col == j)
                            {
                                MyControl1.IsBomb = true;
                                MyControl1.Content = "Bomb!";
                            }
                        }
                    }



                    
                    BtnList.Add(MyControl1);
                    count++;
                }

            }
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GenerateBombsCordinates();
            GenereateGridButtons();
            RegenerateGrid();
        }
    }
}
