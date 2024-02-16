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

namespace lab1_compilator_
{
    /// <summary>
    /// Логика взаимодействия для WorkPlace.xaml
    /// </summary>
    public partial class WorkPlace : Page
    {
        public string _fileName;
        public Stack<string> undoStack = new Stack<string>();
        public Stack<string> redoStack = new Stack<string>();
        private int count;
        public WorkPlace()
        {
            InitializeComponent();
            for (count = 1; count < 70; count++)
            {
                Counter.Text += count + "\n";
            }
        }

        public TextBox getTxt()
        {
            return txtEditor;
        }
        public TextBox getCount()
        {
            return Counter;
        }

        private void TextChanged_Handler(object sender, TextChangedEventArgs e)
        {
            undoStack.Push(txtEditor.Text);
            redoStack.Clear();
        }

        private void txtEditor_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double q = txtEditor.VerticalOffset;
            Counter.ScrollToVerticalOffset(q);
        }
    }
}
