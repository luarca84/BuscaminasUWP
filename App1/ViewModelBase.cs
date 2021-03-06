﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace App1
{
    class ViewModelBase: INotifyPropertyChanged
    {
        #region Attributes
        int numMinas = 10;
        int numDificultad = 10;
        List<Celda> celdas;
        private bool _canExecute;
        private ICommand _clickCommand;
        private ICommand _leftClickCommand;
        #endregion

        #region Constructor
        public ViewModelBase()
        {
            _canExecute = true;
        }
        #endregion


        #region Properties
        public int NumMinas
        {
            get { return numMinas; }
            set
            {
                numMinas = value;
                RaisePropertyChanged("NumMinas");
            }
        }


        public int NumDificultad
        {
            get { return numDificultad; }
            set
            {
                numDificultad = value;
                RaisePropertyChanged("NumDificultad");
            }
        }

        public List<Celda> Celdas
        {
            get { return celdas; }
            set
            {
                celdas = value;
                RaisePropertyChanged("Celdas");
            }
        }


        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), _canExecute));
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

        public void MyAction()
        {
            NuevoJuego();
        }
              

        public async void MyActionLeftClick(Celda c)
        {
            if (c.Mina)
            {
                c.Text = "M";
                c.ShowBomb = Visibility.Visible;
                c.ShowButton = Visibility.Collapsed;

                MessageDialog msgbox = new MessageDialog("Game Over");
                var res = await msgbox.ShowAsync();
                NuevoJuego();
            }
            else
            {
                int n = GetNumMinasAlrededorCelda(c.Row, c.Column);
                c.Text = n.ToString();
                if (n == 0)
                {
                    DespejarCeldasAlrededor(c.Row, c.Column);
                }

                if (GetNumCeldasSinAbrir() == NumMinas)
                {
                    MessageDialog msgbox = new MessageDialog("You Win");
                    var res = await msgbox.ShowAsync();
                    NuevoJuego();
                }
            }
        }

        public void MyActionRightClick(Celda c)
        {
            if (c.ShowFlag == Visibility.Collapsed && c.ShowQuestion == Visibility.Collapsed)
            {
                c.ShowFlag = Visibility.Visible;
            }
            else if (c.ShowFlag == Visibility.Visible)
            {
                c.ShowFlag = Visibility.Collapsed;
                c.ShowQuestion = Visibility.Visible;
            }
            else if (c.ShowQuestion == Visibility.Visible)
            {
                c.ShowQuestion = Visibility.Collapsed;
            }
        }

        private void NuevoJuego()
        {
            List<Celda> lstCeldas = new List<Celda>();
            for (int i = 0; i < NumDificultad; i++)
                for (int j = 0; j < NumDificultad; j++)
                {
                    Celda c = new Celda();
                    c.Row = i;
                    c.Column = j;
                    c.Text = "";
                    c.Mina = false;
                    c.ViewModelBase = this;
                    lstCeldas.Add(c);
                }

            Random r = new Random();
            for (int i = 0; i < NumMinas; i++)
            {
                int x = r.Next(0, lstCeldas.Count);
                if (lstCeldas[x].Mina)
                    i--;
                lstCeldas[x].Mina = true;
            }

            Celdas = lstCeldas;
        }

        public int GetNumCeldasSinAbrir()
        {
            int n = 0;
            for (int i = 0; i < Celdas.Count; i++)
                if (Celdas[i].Text == string.Empty)
                    n++;
            return n;
        }

        private void DespejarCeldasAlrededor(int i, int j)
        {
            if (i >= 0 && i < numDificultad && j >= 0 && j < numDificultad)
            {
                DespejarCeldasAlrededor_Pos(i - 1, j - 1);
                DespejarCeldasAlrededor_Pos(i - 1, j);
                DespejarCeldasAlrededor_Pos(i - 1, j + 1);
                DespejarCeldasAlrededor_Pos(i, j - 1);
                DespejarCeldasAlrededor_Pos(i, j + 1);
                DespejarCeldasAlrededor_Pos(i + 1, j - 1);
                DespejarCeldasAlrededor_Pos(i + 1, j);
                DespejarCeldasAlrededor_Pos(i + 1, j + 1);
            }
        }

        private void DespejarCeldasAlrededor_Pos(int a, int b)
        {
            int n1 = GetNumMinasAlrededorCelda(a, b);

            if (a >= 0 && a < numDificultad && b >= 0 && b < numDificultad && (GetCelda(a, b).Text == string.Empty))
            {
                GetCelda(a, b).Text = n1.ToString();

                if (n1 == 0)
                {
                    DespejarCeldasAlrededor(a, b);
                }
            }


        }

        private int GetNumMinasAlrededorCelda(int i, int j)
        {
            bool b1 = ExisteMinaEnCelda(i - 1, j - 1);
            bool b2 = ExisteMinaEnCelda(i - 1, j);
            bool b3 = ExisteMinaEnCelda(i - 1, j + 1);
            bool b4 = ExisteMinaEnCelda(i, j - 1);
            bool b5 = ExisteMinaEnCelda(i, j + 1);
            bool b6 = ExisteMinaEnCelda(i + 1, j - 1);
            bool b7 = ExisteMinaEnCelda(i + 1, j);
            bool b8 = ExisteMinaEnCelda(i + 1, j + 1);

            int n = 0;
            if (b1) n++;
            if (b2) n++;
            if (b3) n++;
            if (b4) n++;
            if (b5) n++;
            if (b6) n++;
            if (b7) n++;
            if (b8) n++;
            return n;
        }


        private bool ExisteMinaEnCelda(int i, int j)
        {
            if (i >= 0 && i < numDificultad && j >= 0 && j < numDificultad)
                return GetCelda(i, j).Mina;
            return false;
        }

        private Celda GetCelda(int row, int column)
        {
            return celdas.Where(e => e.Row == row && e.Column == column).First();
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


    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }

    public class CommandHandler_Celda : ICommand
    {
        private Action<Celda> _action;
        private bool _canExecute;
        public CommandHandler_Celda(Action<Celda> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action((Celda)parameter);
        }
    }
}
