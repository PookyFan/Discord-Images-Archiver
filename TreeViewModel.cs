//Helper class enabling checkbox items in TreeView control
//(courtesy of http://thebestcsharpprogrammerintheworld.azurewebsites.net/blogs/Treeview-with-checkbox-in-WPF-using-csharp.aspx)

using System.Collections.Generic;
using System.ComponentModel;

namespace DiscordImagesArchiver
{
    public class TreeViewModel : INotifyPropertyChanged
    {
        public TreeViewModel(string name)
        {
            Name = name;
            Children = new List<TreeViewModel>();
        }

        #region Properties

        public string Name { get; private set; }
        public List<TreeViewModel> Children { get; private set; }
        public bool IsInitiallySelected { get; private set; }
        public object Tag { get; set; } = null;

        bool? _isChecked = false;
        TreeViewModel _parent;

        #region IsChecked

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if(value == _isChecked) return;

            _isChecked = value;

            if(updateChildren && _isChecked.HasValue) Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if(updateParent && _parent != null) _parent.VerifyCheckedState();

            NotifyPropertyChanged("IsChecked");
        }

        void VerifyCheckedState()
        {
            bool? state = null;

            for(int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if(i == 0)
                {
                    state = current;
                }
                else if(state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsChecked(state, false, true);
        }

        #endregion

        #endregion

        void Initialize()
        {
            foreach(TreeViewModel child in Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        public static List<string> GetTree()
        {
            List<string> selected = new List<string>();

            //select = recursive method to check each tree view item for selection (if required)

            return selected;

            //***********************************************************
            //From your window capture selected your treeview control like:   TreeViewModel root = (TreeViewModel)TreeViewControl.Items[0];
            //                                                                List<string> selected = new List<string>(TreeViewModel.GetTree());
            //***********************************************************
        }

        #region INotifyPropertyChanged Members

        void NotifyPropertyChanged(string info)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
