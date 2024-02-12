using System;
using System.Linq;

namespace Transporter_Bercut_v._2
{
    //Класс инкапсулирует обработку детали
    public class Detail
    {
        private string _operationType;
        private bool _success;

        public string OperationType { get { return _operationType; } set { _operationType = value; } }//Тип выполняемой операции ИМ.
        public bool Success { get { return _success; } set { _success = value; } }// Флаг успешности выполнения операции ИМ.

        public Detail(string operationType, bool success)
        {
            OperationType = operationType;
            Success = success;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("======Запускаю ленту======");
            Console.ResetColor();

            bool Status_First_IM = true; //Флаг выполнения первого ИМ. Становится false, если при выполнении обработки на первом участке произошла ошибка.
            bool Status_Second_IM = true; //Флаг выполнения второго ИМ. Становится false, если при выполнении обработки на втором участке произошла ошибка.

            Detail[] First_IM = First_Sector(); //Инициализация массива первого ИМ с выполняемыми операциями на первом участке.
            Detail[] Second_IM = Second_Sector(); //Инициализация массива второго ИМ с выполняемыми операциями на втором участке.

            // Технологический цикл повторяется до тех пор пока один из секторов конвейера обрабатываемыми двумя ИМ: First_IM , Second_IM не вызовет ошибку.
            while (Status_First_IM != false && Status_Second_IM != false)
            {
                //Рисую визуальную таблицу в консоли с заголовками
                var HeaderTable = String.Format("|{0,30}|{1,40}|", "Тип операции", "ИМ завершил операцию успешно?");
                Console.WriteLine(HeaderTable);
               
                //Обработка первым ИМ, первого участка
                for (int i = 0; i <= First_IM.Count() - 1; i++)
                {
                    Console.WriteLine(string.Format("|{0,30}|{1,40}|", First_IM[i].OperationType, First_IM[i].Success));
                    if (First_IM[i].Success == false)//При обработке участка первым ИМ операция вызвала ошибку
                    {
                        Status_First_IM = false;//Меняю флаг на false, т.к. одна из выполняемых операций вызвала остановку конвейера
                        if (CheckDetail_IM(First_IM[i]))//В методе содержится обработка действий оператором. Действия оператора возвращают true.
                        {
                            First_IM[i].Success = true; //После обработки оператором у операции вызвавшей ошибку меняем статус с false на true
                            Status_First_IM = true;//Метод CheckDetail_IM вернул true, а значит оператор исправил ошибку у операции вызвавшей остановку.
                        }
                        break;
                    }
                }
                if (Status_First_IM != false)//Если первый участок не вызвал ошибок, отрабатываем второй участок
                {
                    //Обработка вторым ИМ, второго участка
                    for (int j = 0; j <= Second_IM.Count() - 1; j++)
                    {
                        Console.WriteLine(string.Format("|{0,30}|{1,40}|", Second_IM[j].OperationType, Second_IM[j].Success));
                        if (Second_IM[j].Success == false)// При обработке участка вторым ИМ операция вызвала ошибку
                        {
                            Status_Second_IM = false;//Меняю флаг на false, т.к. одна из выполняемых операций вызвала остановку конвейера

                            if (CheckDetail_IM(Second_IM[j]))
                            {
                                Second_IM[j].Success = true;
                                Status_Second_IM = true;
                            }
                            break;
                        }
                    }
                }

                //Сдвиг ленты, если оба ИМ отработали успешно. Сдвигаю ленту и выполняю одну технологическую операцию над деталью
                if (Status_First_IM==true && Status_Second_IM==true)
                {
                    Console.WriteLine(new string('_', 50));
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Сдвигаю ленту!");
                    Console.ResetColor();
                    Console.WriteLine(new string('_', 50));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Выполняю одну технологическую операцию после сдвига ленты.");
                    Console.WriteLine(string.Format("| Тип операции = '{0}' | Отметка ИМ об обработке = '{1}' |", First_IM[0].OperationType, First_IM[0].Success));
                    Console.ResetColor();
                    Console.WriteLine(new string('#', 50));
                }
            }
        }

        //Первый участок конвейера
        static Detail[] First_Sector()
        {
            Detail[] First_operation = {
                new Detail ("Отрезка заготовки",true),
                new Detail ("Центровка",true),
                new Detail ("Расточка",true),
                new Detail ("Предварительная обточка",true)
            };
            return First_operation;
        }

        //Второй участок конвейера
        static Detail[] Second_Sector()
        {
            Detail[] Second_operation = {
                new Detail ("Чистовая обточка",true),
                new Detail ("Фрезерование паза",false), //операция над деталью, которая вызвала ошибку
                new Detail("Сверление отверстий",true),
                new Detail("Шлифование",true)
            };
            return Second_operation;
        }


        //Обработка операции, которая вызвала ошибку
        static bool CheckDetail_IM(Detail FailDetail)
        {
            if (FailDetail.Success == false)
            {
                Console.WriteLine(new string('*', 50));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Внимание!!! Warning!!! Achtung!!!");
                Console.ResetColor();
                Console.WriteLine(new string('*', 50));
                Console.WriteLine(string.Format("Регистрирую аварию на конвейере. Во время выполнения операции '{0}' произошла ошибка. Время остановки => '{1}' ", FailDetail.OperationType, DateTime.Now.ToString()));

                Console.WriteLine(new string('-', 50));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Для возврата конвейера в автоматический режим нажмите 'Enter'");
                Console.ResetColor();
                Console.WriteLine(new string('-', 50));
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("Оператор перезапустил конвейер");
                    return true;
                }
            }
            return false;
        }
    }
}
