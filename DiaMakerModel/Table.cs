using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common; 
using System.ComponentModel;
using Common.CommonLib.Interfaces;
using System.Windows.Media;

namespace DiaMakerModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class Table
    //----------------------------------------------------------------------------------------------------------------------
    public class Table : ITableInfo, INotifyPropertyChanged, INotifyPropertyChanging, IEditableObject
    {
        private static SolidColorBrush foreColorLight;
        private static SolidColorBrush foreColorDark;
        public static List<SolidColorBrush> TableColorSet = new List<SolidColorBrush>();

        public string Scheme { get; set; }
        public string Name { get; set; }
        private bool isSelected;
        private bool isColored = true;
        private int group;
        private List<ForeignKey> foreignKeysFrom = new List<ForeignKey>();
        private List<ForeignKey> foreignKeysTo = new List<ForeignKey>();

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        //----------------------------------------------------------------------------------------------------------------------
        public List<ForeignKey> ForeignKeysFrom
        {
            get { return foreignKeysFrom; }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public List<ForeignKey> ForeignKeysTo
        {
            get { return foreignKeysFrom; }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public bool IsSelected 
        { 
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public int Group
        { 
            get
            {
                return group;
            }
            set
            {
                group = value;
                OnPropertyChanged("Group");
                OnPropertyChanged("TableColor");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public SolidColorBrush TableColor
        {
            get
            {
                if (TableColorSet != null && group < TableColorSet.Count)
                    return TableColorSet[group];
                return null;
            }
            set
            {
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public SolidColorBrush ForeColor
        {
            get
            {
                if (TableColor != null && TableColor.Color.G < 170 && TableColor.Color.B < 170)
                    return foreColorLight;
                return foreColorDark;
            }
            set
            {
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public bool IsColored
        { 
            get
            {
                return isColored;
            }
            set
            {
                isColored = value;
                OnPropertyChanged("IsColored");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public string FullName
        {
            get
            {
                return Scheme + "." + Name;
            }
            set
            {
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected virtual void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected virtual void OnPropertyChanging(string propertyName)
        {

            PropertyChangingEventHandler handler = this.PropertyChanging;
            if (handler != null)
            {
                var e = new PropertyChangingEventArgs(propertyName);
                handler(this, e);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public static void CreateTableColorSet(int pNumbreColor)
        {
            List<SolidColorBrush> preTableColorSet = new List<SolidColorBrush>();
            double dvd = Math.Ceiling(Math.Sqrt(pNumbreColor + 2));
            byte delta = (byte)(200 / (byte)dvd);
            int blue = 55 + delta;
            while (blue <= 255)
            {
                int green = 55 + delta;
                while (green <= 255)
                {
                    SolidColorBrush scb = new SolidColorBrush();
                    scb.Color = Color.FromArgb(255, 0, (byte)green, (byte)blue);
                    preTableColorSet.Add(scb);
                    green += delta;
                }
                blue += delta;
            }
            foreColorLight = new SolidColorBrush();
            foreColorLight.Color = Colors.White;
            foreColorDark = new SolidColorBrush();
            foreColorDark.Color = Colors.Black;

            TableColorSet.Clear();
            int i = 0;
            while (TableColorSet.Count < preTableColorSet.Count)
            {
                TableColorSet.Add(preTableColorSet[i++]);
                if (TableColorSet.Count < preTableColorSet.Count)
                    TableColorSet.Add(preTableColorSet[preTableColorSet.Count-i]);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------

        public void BeginEdit()
        {
            //throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            //throw new NotImplementedException();
        }

        public void EndEdit()
        {
            //throw new NotImplementedException();
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
}
