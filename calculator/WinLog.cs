using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace calculator
{
    public class WinLog
    {
        internal List<double> Nums = new List<double>();

        internal Operation? _operation = new Operation(); // Лист со всеми операциями (возможно нужно сделать переменную)

        internal readonly Dictionary<Operation?, string> operationToStringDict = new Dictionary<Operation?, string>
        {
            { calculator.Operation.Plus, "+"},
            { calculator.Operation.Minus, "-"},
            { calculator.Operation.Multiply, "*"},
            { calculator.Operation.Divide, "/"}
        }; // Словарь для преоброзования операции в строку
        internal readonly Dictionary<Key, Operation> KeyToOperationDict = new Dictionary<Key, Operation>
        {
            { Key.Add, calculator.Operation.Plus },
            { Key.Subtract, calculator.Operation.Minus },
            { Key.Multiply, calculator.Operation.Multiply },
            { Key.Divide, calculator.Operation.Divide }
        }; // Словарь для преоброзования ключа кнопки с клавиатуры в операцию
        internal readonly Dictionary<Key, string> KeyToNumDict = new Dictionary<Key, string>
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


        internal int MaxLenNum = 25; // Максимальное количество символов в строке

        internal string meaning; // Строка которая потом превратится в число
        internal bool _answerIs; // Получен ли ответ
        internal bool _isRemoveNumInString; // Если строка TextQuestion (нижняя строка) пустая, то стоит true
        internal bool _powIsApplied;

        readonly MainWindow window;

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


        internal int minSizeNum = 16; // Минимальное и максимальное значене для кнопок 1, 2, 3 и т.д.
        internal int maxSizeNum = 20;

        internal int minSizeOper = 18; // Минимальное и максимальное значене для кнопок плюс, минус, равно и т.д.
        internal int maxSizeOper = 31;

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

        internal IEnumerable<Button> GetButtonList() // Добавление всех кнопок в NumberPanel в лист
        {
            return window.NumberPanel.Children.OfType<Button>();
        }

        internal void AddOperation(Operation operation) // Добавление операции
        {
            if (CheckError())
            {
                string line = window.TextQuestion.Content.ToString();

                if (_powIsApplied)
                {
                    Clear();
                    Nums.Add(Convert.ToDouble(line));
                    _powIsApplied = false;
                }
                else if (!_isRemoveNumInString && !_answerIs)
                    Nums.Add(Convert.ToDouble(line));

                _operation = operation;

                if (Nums.Count == 2)
                    window.TextAnswer.Content = string.Format("{1} {0}", operationToStringDict[_operation], Couting());
                else if (Nums.Count == 1)
                    window.TextAnswer.Content = Nums[0] + " " + operationToStringDict[_operation];

                _answerIs = false;
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
            _operation = null;
            meaning = "";
        }

        internal void CountingInEqual() // нажатие равно или enter
        {
            if (_powIsApplied)
            {
                string line = window.TextQuestion.Content.ToString();
                Clear();
                Nums.Add(Convert.ToDouble(line));
                window.TextQuestion.Content = line;
                _powIsApplied = false;
            }
            else if (!_isRemoveNumInString && meaning != "")
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
                answerOnButtonText = Nums[0] + " " + operationToStringDict[_operation] + " " + Nums[1] + " =";


            window.TextAnswer.Content = answerOnButtonText;

            string answer = Couting().ToString();
            window.TextQuestion.Content = answer;
            SeparateDigits();
            _answerIs = true;

            JournalAdd();
        }

        internal double Couting() // Считалка
        {
            if (_operation == null)
                return Nums[0];

            switch (_operation)
            {
                case Operation.Plus: Nums[0] = Nums[0] + Nums[1]; break;
                case Operation.Minus: Nums[0] = Nums[0] - Nums[1]; break;
                case Operation.Multiply: Nums[0] = Nums[0] * Nums[1]; break;
                case Operation.Divide: Nums[0] = Nums[0] / Nums[1]; break;
            }

            Nums.RemoveAt(1);

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

        internal void PutPlusAndMinus() // Поставить Минус или убрать его
        {
            string lineQ = window.TextQuestion.Content.ToString();

            if (lineQ != "0" && lineQ.IndexOf("-") == -1)
                lineQ = "-" + lineQ;
            else
                lineQ = lineQ.Replace("-", "");

            window.TextQuestion.Content = lineQ;
        }

        internal void Pow(float pow) // Возвести в степень
        {
            if(pow >= 0)
            {
                if (pow < 1)
                    window.TextAnswer.Content = "√" + window.TextQuestion.Content.ToString();
                else
                    window.TextAnswer.Content = window.TextQuestion.Content.ToString() + "²";

                window.TextQuestion.Content = Math.Pow(Convert.ToSingle(window.TextQuestion.Content), pow).ToString();
                _powIsApplied = true;
                JournalAdd(" =");
            }
        }

        internal ObservableCollection<Nums> listMain = new ObservableCollection<Nums>(); // Список со всеми операциями
        internal List<NumsAndInformation> listInformation = new();

        internal void JournalAdd(string line = "") // Добавление списка в журнал
        {
            string oneNum = window.TextAnswer.Content.ToString() + line;
            string twoNum = window.TextQuestion.Content.ToString();

            listMain.Insert(0, new Nums
            {
                OneNum = oneNum,
                TwoNum = twoNum
            });

            window.TableJournal.ItemsSource = listMain;

            listInformation.Insert(0, new NumsAndInformation
            {
                OneNum = oneNum,
                TwoNum = twoNum,
                PowIsApplied = _powIsApplied,
                AnswerIs = _answerIs,
                IsRemoveNumInString = _isRemoveNumInString,
                Operation = _operation,
                nums = Nums
            });
        }
    }

    internal class Nums
    {
        public string OneNum { get; set; }
        public string TwoNum { get; set; }
    }

    internal class NumsAndInformation : Nums
    {
        public bool PowIsApplied { get; set; }
        public bool AnswerIs { get; set; }
        public bool IsRemoveNumInString { get; set; }
        public Operation? Operation { get; set; }
        public List<double> nums { get; set; }
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
