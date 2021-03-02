using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleFTPServer
{
    public static class RequestParser
    {
        public static bool IsCorrectRequestFormat(string str) => false;

        public static (int, string) Parse(string str) // вернем модифицированную строку и номер
        {
            // если не корректно, свалимся с исключением
            // бьем на пару - (номер, строка)
            // модифицируем строку
            return (-1, "./ololo");
        }

        private static void ModifyPath(string str)
        {
            // точку убрать, добавить много слэшей и тд
            // неплохо бы его на корректность проверить (ну и Parse млжет быть здесь вспомагательным методом)
        }
    }
}
