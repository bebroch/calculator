using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Drawing;

namespace calculator
{
    public partial class MainWindow : Window
    {
        readonly WinLog Log;
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
            btnMultiply.Click += (o, e) => ButtonClick(Key.Multiply);
            btnDivide.Click += (o, e) => ButtonClick(Key.Divide);

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

        private void MainWindowResized(object sender, SizeChangedEventArgs e)
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

        private void ButtonClick(Key e) // Все кнопки
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

        private void DoubleClickJournal(object sender, MouseEventArgs e)
        {
            NumsAndInformation itemTableJournal = Log.listInformation[TableJournal.SelectedIndex];
            Log.Clear();

            TextAnswer.Content = itemTableJournal.OneNum;
            TextQuestion.Content = itemTableJournal.TwoNum;
            Log._powIsApplied = itemTableJournal.PowIsApplied;
        }
    }
}