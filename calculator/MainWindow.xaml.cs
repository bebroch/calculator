using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

namespace calculator
{
    public partial class MainWindow : Window
    {

        WinLog Log;
        private int _minSizeWidth = 289;
        private int _minSizeHeight = 443;

        public MainWindow()
        {
            Log = new WinLog(this);
            InitializeComponent();
            this.Width = _minSizeWidth;
            this.Height = _minSizeHeight;
            firstColumn.Width = new GridLength(100, GridUnitType.Star);
            secondColumn.Width = new GridLength(0, GridUnitType.Star);

            ButtonClickArgument();

            foreach (var bt in Log.GetButtonList())
            {
                if (bt.FontSize == 12)
                    bt.FontSize = Log.minSizeNum;
                else if (bt.FontSize == 18)
                    bt.FontSize = Log.minSizeOper;
            }
        }

        private void ButtonClickArgument()
        {
            KeyDown += (o, e) => ButtonClick(e.Key);

            btnEnter.Click += (o, e) => ButtonClick(Key.Enter);
            btnClear.Click += (o, e) => Log.Clear();

            btnZero.Click += (o, e) => ButtonClick(Key.D0);
            btnOne.Click += (o, e) => ButtonClick(Key.D1);
            btnTwo.Click += (o, e) => ButtonClick(Key.D2);
            btnThree.Click += (o, e) => ButtonClick(Key.D3);
            btnFour.Click += (o, e) => ButtonClick(Key.D4);
            btnFive.Click += (o, e) => ButtonClick(Key.D5);
            btnSix.Click += (o, e) => ButtonClick(Key.D6);
            btnSeven.Click += (o, e) => ButtonClick(Key.D7);
            btnEight.Click += (o, e) => ButtonClick(Key.D8);
            btnNine.Click += (o, e) => ButtonClick(Key.D9);

            btnPlus.Click += (o, e) => ButtonClick(Key.Add);
            btnMinus.Click += (o, e) => ButtonClick(Key.Subtract);
            btnPow.Click += (o, e) => ButtonClick(Key.Multiply);
            btnRadical.Click += (o, e) => ButtonClick(Key.Divide);

            btnPlusAndMinus.Click += (o, e) => Log.PutPlusAndMinus();
            btnComma.Click += (o, e) => ButtonClick(Key.Decimal);

            btnPow.Click += (o, e) => Log.Pow(2);
            btnRadical.Click += (o, e) => Log.Pow(0.5f);
        }

        private int _width = 1074; // Минимальный размер для большого размера font size кнопок
        private int _height = 950;
        private int showHistoryWidth = 750; // Минимальный размер для открытия боковой панели "Журнал"

        private SizeButton _sizeApplied = SizeButton.AppliedOne; // 
        private Panel _column = Panel.NotApplied;

        private void windowResized(object sender, SizeChangedEventArgs e)
        {
            double width = this.Width;
            double height = this.Height;

            if (_sizeApplied == SizeButton.AppliedTwo && (width > _width || height > _height)) // При определённом значении изменять размер кнопок
            {
                Log.ChangeSizeButtonUp();
                _sizeApplied = SizeButton.AppliedOne;
            }
            else if (_sizeApplied == SizeButton.AppliedOne && width < _width || height < _height)
            {
                Log.ChangeSizeButtonDown();
                _sizeApplied = SizeButton.AppliedTwo;
            }


            if (_column == Panel.NotApplied && width > showHistoryWidth) // Показывать боковую панель "Журнал" при 750 пикселях
            {
                IncreaseJournalPanel();
                _column = Panel.Applied;
            }
            else if (_column == Panel.Applied && width < showHistoryWidth)
            {
                DecreaseJournalPanel();
                _column = Panel.NotApplied;
            }
        }

        private async void IncreaseJournalPanel() // Анимация увеличение боковой панели (Журнала)
        {
            for (int i = 0; i <= 30; i++)
            {
                secondColumn.Width = new GridLength(i, GridUnitType.Star);
                if (i % 10 == 0)
                    await Task.Delay(5);
            }
            
        }

