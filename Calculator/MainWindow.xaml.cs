using MongoDB.Driver;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double lastnumber, result,newnumber;
        public Selectedoperator? selectedoperator,firstoperator=null,secondoperator=null;
        public bool temp=false,check=false,sample=false;
        private simp simp;
        
        public MainWindow()
        {
            InitializeComponent();
            acbutton.Click += Acbutton_Click;
            negbutton.Click += Negbutton_Click;
            perbutton.Click += Perbutton_Click;
            equalbutton.Click += Equalbutton_Click;
            simp = new();
        }

        private void Equalbutton_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(resultlabel.Content.ToString(), out newnumber);
            switch(selectedoperator)
            {
                case Selectedoperator.sumbutton:
                    result = simp.sum(lastnumber, newnumber);
                    break;
                case Selectedoperator.subbutton:
                    result = simp.sub(lastnumber, newnumber);
                    break;
                case Selectedoperator.mulbutton:
                    result = simp.mul(lastnumber, newnumber);
                    break;
                case Selectedoperator.divbutton:
                    result = simp.div(lastnumber, newnumber);
                    break;
            }
            resultlabel.Content = result.ToString();
            lastnumber = result;
            check = false;
        }


        


        private void Perbutton_Click(object sender, RoutedEventArgs e)
        {
            if (resultlabel.Content.ToString() == "0")
            {
                return;
            }
            else
            {
                double.TryParse(resultlabel.Content.ToString(), out lastnumber);
                lastnumber /= 100;
                resultlabel.Content = lastnumber.ToString();
            }
        }

        private void Negbutton_Click(object sender, RoutedEventArgs e)
        {
            if (resultlabel.Content.ToString() == "0")
            {
                return;
            }
            else
            {
                double.TryParse(resultlabel.Content.ToString(), out lastnumber);
                lastnumber *= -1;
                resultlabel.Content = lastnumber.ToString();
            }
        }

        private void numbutton_Click(object sender, RoutedEventArgs e)
        {
            int val = 0;
            
            //sender==sevenbutton way is too big code 
            val = int.Parse((sender as Button).Content.ToString());
            
            if (resultlabel.Content.ToString() == "0"||temp==true)
            {
                
                resultlabel.Content = $"{val}";
                temp = false;
                
            }
            else
            {
                resultlabel.Content = $"{resultlabel.Content}{val}";
               
            }
            if(sample==true)
                toplabel.Content= $"{toplabel.Content}{val}";
            if (sample == false)
            {
                toplabel.Content = val.ToString();
                sample = true;
            }

        }

        private void operation_Click(object sender, RoutedEventArgs e)
        {
           
            if (check == false)
            {
                if (double.TryParse(resultlabel.Content.ToString(), out lastnumber))
                {
                    resultlabel.Content = (sender as Button).Content.ToString();
                    
                    temp = true;
                }
            }
            if (check == true)
            {
                if (double.TryParse(resultlabel.Content.ToString(), out newnumber))
                {
                    resultlabel.Content = (sender as Button).Content.ToString();
                    
                    temp = true;
                }
            }

            toplabel.Content = $"{toplabel.Content}{resultlabel.Content}";
            if (sender == sumbutton)
                selectedoperator = Selectedoperator.sumbutton;
            if (sender == subbutton)
                selectedoperator = Selectedoperator.subbutton;
            if (sender == divbutton)
                selectedoperator = Selectedoperator.divbutton;
            if (sender == mulbutton)
                selectedoperator = Selectedoperator.mulbutton;
            if (check == true)
            {
                operating();
                firstoperator = selectedoperator;
            }
            
            if (check == false)
            {
                firstoperator = selectedoperator;
                check = true;
            }


        }

        private void operating()
        {
            
            switch (firstoperator)
            {
                case Selectedoperator.sumbutton:
                    result = simp.sum(lastnumber, newnumber);
                    break;
                case Selectedoperator.subbutton:
                    result = simp.sub(lastnumber, newnumber);
                    break;
                case Selectedoperator.mulbutton:
                    result = simp.mul(lastnumber, newnumber);
                    break;
                case Selectedoperator.divbutton:
                    result = simp.div(lastnumber, newnumber);
                    break;
            }
            lastnumber = result;
            check = false;
            
        }

        private void pointbutton_Click(object sender, RoutedEventArgs e)
        {
            if (resultlabel.Content.ToString().Contains(".")){
                
            }
            else
            {
                resultlabel.Content = $"{resultlabel.Content}{"."}";
               
            }
        }

        private void Acbutton_Click(object sender, RoutedEventArgs e)
        {
            resultlabel.Content = "0";
            check = false;
            sample = false;
        }

        public enum Selectedoperator
        {
            sumbutton,
            subbutton,
            mulbutton,
            divbutton
        }
        
    }
    public class simp
    {
      
        public double sum(double n1,double n2)
        {
            var db = new OperationDb
            {
                Number1 = n1,
                Number2 = n2,
                Operator = "+",
                Result = n1 + n2,
                CreatedOn = DateTime.Now
            };
            Insert2Db(db, "Additions");
            return n1 + n2;
        }
        public double sub(double n1, double n2)
        {
            var db = new OperationDb
            {
                Number1 = n1,
                Number2 = n2,
                Operator = "-",
                Result = n1 - n2,
                CreatedOn = DateTime.Now
            };
            Insert2Db(db, "Subtractions");
            
            return n1 - n2;
        }
        public double div(double n1, double n2)
        {
            if (n2 == 0)
            {
                MessageBox.Show("Div by 0 not supported", "Wrong operation", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
            else
            {
                var db = new OperationDb
                {
                    Number1 = n1,
                    Number2 = n2,
                    Operator = "/",
                    Result = n1 / n2,
                    CreatedOn = DateTime.Now
                };
                Insert2Db(db, "Divisions");
                return n1 / n2;
            }
        }
        public double mul(double n1, double n2)
        {
            var db = new OperationDb
            {
                Number1 = n1,
                Number2 = n2,
                Operator = "*",
                Result = n1 * n2,
                CreatedOn = DateTime.Now
            };
            Insert2Db(db, "Multiplications");
            return n1 *n2;
        }

        const string MongoDBConnectionString = "mongodb://localhost";

        public IMongoCollection<OperationDb> GetCollections(string collectionName)
        {
            var client = new MongoClient(MongoDBConnectionString);
            var database = client.GetDatabase("Calculator");
            return database.GetCollection<OperationDb>(collectionName);

        }

        private void Insert2Db(OperationDb operationDb, string collectionName)
        {
            var collections = GetCollections(collectionName);
            collections.InsertOne(operationDb);
        }
    }
}





