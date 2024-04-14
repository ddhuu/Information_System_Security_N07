using ATBM_Project.ViewsModels;
using System.Windows.Controls;

namespace ATBM_Project.Views.Role
{
    /// <summary>
    /// Interaction logic for PrivOnColumn.xaml
    /// </summary>
    /// 

    public partial class PrivOnColumn_View : UserControl
    {
        private Admin_VM _admin;
        private string _roleName;
        public PrivOnColumn_View(Admin_VM admin, string roleName)
        {
            _admin = admin;
            _roleName = roleName;
            InitializeComponent();
            //admin
            PrivOnColumnGrid.ItemsSource = _admin.GetPrivsOnColumnOfRole(_roleName);
        }

    }
}
