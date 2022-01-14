﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace calculator
{
    public class WinLog
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

        public WinLog(MainWindow mainWindow)
        {
            this.window = mainWindow;
        }


        internal void AddNum(string numString) // Загружаем ведённое число в массив
        {
            if (ChectError(numString))
            {
                if (_isRemoveNumInString && numString != "," && window.TextQuestion.Content.ToString().Length < MaxLenNum || _answerIs)
                {
                    meaning = numString;
                    window.TextQuestion.Content = numString;
                    _isRemoveNumInString = false;
                }
                else if (window.TextQuestion.Content.ToString().Length < MaxLenNum)
                {
                    meaning += numString;
                    window.TextQuestion.Content += numString;
                }
            }

            if (window.TextQuestion.Content.ToString().Length / 3f > 1)
                SeparateDigits();
        }

        internal bool ChectError(string numString) // Проверяет на добавление числа
        {
            if (numString == "0" && window.TextQuestion.Content.ToString() == "0")
                return false;

            if (_answerIs) Clear(); _answerIs = false;

            if (numString != "0" && window.TextQuestion.Content.ToString() == "0" && numString != ",")
                window.TextQuestion.Content = "";

            return true;
        }


        public int minSizeNum = 16; // Минимальное и максимальное значене для кнопок 1, 2, 3 и т.д.
        private int maxSizeNum = 20;

        public int minSizeOper = 18; // Минимальное и максимальное значене для кнопок плюс, минус, равно и т.д.
        private int maxSizeOper = 31;

        internal void ChangeSizeButtonUp() // Изменение размера Font кнопок вверх
        {

            foreach (var bt in GetButtonList())
            {
                if (bt != null)
                {
                    if (bt.FontSize == minSizeNum)
                        bt.FontSize = maxSizeNum;

                    else if (bt.FontSize == minSizeOper)
                        bt.FontSize = maxSizeOper;
                }
            }

        }

        internal void ChangeSizeButtonDown() // Изменение размера Font кнопок вниз
        {

            foreach (var bt in GetButtonList())
            {
                if (bt != null)
                {
                    if (bt.FontSize == maxSizeNum)
                        bt.FontSize = minSizeNum;

                    else if (bt.FontSize == maxSizeOper)
                        bt.FontSize = minSizeOper;

                }
            }

        }

        internal IEnumerable<Button> GetButtonList()
        {
            return window.NumberPanel.Children.OfType<Button>();
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
                    window.TextAnswer.Content = String.Format("{1} {0}", operationToStringDict[Operations[0]], Couting());
                else
                    window.TextAnswer.Content = Nums[0] + " " + operationToStringDict[Operations[0]];


                _isRemoveNumInString = true;
            }
        }

        internal bool CheckError() // Проверяет на добавление операции
        {
            if (window.TextQuestion.Content.ToString() == "0")
                return false;

            return true;
        }

        internal void Clear() // Очищает строки
        {
            window.TextQuestion.Content = "0";
            window.TextAnswer.Content = "";
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
                Nums.Add(Convert.ToDouble(window.TextQuestion.Content));
                _isRemoveNumInString = false;
            }

            string answerOnButtonText;


            if (window.TextAnswer.Content.ToString().IndexOf("=") == -1)
                answerOnButtonText = window.TextAnswer.Content + " " + window.TextQuestion.Content + " =";
            else
                answerOnButtonText = Nums[0] + " " + operationToStringDict[Operations[0]] + " " + Nums[1] + " =";


            window.TextAnswer.Content = answerOnButtonText;

            if (Nums.Count >= 3)
                Nums.RemoveAt(1);

            string answer = Couting().ToString();
            window.TextQuestion.Content = answer;
            SeparateDigits();
            _answerIs = true;



            // Добавление в журнал
            /*
            Button btn = new Button();
            window.TableJournal.Controls.Add(btn, 0, 0);

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

            MessageBox.Show(btn.Width.ToString() + "\n" + window.TableJournal.Width.ToString());
            */
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
            string text = new string(window.TextQuestion.Content.ToString().Replace(" ", "").Reverse().ToArray());
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

            window.TextQuestion.Content = new string(newText.Reverse().ToArray());
        }

        internal void PutComma() // Поставить запятую
        {
            if (window.TextQuestion.Content.ToString().IndexOf(",") == -1)
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