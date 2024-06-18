using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;

namespace ATBM_Project.Views.Message
{
    /// <summary>
    /// Interaction logic for Message_View.xaml
    /// </summary>
    public partial class Message_View : UserControl
    {
        private OracleConnection _connection;
        public Message_View(OracleConnection conn)
        {
            _connection = conn;
            InitializeComponent();
            messagesDataGrid.ItemsSource = getMessages();
        }

        private List<Models.Message> getMessages()
        {
            List<Models.Message> messages = new List<Models.Message>();
            string SQLcontex = "SELECT * FROM ADMIN.THONGBAO";
            OracleCommand cmd = new OracleCommand(SQLcontex, _connection);

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int messageID = reader.GetInt32(reader.GetOrdinal("ID"));
                    string messageContent = reader.GetString(reader.GetOrdinal("NOIDUNG"));
                    messages.Add(new Models.Message
                    {
                        MessageID = messageID,
                        MessageContent = messageContent
                    });
                }
            }
            return messages;
        }
    }
}
