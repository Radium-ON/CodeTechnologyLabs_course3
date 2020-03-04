using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDialogService
    {
        string FilePath { get; set; }   // путь к выбранному файлу

        void ShowMessage(string message);   // показ сообщения

        bool OpenFileDialog();  // открытие файла
        bool SaveFileDialog();  // сохранение файла
    }
}