        private async void DecreaseJournalPanel() // Анимация уменьшение боковой панели (Журнала)
        {
            for (int i = 30; i >= 0; i--)
            {
                secondColumn.Width = new GridLength(i, GridUnitType.Star);
                if (i % 10 == 0)
                    await Task.Delay(5);
            }
        }

        private void ButtonClick(Key e)
        {
            switch (e)
            {
                case Key.D0: // ноль
                case Key.NumPad0:
                case Key.D1: // один
                case Key.NumPad1:
                case Key.D2: // два
                case Key.NumPad2:
                case Key.D3: // три
                case Key.NumPad3:
                case Key.D4: // четыре
                case Key.NumPad4:
                case Key.D5: // пять
                case Key.NumPad5:
                case Key.D6: // шесть
                case Key.NumPad6:
                case Key.D7: // семь
                case Key.NumPad7:
                case Key.D8: // восемь
                case Key.NumPad8:
                case Key.D9: // девять
                case Key.NumPad9:
                    Log.AddNum(Log.KeyToNumDict[e]);
                    break;

                case Key.Add: // плюс
                case Key.Subtract: // минус
                case Key.Multiply: // умножить
                case Key.Divide: // разделить
                    Log.AddOperation(Log.KeyToOperationDict[e]);
                    break;

                case Key.Enter: // равно
                    Log.CountingInEqual();
                    break;

                case Key.Decimal: // запятая
                    Log.PutComma();
                    break;
            }
        }
    }
}



