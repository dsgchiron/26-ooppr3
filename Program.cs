using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoFPatternsWinForms
{
    // 1. Породжувальний патерн: Singleton
    public class AppLoggerSingleton
    {
        private static AppLoggerSingleton instance;
        public string LogStatus { get; set; }

        private AppLoggerSingleton()
        {
            LogStatus = "Екземпляр Singleton успішно ініціалізовано.";
        }

        public static AppLoggerSingleton GetInstance()
        {
            if (instance == null)
                instance = new AppLoggerSingleton();
            return instance;
        }
    }

    // 2. Структурний патерн: Adapter
    public interface ITarget
    {
        string GetStandardData();
    }

    public class LegacySystemAdaptee
    {
        public string GetSpecificData()
        {
            return "Дані зі старого інтерфейсу (Adaptee)";
        }
    }

    public class DataAdapter : ITarget
    {
        private readonly LegacySystemAdaptee _adaptee;
        public DataAdapter(LegacySystemAdaptee adaptee)
        {
            _adaptee = adaptee;
        }

        public string GetStandardData()
        {
            return $"Адаптовано: [{_adaptee.GetSpecificData()}]";
        }
    }

    // 3. Поведінковий патерн: Strategy
    public interface ICalculationStrategy
    {
        int Calculate(int a, int b);
    }

    public class AddStrategy : ICalculationStrategy
    {
        public int Calculate(int a, int b) => a + b;
    }

    public class MultiplyStrategy : ICalculationStrategy
    {
        public int Calculate(int a, int b) => a * b;
    }

    public class CalculationContext
    {
        private ICalculationStrategy _strategy;
        public CalculationContext(ICalculationStrategy strategy)
        {
            _strategy = strategy;
        }
        public void SetStrategy(ICalculationStrategy strategy)
        {
            _strategy = strategy;
        }
        public int Execute(int a, int b) => _strategy.Calculate(a, b);
    }

    // Головна форма додатку
    public class MainForm : Form
    {
        private Button btnSingleton;
        private Button btnAdapter;
        private Button btnStrategy;
        private TextBox txtOutput;

        public MainForm()
        {
            this.Text = "Шаблони проектування GoF";
            this.Size = new Size(500, 350);

            btnSingleton = new Button { Text = "Тест Singleton", Location = new Point(20, 20), Width = 130 };
            btnAdapter = new Button { Text = "Тест Adapter", Location = new Point(170, 20), Width = 130 };
            btnStrategy = new Button { Text = "Тест Strategy", Location = new Point(320, 20), Width = 130 };

            txtOutput = new TextBox
            {
                Location = new Point(20, 60),
                Size = new Size(430, 220),
                Multiline = true,
                ReadOnly = true
            };

            btnSingleton.Click += (s, e) =>
            {
                var s1 = AppLoggerSingleton.GetInstance();
                var s2 = AppLoggerSingleton.GetInstance();
                txtOutput.AppendText($"[Singleton] {s1.LogStatus}\r\nОднакові посилання (s1 == s2): {ReferenceEquals(s1, s2)}\r\n\r\n");
            };

            btnAdapter.Click += (s, e) =>
            {
                ITarget target = new DataAdapter(new LegacySystemAdaptee());
                txtOutput.AppendText($"[Adapter] {target.GetStandardData()}\r\n\r\n");
            };

            btnStrategy.Click += (s, e) =>
            {
                var context = new CalculationContext(new AddStrategy());
                int res1 = context.Execute(10, 5);
                context.SetStrategy(new MultiplyStrategy());
                int res2 = context.Execute(10, 5);
                txtOutput.AppendText($"[Strategy] Додавання (10, 5) = {res1}; Множення (10, 5) = {res2}\r\n\r\n");
            };

            this.Controls.Add(btnSingleton);
            this.Controls.Add(btnAdapter);
            this.Controls.Add(btnStrategy);
            this.Controls.Add(txtOutput);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}