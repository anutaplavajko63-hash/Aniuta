using System;
using System.Text;

namespace DostavkaIzhi
{
    internal class Program
    {
        const double BasePrice = 50.0;
        const double PricePerKm = 12.0;
        const double PricePerKg = 8.0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            // Головне меню працює, доки користувач не вибере вихід.
            while (running)
            {
                Console.WriteLine("=== Доставка їжі ===");
                Console.WriteLine("1 — Довідка про програму");
                Console.WriteLine("2 — Таблиця вартості доставки");
                Console.WriteLine("3 — Таблиця за відстанню та вагою");
                Console.WriteLine("0 — Вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowDeliveryPriceTable();
                        Pause();
                        break;
                    case "3":
                        ShowTwoParameterTable();
                        Pause();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Роботу програми завершено.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту меню. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        // Виведення відомостей про тему та поля головної сутності.
        static void ShowAbout()
        {
            Console.WriteLine("Тема проєкту №20 — Доставка їжі.");
            Console.WriteLine("Сутності: Користувач, Замовлення, Статус (довідник).");
            Console.WriteLine("Поля сутності «Замовлення»:");
            Console.WriteLine("int номер; string клієнт; string адреса;");
            Console.WriteLine("string склад замовлення; double сума; string кур'єр;");
            Console.WriteLine("string час прийняття; string час доставки; int код статусу.");
            Console.WriteLine("Підсумкова величина: середній час доставки по кожному кур'єру.");
        }

        // Розрахунок умовної вартості доставки за відстанню.
        static double CalculateDeliveryPrice(double distance)
        {
            return BasePrice + PricePerKm * distance;
        }

        // Табулювання функції з дробовим кроком.
        static void ShowDeliveryPriceTable()
        {
            double from = 0.5;
            double to = 4.0;
            double step = 0.5;

            Console.WriteLine("Вартість доставки залежно від відстані");
            Console.WriteLine($"Базова ціна: {BasePrice:F2} грн; тариф: {PricePerKm:F2} грн/км.");
            Console.WriteLine($"{ "Відстань, км",14}{ "Вартість, грн",18}");

            for (double distance = from; distance <= to + 1e-9; distance += step)
            {
                double price = CalculateDeliveryPrice(distance);
                Console.WriteLine($"{distance,14:F2}{price,18:F2}");
            }
        }

        // Побудова таблиці за відстанню та вагою з підсумками.
        static void ShowTwoParameterTable()
        {
            Console.Write("Введіть кількість рядків (відстаней): ");
            string rowsText = Console.ReadLine() ?? "";
            if (!int.TryParse(rowsText, out int rows) || rows <= 0)
            {
                Console.WriteLine("Помилка: введіть додатне ціле число.");
                return;
            }

            Console.Write("Введіть кількість стовпців (значень ваги): ");
            string colsText = Console.ReadLine() ?? "";
            if (!int.TryParse(colsText, out int cols) || cols <= 0)
            {
                Console.WriteLine("Помилка: введіть додатне ціле число.");
                return;
            }

            double[,] prices = new double[rows, cols];
            double[] columnTotals = new double[cols];
            double grandTotal = 0.0;

            Console.WriteLine();
            Console.Write($"{ "км / кг",10}");
            for (int col = 0; col < cols; col++)
                Console.Write($"{col + 1,10:F1}");
            Console.WriteLine($"{ "Разом",12}");

            for (int row = 0; row < rows; row++)
            {
                double distance = row + 1;
                double rowTotal = 0.0;
                Console.Write($"{distance,10:F1}");

                for (int col = 0; col < cols; col++)
                {
                    double weight = col + 1;
                    double price = BasePrice + PricePerKm * distance + PricePerKg * weight;
                    prices[row, col] = price;
                    rowTotal += price;
                    columnTotals[col] += price;
                    grandTotal += price;
                    Console.Write($"{price,10:F2}");
                }

                Console.WriteLine($"{rowTotal,12:F2}");
            }

            // Нижній рядок містить підсумки стовпців і загальну суму.
            Console.Write($"{ "Разом",10}");
            for (int col = 0; col < cols; col++)
                Console.Write($"{columnTotals[col],10:F2}");
            Console.WriteLine($"{grandTotal,12:F2}");
        }

        // Пауза перед поверненням до головного меню.
        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для повернення до меню...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
