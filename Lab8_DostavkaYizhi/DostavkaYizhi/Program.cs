using System;
using System.Text;

class Program
{
    const int MaxOrders = 100;
    const int MaxStatuses = 4;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        int count = 0;
        int[] number = new int[MaxOrders];
        string[] customer = new string[MaxOrders];
        string[] address = new string[MaxOrders];
        string[] composition = new string[MaxOrders];
        double[] amount = new double[MaxOrders];
        string[] courier = new string[MaxOrders];
        string[] acceptedAt = new string[MaxOrders];
        string[] deliveredAt = new string[MaxOrders];
        int[] statusCode = new int[MaxOrders];

        int[] statusCodes = { 1, 2, 3, 4 };
        string[] statusNames = { "Прийнято", "Готується", "У дорозі", "Доставлено" };

        bool running = true;
        while (running)
        {
            ShowMenu();
            string choice = Console.ReadLine() ?? "";
            Console.WriteLine();

            switch (choice)
            {
                case "1": ShowAbout(); Pause(); break;
                case "2":
                    AddOrder(ref count, number, customer, address, composition, amount,
                        courier, acceptedAt, deliveredAt, statusCode);
                    Pause();
                    break;
                case "3":
                    PrintOrders(count, number, customer, address, composition, amount,
                        courier, acceptedAt, deliveredAt, statusCode, statusCodes, statusNames);
                    Pause();
                    break;
                case "4":
                    PrintAverageDeliveryByCourier(count, courier, acceptedAt, deliveredAt);
                    Pause();
                    break;
                case "5": PrintStatusDirectory(statusCodes, statusNames); Pause(); break;
                case "0": running = false; break;
                default: Console.WriteLine("Немає такого пункту меню."); Pause(); break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("========== ДОСТАВКА ЇЖІ ==========");
        Console.WriteLine("1 — Про програму");
        Console.WriteLine("2 — Додати замовлення");
        Console.WriteLine("3 — Переглянути замовлення");
        Console.WriteLine("4 — Середній час доставки за кур'єрами");
        Console.WriteLine("5 — Довідник статусів");
        Console.WriteLine("0 — Вихід");
        Console.Write("Ваш вибір: ");
    }

    static void ShowAbout()
    {
        Console.WriteLine("Тема 20. Доставка їжі");
        Console.WriteLine("Сутності: Користувач, Замовлення, Статус (довідник).");
        Console.WriteLine("Поля сутності «Замовлення»:");
        Console.WriteLine("int номер; string клієнт; string адреса; string склад замовлення;");
        Console.WriteLine("double сума; string кур'єр; string час прийняття; string час доставки;");
        Console.WriteLine("int код статусу (з довідника).");
        Console.WriteLine("Підсумкова величина: середній час доставки по кожному кур'єру.");
    }

    static void AddOrder(ref int count, int[] number, string[] customer, string[] address,
        string[] composition, double[] amount, string[] courier, string[] acceptedAt,
        string[] deliveredAt, int[] statusCode)
    {
        if (count >= number.Length)
        {
            Console.WriteLine("Досягнуто максимальної кількості замовлень.");
            return;
        }

        Console.WriteLine("=== Нове замовлення ===");
        number[count] = ReadInt("Номер замовлення: ", 1, 1000000);
        customer[count] = ReadText("Клієнт: ");
        address[count] = ReadText("Адреса доставки: ");
        composition[count] = ReadText("Склад замовлення: ");
        amount[count] = ReadDouble("Сума замовлення: ", 0.01, 1000000);
        courier[count] = ReadText("Кур'єр: ");
        acceptedAt[count] = ReadDateTime("Час прийняття (наприклад 19.11.2026 12:30): ");
        deliveredAt[count] = ReadDateTime("Час доставки (наприклад 19.11.2026 13:10): ");

        DateTime start = DateTime.Parse(acceptedAt[count]);
        DateTime finish = DateTime.Parse(deliveredAt[count]);
        while (finish < start)
        {
            Console.WriteLine("Час доставки не може бути раніше часу прийняття.");
            deliveredAt[count] = ReadDateTime("Введіть час доставки ще раз: ");
            finish = DateTime.Parse(deliveredAt[count]);
        }

        statusCode[count] = ReadInt("Код статусу (1–4): ", 1, 4);
        count++;
        Console.WriteLine("Замовлення додано.");
    }

    static void PrintOrders(int count, int[] number, string[] customer, string[] address,
        string[] composition, double[] amount, string[] courier, string[] acceptedAt,
        string[] deliveredAt, int[] statusCode, int[] statusCodes, string[] statusNames)
    {
        if (count == 0)
        {
            Console.WriteLine("Замовлень поки немає.");
            return;
        }

        Console.WriteLine("№ | Клієнт | Адреса | Склад | Сума | Кур'єр | Прийнято | Доставлено | Статус");
        Console.WriteLine(new string('-', 120));
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"{number[i]} | {customer[i]} | {address[i]} | {composition[i]} | " +
                $"{amount[i]:F2} грн | {courier[i]} | {acceptedAt[i]} | {deliveredAt[i]} | " +
                $"{GetStatusName(statusCode[i], statusCodes, statusNames)}");
        }
    }

    static void PrintAverageDeliveryByCourier(int count, string[] courier,
        string[] acceptedAt, string[] deliveredAt)
    {
        if (count == 0)
        {
            Console.WriteLine("Немає даних для обчислення.");
            return;
        }

        Console.WriteLine("=== Середній час доставки за кур'єрами ===");
        bool foundAny = false;

        for (int i = 0; i < count; i++)
        {
            bool alreadyShown = false;
            for (int j = 0; j < i; j++)
            {
                if (string.Equals(courier[i], courier[j], StringComparison.OrdinalIgnoreCase))
                {
                    alreadyShown = true;
                    break;
                }
            }
            if (alreadyShown) continue;

            double totalMinutes = 0;
            int deliveries = 0;
            for (int j = 0; j < count; j++)
            {
                if (string.Equals(courier[i], courier[j], StringComparison.OrdinalIgnoreCase) &&
                    DateTime.TryParse(acceptedAt[j], out DateTime start) &&
                    DateTime.TryParse(deliveredAt[j], out DateTime finish) &&
                    finish >= start)
                {
                    totalMinutes += (finish - start).TotalMinutes;
                    deliveries++;
                }
            }

            if (deliveries > 0)
            {
                double average = CalculateAverage(totalMinutes, deliveries);
                Console.WriteLine($"{courier[i]} — {average:F1} хв. (замовлень: {deliveries})");
                foundAny = true;
            }
        }

        if (!foundAny) Console.WriteLine("Немає завершених доставок для обчислення.");
    }

    static double CalculateAverage(double total, int count)
    {
        if (count <= 0) return 0;
        return total / count;
    }

    static void PrintStatusDirectory(int[] statusCodes, string[] statusNames)
    {
        Console.WriteLine("=== Довідник статусів ===");
        for (int i = 0; i < statusCodes.Length; i++)
            Console.WriteLine($"{statusCodes[i]} — {statusNames[i]}");
    }

    static string GetStatusName(int code, int[] statusCodes, string[] statusNames)
    {
        for (int i = 0; i < statusCodes.Length; i++)
            if (statusCodes[i] == code) return statusNames[i];
        return "Невідомий статус";
    }

    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;
            Console.WriteLine($"Помилка: введіть ціле число від {min} до {max}.");
        }
    }

    static double ReadDouble(string prompt, double min, double max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? "").Replace(',', '.');
            if (double.TryParse(input, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double value) &&
                value >= min && value <= max)
                return value;
            Console.WriteLine($"Помилка: введіть число від {min} до {max}.");
        }
    }

    static string ReadText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = (Console.ReadLine() ?? "").Trim();
            if (value.Length > 0) return value;
            Console.WriteLine("Поле не може бути порожнім.");
        }
    }

    static string ReadDateTime(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine() ?? "";
            if (DateTime.TryParse(value, out DateTime parsed))
                return parsed.ToString("dd.MM.yyyy HH:mm");
            Console.WriteLine("Неправильний формат дати й часу. Спробуйте ще раз.");
        }
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.Write("Натисніть Enter, щоб продовжити...");
        Console.ReadLine();
        Console.WriteLine();
    }
}
