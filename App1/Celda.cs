using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace App1
{
    public class Celda : INotifyPropertyChanged
    {
        #region Attributes
        int row;
        int column;
        string text;
        bool mina = false;
        Visibility showBomb = Visibility.Collapsed;
        Visibility showFlag = Visibility.Collapsed;
        Visibility showQuestion = Visibility.Collapsed;
        Visibility showButton = Visibility.Visible;
        ViewModelBase viewModelBase= null;
        private ICommand _leftClickCommand;
        #endregion

        #region Properties
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                RaisePropertyChanged("Text");
            }
        }

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        public bool Mina
        {
            get
            {
                return mina;
            }

            set
            {
                mina = value;
            }
        }

        public Visibility ShowBomb
        {
            get
            {
                return showBomb;
            }

            set
            {
                showBomb = value;
                RaisePropertyChanged("ShowBomb");
            }
        }

        public Visibility ShowFlag
        {
            get
            {
                return showFlag;
            }

            set
            {
                showFlag = value;
                RaisePropertyChanged("ShowFlag");
            }
        }

        public Visibility ShowQuestion
        {
            get
            {
                return showQuestion;
            }

            set
            {
                showQuestion = value;
                RaisePropertyChanged("ShowQuestion");
            }
        }

        internal ViewModelBase ViewModelBase
        {
            get
            {
                return viewModelBase;
            }

            set
            {
                viewModelBase = value;
            }
        }

        public Visibility ShowButton
        {
            get
            {
                return showButton;
            }

            set
            {
                showButton = value;
                RaisePropertyChanged("ShowButton");
            }
        }
        
                
        public ICommand LeftClickCommand
        {
            get
            {
                return _leftClickCommand ?? (_leftClickCommand = new CommandHandler_Celda((Celda c) => MyActionLeftClick(c), true));
            }
        }

        #endregion

        #region Methods
        public void MyActionLeftClick(Celda c)
        {
            viewModelBase.MyActionLeftClick(c);
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
