using System;

class Program
{
    const int Capacity = 100;
    const int StatusCount = 4;

    static void Main()
    {
        // Сутність «Користувач» — окремі масиви користувачів.
        string[] userNames = new string[Capacity];
        string[] userAddresses = new string[Capacity];
        int userCount = 0;

        // Сутність «Замовлення» — поля зберігаються у паралельних масивах.
        int[] numbers = new int[Capacity];
        string[] clients = new string[Capacity];
        string[] addresses = new string[Capacity];
        string[] compositions = new string[Capacity];
        double[] sums = new double[Capacity];
        string[] couriers = new string[Capacity];
        DateTime[] acceptedAt = new DateTime[Capacity];
        DateTime[] deliveredAt = new DateTime[Capacity];
        int[] statusCodes = new int[Capacity];
        int orderCount = 0;

        // Сутність-довідник «Статус».
        int[] statusIds = { 1, 2, 3, 4 };
        string[] statusNames = { "Прийнято", "Готується", "У дорозі", "Доставлено" };

        // Початкові користувачі.
        userNames[userCount] = "Іван Петренко";
        userAddresses[userCount++] = "м. Ужгород, вул. Шевченка, 10";
        userNames[userCount] = "Олена Коваль";
        userAddresses[userCount++] = "м. Ужгород, вул. Минайська, 5";

        // Демонстраційні замовлення.
        AddOrder(numbers, clients, addresses, compositions, sums, couriers,
            acceptedAt, deliveredAt, statusCodes, ref orderCount,
            101, "Іван Петренко", "м. Ужгород, вул. Шевченка, 10",
            "Піца Маргарита, сік", 320, "Андрій",
            new DateTime(2026, 11, 19, 12, 0, 0),
            new DateTime(2026, 11, 19, 12, 35, 0), 4);

        AddOrder(numbers, clients, addresses, compositions, sums, couriers,
            acceptedAt, deliveredAt, statusCodes, ref orderCount,
            102, "Олена Коваль", "м. Ужгород, вул. Минайська, 5",
            "Бургер, картопля фрі", 245, "Максим",
            new DateTime(2026, 11, 19, 13, 0, 0),
            new DateTime(2026, 11, 19, 13, 50, 0), 4);

        AddOrder(numbers, clients, addresses, compositions, sums, couriers,
            acceptedAt, deliveredAt, statusCodes, ref orderCount,
            103, "Іван Петренко", "м. Ужгород, вул. Шевченка, 10",
            "Суші-сет", 510, "Андрій",
            new DateTime(2026, 11, 19, 14, 0, 0),
            new DateTime(2026, 11, 19, 14, 40, 0), 4);

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n===== ДОСТАВКА ЇЖІ =====");
            Console.WriteLine("1. Показати користувачів");
            Console.WriteLine("2. Додати користувача");
            Console.WriteLine("3. Додати замовлення");
            Console.WriteLine("4. Показати всі замовлення");
            Console.WriteLine("5. Показати довідник статусів");
            Console.WriteLine("6. Середній час доставки по кур'єрах");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    ShowUsers(userNames, userAddresses, userCount);
                    break;
                case "2":
                    AddUser(userNames, userAddresses, ref userCount);
                    break;
                case "3":
                    AddOrderFromKeyboard(numbers, clients, addresses, compositions,
                        sums, couriers, acceptedAt, deliveredAt, statusCodes,
                        ref orderCount, userNames, userAddresses, userCount);
                    break;
                case "4":
                    ShowOrders(numbers, clients, addresses, compositions, sums,
                        couriers, acceptedAt, deliveredAt, statusCodes,
                        orderCount, statusIds, statusNames);
                    break;
                case "5":
                    ShowStatuses(statusIds, statusNames);
                    break;
                case "6":
                    ShowCourierAverages(couriers, acceptedAt, deliveredAt, statusCodes,
                        orderCount, statusIds);
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Невірний пункт меню.");
                    break;
            }
        }
    }

    static void AddUser(string[] names, string[] addresses, ref int count)
    {
        if (count >= names.Length)
        {
            Console.WriteLine("Список користувачів заповнений.");
            return;
        }

        Console.Write("Ім'я клієнта: ");
        names[count] = Console.ReadLine() ?? "";
        Console.Write("Адреса: ");
        addresses[count] = Console.ReadLine() ?? "";
        count++;
        Console.WriteLine("Користувача додано.");
    }

    static void ShowUsers(string[] names, string[] addresses, int count)
    {
        Console.WriteLine("\n--- КОРИСТУВАЧІ ---");
        if (count == 0)
        {
            Console.WriteLine("Користувачів поки немає.");
            return;
        }

        for (int i = 0; i < count; i++)
            Console.WriteLine($"{i + 1}. {names[i]} — {addresses[i]}");
    }

    static void AddOrderFromKeyboard(
        int[] numbers, string[] clients, string[] addresses, string[] compositions,
        double[] sums, string[] couriers, DateTime[] acceptedAt,
        DateTime[] deliveredAt, int[] statusCodes, ref int count,
        string[] userNames, string[] userAddresses, int userCount)
    {
        if (count >= numbers.Length)
        {
            Console.WriteLine("Немає місця для нових замовлень.");
            return;
        }

        Console.Write("Номер замовлення: ");
        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Номер має бути цілим числом.");
            return;
        }

        Console.Write("Ім'я клієнта: ");
        string client = Console.ReadLine() ?? "";
        string address = "";
        bool found = false;

        for (int i = 0; i < userCount; i++)
        {
            if (string.Equals(userNames[i], client, StringComparison.OrdinalIgnoreCase))
            {
                address = userAddresses[i];
                found = true;
                break;
            }
        }

        if (!found)
        {
            Console.WriteLine("Користувача не знайдено. Спочатку додайте його.");
            return;
        }

        Console.WriteLine("Адреса з профілю: " + address);
        Console.Write("Склад замовлення: ");
        string composition = Console.ReadLine() ?? "";
        Console.Write("Сума: ");
        if (!double.TryParse(Console.ReadLine(), out double sum) || sum < 0)
        {
            Console.WriteLine("Некоректна сума.");
            return;
        }

        Console.Write("Кур'єр: ");
        string courier = Console.ReadLine() ?? "";
        Console.Write("Час прийняття (рррр-мм-дд гг:хх): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime accepted))
        {
            Console.WriteLine("Некоректна дата/час.");
            return;
        }

        Console.Write("Час доставки (рррр-мм-дд гг:хх): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime delivered) ||
            delivered < accepted)
        {
            Console.WriteLine("Некоректний час доставки.");
            return;
        }

        Console.WriteLine("Статуси: 1 — Прийнято, 2 — Готується, 3 — У дорозі, 4 — Доставлено");
        Console.Write("Код статусу: ");
        if (!int.TryParse(Console.ReadLine(), out int status) ||
            status < 1 || status > 4)
        {
            Console.WriteLine("Невірний код статусу.");
            return;
        }

        AddOrder(numbers, clients, addresses, compositions, sums, couriers,
            acceptedAt, deliveredAt, statusCodes, ref count, number, client,
            address, composition, sum, courier, accepted, delivered, status);
        Console.WriteLine("Замовлення додано.");
    }

    static void AddOrder(
        int[] numbers, string[] clients, string[] addresses, string[] compositions,
        double[] sums, string[] couriers, DateTime[] acceptedAt,
        DateTime[] deliveredAt, int[] statusCodes, ref int count,
        int number, string client, string address, string composition,
        double sum, string courier, DateTime accepted, DateTime delivered, int status)
    {
        if (count >= numbers.Length)
        {
            Console.WriteLine("Немає місця для нового замовлення.");
            return;
        }

        numbers[count] = number;
        clients[count] = client;
        addresses[count] = address;
        compositions[count] = composition;
        sums[count] = sum;
        couriers[count] = courier;
        acceptedAt[count] = accepted;
        deliveredAt[count] = delivered;
        statusCodes[count] = status;
        count++;
    }

    static void ShowOrders(
        int[] numbers, string[] clients, string[] addresses, string[] compositions,
        double[] sums, string[] couriers, DateTime[] acceptedAt,
        DateTime[] deliveredAt, int[] statusCodes, int count,
        int[] statusIds, string[] statusNames)
    {
        Console.WriteLine("\n--- ЗАМОВЛЕННЯ ---");
        if (count == 0)
        {
            Console.WriteLine("Замовлень немає.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            string statusName = FindStatus(statusCodes[i], statusIds, statusNames);
            Console.WriteLine(
                $"№{numbers[i]} | {clients[i]} | {addresses[i]} | {compositions[i]} | " +
                $"{sums[i]:F2} грн | Кур'єр: {couriers[i]} | " +
                $"Прийнято: {acceptedAt[i]:dd.MM.yyyy HH:mm} | " +
                $"Доставлено: {deliveredAt[i]:dd.MM.yyyy HH:mm} | Статус: {statusName}");
        }
    }

    static void ShowStatuses(int[] ids, string[] names)
    {
        Console.WriteLine("\n--- ДОВІДНИК СТАТУСІВ ---");
        for (int i = 0; i < ids.Length; i++)
            Console.WriteLine($"{ids[i]} — {names[i]}");
    }

    static string FindStatus(int code, int[] ids, string[] names)
    {
        for (int i = 0; i < ids.Length; i++)
            if (ids[i] == code)
                return names[i];

        return "Невідомий статус";
    }

    static void ShowCourierAverages(
        string[] couriers, DateTime[] acceptedAt, DateTime[] deliveredAt,
        int[] statusCodes, int count, int[] statusIds)
    {
        Console.WriteLine("\n--- СЕРЕДНІЙ ЧАС ДОСТАВКИ ПО КУР'ЄРАХ ---");
        bool[] processed = new bool[count];
        bool any = false;

        for (int i = 0; i < count; i++)
        {
            if (processed[i])
                continue;

            double totalMinutes = 0;
            int deliveredCount = 0;

            for (int j = i; j < count; j++)
            {
                if (couriers[j] == couriers[i])
                {
                    processed[j] = true;

                    // До середнього включаються лише доставлені замовлення.
                    if (statusCodes[j] == 4)
                    {
                        totalMinutes += (deliveredAt[j] - acceptedAt[j]).TotalMinutes;
                        deliveredCount++;
                    }
                }
            }

            if (deliveredCount > 0)
            {
                double average = totalMinutes / deliveredCount;
                Console.WriteLine($"{couriers[i]}: {average:F1} хв. ({deliveredCount} доставлених замовлень)");
                any = true;
            }
            else
            {
                Console.WriteLine($"{couriers[i]}: ще немає доставлених замовлень.");
            }
        }

        if (!any)
            Console.WriteLine("Поки немає завершених доставок.");
    }
}
