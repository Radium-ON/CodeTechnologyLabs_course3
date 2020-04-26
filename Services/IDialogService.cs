using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDialogService
    {
        //путь к выбранному файлу
        string FilePath { get; set; }

        void ShowMessage(string message);

        bool OpenFileDialog();
        bool SaveFileDialog();
    }
}