/*

namespace calculator
{
    public partial class MainWindow : Window
        {
        FormLog Log;
        private int _minSizeWidth = 289;
        private int _minSizeHeight = 443;

        public MainWindow()
        {
            Log = new FormLog(this);
            InitializeComponent();
            this.Width = _minSizeWidth;
            this.Height = _minSizeHeight;
            FullForm.ColumnStyles[1].Width = 0;

            foreach (var bt in NumberPanel.Controls.OfType<Button>())
            {
                if (bt.Font.Size == 12)
                    bt.Font = new Font(bt.Font.Name, Log.minSizeNum);
                else if (bt.Font.Size == 18)
                    bt.Font = new Font(bt.Font.Name, Log.minSizeOper);
            }

        }

        private void Equals_Click(object sender, EventArgs e) => Log.CountingInEqual(); // Кнопка равно
        private void Elevate_Click(object sender, EventArgs e) => TextQuestion.Text = Math.Pow(Convert.ToSingle(TextQuestion.Text), 2).ToString(); // Кнопка

        private void Radical_Click(object sender, EventArgs e) => TextQuestion.Text = Math.Pow(Convert.ToSingle(TextQuestion.Text), 0.5f).ToString(); // Кнопка
        //{
        //    string lineQ = TextQuestion.Text;

        //    if (lineQ.IndexOf("-") == -1)
        //        TextQuestion.Text = Math.Pow(Convert.ToSingle(lineQ), 0.5f).ToString();
        //}

        private void Divide_Click(object sender, EventArgs e) => Log.AddOperation(Operation.Divide);
        private void Multiply_Click(object sender, EventArgs e) => Log.AddOperation(Operation.Multiply);
        private void Minus_Click(object sender, EventArgs e) => Log.AddOperation(Operation.Minus);
        private void Plus_Click(object sender, EventArgs e) => Log.AddOperation(Operation.Plus);
        private void Delete_Click(object sender, EventArgs e) => Log.Clear();

        private void Comma_Click(object sender, EventArgs e) => Log.PutComma();

        private void PlusOrMinus_Click(object sender, EventArgs e)
        {
            string lineQ = TextQuestion.Text;

            if (lineQ != "0" && lineQ.IndexOf("-") == -1)
                lineQ = "-" + lineQ;
            else
                lineQ = lineQ.Replace("-", "");

            TextQuestion.Text = lineQ;
        }


        private void Num0_Click(object sender, EventArgs e) => Log.AddNum("0"); // Нажатие каждой кнопки
        private void Num1_Click(object sender, EventArgs e) => Log.AddNum("1");
        private void Num2_Click(object sender, EventArgs e) => Log.AddNum("2");
        private void Num3_Click(object sender, EventArgs e) => Log.AddNum("3");
        private void Num4_Click(object sender, EventArgs e) => Log.AddNum("4");
        private void Num5_Click(object sender, EventArgs e) => Log.AddNum("5");
        private void Num6_Click(object sender, EventArgs e) => Log.AddNum("6");
        private void Num7_Click(object sender, EventArgs e) => Log.AddNum("7");
        private void Num8_Click(object sender, EventArgs e) => Log.AddNum("8");
        private void Num9_Click(object sender, EventArgs e) => Log.AddNum("9");

        private int _width = 1074; // Минимальный размер для большого размера font size кнопок
        private int _height = 950;
        private int showHistoryWidth = 750; // Минимальный размер для открытия боковой панели "Журнал"

        private SizeButton _sizeApplied = SizeButton.AppliedOne; // 
        private Panel _column = Panel.NotApplied;

        private void Form1_Resize(object sender, EventArgs e)
        {
            int width = this.Size.Width;
            int height = this.Size.Height;

            if (_sizeApplied == SizeButton.AppliedTwo && (width > _width || height > _height)) // При определённом значении изменять размер кнопок
            {
                Log.ChangeSizeButtonUp();
                _sizeApplied = SizeButton.AppliedOne;
            }
            else if (_sizeApplied == SizeButton.AppliedOne && (width < _width && height < _height))
            {
                Log.ChangeSizeButtonDown();
                _sizeApplied = SizeButton.AppliedTwo;
            }


            if (_column == Panel.NotApplied && width > showHistoryWidth) // Показывать боковую панель "Журнал" при 750 пикселях
            {
                FullForm.ColumnStyles[1].Width = 30;
                _column = Panel.Applied;
            }
            else if (_column == Panel.Applied && width < showHistoryWidth)
            {
                FullForm.ColumnStyles[1].Width = 0;
                _column = Panel.NotApplied;
            }
        }

        public void KeyRead(object _, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Key.D0:
                case Key.NumPad0:
                case Key.D1:
                case Key.NumPad1:
                case Key.D2:
                case Key.NumPad2:
                case Key.D3:
                case Key.NumPad3:
                case Key.D4:
                case Key.NumPad4:
                case Key.D5:
                case Key.NumPad5:
                case Key.D6:
                case Key.NumPad6:
                case Key.D7:
                case Key.NumPad7:
                case Key.D8:
                case Key.NumPad8:
                case Key.D9:
                case Key.NumPad9:
                    Log.AddNum(Log.KeyToNumDict[e.KeyCode]);
                    TextAnswer.Focus();
                    break;
                case Key.Add:
                case Key.Subtract:
                case Key.Multiply:
                case Key.Divide:
                    Log.AddOperation(Log.KeyToOperationDict[e.KeyCode]);
                    TextAnswer.Focus();
                    break;
                case Key.Enter:
                    Log.CountingInEqual();
                    break;
                case Key.Decimal:
                    Log.PutComma();
                    break;
            }
        }

        private void deleteButton_MouseEnter(object sender, EventArgs e) => new ToolTip().SetToolTip(deleteButton, "Удалить всё");

        private void deleteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                Width.ToString()
                );
        }

    }

    public class FormLog
    {
        private List<double> Nums = new List<double>();

        internal static List<Operation> Operations = new List<Operation>(); // Лист со всеми операциями (возможно нужно сделать переменную)
        internal static Dictionary<Operation, string> operationToStringDict = new Dictionary<Operation, string>
        {
            { Operation.Plus, "+"},
            { Operation.Minus, "-"},
            { Operation.Multiply, "*"},
            { Operation.Divide, "/"}
        }; // Словарь для преоброзования операции в строку
        internal Dictionary<Key, Operation> KeyToOperationDict = new Dictionary<Key, Operation>
        {
            { Key.Add, Operation.Plus },
            { Key.Subtract, Operation.Minus },
            { Key.Multiply, Operation.Multiply },
            { Key.Divide, Operation.Divide }
        }; // Словарь для преоброзования ключа кнопки с клавиатуры в операцию
        internal Dictionary<Key, string> KeyToNumDict = new Dictionary<Key, string>
        {
            { Key.D0, "0" }, { Key.D1, "1" }, { Key.D2, "2" },
            { Key.D3, "3" }, { Key.D4, "4" }, { Key.D5, "5" },
            { Key.D6, "6" }, { Key.D7, "7" }, { Key.D8, "8" },
            { Key.D9, "9" },
            { Key.NumPad0, "0" }, { Key.NumPad1, "1" }, { Key.NumPad2, "2" },
            { Key.NumPad3, "3" }, { Key.NumPad4, "4" }, { Key.NumPad5, "5" },
            { Key.NumPad6, "6" }, { Key.NumPad7, "7" }, { Key.NumPad8, "8" },
            { Key.NumPad9, "9" }
        }; // Словарь для преоброзования ключа с клавиатуры в число


        private int MaxLenNum = 25; // Максимальное количество символов в строке

        private string meaning; // Строка которая потом превратится в число
        private bool _answerIs; // Получен ли ответ
        private bool _isRemoveNumInString; // Если строка TextQuestion (нижняя строка) пустая, то стоит true

        MainWindow window;

        public FormLog(MainWindow mainWindow)
        {
            this.window = mainWindow;
        }


        internal void AddNum(string numString) // Загружаем ведённое число в массив
        {
            if (ChectError(numString))
            {
                if (_isRemoveNumInString && numString != "," && form.TextQuestion.Text.Length < MaxLenNum || _answerIs)
                {
                    meaning = numString;
                    form.TextQuestion.Text = numString;
                    _isRemoveNumInString = false;
                }
                else if (form.TextQuestion.Text.Length < MaxLenNum)
                {
                    meaning += numString;
                    form.TextQuestion.Text += numString;
                }
            }

            if (form.TextQuestion.Text.Length / 3f > 1)
                SeparateDigits();
        }

        internal bool ChectError(string numString) // Проверяет на добавление числа
        {
            if (numString == "0" && form.TextQuestion.Text == "0")
                return false;

            if (_answerIs) Clear(); _answerIs = false;

            if (numString != "0" && form.TextQuestion.Text == "0" && numString != ",")
                form.TextQuestion.Text = "";

            return true;
        }


        public int minSizeNum = 14; // Минимальное и максимальное значене для кнопок 1, 2, 3 и т.д.
        private int maxSizeNum = 20;

        public int minSizeOper = 18; // Минимальное и максимальное значене для кнопок плюс, минус, равно и т.д.
        private int maxSizeOper = 31;

        internal void ChangeSizeButtonUp() // Изменение размера Font кнопок вверх
        {
            foreach (var bt in form.NumberPanel.Controls.OfType<Button>())
            {
                if (bt != null)
                {
                    if (bt.Font.Size == minSizeNum)
                        bt.Font = new Font(bt.Font.Name, maxSizeNum);

                    else if (bt.Font.Size == minSizeOper)
                        bt.Font = new Font(bt.Font.Name, maxSizeOper);
                }
            }
        }

        internal void ChangeSizeButtonDown() // Изменение размера Font кнопок вниз
        {
            foreach (var bt in form.NumberPanel.Controls.OfType<Button>())
            {
                if (bt != null)
                {
                    if (bt.Font.Size == maxSizeNum)
                        bt.Font = new Font(bt.Font.Name, minSizeNum);

                    else if (bt.Font.Size == maxSizeOper)
                        bt.Font = new Font(bt.Font.Name, minSizeOper);

                }
            }
        }

        internal void AddOperation(Operation operation) // Добавление операции
        {
            if (CheckError())
            {
                if (!_isRemoveNumInString)
                    Nums.Add(Convert.ToDouble(meaning));
                else
                    Operations.RemoveAt(0);


                Operations.Add(operation);

                if (Nums.Count > 1)
                    form.TextAnswer.Text = String.Format("{1} {0}", operationToStringDict[Operations[0]], Couting());
                else
                    form.TextAnswer.Text = Nums[0] + " " + operationToStringDict[Operations[0]];

                _isRemoveNumInString = true;
            }
        }

        internal bool CheckError() // Проверяет на добавление операции
        {
            if (form.TextQuestion.Text == "0")
                return false;

            return true;
        }

        internal void Clear() // Очищает строки
        {
            form.TextQuestion.Text = "0";
            form.TextAnswer.Text = "";
            Nums = new List<double>();
            Operations = new List<Operation>();
            meaning = "";
        }

        internal void CountingInEqual() // нажатие равно или enter
        {
            if (!_isRemoveNumInString)
            {
                Nums.Add(Convert.ToDouble(meaning));
            }
            else
            {
                Nums.Add(Convert.ToDouble(form.TextQuestion.Text));
                _isRemoveNumInString = false;
            }

            string answerOnButtonText;

            if (form.TextAnswer.Text.IndexOf("=") == -1)
                answerOnButtonText = form.TextAnswer.Text + " " + form.TextQuestion.Text + " =";
            else
                answerOnButtonText = Nums[0] + " " + operationToStringDict[Operations[0]] + " " + Nums[1] + " =";

            form.TextAnswer.Text = answerOnButtonText;

            if (Nums.Count >= 3)
                Nums.RemoveAt(1);

            string answer = Couting().ToString();
            form.TextQuestion.Text = answer;
            SeparateDigits();
            _answerIs = true;



            // Добавление в журнал ()

            Button btn = new Button();
            form.TableJournal.Controls.Add(btn, 0, 0);

            btn.Text = answerOnButtonText + "\n" + answer;
            btn.Font = new Font(btn.Text, 15);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Height = 40;
            btn.TabStop = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            //btn.Dock = DockStyle.Top;
            btn.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btn.AutoSize = true;

            MessageBox.Show(btn.Width.ToString() + "\n" + form.TableJournal.Width.ToString());
        }

        internal double Couting() // Считалка
        {
            if (Operations.Count == 0)
                return Nums[0];

            switch (Operations[0])
            {
                case Operation.Plus: Nums[0] = Nums[0] + Nums[1]; break;
                case Operation.Minus: Nums[0] = Nums[0] - Nums[1]; break;
                case Operation.Multiply: Nums[0] = Nums[0] * Nums[1]; break;
                case Operation.Divide: Nums[0] = Nums[0] / Nums[1]; break;
            }

            Nums.RemoveAt(1);

            if (!_isRemoveNumInString && Operations.Count >= 2)
                Operations.RemoveAt(1);

            return Nums[0];
        }

        internal void SeparateDigits() // отделяем по 3 цифры
        {
            string text = new string(form.TextQuestion.Text.Replace(" ", "").Reverse().ToArray());
            string newText = "";


            if (text.IndexOf(",") != -1)
            {
                newText = text.Substring(0, text.IndexOf(","));

                for (int i = 1; i <= text.Length - text.IndexOf(","); i++)
                {
                    newText += text[i + text.IndexOf(",") - 1];

                    if (i % 4 == 0)
                        newText += " ";
                }
            }
            else
            {
                for (int i = 1; i <= text.Length; i++)
                {
                    newText += text[i - 1];

                    if (i % 3 == 0)
                        newText += " ";
                }
            }

            form.TextQuestion.Text = new string(newText.Reverse().ToArray());
        }

        internal void PutComma() // Поставить запятую
        {
            if (form.TextQuestion.Text.IndexOf(",") == -1)
                AddNum(",");
        }
    }

    public enum Operation // Операции над числами
    {
        Plus, Minus, Multiply, Divide
    }
    public enum SizeButton // Размер Font кнопок
    {
        AppliedOne,
        AppliedTwo
    }
    public enum Panel // Есть ли боковая панель "Журнал"
    {
        Applied,
        NotApplied
    }
}
*/