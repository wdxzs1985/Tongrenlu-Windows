using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tongrenlu_Windows.Model
{
    #region ** Class : DelegateCommand
    // ICommand実装用のヘルパークラス
    public class DelegateCommand : ICommand
    {
        private Action<object> _Command;        // コマンド本体
        private Func<object, bool> _CanExecute;  // 実行可否

        // コンストラクタ
        public DelegateCommand(Action<object> command, Func<object, bool> canExecute)
        {
            if (command == null)
                throw new ArgumentNullException();

            _Command = command;
            _CanExecute = canExecute;
        }

        // ICommand.Executeの実装
        void ICommand.Execute(object parameter)
        {
            _Command(parameter);
        }

        // ICommand.Executeの実装
        bool ICommand.CanExecute(object parameter)
        {
            if (_CanExecute != null)
                return _CanExecute(parameter);
            else
                return true;
        }

        // ICommand.CanExecuteChanged の実装
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        // コマンドの生成
        public static ICommand CreateCommand(Action<object> command, Func<object, bool> canExecute)
        {
            return new DelegateCommand(command, canExecute);
        }
    }
    #endregion
}
