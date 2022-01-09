using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace калькулятор_на_проект
{
    public partial class Form1 : Form
    {
        FormLog Log;
        private int _minSizeWidth = 289;
        private int _minSizeHeight = 443;

        public Form1()
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
                else if(bt.Font.Size == 18)
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

        private int _width = 1000; // Минимальный размер для большого размера font size кнопок
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
            } else if(_sizeApplied == SizeButton.AppliedOne && (width < _width && height < _height))
            {
                Log.ChangeSizeButtonDown();
                _sizeApplied = SizeButton.AppliedTwo;
            }


            if (_column == Panel.NotApplied && width > showHistoryWidth) // Показывать боковую панель "Журнал" при 750 пикселях
            {
                FullForm.ColumnStyles[1].Width = 30;
                _column = Panel.Applied;
            } else if(_column == Panel.Applied && width < showHistoryWidth)
            {
                FullForm.ColumnStyles[1].Width = 0;
                _column = Panel.NotApplied;
            }
        }

        public void KeyRead(object _, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                case Keys.D1:
                case Keys.NumPad1:
                case Keys.D2:
                case Keys.NumPad2:
                case Keys.D3:
                case Keys.NumPad3:
                case Keys.D4:
                case Keys.NumPad4:
                case Keys.D5:
                case Keys.NumPad5:
                case Keys.D6:
                case Keys.NumPad6:
                case Keys.D7:
                case Keys.NumPad7:
                case Keys.D8:
                case Keys.NumPad8:
                case Keys.D9:
                case Keys.NumPad9:
                    Log.AddNum(Log.keysToNumDict[e.KeyCode]);
                    TextAnswer.Focus();
                    break;
                case Keys.Add:
                case Keys.Subtract:
                case Keys.Multiply:
                case Keys.Divide:
                    Log.AddOperation(Log.keysToOperationDict[e.KeyCode]);
                    TextAnswer.Focus();
                    break;
                case Keys.Enter:
                    Log.CountingInEqual();
                    break;
                case Keys.Decimal:
                    Log.PutComma();
                    break;
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e) => new ToolTip().SetToolTip(button2, "Удалить всё");
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
        internal Dictionary<Keys, Operation> keysToOperationDict = new Dictionary<Keys, Operation> 
        {
            { Keys.Add, Operation.Plus },
            { Keys.Subtract, Operation.Minus },
            { Keys.Multiply, Operation.Multiply },
            { Keys.Divide, Operation.Divide }
        }; // Словарь для преоброзования ключа кнопки с клавиатуры в операцию
        internal Dictionary<Keys, string> keysToNumDict = new Dictionary<Keys, string>
        {
            { Keys.D0, "0" }, { Keys.D1, "1" }, { Keys.D2, "2" },
            { Keys.D3, "3" }, { Keys.D4, "4" }, { Keys.D5, "5" },
            { Keys.D6, "6" }, { Keys.D7, "7" }, { Keys.D8, "8" },
            { Keys.D9, "9" },
            { Keys.NumPad0, "0" }, { Keys.NumPad1, "1" }, { Keys.NumPad2, "2" },
            { Keys.NumPad3, "3" }, { Keys.NumPad4, "4" }, { Keys.NumPad5, "5" },
            { Keys.NumPad6, "6" }, { Keys.NumPad7, "7" }, { Keys.NumPad8, "8" },
            { Keys.NumPad9, "9" }
        }; // Словарь для преоброзования ключа с клавиатуры в число


        private int MaxLenNum = 25; // Максимальное количество символов в строке

        private string meaning; // Строка которая потом превратится в число
        private bool _answerIs; // Получен ли ответ
        private bool _isRemoveNumInString; // Если строка TextQuestion (нижняя строка) пустая, то стоит true

        Form1 form;

        public FormLog(Form1 form)
        {
            this.form = form;
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
            if (numString == "0" && form.TextQuestion.Text == "0" )
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

            journalOperation btn = new journalOperation();
            btn.Width = form.TableJournal.Width;
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            form.TableJournal.Controls.Add(btn, 0, 1);
            

            //Button btn = new Button();
            //btn.Text = answerOnButtonText + "\n" + answer;
            //btn.Width = form.TableJournal.Width;
            //btn.Height = 60;
            //btn.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            //form.TableJournal.Controls.Add(btn, 0, 1);
            //MessageBox.Show(btn.Anchor.ToString());


            //form.LogPanel.SetRow(btn, 1);
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