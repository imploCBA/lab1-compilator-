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
using System.IO;


namespace lab1_compilator_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int fileCounter = 0;
        private int activeFile = 0;
        private bool ctrAct = false;
        private int q = 0;
        private List<WorkPlace> frames = new List<WorkPlace>();
        private List<Button> buttons = new List<Button>();
        private TextBox txtEditor;
        private TextBox countEdit;
        public MainWindow()
        {
            InitializeComponent();

            lang.Text = "Язык ввода: " + InputLanguageManager.Current.CurrentInputLanguage.DisplayName;

            
            System.Windows.Input.InputLanguageManager.Current.InputLanguageChanged += new InputLanguageEventHandler((sender, e) =>
            {
                lang.Text = "Язык ввода: " + e.NewLanguage.DisplayName;
            });

            //filesCount.Text = "Количество открытых файлов: " + fileCounter;

            buttons.AddRange(Enumerable.Repeat(default(Button), 1));
            buttons[fileCounter] = new Button();
            buttons[fileCounter].Margin = new Thickness(5, 1, 0, 1);
            {
                int z = fileCounter + 1;
                string s = "Новый файл " + z;
                buttons[fileCounter].Content = s;
                buttons[fileCounter].Name = "id"+fileCounter;
                buttons[fileCounter].Click += new RoutedEventHandler(ChangePage_Click);
            }
            FileMenu.Children.Add(buttons[fileCounter]);
            frames.AddRange(Enumerable.Repeat(default(WorkPlace), 1));
            frames[fileCounter] = new WorkPlace();
            wpFrame.Content = frames[fileCounter];
            txtEditor = frames[fileCounter].getTxt();
            countEdit = frames[fileCounter].getCount();
            fileCounter++;
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            buttons.AddRange(Enumerable.Repeat(default(Button), 1));
            buttons[fileCounter] = new Button();
            buttons[fileCounter].Margin = new Thickness(10, 1, 0, 1);
            {
                int z = fileCounter + 1;
                string s = "Новый файл " + z;
                buttons[fileCounter].Content = s;
                buttons[fileCounter].Name = "id"+fileCounter;
                buttons[fileCounter].Click += new RoutedEventHandler(ChangePage_Click);
            }
            FileMenu.Children.Add(buttons[fileCounter]);
            frames.AddRange(Enumerable.Repeat(default(WorkPlace), 1));
            frames[fileCounter] = new WorkPlace(); 
            wpFrame.Content = frames[fileCounter];
            txtEditor = frames[fileCounter].getTxt();
            countEdit = frames[fileCounter].getCount();
            fileCounter++;
            activeFile = fileCounter-1;
            frames[activeFile]._fileName = null;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {

            // OpenFileDialog
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "C# файлы (*.cs)|*.cs";

            if (dialog.ShowDialog() == true)
            {

                buttons.AddRange(Enumerable.Repeat(default(Button), 1));
                buttons[fileCounter] = new Button();
                buttons[fileCounter].Margin = new Thickness(10, 1, 0, 1);
                {
                    int z = fileCounter + 1;
                    string s = "Новый файл " + z;
                    buttons[fileCounter].Content = s;
                    buttons[fileCounter].Name = "id" + fileCounter;
                    buttons[fileCounter].Click += new RoutedEventHandler(ChangePage_Click);
                }
                FileMenu.Children.Add(buttons[fileCounter]);
                frames.AddRange(Enumerable.Repeat(default(WorkPlace), 1));
                frames[fileCounter] = new WorkPlace();
                wpFrame.Content = frames[fileCounter];
                txtEditor = frames[fileCounter].getTxt();
                countEdit = frames[fileCounter].getCount();
                fileCounter++;
                activeFile = fileCounter - 1;


                frames[activeFile]._fileName = dialog.FileName;
                txtEditor.Text = File.ReadAllText(frames[activeFile]._fileName);
                buttons[activeFile].Content = frames[activeFile]._fileName.Split('\\').Last();
            }
        }
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (frames[activeFile]._fileName == null)
            {
                // SaveFileDialog
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Filter = "C# файлы (*.cs)|*.cs";

                if (dialog.ShowDialog() == true)
                {
                    frames[activeFile]._fileName = dialog.FileName;
                }

                buttons[activeFile].Content = frames[activeFile]._fileName.Split('\\').Last();
            }

            if (frames[activeFile]._fileName != null)
            {
                File.WriteAllText(frames[activeFile]._fileName, txtEditor.Text);
            }
        }
        private void SaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            // SaveFileDialog
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "C# файлы (*.cs)|*.cs";

            if (dialog.ShowDialog() == true)
            {
                frames[activeFile]._fileName = dialog.FileName;
                File.WriteAllText(frames[activeFile]._fileName, txtEditor.Text);
                buttons[activeFile].Content = frames[activeFile]._fileName.Split('\\').Last();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (frames[activeFile].undoStack.Count > 0)
            {
                string previousText = frames[activeFile].undoStack.Pop();
                frames[activeFile].redoStack.Push(txtEditor.Text);
                txtEditor.Text = previousText;
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (frames[activeFile].redoStack.Count > 0)
            {
                string nextText = frames[activeFile].redoStack.Pop();
                frames[activeFile].undoStack.Push(txtEditor.Text);
                txtEditor.Text = nextText;
            }
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.Cut();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.Paste();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.SelectedText = "";
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.SelectAll();
        }

        private void Task_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Постановка задачи" click.
        }

        private void Grammar_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Грамматика" click.
        }

        private void Classification_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Классификация грамматики" click.
        }

        private void Method_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Метод анализа" click.
        }

        private void Diagnostic_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Диагностика и нейтрализация ошибок" click.
        }

        private void Example_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Тестовый пример" click.
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Список литературы" click.
        }

        private void Code_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to handle "Исходный код программы" click.
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement code to compile and run the C# code in the editor.
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            WindowHelp help = new WindowHelp();
            help.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            WindowAbout about = new WindowAbout();
            about.Show();
        }

       
     
        private void FontSize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePage_Click(object sender, RoutedEventArgs e)
        {
            string s = (sender as Button).Name;
            s = s.Remove(0, 2);
            int id = Int32.Parse(s);
            activeFile = id;
            wpFrame.Content = frames[id];
            txtEditor = frames[id].getTxt();
            countEdit = frames[id].getCount();

        }



        private void Language_ClickRu(object sender, RoutedEventArgs e)
        {
            UpdateMenuTexts("ru");
        }

        private void Language_ClickEn(object sender, RoutedEventArgs e)
        {
            UpdateMenuTexts("en");
        }

        private void UpdateMenuTexts(string language)
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (language)
            {
                case "ru":
                    dict.Source = new Uri("Resources_ru.resx", UriKind.Relative);
                    break;
                case "en":
                    dict.Source = new Uri("Resources.en.resx", UriKind.Relative);
                    break;
                default:
                    return;
            }

            foreach (var menuItem in FindVisualChildren<MenuItem>(menuMain))
            {
                if (menuItem.Name != null)
                {
                    string resourceName = menuItem.Name.Replace("menuItem", "");
                    if (dict.Contains(resourceName))
                    {
                        menuItem.Header = dict[resourceName];
                    }
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }



        private void Hotkey(object sender, KeyEventArgs e)
        {
            q++;
            string s = "жмак| " + e.Key;
            int z = q * q;
            Data data = new Data() { Counter = q, Description = s, source = z.ToString() };
            
           
            lbxEvents.Items.Add(data);



            if (e.Key.ToString() == "OemPlus" && ctrAct == true)
            {
                txtEditor.FontSize += 1;
                lbxEvents.FontSize += 1;
                countEdit.FontSize += 1; 
            }
            if (e.Key.ToString() == "OemMinus" && ctrAct == true && txtEditor.FontSize > 1)
            {
                txtEditor.FontSize -= 1;
                lbxEvents.FontSize -= 1;
                countEdit.FontSize -= 1;
            }
           
            if (e.Key.ToString() == "LeftCtrl" || e.Key.ToString() == "OemPlus" || e.Key.ToString() == "OemMinus")
            {
                ctrAct = true;
            }
            else
            {
                ctrAct = false;
            } 
           
        }

        class Data
        {
            public int Counter { get; set; }
            public string Description { get; set; }
            public string source { get; set; }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
